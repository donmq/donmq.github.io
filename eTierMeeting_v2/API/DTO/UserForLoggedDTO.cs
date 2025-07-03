using System.Collections.Generic;

namespace eTierV2_API.DTO
{
    public class UserForLoggedDTO
    {
         public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public List<string> Role { get; set; }
    }
}