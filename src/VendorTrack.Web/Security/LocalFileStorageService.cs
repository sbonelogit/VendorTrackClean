using Microsoft.Extensions.Options;

namespace VendorTrack.Web.Security;

public sealed class LocalFileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _environment;
    private readonly UploadOptions _options;

    public LocalFileStorageService(IWebHostEnvironment environment, IOptions<UploadOptions> options)
    {
        _environment = environment;
        _options = options.Value;
    }

    public async Task<StoredFileResult> SaveAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        if (file is null || file.Length == 0)
            throw new InvalidOperationException("Please choose a file to upload.");

        var maxBytes = _options.MaxFileSizeMb * 1024L * 1024L;
        if (file.Length > maxBytes)
            throw new InvalidOperationException($"File is too large. Maximum size is {_options.MaxFileSizeMb}MB.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_options.AllowedExtensions.Contains(extension))
            throw new InvalidOperationException("This file type is not allowed.");

        var storedFileName = $"{Guid.NewGuid():N}{extension}";
        var rootPath = Path.Combine(_environment.ContentRootPath, _options.RootPath);
        Directory.CreateDirectory(rootPath);

        var fullPath = Path.Combine(rootPath, storedFileName);
        await using var stream = new FileStream(fullPath, FileMode.CreateNew);
        await file.CopyToAsync(stream, cancellationToken);

        return new StoredFileResult
        {
            OriginalFileName = Path.GetFileName(file.FileName),
            StoredFileName = storedFileName,
            FilePath = fullPath,
            ContentType = file.ContentType,
            FileSizeBytes = file.Length
        };
    }
}
