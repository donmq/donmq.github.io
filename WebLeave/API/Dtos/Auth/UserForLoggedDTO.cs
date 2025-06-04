using System.Text.Json.Serialization;

namespace API.Dtos.Auth
{
    public class UserForLoggedDto
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public int? UserRank { get; set; }
        public List<LoggedRolesDto> Roles { get; set; }
        [JsonIgnore]
        public string Token { get; set; }
        // [JsonIgnore]
        // public string RefreshToken { get; set; }
    }
}