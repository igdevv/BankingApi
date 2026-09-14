using BankingApi.Enums;

namespace BankingApi.Dtos.Accounts;

public class CreateAccountRequest
{
    public int UserId { get; set; }
    public AccountType Type { get; set; }
}
