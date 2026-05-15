namespace VendorTrack.Business.Services;

public interface IAssignmentService
{
    Task<int> AssignAsync(int tenantId, int vendorId, int requirementId, int userId);
}
