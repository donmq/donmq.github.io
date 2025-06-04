using API.Models;

namespace API.Dtos.Common
{
    public class BrowserInfoDto
    {
        public string IpLocal { get; set; }
        public string Factory { get; set; }
        public LoginDetect LoginDetect { get; set; }
    }
}