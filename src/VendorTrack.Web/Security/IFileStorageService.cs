namespace VendorTrack.Web.Security;

public interface IFileStorageService
{
    Task<StoredFileResult> SaveAsync(IFormFile file, CancellationToken cancellationToken = default);
}

public sealed class StoredFileResult
{
    public string OriginalFileName { get; init; } = string.Empty;
    public string StoredFileName { get; init; } = string.Empty;
    public string FilePath { get; init; } = string.Empty;
    public string? ContentType { get; init; }
    public long FileSizeBytes { get; init; }
}
