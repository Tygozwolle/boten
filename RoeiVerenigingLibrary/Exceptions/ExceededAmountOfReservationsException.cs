namespace RoeiVerenigingLibrary.Exceptions
{
    public class ExceededAmountOfReservationsException : Exception
    {
        public override string Message => "Je mag maximaal 2 reserveringen hebben tegelijk!";
    }
}