namespace RoeiVerenigingLibrary.Exceptions;

public class ReservationInThePastException : Exception
{
    public override string Message => "Je kan geen reservering in het verleden plaatsen!";
}