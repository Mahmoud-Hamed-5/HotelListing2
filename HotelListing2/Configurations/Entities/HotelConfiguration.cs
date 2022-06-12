using HotelListing2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing2.Configurations.Entities
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
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
