using Microsoft.AspNetCore.Mvc;
using Unit_Conversion_API.DTOs;
using Unit_Conversion_API.Models;
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
                var units2 = _unitRepository.GetUnitCategories(category);

                return Ok(units);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("Availabe Unit Categories")]
        public IActionResult GetUnitCategories(string category)
        {
            try
            {
                var unitCategories = _unitRepository.GetUnitCategories(category);
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
        public IActionResult AddUnit(AddUnitRequestDto request)
        {
            try
            {
                var unit = new UnitDefinition
                {
                    Name = request.Name,
                    ToBaseFactor = request.ToBaseFactor
                };


                var added = _unitRepository.AddUnit(
                    request.Category,
                    unit);


                if (!added)
                {
                    return BadRequest(new
                    {
                        message = $"Unit '{request.Name}' already exists in category '{request.Category}'."
                    });
                }


                return Ok(new
                {
                    message = "Unit added successfully",
                    unit = request.Name
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