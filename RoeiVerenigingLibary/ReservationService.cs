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
        if (start < end)
        {
            return true;
        }

        throw new InvalidTimeException("not valid time, start time should be earlier than end time. ");
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