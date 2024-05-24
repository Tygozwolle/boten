namespace RoeiVerenigingLibrary.Exceptions
{
    public class InvalidTimeException : Exception
    {
        public override string Message => "De starttijd moet eerder zijn dan de eindtijd!";
    }
}