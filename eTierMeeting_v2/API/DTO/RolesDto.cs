namespace Machine_API.DTO
{
    public class RolesDto
    {
        public int ID { get; set; }
        public string RoleName { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int? RoleSequence { get; set; }
        public bool? Checked { get; set; }
        public bool? Visible { get; set; }
    }
}