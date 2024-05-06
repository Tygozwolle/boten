namespace RoeiVerenigingLibary;

public interface IReservationRepository
{
    public void Create(int boatId, int userId, DateTime? startTime, DateTime? endTime);
}