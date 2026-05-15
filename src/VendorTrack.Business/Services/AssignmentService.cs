using VendorTrack.Data.Repositories;

namespace VendorTrack.Business.Services;

public sealed class AssignmentService : IAssignmentService
{
    private readonly IAssignmentRepository _repository;

    public AssignmentService(IAssignmentRepository repository)
    {
        _repository = repository;
    }

    public Task<int> AssignAsync(int tenantId, int vendorId, int requirementId, int userId)
    {
        if (vendorId <= 0) throw new ArgumentException("Vendor is required.");
        if (requirementId <= 0) throw new ArgumentException("Requirement is required.");

        return _repository.AssignAsync(tenantId, vendorId, requirementId, userId);
    }
}
