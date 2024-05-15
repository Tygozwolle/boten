using System.Data;
using System.Runtime.CompilerServices;
using MySqlConnector;
using RoeiVerenigingLibary;

namespace DataAccessLibary;

public class ReservationRepository : IReservationRepository
{
    public Reservation GetSingleReservation(int memberId, int boatId)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            string query = $"SELECT * FROM reservation WHERE member_id = @userId AND boat_id = @boatId";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("@userId", DbType.Int32);
                command.Parameters.Add("@boatId", DbType.Int32);
                
                command.Parameters["@boatId"].Value = boatId;
                command.Parameters["@userId"].Value = memberId;
                
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                  
                    
                    while (reader.Read())
                    {
                        return new Reservation(reader.GetInt16(2), reader.GetInt16(1), reader.GetDateTime(4),
                            reader.GetDateTime(5));
                    }
                }
            }
        }

        return null;
    }
    
    public Reservation checkReservations(Member member, int boat)
    {
        return null;
    }

    public Reservation ChangeReservation(Member member, int boatId,  DateTime startTime, DateTime endTime)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            string query = $"UPDATE reservation SET boat_id = @boatId, start_time = @startTime, end_time = @endTime WHERE member_id = @memberId AND boat_id = @boatId";
            Console.WriteLine(query);
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("@memberId", MySqlDbType.Int16);
                command.Parameters.Add("@boatId", MySqlDbType.Int16);
                command.Parameters.Add("@startTime", MySqlDbType.DateTime);
                command.Parameters.Add("@endtTime", MySqlDbType.DateTime);
                
                command.Parameters["@boatId"].Value = boatId;
                command.Parameters["@memberId"].Value = member.Id;
                command.Parameters["@startTime"].Value = startTime;
                command.Parameters["@endTime"].Value = endTime;

                command.ExecuteReader();
                return GetSingleReservation(member.Id, boatId);
            }
        }
    }
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
                    while (reader.Read())
                    {
                        return new Reservation(member, reader.GetDateTime(3), startTime, endTime, boatId,
                            reader.GetInt32(0));
                    }
                }
            }

            connection.Close();
        }

        return null;
    }

    public List<Reservation> GetReservations()
    {
        List<Reservation> reservations;

        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql = $"SELECT * FROM reservation";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    List<Task> tasks = new List<Task>(reader.FieldCount);
                    reservations = new List<Reservation>(reader.FieldCount);

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
                            var reservationToAdd = new Reservation(MemberRepository.Get(memberId), creationDate,
                                startTime, endTime, GetBoatById(boatId), id);
                            lock (reservations)
                            {
                                reservations.Add(reservationToAdd);
                            }
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


    public Boat GetBoatById(int boatId)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql = $"SELECT * FROM boats WHERE id = @Id ";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@Id", MySqlDbType.Int32);
                command.Parameters["@Id"].Value = boatId;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new Boat(reader.GetInt32(0), reader.GetBoolean(1), reader.GetInt32(2), reader.GetInt32(3));
                    }
                }
            }
        }
        return null;
    }


    public int GetAmountOfBoatsCurrRenting(int ID)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql =
                $"SELECT COUNT(reservation_id) FROM reservation WHERE member_id = @Id AND start_time > NOW()";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@Id", MySqlDbType.Int32);
                command.Parameters["@Id"].Value = ID;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return (reader.GetInt32(0));
                    }
                }
            }
        }

        return -1;
    }

    public List<Reservation> GetReservations(Member member)
    {
        List<Reservation> reservations = new List<Reservation>();

        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql = $"SELECT * FROM reservation WHERE `member_id` = @memberId";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@memberId", MySqlDbType.Int32);
                command.Parameters["@memberId"].Value = member.Id;

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var boatId = reader.GetInt32(1);
                        var memberId = reader.GetInt32(2);
                        var creationDate = reader.GetDateTime(3);
                        var startTime = reader.GetDateTime(4);
                        var endTime = reader.GetDateTime(5);

                        reservations.Add(new Reservation(member, creationDate,
                            startTime, endTime, GetBoatById(boatId), id));
                    }
                }
            }
        }

        return reservations.OrderBy(x => x.Id).ToList();
    }
}