using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalePurchaseAccountant.BLL;
using SalePurchaseAccountant.Models.BasicSettings;
using SalePurchaseAccountant.Models.Helpers;

namespace SalePurchaseAccountant.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly SettingsBll _settings;
        public SettingsController()
        {
            _settings = new SettingsBll();
        }
        
        [HttpGet]
        [Route("district/{districtId=-1}")]
        public IActionResult GetDistrict(int districtId)
        {
            try
            {
                var districts = _settings.GetDistrict(districtId);
                if (districts.Count > 0)
                {
                    return Ok(new { status = true, result = districts });
                }
                else
                {
                    return Ok(new { status = false, result = "No district found." });
                }
            }
            catch (InvalidException err) { return Ok(new { status = false, result = err.Message }); }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
        [HttpGet]
        [Route("thana/{districtId=-1}/{thanaId=-1}")]
        public IActionResult GetThana(int districtId,int thanaId)
        {
            try
            {
                var thana = _settings.GetThana(districtId,thanaId);
                if (thana.Count > 0)
                {
                    return Ok(new { status = true, result = thana });
                }
                else
                {
                    return Ok(new { status = false, result = thana });
                }
            }
            catch (InvalidException err) { return Ok(new { status = false, result = err.Message }); }catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpGet]
        public IActionResult GetnewCompanyCode()
        {
            try
            {
                string code = _settings.GetNewCompanyCode();
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
        public IActionResult Registration(CompanyModel company)
        {
            try
            {
                var newCompany =_settings.Registration(company);
                if (newCompany!=null)
                {
                    return Ok(new { status = true, result = "Company Registered Successfully." });
                }
                else
                {
                    return Ok(new { status = false, result = "Failed to Registration" });
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

    }
}