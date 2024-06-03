using MySqlConnector;
using RoeiVerenigingLibrary;

namespace DataAccessLibrary
{
    public class DamageRepository : IDamageRepository
    {
        public Damage Create(Member member, Boat boat, string description)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                Retry.RetryConnectionOpen(connection);

                const string sql =
                    "INSERT INTO `damage_reports`( `boat_id`, `description`, `member_id`) VALUES (@boat_id, @description, @member_id)";


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
            var damageReports = new List<Damage>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                Retry.RetryConnectionOpen(connection);

                const string sql = "SELECT d.*, m.*, b.* " +
                                   "FROM `damage_reports` AS d " +
                                   "INNER JOIN `members` AS m ON d.member_id = m.member_id " +
                                   "INNER JOIN `boats` AS b ON d.boat_id = b.id " +
                                   "ORDER BY d.`fixed`, d.`report_time` DESC";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string infix = string.Empty;
                                if(!reader.IsDBNull(reader.GetOrdinal("infix")))
                                {
                                    infix = reader.GetString("infix");
                                }
                            var member = new Member(reader.GetInt32("member_id"), reader.GetString("first_name"),
                                infix, reader.GetString("last_name"),
                                reader.GetString("email"), reader.GetInt32(11));
                            var boat = new Boat(reader.GetInt32("boat_id"), reader.GetBoolean("captain_seat_available"), reader.GetInt32("seats"),
                                reader.GetInt32(17), reader.GetString(18), reader.GetString("name"));
                            var id = reader.GetInt32(0);
                            var description = reader.GetString(2);
                            var fixedboat = reader.GetBoolean("fixed");
                            var usable = reader.GetBoolean("usable");
                            var reporttime = reader.GetDateTime("report_time");
                            //add report
                            damageReports.Add(new Damage(id, member, boat, description, fixedboat, usable, reporttime));
                        }
                    }
                }
            }

            return damageReports.OrderBy(damageReport => damageReport.BoatFixed)
                .ThenByDescending(damageReport => damageReport.ReportTime).ToList();
        }

        public List<Damage> GetRelatedToUser(Member member)
        {
            var damageReports = new List<Damage>();
            BoatRepository boatRepository = new BoatRepository();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                Retry.RetryConnectionOpen(connection);

                const string sql =
                    "SELECT d.*, b.* " +
                    "FROM `damage_reports` AS d " +
                    "INNER JOIN `boats` AS b ON d.boat_id = b.id " +
                    "WHERE d.`member_id` = @memberId " +
                    "ORDER BY `fixed`, `report_time` DESC";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@memberId", MySqlDbType.Int32);
                    command.Parameters["@memberId"].Value = member.Id;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //get member
                            var boat = new Boat(reader.GetInt32("boat_id"),
                                reader.GetBoolean("captain_seat_available"), reader.GetInt32("seats"),
                                reader.GetInt32("level"), reader.GetString(11),
                                reader.GetString("name"));
                            var id = reader.GetInt32("id");
                            var description = reader.GetString(2);
                            var fixedboat = reader.GetBoolean("fixed");
                            var usable = reader.GetBoolean("usable");
                            var reporttime = reader.GetDateTime("report_time");
                            //get boat

                            Damage damageReport = new Damage(id, member, boat, description, fixedboat, usable,
                                reporttime);
                            damageReports.Add(damageReport);
                        }
                    }
                }

                return damageReports.OrderBy(damageReport => damageReport.BoatFixed)
                    .ThenByDescending(damageReport => damageReport.ReportTime).ToList();
            }
        }

        public Damage Update(int id, bool boatFixed, bool usable, string description)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                Retry.RetryConnectionOpen(connection);

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
                    return GetById(id);
                }
            }

            return null;
        }

        public Damage GetById(int id)
        {
            MemberRepository memberRepository = new MemberRepository();
            BoatRepository boatRepository = new BoatRepository();

            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                Retry.RetryConnectionOpen(connection);

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
                            ImageRepository imageRepository = new ImageRepository();
                            Member member = memberRepository.GetById(reader.GetInt32("member_id"));
                            //get boat
                            Boat boat = boatRepository.GetBoatById(reader.GetInt32("boat_id"));
                            return new Damage(reader.GetInt32("id"), member, boat,
                                reader.GetString("description"),
                                reader.GetBoolean("fixed"), reader.GetBoolean("usable"),
                                imageRepository.Get(reader.GetInt32("id")));
                        }
                    }
                }
            }

            return null;
        }
    }
}