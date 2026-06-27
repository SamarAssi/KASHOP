using Microsoft.EntityFrameworkCore;

namespace KASHOP.DAL;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {   
    }
}
