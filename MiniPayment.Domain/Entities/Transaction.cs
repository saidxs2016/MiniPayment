using System.Collections.ObjectModel;

namespace MiniPayment.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public short BankId { get; set; }

    public decimal TotalAmount { get; set; }
    public decimal NetAmount { get; set; }

    public bool Status { get; set; }

    public string OrderReference { get; set; }

    public DateTime TransactionDate { get; set; }

    public Collection<TransactionDetail> TransactionDetails { get; set; }

    public Transaction(Guid? id = null, short? bankId = null, decimal? totalAmount = null, decimal? netAmount = null, bool? status = null, string? orderReferance = null, DateTime? transactionDate = null, Collection<TransactionDetail>? transactionDetails = null)
    {
        Id = id ?? Guid.NewGuid();
        BankId = bankId ?? (short)0;
        TotalAmount = totalAmount ?? 0;
        NetAmount = netAmount ?? 0;
        Status = status ?? false;
        OrderReference = orderReferance!;
        TransactionDate = transactionDate ?? DateTime.MinValue;
        TransactionDetails = transactionDetails!;
    }
    public Transaction(short bankId, decimal totalAmount, decimal netAmount, bool status, string orderReferance, DateTime transactionDate, Collection<TransactionDetail>? transactionDetails = null)
    {
        Id = Guid.NewGuid();
        BankId = bankId;
        TotalAmount = totalAmount;
        NetAmount = netAmount;
        Status = status;
        OrderReference = orderReferance;
        TransactionDate = transactionDate;
        TransactionDetails = transactionDetails!;
    }
}
