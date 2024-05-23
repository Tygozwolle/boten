namespace RoeiVerenigingLibary;

public class Reservation
{
    public Member Member { get; set; }
    public int MemberId { get; set; }
    public DateTime CreationDate { get; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int BoatId { get; set; }
    public Boat Boat { get; set; }
    public int Id { get; }

    public Reservation(Member member, DateTime creationDate, DateTime startTime, DateTime endTime, int boatId, int id)
    {
        Member = member;
        CreationDate = creationDate;
        StartTime = startTime;
        EndTime = endTime;
        BoatId = boatId;
        Id = id;
    }
    
    public Reservation(Member member, DateTime creationDate, DateTime startTime, DateTime endTime, Boat boat, int id)
    {
        Member = member;
        CreationDate = creationDate;
        StartTime = startTime;
        EndTime = endTime;
        Boat = boat;
        Id = id;
    }

    public Reservation(Member member, int boatId, DateTime startTime, DateTime endTime)
    {
        Member = member;
        StartTime = startTime;
        EndTime = endTime;
        BoatId = boatId;
    }

    public Reservation(int memberId, int boatId, DateTime startTime, DateTime endTime)
    {
        MemberId = memberId;
        StartTime = startTime;
        EndTime = endTime;
        BoatId = boatId;
    }

    public void AddBoatObject(int boatId)
    {
        
    }
}