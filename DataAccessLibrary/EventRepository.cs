using MySqlConnector;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Interfaces;
using RoeiVerenigingLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public partial class EventRepository : IEventRepository
    {
        public List<Event> GetAll()
        {
            var list = new List<Event>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();

                const string sql = "SELECT * FROM `events`";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Event(new List<Member>(), reader.GetDateTime("start_time"), reader.GetDateTime("end_time"), reader.GetString("description"), reader.GetString("name"),
                                reader.GetInt32("id"), reader.GetInt32("max_participants"), new List<Boat>()));
                        }
                    }
                }
            }
            return list;
        }
        private int GetSystemId()
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();
                const string sql = "SELECT member_id FROM members WHERE first_name = @system ";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@system", MySqlDbType.VarChar);
                    command.Parameters["@system"].Value = "System";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return reader.GetInt32("member_id");
                        }
                    }
                }
            }
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();
                const string sql = " INSERT INTO boten_reservering.members (member_id, first_name, infix, last_name, level, email, password)\nVALUES (0, 'System', null, 'System', DEFAULT, 'System', 'System');";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return reader.GetInt32("member_id");
                        }
                    }
                }
            }
            throw new Exception("System member not found");
        }
        
        
    }
}
