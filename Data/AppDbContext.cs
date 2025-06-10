using Microsoft.EntityFrameworkCore;
using JrEntryWebApi.Models;
using JrEntryWebApi.Models;
namespace JerEntryWebApp.Data
{
    public class AppDbContext : DbContext
    {
        /*
       ✅ 1. AppDbContext(DbContextOptions<AppDbContext> options)
         Isme DbContextOptions<AppDbContext> parameter diya gaya hai, jisme database connection aur configuration ki details hoti hain.
         Iska matlab: Jab EF Core aapka DbContext banata hai, toh wo yahan se options pass karega (e.g., SQL Server ka connection string, lazy loading config, etc.).

        ✅ 2. : base(options)
            Ye DbContext base class ka constructor call karta hai.
            EF Core ke DbContext class me internal logic hota hai jo options ke basis par database ke saath connection banata hai, migrations handle karta hai, etc.
            Agar aap base(options) nahi likhenge, toh EF Core ko pata hi nahi chalega ki kaunsi database configuration use karni hai.
         */
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        {

        }

        /* DB me Table banegi JournalEntries name se
         * 👉 यह property आपके DbContext में एक table की तरह काम करेगी, जिसका नाम है JournalEntries
           👉 और हर row का type है JournalEntry*/
        public DbSet<JournalEntry> JournalEntries { get; set; }


    }
}
