namespace RoeiVerenigingLibrary.Exceptions;

public class InvalidEmailException : Exception
{
    public override string Message => "Het email adres dat je hebt opgegevens bestaat niet!";
}