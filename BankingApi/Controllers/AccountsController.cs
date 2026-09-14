using BankingApi.Data;
using BankingApi.Dtos.Accounts;
using BankingApi.Dtos.Transactions;
using BankingApi.Entities;
using BankingApi.Enums;
using BankingApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly AccountService _service;
    private readonly IValidator<WithdrawRequest> _withdrawValidator;
    private readonly IValidator<DepositRequest> _depositValidator;

    public AccountsController(
        AppDbContext context,
        AccountService service,
        IValidator<WithdrawRequest> withdrawValidator,
        IValidator<DepositRequest> depositValidator)
    {
        _context = context;
        _service = service;
        _withdrawValidator = withdrawValidator;
        _depositValidator = depositValidator;
    }

    [HttpPost]
    public async Task<ActionResult<AccountResponse>> Create(
        CreateAccountRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId);

        if (user is null)
        {
            return BadRequest("User does not exist.");
        }

        if (!user.IsActive)
        {
            return BadRequest("User is inactive.");
        }

        var account = new Account
        {
            UserId = request.UserId,
            Type = request.Type,
            AccountNumber = GenerateAccountNumber(),
            Balance = 0,
            Status = AccountStatus.Active
        };

        _context.Accounts.Add(account);

        await _context.SaveChangesAsync();

        var response = MapToResponse(account);

        return CreatedAtAction(
            nameof(GetById),
            new { id = account.Id },
            response
        );
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AccountResponse>> GetById(int id)
    {
        var account = await _context.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);

        if (account is null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(account));
    }

    [HttpPost("{id:int}/deposit")]
    public async Task<ActionResult<AccountResponse>> Deposit(
        int id,
        DepositRequest request)
    {
        var validationResult = await _depositValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var account = await _service.DepositAsync(
            id,
            request.Amount,
            request.Description
        );

        return Ok(MapToResponse(account));
    }

    [HttpPost("{id:int}/withdraw")]
    public async Task<ActionResult<AccountResponse>> Withdraw(
        int id,
        WithdrawRequest request)
    {
        var validationResult = await _withdrawValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var account = await _service.WithdrawAsync(
            id,
            request.Amount,
            request.Description
        );

        return Ok(MapToResponse(account));
    }

    [HttpGet("{id:int}/transactions")]
    public async Task<ActionResult<List<TransactionResponse>>> GetTransactions(
        int id)
    {
        var accountExists = await _context.Accounts
            .AnyAsync(a => a.Id == id);

        if (!accountExists)
        {
            return NotFound("Account not found.");
        }

        var transactions = await _context.Transactions
            .AsNoTracking()
            .Where(t => t.AccountId == id)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TransactionResponse
            {
                Id = t.Id,
                Amount = t.Amount,
                Type = t.Type,
                Description = t.Description,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync();

        return Ok(transactions);
    }

    private static AccountResponse MapToResponse(Account account)
    {
        return new AccountResponse
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber,
            Balance = account.Balance,
            Type = account.Type,
            Status = account.Status,
            CreatedAt = account.CreatedAt,
            UserId = account.UserId
        };
    }

    private static string GenerateAccountNumber()
    {
        return Random.Shared
            .NextInt64(1000000000, 9999999999)
            .ToString();
    }
}