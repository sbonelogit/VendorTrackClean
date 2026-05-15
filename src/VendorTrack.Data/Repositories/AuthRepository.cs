using System.Data;
using Microsoft.Data.SqlClient;
using VendorTrack.Data.Database;
using VendorTrack.Domain.Entities;

namespace VendorTrack.Data.Repositories;

public sealed class AuthRepository : IAuthRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public AuthRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<AppUser?> GetUserByEmailAsync(string email)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("dbo.usp_Auth_GetUserByEmail", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@Email", email.Trim());

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new AppUser
        {
            UserId = reader.GetInt32Safe("UserId"),
            TenantId = reader.GetInt32Safe("TenantId"),
            TenantName = reader.GetStringSafe("TenantName"),
            RoleId = reader.GetInt32Safe("RoleId"),
            RoleName = reader.GetStringSafe("RoleName"),
            FullName = reader.GetStringSafe("FullName"),
            Email = reader.GetStringSafe("Email"),
            PasswordHash = reader.GetStringSafe("PasswordHash"),
            IsActive = reader.GetBooleanSafe("IsActive"),
            TenantIsActive = reader.GetBooleanSafe("TenantIsActive")
        };
    }

    public async Task MarkLoginSuccessAsync(int userId)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("dbo.usp_Auth_MarkLoginSuccess", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@UserId", userId);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }
}
