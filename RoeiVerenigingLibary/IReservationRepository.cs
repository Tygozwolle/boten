namespace RoeiVerenigingLibary;

public interface IReservationRepository
{
    public ReservationCreator Create(int BoatId, int UserId, string StartTime, string EndTime);
}