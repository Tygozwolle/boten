namespace RoeiVerenigingLibrary.Exceptions
{
    public class ExceededAmountOfReservationsException : Exception
    {
        public ExceededAmountOfReservationsException(string message) : base(message)
        {
        }
    }
}