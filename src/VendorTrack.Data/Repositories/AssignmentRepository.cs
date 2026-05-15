using System.Data;
using Microsoft.Data.SqlClient;
using VendorTrack.Data.Database;

namespace VendorTrack.Data.Repositories;

public sealed class AssignmentRepository : IAssignmentRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public AssignmentRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> AssignAsync(int tenantId, int vendorId, int requirementId, int userId)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("dbo.usp_VendorRequirement_Assign", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@TenantId", tenantId);
        command.Parameters.AddWithValue("@VendorId", vendorId);
        command.Parameters.AddWithValue("@RequirementId", requirementId);
        command.Parameters.AddWithValue("@UserId", userId);
        var output = new SqlParameter("@AssignmentId", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(output);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        return (int)output.Value;
    }
}
