namespace Journey.Api.Models
{
    public class CaptchaVerification
    {
        public bool Success { get;set; }
        public double Score { get;set; }  
        public string Action { get;set; }
    }
}
