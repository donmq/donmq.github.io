namespace API.Dtos.SeaHr
{
    public class AllowLeaveSundayDto
    {
        public int EmpID { get; set; }
        public string PartName { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public string EmpName { get; set; }
        public string EmpNumber { get; set; }
        public bool? IsSun { get; set; }

    }

    public class AllowLeaveSundayParam
    {
        public List<int> dataSelected { get; set; }
        public List<int> dataAll { get; set; }
        public int? PartId { get; set; }
        public string Keyword { get; set; }
    }
}