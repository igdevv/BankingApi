namespace BankingApi.Entities;

public class Transfer
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int SourceAccountId { get; set; }
    public Account SourceAccount { get; set; } = null!;
    public int DestinationAccountId { get; set; }
    public Account DestinationAccount { get; set; } = null!;
}
