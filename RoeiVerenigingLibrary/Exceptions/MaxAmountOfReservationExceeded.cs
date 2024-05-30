namespace RoeiVerenigingLibrary.Exceptions;

public class MaxAmountOfReservationExceeded : Exception
{
    public override string Message => "Er zijn al twee of meer reserveringen voor dit lid!";
}