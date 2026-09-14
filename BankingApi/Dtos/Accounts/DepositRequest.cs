namespace BankingApi.Dtos.Accounts;

public class DepositRequest
{
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}
