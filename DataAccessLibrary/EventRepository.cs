using MySqlConnector;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Interfaces;
using RoeiVerenigingLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataReaderExtensions = System.Data.DataReaderExtensions;

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
        public Event Get(int Id)
        {
            Event events = null;
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
                            events = new Event(getMembers(reader.GetInt32("id")), reader.GetDateTime("start_time"), reader.GetDateTime("end_time"), reader.GetString("description"), reader.GetString("name"),
                                reader.GetInt32("id"), reader.GetInt32("max_participants"), GetBoats(reader.GetInt32("id")));
                        }
                    }
                }
            }
            return events;
        }
        private List<Boat> GetBoats(int eventId)
        {
            var list = new List<Boat>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();

                const string sql = "SELECT b.* " +
                                   "FROM event_reserved_boats erb " +
                                   "INNER JOIN boats b ON erb.boat_id = b.id " +
                                   "WHERE erb.event_id = @id";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32);
                    command.Parameters["@id"].Value = eventId;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Boat(reader.GetInt32("id"), reader.GetBoolean("captain_seat_available"), reader.GetInt32("seats"), reader.GetInt32("level"), reader.GetString("description"), reader.GetString("name")));
                        }
                    }
                }
            }
            return list;
        }
        private List<Member> getMembers(int eventId)
        {
            var list = new List<Member>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();

                const string sql = "SELECT m.* " +
                                   "FROM event_participant p " +
                                   "INNER JOIN members m ON p.member_id = m.member_id " +
                                   "WHERE p.event_id = @id";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32);
                    command.Parameters["@id"].Value = eventId;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var infix = "";
                            if (!(reader.IsDBNull(reader.GetOrdinal("infix"))))
                            {
                                infix = reader.GetString("infix");
                            }
                            list.Add(new Member(reader.GetInt32("member_id"), reader.GetString("first_name"), reader.GetString("infix"), reader.GetString("last_name"),  reader.GetString("email"),reader.GetInt32("level")));
                        }
                    }
                }
            }
            return list;
        }
    }

}
