using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace JrEntryWebApi.Models
{
    

    public class User
    {
        [Key]
        public int? Id { get; set; } // SQL Server में ज्यादातर int primary key होता है, लेकिन आप GUID भी use कर सकते हैं


        [Required] // EF Core में सीधे attribute से नहीं होता, नीचे Fluent API में दिखाते हैं
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        // Navigation property       
        public List<JournalEntry> JournalEntries { get; set; } = new();   // journal entry k referance hold karegi 
    }

}
