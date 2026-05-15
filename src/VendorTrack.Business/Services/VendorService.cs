using VendorTrack.Data.Repositories;
using VendorTrack.Domain.Entities;

namespace VendorTrack.Business.Services;

public sealed class VendorService : IVendorService
{
    private readonly IVendorRepository _repository;

    public VendorService(IVendorRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<VendorListItem>> GetPagedAsync(int tenantId, string? search, int pageNumber, int pageSize)
        => _repository.GetPagedAsync(tenantId, search, pageNumber, pageSize);

    public Task<Vendor?> GetByIdAsync(int tenantId, int vendorId)
        => _repository.GetByIdAsync(tenantId, vendorId);

    public Task<int> CreateAsync(int tenantId, string companyName, string? registrationNumber, string? email, string? phone, int createdBy)
    {
        if (string.IsNullOrWhiteSpace(companyName))
            throw new ArgumentException("Company name is required.");

        return _repository.CreateAsync(tenantId, companyName, registrationNumber, email, phone, createdBy);
    }

    public Task UpdateAsync(int tenantId, int vendorId, string companyName, string? registrationNumber, string? email, string? phone, string status, int userId)
    {
        if (string.IsNullOrWhiteSpace(companyName))
            throw new ArgumentException("Company name is required.");

        return _repository.UpdateAsync(tenantId, vendorId, companyName, registrationNumber, email, phone, status, userId);
    }
}
