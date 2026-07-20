using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KASHOP.PL;

public class RoleSeedData : ISeedData
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleSeedData(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task DataSeed()
    {
        if (!await _roleManager.Roles.AnyAsync())
        {
            foreach (var role in Enum.GetValues<Role>())
            {
                await _roleManager.CreateAsync(
                    new IdentityRole(role.ToString())
                );
            }
        }
    }

    enum Role
    {
        SuperAdmin,
        Admin,
        User
    }
}
