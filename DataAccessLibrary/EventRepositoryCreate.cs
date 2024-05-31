using MySqlConnector;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Interfaces;
using RoeiVerenigingLibrary.Model;

namespace DataAccessLibrary
{
    public partial class EventRepository : IEventRepository
    {
          public Event Create(DateTime startTime, DateTime endDate, string descriptions, string name, int maxParticipants, List<Boat> boats, Member member)
        {
            var systemId = GetSystemId();
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
                            MakeEventReservation(eventTemp, boat, systemId, connection, transaction);
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
        
        private void MakeEventReservation(Event events, Boat boat, int memberId, MySqlConnection connection, MySqlTransaction transaction)
        {
            const string sql =
                "INSERT INTO `reservation` (`boat_id`, `member_id`, `start_time`, `end_time`) VALUES (@boat_id, @member_id, @start_time, @end_time)";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@boat_id", MySqlDbType.Int32);
                command.Parameters["@boat_id"].Value = boat.Id;

                command.Parameters.Add("@member_id", MySqlDbType.Int32);
                command.Parameters["@member_id"].Value = memberId;

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
                return new Event(new List<EventParticipant>(), startTime, endDate, descriptions, name, (int)command.LastInsertedId, maxParticipants, boats);
            }
        }
        
    }
}