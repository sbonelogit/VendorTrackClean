using Microsoft.Data.SqlClient;

namespace VendorTrack.Data.Database;

public interface ISqlConnectionFactory
{
    SqlConnection CreateConnection();
}
