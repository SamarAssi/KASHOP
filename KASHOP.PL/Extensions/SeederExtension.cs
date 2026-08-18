namespace KASHOP.PL;

public static class SeederExtension
{
    public static async Task SeedDatabaseAsync(
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
