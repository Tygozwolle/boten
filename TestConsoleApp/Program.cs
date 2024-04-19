using DataAccessLibary;
using System.Data.SqlClient;

namespace TestConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Console.WriteLine(ConnectionString.ConectionString());
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConectionString()))
            {
                connection.Open();
                // Do work here; connection closed on following line.
                String sql = "SELECT * FROM 'members'";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                        }
                    }
                }
            }

    } }
    }

