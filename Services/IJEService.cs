using JrEntryWebApi.Models;

namespace JrEntryWebApi.Services
{
    public interface IJEService
    {
        Task<JournalEntry> SaveEntry(JournalEntry entry );
        Task<JournalEntry> SaveEntryWithUser(JournalEntry entry , string userName);
        Task<List<JournalEntry>> FindAll();
        Task<JournalEntry> FindByIdAsync(string id);
        Task DeleteByIdAsync(string id);
    }
}

