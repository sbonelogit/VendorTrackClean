using VendorTrack.Data.Repositories;
using VendorTrack.Domain.Entities;

namespace VendorTrack.Business.Services;

public sealed class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _repository;

    public DashboardService(IDashboardRepository repository)
    {
        _repository = repository;
    }

    public Task<DashboardSummary> GetSummaryAsync(int tenantId) => _repository.GetSummaryAsync(tenantId);
}
