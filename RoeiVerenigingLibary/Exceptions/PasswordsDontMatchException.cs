namespace RoeiVerenigingLibary.Exceptions;

public class PasswordsDontMatchException : Exception
{
    public override string Message => ("De wachtwoorden komen niet overeen!");
}