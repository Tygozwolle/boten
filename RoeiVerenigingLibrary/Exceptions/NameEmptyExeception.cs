namespace RoeiVerenigingLibrary.Exceptions;

public class NameEmptyExeception : Exception

{
    public override string Message => "Dit naam mag niet leeg zijn!";
}