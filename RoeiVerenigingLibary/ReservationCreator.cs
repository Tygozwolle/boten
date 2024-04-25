using RoeiVerenigingLibary.Exceptions;
namespace RoeiVerenigingLibary;

public class ReservationCreator
{
    private int UserId { get; set; }
    private int BoatId { get; set; }
    public string? Email { get; set; }
    private string StartTime { get; set; }
    private string EndTime { get; set; }
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
        if (int.Parse(start.Value.ToString("HH")) < int.Parse(end.Value.ToString("HH")) || start.Value.Equals(end.Value))
        {
            this.StartTime = start.Value.ToString("HH:mm");
            this.EndTime = end.Value.ToString("HH:mm");
            return true;
        }
        else
        {
            throw new InvalidTimeException("not valid time, time should be ");
        }
    }

    public void CommitToDb()
    {
        _reservationRepository.Create(this.BoatId, this.UserId, this.StartTime, this.EndTime);
    }
}