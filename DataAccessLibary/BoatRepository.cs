using MySqlConnector;
using RoeiVerenigingLibary;

namespace DataAccessLibary
{
    public class BoatRepository : IBoatRepository
    {
        public List<Boat> Get()
        {
            var boat = new List<Boat>();
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
                            List<Task> tasks = new List<Task>(reader.FieldCount);

                            while (reader.Read())
                            {
                                var id = reader.GetInt32(0);
                                var captainSeat = reader.GetInt32(1);
                                var Seats = reader.GetInt32(2);

                                var task = new Task(() =>
                                {
                                    boat.Add(new Boat(id, captainSeat, Seats));
                                });
                                task.Start();
                                tasks.Add(task);
                            }
                            Task.WaitAll(tasks.ToArray());
                        }
                    }
                }
            }

            return boat;

        }
    }
}
