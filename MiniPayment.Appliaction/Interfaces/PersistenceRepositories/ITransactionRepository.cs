using MiniPayment.Appliaction.Queries;
using MiniPayment.Domain.Entities;
using System.Linq.Expressions;

namespace MiniPayment.Appliaction.Interfaces.PersistenceRepositories;

public interface ITransactionRepository
{
    Task<Transaction> GetAsync(Expression<Func<Transaction, bool>> predicate);

    Task<List<Transaction>> Filter(FilterByRequest request);
}
