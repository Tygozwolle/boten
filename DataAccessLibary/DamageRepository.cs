using MySqlConnector;
using RoeiVerenigingLibary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibary
{
    public class DamageRepository : IDamageRepository
    {
        public void Create(string firstName, string infix, string lastName, string email, string passwordHash, int level)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();

                const string sql =
                    $"INSERT INTO `damage_reports`( `boat_id`, `description`, `member_id`) VALUES (@boat_id, @description, @member_id)";


                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@boat_id", MySqlDbType.Int32);
                    command.Parameters["@boat_id"].Value = firstName;

                    command.Parameters.Add("@description", MySqlDbType.VarChar);
                    command.Parameters["@description"].Value = infix;

                    command.Parameters.Add("@member_id", MySqlDbType.Int32);
                    command.Parameters["@member_id"].Value = lastName;


                    command.ExecuteReader();
                    return ;
                }
            }
        }
    }
}
