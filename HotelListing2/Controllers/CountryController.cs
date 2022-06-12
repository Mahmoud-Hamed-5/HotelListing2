using AutoMapper;
using HotelListing2.Data;
using HotelListing2.IRepository;
using HotelListing2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelListing2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;

        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetAll();
                var results = _mapper.Map<IList<CountryDTO>>(countries);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Somthing went Wrong in the {nameof(GetCountry)}");
                return StatusCode(500, "Internal server error, please try again later");
            }
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id:int}", Name = "GetCountry")]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                var country = await _unitOfWork.Countries.Get(q => q.Id == id , new List<string> {"Hotels"});
                var result = _mapper.Map<CountryDTO>(country);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Somthing went Wrong in the {nameof(GetCountries)}");
                return StatusCode(500, "Internal server error, please try again later");
            }
        }



        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countryDTO)
        {
            _logger.LogInformation($"Creating Country {countryDTO.Name}");
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST Attempt in {nameof(CreateCountry)}");
                return BadRequest(ModelState);
            }

            try
            {               
                var country = _mapper.Map<Country>(countryDTO);
                await _unitOfWork.Countries.Insert(country);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetCountry", new { id = country.Id }, country);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Somthing went wrong in the {nameof(CreateCountry)}");
                return Problem($"Somthing went wrong in the {nameof(CreateCountry)}", statusCode: 500);
            }
        }



        [Authorize(Roles = "Administrator")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDTO countryDTO)
        {
            _logger.LogInformation($"Updating Country {countryDTO.Name}");
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid UPDATE Attempt in {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }

            try
            {             

                var country = await _unitOfWork.Countries.Get(q => q.Id == id);
                if (country == null)
                {
                    _logger.LogError($"Invalid UPDATE Attempt in {nameof(UpdateCountry)}");
                    return BadRequest("Submitted data is invalid, Provide a valid Country-id");
                }

                _mapper.Map(countryDTO, country);

                _unitOfWork.Countries.Update(country);
                await _unitOfWork.Save();

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Somthing went wrong in the {nameof(UpdateCountry)}");
                return Problem($"Somthing went wrong in the {nameof(UpdateCountry)}", statusCode: 500);
            }
        }



        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            _logger.LogInformation($"Deleting Country with id: {id}");
            if (id < 1)
            {
                _logger.LogError($"Invalid UPDATE Attempt in {nameof(DeleteCountry)}");
                return BadRequest(ModelState);
            }

            try
            {
                var country = await _unitOfWork.Countries.Get(q => q.Id == id);
                if (country == null)
                {
                    _logger.LogError($"Invalid DELETE Attempt in {nameof(DeleteCountry)}");
                    return BadRequest("Submitted data is invalid, Provide a valid Country-id");
                }

                await _unitOfWork.Countries.Delete(id);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Somthing went wrong in the {nameof(DeleteCountry)}");
                return Problem($"Somthing went wrong in the {nameof(DeleteCountry)}", statusCode: 500);
            }
        }




    }

}

