using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing2.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Damascus",
                    ShortName = "DM"
                },
                new Country
                {
                    Id = 2,
                    Name = "Lattakia",
                    ShortName = "LT"
                },
                new Country
                {
                    Id = 3,
                    Name = "Tartus",
                    ShortName = "TR"
                }
                );

            builder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Dama Rose",
                    Address = "Omaween Square",
                    Rating = 4.5,
                    CountryId = 1
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Meredian",
                    Address = "10th Street",
                    Rating = 3.3,
                    CountryId = 2
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Holiday Beach",
                    Address = "Tartus-Homs Highway",
                    Rating = 2.9,
                    CountryId = 3
                },
                new Hotel
                {
                    Id = 4,
                    Name = "Sheraton",
                    Address = "DownTown",
                    Rating = 4.2,
                    CountryId = 1
                }
                );

        }

       

    }
}
