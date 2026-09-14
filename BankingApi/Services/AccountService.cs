using BankingApi.Data;
using BankingApi.Entities;
using BankingApi.Enums;
using Microsoft.EntityFrameworkCore;

namespace BankingApi.Services;

public class AccountService
{
    private readonly AppDbContext _context;

    public AccountService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Account?> DepositAsync(int accountId, decimal amount, string? description)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);

        if (account is null)
        {
            return null;
        }

        if (account.Status != AccountStatus.Active)
        {
            throw new InvalidOperationException("Account is not active.");
        }

        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.");
        }

        account.Balance += amount;

        var transaction = new Transaction
        {
            AccountId = account.Id,
            Amount = amount,
            Type = TransactionType.Deposit,
            Description = description
        };

        _context.Transactions.Add(transaction);

        await _context.SaveChangesAsync();

        return account;
    }
    public async Task<Account?> WithdrawAsync(int accountId, decimal amount, string? description)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);

        if (account is null)
        {
            return null;
        }

        if (account.Status != AccountStatus.Active)
        {
            throw new InvalidOperationException("Account is not active.");
        }

        if (amount <= 0)
        {
            throw new ArgumentException(
                "Amount must be greater than zero."
            );
        }

        if (account.Balance < amount)
        {
            throw new InvalidOperationException(
                "Insufficient funds."
            );
        }

        account.Balance -= amount;

        var transaction = new Transaction
        {
            AccountId = account.Id,
            Amount = amount,
            Type = TransactionType.Withdrawal,
            Description = description
        };

        _context.Transactions.Add(transaction);

        await _context.SaveChangesAsync();

        return account;
    }
}
