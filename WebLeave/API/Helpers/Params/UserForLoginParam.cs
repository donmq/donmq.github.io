namespace API.Helpers.Params
{
    public class UserForLoginParam
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string IpLocal { get; set; }
        public bool ConfirmReLogin { get; set; }
    }
}