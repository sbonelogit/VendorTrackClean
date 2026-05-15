using System.Data;
using Microsoft.Data.SqlClient;
using VendorTrack.Data.Database;
using VendorTrack.Domain.Entities;

namespace VendorTrack.Data.Repositories;

public sealed class DashboardRepository : IDashboardRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public DashboardRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<DashboardSummary> GetSummaryAsync(int tenantId)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("dbo.usp_Dashboard_GetSummary", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@TenantId", tenantId);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync()) return new DashboardSummary();

        return new DashboardSummary
        {
            TotalVendors = reader.GetInt32Safe("TotalVendors"),
            ActiveVendors = reader.GetInt32Safe("ActiveVendors"),
            PendingUpload = reader.GetInt32Safe("PendingUpload"),
            PendingReview = reader.GetInt32Safe("PendingReview"),
            Approved = reader.GetInt32Safe("Approved"),
            Rejected = reader.GetInt32Safe("Rejected"),
            Expired = reader.GetInt32Safe("Expired"),
            ExpiringSoon = reader.GetInt32Safe("ExpiringSoon")
        };
    }
}
