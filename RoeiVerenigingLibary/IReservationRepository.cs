namespace RoeiVerenigingLibary;

public interface IReservationRepository
{
    public Reservation CreateReservation(Member member, int boatId, DateTime startTime, DateTime endTime);
    public Reservation checkReservations(Member member, int boatId);
    public List<Reservation> GetReservations();
}