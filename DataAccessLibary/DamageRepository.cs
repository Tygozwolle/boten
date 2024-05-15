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
        public Damage Create( Member member, Boat boat, string description)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();

                const string sql =
                    $"INSERT INTO `damage_reports`( `boat_id`, `description`, `member_id`) VALUES (@boat_id, @description, @member_id)";


                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@boat_id", MySqlDbType.Int32);
                    command.Parameters["@boat_id"].Value = boat.Id;

                    command.Parameters.Add("@description", MySqlDbType.VarChar);
                    command.Parameters["@description"].Value = description;

                    command.Parameters.Add("@member_id", MySqlDbType.Int32);
                    command.Parameters["@member_id"].Value = member.Id;


                    command.ExecuteReader();
                    return new Damage((int)command.LastInsertedId,member,boat,false,false);
                }
            }
        }
    }
}
