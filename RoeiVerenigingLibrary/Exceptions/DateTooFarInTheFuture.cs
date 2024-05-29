namespace RoeiVerenigingLibrary.Exceptions;

public class DateTooFarInTheFuture : Exception
{
    public override string Message => "De geselecteerde datum mag niet meer dan 2 weken in de toekomst zijn!";
}