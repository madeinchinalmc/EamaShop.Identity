using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EamaShop.Identity.API.Controllers
{
    /// <summary>
    /// doc
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// doc
        /// </summary>
        /// <returns></returns>
        public IActionResult Index() => Redirect("~/swagger");
    }
}
