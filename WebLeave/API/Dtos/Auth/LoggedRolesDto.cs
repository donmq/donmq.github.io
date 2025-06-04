
using System.Text.Json.Serialization;

namespace API.Dtos.Auth
{
    public partial class LoggedRolesDto
    {
        public string RoleSym { get; set; }
        public string Route { get; set; }
        public string ImgSrc { get; set; }
        [JsonIgnore]
        public bool Status { get; set; }
        public int Badge { get; set; }
        public List<LoggedRolesDto> SubRoles { get; set; }
    }
}