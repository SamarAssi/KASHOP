using Microsoft.AspNetCore.Http;

namespace KASHOP.BLL;

public interface IFileService
{
    Task<Result<string?>> UploadAsync(IFormFile file);
}
