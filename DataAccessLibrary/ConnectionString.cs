using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace DataAccessLibrary
{
    public abstract class ConnectionString
    {
        public static string GetString()
        {
            IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<ConnectionString>().Build();
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();

            builder.UserID = config["DB:username"];
            builder.Password = config["DB:password"];
            builder.Port = 22;
            builder.Server = config["DB:adress"];
            builder.Database = "boten_reservering";
            builder.SslMode = MySqlSslMode.VerifyFull;
            builder.DnsCheckInterval = 10;
            builder.ConnectionProtocol = MySqlConnectionProtocol.Tcp;

            return builder.ConnectionString;
        }
    }
}