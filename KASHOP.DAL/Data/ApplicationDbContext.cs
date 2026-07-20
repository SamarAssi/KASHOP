using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KASHOP.DAL;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {   
    }
}
