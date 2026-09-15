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
            if (file is null || file.Length == 0)
            {
                return Result<string?>.Fail("No file was provided");
            }

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!_allowedExtensions.Contains(extension))
            {
                return Result<string?>.Fail($"File type {extension} is not allowed");
            }

            if (file.Length > MaxFileSize)
            {
                return Result<string?>.Fail("File size exceeds the 5MB limit");
            }

            var fileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "Uploads", 
                fileName
            );

            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            return Result<string?>.Ok(
                "Success",
                fileName
            );
        } catch (Exception exception)
        {
            return Result<string?>.Fail(exception.InnerException!.Message);
        }
    }
}
