using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Shunmai.Bxb.Common.Attributes
{
    public class PatternAttribute : ValidationAttribute
    {
        public string Pattern { get; set; }

        public PatternAttribute(string pattern)
        {
            Pattern = pattern;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The field '{name}' does not match the pattern '{Pattern}'.";
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            return Regex.IsMatch(value.ToString(), Pattern);
        }
    }
}
