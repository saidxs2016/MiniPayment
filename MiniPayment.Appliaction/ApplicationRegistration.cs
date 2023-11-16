using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniPayment.Appliaction.MediatrBehaviors;
using System.Reflection;

namespace MiniPayment.Appliaction;

public static class ApplicationRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {

        // ======== Add Mapper Service ========
        services.AddAutoMapper(typeof(ApplicationRegistration));

        // ======== Add Mediatr Service ========
        services.AddMediatR(conf =>
        {
            conf.RegisterServicesFromAssembly(typeof(ApplicationRegistration).Assembly);
            conf.AddOpenBehavior(typeof(MedPipelineFilter<,>));
        });
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


        return services;
    }
}
