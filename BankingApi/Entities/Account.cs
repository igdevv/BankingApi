using BankingApi.Enums;
using System.Security.Cryptography.Xml;
using System.Transactions;

namespace BankingApi.Entities;

public class Account
{
    public int Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public AccountType Type { get; set; }
    public AccountStatus Status { get; set; } = AccountStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;



    //Foreign Key
    public int UserId { get; set; }

    //Navigation Property
    public User User { get; set; } = null!;

    public ICollection<Transaction> Transactions { get; set; } = [];
    public ICollection<Card> Cards { get; set; } = [];
    public ICollection<Transfer> TransfersSent { get; set; } = [];
    public ICollection<Transfer> TransfersReceived { get; set; } = [];


}
