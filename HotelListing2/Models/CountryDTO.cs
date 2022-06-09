using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing2.Models
{
    public class CreateCountryDTO
    {
        [Required]
        [StringLength(maximumLength:50, ErrorMessage = "Country Name Is Too Long!")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 2, ErrorMessage = "Country ShortName Must Be 2 Characters Long Only!")]
        public string ShortName { get; set; }
    }

    public class CountryDTO : CreateCountryDTO
    {
        public int Id { get; set; }

        public IList<HotelDTO> Hotels { get; set; }
    }
}
