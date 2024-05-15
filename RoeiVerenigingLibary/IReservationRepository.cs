namespace RoeiVerenigingLibary;

public interface IReservationRepository
{
    public Reservation CreateReservation(Member member, int boatId, DateTime startTime, DateTime endTime);
    public List<Reservation> GetReservations();

    public List<Reservation> GetReservations(Member member);
    public int GetAmountOfBoatsCurrRenting(int ID);
}