using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorTrack.Business.Services;
using VendorTrack.Domain.Constants;
using VendorTrack.Web.Auth;
using VendorTrack.Web.ViewModels.Vendors;

namespace VendorTrack.Web.Controllers;

[Authorize(Roles = RoleNames.AdminOrCompliance)]
public sealed class VendorsController : Controller
{
    private readonly IVendorService _vendorService;
    private readonly ICurrentUserService _currentUser;

    public VendorsController(IVendorService vendorService, ICurrentUserService currentUser)
    {
        _vendorService = vendorService;
        _currentUser = currentUser;
    }

    public async Task<IActionResult> Index(string? search, int pageNumber = 1)
    {
        var vendors = await _vendorService.GetPagedAsync(_currentUser.TenantId, search, pageNumber, 20);
        var model = new VendorIndexViewModel
        {
            Search = search,
            PageNumber = pageNumber,
            Vendors = vendors,
            TotalCount = vendors.FirstOrDefault()?.TotalCount ?? 0
        };
        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = RoleNames.TenantAdmin)]
    public IActionResult Create()
    {
        return View(new VendorFormViewModel());
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.TenantAdmin)]
    public async Task<IActionResult> Create(VendorFormViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _vendorService.CreateAsync(_currentUser.TenantId, model.CompanyName, model.RegistrationNumber, model.Email, model.Phone, _currentUser.UserId);
        TempData["Success"] = "Vendor created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var vendor = await _vendorService.GetByIdAsync(_currentUser.TenantId, id);
        if (vendor is null) return NotFound();

        return View(new VendorFormViewModel
        {
            VendorId = vendor.VendorId,
            CompanyName = vendor.CompanyName,
            RegistrationNumber = vendor.RegistrationNumber,
            Email = vendor.Email,
            Phone = vendor.Phone,
            Status = vendor.Status
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(VendorFormViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _vendorService.UpdateAsync(_currentUser.TenantId, model.VendorId, model.CompanyName, model.RegistrationNumber, model.Email, model.Phone, model.Status, _currentUser.UserId);
        TempData["Success"] = "Vendor updated successfully.";
        return RedirectToAction(nameof(Index));
    }
}
