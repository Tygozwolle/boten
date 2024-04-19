using DataAccessLibary;
using MySqlConnector;
using System.Data.SqlClient;

namespace TestConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Console.WriteLine(ConnectionString.ConectionString());
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.ConectionString()))
            {
                connection.Open();

                String sql = "SELECT * FROM members";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("{0} {1}", reader.GetInt32(0), reader.GetString(1));
                        }
                    }
                }
            }

    } }
    }

