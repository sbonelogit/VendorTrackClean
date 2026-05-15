namespace VendorTrack.Data.Repositories;

public interface IAssignmentRepository
{
    Task<int> AssignAsync(int tenantId, int vendorId, int requirementId, int userId);
}
