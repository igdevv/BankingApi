namespace BankingApi.Dtos.Accounts;

public class WithdrawRequest
{
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}
