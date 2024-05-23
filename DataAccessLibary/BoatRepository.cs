using MySqlConnector;
using RoeiVerenigingLibary;

namespace DataAccessLibary;

public class BoatRepository : IBoatRepository
{
    public List<Boat> Getboats()
    {
        var boat = new List<Boat>();
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            var sql = "SELECT * FROM boats";

            using (var command = new MySqlCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
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

        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();

            const string sql = "SELECT * FROM `boats` WHERE `id` = @boatId";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@boatId", MySqlDbType.Int32);
                command.Parameters["@boatId"].Value = boatId;

                using (var reader = command.ExecuteReader())
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
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            var sql = "SELECT * FROM boats WHERE id = @id";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32);
                command.Parameters["@id"].Value = idBoat;

                using (var reader = command.ExecuteReader())
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
}