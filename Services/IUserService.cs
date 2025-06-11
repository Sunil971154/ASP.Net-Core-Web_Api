using JrEntryWebApi.Models;

namespace JrEntryWebApi.Services
{
    public interface IUserService
    {

        Task AddNewUser(User user);                            // Create/Update
        Task UpdateUser(User user);                            // Create/Update

        Task<List<User>> GetAllUser();                             // Read All
        Task<User?> FindByUserName(Guid id);                       // Read by ID
        Task DeleteByIdAsync(Guid id);                            // Delete
        Task<User?> FindByUserName(string userName);         // Read by Username
        



    }
}
