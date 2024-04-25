namespace RoeiVerenigingLibary;

public interface IReservationRepository
{
    public ReservationCreator Create(int boatId, int userId, string startTime, string endTime);
}