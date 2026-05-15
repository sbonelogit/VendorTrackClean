namespace VendorTrack.Domain.Entities;

public sealed class DashboardSummary
{
    public int TotalVendors { get; set; }
    public int ActiveVendors { get; set; }
    public int PendingUpload { get; set; }
    public int PendingReview { get; set; }
    public int Approved { get; set; }
    public int Rejected { get; set; }
    public int Expired { get; set; }
    public int ExpiringSoon { get; set; }
}
