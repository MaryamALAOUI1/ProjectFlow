namespace ProjectFlow.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"L'entité '{name}' ({key}) est introuvable.")
        {
        }
    }
}
