namespace RoeiVerenigingLibrary.Exceptions
{
    public class IncorrectRightsException : Exception
    {
        public override string Message => "U heeft geen rechten om deze actie uit te voeren.";
    }
}