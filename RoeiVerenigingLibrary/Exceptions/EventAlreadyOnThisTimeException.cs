namespace RoeiVerenigingLibrary.Exceptions
{
    public class EventAlreadyOnThisTimeException : Exception
    {
        public override string Message => "Er is al een evenement op dit tijdstip!";
    }
}