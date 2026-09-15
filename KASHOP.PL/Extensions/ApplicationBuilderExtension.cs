using Microsoft.Extensions.Options;
using Microsoft.Identity.Client.AppConfig;

namespace KASHOP.PL;

public static class ApplicationBuilderExtension
{
    public static async Task<WebApplication> UseApplicationPipeline(
        this WebApplication app
    )
    {
        app.UseExceptionHandler();
        app.UseStaticFiles();
        app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
        MapsterExtension.MapsterServices();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        await app.SeedDatabaseAsync();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
