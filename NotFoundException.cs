namespace ProjectFlow.Application.Projects
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"L'entit� '{name}' ({key}) est introuvable.")
        {
        }
    }
}
using ProjectFlow.Application.Projects;
