namespace RoeiVerenigingLibary.Exceptions
{
    public class ReservationNotInDaylightException : Exception
    {
        public override string Message => "De geselecteerde tijd moet tussen zonsopkomst en zonsondergang vallen.";
    }
}