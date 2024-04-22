using MySqlConnector;
using RoeiVerenigingLibary;
using System.ComponentModel;
using System.Net.Mail;
using Microsoft.VisualBasic.CompilerServices;

namespace DataAccessLibary;

public class MemberRepository : IMemberRepository
{
    public Member Get(string email, string passwordHash)
    {
        if (!IsValid(email))
        {
            throw new Exception("is not a email");
        }

        Member? member;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            String sql = $"SELECT * FROM members WHERE email = @email AND password = @passwordHash";
            Console.WriteLine(sql);

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
                            reader.GetString(3), GetRoles(reader.GetInt32(0)));
                    }
                }
            }
        }

        return null;
    }

    private List<string> GetRoles(int id)
    {
        var list = new List<string>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            String sql = $"SELECT * FROM member_roles WHERE member_id = {id}";
            Console.WriteLine(sql);

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
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

    private bool IsValid(string email)
    {
        return MailAddress.TryCreate(email, out var result);

    }

    public Member Create(string firstName, string lastName, string email, string passwordHash)
    {

        Member? member;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            String sql = $"INSERT INTO `members`( `first_name`, `last_name`, `email`, `password`) VALUES (@firstName,@lastName,@email,@passwordHash)";
            Console.WriteLine(sql);

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@firstName", MySqlDbType.VarChar);
                command.Parameters["@firstName"].Value = firstName;

                command.Parameters.Add("@lastName", MySqlDbType.VarChar);
                command.Parameters["@lastName"].Value = lastName;

                command.Parameters.Add("@email", MySqlDbType.VarChar);
                command.Parameters["@email"].Value = email;

                command.Parameters.Add("@passwordHash", MySqlDbType.VarChar);
                command.Parameters["@passwordHash"].Value = passwordHash;
                command.ExecuteReader();
                 return new Member((int) command.LastInsertedId, firstName, lastName, email, GetRoles((int) command.LastInsertedId));
              
                }
            }
            
        }

       
        
    }
