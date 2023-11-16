namespace MiniPayment.Domain.Entities;

public class TransactionDetail
{
    public Guid Id { get; set; }
    public Guid TransactionId { get; set; }

    public string TransactionType { get; set; }
    public bool Status { get; set; }
    public decimal Amount { get; set; }

    public Transaction Transaction { get; set; }


    public TransactionDetail(Guid transactionId, string transactionType, bool status, decimal amount, Transaction? transaction = null)
    {
        Id = Guid.NewGuid();
        TransactionId = transactionId;
        TransactionType = transactionType;
        Status = status;
        Amount = amount;

        Transaction = transaction!;
    }
}
