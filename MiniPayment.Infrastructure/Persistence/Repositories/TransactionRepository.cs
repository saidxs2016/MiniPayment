using Microsoft.EntityFrameworkCore;
using MiniPayment.Appliaction.Interfaces.PersistenceRepositories;
using MiniPayment.Appliaction.Queries;
using MiniPayment.Domain.Entities;
using MiniPayment.Infrastructure.Persistence.Context;
using MiniPayment.Infrastructure.Persistence.Extensions;
using System.Linq.Expressions;

namespace MiniPayment.Infrastructure.Persistence.Repositories;

public class TransactionRepository : ITransactionRepository
{

    private readonly PaymentDbContext _db;

    public TransactionRepository(PaymentDbContext db)
    {
        _db = db;
    }

    public async Task<List<Transaction>> Filter(FilterByRequest request)
    {
        Expression<Func<Transaction, bool>> predicate = _ => true;

        if (!string.IsNullOrEmpty(request.OrderReference) && request.OrderReference.Length == 8)
            predicate = predicate.And<Transaction>(i => i.OrderReference == request.OrderReference);

        if (request.Status.HasValue)
            predicate = predicate.And<Transaction>(i => i.Status == request.Status.Value);

        if (request.BankId.HasValue && request.BankId.Value > 0)
            predicate = predicate.And<Transaction>(i => i.BankId == request.BankId.Value);

        if (request.StartDate.HasValue && request.StartDate.Value > DateTime.MinValue)
            predicate = predicate.And<Transaction>(i => i.TransactionDate >= request.StartDate.Value);

        if (request.EndDate.HasValue && request.EndDate.Value > DateTime.MinValue)
            predicate = predicate.And<Transaction>(i => i.TransactionDate <= request.EndDate.Value);


        var result = await _db.Transactions.Include(i => i.TransactionDetails).Where(predicate).ToListAsync();
        return result;
    }

    public async Task<Transaction> GetAsync(Expression<Func<Transaction, bool>> predicate) => 
        await _db.Transactions.Include(i => i.TransactionDetails).FirstOrDefaultAsync(predicate) ?? new Transaction();
}
