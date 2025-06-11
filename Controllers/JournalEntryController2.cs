using JerEntryWebApp.Data;
using JrEntryWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JerEntryWebApp.Controllers
{
    //[ApiController]    // यह attribute बताता है कि यह class एक API controller है।  
   // [Route("journal")] // यह route prefix है, जिसका मतलब है कि इस controller के सभी action methods की URL में "journal" शामिल होगा।
    [NonController]
    public class JournalEntryController2 : ControllerBase /* Object → ControllerBase( support nahi karta- Views, Razor pages, or UI rendering.)
                                                          * ControllerBase→Controller(support karta hai-Views, Razor pages, or UI rendering.) -> ApiController
                                                          */
    {
        //इस readonly variable को constructor के बाहर assign नहीं किया जा सकता, सिर्फ constructor में (या declaration में) एक बार assign किया जा सकता है।
        private readonly AppDbContext _context;

        /*  
         * COnstrector injetion 
         * Dependency Injection के ज़रिए AppDbContext instance को _context में assign किया गया है , यहाँ AppDbContext एक Entity Framework Core का database context है
         * जिससे database से data access किया जा सके controller के methods में  
         */
        public JournalEntryController2(AppDbContext context)
        {
            
            _context = context;
        }

        /*
         * जब आपको कोई long-running काम (जैसे database call, file read, web API call) करना हो और आप चाहते हैं कि आपकी app block न हो, 
           तो आप Task और async/await का इस्तेमाल करते हो।
         * Task is an object that represents an asynchronous operation. It allows background work without blocking the main thread.
         * ActionResult -->HTTP response format me data bhejta hai  
         * Client Request → API Controller → DbContext → Query Database → Get Data → Return to Client
         * Entity Framework Core का async method ToListAsync()--->Saara data laata hai list me
         * JournalEntries table से सारे records को List में fetch करता है
         * JournalEntries-->table hai jo data base me hai 
         * _context Aapke C# code aur actual SQL database ke beech communication karwana." e object hai AppDbContext ka jo inharit hai  DbContext class se   
         */
        [HttpGet]
        public async Task<ActionResult<List<JournalEntry>>> GetAll()
        {
            
            return await _context.JournalEntries.ToListAsync();
        }

        /*
           HTTP POST request ko handle karta hai, jo client se data create karne ke liye aata hai.
           Step 2: [FromBody] ka matlab hai ki client se aane wale JSON data ko ,JournalEntry object me bind kiya jaayega automatically.      
           Step 3: Agar journalEntry object null hai, toh BadRequest return karo.      
           Step 4: Agar journalEntry ka Title ya Content null nahi hai , toh object database ki table me add karna hai (track karo).             
           Step 5: SaveChangesAsync() us data ko database me actual insert karta hai ASynchronously.         
           Step 5: Agar sab kuch sahi hua to HTTP 200 OK response bheja jaata hai client ko
         */

        [HttpPost]
        public async Task<ActionResult> CreateEntry([FromBody] JournalEntry journalEntry)
        {
            if (journalEntry == null)
                {
                     return BadRequest("Journal entry cannot be null.");
                }           
                _context.JournalEntries.Add(journalEntry);
                await _context.SaveChangesAsync();
                return Ok();
        }


        [HttpGet("id/{id}")]
        /*
            async keyword marks a method as asynchronous.
            await pauses the current method until a task finishes, letting the program keep running smoothly without freezing.
            FindAsync(id)--> database से उस specific entry को ढूंढता है जिसकी id दी गई है।
         */
        public async Task<ActionResult<JournalEntry>> GetById(long id)
        {
            var entry = await _context.JournalEntries.FindAsync(id);
            if (entry == null) return NotFound();
            return Ok(entry);
        }


        /*database से उस रिकॉर्ड को asynchronously ढूंढता है जिसकी id वही होती है जो आपने दी है। अगर रिकॉर्ड मिल जाता है,
             तो उसे existing में रखता है और फिर आप उसे update करते हो।
         *Database में किए गए changes को सुरक्षित रूप से save करो और इस काम के खत्म होने तक asynchronously इंतजार करो।"
         *SaveChangesAsync()-> यह method database में किए गए changes को सुरक्षित रूप से save करता है और इस काम के खत्म होने तक asynchronously इंतजार करता है।
         */
        [HttpPut("id/{id}")]
        public async Task<ActionResult> Update(long id, [FromBody] JournalEntry entry)
        {
            var existing = await _context.JournalEntries.FindAsync(id); 
            if (existing == null) return NotFound();

            existing.Title = entry.Title;
            existing.Content = entry.Content;
            //_context.JournalEntries.Update(entry);
            await _context.SaveChangesAsync(); 
            return Ok(existing);
        }
        /* database से उस रिकॉर्ड को asynchronously ढूंढता है जिसकी id वही होती है जो आपने दी है। अगर रिकॉर्ड मिल जाता है,
         * REMOVE करता है उसे context से, ताकि वह database से भी हट जाए।
         * THNE ENTRY को database से हटाने के लिए SaveChangesAsync() method का उपयोग करता है।
         */
        [HttpDelete("id/{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var entry = await _context.JournalEntries.FindAsync(id);
            if (entry == null) 
              { 
                 return NotFound(); 
              }

            _context.JournalEntries.Remove(entry);// कह रहा है — "इस entry को हटाना है" (पर अभी सिर्फ context के अंदर बताना है)
            await _context.SaveChangesAsync(); //कह रहा है — "अब जो भी changes हैं, जैसे delete, update या add, उन्हें database में apply करो"
            return Ok(entry);
        }

    }
}
