using AutoMapper;

namespace CleanArch.WebApp.Services.Session
{
    public class SessionUser
    {
        public string ID { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
       
    }
}
