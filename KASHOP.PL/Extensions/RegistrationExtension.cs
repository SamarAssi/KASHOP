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
        services.AddTransient<IEmailSender, EmailSender>();

        return services;
    }
}
