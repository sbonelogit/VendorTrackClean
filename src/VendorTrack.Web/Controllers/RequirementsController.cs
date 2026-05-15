using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorTrack.Business.Services;
using VendorTrack.Domain.Constants;
using VendorTrack.Web.Auth;
using VendorTrack.Web.ViewModels.Requirements;

namespace VendorTrack.Web.Controllers;

[Authorize(Roles = RoleNames.TenantAdmin)]
public sealed class RequirementsController : Controller
{
    private readonly IRequirementService _requirementService;
    private readonly ICurrentUserService _currentUser;

    public RequirementsController(IRequirementService requirementService, ICurrentUserService currentUser)
    {
        _requirementService = requirementService;
        _currentUser = currentUser;
    }

    public async Task<IActionResult> Index()
    {
        var requirements = await _requirementService.GetAllAsync(_currentUser.TenantId);
        return View(requirements);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new RequirementCreateViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(RequirementCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _requirementService.CreateAsync(_currentUser.TenantId, model.Name, model.Description, model.RequiresExpiry, model.ExpiryReminderDays, _currentUser.UserId);
        TempData["Success"] = "Requirement created successfully.";
        return RedirectToAction(nameof(Index));
    }
}
