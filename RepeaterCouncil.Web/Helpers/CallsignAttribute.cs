using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RepeaterCouncil.Web.Helpers
{
    public class CallsignAttribute : ValidationAttribute
    {
        private static readonly Regex _regex = new Regex(
            @"^[AKNW][A-Z]{0,2}[0-9][A-Z]{1,3}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; // Let [Required] handle nulls
            }

            var stringValue = value as string;
            if (stringValue != null && _regex.IsMatch(stringValue))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid US amateur radio callsign.");
        }
    }
}
