using VendorTrack.Domain.Entities;

namespace VendorTrack.Data.Repositories;

public interface IAuthRepository
{
    Task<AppUser?> GetUserByEmailAsync(string email);
    Task MarkLoginSuccessAsync(int userId);
}
