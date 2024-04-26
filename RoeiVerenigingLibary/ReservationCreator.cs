using RoeiVerenigingLibary.Exceptions;
namespace RoeiVerenigingLibary;

public class ReservationCreator
{
    private int UserId { get; set; }
    private int BoatId { get; set; }
    public string? Email { get; set; }
    private DateTime? StartTime { get; set; }
    private DateTime? EndTime { get; set; }
    private IReservationRepository _reservationRepository;
    

    public ReservationCreator(Member member, int boat, IReservationRepository reservationRepository)
    {
        this.Email = member.Email;
        this.UserId = member.Id;
        this.BoatId = boat;
        _reservationRepository = reservationRepository;
    }
    
    public bool TimeChecker(DateTime? start, DateTime? end)
    {
        if (start < end)
        {
            this.StartTime = start;
            this.EndTime = end;
            return true;
        }
        else
        {
            throw new InvalidTimeException("not valid time, start time should be earlier than end time. ");
        }
    }

    public void CommitToDb()
    {
        _reservationRepository.Create(this.BoatId, this.UserId, this.StartTime, this.EndTime);
    }
}