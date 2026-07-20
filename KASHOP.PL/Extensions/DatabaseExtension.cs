using KASHOP.DAL;
using Microsoft.EntityFrameworkCore;

namespace KASHOP.PL;

public static class DatabaseExtension
{
    public static WebApplicationBuilder ConnectWithDatabase(
        this WebApplicationBuilder builder
    )
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        return builder;
    }
}
