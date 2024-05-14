using RoeiVerenigingLibary.Exceptions;

namespace RoeiVerenigingLibary;

public class ReservationService
{
    private IReservationRepository _reservationRepository;
    private readonly TimeSpan maxReservationTime = new TimeSpan(2, 0, 0);

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

        throw new InvalidTimeException();
    }

    public Reservation Create(Member member, int boatId, DateTime startTime, DateTime endTime)
    {
        if (!member.Roles.Contains("beheerder"))
        {
            if (AmountOfBoatsCurrentlyRenting(member.Id) >= 2)
            {
                throw new MaxAmountOfReservationExceeded();
            }
            if (endTime - startTime >= maxReservationTime)
            {
                string message = "Je kan voor maximaal " + maxReservationTime.Hours + " uur reserveren!";
                throw new ArgumentOutOfRangeException("startTime", message);
            }
            // TODO bij daglicht
            // TODO bij niveau --> moet deze niet bij de klik op een boot?
        }
       
        return _reservationRepository.CreateReservation(member, boatId, startTime, endTime);
    }

    public List<Reservation> GetReservations()
    {
        return _reservationRepository.GetReservations();
    }

    public int AmountOfBoatsCurrentlyRenting(int ID)
    {
        return _reservationRepository.GetAmountOfBoatsCurrRenting(ID);
    }
}
