using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiniPayment.Appliaction.Functions;
using MiniPayment.Domain.Entities;
using MiniPayment.Domain.Helpers;
using MiniPayment.Infrastructure.Persistence.Context;
using System.Collections.ObjectModel;

namespace MiniPayment.Infrastructure.WorkerServices;

public class InitDatabaseWorker : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<InitDatabaseWorker> _logger;
    public InitDatabaseWorker(IServiceScopeFactory serviceScopeFactory, ILogger<InitDatabaseWorker> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken token)
    {
        try
        {
            // =========== Bu Worker Service Seeding İşlemi için oluşturuldu. ===========
            token.ThrowIfCancellationRequested();
            Task.Run(async () => await DoWork(token), token);
        }
        catch (Exception) { }

        return Task.CompletedTask;

    }

    

    private async Task DoWork(CancellationToken token)
    {
        List<string> GenerateCodes()
        {
            List<string> res = new();
            for(int i = 0; i < 100; i++)
                res.Add(BaseFunctions.GenerateCode(8));
            
            return res;
        }       


        try
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var db = scope.ServiceProvider.GetService<PaymentDbContext>();
            //var arr = await db.Database.GetPendingMigrationsAsync(token);
            //if (arr.Any())
            //    await db.Database.MigrateAsync(token);


            if (db!.Transactions.Any())
                return;

            //// seeding
            var fake_transactions = new Faker<Transaction>()
                .RuleFor(i => i.Id, j => j.Random.Guid())
                .RuleFor(i => i.BankId, j=> j.PickRandom(new List<short>() { BanksHelper.AKBANK, BanksHelper.GARANTI, BanksHelper.YAPIKREDI }))
                .RuleFor(i => i.OrderReference, j=> j.PickRandom(GenerateCodes()))
                .RuleFor(i => i.Status, StatusHelper.SUCCESS)
                .RuleFor(i => i.TotalAmount, 100)
                .RuleFor(i => i.NetAmount, 100)
                .RuleFor(i => i.TransactionDate, DateTime.Now);


            var transactions = fake_transactions.Generate(5);


            foreach (var transaction in transactions)
            {
                var day = new Random().Next(0,10);
                transaction.TransactionDate = transaction.TransactionDate.AddDays(-day);

                var transactionDetail = new TransactionDetail(transaction.Id, TransactionTypesHelper.Sale, StatusHelper.SUCCESS, transaction.NetAmount);
                transaction.TransactionDetails = new Collection<TransactionDetail> { transactionDetail };
            }


            
            await db.Transactions.AddRangeAsync(transactions, cancellationToken: token);
            _ = await db.SaveChangesAsync(token);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

    }
}
