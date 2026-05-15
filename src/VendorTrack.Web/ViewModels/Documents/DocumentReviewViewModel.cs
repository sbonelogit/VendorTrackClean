using System.ComponentModel.DataAnnotations;

namespace VendorTrack.Web.ViewModels.Documents;

public sealed class DocumentReviewViewModel
{
    [Required]
    public int DocumentId { get; set; }

    [Required]
    public string Decision { get; set; } = "Approved";

    [StringLength(500)]
    public string? Comment { get; set; }
}
