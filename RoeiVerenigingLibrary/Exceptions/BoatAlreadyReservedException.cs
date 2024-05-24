namespace RoeiVerenigingLibary.Exceptions
{
    public class BoatAlreadyReservedException : Exception
    {
        public override string Message => "De boot is al gereserveerd op dit tijdstip";
    }
}