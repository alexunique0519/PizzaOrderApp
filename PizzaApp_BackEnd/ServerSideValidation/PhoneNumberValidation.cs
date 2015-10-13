using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServerSideValidation
{
    public class PhoneNumberValidation : ValidationAttribute
    {
        public PhoneNumberValidation() : base("{0} is not a valid phone number") { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Regex pattern = new Regex(@"^(\d{3})[ -.]?(\d{3})[ -.]?(\d{4})$", RegexOptions.IgnoreCase);
           
            //is the value passed in equals null or meets the correct pattern, return success
            if (pattern.IsMatch(value.ToString()) || value == null)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
        }
    }
}
