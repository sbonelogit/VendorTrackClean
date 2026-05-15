using Microsoft.Extensions.DependencyInjection;
using VendorTrack.Data.Database;
using VendorTrack.Data.Repositories;

namespace VendorTrack.Data.DependencyInjection;

public static class DataServiceCollectionExtensions
{
    public static IServiceCollection AddVendorTrackData(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton(new DatabaseOptions { ConnectionString = connectionString });
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IDashboardRepository, DashboardRepository>();
        services.AddScoped<IVendorRepository, VendorRepository>();
        services.AddScoped<IRequirementRepository, RequirementRepository>();
        services.AddScoped<IAssignmentRepository, AssignmentRepository>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        return services;
    }
}
