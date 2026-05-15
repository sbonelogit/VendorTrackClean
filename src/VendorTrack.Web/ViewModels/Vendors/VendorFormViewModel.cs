using System.ComponentModel.DataAnnotations;

namespace VendorTrack.Web.ViewModels.Vendors;

public sealed class VendorFormViewModel
{
    public int VendorId { get; set; }

    [Required, StringLength(150)]
    public string CompanyName { get; set; } = string.Empty;

    [StringLength(100)]
    public string? RegistrationNumber { get; set; }

    [EmailAddress, StringLength(150)]
    public string? Email { get; set; }

    [StringLength(30)]
    public string? Phone { get; set; }

    [Required]
    public string Status { get; set; } = "Active";
}
