using Microsoft.Extensions.Configuration;
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
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
       
      
      //  builder.DataSource = $"tcp:{config["DB:adress"]},22";
        builder.UserID = config["DB:username"];
        builder.Password = config["DB:password"];
        builder.InitialCatalog = "boten_reservering";
        
           // builder.TransparentNetworkIPResolution = 
            // builder.


            //  string conection = $"Server = {config["DB:adress"]}:22; Database = boten_reservering; User Id = {config["DB:username"]}; Password = {config["DB:password"]};";
            return $"Network Address=tcp:{config["DB:adress"]},22;" + builder.ConnectionString;
            //return "Server=boten.vanolst.tech,22;Database=YourDatabaseName;User Id=boten;Password=Boten123456789;";
        }
    }
}
