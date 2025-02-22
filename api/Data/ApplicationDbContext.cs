
using api.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



namespace api.Data;
public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions dbContextOptions)  
    : base(dbContextOptions)
    {
    }
        public DbSet <Stock> Stocks { get; set; }

        public DbSet <Comment> Comments { get; set; }
        public DbSet <Portfolio> Portfolios { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)  //Δημιουργεί δύο ρόλους (Admin και User).
                                                             // Χρησιμοποιεί το Entity Framework Core HasData() για να κάνει data seeding.
                                                             //Όταν γίνει migration και update database, αυτοί οι ρόλοι θα εισαχθούν στη βάση.
    {
        base.OnModelCreating(builder);

    builder.Entity<Portfolio>()
        .HasKey(p => new { p.AppUserId, p.StockId }); // ✅ Σωστό composite key

    builder.Entity<Portfolio>()
        .HasOne(p => p.AppUser)
        .WithMany(u => u.Portfolios)
        .HasForeignKey(p => p.AppUserId)
        .OnDelete(DeleteBehavior.Cascade); // ✅ Αφαίρεση null reference, εάν διαγραφεί ο χρήστης

    builder.Entity<Portfolio>()
        .HasOne(p => p.Stock)
        .WithMany(s => s.Portfolios)
        .HasForeignKey(p => p.StockId)
        .OnDelete(DeleteBehavior.Restrict); 

        List<IdentityRole> roles = new List<IdentityRole> //Το IdentityRole είναι η προεπιλεγμένη κλάση του ASP.NET Core Identity για ρόλους χρηστών.
        {
            new IdentityRole 
            {
                Name = "Admin",
                NormalizedName = "ADMIN" //Το NormalizedName πρέπει να είναι κεφαλαία (π.χ. "ADMIN", "USER").

            },

             new IdentityRole 
            {
                Name = "User",
                NormalizedName = "USER"

            }
        };

        builder.Entity<IdentityRole>().HasData(roles); //Το HasData() λέει στο Entity Framework να γεμίσει τη βάση με αυτά τα δεδομένα κατά τη migration.

    }

}