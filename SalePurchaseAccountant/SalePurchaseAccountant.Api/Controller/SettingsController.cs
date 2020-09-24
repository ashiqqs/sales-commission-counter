using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalePurchaseAccountant.BLL;

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
                return Ok(districts);
            }
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
                return Ok(thana);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}