using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSideValidation
{
    public class ProvinceValidation : ValidationAttribute
    {
        public ProvinceValidation() : base("{0} is not a valid province name") { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            List<string> provinceNames = new List<string> { "Ontario", "Quebec", "Manitoba", "Saskatchewan"};

            bool bContains = provinceNames.Contains(value.ToString());

            if (!bContains)
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            return ValidationResult.Success;
        }
    }
}
