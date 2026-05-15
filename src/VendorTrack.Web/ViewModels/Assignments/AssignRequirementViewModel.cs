using System.ComponentModel.DataAnnotations;
using VendorTrack.Domain.Entities;

namespace VendorTrack.Web.ViewModels.Assignments;

public sealed class AssignRequirementViewModel
{
    [Required]
    public int VendorId { get; set; }

    [Required]
    public int RequirementId { get; set; }

    public IReadOnlyList<VendorListItem> Vendors { get; set; } = Array.Empty<VendorListItem>();
    public IReadOnlyList<ComplianceRequirement> Requirements { get; set; } = Array.Empty<ComplianceRequirement>();
}
