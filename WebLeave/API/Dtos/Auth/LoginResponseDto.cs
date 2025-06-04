namespace API.Dtos.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public UserForLoggedDto User { get; set; }
        public bool AlreadyLoggedIn { get; set; }
    }
}