using VendorTrack.Data.Repositories;
using VendorTrack.Domain.Entities;

namespace VendorTrack.Business.Services;

public sealed class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _repository;

    public DocumentService(IDocumentRepository repository)
    {
        _repository = repository;
    }

    public Task<int> SaveUploadAsync(int tenantId, int assignmentId, string originalFileName, string storedFileName, string filePath, string? contentType, long fileSizeBytes, DateTime? issueDate, DateTime? expiryDate, int uploadedById)
    {
        if (assignmentId <= 0) throw new ArgumentException("Assignment is required.");
        if (string.IsNullOrWhiteSpace(originalFileName)) throw new ArgumentException("File name is required.");
        if (fileSizeBytes <= 0) throw new ArgumentException("File cannot be empty.");

        return _repository.SaveUploadAsync(tenantId, assignmentId, originalFileName, storedFileName, filePath, contentType, fileSizeBytes, issueDate, expiryDate, uploadedById);
    }

    public Task<IReadOnlyList<PendingDocumentReview>> GetPendingReviewAsync(int tenantId)
        => _repository.GetPendingReviewAsync(tenantId);

    public Task ReviewAsync(int tenantId, int documentId, int reviewedById, string decision, string? comment)
    {
        if (decision != "Approved" && decision != "Rejected")
            throw new ArgumentException("Decision must be Approved or Rejected.");

        return _repository.ReviewAsync(tenantId, documentId, reviewedById, decision, comment);
    }
}
