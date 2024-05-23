using MySqlConnector;
using RoeiVerenigingLibary;

namespace DataAccessLibary;

public class ImageRepository : IImageRepository
{
    public void Add(int id, Stream image)
    {
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
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
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    const string sqlAutocommit = "SET autocommit=0";
                    using (var command = new MySqlCommand(sqlAutocommit, connection))
                    {
                        command.Transaction = transaction;
                        command.ExecuteNonQuery();
                    }

                    foreach (var image in images) Add(id, image, connection, transaction);

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
        //var list = new List<Stream>();
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql = "SELECT * FROM damage_report_fotos WHERE damage_report_id = @id  LIMIT 1";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32);
                command.Parameters["@id"].Value = id;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) return reader.GetStream(2);
                }
            }
        }

        return null;
    }

    public List<Stream> getAsync(int id)
    {
        var list = new List<Stream>();
        var ids = new List<int>();
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql = "SELECT * FROM damage_report_fotos WHERE damage_report_id = @id";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32);
                command.Parameters["@id"].Value = id;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) ids.Add(reader.GetInt32("Id"));
                }
            }
        }

        var tasks = new List<Task>();


        foreach (var imageId in ids)
        {
            var task = new Task(() =>
            {
                var streamToAdd = GetImage(imageId);

                lock (list)
                {
                    list.Add(streamToAdd);
                }
            });
            task.Start();
            tasks.Add(task);
            //  task.Wait();
        }

        Task.WaitAll(tasks.ToArray());
        return list;
    }

    public List<Stream> get(int id)
    {
        var list = new List<Stream>();
        var ids = new List<int>();
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql = "SELECT * FROM damage_report_fotos WHERE damage_report_id = @id";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32);
                command.Parameters["@id"].Value = id;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) list.Add(reader.GetStream(2));
                }
            }
        }

        return list;
    }

    private void Add(int id, Stream image, MySqlConnection connection, MySqlTransaction transaction)
    {
        const string sql =
            "INSERT INTO `damage_report_fotos`(`damage_report_id`, `image`) VALUES (@reportID,@image)";

        using (var command = new MySqlCommand(sql, connection))
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
        //var list = new List<Stream>();
        using (var connection = new MySqlConnection(ConnectionString.GetString()))
        {
            connection.Open();
            const string sql = "SELECT * FROM damage_report_fotos WHERE damage_report_id = @id";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32);
                command.Parameters["@id"].Value = id;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) return reader.GetStream(2);
                }
            }
        }

        return null;
    }

    //var i = new BitmapImage();
    //i.BeginInit();
    //i.StreamSource = ImageRepository.get(1)[0];
    //i.CacheOption = BitmapCacheOption.OnLoad;
    //i.EndInit();
}