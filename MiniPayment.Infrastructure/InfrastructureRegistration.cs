using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniPayment.Appliaction.Interfaces.BanksInterfaces;
using MiniPayment.Appliaction.Interfaces.PersistenceRepositories;
using MiniPayment.Infrastructure.BankService;
using MiniPayment.Infrastructure.Persistence.Context;
using MiniPayment.Infrastructure.Persistence.Repositories;
using MiniPayment.Infrastructure.WorkerServices;

namespace MiniPayment.Infrastructure;

public static class InfrastructureRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {


        // ======== Add Hosted Services ========
        services.AddHostedService<InitDatabaseWorker>();

        // ======== Add Persistence Repositories ========
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        // ======== Add Banks Service ========
        services.AddScoped<IAkBankService, AkBankService>();
        services.AddScoped<IGarantiBankService, GarantiService>();
        services.AddScoped<IYapiKrediBankService, YapiKrediService>();

        // ======== Init DB1(PaymentDB) ========
        services.AddDbContext<PaymentDbContext>((sp, options) =>
        {
            options.UseInMemoryDatabase("mini_payment_db");
        }, ServiceLifetime.Scoped);



        return services;
    }
}