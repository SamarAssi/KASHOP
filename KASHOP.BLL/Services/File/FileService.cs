using System.Security.Cryptography.X509Certificates;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using KASHOP.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KASHOP.BLL;

public class FileService : IFileService
{
    const long MaxFileSize = 5 * 1024 * 1024;
    private readonly string[] _allowedExtensions = { ".jpg", ".png", ".webp", ".jpeg", ".svg" };
    private readonly Cloudinary _cloudinary;

    public FileService(IConfiguration configuration)
    {
        var account = new Account(
            configuration["CloudinarySettings:CloudName"],
            configuration["CloudinarySettings:APIKey"],
            configuration["CloudinarySettings:APISecret"]
        );

        _cloudinary = new Cloudinary(account);
    }
    public async Task<Result<FileUploadResult>> UploadAsync(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            return Result<FileUploadResult>.Fail("No file was provided");
        }

        var extension = Path.GetExtension(file.FileName).ToLower();

        if (!_allowedExtensions.Contains(extension))
        {
            return Result<FileUploadResult>.Fail($"File type {extension} is not allowed");
        }

        if (file.Length > MaxFileSize)
        {
            return Result<FileUploadResult>.Fail("File size exceeds the 5MB limit");
        }

        using (var stream = file.OpenReadStream())
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "KASHOP"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                return Result<FileUploadResult>.Fail(uploadResult.Error.Message);
            }

            return Result<FileUploadResult>.Ok(
                "Success",
                new FileUploadResult
                {
                    Url = uploadResult.SecureUri.ToString(),
                    PublicId = uploadResult.PublicId
                }
            );
        }
    }

    public async Task<Result<bool>> Delete(string publicId)
    {
        var deletionParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deletionParams);

        return result.Result != "ok" ?
            Result<bool>.Fail("Failed to delete image from cloudinary") :
            Result<bool>.Ok();
    }
}
