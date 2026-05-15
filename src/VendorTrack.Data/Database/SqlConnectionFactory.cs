using Microsoft.Data.SqlClient;

namespace VendorTrack.Data.Database;

public sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly DatabaseOptions _options;

    public SqlConnectionFactory(DatabaseOptions options)
    {
        _options = options;
    }

    public SqlConnection CreateConnection()
    {
        if (string.IsNullOrWhiteSpace(_options.ConnectionString))
        {
            throw new InvalidOperationException("Database connection string is missing. Check appsettings.json or environment variable VENDORTRACK_CONNECTION_STRING.");
        }

        return new SqlConnection(_options.ConnectionString);
    }
}
