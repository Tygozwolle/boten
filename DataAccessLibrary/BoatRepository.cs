using MySqlConnector;
using RoeiVerenigingLibrary.Interfaces;
using RoeiVerenigingLibrary.Model;

namespace DataAccessLibrary;

public class BoatRepository : IBoatRepository
{
    public List<Boat> GetBoats()
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
                        var seats = reader.GetInt32(2);
                        var level = reader.GetInt32(3);
                        var description = reader.GetString(4);
                        var boatName = reader.GetString(5);

                        boat.Add(new Boat(id, captainSeat, seats, level, description, boatName));
                    }
                }
            }
        }

        return boat;
    }

    public Boat GetBoatById(int boatId)
    {
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

                        return new Boat(id, captainSeat, seats, level, description, name, GetImage(id));
                    }
                }
            }
        }

        return null;
    }

    public Boat Create(string name, string description, int seats, bool captainSeat, int level)
    {
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();

            const string sql =
                "INSERT INTO `boats`( `captain_seat_available`, `seats`, `level`, `description`, `name`) VALUES (@captainSeat,@seats,@level,@description,@name)";

            using (var command = new MySqlCommand(sql, connection))
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

    public Boat Update(Boat boat, string name, string description, int seats, bool captainSeat, int level)
    {
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();

            const string sql =
                "UPDATE `boats` SET `captain_seat_available` = @captainSeat, `seats` = @seats, `level` = @level, `description` = @description, `name` = @name WHERE id = @id";

            using (var command = new MySqlCommand(sql, connection))
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

                command.Parameters.Add("@id", MySqlDbType.Int32);
                command.Parameters["@id"].Value = boat.Id;

                command.ExecuteReader();
                return new Boat(boat.Id, captainSeat, seats, level, description, name, GetImage(boat.Id));
            }
        }
    }

    public void Delete(Boat boat)
    {
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();

            const string sql = "DELETE FROM `boats` WHERE `id` = @id";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32);
                command.Parameters["@id"].Value = boat.Id;

                command.ExecuteNonQuery();
            }
        }
    }

    public void AddImage(Boat boat, Stream stream)
    {
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();

            const string sql = "INSERT INTO `boat_images`(`id`, `image`) VALUES (@boatId, @image)";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@boatId", MySqlDbType.Int32);
                command.Parameters["@boatId"].Value = boat.Id;

                command.Parameters.Add("@image", MySqlDbType.Blob);
                command.Parameters["@image"].Value = stream;

                command.ExecuteNonQuery();
            }
        }
    }

    public Stream GetImage(Boat boat)
    {
        return GetImage(boat.Id);
    }

    public void UpdateImage(Boat boat, Stream stream)
    {
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();

            const string sql = "UPDATE `boat_images` SET `image` = @image WHERE `id` = @boatId";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@boatId", MySqlDbType.Int32);
                command.Parameters["@boatId"].Value = boat.Id;

                command.Parameters.Add("@image", MySqlDbType.Blob);
                command.Parameters["@image"].Value = stream;

                command.ExecuteNonQuery();
            }
        }
    }

    public Stream GetImage(int id)
    {
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();

            const string sql = "SELECT `image` FROM `boat_images` WHERE `id` = @boatId";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@boatId", MySqlDbType.Int32);
                command.Parameters["@boatId"].Value = id;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read()) return reader.GetStream(0);
                }
            }
        }

        return null;
    }
}