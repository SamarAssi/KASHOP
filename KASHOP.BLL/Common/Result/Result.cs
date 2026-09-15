namespace KASHOP.BLL;

public class Result<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public static Result<T> Ok(string message = "Success")
    {
        return new Result<T>
        {
            Success = true,
            Message = message
        };
    }

    public static Result<T> Ok(string message, T data)
    {
        return new Result<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static Result<T> Fail(string message)
    {
        return new Result<T>
        {
            Success = false,
            Message = message
        };
    }

    public static Result<T> Fail(string message, List<string> errors)
    {
        return new Result<T>
        {
            Success = false,
            Message = message,
            Errors = errors
        };
    }
}
