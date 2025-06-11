using JerEntryWebApp.Data;
using JrEntryWebApi.Models;
using JrEntryWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace JerEntryWebApp.Controllers
{
    [ApiController]
    [Route("journal")]
    public class JournalEntryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUserService _iuserService;
        private readonly IJEService _iJerService;

        public JournalEntryController(AppDbContext context, IUserService userService, IJEService iJerService)
        {
            _context = context;
            _iuserService = userService;
            _iJerService = iJerService;
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetAllJournalEntriesOfUser(string userName)
        {
            var user = await _iuserService.FindByUserName(userName);
            if (user == null)
            {
                return NotFound(); // User not found
            }
            var journalEntries = user.JournalEntries; // Navigation property
            if (journalEntries != null && journalEntries.Any())
            {
                return Ok(journalEntries); // Return list of journal entries
            }
            return NotFound(); // No entries found
        }

        [HttpPost("{userName}")]
        public async Task<ActionResult<JournalEntry>> CreateEntryOfUser([FromBody] JournalEntry myEntry, [FromRoute] string userName)
        {
            try
            {
               
                await _iJerService.SaveEntryWithUser(myEntry, userName);
                return Created(string.Empty, myEntry); // 201 Created
            }
            catch (Exception ex)
            {
                // Optionally log the error
                return StatusCode(500, "An error occurred while creating the journal entry.");
            }
        }


        [HttpGet("id/{id}")]
        public async Task<ActionResult<JournalEntry>> GetById(int id)
        {
            var entry = await _context.JournalEntries.FindAsync(id);
            if (entry == null)
                return NotFound();

            return Ok(entry);
        }

        [HttpPut("id/{userName}/{myid}")]
        public async Task<IActionResult> UpdateJEById(string userName, int myid, [FromBody] JournalEntry updatedEntry)
        {
            var oldEntry = await _context.JournalEntries
                                         .FirstOrDefaultAsync(e => e.Id == myid && e.User.UserName == userName);

            if (oldEntry != null)
            {
                oldEntry.Title = !string.IsNullOrWhiteSpace(updatedEntry.Title)
                    ? updatedEntry.Title
                    : oldEntry.Title;

                oldEntry.Content = !string.IsNullOrWhiteSpace(updatedEntry.Content)
                    ? updatedEntry.Content
                    : oldEntry.Content;

                _context.JournalEntries.Update(oldEntry);
                await _context.SaveChangesAsync();

                return Ok(oldEntry); // 200 OK
            }

            return NotFound(); // 404
        }


        [HttpDelete("id/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entry = await _context.JournalEntries.FindAsync(id);
            if (entry == null)
                return NotFound();

            _context.JournalEntries.Remove(entry);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
