using MySqlConnector;
using RoeiVerenigingLibary;

namespace DataAccessLibrary
{
    public class BoatRepository : IBoatRepository
    {
        public List<Boat> Getboats()
        {
            var boat = new List<Boat>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();
                string sql = "SELECT * FROM boats";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            bool captainSeat = reader.GetBoolean(1);
                            int Seats = reader.GetInt32(2);
                            int Level = reader.GetInt32(3);
                            string description = reader.GetString(4);
                            string name = reader.GetString(5);

                            boat.Add(new Boat(id, captainSeat, Seats, Level, description, name));
                        }
                    }
                }
            }

            return boat;
        }

        public Boat GetBoatById(int boatId)
        {
            Boat boat = null;

            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();

                const string sql = "SELECT * FROM `boats` WHERE `id` = @boatId";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@boatId", MySqlDbType.Int32);
                    command.Parameters["@boatId"].Value = boatId;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            bool captainSeat = reader.GetBoolean(1);
                            int seats = reader.GetInt32(2);
                            int level = reader.GetInt32(3);
                            string description = reader.GetString(4);
                            string name = reader.GetString(5);

                            boat = new Boat(id, captainSeat, seats, level, description, name);
                        }
                    }
                }
            }

            return boat;
        }

        public Boat Getboat(int idBoat)
        {

            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();
                string sql = "SELECT * FROM boats WHERE id = @id";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32);
                    command.Parameters["@id"].Value = idBoat;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            bool captainSeat = reader.GetBoolean(1);
                            int Seats = reader.GetInt32(2);
                            int Level = reader.GetInt32(3);
                            string description = reader.GetString(4);
                            string name = reader.GetString(5);
                            return new Boat(id, captainSeat, Seats, Level, description, name);
                        }

                    }
                }
            }

            return null;
        }
    }
}