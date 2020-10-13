using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalePurchaseAccountant.Api.ViewModels;
using SalePurchaseAccountant.BLL;
using SalePurchaseAccountant.Models.Helpers;

namespace SalePurchaseAccountant.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountsBll _account;
        public AccountController()
        {
            _account = new AccountsBll();
        }

        [HttpGet]
        [Route("salary/process")]
        public IActionResult ProcessSalary()
        {
            try
            {
                bool isSuccess = _account.ProcessSalary();
                if (isSuccess)
                {
                    return Ok(new { status = true, result = "Salary Processed Successfully" });
                }
                else
                {
                    return Ok(new { status = false, result = "Operation failed to process salary." });
                }
            }
            catch (InvalidException ex)
            {
                return Ok(new { status = false, result = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("salary/get/{code}/{month}")]
        public IActionResult GetSalary(string code, string month)
        {
            try
            {
                var salaries = _account.GetSalary<SalaryViewModel>(code, month);
                if (salaries.Count > 0)
                {
                    return Ok(new { status = true, result = salaries });
                }
                else
                {
                    return NotFound(new { status = false, result = "Salary not processed yet." });
                }
            }
            catch (InvalidException ex)
            {
                return Ok(new { status = false, result = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}