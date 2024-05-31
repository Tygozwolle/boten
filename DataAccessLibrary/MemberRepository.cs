using MySqlConnector;
using RoeiVerenigingLibrary;

namespace DataAccessLibrary;

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
                        string? infix = null;
                        if (!reader.IsDBNull(2))
                        {
                            infix = reader.GetString(2);
                        }

                        return new Member(reader.GetInt32(0), reader.GetString(1), infix,
                            reader.GetString(3),
                            reader.GetString(5), GetRoles(reader.GetInt32(0)), reader.GetInt32(4));
                    }
                }
            }
        }

        return null;
    }

    public Member GetById(int id)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql = "SELECT * FROM members WHERE member_id = @id";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32);
                command.Parameters["@id"].Value = id;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string? infix = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        return new Member(reader.GetInt32(0), reader.GetString(1), infix, reader.GetString(3),
                            reader.GetString(5), GetRoles(reader.GetInt32(0)), reader.GetInt32(4));
                    }
                }
            }
        }

        return null;
    }

    public Member Update(int id, string firstName, string infix, string lastName, string email, int level)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql =
                "UPDATE `members` SET `first_name` = @firstName, `infix` = @infix, `last_name` = @lastName, `email` = @email, `level` = @level WHERE `member_id` = @id";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32);
                command.Parameters["@id"].Value = id;

                command.Parameters.Add("@firstName", MySqlDbType.VarChar);
                command.Parameters["@firstName"].Value = firstName;

                command.Parameters.Add("@infix", MySqlDbType.VarChar);
                command.Parameters["@infix"].Value = infix;

                command.Parameters.Add("@lastName", MySqlDbType.VarChar);
                command.Parameters["@lastName"].Value = lastName;

                command.Parameters.Add("@email", MySqlDbType.VarChar);
                command.Parameters["@email"].Value = email;

                command.Parameters.Add("@level", MySqlDbType.Int32);
                command.Parameters["@level"].Value = level;

                command.ExecuteNonQuery();
                
                return new Member(id, firstName, infix, lastName, email, GetRoles(id), level);
            }
        }
    }

    public Member Update(int id, string firstName, string infix, string lastName, string email)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql =
                "UPDATE `members` SET `first_name` = @firstName, `infix` = @infix, `last_name` = @lastName, `email` = @email WHERE `member_id` = @id";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32);
                command.Parameters["@id"].Value = id;

                command.Parameters.Add("@firstName", MySqlDbType.VarChar);
                command.Parameters["@firstName"].Value = firstName;

                command.Parameters.Add("@infix", MySqlDbType.VarChar);
                command.Parameters["@infix"].Value = infix;

                command.Parameters.Add("@lastName", MySqlDbType.VarChar);
                command.Parameters["@lastName"].Value = lastName;

                command.Parameters.Add("@email", MySqlDbType.VarChar);
                command.Parameters["@email"].Value = email;

                command.ExecuteNonQuery();
                //todo: add level
                return new Member(id, firstName, infix, lastName, email, GetRoles(id), 1);
            }
        }
    }


    public Member Create(string firstName, string infix, string lastName, string email, string passwordHash, int level)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();

            const string sql =
                "INSERT INTO `members`( `first_name`,`infix`, `last_name`, `email`, `password`, `level`) VALUES (@firstName,@infix,@lastName,@email,@passwordHash, @level)";


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
            const string sql =
                "UPDATE `members` SET `password` = @passwordHash WHERE `email` = @email;";

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
            const string sql = "SELECT * FROM members";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    members = new List<Member>(reader.FieldCount);
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("member_id");
                        string firstName = reader.GetString("first_name");
                        string? infix = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("infix")))
                        {
                            infix = reader.GetString("infix");
                        }

                        string lastName = reader.GetString("last_name");
                        string email = reader.GetString("email");
                        int level = reader.GetInt32("level");
                        Member memberToAdd = new Member(id, firstName, infix, lastName, email, new List<string>(), level);
                        members.Add(memberToAdd);
                    }
                    AddRolesToMembers(members);
                }
            }
        }

        return members.OrderBy(x => x.Id).ToList();
    }
    /// <summary>
    /// Add all rolls to the list with members
    /// </summary>
    /// <param name="members"></param>
    private void AddRolesToMembers(List<Member> members)
    {
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();
                const string sql = "SELECT * FROM member_roles";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = reader.GetInt32("member_id");
                            var role = reader.GetString("role");
                            members.Find(x => x.Id == id)?.Roles.Add(role);
                        }
                    }
                }
            }
        }
    }

    public void AddRole(int memberId, string role)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            string insertSql = "INSERT INTO member_roles(member_id, role) VALUES (@memberId, @role)";
            using (MySqlCommand insertCommand = new MySqlCommand(insertSql, connection))
            {
                insertCommand.Parameters.Add("@memberId", MySqlDbType.Int32);
                insertCommand.Parameters["@memberId"].Value = memberId;
                insertCommand.Parameters.Add("@role", MySqlDbType.VarChar);
                insertCommand.Parameters["@role"].Value = role;
                insertCommand.ExecuteNonQuery();
            }
        }
    }

    public void RemoveRoles(int memberId)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            string deleteSql = "DELETE FROM member_roles WHERE member_id = @memberId";
            using (MySqlCommand deleteCommand = new MySqlCommand(deleteSql, connection))
            {
                deleteCommand.Parameters.Add("@memberId", MySqlDbType.Int32);
                deleteCommand.Parameters["@memberId"].Value = memberId;
                deleteCommand.ExecuteNonQuery();
            }
        }
    }

    public List<string> GetAvailableRoles()
    {
        var roles = new List<string>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql = "SELECT DISTINCT role FROM member_roles";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(reader.GetString(0));
                    }
                }
            }
        }

        return roles;
    }

    public static Member Get(int id)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql = "SELECT * FROM members WHERE member_id = @Id ";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@Id", MySqlDbType.Int32);
                command.Parameters["@Id"].Value = id;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string? infix = null;
                        if (!reader.IsDBNull(2))
                        {
                            infix = reader.GetString(2);
                        }

                        return new Member(reader.GetInt32(0), reader.GetString(1), infix,
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
            const string sql = "SELECT * FROM member_roles WHERE member_id = @id";

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
}