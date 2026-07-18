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


        [HttpPost]
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
    }
}