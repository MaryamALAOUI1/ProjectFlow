namespace ProjectFlow.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"L'entit� '{name}' ({key}) est introuvable.")
        {
        }
    }
}
