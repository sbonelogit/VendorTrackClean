namespace VendorTrack.Domain.Entities;

public sealed class VendorListItem
{
    public int VendorId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? RegistrationNumber { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int TotalCount { get; set; }
}
