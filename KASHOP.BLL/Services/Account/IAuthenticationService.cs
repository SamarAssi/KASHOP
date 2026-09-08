using KASHOP.DAL;

namespace KASHOP.BLL;

public interface IAuthenticationService
{
    Task<Result<bool>> Register(RegisterRequest request);
    Task<Result<bool>> ConfirmEmail(ConfirmEmailRequest request);
    Task<Result<LoginResponse>> Login(LoginRequest request);
}
