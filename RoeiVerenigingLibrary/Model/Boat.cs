﻿using System.Text;

namespace RoeiVerenigingLibrary
{
    public class Boat
    {
        public Boat(int id, bool captainSeat, int seats, int level, string description, string name, Stream image)
        {
            Id = id;
            CaptainSeat = captainSeat;
            Seats = seats;
            Level = level;
            Description = description;
            Name = name;
            Image = image;
        }

        public Boat(int id, bool captainSeat, int seats, int level, string description, string name)
        {
            Id = id;
            CaptainSeat = captainSeat;
            Seats = seats;
            Level = level;
            Description = description;
            Name = name;
        }

        public int Id { get; set; }
        public bool CaptainSeat { get; set; }
        public int Seats { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public Stream Image { get; set; }
        private string _Description;

        public string Description
        {
            get
            {
                //convert enters stored in database
                string input = _Description;
                if (input.Length > 60)
                {
                    string[] words = input.Split(' ');
                    StringBuilder sb = new StringBuilder();
                    int currLength = 0;
                    foreach (string word in words)
                    {
                        if (currLength + word.Length + 1 < 60)
                        {
                            sb.AppendFormat(" {0}", word);
                            currLength = sb.Length % 60;
                        }
                        else
                        {
                            sb.AppendFormat("{0}{1}", Environment.NewLine, word);
                            currLength = 0;
                        }
                    }

                    return sb.ToString();
                }

                return input;
            }
            set => _Description = value;
        }

        public String CaptainSeatToString()
        {
            if (CaptainSeat)
            {
                return "Ja";
            }
            else
            {
                return "Nee";
            }
        }
    }
}