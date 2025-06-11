using JerEntryWebApp.Data;
using JrEntryWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JrEntryWebApi.Services
{
    public class JournalEntryService : IJEService
    {


        private readonly IUserService _iUserService;
        private readonly AppDbContext _context;

        public JournalEntryService(AppDbContext context, IUserService userService)
        {
            _context = context;
            _iUserService = userService;
        }


        public async Task<JournalEntry> SaveEntry(JournalEntry entry)
        {
            try
            {
                _context.JournalEntries.Add(entry);
                await _context.SaveChangesAsync();
                return entry;
            }
            catch (Exception ex)
            {
                Console.WriteLine("⚠️ Error in SaveEntry: " + ex.Message);
                Console.WriteLine("UserId: " + entry.UserId);
                Console.WriteLine("Title: " + entry.Title);
                Console.WriteLine("User Is Null: " + (entry.User == null));
                throw;
            }
        }


        public async Task<JournalEntry> SaveEntryWithUser(JournalEntry journalEntry, string username)
        {
            var user = await _iUserService.FindByUserName(username);
            journalEntry.LocalDateTime = DateTime.Now;
            journalEntry.UserId = (int) user.Id;   // ✅ Required
            journalEntry.User = null;

            var saved = await SaveEntry(journalEntry);

            user.JournalEntries ??= new List<JournalEntry>();
            user.JournalEntries.Add(saved);

            await _iUserService.UpdateUser(user);

            return saved;
        }
        public async Task<List<JournalEntry>> FindAll()
        {
            return await _context.JournalEntries.ToListAsync();
        }

        public async Task<JournalEntry> FindByIdAsync(string id)
        {
            if (!long.TryParse(id, out var entryId))
                return null;

            return await _context.JournalEntries.FindAsync(entryId);
        }

        public async Task DeleteByIdAsync(string id)
        {
            if (!long.TryParse(id, out var entryId))
                return;

            var entry = await _context.JournalEntries.FindAsync(entryId);
            if (entry != null)
            {
                _context.JournalEntries.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }
    }
}
