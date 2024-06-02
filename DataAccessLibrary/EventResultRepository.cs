using MySqlConnector;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Model;

namespace DataAccessLibrary;

public class EventResultRepository : IEventResultRepository
{
    public void SaveTime(EventParticipant eventParticipant)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            Retry.RetryConnectionOpen(connection);

            const string sql = @"
        UPDATE `event_participant`
        SET `result_time` = @resultTime, `result` = @result
        WHERE `event_id` = @eventId AND `member_id` = @memberId";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@resultTime", MySqlDbType.Time);
                command.Parameters["@resultTime"].Value = eventParticipant.ResultTime;

                command.Parameters.Add("@result", MySqlDbType.VarChar);
                command.Parameters["@result"].Value = eventParticipant.Description;

                command.Parameters.Add("@eventId", MySqlDbType.Int32);
                command.Parameters["@eventId"].Value = eventParticipant.EventId;

                command.Parameters.Add("@memberId", MySqlDbType.Int32);
                command.Parameters["@memberId"].Value = eventParticipant.Id;

                command.ExecuteNonQuery();
            }
        }
    }

    public List<EventParticipant> GetParticipants(int eventId)
    {
        List<EventParticipant> participants = new List<EventParticipant>();

        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            Retry.RetryConnectionOpen(connection);

            const string sql = @"
    SELECT m.*, ep.*
    FROM `event_participant` ep
    JOIN `members` m ON ep.`member_id` = m.`member_id`
    WHERE ep.`event_id` = @eventId
    ORDER BY ep.`result_time` ASC";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@eventId", MySqlDbType.Int32);
                command.Parameters["@eventId"].Value = eventId;

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Member member = new Member(reader.GetInt32("member_id"),
                            reader.GetString("first_name"),
                            reader.GetString("infix"),
                            reader.GetString("last_name"),
                            reader.GetString("email"),
                            new List<String>(),
                            reader.GetInt32("level")
                        );

                        TimeSpan resultTime;
                        if (!reader.IsDBNull(reader.GetOrdinal("result_time")))
                        {
                            resultTime = reader.GetTimeSpan("result_time");
                        }
                        else
                        {
                            resultTime = new TimeSpan();
                        }

                        String resultDesc;
                        if (!reader.IsDBNull(reader.GetOrdinal("result")))
                        {
                            resultDesc = reader.GetString("result");
                        }
                        else
                        {
                            resultDesc = "";
                        }

                        EventParticipant participant = new EventParticipant(
                            member,
                            eventId,
                            resultTime,
                            resultDesc
                        );

                        participants.Add(participant);
                    }
                }
            }
        }

        return participants;
    }
}