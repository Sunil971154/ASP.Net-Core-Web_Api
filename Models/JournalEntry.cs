using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace JrEntryWebApi.Models
{
    public class JournalEntry
    {
        [Key]
        public int Id { get; set; }

       
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime LocalDateTime { get; set; } // Date and time of the entry

     
        public int UserId { get; set; } // Foreign Key


        [ForeignKey("UserId")]
        [ValidateNever]
        [JsonIgnore]
        public User User { get; set; } // Navigation property


    }
}
