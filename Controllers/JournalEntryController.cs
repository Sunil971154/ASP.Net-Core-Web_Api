using JerEntryWebApp.Data;
using JrEntryWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JerEntryWebApp.Controllers
{
    [ApiController]
    [Route("journal")]
    public class JournalEntryController : Controller
    {
        private readonly AppDbContext _context;

        public JournalEntryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<JournalEntry>>> GetAll()
        {
            return await _context.JournalEntries.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> CreateEntry([FromBody] JournalEntry journalEntry)
        {
            _context.JournalEntries.Add(journalEntry);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<JournalEntry>> GetById(long id)
        {
            var entry = await _context.JournalEntries.FindAsync(id);
            if (entry == null) return NotFound();
            return Ok(entry);
        }

        [HttpPut("id/{id}")]
        public async Task<ActionResult> Update(long id, [FromBody] JournalEntry entry)
        {
            var existing = await _context.JournalEntries.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Title = entry.Title;
            existing.Content = entry.Content;
            

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpDelete("id/{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var entry = await _context.JournalEntries.FindAsync(id);
            if (entry == null) return NotFound();

            _context.JournalEntries.Remove(entry);
            await _context.SaveChangesAsync();
            return Ok(entry);
        }

    }
}
