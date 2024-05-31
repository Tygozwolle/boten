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
                            list.Add(new Event(new List<EventParticipant>(), reader.GetDateTime("start_time"),
                                reader.GetDateTime("end_time"), reader.GetString("description"),
                                reader.GetString("name"),
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
                const string sql =
                    " INSERT INTO boten_reservering.members (member_id, first_name, infix, last_name, level, email, password)\nVALUES (0, 'System', null, 'System', DEFAULT, 'System', 'System');";

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
                            list.Add(new Boat(reader.GetInt32("id"), reader.GetBoolean("captain_seat_available"),
                                reader.GetInt32("seats"), reader.GetInt32("level"), reader.GetString("description"),
                                reader.GetString("name")));
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

                            list.Add(new Member(reader.GetInt32("member_id"), reader.GetString("first_name"), infix,
                                reader.GetString("last_name"), reader.GetString("email"), reader.GetInt32("level")));
                        }
                    }
                }
            }

            return list;
        }

        public List<Event> GetEvents()
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();


                const string sql =
                    "SELECT * FROM 'events'";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    var events = new List<Event>();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int eventId = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string description = reader.GetString(2);
                            int maxParticipants = reader.GetInt32(3);
                            DateTime startTime = reader.GetDateTime(4);
                            DateTime endTime = reader.GetDateTime(5);

                            Event addevent = new(GetEventMembersid(eventId), startTime, endTime, description, name, eventId,
                                maxParticipants, GetEventBoatid(eventId));
                            events.Add(addevent);
                        }

                    }

                    return events;
                }
            }
        }

        public List<Member> GetEventMembersid(int eventid)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();

                var member = new List<Member>();
                MemberService _memberService = new MemberService(new MemberRepository());


                const string sql =
                    "SELECT * FROM 'event_participant' WHERE event_id = @event_id";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        command.Parameters.Add("@id", MySqlDbType.Int32);
                        command.Parameters["@event_id"].Value = eventid;

                        while (reader.Read())
                        {
                            int memberId = reader.GetInt32(1);
                            member.Add(_memberService.GetById(memberId));
                        }

                    }

                    return member;
                }
            }
        }

        public List<Boat> GetEventBoatid(int eventid)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();

                var boat = new List<Boat>();
                BoatService _boatService = new BoatService(new BoatRepository());


                const string sql =
                    "SELECT * FROM 'event_participant' WHERE event_id = @event_id";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        command.Parameters.Add("@id", MySqlDbType.Int32);
                        command.Parameters["@event_id"].Value = eventid;

                        while (reader.Read())
                        {
                            int memberId = reader.GetInt32(1);
                            boat.Add(_boatService.GetBoatById(memberId));
                        }

                    }

                    return boat;
                }
            }
        }
    }

            return eventTemp;
        }

        public List<Event> GetEventsFromPastMonths(int AmountOfMonths)
        {
            List<Event> events = new List<Event>();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();

                const string sql = @"
            SELECT *
            FROM `events`
            WHERE `end_time` BETWEEN DATE_ADD(CURDATE(), INTERVAL -@AmountOfMonths MONTH) AND CURDATE()";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@AmountOfMonths", MySqlDbType.Int32);
                    command.Parameters["@AmountOfMonths"].Value = AmountOfMonths;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var participants =
                                new List<EventParticipant>(); //Empty because the data can be retrieved when it is needed
                            var boats = new List<Boat>(); //Empty because the data can be retrieved when it is needed

                            Event eventTemp = new Event(
                                participants,
                                reader.GetDateTime("start_time"),
                                reader.GetDateTime("end_time"),
                                reader.GetString("description"),
                                reader.GetString("name"),
                                reader.GetInt32("id"),
                                reader.GetInt32("max_participants"),
                                boats
                            );

                            events.Add(eventTemp);
                        }
                    }
                }
            }

            return events;
        }
    }
}