using MySqlConnector;
using RoeiVerenigingLibary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibary
{
    public class DamageRepository : IDamageRepository
    {
        public Damage Create(Member member, Boat boat, string description)
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
                    return new Damage((int)command.LastInsertedId, member, boat, description, false, false);
                }
            }
        }

        public List<Damage> GetAllDamageReports()
        {
            List<Damage> damageReports = new List<Damage>();
            MemberRepository memberRepository = new MemberRepository();
            BoatRepository boatRepository = new BoatRepository();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();

                const string sql =
                    "SELECT * FROM `damage_reports` ORDER BY `fixed`, `report_time` DESC";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //get member
                            Member member = memberRepository.GetById(reader.GetInt32("member_id"));
                            //get boat
                            Boat boat = boatRepository.GetBoatById(reader.GetInt32("boat_id"));
                            Damage damageReport = new Damage(reader.GetInt32("id"), member, boat,
                                reader.GetString("description"),
                                reader.GetBoolean("fixed"), reader.GetBoolean("usable"));

                            damageReports.Add(damageReport);
                        }
                    }
                }
            }

            return damageReports;
        }

        public List<Damage> GetRelatedToUser(int memberId)
        {
            List<Damage> damageReports = new List<Damage>();
            MemberRepository memberRepository = new MemberRepository();
            BoatRepository boatRepository = new BoatRepository();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();

                const string sql =
                    "SELECT * FROM `damage_reports` WHERE `member_id` = @memberId ORDER BY `fixed`, `report_time` DESC";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@memberId", MySqlDbType.Int32);
                    command.Parameters["@memberId"].Value = memberId;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //get member
                            Member member = memberRepository.GetById(reader.GetInt32("member_id"));
                            //get boat
                            Boat boat = boatRepository.GetBoatById(reader.GetInt32("boat_id"));
                            Damage damageReport = new Damage(reader.GetInt32("id"), member, boat,
                                reader.GetString("description"),
                                reader.GetBoolean("fixed"), reader.GetBoolean("usable"));

                            damageReports.Add(damageReport);
                        }
                    }
                }
            }

            return damageReports;
        }

        public Damage Update(int id, bool boatFixed, bool usable, string description)
        {
            Damage damage = null;

            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();

                const string sql =
                    "UPDATE `damage_reports` SET `fixed` = @boatFixed, `usable` = @usable, `description` = @description WHERE `id` = @id";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@boatFixed", MySqlDbType.Bool);
                    command.Parameters["@boatFixed"].Value = boatFixed;

                    command.Parameters.Add("@usable", MySqlDbType.Bool);
                    command.Parameters["@usable"].Value = usable;

                    command.Parameters.Add("@description", MySqlDbType.VarChar);
                    command.Parameters["@description"].Value = description;

                    command.Parameters.Add("@id", MySqlDbType.Int32);
                    command.Parameters["@id"].Value = id;

                    command.ExecuteNonQuery();

                    // After updating, retrieve the updated damage report
                    damage = GetById(id);
                }
            }

            return damage;
        }

        public Damage GetById(int id)
        {
            Damage damage = null;
            MemberRepository memberRepository = new MemberRepository();
            BoatRepository boatRepository = new BoatRepository();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();

                const string sql = "SELECT * FROM `damage_reports` WHERE `id` = @id";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32);
                    command.Parameters["@id"].Value = id;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //get member
                            var imageRepository = new ImageRepository();
                            Member member = memberRepository.GetById(reader.GetInt32("member_id"));
                            //get boat
                            Boat boat = boatRepository.GetBoatById(reader.GetInt32("boat_id"));
                            damage = new Damage(reader.GetInt32("id"), member, boat,
                                reader.GetString("description"),
                                reader.GetBoolean("fixed"), reader.GetBoolean("usable"), imageRepository.get(reader.GetInt32("id")));
                        }
                    }
                }
            }

            return damage;
        }
    }
}