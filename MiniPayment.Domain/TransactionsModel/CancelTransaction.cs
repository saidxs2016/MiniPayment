namespace MiniPayment.Domain.TransactionsModel;

public class CancelTransaction
{
    public bool Status { get; set; } = false;

    public string? OrderReference { get; set; }

    public DateTime TransactionDate { get; set; } = DateTime.Now;
}
