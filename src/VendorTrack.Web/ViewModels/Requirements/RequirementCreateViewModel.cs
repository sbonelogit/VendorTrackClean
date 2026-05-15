using System.ComponentModel.DataAnnotations;

namespace VendorTrack.Web.ViewModels.Requirements;

public sealed class RequirementCreateViewModel
{
    [Required, StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public bool RequiresExpiry { get; set; } = true;

    [Range(1, 365)]
    public int ExpiryReminderDays { get; set; } = 30;
}
