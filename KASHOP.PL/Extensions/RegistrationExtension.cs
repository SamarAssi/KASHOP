using KASHOP.BLL;
using KASHOP.DAL;
using Microsoft.AspNetCore.Identity;

namespace KASHOP.PL;

public static class RegistrationExtension
{
    public static IServiceCollection RegisterService(
        this IServiceCollection services
    )
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IAuthenticationService, AuthenticationSerivce>();
        services.AddScoped<ISeedData, RoleSeedData>();

        return services;
    }

    public static IServiceCollection AddIdentityServices(
        this IServiceCollection services
    )
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public async static Task CreateObject(
        this WebApplication app
    )
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var seeders = services.GetServices<ISeedData>();

            foreach (var seeder in seeders)
            {
                await seeder.DataSeed();
            }
        }
    }
}
