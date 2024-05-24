using MySqlConnector;
using RoeiVerenigingLibary;
using System.Reflection.Emit;
using System;

namespace DataAccessLibary
{
    public class BoatRepository : IBoatRepository
    {
        public List<Boat> Getboats()
        {
            List<Boat> boat = new List<Boat>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();
                String sql = $"SELECT * FROM boats";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = reader.GetInt32(0);
                            var captainSeat = reader.GetBoolean(1);
                            var Seats = reader.GetInt32(2);
                            var Level = reader.GetInt32(3);
                            var description = reader.GetString(4);
                            var name = reader.GetString(5);

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
                            var id = reader.GetInt32(0);
                            var captainSeat = reader.GetBoolean(1);
                            var seats = reader.GetInt32(2);
                            var level = reader.GetInt32(3);
                            var description = reader.GetString(4);
                            var name = reader.GetString(5);

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
                String sql = $"SELECT * FROM boats WHERE id = @id";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32);
                    command.Parameters["@id"].Value = idBoat;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = reader.GetInt32(0);
                            var captainSeat = reader.GetBoolean(1);
                            var Seats = reader.GetInt32(2);
                            var Level = reader.GetInt32(3);
                            var description = reader.GetString(4);
                            var name = reader.GetString(5);
                            return new Boat(id, captainSeat, Seats, Level, description, name);
                        }

                    }
                }
            }

            return null;
        }
        public Boat Create(string name, string description, int seats, bool captainSeat, int level)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();

                const string sql =
                    $"INSERT INTO `boats`( `captain_seat`, `seats`, `level`, `description`, `name`) VALUES (@captainSeat,@seats,@level,@description,@name)";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@captainSeat", MySqlDbType.Bool);
                    command.Parameters["@captainSeat"].Value = captainSeat;
                    
                    command.Parameters.Add("@seats", MySqlDbType.Int32);
                    command.Parameters["@seats"].Value = seats;
                    
                    command.Parameters.Add("@level", MySqlDbType.Int32);
                    command.Parameters["@level"].Value = level;
                    
                    command.Parameters.Add("@description", MySqlDbType.VarChar);
                    command.Parameters["@description"].Value = description;
                    
                    command.Parameters.Add("@name", MySqlDbType.VarChar);
                    command.Parameters["@name"].Value = name;
                    
                    command.ExecuteReader();
                    return new Boat((int)command.LastInsertedId, captainSeat, seats, level, description, name);
                }
            }
        }
    }
}