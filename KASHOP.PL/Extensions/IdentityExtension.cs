using KASHOP.DAL;
using Microsoft.AspNetCore.Identity;

namespace KASHOP.PL;

public static class IdentityExtension
{
    public static IServiceCollection AddIdentityServices(
        this IServiceCollection services
    )
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }
}
