
using DataAccessLibary;
using MySqlConnector;


using RoeiVerenigingLibary;


namespace TestConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            MemberService memberService = new MemberService(new MemberRepository());
            Member tygo = memberService.Login("tygo@windesheim.be", "Test123$");
            Console.WriteLine(tygo.FirstName);
            Console.WriteLine(tygo.Roles.Count);
            using (MySqlConnection connection = new MySqlConnection(ConnectionString.GetString()))
            {
                connection.Open();
                String sql = $"SELECT * FROM member_roles WHERE member_id = 1";
                Console.WriteLine(sql);

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("{0} {1}",reader.GetInt32(0), reader.GetString(1));
                        }
                    }
                }
            }
        }
    }
}