using System;
using System.Data;
using System.Threading.Channels;
using MySqlConnector;
using RoeiVerenigingLibary;

namespace DataAccessLibary;

public class ReservationRepository : IReservationRepository
{
    public Reservation CreateReservation(Member member, int boatId, DateTime startTime, DateTime endTime)
    {   
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            String query = $"INSERT INTO reservation(boat_id, member_id, start_time, end_time)" +
                           "VALUES(@BoatId, @UserID, @StartingTime, @EndTime)";
            Console.WriteLine(query);

            using (MySqlCommand
                   command = new MySqlCommand(query, connection)) //defining the paramaters against sql-injection
            {
                command.Parameters.Add("@BoatId", MySqlDbType.Int16);
                command.Parameters.Add("@UserId", MySqlDbType.Int16);
                command.Parameters.Add("@StartingTime", MySqlDbType.DateTime);
                command.Parameters.Add("@EndTime", MySqlDbType.DateTime);

                command.Parameters["@BoatId"].Value = boatId;
                command.Parameters["@UserId"].Value = member.Id;
                command.Parameters["@StartingTime"].Value = startTime;
                command.Parameters["@EndTime"].Value = endTime;

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read()) { 
                    return new Reservation(member, reader.GetDateTime(3), startTime, endTime, boatId,   reader.GetInt32(0));
                } }}

            connection.Close();
        }
        return null;
    }

    public List<Reservation> GetReservations()
    {
        List<Reservation> reservations = new List<Reservation>();

        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql = $"SELECT * FROM reservation";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    List<Task> tasks = new List<Task>(reader.FieldCount);

                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var boatId = reader.GetInt32(1);
                        var memberId = reader.GetInt32(2);
                        var creationDate = reader.GetDateTime(3);
                        var startTime = reader.GetDateTime(4);
                        var endTime = reader.GetDateTime(5);

                        var task = new Task(() =>
                        {
                            reservations.Add(new Reservation(MemberRepository.Get(memberId), creationDate,
                                startTime, endTime, boatId, id));
                        });
                        task.Start();
                        tasks.Add(task);
                    }

                    Task.WaitAll(tasks.ToArray());
                }
            }
        }

        return reservations.OrderBy(x => x.Id).ToList();
    }
}