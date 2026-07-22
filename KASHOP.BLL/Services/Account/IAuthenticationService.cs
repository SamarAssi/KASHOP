using KASHOP.DAL;

namespace KASHOP.BLL;

public interface IAuthenticationService
{
    Task<RegisterResponse> Register(RegisterRequest request);
    Task<LoginResponse> Login(LoginRequest request);
}
