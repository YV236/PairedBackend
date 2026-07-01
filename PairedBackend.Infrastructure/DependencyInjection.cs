using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PairedBackend.Application.Services;
using PairedBackend.Infrastructure.Identity;
using PairedBackend.Infrastructure.Options;
using PairedBackend.Infrastructure.Persistence;
using PairedBackend.Infrastructure.Services;

namespace PairedBackend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"));
        });
        services
            .AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserSessionService, UserSessionService>();
        services.AddSingleton<ITokenProvider, TokenProvider>();

        services.Configure<JwtOptions>(
            configuration.GetSection(JwtOptions.SectionName));

        return services;
    }
}
