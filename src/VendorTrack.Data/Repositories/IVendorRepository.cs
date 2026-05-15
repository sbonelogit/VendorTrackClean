using VendorTrack.Domain.Entities;

namespace VendorTrack.Data.Repositories;

public interface IVendorRepository
{
    Task<IReadOnlyList<VendorListItem>> GetPagedAsync(int tenantId, string? search, int pageNumber, int pageSize);
    Task<Vendor?> GetByIdAsync(int tenantId, int vendorId);
    Task<int> CreateAsync(int tenantId, string companyName, string? registrationNumber, string? email, string? phone, int createdBy);
    Task UpdateAsync(int tenantId, int vendorId, string companyName, string? registrationNumber, string? email, string? phone, string status, int userId);
}
