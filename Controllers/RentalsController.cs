using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Controllers
{
    public class RentalsController : Controller
    {

		[Authorize(Roles = "Owner")]
        public IActionResult New()
        {
            return View();
        }
    }
}
