using JerEntryWebApp.Data;
using JrEntryWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JrEntryWebApi.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }


        public async Task AddNewUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            // EF is already tracking the user (fetched from DB), so just save
            await _context.SaveChangesAsync();
        }




        public async Task<List<User>> GetAllUser()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> FindByUserName(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        /// <summary>
        /// Finds a user by their username and includes all associated journal entries.
        /// </summary>
        /// <param name="userName">The username of the user to find.</param>
        /// <returns>
        /// The <see cref="User"/> object with its associated <see cref="JournalEntry"/> list, 
        /// or null if no user with the given username is found.
        /// </returns>
        public async Task<User?> FindByUserName(string userName)
        {
            return await _context.Users
                // JournalEntries navigation property ko eager loading ke zariye fetch karte hain
                // Taaki user ke saath uski saari journal entries bhi mil jaayein
                .Include(u => u.JournalEntries)

                // userName ke base par first matching user ko laate hain
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

    }
}
