using Microsoft.Extensions.Configuration;
using MySqlConnector;
using RoeiVerenigingLibary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibary
{
    public class ConnectionString
    {
       
        public static string ConectionString()
        {
        IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<ConnectionString>().Build();
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
       
      
       //builder.DataSource = $"tcp:{config["DB:adress"]}";
        builder.UserID = config["DB:username"];
        builder.Password = config["DB:password"];
      //  builder.InitialCatalog = "boten_reservering";
            builder.Port = 22;
            builder.Server = config["DB:adress"];
            builder.Database = "boten_reservering";
            builder.SslMode = MySqlSslMode.Disabled;
            builder.DnsCheckInterval = 10;
            builder.ConnectionProtocol = MySqlConnectionProtocol.Tcp;


           // builder.TransparentNetworkIPResolution = 
           // builder.


            //  string conection = $"Server = {config["DB:adress"]}:22; Database = boten_reservering; User Id = {config["DB:username"]}; Password = {config["DB:password"]};";
            return builder.ConnectionString;
            //return "Server=boten.vanolst.tech,22;Database=YourDatabaseName;User Id=boten;Password=Boten123456789;";$"Network Address=tcp:{config["DB:adress"]},22;" +
        }
    }
}//$" Uid={config["DB:username"]}@{config["DB:password"]};" +
