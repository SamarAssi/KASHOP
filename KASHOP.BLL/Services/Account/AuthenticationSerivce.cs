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

    public async Task<Result<bool>> Register(RegisterRequest request)
    {
        try
        {
            var user = request.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return Result<bool>.Fail(
                    "Failed to register user",
                    result.Errors.Select(error => error.Description).ToList()
                );
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

            return Result<bool>.Ok();
        }
        catch (Exception exception)
        {
            return Result<bool>.Fail(exception.InnerException!.Message);
        }
    }

    public async Task<Result<bool>> ConfirmEmail(ConfirmEmailRequest request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user is null)
            {
                return Result<bool>.Fail("User Not Found");
            }

            request.Token = Uri.UnescapeDataString(request.Token);

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);

            return result.Succeeded ?
                Result<bool>.Ok() :
                Result<bool>.Fail("Failed to Confirm Email");
        }
        catch (Exception exception)
        {
            return Result<bool>.Fail(exception.InnerException!.Message);
        }
    }

    public async Task<Result<LoginResponse>> Login(LoginRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return Result<LoginResponse>.Fail("Invalid Email");
            }

            var isConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            if (!isConfirmed)
            {
                return Result<LoginResponse>.Fail("Email is not confirmed");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                return Result<LoginResponse>.Fail("Invalid Password");
            }

            var token = await GenerateJWT(user);

            return Result<LoginResponse>.Ok(
                "Success",
                new LoginResponse { AccessToken = token }
            );
        } catch (Exception exception)
        {
            return Result<LoginResponse>.Fail(exception.InnerException!.Message);
        }
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
