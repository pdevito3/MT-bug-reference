namespace TaxiDispatcher.Infrastructure
{
    using TaxiDispatcher.Infrastructure.Contexts;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Sieve.Services;

    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            // DbContext -- Do Not Delete
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<>(options =>
                    options.UseInMemoryDatabase($""));
            }
            else
            {
                services.AddDbContext<>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString(""),
                        builder => builder.MigrationsAssembly(typeof().Assembly.FullName)));
            }

            services.AddScoped<SieveProcessor>();

            // Auth -- Do Not Delete
        }
    }
}
