namespace RoeiVerenigingLibary.Exceptions;

public class MemberAlreadyExistsException : Exception
{
    public override string Message => ("Er bestaat al een lid met dit email adres");

}