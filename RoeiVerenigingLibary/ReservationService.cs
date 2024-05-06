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

    public void Create(Reservation reservation)
    {
        //TODO: check if boat is available
        //TODO: check if duration is less than 2 hours
        _reservationRepository.CreateReservation(reservation);
    }

    public List<Reservation> GetReservations()
    {
        return _reservationRepository.GetReservations();
    }
}