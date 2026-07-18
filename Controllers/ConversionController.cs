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
        private readonly Unit_Conversion_API.Services.Implementation.FormulaCategoryRegistry _formulaRegistry;

        public ConversionController(IConversionService conversionService, Unit_Conversion_API.Services.Implementation.FormulaCategoryRegistry formulaRegistry)
        {
            _conversionService = conversionService;
            _formulaRegistry = formulaRegistry;
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

            // Ensure category is marked as formula-based
            _formulaRegistry.AddFormulaCategory(formula.Category);
            var added = _formulaRegistry.AddFormula(formula);
            if (!added)
            {
                return Conflict("Formula already exists.");
            }

            return Ok("Formula added successfully.");
        }
    }
}