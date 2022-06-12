using AutoMapper;
using HotelListing2.IRepository;
using HotelListing2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using HotelListing2.Data;

namespace HotelListing2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HotelController> _logger;
        private readonly IMapper _mapper;

        public HotelController(IUnitOfWork unitOfWork, ILogger<HotelController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotels()
        {
            try
            {
                var hotels = await _unitOfWork.Hotels.GetAll();
                var results = _mapper.Map<IList<HotelDTO>>(hotels);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Somthing went wrong in the {nameof(GetHotels)}");
                return StatusCode(500, "Internal server error, please try again later");
            }
        }



        [HttpGet("{id:int}", Name = "GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id , new List<string> { "Country" });
                 var result = _mapper.Map<HotelDTO>(hotel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Somthing went wrong in the {nameof(GetHotel)}");
                return StatusCode(500, "Internal server error, please try again later");
            }
        }



        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDTO hotelDTO)
        {
            _logger.LogInformation($"Creating Hotel {hotelDTO.Name}");
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST Attempt in {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }       

            try
            {
                var country_id = hotelDTO.CountryId;
                var country = await _unitOfWork.Countries.Get(q => q.Id == country_id);
                if (country == null)
                {
                    _logger.LogInformation($"Invalid Country ID : {country_id}");
                    return Problem($"Invalid Country ID : {country_id}");
                }

                var hotel = _mapper.Map<Hotel>(hotelDTO);
                await _unitOfWork.Hotels.Insert(hotel);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetHotel",new {id = hotel.Id }, hotel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Somthing went wrong in the {nameof(CreateHotel)}");
                return Problem($"Somthing went wrong in the {nameof(CreateHotel)}", statusCode: 500);
            }
        }




        [Authorize(Roles = "Administrator")]       
        [HttpPut("{id:int}")]      
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelDTO hotelDTO)
        {
            _logger.LogInformation($"Updating Hotel {hotelDTO.Name}");
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid UPDATE Attempt in {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                var country_id = hotelDTO.CountryId;
                var country = await _unitOfWork.Countries.Get(q => q.Id == country_id);
                if (country == null)
                {
                    _logger.LogInformation($"Invalid Country ID : {country_id}");
                    return Problem($"Invalid Country ID : {country_id}");
                }

                var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);
                if (hotel == null)
                {
                    _logger.LogError($"Invalid UPDATE Attempt in {nameof(UpdateHotel)}");
                    return BadRequest("Submitted data is invalid, Provide a valid Hotel-id");
                }

                _mapper.Map(hotelDTO, hotel);

                _unitOfWork.Hotels.Update(hotel);
                await _unitOfWork.Save();

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Somthing went wrong in the {nameof(CreateHotel)}");
                return Problem($"Somthing went wrong in the {nameof(CreateHotel)}", statusCode: 500);
            }
        }



        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            _logger.LogInformation($"Deleting Hotel with id: {id}");
            if (id < 1)
            {
                _logger.LogError($"Invalid UPDATE Attempt in {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {               
                var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);                          
                if (hotel == null)
                {
                    _logger.LogError($"Invalid DELETE Attempt in {nameof(DeleteHotel)}");
                    return BadRequest("Submitted data is invalid, Provide a valid Hotel-id");
                }

                await _unitOfWork.Hotels.Delete(id);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Somthing went wrong in the {nameof(DeleteHotel)}");
                return Problem($"Somthing went wrong in the {nameof(DeleteHotel)}", statusCode: 500);
            }
        }



    }
}
