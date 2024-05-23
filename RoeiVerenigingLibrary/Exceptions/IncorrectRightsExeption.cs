namespace RoeiVerenigingLibary.Exceptions;

public class IncorrectRightsExeption : Exception
{
    public override string Message => "U heeft geen rechten om deze actie uit te voeren.";
}