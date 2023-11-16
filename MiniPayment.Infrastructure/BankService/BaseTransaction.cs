using Microsoft.EntityFrameworkCore;
using MiniPayment.Appliaction.Interfaces;
using MiniPayment.Domain.Entities;
using MiniPayment.Domain.Helpers;
using MiniPayment.Domain.TransactionsModel;
using MiniPayment.Infrastructure.Persistence.Context;
using System.Collections.ObjectModel;

namespace MiniPayment.Infrastructure.BankService;

public class BaseTransaction : IBank
{
    private readonly PaymentDbContext _db;
    public BaseTransaction(PaymentDbContext db)
    {
        _db = db;
    }


    public virtual async Task<Transaction> Pay(SaleTransaction transaction)
    {       
        var saleTransaction = new Transaction(transaction.BankId, transaction.TotalAmount, transaction.NetAmount, transaction.Status, transaction.OrderReference!, transaction.TransactionDate);
        var saleTransactionDetail = new TransactionDetail(saleTransaction.Id, TransactionTypesHelper.Sale, transaction.Status, transaction.NetAmount);
        saleTransaction.TransactionDetails = new Collection<TransactionDetail>() { saleTransactionDetail };

        _ = await _db.Transactions.AddAsync(saleTransaction);
        _ = await _db.SaveChangesAsync();

        saleTransaction.TransactionDetails = new() { saleTransactionDetail };
        return saleTransaction;
    }

    public virtual async Task<Transaction> Cancel(CancelTransaction transaction)
    {

        var cancelTransaction = await _db.Transactions.Include(i => i.TransactionDetails).FirstOrDefaultAsync(i => i.OrderReference == transaction.OrderReference);
        var cancelTransactionDetail = cancelTransaction!.TransactionDetails[0];

        cancelTransaction.NetAmount = 0;
        cancelTransactionDetail.TransactionType = TransactionTypesHelper.Cancel;
        cancelTransactionDetail.Status = transaction.Status;

        _ = _db.Transactions.Update(cancelTransaction);
        _ = await _db.SaveChangesAsync();
        return cancelTransaction;
    }

    

    public virtual async Task<Transaction> Refund(RefundTransaction transaction)
    {
        var cancelTransaction = await _db.Transactions.Include(i => i.TransactionDetails).FirstOrDefaultAsync(i => i.OrderReference == transaction.OrderReference);
        var cancelTransactionDetail = cancelTransaction!.TransactionDetails[0];

        cancelTransaction.NetAmount = 0;
        cancelTransactionDetail.TransactionType = TransactionTypesHelper.Refund;
        cancelTransactionDetail.Status = transaction.Status;

        _ = _db.Transactions.Update(cancelTransaction);
        _ = await _db.SaveChangesAsync();
        return cancelTransaction;
    }
}
