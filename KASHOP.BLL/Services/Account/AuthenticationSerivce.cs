using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KASHOP.DAL;
using KASHOP.DAL.Migrations;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace KASHOP.BLL;

public class AuthenticationSerivce : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;
    private readonly IConfiguration _configuration;

    public AuthenticationSerivce(
        UserManager<ApplicationUser> userManager,
        IEmailSender emailSender,
        IConfiguration configuration
    )
    {
        _userManager = userManager;
        _emailSender = emailSender;
        _configuration = configuration;
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

        return result == false ?
        new LoginResponse() { Message = "Invalid Password" } :
        new LoginResponse() 
        { 
            Message = "Success",
            AccessToken = await GenerateJWT(user)
        };
    }

    private async Task<string> GenerateJWT(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var userClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, string.Join(',', roles))
        };

        var secretKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Apisettings:SecretKey"]!)
        );

        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Apisettings:Issuer"],
            audience: _configuration["Apisettings:Audience"],
            claims: userClaims,
            expires: DateTime.UtcNow.AddYears(20),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
