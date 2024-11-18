using Backend.Database.Adapter.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Database.Adapter;

public static class DatabaseAdapterConfigurations
{
    public static IServiceCollection AddDatabaseAdapter(
        this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IBackendDbContext, BackendDbContext>();
        services.AddDbContext<BackendDbContext>(options => options.UseNpgsql(connectionString));

        return services;
    }
}