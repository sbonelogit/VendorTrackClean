namespace VendorTrack.Domain.Constants;

public static class RoleNames
{
    public const string SystemOwner = "SystemOwner";
    public const string TenantAdmin = "TenantAdmin";
    public const string ComplianceOfficer = "ComplianceOfficer";
    public const string VendorUser = "VendorUser";
    public const string ReadOnlyViewer = "ReadOnlyViewer";

    public const string AdminOrCompliance = TenantAdmin + "," + ComplianceOfficer;
    public const string AdminComplianceOrViewer = TenantAdmin + "," + ComplianceOfficer + "," + ReadOnlyViewer;
}
