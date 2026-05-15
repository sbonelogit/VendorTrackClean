namespace VendorTrack.Business.Services;

public interface IAuthService
{
    Task<AuthResult> AuthenticateAsync(string email, string password);
}
