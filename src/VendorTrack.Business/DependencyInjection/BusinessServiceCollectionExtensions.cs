using Microsoft.Extensions.DependencyInjection;
using VendorTrack.Business.Security;
using VendorTrack.Business.Services;

namespace VendorTrack.Business.DependencyInjection;

public static class BusinessServiceCollectionExtensions
{
    public static IServiceCollection AddVendorTrackBusiness(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IVendorService, VendorService>();
        services.AddScoped<IRequirementService, RequirementService>();
        services.AddScoped<IAssignmentService, AssignmentService>();
        services.AddScoped<IDocumentService, DocumentService>();
        return services;
    }
}
