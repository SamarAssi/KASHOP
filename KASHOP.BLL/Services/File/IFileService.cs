using KASHOP.DAL;
using Microsoft.AspNetCore.Http;

namespace KASHOP.BLL;

public interface IFileService
{
    Task<Result<FileUploadResult>> UploadAsync(IFormFile file);
    Task<Result<bool>> Delete(string publicId);
}
