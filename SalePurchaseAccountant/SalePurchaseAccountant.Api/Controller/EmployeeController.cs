using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalePurchaseAccountant.Api.ViewModels;
using SalePurchaseAccountant.BLL;
using SalePurchaseAccountant.Models.Accounts;
using SalePurchaseAccountant.Models.Employee;
using SalePurchaseAccountant.Models.Helpers;

namespace SalePurchaseAccountant.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeBll _employee;
        public EmployeeController()
        {
            _employee = new EmployeeBll();
        }

        [HttpGet]
        [Route("get/newCode/{companyCode}/{type}")]
        public IActionResult GetNewCode(string companyCode,UserType type)
        {
            try
            {
                string code = _employee.GetNewCode(companyCode, type);
                if (!String.IsNullOrEmpty(code))
                {
                    return Ok(new { status = true, result = code });
                }
                else
                {
                    return NotFound(new { status = false, result = "Failed to generate new code." });
                }
            }
            catch (InvalidException err) { return Ok(new { status = false, result = err.Message }); }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("salesman/save")]
        public IActionResult SaveSalesman(SalesmanModel salesman)
        {
            try
            {
                if (_employee.SaveOrUpdateSalesman(salesman))
                {
                    return Ok(new { status = true, result = "Salesman Saved Successfully." });
                }
                else
                {
                    return Ok(new { status = false, result = "Failed to save the salesman." });
                }
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

        [HttpGet]
        [Route("salesman/get/{companyCode}/{code}")]
        public IActionResult GetSalesman(string companyCode, string code)
        {
            try
            {
                var salesman = _employee.GetSalesman(companyCode,code);
                if (salesman.Count > 0)
                {
                    return Ok(new { status = true, result = salesman });
                }
                else
                {
                    return NotFound(new { status = false, result = "No salesman found" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("salesman/purchase")]
        public IActionResult SalesmanPurchase(SalesmanAccountModel accountModel)
        {
            try
            {
                var currentMonthPurchase = _employee.PurchaseBySalesman(accountModel);
                return Ok(new { status = true, result = currentMonthPurchase });
            }
            catch (InvalidException err) { return Ok(new { status = false, result = err.Message }); }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("salesman/sale")]
        public IActionResult SalesmanSale(SalesmanAccountModel accountModel)
        {
            try
            {
                var currentMonthSale = _employee.SaleBySalesman(accountModel);
                return Ok(new { status = true, result = currentMonthSale });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("member/save")]
        public IActionResult SaveMember(MemberModel member)
        {
            try
            {
                if (_employee.SaveOrUpdateMember(member))
                {
                    return Ok(new { status = true, result = "Member Saved Successfully." });
                }
                else
                {
                    return Ok(new { status = false, result = "Failed to save the Member." });
                }
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

        [HttpGet]
        [Route("member/get/{companyCode}/{code}")]
        public IActionResult GetMember(string companyCode,string code)
        {
            try
            {
                var members = _employee.GetMember(companyCode,code);
                if (members.Count > 0)
                {
                    return Ok(new { status = true, result = members });
                }
                else
                {
                    return NotFound(new { status = false, result = "No member found" });
                }
            }
            catch (InvalidException err) { return Ok(new { status = false, result = err.Message }); }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("member/purchase")]
        public IActionResult MemberPurchase(MemberAccountModel accountModel)
        {
            try
            {
                var currentMonthPurchase = _employee.PurchaseByMember(accountModel);
                return Ok(new { status = true, result = currentMonthPurchase });
            }
            catch (InvalidException err) { return Ok(new { status = false, result = err.Message }); }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("member/sale")]
        public IActionResult MemberSale(MemberAccountModel accountModel)
        {
            try
            {
                var currentMonthSale = _employee.SaleByMember(accountModel);
                return Ok(new { status = true, result = currentMonthSale });
            }
            catch (InvalidException err) { return Ok(new { status = false, result = err.Message }); }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("count/{companyCode}/{type}")]
        public IActionResult Count(string companyCode, UserType type)
        {
            try
            {
                var count = _employee.Count(companyCode,type);
                return Ok(new { status = true, result = count });
            }
            catch (InvalidException err) { return Ok(new { status = false, result = err.Message }); }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get/{companyCode}")]
        public IActionResult GetEmployees(string companyCode)
        {
            try
            {
                var employees = _employee.GetEmployees<EmployeeViewModel>(companyCode);
                if (employees.Count > 0)
                {
                    return Ok(new { status = true, result = employees });
                }
                else
                {
                    return NotFound(new { status = false, result = "No Employee found" });
                }
            }
            catch (InvalidException err) { return Ok(new { status = false, result = err.Message }); }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}