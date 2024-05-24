using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace DataAccessLibary
{
    public abstract class ConnectionString
    {
        private static readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public static string GetString()
        {
            string cacheKey = "ConnectionString";
            if (_cache.TryGetValue(cacheKey, out string connectionString))
            {
                return connectionString;
            }

            IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<ConnectionString>().Build();
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
#if !RELEASE
            builder.UserID = config["DB:username"];
            builder.Password = config["DB:password"];
            builder.Port = 22;
            builder.Server = config["DB:adress"];
#elif CONFIGFILEFILLED || RELEASE
            builder.UserID = Config.DBUsername;
            builder.Password = Config.DBPassword;
            builder.Port = Config.DBPortInt;
            builder.Server = Config.DBAdress;
#endif
            builder.Database = "boten_reservering";
            builder.SslMode = MySqlSslMode.VerifyFull;
            builder.DnsCheckInterval = 10;
            builder.ConnectionProtocol = MySqlConnectionProtocol.Tcp;

            connectionString = builder.ConnectionString;

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10));

            _cache.Set(cacheKey, connectionString, cacheEntryOptions);

            return connectionString;
        }
    }
}