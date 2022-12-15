using System;

namespace ReferralCodeAPI
{
    public class ScoreBoard_Model
    {
        public string phone_guid {get;set;} 
        public string nickname {get;set;} 
        public int score {get;set;}
        public DateTime time {get;set;}
    }
}