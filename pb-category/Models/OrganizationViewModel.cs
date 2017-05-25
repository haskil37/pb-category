using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace pb_category.Models
{
    public class ValidateDimensions : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                bool intResultTryParse = double.TryParse(value.ToString(), out double dimension);
                if (intResultTryParse == true && dimension > 0)
                    return ValidationResult.Success;
            }
            return new ValidationResult("Некорректный габаритный размер");
        }
    }
    public class OrganizationViewModel
    {
        [Required(ErrorMessage = "Заполните наименование организации")]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Annotation { get; set; }
        public string Characteristic { get; set; }
        [ValidateDimensions]
        public string Length { get; set; }
        [ValidateDimensions]
        public string Width { get; set; }
        [ValidateDimensions]
        public string Height { get; set; }
        public List<int> A { get; set; }
        public List<int> B { get; set; }
        public List<int> C { get; set; }
        public List<int> D { get; set; }
        public List<int> E { get; set; }
    }
}