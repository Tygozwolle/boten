using MySqlConnector;

namespace DataAccessLibary
{
    public class ReserveDB
    {
        private List<string> GetBoats()
        {
            var list = new List<string>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();
                String sql = $"SELECT * FROM boats";
                Console.WriteLine(sql);

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetString(1));
                        }
                    }
                }
            }

            return list;
        }
    }
}
