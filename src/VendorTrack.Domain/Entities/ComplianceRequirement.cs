namespace VendorTrack.Domain.Entities;

public sealed class ComplianceRequirement
{
    public int RequirementId { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool RequiresExpiry { get; set; }
    public int ExpiryReminderDays { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
