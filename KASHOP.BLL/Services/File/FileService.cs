using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KASHOP.BLL;

public class FileService : IFileService
{
    const long MaxFileSize = 5 * 1024 * 1024;
    private readonly string[] _allowedExtensions = { ".jpg", ".png", ".webp", ".jpeg", ".svg" };
    public async Task<Result<string?>> UploadAsync(IFormFile file)
    {
        try
        {
            if (file is null || file.Length <= 0)
            {
                return new Result<string?>
                {
                    Success = false,
                    Message = "No file was provided"
                };
            }

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!_allowedExtensions.Contains(extension))
            {
                return new Result<string?>
                {
                    Success = false,
                    Message = $"File type {extension} is not allowed"
                };
            }

            if (file.Length > MaxFileSize)
            {
                return new Result<string?>
                {
                    Success = false,
                    Message = "File size exceeds the 5MB limit"
                };
            }

            var fileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            return new Result<string?>
            {
                Success = true,
                Message = "Success",
                Data = fileName
            };
        } catch (Exception exception)
        {
            return new Result<string?>
            {
                Success = false,
                Message = exception.InnerException!.Message
            };
        }
    }
}
