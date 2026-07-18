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

        
    //    public IActionResult SearchCategories(
    //string text)
    //    {
    //        var result = _unitRepository
    //            .SearchCategories(text);

    //        return Ok(result);
    //    }

        [HttpGet("SearchUnits")]
        public IActionResult GetUnits(string category)
        {
            var units = _unitRepository.GetUnits(category);

            return Ok(units);
        }

        [HttpPost("/api/AddUnit")]
        public IActionResult AddUnit(
     AddUnitRequestDto request)
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
    }
}