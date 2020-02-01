using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupplierCustomer.WebApi.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeApiController : Controller
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Server is working.";
        }
    }
}
