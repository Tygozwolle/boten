namespace RoeiVerenigingLibary.Exceptions;

public class PasswordsDontMatchException : Exception
{
    public override string Message => ("De nieuw opgegeven wachtwoorden komen niet overeen");
}