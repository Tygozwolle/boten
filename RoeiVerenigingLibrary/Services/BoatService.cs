using RoeiVerenigingLibrary.Exceptions;

namespace RoeiVerenigingLibrary
{
    public class BoatService(IBoatRepository repository)
    {
        public List<Boat>? GetBoats()
        {
            return repository.GetBoats();
        }

        public Boat GetBoatById(int id)
        {
            return repository.GetBoatById(id);
        }

        public Boat Create(Member loggedInMember, string name, string description, int seats, bool captainSeat,
            int level)
        {
            if (loggedInMember.Roles.Contains("beheerder") || loggedInMember.Roles.Contains("materiaal_commissaris"))
            {
                if (name.Length >= 1)
                {
                    if (!(level >= 1 && level <= 10))
                    {
                        throw new IncorrectLevelException();
                    }
                    else
                    {
                        return repository.Create(name, description, seats, captainSeat, level);
                    }
                }
                else
                {
                    throw new NameEmptyExeception();
                }
            }
            else
            {
                throw new IncorrectRightsException();
            }
        }

        public Boat Update(Member loggedInMember, Boat boat, string name, string description, int seats,
            bool captainSeat, int level)
        {
            if (loggedInMember.Roles.Contains("beheerder") || loggedInMember.Roles.Contains("materiaal_commissaris"))
            {
                if (name.Length >= 1)
                {
                    if (!(level >= 1 && level <= 10))
                    {
                        throw new IncorrectLevelException();
                    }
                    else
                    {
                        return repository.Update(boat, name, description, seats, captainSeat, level);
                    }
                }
                else
                {
                    throw new NameEmptyExeception();
                }
            }
            else
            {
                throw new IncorrectRightsException();
            }
        }

        public void AddImage(Member loggedInMember, Boat boat, Stream stream)
        {
            if (loggedInMember.Roles.Contains("beheerder") || loggedInMember.Roles.Contains("materiaal_commissaris"))
            {
                repository.AddImage(boat, stream);
            }
            else
            {
                throw new IncorrectRightsException();
            }
        }

        public List<Stream> getImageByReservation(List<Reservation> reservations)
        {
            List<Stream> images = new List<Stream>();
            foreach (var reservation in reservations)
            {
                images.Add(reservation.Boat.Image);
            }

            return images;
        }

        public void GetImageBoat(Boat boat)
        {
            boat.Image = repository.GetImage(boat);
        }

        public void GetImageBoats(List<Boat> boats)
        {
            List<Task> tasks = new List<Task>(boats.Count);
            foreach (Boat boat in boats)
            {
                Task task = new Task(() =>
                {
                    Boat save = boat;
                    save.Image = repository.GetImage(save);
                });
                task.Start();
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());
        }

        public void Delete(Member loggedInMember, Boat boat)
        {
            if (loggedInMember.Roles.Contains("beheerder") || loggedInMember.Roles.Contains("materiaal_commissaris"))
            {
                repository.Delete(boat);
            }
            else
            {
                throw new IncorrectRightsException();
            }
        }

        public void UpdateImage(Member loggedInMember, Boat boat, Stream stream)
        {
            if (loggedInMember.Roles.Contains("beheerder") || loggedInMember.Roles.Contains("materiaal_commissaris"))
            {
                if (repository.GetImage(boat) != null)
                {
                    repository.UpdateImage(boat, stream);
                }
                else
                {
                    repository.AddImage(boat, stream);
                }
            }
            else
            {
                throw new IncorrectRightsException();
            }
        }

        public List<Boat> GetMostANdLeastPopulairBoat(List<Reservation> reservations)
        {
            Dictionary<Boat, int> boatDictionary = new Dictionary<Boat, int>();

            foreach (Reservation res in reservations)
            {
                if (boatDictionary.ContainsKey(res.Boat))
                {
                    // Increment the count for the existing boat
                    boatDictionary[res.Boat]++;
                }
                else
                {
                    // Add the boat to the dictionary with an initial count of 1
                    boatDictionary[res.Boat] = 1;
                }
            }

            // Find the boat with the highest count
            Boat mostPopularBoat = null;
            Boat leastPopularBoat = null;
            int maxCount = 0;
            int minCount = 100;

            foreach (var entry in boatDictionary)
            {
                if (entry.Value > maxCount)
                {
                    mostPopularBoat = entry.Key;
                    maxCount = entry.Value;
                } else if (entry.Value < minCount)
                {
                    leastPopularBoat = entry.Key;
                    minCount = entry.Value;
                }
            }

            return new List<Boat>() { leastPopularBoat, mostPopularBoat };
        }

        public Dictionary<Boat, int> GetTopFiveBoats(List<Reservation> reservations)
        {
            Dictionary<Boat, int > boatDictionary = new Dictionary<Boat, int>();

            foreach (Reservation res in reservations)
            {
                if (boatDictionary.ContainsKey(res.Boat))
                {
                    boatDictionary[res.Boat]++;
                }
                else
                {
                    boatDictionary[res.Boat] = 1;
                }
            }
            return boatDictionary;
        }

        public Dictionary<String, int> BoatsPerLevel()
        {
            Dictionary<string, int> sortedBoats = new Dictionary<string, int>();
            sortedBoats.Add("Level 1", 0);
            sortedBoats.Add("Level 2", 0);
            sortedBoats.Add("Level 3", 0);
            sortedBoats.Add("Level 4", 0);
            sortedBoats.Add("Level 5", 0);
            
            var allBoats = GetBoats();
            foreach (var boat in allBoats)
            {
                if (boat.Level == 1)
                {
                    sortedBoats["Level 1"]++;
                }else if (boat.Level == 2)
                {
                    sortedBoats["Level 2"]++;
                }else if (boat.Level == 3)
                {
                    sortedBoats["Level 3"]++;
                }else if (boat.Level == 4)
                {
                    sortedBoats["Level 4"]++;
                }else if (boat.Level == 5)
                {
                    sortedBoats["Level 5"]++;
                }
            }
            return sortedBoats;
        }
        
    }
}