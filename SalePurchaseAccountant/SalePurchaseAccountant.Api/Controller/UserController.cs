using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalePurchaseAccountant.BLL;
using SalePurchaseAccountant.Models;
using SalePurchaseAccountant.Models.Helpers;

namespace SalePurchaseAccountant.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserBll _user;
        public UserController()
        {
            _user = new UserBll();
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login(UserModel user)
        {
            try
            {
                var loggedUser = _user.Login(user.Name, user.Password);
                return Ok(new { status = true, result = loggedUser });
            }
            catch (InvalidException err) 
            { 
                return Ok(new { status = false, result = err.Message }); 
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}