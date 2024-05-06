namespace RoeiVerenigingLibary.Exceptions;

public class MemberAlreadyExistsException : Exception
{
    public override string Message => ("Dit email adress wordt al gebruikt");
}