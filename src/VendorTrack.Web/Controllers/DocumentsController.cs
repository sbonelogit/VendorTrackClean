using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorTrack.Business.Services;
using VendorTrack.Domain.Constants;
using VendorTrack.Web.Auth;
using VendorTrack.Web.Security;
using VendorTrack.Web.ViewModels.Documents;

namespace VendorTrack.Web.Controllers;

[Authorize]
public sealed class DocumentsController : Controller
{
    private readonly IDocumentService _documentService;
    private readonly IFileStorageService _fileStorageService;
    private readonly ICurrentUserService _currentUser;

    public DocumentsController(IDocumentService documentService, IFileStorageService fileStorageService, ICurrentUserService currentUser)
    {
        _documentService = documentService;
        _fileStorageService = fileStorageService;
        _currentUser = currentUser;
    }

    [HttpGet]
    public IActionResult Upload(int assignmentId)
    {
        return View(new DocumentUploadViewModel { AssignmentId = assignmentId });
    }

    [HttpPost]
    public async Task<IActionResult> Upload(DocumentUploadViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var storedFile = await _fileStorageService.SaveAsync(model.File!);
            await _documentService.SaveUploadAsync(
                _currentUser.TenantId,
                model.AssignmentId,
                storedFile.OriginalFileName,
                storedFile.StoredFileName,
                storedFile.FilePath,
                storedFile.ContentType,
                storedFile.FileSizeBytes,
                model.IssueDate,
                model.ExpiryDate,
                _currentUser.UserId);

            TempData["Success"] = "Document uploaded and sent for review.";
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [Authorize(Roles = RoleNames.AdminOrCompliance)]
    public async Task<IActionResult> PendingReview()
    {
        var documents = await _documentService.GetPendingReviewAsync(_currentUser.TenantId);
        return View(documents);
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.AdminOrCompliance)]
    public async Task<IActionResult> Review(DocumentReviewViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Invalid review details.";
            return RedirectToAction(nameof(PendingReview));
        }

        await _documentService.ReviewAsync(_currentUser.TenantId, model.DocumentId, _currentUser.UserId, model.Decision, model.Comment);
        TempData["Success"] = $"Document {model.Decision.ToLower()} successfully.";
        return RedirectToAction(nameof(PendingReview));
    }
}
