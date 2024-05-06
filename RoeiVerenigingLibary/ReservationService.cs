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

    public void Create(Member loggedInMember, int boatId, DateTime startTime, DateTime endTime)
    {
        //todo: check if boat is available
        _reservationRepository.Create(boatId, loggedInMember.Id, startTime, endTime);
    }
    
    public List<Reservation> GetReservations()
    {
        return _reservationRepository.GetReservations();
    }
}