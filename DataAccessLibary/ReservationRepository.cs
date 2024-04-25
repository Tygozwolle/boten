using System.Threading.Channels;
using MySqlConnector;
using RoeiVerenigingLibary;
 
namespace DataAccessLibary;

public class ReservationRepository : IReservationRepository
{
    public ReservationCreator Create(int userId, int boatId, DateTime StartTime, DateTime EndTime)
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
                command.Parameters["@StartingTime"].Value = StartTime;
                command.Parameters["@EndTime"].Value = EndTime;
                command.ExecuteReader();
            }
            
            connection.Close();
        }
      


        return null;
    }
}