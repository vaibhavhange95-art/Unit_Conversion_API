using Unit_Conversion_API.DTOs;

namespace Unit_Conversion_API.Services.Interfaces
{
    public interface IConversionService
    {
        ConversionResponseDto Convert(ConversionRequestDto request);
    }
}