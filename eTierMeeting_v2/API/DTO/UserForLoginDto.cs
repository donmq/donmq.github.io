namespace eTierV2_API.DTO
{
    public class UserForLoginDto
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
    }
}