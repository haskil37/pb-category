using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pb_category.Models
{
    public class OrganizationViewModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Annotation { get; set; }
        public string Characteristic { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}