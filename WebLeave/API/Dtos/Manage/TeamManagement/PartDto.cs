namespace API.Dtos.Manage.TeamManagement
{
    public class PartDto
    {
        public int PartID { get; set; }
        public string PartName { get; set; }
        public string PartSym { get; set; }
        public string PartCode { get; set; }
        public int? Number { get; set; }
        public int? DeptID { get; set; }
        public bool? Visible { get; set; }
        public string PartNameVN { get; set; }
        public string PartNameEN { get; set; }
        public string PartNameTW { get; set; }
    }
    public class PartParam
    {
        public string DeptID { get; set; }
        public string PartSym { get; set; }
        public string PartCode { get; set; }
        public string Label_PartName { get; set; }
        public string Label_PartCode { get; set; }
        public string Label_Number { get; set; }
    }
}