using VendorTrack.Domain.Entities;

namespace VendorTrack.Data.Repositories;

public interface IDashboardRepository
{
    Task<DashboardSummary> GetSummaryAsync(int tenantId);
}
