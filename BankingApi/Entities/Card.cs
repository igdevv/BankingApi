using BankingApi.Enums;

namespace BankingApi.Entities;

public class Card
{
    public int Id { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public DateTime ExpirationDate { get; set; }
    public CardStatus Status { get; set; } = CardStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;
}
