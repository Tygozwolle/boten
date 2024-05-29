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
    public class EventRepository : IEventRepository
    {
        public Event Create(DateTime startTime, DateTime endDate, string descriptions, string name, int maxParticipants, List<Boat> boats, Member member)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        const string sqlAutocommit = "SET autocommit=0";
                        using (MySqlCommand command = new MySqlCommand(sqlAutocommit, connection))
                        {
                            command.Transaction = transaction;
                            command.ExecuteNonQuery();
                        }
                        Event eventTemp = CreateEvent(startTime, endDate, descriptions, name, maxParticipants, boats, connection, transaction);
                        foreach (Boat boat in boats)
                        {
                            AddBoatsToEvent(eventTemp, boat, connection, transaction);
                            MakeEventReservation(eventTemp, boat, member, connection, transaction);
                        }
                        transaction.Commit();
                        return eventTemp;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        private void AddBoatsToEvent(Event events, Boat boat, MySqlConnection connection, MySqlTransaction transaction)
        {
            const string sql =
                "INSERT INTO `event_reserved_boats` (`event_id`, `boat_id`) VALUES (@event_id, @boat_id)";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@event_id", MySqlDbType.Int32);
                command.Parameters["@event_id"].Value = events.Id;

                command.Parameters.Add("@boat_id", MySqlDbType.Int32);
                command.Parameters["@boat_id"].Value = boat.Id;

                command.Transaction = transaction;
                command.ExecuteNonQuery();
            }
        }

        private void MakeEventReservation(Event events, Boat boat, Member member, MySqlConnection connection, MySqlTransaction transaction)
        {
            const string sql =
                "INSERT INTO `reservation` (`boat_id`, `member_id`, `start_time`, `end_time`) VALUES (@boat_id, @member_id, @start_time, @end_time)";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@boat_id", MySqlDbType.Int32);
                command.Parameters["@boat_id"].Value = boat.Id;

                command.Parameters.Add("@member_id", MySqlDbType.Int32);
                command.Parameters["@member_id"].Value = member.Id;

                command.Parameters.Add("@start_time", MySqlDbType.DateTime);
                command.Parameters["@start_time"].Value = events.StartDate;

                command.Parameters.Add("@end_time", MySqlDbType.DateTime);
                command.Parameters["@end_time"].Value = events.EndDate;

                command.Transaction = transaction;
                command.ExecuteNonQuery();
            }
        }

        private Event CreateEvent(DateTime startTime, DateTime endDate, string descriptions, string name, int maxParticipants, List<Boat> boats, MySqlConnection connection, MySqlTransaction transaction)
        {
            const string sql =
                "INSERT INTO `events` (`start_time`, `end_time`, `description`, `name`, `max_participants`) VALUES (@start_time, @end_time, @descriptions, @name, @max_participants)";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@start_time", MySqlDbType.DateTime);
                command.Parameters["@start_time"].Value = startTime;

                command.Parameters.Add("@end_time", MySqlDbType.DateTime);
                command.Parameters["@end_time"].Value = endDate;

                command.Parameters.Add("@descriptions", MySqlDbType.VarChar);
                command.Parameters["@descriptions"].Value = descriptions;

                command.Parameters.Add("@name", MySqlDbType.VarChar);
                command.Parameters["@name"].Value = name;

                command.Parameters.Add("@max_participants", MySqlDbType.Int32);
                command.Parameters["@max_participants"].Value = maxParticipants;

                command.Transaction = transaction;
                command.ExecuteNonQuery();
                return new Event(new List<Member>(), startTime, endDate, descriptions, name, (int)command.LastInsertedId, maxParticipants, boats);
            }
        }
        public Event Change(Event events, DateTime startDate, DateTime endDate, string description, String name, int maxParticipants)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();

                const string sql =
                    $"UPDATE `events` SET `max_participants` = @participants, `start_time` = @startTime, `end_time` = @endTime, `description` = @description, `name` = @name WHERE id = @id";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@participants", MySqlDbType.Int32);
                    command.Parameters["@participants"].Value = maxParticipants;

                    command.Parameters.Add("@startTime", MySqlDbType.DateTime);
                    command.Parameters["@startTime"].Value = startDate;

                    command.Parameters.Add("@endTime", MySqlDbType.DateTime);
                    command.Parameters["@endTime"].Value = endDate;

                    command.Parameters.Add("@description", MySqlDbType.VarChar);
                    command.Parameters["@description"].Value = description;

                    command.Parameters.Add("@name", MySqlDbType.VarChar);
                    command.Parameters["@name"].Value = name;

                    command.Parameters.Add("@id", MySqlDbType.Int32);
                    command.Parameters["@id"].Value = events.Id;

                    command.ExecuteReader();
                    //TODO: return event with boats en members waiting for other branches 
                    return new Event(new List<Member>(), startDate, endDate, description, name, (int)command.LastInsertedId, maxParticipants, new List<Boat>());
                }
            }
        }
        public List<int> GetEventReservationsIds(Event events)
        {
            var list = new List<int>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();

                const string sql = "SELECT `reservation_id` FROM `reservation` WHERE (SELECT `boat_id` FROM `event_reserved_boats` WHERE `event_id` = @id) = `boat_id` AND `start_time` = @start_time AND `end_time` = @end_time";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32);
                    command.Parameters["@id"].Value = events.Id;
                    
                    command.Parameters.Add("@start_time", MySqlDbType.DateTime);
                    command.Parameters["@start_time"].Value = events.StartDate;
                    
                    command.Parameters.Add("@end_time", MySqlDbType.DateTime);
                    command.Parameters["@end_time"].Value = events.EndDate;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetInt32("reservation_id"));
                        }
                    }
                }
            }
            return list;
        }
        
    }
}
