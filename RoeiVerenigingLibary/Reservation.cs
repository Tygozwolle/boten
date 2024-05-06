namespace RoeiVerenigingLibary;

public class Reservation
{
    public Member Member { get; set; }
    public DateTime CreationDate { get; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int BoatID { get; set; }
    public int Id {get;}

    public Reservation(Member member, DateTime CreationDate, DateTime StartTime, DateTime EndTime, int BoatID, int ID)
    {
        this.Member = member;
        this.CreationDate = CreationDate;
        this.StartTime = StartTime;
        this.EndTime = EndTime;
        this.BoatID = BoatID;
        this.Id = ID;
    }

}