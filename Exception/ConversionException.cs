namespace Unit_Conversion_API.Exceptions
{
    public class ConversionException : Exception
    {
        public ConversionException(string message)
            : base(message)
        {
        }
    }
}