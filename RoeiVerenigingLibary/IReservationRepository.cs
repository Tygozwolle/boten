namespace RoeiVerenigingLibary;

public interface IReservationRepository
{
    public void CreateReservation(Reservation reservation);
    public List<Reservation> GetReservations();
}