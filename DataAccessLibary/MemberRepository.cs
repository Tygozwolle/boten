using MySqlConnector;
using RoeiVerenigingLibary;
using System.ComponentModel;
using System.Net.Mail;
using Microsoft.VisualBasic.CompilerServices;
using System.Reflection.Metadata.Ecma335;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccessLibary;

public class MemberRepository : IMemberRepository
{
    public Member Get(string email, string passwordHash)
    {
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
                            reader.GetString(5), GetRoles(reader.GetInt32(0)), reader.GetInt32(4));
                    }
                }
            }
        }

        return null;
    }

    public static Member Get(int ID)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql = $"SELECT * FROM members WHERE member_id = @Id ";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@Id", MySqlDbType.Int32);
                command.Parameters["@Id"].Value = ID;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new Member(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(5), GetRoles(reader.GetInt32(0)), reader.GetInt32(4));
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

    public Member Create(string firstName, string infix, string lastName, string email, string passwordHash)
    {
        return Create(firstName, infix, lastName, email, passwordHash, 1);
    }

    public Member Create(string firstName, string infix, string lastName, string email, string passwordHash, int level)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();

            const string sql =
                $"INSERT INTO `members`( `first_name`,`infix`, `last_name`, `email`, `password`, 'level') VALUES (@firstName,@infix,@lastName,@email,@passwordHash, @level)";


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

                command.Parameters.Add("@level", MySqlDbType.Int16);
                command.Parameters["@level"].Value = level;

                command.ExecuteReader();
                return new Member((int)command.LastInsertedId, firstName, infix, lastName, email,
                    GetRoles((int)command.LastInsertedId), level);
            }
        }
    }

    public void ChangePassword(string email, string passwordHash)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const String sql =
                $"UPDATE `members` SET `password` = @passwordHash WHERE `email` = @email;";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@email", MySqlDbType.VarChar);
                command.Parameters["@email"].Value = email;

                command.Parameters.Add("@passwordHash", MySqlDbType.VarChar);
                command.Parameters["@passwordHash"].Value = passwordHash;
                command.ExecuteReader();
            }
        }
    }


    public List<Member> GetMembers()
    {
        List<Member> members;

        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql = $"SELECT * FROM members";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    List<Task> tasks = new List<Task>(reader.FieldCount);
                    members = new List<Member>(reader.FieldCount);
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var firstName = reader.GetString(1);
                        string? infix = null;
                        if (!reader.IsDBNull(2))
                        {
                            infix = reader.GetString(2);
                        }

                        var lastName = reader.GetString(3);
                        var email = reader.GetString(5);
                        var level = reader.GetInt32(4);
                        var task = new Task(() =>
                        {
                            Member memberToAdd = new Member(id, firstName, infix, lastName, email, GetRoles(id), level);
                            lock (members)
                            {
                                members.Add(memberToAdd);
                            }
                        });
                        task.Start();
                        tasks.Add(task);
                    }

                    Task.WaitAll(tasks.ToArray());
                }
            }
        }

        return members.OrderBy(x => x.Id).ToList();
    }
}