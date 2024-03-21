using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Services;
using Order.Persistance.Contexts;
using Order.Persistance.Repositories;

namespace Order.Persistance;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ProjectLothal")));
        services.AddScoped<IOrderRepository, OrderRepository>();
        return services;
    }
}
