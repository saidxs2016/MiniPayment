using MiniPayment.Domain.Entities;
using MiniPayment.Domain.TransactionsModel;

namespace MiniPayment.Appliaction.Interfaces;

public interface IBank
{
    Task<Transaction> Pay(SaleTransaction request);
    Task<Transaction> Cancel(CancelTransaction request);
    Task<Transaction> Refund(RefundTransaction request);

}
