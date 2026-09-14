using BankingApi.Enums;

namespace BankingApi.Dtos.Accounts;

public class AccountResponse
{
    public int Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public AccountType Type { get; set; }
    public AccountStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
}
