using VendorTrack.Domain.Entities;

namespace VendorTrack.Data.Repositories;

public interface IDocumentRepository
{
    Task<int> SaveUploadAsync(int tenantId, int assignmentId, string originalFileName, string storedFileName, string filePath, string? contentType, long fileSizeBytes, DateTime? issueDate, DateTime? expiryDate, int uploadedById);
    Task<IReadOnlyList<PendingDocumentReview>> GetPendingReviewAsync(int tenantId);
    Task ReviewAsync(int tenantId, int documentId, int reviewedById, string decision, string? comment);
}
