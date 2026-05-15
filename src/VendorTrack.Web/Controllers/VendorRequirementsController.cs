using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorTrack.Business.Services;
using VendorTrack.Domain.Constants;
using VendorTrack.Web.Auth;
using VendorTrack.Web.ViewModels.Assignments;

namespace VendorTrack.Web.Controllers;

[Authorize(Roles = RoleNames.TenantAdmin)]
public sealed class VendorRequirementsController : Controller
{
    private readonly IVendorService _vendorService;
    private readonly IRequirementService _requirementService;
    private readonly IAssignmentService _assignmentService;
    private readonly ICurrentUserService _currentUser;

    public VendorRequirementsController(IVendorService vendorService, IRequirementService requirementService, IAssignmentService assignmentService, ICurrentUserService currentUser)
    {
        _vendorService = vendorService;
        _requirementService = requirementService;
        _assignmentService = assignmentService;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<IActionResult> Assign()
    {
        return View(await BuildAssignModelAsync(new AssignRequirementViewModel()));
    }

    [HttpPost]
    public async Task<IActionResult> Assign(AssignRequirementViewModel model)
    {
        if (!ModelState.IsValid)
            return View(await BuildAssignModelAsync(model));

        await _assignmentService.AssignAsync(_currentUser.TenantId, model.VendorId, model.RequirementId, _currentUser.UserId);
        TempData["Success"] = "Requirement assigned successfully.";
        return RedirectToAction(nameof(Assign));
    }

    private async Task<AssignRequirementViewModel> BuildAssignModelAsync(AssignRequirementViewModel model)
    {
        model.Vendors = await _vendorService.GetPagedAsync(_currentUser.TenantId, null, 1, 200);
        model.Requirements = await _requirementService.GetAllAsync(_currentUser.TenantId);
        return model;
    }
}
