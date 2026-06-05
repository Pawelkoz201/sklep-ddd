using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sklep.Application.Ports;
using Sklep.Infrastructure.Persistence;
using Sklep.Infrastructure.Persistence.Repositories;
using Sklep.Infrastructure.Security;

namespace Sklep.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<SklepDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("SklepContext")));

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<SklepDbContext>());
        services.AddScoped<IProductRepository, EfProductRepository>();
        services.AddScoped<ICategoryRepository, EfCategoryRepository>();
        services.AddScoped<IUserRepository, EfUserRepository>();
        services.AddScoped<ICartRepository, EfCartRepository>();
        services.AddScoped<IOrderRepository, EfOrderRepository>();
        services.AddScoped<IPasswordHasher, AspNetPasswordHasher>();
        services.AddSingleton<ITokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
