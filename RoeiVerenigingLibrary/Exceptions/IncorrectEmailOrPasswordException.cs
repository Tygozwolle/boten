namespace RoeiVerenigingLibrary.Exceptions;

public class IncorrectEmailOrPasswordException : Exception
{
    public override string Message => "De combinatie van het email adres en wachtwoord bestaat niet!";
}