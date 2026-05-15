using VendorTrack.Domain.Entities;

namespace VendorTrack.Business.Services;

public interface IDashboardService
{
    Task<DashboardSummary> GetSummaryAsync(int tenantId);
}
