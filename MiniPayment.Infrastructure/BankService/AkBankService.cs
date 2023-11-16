using MiniPayment.Appliaction.Functions;
using MiniPayment.Appliaction.Interfaces.BanksInterfaces;
using MiniPayment.Domain.Entities;
using MiniPayment.Domain.Helpers;
using MiniPayment.Domain.TransactionsModel;
using MiniPayment.Infrastructure.Persistence.Context;

namespace MiniPayment.Infrastructure.BankService;

public class AkBankService : BaseTransaction, IAkBankService
{

    public AkBankService(PaymentDbContext db) : base(db) { }

    public override Task<Transaction> Pay(SaleTransaction transaction)
    {

        transaction.OrderReference = BaseFunctions.GenerateCode(8);

        // AkBank API İşlemleri
        // dönen response başarılı olduğunu varsayarak, ilerliyoruz.

        transaction.Status = StatusHelper.SUCCESS;
        return base.Pay(transaction);
    }

    public override Task<Transaction> Cancel(CancelTransaction transaction)
    {
        // AkBank API İşlemleri
        // dönen response başarılı olduğunu varsayarak, ilerliyoruz.

        transaction.Status = StatusHelper.SUCCESS;

        return base.Cancel(transaction);
    }

    public override Task<Transaction> Refund(RefundTransaction transaction)
    {

        // AkBank API İşlemleri
        // dönen response başarılı olduğunu varsayarak, ilerliyoruz.

        transaction.Status = StatusHelper.SUCCESS;

        return base.Refund(transaction);
    }
}
