namespace RoeiVerenigingLibary.Exceptions;

public class IncorrectPasswordException : Exception
{
    public override string Message => ("Het opgegeven wachtwoord is onjuist");
}