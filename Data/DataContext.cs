using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RunWebAppGroup.Models;

namespace RunWebAppGroup.Data
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }


        public DbSet<Race> Races { get; set; }

        public DbSet<Club> Clubs { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }

    }


}


