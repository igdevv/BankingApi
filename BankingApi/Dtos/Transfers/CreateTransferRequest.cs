namespace BankingApi.Dtos.Transfers;

public class CreateTransferRequest
{
    public int SourceAccountId { get; set; }
    public int DestinationAccountId { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}
