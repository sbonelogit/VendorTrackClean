using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorTrack.Business.Services;
using VendorTrack.Web.Auth;
using VendorTrack.Web.ViewModels.Dashboard;

namespace VendorTrack.Web.Controllers;

[Authorize]
public sealed class HomeController : Controller
{
    private readonly IDashboardService _dashboardService;
    private readonly ICurrentUserService _currentUser;

    public HomeController(IDashboardService dashboardService, ICurrentUserService currentUser)
    {
        _dashboardService = dashboardService;
        _currentUser = currentUser;
    }

    public async Task<IActionResult> Index()
    {
        var summary = await _dashboardService.GetSummaryAsync(_currentUser.TenantId);
        var model = new DashboardViewModel
        {
            TotalVendors = summary.TotalVendors,
            ActiveVendors = summary.ActiveVendors,
            PendingUpload = summary.PendingUpload,
            PendingReview = summary.PendingReview,
            Approved = summary.Approved,
            Rejected = summary.Rejected,
            Expired = summary.Expired,
            ExpiringSoon = summary.ExpiringSoon
        };
        return View(model);
    }

    [AllowAnonymous]
    public IActionResult Error()
    {
        return View();
    }
}
