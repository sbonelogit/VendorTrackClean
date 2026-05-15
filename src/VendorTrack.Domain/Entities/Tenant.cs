namespace VendorTrack.Domain.Entities;

public sealed class Tenant
{
    public int TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
