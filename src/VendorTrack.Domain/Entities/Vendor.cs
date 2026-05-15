namespace VendorTrack.Domain.Entities;

public sealed class Vendor
{
    public int VendorId { get; set; }
    public int TenantId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? RegistrationNumber { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string Status { get; set; } = "Active";
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
