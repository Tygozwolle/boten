using Aspose.Email.Clients.Activity;
using System.Text;

namespace RoeiVerenigingLibary
{
    public class Boat
    {
        public int Id { get; set; }
        public Boolean CaptainSeat { get; set; }
        public int Seats { get; set; }

        public int Level { get; set; }
        private string _Description;
        public string Description
        {
            get
            {

                string input = _Description;
                if (input.Length > 60)
                {
                    string[] words = input.Split(' ');
                    StringBuilder sb = new StringBuilder();
                    int currLength = 0;
                    foreach (string word in words)
                    {
                        if (currLength + word.Length + 1 < 60) // +1 accounts for adding a space
                        {
                            sb.AppendFormat(" {0}", word);
                            currLength = (sb.Length % 60);
                        }
                        else
                        {
                            sb.AppendFormat("{0}{1}", Environment.NewLine, word);
                            currLength = 0;
                        }
                    }

                    return sb.ToString();

                }
                else
                {
                    return input;
                }

            }
            set
            {
                _Description = value;
            }
        }
        

        public Boat(int id, Boolean captainSeat, int Seats, int level, string description)
        {
            this.Id = id;
            CaptainSeat = captainSeat;
            this.Seats = Seats;
            Level = level;
            this.Description = description;
        }
        public Boat(int id, Boolean captainSeat, int Seats, int level)
        {
            this.Id = id;
            CaptainSeat = captainSeat;
            this.Seats = Seats;
            Level = level;
        }
    }
}
