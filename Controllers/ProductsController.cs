using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kundur_Meghna_HW4.DAL;
using Kundur_Meghna_HW4.Models;
using Microsoft.AspNetCore.Authorization;

namespace Kundur_Meghna_HW4.Controllers
{
    public class ProductsController : Controller
    {

        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            //this sets the value of the private variable equal to the AppDbContext object that was  
            //injected into the controller 
            _context = context;
        }

        // GET: Products
        //This is the action method for the Index page  
        //This will show a list of all the products in the database
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Products/Details/5
        //This method will show the details for one product 
        //The id parameter represents the ProductID of the product that the user wants to see
        public async Task<IActionResult> Details(int? id)
        {
            // the id parameter is nullable because we would rather show the user an error than have the program crash if the user did not specify an id
            if (id == null) //the user did not specify the id of the product they wanted
            {

                return View("Error", new String[] { "Please specify a product to view! " });
            }


            Product product = await _context.Products.FirstOrDefaultAsync(m => m.ProductID == id);


            //If the specified id does not match a product in the database, product will be null 
            //Show the user an error message
            if (product == null)
            {

                return View("Error", new String[] { "This product was not found!" });

            }
            //if code gets this far, everything is okay; send the user to the Product/Details 
            //view with the product you just found in the database
            return View(product);
        }

        // GET: Products/Create
        //This method will show the user the blank create product view 
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            //Show the user the Views/Product/Create view 
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //This tells the second overload of the Create method to respond to HTTP Post requests 
        [HttpPost]
        //This prevents cross-site scripting attacks – this helps make sure that the user actually  
        //intended to make this request.   
        [ValidateAntiForgeryToken]

        //this method signature includes a bind list, which means only the ProductID, Name, Description 
        //and price will be included in this HTTP request. This keeps malicious users from modifying fields

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,ProductDescription,Price,ProductType")] Product product)
        {
            //make sure that the product to be created meets all the rules specified in the  
            //product model class
            if (ModelState.IsValid)
            {
                //add the product to the database 
                _context.Add(product);

                //save the changes to the database 
                await _context.SaveChangesAsync();

                //send the user back to the Index action to re-generate the list of products 
                return RedirectToAction(nameof(Index));
            }
            //this is the sad path – the model state was not valid, so the user gets sent back to the 
            //create product view to try again.
            return View(product);
        }

        // GET: Products/Edit/5
        //This method will allow the user to edit a product 
        //The id parameter represents the ProductID of the product that the user wants to edit 

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            // the id parameter is nullable because we would rather show the user an error than have the program crash if the user did not specify an id.

            // FIRST NULL = tell me which product you want to edit
            if (id == null)
            {

                return View("Error", new String[] { "Please specify a product to edit! " });
            }
            //This line of code will look for the desired ProductID in the database 
            Product product = await _context.Products.FindAsync(id);
 

            //If the specified id does not match a product in the database, product will be null 
            //Show the user an error message
            //SECOND NULL = you told me which product to edit, but the id does not exist
            if (product == null)
            {

                return View("Error", new String[] { "This product was not found!" });
            }
            //if code gets this far, everything is okay; send the user to the Product/Edit 
            //view with the product you just found in the database
            return View(product);
        }

        // POST: Products/Edit/5

        //This tells the second overload of the Edit method to respond to HTTP Post requests 
        [HttpPost]
        [ValidateAntiForgeryToken]
        //this method signature includes a bind list, which means only the ProductID, Name, Description 
        //and price will be included in this HTTP request. This keeps malicious users from modifying fields.
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,ProductDescription,Price,ProductType")] Product product)
        {
            //if the id specified is not the same as the product id, then the user is trying to modify  
            //a different product than the one being displayed.  This is a security precaution. 
            if (id != product.ProductID)
            {
   
                return View("Error", new String[] { "There was a problem editing this product!" });
            }

            //make sure that the new information on the product meets all the rules specified in the
            //product model class 
            if (ModelState.IsValid)
            {
                //editing code in the database can sometimes cause errors, so wrap this code  
                //in a try/catch block
                try
                {
                    //update the product in the database 
                    _context.Update(product);

                    //save the changes in the database 
                    await _context.SaveChangesAsync();
                }
                //the code inside the catch block will run if there is a problem saving 
                //the changes to the database 
                catch (DbUpdateConcurrencyException)
                {
                    //this code calls a method at the end of this controller that checks 
                    //to see if the product is in the database.  If the product does not exist 
                    //then the user gets an error message
                    if (!ProductExists(product.ProductID))
                    {

                        return View("Error", new String[] { "This product was not found!" });
                    }
                    //if the code gets to the ‘else’ then there was a problem with the edits 
                    //this will throw the incoming exception and crash the program 
                    else
                    {
                        //you need to change this code to display an error message rather than 
                        //causing the program to crash. A better option is something like 
                        return View("Error", new String[] { "There was a problem with your edits!" });
                        //throw;
                    }
                }
                //this is the happy path – if the code gets to here, the edits were successfully 
                //saved to the database.  Send the user back to back to the Index action to  
                //re-generate the list of products 
                return RedirectToAction(nameof(Index));
            }
            //this is the sad path – the model state was not valid, so you need to send the user
            //back to the Views/Product/Edit page to try again
            return View(product);
        }

        // GET: Products/Delete/5
        //This method will allow the user to delete a product 
        //The id parameter represents the ProductID of the product that the user wants to delete 
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            // the id parameter is nullable because we would rather show the user an error than have the program crash if the user did not specify an id.
            if (id == null)
            {

                return View("Error", new String[] { "Please specify a product to delete! " });

            }
            //This line of code will look for the desired ProductID in the database 
            Product product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);

            //If the specified id does not match a product in the database, product will be null 
            //Show the user an error message 
            if (product == null)
            {

                return View("Error", new String[] { "This product was not found!" });

            }
            //if code gets this far, everything is okay; send the user to the Product/Delete 
            //view with the product you just found in the database 
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //This line of code will look for the desired ProductID in the database 
            Product product = await _context.Products.FindAsync(id);

            //remove the product from the database 
            _context.Products.Remove(product);

            //save changes to the database 
            await _context.SaveChangesAsync();

            //Send the user back to back to the Index action to re-generate the list of products 
            return RedirectToAction(nameof(Index));
        }

        //this is a private method to see if a product with a particular id exists in the database 
        private bool ProductExists(int id)
        {
            //if there is a product with this id in the database, return true if not,return false 
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}