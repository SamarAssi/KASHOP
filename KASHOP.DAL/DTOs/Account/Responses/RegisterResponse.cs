namespace KASHOP.DAL;

public class RegisterResponse
{
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; }
}
