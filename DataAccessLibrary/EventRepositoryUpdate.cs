using MySqlConnector;
using RoeiVerenigingLibrary.Interfaces;
using RoeiVerenigingLibrary.Model;

namespace DataAccessLibrary
{
    public partial class EventRepository : IEventRepository
    {
        public Event Change(Event events, DateTime startDate, DateTime endDate, string description, String name, int maxParticipants)
        {
            var reservations = GetEventReservationsIds(events);
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
                        Event eventTemp = ChangeEvent(events, startDate, endDate, description, name, maxParticipants, connection, transaction);
                        foreach (int reservation in reservations)
                        {
                            UpdateReservation(reservation, startDate, endDate, connection, transaction);
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
        private void UpdateReservation(int reservation, DateTime startDate, DateTime endDate, MySqlConnection connection, MySqlTransaction transaction)
        {
            const string sql =
                "UPDATE `reservation` SET `start_time` = @start_time, `end_time` = @end_time WHERE `id` = @id";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@start_time", MySqlDbType.DateTime);
                command.Parameters["@start_time"].Value = startDate;

                command.Parameters.Add("@end_time", MySqlDbType.DateTime);
                command.Parameters["@end_time"].Value = endDate;

                command.Parameters.Add("@id", MySqlDbType.Int32);
                command.Parameters["@id"].Value = reservation;

                command.Transaction = transaction;
                command.ExecuteNonQuery();
            }
        }
        private Event ChangeEvent(Event events, DateTime startDate, DateTime endDate, string description, string name, int maxParticipants, MySqlConnection connection, MySqlTransaction transaction)
        {

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

                command.Transaction = transaction;
                command.ExecuteNonQuery();
            }
            return new Event(events.Participants, startDate, endDate, description, name, events.Id, maxParticipants, events.Boats);
        }
        public List<int> GetEventReservationsIds(Event events) 
        {
            var list = new List<int>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();

                const string sql = "SELECT `reservation_id` FROM `reservation` WHERE `boat_id` IN (SELECT `boat_id` FROM `event_reserved_boats` WHERE `event_id` = @id) " +
                                   "AND `start_time` = @start_time " +
                                   "AND `end_time` = @end_time";

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