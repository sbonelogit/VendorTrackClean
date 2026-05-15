using System.Security.Claims;

namespace VendorTrack.Web.Auth;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId => GetIntClaim(ClaimTypes.NameIdentifier);
    public int TenantId => GetIntClaim(CustomClaimTypes.TenantId);
    public string FullName => _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? string.Empty;
    public string RoleName => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

    private int GetIntClaim(string claimType)
    {
        var value = _httpContextAccessor.HttpContext?.User?.FindFirst(claimType)?.Value;
        return int.TryParse(value, out var result) ? result : 0;
    }
}
