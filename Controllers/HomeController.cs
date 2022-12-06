using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kundur_Meghna_HW4.DAL;
using Kundur_Meghna_HW4.Models;
using Microsoft.EntityFrameworkCore;

//Name: Meghna Kundur
//Date: 10/28/2022
//Description: HW4 - Product Tracker (Databases & Identity)

namespace Kundur_Meghna_HW4.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}

