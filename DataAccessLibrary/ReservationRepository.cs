using MySqlConnector;
using RoeiVerenigingLibrary.Interfaces;
using RoeiVerenigingLibrary.Model;

namespace DataAccessLibrary;

public class ReservationRepository : IReservationRepository
{
    public Reservation ChangeReservation(int reservationId, Member member, int boatId, DateTime startTime,
        DateTime endTime)
    {
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            var query =
                "UPDATE reservation SET boat_id = @boatId, start_time = @startTime, end_time = @endTime WHERE member_id = @memberId AND boat_id = @boatId AND reservation_id = @reservationid";
            Console.WriteLine(query);
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.Add("@memberId", MySqlDbType.Int16);
                command.Parameters.Add("@boatId", MySqlDbType.Int16);
                command.Parameters.Add("@reservationid", MySqlDbType.Int16);
                command.Parameters.Add("@startTime", MySqlDbType.DateTime);
                command.Parameters.Add("@endTime", MySqlDbType.DateTime);

                command.Parameters["@boatId"].Value = boatId;
                command.Parameters["@memberId"].Value = member.Id;
                command.Parameters["@reservationid"].Value = reservationId;
                command.Parameters["@startTime"].Value = startTime;
                command.Parameters["@endTime"].Value = endTime;

                command.ExecuteReader();
            }
        }

        return null;
    }

    public Reservation CreateReservation(Member member, int boatId, DateTime startTime, DateTime endTime)
    {
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            var query = "INSERT INTO reservation(boat_id, member_id, start_time, end_time)" +
                        "VALUES(@BoatId, @UserID, @StartingTime, @EndTime)";
            Console.WriteLine(query);

            using (var
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

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        return new Reservation(member, reader.GetDateTime(3), startTime, endTime, GetBoatById(boatId),
                            reader.GetInt32(0));
                }
            }

            connection.Close();
        }

        return null;
    }

    public List<Reservation> GetReservations()
    {
        List<Reservation> reservations;

        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql = "SELECT * FROM reservation";

            using (var command = new MySqlCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    var tasks = new List<Task>(reader.FieldCount);
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


    public int GetAmountOfBoatsCurrRenting(int id)
    {
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql =
                "SELECT COUNT(reservation_id) FROM reservation WHERE member_id = @Id AND start_time > NOW()";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@Id", MySqlDbType.Int32);
                command.Parameters["@Id"].Value = id;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) return reader.GetInt32(0);
                }
            }
        }

        return -1;
    }

    public bool BoatAlreadyReserved(int boatId, DateTime startTime, DateTime endTime)
    {
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            var sql =
                "SELECT COUNT(*) FROM reservation WHERE boat_id = @BoatId AND ((start_time < @EndTime AND end_time > @StartTime))";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@BoatId", MySqlDbType.Int32);
                command.Parameters.Add("@StartTime", MySqlDbType.DateTime);
                command.Parameters.Add("@EndTime", MySqlDbType.DateTime);

                command.Parameters["@BoatId"].Value = boatId;
                command.Parameters["@StartTime"].Value = startTime;
                command.Parameters["@EndTime"].Value = endTime;

                var count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }
    }

    public List<Reservation> GetReservations(Member member)
    {
        var reservations = new List<Reservation>();

        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql = "SELECT * FROM reservation WHERE `member_id` = @memberId";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@memberId", MySqlDbType.Int32);
                command.Parameters["@memberId"].Value = member.Id;

                using (var reader = command.ExecuteReader())
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

    public Reservation GetReservation(int reservationid)
    {
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql = "SELECT * FROM reservation WHERE `reservation_id` = @reservation_id";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@reservation_id", MySqlDbType.Int32);
                command.Parameters["@reservation_id"].Value = reservationid;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var boatId = reader.GetInt32(1);
                        var memberId = reader.GetInt32(2);
                        var startTime = reader.GetDateTime(4);
                        var endTime = reader.GetDateTime(5);

                        return new Reservation(MemberRepository.Get(memberId), boatId, startTime, endTime);
                    }
                }
            }
        }

        return null;
    }

    public Boat GetBoatById(int boatId)
    {
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql = "SELECT * FROM boats WHERE id = @Id ";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@Id", MySqlDbType.Int32);
                command.Parameters["@Id"].Value = boatId;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        return new Boat(reader.GetInt32(0), reader.GetBoolean(1), reader.GetInt32(2),
                            reader.GetInt32(3), reader.GetString(4), reader.GetString(5));
                }
            }
        }

        return null;
    }
}