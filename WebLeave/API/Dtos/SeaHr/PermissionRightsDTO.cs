namespace API.Dtos.SeaHr
{
    public class PermissionRightsDTO
    {
        public int STT { get; set; }
        public string EmpNumber { get; set; }
        public string EmpName { get; set; }
        public string Part { get; set; }
        public string PositionName { get; set; }
        public string ApprovalUsers { get; set; }
    }

    public class PermissionParam
    {
        public string EmpNumber { get; set; }
        public int PartID { get; set; }
        // title excel
        public string Label_Stt { get; set; }
        public string Label_EmpNumber { get; set; }
        public string Label_EmpName { get; set; }
        public string Label_PositionName { get; set; }
        public string Label_Part { get; set; }
        public string Label_ApprovalUsers { get; set; }
    }
}