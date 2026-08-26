using Microsoft.AspNetCore.Http;

namespace KASHOP.BLL;

public interface IFileService
{
    Task<string?> UploadAsync(IFormFile file);
}
