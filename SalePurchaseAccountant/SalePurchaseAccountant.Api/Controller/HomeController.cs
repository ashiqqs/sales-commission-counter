using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalePurchaseAccountant.BLL;

namespace SalePurchaseAccountant.Api.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public IActionResult Index(string user="Ashiq")
        {
            bool isConnected = Connection.IsConnected();
            string status = isConnected ? $"Hello {user}, Your Database Connected" : $"Hello {user}, Your Database Disconnected";
            return Ok(status);
        }
    }
}