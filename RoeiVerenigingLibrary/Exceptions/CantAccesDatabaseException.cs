namespace RoeiVerenigingLibary.Exceptions
{
    public class CantAccesDatabaseException : Exception
    {
        public override string Message => "Kan niet verbinden met de database";
    }
}