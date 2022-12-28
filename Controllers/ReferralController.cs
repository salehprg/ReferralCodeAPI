using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

        [HttpPost]
        public IActionResult CheckReferral([FromForm]ReferralValidity_Model referralValidity_Model)
        {
            try
            {
                string license = RSA.Decrypt(referralValidity_Model.referralCode);
                 if(license == null)
                    return BadRequest("License invalid");

                ReferralCode referralCode = context.referralCodes.Where(x => x.referralCode == license).FirstOrDefault();
                if(referralCode != null)
                {
                    if(!referralCode.used)
                    {
                        bool checkNickname = context.referralCodes.Where(x => x.nickname == referralValidity_Model.nickname).Any();
                        if(checkNickname)
                            return BadRequest("Nickname already taken");

                        referralCode.used = true;
                        referralCode.phone_guid = referralValidity_Model.guid;
                        referralCode.nickname = referralValidity_Model.nickname;

                        context.referralCodes.Update(referralCode);
                        context.SaveChanges();

                        ScoreBoard scoreBoard = new ScoreBoard();
                        scoreBoard.referal_id = referralCode.Id;
                        scoreBoard.score = 0;
                        scoreBoard.time = DateTime.Now;

                        context.scoreBoards.Add(scoreBoard);
                        context.SaveChanges();

                        return Ok(referralCode.nickname);
                    }
                    if(referralCode.phone_guid == referralValidity_Model.guid)
                    {
                        return Ok(referralCode.nickname);
                    }
                    
                    return BadRequest("License Used");
                }

                return BadRequest("Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
