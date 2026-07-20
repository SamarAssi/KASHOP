using Microsoft.AspNetCore.Identity;

namespace KASHOP.DAL;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? Street { get; set; }
}
