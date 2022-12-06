using System;
using Microsoft.EntityFrameworkCore;

//TODO: Update this using statement to include your project name
using Kundur_Meghna_HW4.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    
//TODO: Make this namespace match your project name
namespace Kundur_Meghna_HW4.DAL
{
    //NOTE: This class definition references the user class for this project.  
    //If your User class is called something other than AppUser, you will need
    //to change it in the line below
    public class AppDbContext: IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options){ }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=tcp:fa22kundurmeghnahw4.database.windows.net,1433;Initial Catalog=fa22kundurmeghnahw4;Persist Security Info=False;User ID=MISAdmin;Password=Password123;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //this code makes sure the database is re-created on the $5/month Azure tier
            builder.HasPerformanceLevel("Basic");
            builder.HasServiceTier("Basic");
            base.OnModelCreating(builder);
        }

        //TODO: Add Dbsets here.  Products is included as an example.  
        public DbSet<Product> Products { get; set; }
    }
}
