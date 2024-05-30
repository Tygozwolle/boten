namespace RoeiVerenigingLibrary.Exceptions;

public class EventTimeException : Exception
{
    public override string Message => "De datum van het event moet meer dan 2 weken in de toekomst liggen!";
}