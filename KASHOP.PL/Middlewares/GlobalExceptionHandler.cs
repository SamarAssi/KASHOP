using KASHOP.BLL;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Identity.Client;

namespace KASHOP.PL;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        var result = new Result<Object>
        {
            Success = false,
            Message = exception.InnerException!.Message,
        };

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

        return true;
    }

}
