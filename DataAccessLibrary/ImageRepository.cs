﻿using MySqlConnector;
using RoeiVerenigingLibrary;

namespace DataAccessLibrary
{
    public class ImageRepository : IImageRepository
    {
        public void Add(int id, List<Stream> images)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                Retry.RetryConnectionOpen(connection);
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        const string sqlAutocommit = "SET autocommit=0";
                        using (MySqlCommand command = new MySqlCommand(sqlAutocommit, connection))
                        {
                            command.Transaction = transaction;
                            command.ExecuteNonQuery();
                        }

                        foreach (Stream image in images)
                        {
                            Add(id, image, connection, transaction);
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public Stream GetFirstImage(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                Retry.RetryConnectionOpen(connection);
                const string sql = "SELECT * FROM damage_report_fotos WHERE damage_report_id = @id  LIMIT 1";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32);
                    command.Parameters["@id"].Value = id;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return reader.GetStream(2);
                        }
                    }
                }
            }

            return null;
        }

        public List<Stream> Get(int id)
        {
            var list = new List<Stream>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                Retry.RetryConnectionOpen(connection);
                const string sql = "SELECT * FROM damage_report_fotos WHERE damage_report_id = @id";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32);
                    command.Parameters["@id"].Value = id;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetStream(2));
                        }
                    }
                }
            }

            return list;
        }

        private void Add(int id, Stream image, MySqlConnection connection, MySqlTransaction transaction)
        {
            const string sql =
                "INSERT INTO `damage_report_fotos`(`damage_report_id`, `image`) VALUES (@reportID,@image)";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@reportID", MySqlDbType.Int32);
                command.Parameters["@reportID"].Value = id;

                command.Parameters.Add("@image", MySqlDbType.Blob);
                command.Parameters["@image"].Value = image;
                command.Transaction = transaction;
                command.ExecuteNonQuery();
            }
        }

        private Stream GetImage(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                Retry.RetryConnectionOpen(connection);
                const string sql = "SELECT * FROM damage_report_fotos WHERE damage_report_id = @id";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32);
                    command.Parameters["@id"].Value = id;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return reader.GetStream(2);
                        }
                    }
                }
            }

            return null;
        }
    }
}