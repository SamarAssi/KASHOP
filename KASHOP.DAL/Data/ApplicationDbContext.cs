using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace KASHOP.DAL;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IHttpContextAccessor httpContextAccessor
    ) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<AuditLog>();
        var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(obj => obj.CreatedById).CurrentValue = currentUserId;
                entry.Property(obj => obj.CreatedOn).CurrentValue = DateTime.UtcNow;
            } else if (entry.State == EntityState.Modified)
            {
                entry.Property(obj => obj.UpdatedById).CurrentValue = currentUserId;
                entry.Property(obj => obj.UpdatedOn).CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
