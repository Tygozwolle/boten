using MySqlConnector;
using RoeiVerenigingLibary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Aspose.Email.Mime;
using Microsoft.VisualBasic;

namespace DataAccessLibary
{
    public class ImageRepository : IImageRepository
    {
        private void Add(int id, Stream image, MySqlConnection connection, MySqlTransaction transaction)
        {
            const String sql =
                $"INSERT INTO `damage_report_fotos`(`damage_report_id`, `image`) VALUES (@reportID,@image)";
            //   $"INSERT INTO `members`( `first_name`,`infix`, `last_name`, `email`, `password`) VALUES (@firstName,@infix,@lastName,@email,@passwordHash)";

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

        public void Add(int id, Stream image)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        Add(id, image, connection, transaction);
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

        public void Add(int id, List<Stream> images)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {




                    try
                    {
                        //  connection.BeginTransaction();


                        const String sqlAutocommit = "SET autocommit=0";
                        using (MySqlCommand command = new MySqlCommand(sqlAutocommit, connection))
                        {
                            command.Transaction = transaction;
                            command.ExecuteNonQuery();
                        }

                        foreach (var image in images)
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

        public List<Stream> get(int id)
        {


            var list = new List<Stream>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();
                const String sql = $"SELECT * FROM damage_report_fotos WHERE damage_report_id = @id";

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

        //var i = new BitmapImage();
        //i.BeginInit();
        //i.StreamSource = ImageRepository.get(1)[0];
        //i.CacheOption = BitmapCacheOption.OnLoad;
        //i.EndInit();
    }

}
