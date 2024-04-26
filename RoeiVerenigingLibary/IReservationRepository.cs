namespace RoeiVerenigingLibary;

public interface IReservationRepository
{
    public ReservationCreator Create(int boatId, int userId, DateTime? startTime, DateTime? endTime);
}