namespace RoeiVerenigingLibary;

public class Reservation
{
    public Member Member { get; set; }
    public DateTime CreationDate { get; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int BoatId { get; set; }
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

    public Reservation(Member member, int boatId, DateTime startTime, DateTime endTime)
    {
        Member = member;
        StartTime = startTime;
        EndTime = endTime;
        BoatId = boatId;
    }

    public Reservation(int memberId, int boatId, DateTime startTime, DateTime endtime)
    {
        Id = memberId;
        BoatID = boatId;
        StartTime = startTime;
        EndTime = endtime;
    }
}