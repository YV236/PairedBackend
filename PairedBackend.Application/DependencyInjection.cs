using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PairedBackend.Application.Behaviors;
using PairedBackend.Contracts.Requests.User;

namespace PairedBackend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services.AddValidatorsFromAssembly(typeof(RegisterUserRequest).Assembly);

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        return services;
    }
}
