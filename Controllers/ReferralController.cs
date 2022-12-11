using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ReferralCodeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReferralController : ControllerBase
    {
        private readonly ILogger<ReferralController> _logger;
        DatabaseContext context;

        public ReferralController(ILogger<ReferralController> logger , DatabaseContext _context)
        {
            context = _context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult CheckReferral(ReferralValidity_Model referralValidity_Model)
        {
            try
            {
                ReferralCode referralCode = context.referralCodes.Where(x => x.referralCode == referralValidity_Model.referralCode).FirstOrDefault();
                if(referralCode != null)
                {
                    if(!referralCode.used || referralCode.phone_guid == referralValidity_Model.guid)
                    {
                        referralCode.used = true;
                        referralCode.phone_guid = referralValidity_Model.guid;

                        context.referralCodes.Update(referralCode);
                        return Ok("OK");
                    }
                    
                    return BadRequest("Code Used");
                }

                return BadRequest("Not Found");
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
