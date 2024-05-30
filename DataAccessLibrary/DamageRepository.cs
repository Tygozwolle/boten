﻿using MySqlConnector;
using RoeiVerenigingLibrary.Interfaces;
using RoeiVerenigingLibrary.Model;

namespace DataAccessLibrary;

public class DamageRepository : IDamageRepository
{
    public Damage Create(Member member, Boat boat, string description)
    {
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();

            const string sql =
                "INSERT INTO `damage_reports`( `boat_id`, `description`, `member_id`) VALUES (@boat_id, @description, @member_id)";


            using (var command = new MySqlCommand(sql, connection))
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
        var memberRepository = new MemberRepository();
        var boatRepository = new BoatRepository();
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();

            const string sql =
                "SELECT * FROM `damage_reports` ORDER BY `fixed`, `report_time` DESC";

            using (var command = new MySqlCommand(sql, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    var tasks = new List<Task>();
                    while (reader.Read())
                    {
                        var memberid = reader.GetInt32("member_id");
                        var boatid = reader.GetInt32("boat_id");
                        var id = reader.GetInt32("id");
                        var description = reader.GetString("description");
                        var fixedboat = reader.GetBoolean("fixed");
                        var usable = reader.GetBoolean("usable");
                        var reporttime = reader.GetDateTime("report_time");
                        //get member
                        var task = new Task(() =>
                        {
                            var member = memberRepository.GetById(memberid);
                            //get boat
                            var boat = boatRepository.GetBoatById(boatid);
                            var damageReport = new Damage(id, member, boat, description, fixedboat, usable,
                                reporttime);
                            lock (damageReports)
                            {
                                damageReports.Add(damageReport);
                            }
                        });
                        task.Start();
                        tasks.Add(task);
                    }

                    Task.WaitAll(tasks.ToArray());
                }
            }
        }

        return damageReports.OrderBy(damageReport => damageReport.BoatFixed)
            .ThenByDescending(damageReport => damageReport.ReportTime).ToList();
    }

    public List<Damage> GetRelatedToUser(Member member)
    {
        var damageReports = new List<Damage>();
        var boatRepository = new BoatRepository();
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();

            const string sql =
                "SELECT * FROM `damage_reports` WHERE `member_id` = @memberId ORDER BY `fixed`, `report_time` DESC";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@memberId", MySqlDbType.Int32);
                command.Parameters["@memberId"].Value = member.Id;

                using (var reader = command.ExecuteReader())
                {
                    var tasks = new List<Task>();
                    while (reader.Read())
                    {
                        //get member
                        var boatid = reader.GetInt32("boat_id");
                        var id = reader.GetInt32("id");
                        var description = reader.GetString("description");
                        var fixedboat = reader.GetBoolean("fixed");
                        var usable = reader.GetBoolean("usable");
                        var reporttime = reader.GetDateTime("report_time");
                        //get boat
                        var task = new Task(() =>
                        {
                            //get boat
                            var boat = boatRepository.GetBoatById(boatid);
                            var damageReport = new Damage(id, member, boat, description, fixedboat, usable,
                                reporttime);
                            lock (damageReports)
                            {
                                damageReports.Add(damageReport);
                            }
                        });
                        task.Start();
                        tasks.Add(task);
                    }

                    Task.WaitAll(tasks.ToArray());
                }
            }
        }

        return damageReports.OrderBy(damageReport => damageReport.BoatFixed)
            .ThenByDescending(damageReport => damageReport.ReportTime).ToList();
    }

    public Damage Update(int id, bool boatFixed, bool usable, string description)
    {
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();

            const string sql =
                "UPDATE `damage_reports` SET `fixed` = @boatFixed, `usable` = @usable, `description` = @description WHERE `id` = @id";

            using (var command = new MySqlCommand(sql, connection))
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
        var memberRepository = new MemberRepository();
        var boatRepository = new BoatRepository();

        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();

            const string sql = "SELECT * FROM `damage_reports` WHERE `id` = @id";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32);
                command.Parameters["@id"].Value = id;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        //get member
                        var imageRepository = new ImageRepository();
                        var member = memberRepository.GetById(reader.GetInt32("member_id"));
                        //get boat
                        var boat = boatRepository.GetBoatById(reader.GetInt32("boat_id"));
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