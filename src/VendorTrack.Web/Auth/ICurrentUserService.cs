namespace VendorTrack.Web.Auth;

public interface ICurrentUserService
{
    int UserId { get; }
    int TenantId { get; }
    string FullName { get; }
    string RoleName { get; }
}
