using VendorTrack.Domain.Entities;

namespace VendorTrack.Web.ViewModels.Vendors;

public sealed class VendorIndexViewModel
{
    public string? Search { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public int TotalCount { get; set; }
    public IReadOnlyList<VendorListItem> Vendors { get; set; } = Array.Empty<VendorListItem>();
}
