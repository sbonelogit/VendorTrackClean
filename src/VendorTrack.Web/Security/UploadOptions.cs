namespace VendorTrack.Web.Security;

public sealed class UploadOptions
{
    public string RootPath { get; set; } = "App_Data/SecureUploads";
    public int MaxFileSizeMb { get; set; } = 10;
    public string[] AllowedExtensions { get; set; } = Array.Empty<string>();
}
