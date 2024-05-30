using MySqlConnector;

namespace DataAccessLibrary;

public abstract class TestConnection
{
    public static bool TestString(string userName, string password, string adress, string port,
        out string errorMassage)
    {
        errorMassage = "";
        try
        {
            var builder = new MySqlConnectionStringBuilder();

            builder.UserID = userName;
            builder.Password = password;
            builder.Port = uint.Parse(port);
            builder.Server = adress;
            builder.Database = "boten_reservering";
            builder.SslMode = MySqlSslMode.VerifyFull;
            builder.DnsCheckInterval = 10;
            builder.ConnectionProtocol = MySqlConnectionProtocol.Tcp;

            var connectionString = builder.ConnectionString;
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open(); // throws if invalid
            }

            return true;
        }
        catch (Exception ex)
        {
            errorMassage = ex.Message;
            return false;
        }
    }
}