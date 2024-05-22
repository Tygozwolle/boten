﻿using System.Data;
using MySqlConnector;
using RoeiVerenigingLibary;

namespace DataAccessLibary
{
    public class BoatRepository : IBoatRepository
    {

        public Stream getPicture(Boat boat)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();
                string query = $"SELECT * FROM boat_images " +
                               $"WHERE id= @boatid";
                Console.WriteLine(query);
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.Add("@boatId" , MySqlDbType.Int16);

                    command.Parameters["@boatId"].Value = boat.Id;
                }
            }
            return null;
        }
        public List<Boat> Getboats()
        {
            List<Boat> boat = new List<Boat>();
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
                            var id = reader.GetInt32(0);
                            var captainSeat = reader.GetBoolean(1);
                            var Seats = reader.GetInt32(2);
                            var Level = reader.GetInt32(3);

                            boat.Add(new Boat(id, captainSeat, Seats, Level));
                        }
                    }
                }
            }

            return boat;
        }
    }
}