using VendorTrack.Business.Security;
using VendorTrack.Data.Repositories;

namespace VendorTrack.Business.Services;

public sealed class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(IAuthRepository authRepository, IPasswordHasher passwordHasher)
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResult> AuthenticateAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return AuthResult.Failed("Email and password are required.");

        var user = await _authRepository.GetUserByEmailAsync(email.Trim());
        if (user is null)
            return AuthResult.Failed("Invalid email or password.");

        if (!user.IsActive || !user.TenantIsActive)
            return AuthResult.Failed("This account is inactive.");

        if (!_passwordHasher.VerifyPassword(password, user.PasswordHash))
            return AuthResult.Failed("Invalid email or password.");

        await _authRepository.MarkLoginSuccessAsync(user.UserId);
        return AuthResult.Success(user);
    }
}
