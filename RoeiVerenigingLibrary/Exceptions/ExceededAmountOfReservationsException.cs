namespace RoeiVerenigingLibary.Exceptions;

public class ExceededAmountOfReservationsException: Exception
{
    public ExceededAmountOfReservationsException(String message) : base(message)
    {
    }
}