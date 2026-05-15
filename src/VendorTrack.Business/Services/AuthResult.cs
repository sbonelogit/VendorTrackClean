using VendorTrack.Domain.Entities;

namespace VendorTrack.Business.Services;

public sealed class AuthResult
{
    public bool Succeeded { get; init; }
    public string? ErrorMessage { get; init; }
    public AppUser? User { get; init; }

    public static AuthResult Success(AppUser user) => new() { Succeeded = true, User = user };
    public static AuthResult Failed(string message) => new() { Succeeded = false, ErrorMessage = message };
}
