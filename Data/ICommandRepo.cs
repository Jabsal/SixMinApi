namespace SixMinApi.Data
{
    using Microsoft.EntityFrameworkCore;
    using SixMinApi.Models;

    public interface ICommandRepo
    {
        Task SaveChanges();
        Task<Command?> GetCommandByIdAsync(int id);

        Task<IEnumerable<Command>> GetCommandsAsync();

        Task CreateCommand(Command cmd);

        void DeleteCommand(Command cmd);
    }
}
