using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace pb_category.Models
{
    public class ValidateParametersQn : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                int count = 0;
                foreach (var item in value as List<string>)
                {
                    count++;
                    bool intResultTryParse = double.TryParse(item.ToString(), out double dimension);
                    if (intResultTryParse != true)
                        return new ValidationResult("Нужно указать низшую теплоту сгорания");
                }
                if (count == (value as List<string>).Count)
                    return ValidationResult.Success;
            }
            return new ValidationResult("Нужно указать низшую теплоту сгорания");
        }
    }
    public class ValidateParametersG : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                int count = 0;
                foreach (var item in value as List<string>)
                {
                    count++;
                    bool intResultTryParse = double.TryParse(item.ToString(), out double dimension);
                    if (intResultTryParse != true)
                        return new ValidationResult("Нужно указать количество материала пожарной нагрузки");
                }
                if (count == (value as List<string>).Count)
                    return ValidationResult.Success;
            }
            return new ValidationResult("Нужно указать количество материала пожарной нагрузки");
        }
    }
    public class CalculateDCSViewModel
    {
        public List<string> Name { get; set; }
        [Required]
        [ValidateParametersG]
        public List<string> G { get; set; }
        [Required]
        [ValidateParametersQn]
        public List<string> Qn { get; set; }
        [Required(ErrorMessage = "Не указана площадь")]
        public string S { get; set; }
        public string H { get; set; }
    }
}