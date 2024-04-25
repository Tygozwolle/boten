using MySqlConnector;
using RoeiVerenigingLibary;
using System.ComponentModel;
using System.Net.Mail;
using Microsoft.VisualBasic.CompilerServices;
using System.Reflection.Metadata.Ecma335;

namespace DataAccessLibary;

public class MemberRepository : IMemberRepository
{
    public Member Get(string email, string passwordHash)
    {
        if (!IsValid(email))
        {
            throw new Exception("is not a email");
        }

        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql = $"SELECT * FROM members WHERE email = @email AND password = @passwordHash";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@email", MySqlDbType.VarChar);
                command.Parameters["@email"].Value = email;
                command.Parameters.Add("@passwordHash", MySqlDbType.VarChar);
                command.Parameters["@passwordHash"].Value = passwordHash;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new Member(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4), GetRoles(reader.GetInt32(0)));
                    }
                }
            }
        }

        return null;
    }

    private static List<string> GetRoles(int id)
    {
        var list = new List<string>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const String sql = $"SELECT * FROM member_roles WHERE member_id = @id";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.VarChar);
                command.Parameters["@id"].Value = id;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetString(1));
                    }
                }
            }
        }

        return list;
    }

    private static bool IsValid(string email)
    {
        return MailAddress.TryCreate(email, out var result);
    }

    public Member Create(string firstName, string infix, string lastName, string email, string passwordHash)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const String sql =
                $"INSERT INTO `members`( `first_name`,`infix`, `last_name`, `email`, `password`) VALUES (@firstName,@infix,@lastName,@email,@passwordHash)";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@firstName", MySqlDbType.VarChar);
                command.Parameters["@firstName"].Value = firstName;

                command.Parameters.Add("@infix", MySqlDbType.VarChar);
                command.Parameters["@infix"].Value = infix;

                command.Parameters.Add("@lastName", MySqlDbType.VarChar);
                command.Parameters["@lastName"].Value = lastName;

                command.Parameters.Add("@email", MySqlDbType.VarChar);
                command.Parameters["@email"].Value = email;

                command.Parameters.Add("@passwordHash", MySqlDbType.VarChar);
                command.Parameters["@passwordHash"].Value = passwordHash;
                command.ExecuteReader();
                return new Member((int)command.LastInsertedId, firstName, infix, lastName, email,
                    GetRoles((int)command.LastInsertedId));
            }
        }

        return null;
    }
}