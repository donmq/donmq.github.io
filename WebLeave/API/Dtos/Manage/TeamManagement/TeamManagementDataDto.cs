namespace API.Dtos.Manage.TeamManagement
{
    public class TeamManagementDataDto
    {
        public int PartID { get; set; }
        public string PartName { get; set; }
        public string PartSym { get; set; }
        public string PartCode { get; set; }
        public int? Number { get; set; }
        public int? DeptID { get; set; }
        public bool? Visible { get; set; }
        public string DeptName { get; set; }
    }
}