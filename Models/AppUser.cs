using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Kundur_Meghna_HW4.Models
{
    public class AppUser : IdentityUser
    {
        //Add additional user fields you need here

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name is required.")]
        public String FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name is required.")]
        public String LastName { get; set; }
    }
}
