namespace VendorTrack.Domain.Entities;

public sealed class PendingDocumentReview
{
    public int DocumentId { get; set; }
    public string OriginalFileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DateTime UploadedAt { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string RequirementName { get; set; } = string.Empty;
    public string UploadedByName { get; set; } = string.Empty;
}
