using BankingApi.Data;
using BankingApi.Dtos.Transfers;
using BankingApi.Entities;
using BankingApi.Enums;
using Microsoft.EntityFrameworkCore;

namespace BankingApi.Services;

public class TransferService
{
    private readonly AppDbContext _context;

    public TransferService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TransferResponse> TransferAsync(CreateTransferRequest request)
    {
        if (request.Amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.");
        }

        if (request.SourceAccountId == request.DestinationAccountId)
        {
            throw new InvalidOperationException("Source and destination accounts cannot be the same.");
        }

        var sourceAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == request.SourceAccountId);

        if (sourceAccount is null)
        {
            throw new InvalidOperationException("Source account not found.");
        }

        var destinationAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == request.DestinationAccountId);

        if (destinationAccount is null)
        {
            throw new InvalidOperationException("Destination account not found.");
        }

        if (sourceAccount.Status != AccountStatus.Active)
        {
            throw new InvalidOperationException("Source account is not active.");
        }

        if (destinationAccount.Status != AccountStatus.Active)
        {
            throw new InvalidOperationException("Destination account is not active.");
        }

        if (sourceAccount.Balance < request.Amount)
        {
            throw new InvalidOperationException("Insufficient funds.");
        }

        await using var dbTransaction = await _context.Database.BeginTransactionAsync();

        try
        {
            sourceAccount.Balance -= request.Amount;
            destinationAccount.Balance += request.Amount;

            var transfer = new Transfer
            {
                SourceAccountId = sourceAccount.Id,
                DestinationAccountId = destinationAccount.Id,
                Amount = request.Amount
            };

            var sourceTransaction = new Transaction
            {
                AccountId = sourceAccount.Id,
                Amount = request.Amount,
                Type = TransactionType.TransferOut,
                Description = request.Description
            };

            var destinationTransaction = new Transaction
            {
                AccountId = destinationAccount.Id,
                Amount = request.Amount,
                Type = TransactionType.TransferIn,
                Description = request.Description
            };

            _context.Transfers.Add(transfer);
            _context.Transactions.Add(sourceTransaction);
            _context.Transactions.Add(destinationTransaction);

            await _context.SaveChangesAsync();

            await dbTransaction.CommitAsync();

            return new TransferResponse
            {
                Id = transfer.Id,
                SourceAccountId = transfer.SourceAccountId,
                DestinationAccountId = transfer.DestinationAccountId,
                Amount = transfer.Amount,
                CreatedAt = transfer.CreatedAt
            };
        }
        catch
        {
            await dbTransaction.RollbackAsync();
            throw;
        }
    }
}
