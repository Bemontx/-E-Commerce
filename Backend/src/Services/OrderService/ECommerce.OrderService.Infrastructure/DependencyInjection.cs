using ECommerce.OrderService.Application.Interfaces;
using ECommerce.OrderService.Infrastructure.Persistence;
using ECommerce.OrderService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.OrderService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("OrderDatabase");

        services.AddDbContext<OrderDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Registro de repositorios
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}