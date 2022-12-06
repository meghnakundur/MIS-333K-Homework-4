using Microsoft.AspNetCore.Identity;
using Kundur_Meghna_HW4.DAL;
//Update these using statements to include your project name
using Kundur_Meghna_HW4.Models;
using Kundur_Meghna_HW4.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

//Upddate this namespace to match your project name
namespace Kundur_Meghna_HW4.Seeding
{
    public static class SeedUsers
    {
        public async static Task<IdentityResult> SeedAllUsers(UserManager<AppUser> userManager, AppDbContext context)
        {
            //Create a list of AddUserModels
            List<AddUserModel> AllUsers = new List<AddUserModel>();

            AllUsers.Add(new AddUserModel()
            {
                User = new AppUser()
                {
                    //populate the user properties that are from the 
                    //IdentityUser base class
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    PhoneNumber = "(512)555-1234",

                    //Add additional fields that you created on the AppUser class
                    //FirstName is included as an example
                    FirstName = "Admin",
                    LastName = "Manager"
                },
                Password = "Abc123!",
                RoleName = "Admin"
            });

            AllUsers.Add(new AddUserModel()
            {
                User = new AppUser()
                {
                    //populate the user properties that are from the 
                    //IdentityUser base class
                    UserName = "bevo@example.com",
                    Email = "bevo@example.com",
                    PhoneNumber = "(512)555-5555",

                    //Add additional fields that you created on the AppUser class
                    //FirstName is included as an example
                    FirstName = "Bevo",
                    LastName = "Hookem"
                },
                Password = "Password123!",
                RoleName = "Customer"
            });

            //create flag to help with errors
            String errorFlag = "Start";

            //create an identity result
            IdentityResult result = new IdentityResult();
            //call the method to seed the user
            try
            {
                foreach (AddUserModel aum in AllUsers)
                {
                    errorFlag = aum.User.Email;
                    result = await Utilities.AddUser.AddUserWithRoleAsync(aum, userManager, context);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("There was a problem adding the user with email: "
                    + errorFlag, ex);
            }

            return result;
        }
    }
}
