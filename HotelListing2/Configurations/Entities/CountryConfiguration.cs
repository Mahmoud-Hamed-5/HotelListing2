using HotelListing2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing2.Configurations.Entities
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
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
        }
    }
}
