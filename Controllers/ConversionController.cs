using Microsoft.AspNetCore.Mvc;
using Unit_Conversion_API.DTOs;
using Unit_Conversion_API.Services.Interfaces;

namespace Unit_Conversion_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ConversionController : ControllerBase
    {
        private readonly IConversionService _conversionService;

        public ConversionController(IConversionService conversionService)
        {
            _conversionService = conversionService;
        }


        [HttpPost("/api/Convert")]
        public IActionResult Convert(
     [FromBody] ConversionRequestDto request)
        {
            if (request.Value == 0)
            {
                return BadRequest(
                    "Value cannot be zero.");
            }

            var result = _conversionService.Convert(request);

            return Ok(result);
        }

        // Endpoint to add a formula if not found
        [HttpPost("/api/AddFormula")]
        public IActionResult AddFormula([FromBody] Models.ConversionFormula formula)
        {
            if (formula == null || string.IsNullOrWhiteSpace(formula.Formula))
            {
                return BadRequest("Invalid formula.");
            }

            var repo = HttpContext.RequestServices.GetService(typeof(Repositories.Interfaces.IUnitRepository)) as Repositories.Interfaces.IUnitRepository;
            if (repo == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Repository not available.");
            }

            var added = repo.AddFormula(formula);
            if (!added)
            {
                return Conflict("Formula already exists.");
            }

            return Ok("Formula added successfully.");
        }
    }
}