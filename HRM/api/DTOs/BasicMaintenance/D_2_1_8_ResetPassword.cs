namespace API.DTOs.BasicMaintenance
{
    public class ResetPasswordDto
    {

    }

    public class ResetPasswordParam
    {
        public string Account { get; set; }
        public string NewPassword { get; set; }
    }
}