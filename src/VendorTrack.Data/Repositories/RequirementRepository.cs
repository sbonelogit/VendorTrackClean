using System.Data;
using Microsoft.Data.SqlClient;
using VendorTrack.Data.Database;
using VendorTrack.Domain.Entities;

namespace VendorTrack.Data.Repositories;

public sealed class RequirementRepository : IRequirementRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public RequirementRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ComplianceRequirement>> GetAllAsync(int tenantId)
    {
        var results = new List<ComplianceRequirement>();

        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("dbo.usp_Requirement_GetAll", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@TenantId", tenantId);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            results.Add(new ComplianceRequirement
            {
                RequirementId = reader.GetInt32Safe("RequirementId"),
                TenantId = reader.GetInt32Safe("TenantId"),
                Name = reader.GetStringSafe("Name"),
                Description = reader.GetNullableString("Description"),
                RequiresExpiry = reader.GetBooleanSafe("RequiresExpiry"),
                ExpiryReminderDays = reader.GetInt32Safe("ExpiryReminderDays"),
                IsActive = reader.GetBooleanSafe("IsActive"),
                CreatedAt = reader.GetDateTimeSafe("CreatedAt")
            });
        }

        return results;
    }

    public async Task<int> CreateAsync(int tenantId, string name, string? description, bool requiresExpiry, int expiryReminderDays, int userId)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("dbo.usp_Requirement_Create", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@TenantId", tenantId);
        command.Parameters.AddWithValue("@Name", name.Trim());
        command.Parameters.AddWithValue("@Description", (object?)description ?? DBNull.Value);
        command.Parameters.AddWithValue("@RequiresExpiry", requiresExpiry);
        command.Parameters.AddWithValue("@ExpiryReminderDays", expiryReminderDays);
        command.Parameters.AddWithValue("@UserId", userId);
        var output = new SqlParameter("@RequirementId", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(output);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        return (int)output.Value;
    }
}
