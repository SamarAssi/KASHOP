namespace KASHOP.PL;

public static class ServicesExtension
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddDatabaseServices(configuration);
        services.AddLocalizationServices();
        services.RegisterService();
        services.AddIdentityServices();
        services.AddJwtAuthenticationServices(configuration);

        return services;
    }
}
