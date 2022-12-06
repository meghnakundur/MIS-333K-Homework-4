using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kundur_Meghna_HW4.Models
{
    public enum ProductType {NewHardback, NewPaperback, UsedHardback, UsedPaperback, Other}
    public class Product
    {

        [Display(Name = "Product ID:")]
        [Required(ErrorMessage = "Product ID is a required field.")]
        public Int32 ProductID { get; set; }

        [Display(Name = "Product Name:")]
        [Required(ErrorMessage = "Product name is a required field.")]
        public String ProductName { get; set; }

        [Display(Name = "Product Description:")]
        public String ProductDescription { get; set; }

        [Display(Name = "Product Price:")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [Required(ErrorMessage = "Product price is a required field.")]
        [Range(0, 1000000, ErrorMessage = "Price must be at least $0")]
        public Decimal Price { get; set; }

        [Display(Name = "Product Type:")]
        public ProductType ProductType { get; set; }

    }
}

