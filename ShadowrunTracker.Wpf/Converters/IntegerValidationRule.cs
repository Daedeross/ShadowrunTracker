using System.Globalization;
using System.Windows.Controls;

namespace ShadowrunTracker.Wpf.Converters
{
    public class IntegerValidationRule : ValidationRule
    {
        public int Min { get; set; } = int.MinValue;
        public int Max { get; set; } = int.MaxValue;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value?.ToString();
            if (string.IsNullOrWhiteSpace(str))
            {
                return ValidationResult.ValidResult;
            }
            else if (int.TryParse(str, NumberStyles.Integer, cultureInfo.NumberFormat, out int output))
            {
                if (output >= Min && output <= Max)
                {
                    return ValidationResult.ValidResult;
                }
                else
                {
                    return new ValidationResult(false, $"Value out of bounds.");
                }
            }
            else
            {
                return new ValidationResult(false, $"Not an integer");
            }
        }
    }
}
