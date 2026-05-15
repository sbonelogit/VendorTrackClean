using System.Data;
using Microsoft.Data.SqlClient;
using VendorTrack.Data.Database;
using VendorTrack.Domain.Entities;

namespace VendorTrack.Data.Repositories;

public sealed class DocumentRepository : IDocumentRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public DocumentRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> SaveUploadAsync(int tenantId, int assignmentId, string originalFileName, string storedFileName, string filePath, string? contentType, long fileSizeBytes, DateTime? issueDate, DateTime? expiryDate, int uploadedById)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("dbo.usp_Document_SaveUpload", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@TenantId", tenantId);
        command.Parameters.AddWithValue("@AssignmentId", assignmentId);
        command.Parameters.AddWithValue("@OriginalFileName", originalFileName);
        command.Parameters.AddWithValue("@StoredFileName", storedFileName);
        command.Parameters.AddWithValue("@FilePath", filePath);
        command.Parameters.AddWithValue("@ContentType", (object?)contentType ?? DBNull.Value);
        command.Parameters.AddWithValue("@FileSizeBytes", fileSizeBytes);
        command.Parameters.AddWithValue("@IssueDate", (object?)issueDate?.Date ?? DBNull.Value);
        command.Parameters.AddWithValue("@ExpiryDate", (object?)expiryDate?.Date ?? DBNull.Value);
        command.Parameters.AddWithValue("@UploadedById", uploadedById);
        var output = new SqlParameter("@DocumentId", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(output);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        return (int)output.Value;
    }

    public async Task<IReadOnlyList<PendingDocumentReview>> GetPendingReviewAsync(int tenantId)
    {
        var results = new List<PendingDocumentReview>();

        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("dbo.usp_Document_GetPendingReview", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@TenantId", tenantId);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            results.Add(new PendingDocumentReview
            {
                DocumentId = reader.GetInt32Safe("DocumentId"),
                OriginalFileName = reader.GetStringSafe("OriginalFileName"),
                FilePath = reader.GetStringSafe("FilePath"),
                IssueDate = reader.GetNullableDateTime("IssueDate"),
                ExpiryDate = reader.GetNullableDateTime("ExpiryDate"),
                UploadedAt = reader.GetDateTimeSafe("UploadedAt"),
                CompanyName = reader.GetStringSafe("CompanyName"),
                RequirementName = reader.GetStringSafe("RequirementName"),
                UploadedByName = reader.GetStringSafe("UploadedByName")
            });
        }

        return results;
    }

    public async Task ReviewAsync(int tenantId, int documentId, int reviewedById, string decision, string? comment)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("dbo.usp_Document_Review", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@TenantId", tenantId);
        command.Parameters.AddWithValue("@DocumentId", documentId);
        command.Parameters.AddWithValue("@ReviewedById", reviewedById);
        command.Parameters.AddWithValue("@Decision", decision);
        command.Parameters.AddWithValue("@Comment", (object?)comment ?? DBNull.Value);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }
}
