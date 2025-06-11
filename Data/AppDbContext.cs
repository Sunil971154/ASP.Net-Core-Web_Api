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
        /*
              | Feature                            | Purpose                                                                       |
              | ---------------------------------- | ----------------------------------------------------------------------------- |
              | `HasIndex(...).IsUnique()`         | `UserName` को unique बना रहा है                                               |
              | `HasMany(...).WithOne(...)`        | `User` और `JournalEntry` के बीच 1-to-many relationship बना रहा है             |
              | `HasForeignKey(j => j.UserId)`     | Foreign key define कर रहा है                                                  |
              | `OnDelete(DeleteBehavior.Cascade)` | User delete होने पर उसके journal entries को भी automatically delete कर रहा है |

         
         */
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<User>()         //EF Core को बोल रहे हैं कि User entity की UserName property पर एक index बनाओ। ,Login जैसे ऑपरेशन fast होंगे क्योंकि index बना है।
                .HasIndex(u => u.UserName)
                .IsUnique();                    //.IsUnique() कह रहा है कि ये index unique होना चाहिए, यानी:कोई दो users एक जैसा UserName नहीं रख सकते।


            modelBuilder.Entity<User>()             //User entity की JournalEntries property को configure कर रहे हैं।    
                .HasMany(u => u.JournalEntries)     //User के पास कई JournalEntries हो सकती हैं।
                .WithOne(j => j.User)               //हर JournalEntry एक ही User से जुड़ी होती है (WithOne).
                .HasForeignKey(j => j.UserId)       //यह connection UserId foreign key से होता है (HasForeignKey).
                .OnDelete(DeleteBehavior.Cascade);  //और अगर user delete हो जाए, तो उसके सारे journal entries भी delete हो जाएं (Cascade Delete).

        }






        /* DB me Table banegi JournalEntries name se
         * 👉 यह property आपके DbContext में एक table की तरह काम करेगी, जिसका नाम है JournalEntries
           👉 और हर row का type है JournalEntry*/
        public DbSet<JournalEntry> JournalEntries { get; set; }

        public DbSet<User> Users { get; set; }
      


    }
}
