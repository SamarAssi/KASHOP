using KASHOP.BLL;
using KASHOP.DAL;
using Microsoft.AspNetCore.Identity;

namespace KASHOP.PL;

public static class RegistrationExtension
{
    public static IServiceCollection RegisterServices(
        this IServiceCollection services
    )
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IAuthenticationService, AuthenticationSerivce>();
        services.AddScoped<ISeedData, RoleSeedData>();
        services.AddScoped<IFileService, FileService>();
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddProblemDetails();

        return services;
    }
}
