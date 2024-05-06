using System.Threading.Channels;
using MySqlConnector;
using RoeiVerenigingLibary;
 
namespace DataAccessLibary;

public class ReservationRepository : IReservationRepository
{
    public void Create(int boatId, int userId, DateTime? startTime, DateTime? endTime)
    {

        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            String queery =$"INSERT INTO reservation(boat_id, member_id, start_time, end_time)" +
                           "VALUES(@BoatId, @UserID, @StartingTime, @EndTime)";
            Console.WriteLine(queery);

            using (MySqlCommand command = new MySqlCommand(queery, connection)) //defining the paramaters against sql-injection
            {
                command.Parameters.Add("@BoatId", MySqlDbType.Int16);
                command.Parameters.Add("@UserId", MySqlDbType.Int16);
                command.Parameters.Add("@StartingTime", MySqlDbType.DateTime);
                command.Parameters.Add("@EndTime", MySqlDbType.DateTime);
              
                command.Parameters["@BoatId"].Value = boatId;
                command.Parameters["@UserId"].Value = userId;
                command.Parameters["@StartingTime"].Value = startTime;
                command.Parameters["@EndTime"].Value = endTime;
                command.ExecuteReader();
            }
            
            connection.Close();
        }
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
                            reservations.Add(new Reservation(MemberRepository.Get(memberId),creationDate,startTime,endTime,boatId,id));
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