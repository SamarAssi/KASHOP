using KASHOP.DAL;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace KASHOP.BLL;

public class AuthenticationSerivce : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthenticationSerivce(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<RegisterResponse> Register(RegisterRequest request)
    {
        var user = request.Adapt<ApplicationUser>();
        var result = await _userManager.CreateAsync(user, request.Password);

        return result.Succeeded ?
        new RegisterResponse() { Message = "Success" } :
        new RegisterResponse() { Message = "Error" };
    }

    public async Task<LoginResponse> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return new LoginResponse()
            {
                Message = "Invalid Email"
            };
        }

        var result = await _userManager.CheckPasswordAsync(user, request.Password);

        return result == true ?
        new LoginResponse() { Message = "Success" } :
        new LoginResponse() { Message = "Invalid Password" };
    }
}
