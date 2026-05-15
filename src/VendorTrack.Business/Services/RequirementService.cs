using VendorTrack.Data.Repositories;
using VendorTrack.Domain.Entities;

namespace VendorTrack.Business.Services;

public sealed class RequirementService : IRequirementService
{
    private readonly IRequirementRepository _repository;

    public RequirementService(IRequirementRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<ComplianceRequirement>> GetAllAsync(int tenantId)
        => _repository.GetAllAsync(tenantId);

    public Task<int> CreateAsync(int tenantId, string name, string? description, bool requiresExpiry, int expiryReminderDays, int userId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Requirement name is required.");

        if (expiryReminderDays < 1)
            expiryReminderDays = 30;

        return _repository.CreateAsync(tenantId, name, description, requiresExpiry, expiryReminderDays, userId);
    }
}
