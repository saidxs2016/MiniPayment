namespace MiniPayment.Domain.TransactionsModel;

public class RefundTransaction
{
    public bool Status { get; set; }

    public string? OrderReference { get; set; }

    public DateTime TransactionDate { get; set; } = DateTime.Now;


}
