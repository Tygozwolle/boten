using MySqlConnector;

namespace RoeiVerenigingLibrary
{
    public class Retry
    {
        public static  T RetryMethod<T>(Func<T> method, int maxRetries = 3, int delay = 500)
        {
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    return method();
                }
                catch (MySqlException e)
                {
                    if (i == maxRetries - 1)
                    {
                        throw;
                    }
                }
                System.Threading.Thread.Sleep(delay);
            }

            return default;
        }

        public static async void RetryConnectionOpen(MySqlConnection connection, int maxRetries = 3, int delay = 500)
        {
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    connection.Open();
                    return;
                }
                catch (Exception e)
                {
                    if (i == maxRetries - 1)
                    {
                        throw;
                    }
                }
                await Task.Delay(delay);
                
            }
        }
    }

}
