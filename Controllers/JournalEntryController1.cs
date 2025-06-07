using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using JrEntryWebApi.Models;

namespace JerEntryWebApp.Controllers
{
    //[ApiController]
    //[Route("journal")]
    [NonController]
    public class JournalEntryController1 : ControllerBase
    {
        // Thread-safe dictionary to simulate in-memory data store
        private static ConcurrentDictionary<long, JournalEntry> journalEntries = new ConcurrentDictionary<long, JournalEntry>();



        // GET /journal
        [HttpGet]
        public ActionResult<IEnumerable<JournalEntry>> GetAll()
        {
           //return Ok(new List<JournalEntry>(journalEntries.Values));
            return Ok(journalEntries.Values);
        }

        // POST /journal
        [HttpPost]
        public ActionResult<bool> CreateEntry([FromBody] JournalEntry journalEntry)
        {
            journalEntries[journalEntry.Id] = journalEntry;
            return Ok(true);
        }

        // GET /journal/id/{myId}
        [HttpGet("id/{myId}")]
        public ActionResult<JournalEntry> GetJournalEntryById(long myId)
        {
            //TryGetValue एक method है जो Dictionary या ConcurrentDictionary में मौजूद key के लिए उसकी value को safely निकालने का तरीका है।
            if (journalEntries.TryGetValue(myId, out var entry))
            {
                return Ok(entry);
            }
            return NotFound();
        }

        // DELETE /journal/id/{myId}
        [HttpDelete("id/{myId}")]
        public ActionResult<JournalEntry> DeleteJournalEntry(long myId)
        {
            //TryRemove एक method है जो ConcurrentDictionary में दी गई key को safely हटाता है और उसकी value return करता है—अगर key न मिले तो quietly fail हो जाता है।
            if (journalEntries.TryRemove(myId, out var removedEntry))
            {
                return Ok(removedEntry);
            }
            return NotFound();
        }

        // PUT /journal/id/{id}
        [HttpPut("id/{id}")]
        public ActionResult<JournalEntry> UpdateJournalEntry(long id, [FromBody] JournalEntry myEntry)
        {
            //ContainsKey method है:Dictionary<TKey, TValue> और ConcurrentDictionary<TKey, TValue> का।
            if (!journalEntries.ContainsKey(id))
            {
                return NotFound();
            }
            journalEntries[id] = myEntry;
            return Ok(myEntry);
        }


    }
}
