using System.ComponentModel.DataAnnotations;

namespace VendorTrack.Web.ViewModels.Documents;

public sealed class DocumentUploadViewModel
{
    [Required]
    public int AssignmentId { get; set; }

    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }

    [Required]
    public IFormFile? File { get; set; }
}
