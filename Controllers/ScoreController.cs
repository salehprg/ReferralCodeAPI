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
    public class ScoreController : ControllerBase
    {
        private readonly ILogger<ScoreController> _logger;
        DatabaseContext context;

        public ScoreController(ILogger<ScoreController> logger , DatabaseContext _context)
        {
            context = _context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult HighScores(string guid)
        {
            try
            {
                string _guid = RSA.Decrypt(guid.Replace(" " , "+"));

                ReferralCode referralCode = context.referralCodes.Where(x => x.phone_guid == _guid).FirstOrDefault();
                if(referralCode != null && referralCode.used)
                {
                    List<ScoreBoard> scoreBoards = context.scoreBoards.OrderByDescending(x => x.score).Take(5).ToList();
                    List<ScoreBoard_Model> scoreBoard_Models = new List<ScoreBoard_Model>();

                    foreach (var score in scoreBoards)
                    {
                        string nickname = context.referralCodes.Where(x => x.Id == score.referal_id).FirstOrDefault().nickname;
                        scoreBoard_Models.Add(new ScoreBoard_Model{
                            score = score.score,
                            time = score.time,
                            nickname = nickname
                        });
                    }

                    return Ok(scoreBoard_Models);
                }

                return BadRequest("Not Found");
            }
            catch
            {   
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult SubmitScore([FromForm]ScoreBoard_Model scoreBoard_Model)
        {
            try
            {
                string guid = RSA.Decrypt(scoreBoard_Model.phone_guid);

                if(guid == null)
                    return BadRequest("Data invalid");

                ReferralCode referralCode = context.referralCodes.Where(x => x.phone_guid == guid).FirstOrDefault();
                if(referralCode != null && referralCode.used)
                {
                    ScoreBoard scoreBoard = context.scoreBoards.Where(x => x.referal_id == referralCode.Id).FirstOrDefault();
                    if(scoreBoard.score < scoreBoard_Model.score)
                    {
                        scoreBoard.score = scoreBoard_Model.score;
                        scoreBoard.time = DateTime.Now;
                        context.scoreBoards.Update(scoreBoard);
                        context.SaveChanges();
                    }

                    return Ok();
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
