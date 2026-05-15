using VendorTrack.Domain.Entities;

namespace VendorTrack.Business.Services;

public interface IRequirementService
{
    Task<IReadOnlyList<ComplianceRequirement>> GetAllAsync(int tenantId);
    Task<int> CreateAsync(int tenantId, string name, string? description, bool requiresExpiry, int expiryReminderDays, int userId);
}
