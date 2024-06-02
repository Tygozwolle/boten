using MySqlConnector;

namespace RoeiVerenigingLibrary
{
    public class Retry
    {
        public static T RetryMethod<T>(Func<T> method, int maxRetries = 3, int delay = 500)
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

                if (delay != default)
                {
                    System.Threading.Thread.Sleep(delay);
                }
            }

            return default;
        }
    }


}