using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalePurchaseAccountant.Api.ViewModels;
using SalePurchaseAccountant.BLL;
using SalePurchaseAccountant.Models;
using SalePurchaseAccountant.Models.Employee;
using SalePurchaseAccountant.Models.Helpers;

namespace SalePurchaseAccountant.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserBll _user;
        private readonly EmployeeBll _employee;
        private readonly SettingsBll _settings;
        public UserController()
        {
            _user = new UserBll();
            _employee = new EmployeeBll();
            _settings = new SettingsBll();
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login(UserModel user)
        {
            try
            {
                var loggedUser = _user.Login<UserViewModel>(user.Name, user.Password);
                if (loggedUser == null || (loggedUser.Name != user.Name && user.Password != user.Password))
                {
                    throw new InvalidException("Incorrect username or password!");
                }

                if (loggedUser.UserType == UserType.AlphaMember || loggedUser.UserType == UserType.BetaMember)
                {
                    loggedUser.EmploymentInfo.Membership = _employee.GetMember(loggedUser.CompanyCode, loggedUser.Code).FirstOrDefault() ?? new MemberModel();
                    loggedUser.Company = _settings.GetCompany(loggedUser.CompanyCode);
                    loggedUser.PurchaseAmount = _employee.PurchaseAmount(loggedUser.CompanyCode, DateTime.Now.ToString("yyyyMM"), loggedUser.UserType, loggedUser.EmploymentInfo.Membership.Sidc);
                    loggedUser.SalesAmount = _employee.SalesAmount(loggedUser.CompanyCode, DateTime.Now.ToString("yyyyMM"), loggedUser.UserType, loggedUser.EmploymentInfo.Membership.Sidc);
                }
                else if (loggedUser.UserType == UserType.Salesman)
                {
                    SalesmanModel emp = _employee.GetSalesman(loggedUser.CompanyCode, loggedUser.Code).FirstOrDefault();
                    loggedUser.EmploymentInfo.Id = emp.Id;
                    loggedUser.EmploymentInfo.Code = emp.Code;
                    loggedUser.EmploymentInfo.Name = emp.Name;
                    loggedUser.EmploymentInfo.IsAlphaMember = emp.IsAlphaMember;
                    loggedUser.EmploymentInfo.IsBetaMember = emp.IsBetaMember;
                    loggedUser.EmploymentInfo.Membership = _employee.GetMemberBySidc(loggedUser.CompanyCode, loggedUser.Code) ?? new MemberModel();
                    loggedUser.Company = _settings.GetCompany(emp.CompanyCode);
                    loggedUser.PurchaseAmount = _employee.PurchaseAmount(loggedUser.CompanyCode, DateTime.Now.ToString("yyyyMM"), loggedUser.UserType, loggedUser.Code);
                    loggedUser.SalesAmount = _employee.SalesAmount(loggedUser.CompanyCode, DateTime.Now.ToString("yyyyMM"), loggedUser.UserType, loggedUser.Code);

                }
                else
                {
                    loggedUser.Company = _settings.GetCompany(loggedUser.Code);
                    loggedUser.PurchaseAmount = _employee.PurchaseAmount(loggedUser.Code, DateTime.Now.ToString("yyyyMM"), loggedUser.UserType, loggedUser.Code);
                    loggedUser.SalesAmount = _employee.SalesAmount(loggedUser.Code, DateTime.Now.ToString("yyyyMM"), loggedUser.UserType, loggedUser.Code);

                }

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