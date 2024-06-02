using MySqlConnector;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Interfaces;
using RoeiVerenigingLibrary.Model;

namespace DataAccessLibrary
{
    public partial class EventRepository : IEventRepository
    {
        public Event Change(Event events, DateTime startDate, DateTime endDate, string description, String name, int maxParticipants, List<Boat> boatsToAdd, List<Boat> boatsToRemove)
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
                        foreach (var boat in boatsToRemove)
                        {
                            DeleteBoat(events, boat, connection, transaction);
                        }
                        Event eventTemp = ChangeEvent(events, startDate, endDate, description, name, maxParticipants, connection, transaction);
                        foreach (int reservation in reservations)
                        {
                            UpdateReservation(reservation, startDate, endDate, connection, transaction);
                        }
                        int systemId = GetSystemId();
                        foreach (var boat in boatsToAdd)
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
        
        private void DeleteBoat(Event events, Boat boat, MySqlConnection connection, MySqlTransaction transaction)
        {
            DeleteReservation(events, boat, connection, transaction);
            const string sql = "DELETE FROM `event_reserved_boats` WHERE `event_id` = @event_id AND `boat_id` = @boat_id";
            
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
        private void DeleteReservation(Event events, Boat boat, MySqlConnection connection, MySqlTransaction transaction)
        {
            const string sql = "DELETE FROM `reservation` WHERE `boat_id` = @boat_id AND `start_time` = @start_time AND `end_time` = @end_time";
            
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@boat_id", MySqlDbType.Int32);
                    command.Parameters["@boat_id"].Value = boat.Id;

                    command.Parameters.Add("@start_time", MySqlDbType.DateTime);
                    command.Parameters["@start_time"].Value = events.StartDate;

                    command.Parameters.Add("@end_time", MySqlDbType.DateTime);
                    command.Parameters["@end_time"].Value = events.EndDate;

                    command.Transaction = transaction;
                    command.ExecuteNonQuery();
                }
            
        }
        
        private void UpdateReservation(int reservation, DateTime startDate, DateTime endDate, MySqlConnection connection, MySqlTransaction transaction)
        {
            const string sql =
                "UPDATE `reservation` SET `start_time` = @start_time, `end_time` = @end_time WHERE `reservation_id` = @id";

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