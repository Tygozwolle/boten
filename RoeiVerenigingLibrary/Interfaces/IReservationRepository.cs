namespace RoeiVerenigingLibary;

public interface IReservationRepository
{
    public Reservation CreateReservation(Member member, int boatId, DateTime startTime, DateTime endTime);
    public List<Reservation> GetReservations();
    public List<Reservation> GetReservations(Member member);
    public int GetAmountOfBoatsCurrRenting(int ID);

    public Reservation ChangeReservation(int reservationdId, Member member, int boatId, DateTime startTime,
        DateTime endTime);

    public Reservation GetReservation(int reservationid);
    public bool BoatAlreadyReserved(int boatId, DateTime startTime, DateTime endTime);
}