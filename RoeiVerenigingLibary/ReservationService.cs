using RoeiVerenigingLibary.Exceptions;

namespace RoeiVerenigingLibary;

public class ReservationService
{
    private IReservationRepository _reservationRepository;

    public ReservationService(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public bool TimeChecker(DateTime? start, DateTime? end)
    {
        var hours = (start.Value - end.Value).TotalHours;
        if (start > end)
        {
            throw new InvalidTimeException("Start time should be earlier than end time.");
        }else if(hours > 2)
        {
            throw new InvalidTimeException("A reservation cannot be made for more than 2 hours.");
        }else if (hours < 1)
        {
            throw new InvalidTimeException("A Reservation has to be longer than 1 hour.");
        }
        else
        {
            return true;
        }

      
    }

    public Reservation Create(Member member, int boatId, DateTime startTime, DateTime endTime)
    {
        //TODO: check if boat is available
        //TODO: check if duration is less than 2 hours
        return _reservationRepository.CreateReservation(member, boatId, startTime, endTime);
    }

    public List<Reservation> GetReservations()
    {
        return _reservationRepository.GetReservations();
    }
}