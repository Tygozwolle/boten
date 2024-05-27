namespace RoeiVerenigingLibrary.Exceptions
{
    public class IncorrectLevelException : Exception
    {
        public override string Message => "Het opgegeven level moet tussen 1 en 10 zijn!";
    }
}