﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace ServerSideValidation
{
    public class PostalCodeValidation : ValidationAttribute
    {
        public PostalCodeValidation() : base("{0} dose not match a Canadian Postal Pattern") { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {                             
            Regex pattern = new Regex(@"^[ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ]\s?\d[ABCEGHJKLMNPRSTVWXYZ]\d$", RegexOptions.IgnoreCase);
           
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
