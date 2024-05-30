namespace RoeiVerenigingLibrary.Model;

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

    public string Description { get; set; }

    public string DescriptionNoEnter => Description;

    public string CaptainSeatToString()
    {
        if (CaptainSeat)
            return "Ja";
        return "Nee";
    }
}