using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Unit_Conversion_API.DTOs;
using Unit_Conversion_API.Models;
using System.Text.Json.Serialization;
using Unit_Conversion_API.Repositories.Interfaces;

namespace Unit_Conversion_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnitController : ControllerBase
    {
        private readonly IUnitRepository _unitRepository;


        public UnitController(
            IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        [HttpGet("SearchUnits")]
        public IActionResult GetUnits(string category)
        {
            try
            {

                var units = _unitRepository.GetUnits(category);
 
                return Ok(units);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("Availabe Unit Categories")]
        public IActionResult GetUnitCategories()
        {
            try
            {
                var unitCategories = _unitRepository.GetUnitCategories();
                return Ok(unitCategories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occurred: {ex.Message}");
            }
        }


        [HttpPut("UpdateUnitBaseFactor")]
        public IActionResult UpdateUnitBaseFactor([FromBody] UpdateUnitRequest request)
        {
            try
            {
                var result = _unitRepository.UpdateUnitBaseFactor(
                    request.Category,
                    request.Name,
                    request.ToBaseFactor);

                if (!result)
                {
                    return NotFound("Unit or category not foun");
                }

                return Ok("Unit updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occurred: {ex.Message}");
            }
        }


        [HttpPost("/api/AddUnit")]
        public IActionResult AddUnit([FromBody] List<AddUnitRequestDto> requests)
        {
            try
            {
                if (requests == null || !requests.Any())
                {
                    return BadRequest("No units were provided.");
                }

                var addedUnits = new List<string>();
                var skippedUnits = new List<string>();

                foreach (var item in requests)
                {
                    var unit = new UnitDefinition
                    {
                        Name = item.Name,
                        ToBaseFactor = item.ToBaseFactor
                    };

                    var added = _unitRepository.AddUnit(item.Category, unit);

                    if (added)
                    {
                        addedUnits.Add($"Unit '{item.Name}' added successfully.");
                    }
                    else
                    {
                        skippedUnits.Add($"Unit '{item.Name}' is already available.");
                    }
                }
                return Ok(new
                {
                    Added = addedUnits,
                    Skipped = skippedUnits
                }); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occurred: {ex.Message}");
            }
        }
    }
}