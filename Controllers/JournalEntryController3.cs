using JerEntryWebApp.Data;
using JrEntryWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq; // Ensure this namespace is included for LINQ methods  

namespace JerEntryWebApp.Controllers
{
    //[ApiController]
    //[Route("journal")]
    [NonController]
    public class JournalEntryController3 : ControllerBase
    {
        private readonly AppDbContext _context;

        public JournalEntryController3(AppDbContext context)
        {
            _context = context;
        }

        /*
         * Yahaan Ok(), NotFound(), BadRequest()... ye sab methods internally ObjectResult ya ActionResult<T> return karte hain, 
         * jo ResponseEntity ke barabar ka kaam karte hain.
         * Any() ek LINQ method hai jo check karta hai ki kisi collection me koi element hai ya nahi.
         * IEnumerable<T> ek interface hai .NET mein, jo batata hai ki koi object collection of items hai jise iterate (loop) kiya ja sakta hai, jaise foreach loop se.
         */

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JournalEntry>>> GetSimpleAll()
        {
            var allJe = await _context.JournalEntries.ToListAsync();
            if (allJe != null && allJe.Any())
            {
                // IEnumerable<JournalEntry> is returned,
                // Yeh return type IEnumerable<JournalEntry> kyunki controller sirf data bhej raha hai, modify nahi kar raha.
                return Ok(allJe);       /*Read-only output → IEnumerable<T>
                                           Data modification or in-memory operations → List<T>
                                          */
            }
            return NotFound();
        }

        /*  POST: api/JournalEntries
             Step 1: Accepts a JournalEntry object from the request body.
             Step 2: Checks if the received object is null and returns 400 Bad Request if it is.
             Step 3: Adds the journal entry to the database context.
             Step 4: Saves the changes to the database asynchronously.
             Step 5: Returns a 200 OK response upon successful creation.
         
         */
        [HttpPost]
        public async Task<ActionResult> CreateEntry([FromBody] JournalEntry journalEntry)
        {
            if (journalEntry == null)
                return BadRequest("Journal entry cannot be null.");

            _context.JournalEntries.Add(journalEntry);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /*GET: api/JournalEntries/id/{id}
         Step 1: Accepts an ID from the URL route.
         Step 2: Searches the database asynchronously for a JournalEntry with the given ID.
         Step 3: If no entry is found, returns 404 Not Found.
         Step 4: If found, returns 200 OK with the JournalEntry data. 
        */
        [HttpGet("id/{id}")]
        public async Task<ActionResult<JournalEntry>> GetById(long id)
        {
            var entry = await _context.JournalEntries.FindAsync(id);
            if (entry == null)
                return NotFound();

            return Ok(entry);
        }


        /*
           PUT: api/JournalEntries/id/{id}
           Step 1: Accepts an ID from the URL and a JournalEntry object from the request body.
           Step 2: Looks up the existing journal entry in the database by ID.
           Step 3: If the entry is not found, returns 404 Not Found.
           Step 4: If the entry is found, updates only non-null and non-empty fields (Title and Content).
           Step 5: Saves the changes to the database asynchronously.
           Step 6: Returns 200 OK with the updated JournalEntry object
        */
        [HttpPut("id/{id}")]
        public async Task<ActionResult> Update(long id, [FromBody] JournalEntry entry)
        {
            var existing = await _context.JournalEntries.FindAsync(id);
            if (existing == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(entry.Title))
                existing.Title = entry.Title;

            if (!string.IsNullOrWhiteSpace(entry.Content))
                existing.Content = entry.Content;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }


        /*
           DELETE: api/JournalEntries/id/{id}
           Step 1: Accepts an ID from the URL route.
           Step 2: Looks up the JournalEntry in the database using the given ID.
           Step 3: If the entry is not found, returns 404 Not Found.
           Step 4: If found, removes the entry from the database context.
           Step 5: Saves the changes to the database asynchronously.
           Step 6: Returns 204 No Content to indicate successful deletion with no response body.
         
         */

        [HttpDelete("id/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var entry = await _context.JournalEntries.FindAsync(id);
            if (entry == null)
                return NotFound();

            _context.JournalEntries.Remove(entry);
            await _context.SaveChangesAsync();

            return NoContent(); // 🔁 returns 204 No Content
        }

    }
}
