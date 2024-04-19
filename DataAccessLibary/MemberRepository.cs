using MySqlConnector;
using RoeiVerenigingLibary;

namespace DataAccessLibary;

public class MemberRepository : IMemberRepository
{
    public Member Get(string email, string passwordHash)
    {

        Member? member;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            String sql = $"SELECT * FROM members WHERE email = '{email}' AND password = '{passwordHash}'";
            Console.WriteLine(sql);

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new Member(reader.GetInt32(0), reader.GetString(1), reader.GetString(1), reader.GetString(1));
                    }
                }
            }
        }

        return null;
    }

    public Member Create(string firstName, string lastName, string email, string passwordHash)
    {
        throw new NotImplementedException();
    }
}