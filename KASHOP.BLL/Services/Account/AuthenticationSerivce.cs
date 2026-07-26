using KASHOP.DAL;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace KASHOP.BLL;

public class AuthenticationSerivce : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;

    public AuthenticationSerivce(
        UserManager<ApplicationUser> userManager,
        IEmailSender emailSender
    )
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    public async Task<RegisterResponse> Register(RegisterRequest request)
    {
        var user = request.Adapt<ApplicationUser>();
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return new RegisterResponse() 
            { 
                Message = "Error",
                Errors = result.Errors.Select(error => error.Description).ToList()
            };
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        token = Uri.EscapeDataString(token);

        var emailUrl = $"http://localhost:5270/api/Account/ConfirmEmail?token={token}&UserId={user.Id}";

        await _emailSender.SendEmailAsync(
            email: request.Email,
            subject: "Confirm Email",
            message: $@"
                <div>
                    <h2>Welcome</h2>
                    <a href='{emailUrl}'>Confirm</a>
                </div>
            "
        );

        return new RegisterResponse() { Message = "Success" };
    }

    public async Task<bool> ConfirmEmail(ConfirmEmailRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null) return false;

        request.Token = Uri.UnescapeDataString(request.Token);

        var result = await _userManager.ConfirmEmailAsync(user, request.Token);

        return result.Succeeded ? true : false;
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

        var isConfirmed = await _userManager.IsEmailConfirmedAsync(user);

        if (!isConfirmed)
        {
            return new LoginResponse()
            {
                Message = "Email is not confirmed"
            };
        }

        var result = await _userManager.CheckPasswordAsync(user, request.Password);

        return result == true ?
        new LoginResponse() { Message = "Success" } :
        new LoginResponse() { Message = "Invalid Password" };
    }
}
