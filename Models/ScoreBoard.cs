using System;

namespace ReferralCodeAPI
{
    public class ScoreBoard
    {
        public int Id {get;set;}
        public int referal_id {get;set;}
        public int score {get;set;}
        public DateTime time {get;set;}
    }
}