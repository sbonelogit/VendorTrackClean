using System.Data;
using Microsoft.Data.SqlClient;
using VendorTrack.Data.Database;
using VendorTrack.Domain.Entities;

namespace VendorTrack.Data.Repositories;

public sealed class VendorRepository : IVendorRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public VendorRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<VendorListItem>> GetPagedAsync(int tenantId, string? search, int pageNumber, int pageSize)
    {
        var results = new List<VendorListItem>();

        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("dbo.usp_Vendor_GetPaged", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@TenantId", tenantId);
        command.Parameters.AddWithValue("@Search", string.IsNullOrWhiteSpace(search) ? DBNull.Value : search.Trim());
        command.Parameters.AddWithValue("@PageNumber", pageNumber);
        command.Parameters.AddWithValue("@PageSize", pageSize);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            results.Add(new VendorListItem
            {
                VendorId = reader.GetInt32Safe("VendorId"),
                CompanyName = reader.GetStringSafe("CompanyName"),
                RegistrationNumber = reader.GetNullableString("RegistrationNumber"),
                Email = reader.GetNullableString("Email"),
                Phone = reader.GetNullableString("Phone"),
                Status = reader.GetStringSafe("Status"),
                CreatedAt = reader.GetDateTimeSafe("CreatedAt"),
                TotalCount = reader.GetInt32Safe("TotalCount")
            });
        }

        return results;
    }

    public async Task<Vendor?> GetByIdAsync(int tenantId, int vendorId)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("dbo.usp_Vendor_GetById", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@TenantId", tenantId);
        command.Parameters.AddWithValue("@VendorId", vendorId);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync()) return null;

        return new Vendor
        {
            VendorId = reader.GetInt32Safe("VendorId"),
            TenantId = reader.GetInt32Safe("TenantId"),
            CompanyName = reader.GetStringSafe("CompanyName"),
            RegistrationNumber = reader.GetNullableString("RegistrationNumber"),
            Email = reader.GetNullableString("Email"),
            Phone = reader.GetNullableString("Phone"),
            Status = reader.GetStringSafe("Status"),
            CreatedAt = reader.GetDateTimeSafe("CreatedAt"),
            UpdatedAt = reader.GetNullableDateTime("UpdatedAt")
        };
    }

    public async Task<int> CreateAsync(int tenantId, string companyName, string? registrationNumber, string? email, string? phone, int createdBy)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("dbo.usp_Vendor_Create", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@TenantId", tenantId);
        command.Parameters.AddWithValue("@CompanyName", companyName.Trim());
        command.Parameters.AddWithValue("@RegistrationNumber", (object?)registrationNumber ?? DBNull.Value);
        command.Parameters.AddWithValue("@Email", (object?)email ?? DBNull.Value);
        command.Parameters.AddWithValue("@Phone", (object?)phone ?? DBNull.Value);
        command.Parameters.AddWithValue("@CreatedBy", createdBy);
        var output = new SqlParameter("@VendorId", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(output);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        return (int)output.Value;
    }

    public async Task UpdateAsync(int tenantId, int vendorId, string companyName, string? registrationNumber, string? email, string? phone, string status, int userId)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("dbo.usp_Vendor_Update", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@TenantId", tenantId);
        command.Parameters.AddWithValue("@VendorId", vendorId);
        command.Parameters.AddWithValue("@CompanyName", companyName.Trim());
        command.Parameters.AddWithValue("@RegistrationNumber", (object?)registrationNumber ?? DBNull.Value);
        command.Parameters.AddWithValue("@Email", (object?)email ?? DBNull.Value);
        command.Parameters.AddWithValue("@Phone", (object?)phone ?? DBNull.Value);
        command.Parameters.AddWithValue("@Status", status);
        command.Parameters.AddWithValue("@UserId", userId);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }
}
