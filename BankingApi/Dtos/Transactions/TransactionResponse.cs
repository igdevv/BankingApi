using BankingApi.Enums;

namespace BankingApi.Dtos.Transactions;

public class TransactionResponse
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
