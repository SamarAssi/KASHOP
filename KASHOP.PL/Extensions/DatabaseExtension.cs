using KASHOP.DAL;
using Microsoft.EntityFrameworkCore;

namespace KASHOP.PL;

public static class DatabaseExtension
{
    public static IServiceCollection AddDatabaseServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }
}
