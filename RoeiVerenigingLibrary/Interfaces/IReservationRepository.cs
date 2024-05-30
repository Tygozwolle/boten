#region

using RoeiVerenigingLibrary.Model;

#endregion

namespace RoeiVerenigingLibrary.Interfaces;

public interface IReservationRepository
{
    public Reservation CreateReservation(Member member, int boatId, DateTime startTime, DateTime endTime);
    public List<Reservation> GetReservations();
    public List<Reservation> GetReservations(Member member);
    public int GetAmountOfBoatsCurrRenting(int id);
    public Reservation ChangeReservation(int reservationId, Member member, int boatId, DateTime startTime, DateTime endTime);
    public Reservation GetReservation(int reservationid);
    /**
     * returns true if the boat is already reserved
     */
    public bool BoatAlreadyReserved(int boatId, DateTime startTime, DateTime endTime);
}