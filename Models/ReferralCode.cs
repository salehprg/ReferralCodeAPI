namespace ReferralCodeAPI
{
    public class ReferralCode
    {
        public int Id {get;set;}
        public int? userid {get;set;}
        public string nickname {get;set;}
        public string referralCode {get;set;}
        public string phone_guid {get;set;}
        public bool used {get;set;}
    }
}