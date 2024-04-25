using System.Threading.Channels;
using MySqlConnector;
using RoeiVerenigingLibary;
 
namespace DataAccessLibary;

public class ReserveDB
{
    private Reservation Get(int userId, int boatId, DateTime startTijd, DateTime eindTijd)
    { 

        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            String queery =$"INSERT INTO reservation(boat_id, member_id, start_time, end_time)" +
                           "VALUES(@BoatId, @UserID, @startTijd, @eindTijd)";
            Console.WriteLine(queery);

            using (MySqlCommand command = new MySqlCommand(queery, connection)) //defining the paramaters against sql-injection
            {
                command.Parameters.Add("@BoatId", MySqlDbType.Int16);
                command.Parameters.Add("@UserId", MySqlDbType.Int16);
                command.Parameters.Add("@startTijd", MySqlDbType.Int16);
                command.Parameters.Add("@eindTijd", MySqlDbType.Int16);
              
                command.Parameters["@BoatId"].Value = boatId;
                command.Parameters["@UserId"].Value = userId;
                command.Parameters["@startTijd"].Value = startTijd;
                command.Parameters["@eindTijd"].Value = eindTijd;
            }
            connection.Close();
        }
      


        return null;
    }
}