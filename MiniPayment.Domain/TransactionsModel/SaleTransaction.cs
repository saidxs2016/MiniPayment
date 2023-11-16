namespace MiniPayment.Domain.TransactionsModel;

public class SaleTransaction
{    
    public Guid Id { get; set; }
    public short BankId { get; set; }

    public decimal TotalAmount { get; set; }
    public decimal NetAmount { get; set; }

    public bool Status { get; set; }

    public string? OrderReference { get; set; }

    public DateTime TransactionDate { get; set; }


}
