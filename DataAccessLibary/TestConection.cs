using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibary
{
    public abstract class TestConection
    {
        public static bool TestString(string userName, string password, string adress, string port)
        {
            try
            {
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();

                builder.UserID = userName;
                builder.Password = password;
                builder.Port = uint.Parse(port);
                builder.Server = adress;
                builder.Database = "boten_reservering";
                builder.SslMode = MySqlSslMode.VerifyFull;
                builder.DnsCheckInterval = 10;
                builder.ConnectionProtocol = MySqlConnectionProtocol.Tcp;

                var connectionString = builder.ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // throws if invalid
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
