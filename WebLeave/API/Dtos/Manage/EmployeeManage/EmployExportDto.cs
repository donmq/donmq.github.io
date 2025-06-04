namespace API.Dtos.Manage.EmployeeManage
{
    public class EmployExportDto
    {
        public int EmpID { get; set; }
        public int? PartID { get; set; }
        public string PartName { get; set; }
        public int DeptID { get; set; }
        public string DeptName { get; set; }
        public string Descript { get; set; }
        public string DeptCode { get; set; }
        public string PartCode { get; set; }
        public string NumberID { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string DateIn { get; set; }
        public string DateIn_By_Lang { get; set; }
        public string Date_In_Edit { get; set; }
        
        
        public string EmpPosition { get; set; }
        public int? PositionID { get; set; }
        public string PositionName { get; set; }

        public int? GBID { get; set; }
        public string EmpGroup { get; set; }
        public double? PhepNam { get; set; }
        public double? PNSapXep_DaNghi { get; set; }
        public double? PNSapXep_ChuaNghi { get; set; }
        public double? PNCaNhan_DaNghi_HP { get; set; }
        public double? PNCaNhan_DaNghi_HeThong { get; set; }
        public double? PNCaNhan_ChuaNghi { get; set; }
        public double? TongPhep_DaNghi { get; set; }
        public double? PNCty { get; set; }
        
        public string VoHieu { get; set; }
        public bool? VoHieuBoolean { get; set; }
        
        public string CreateAccount { get; set; }
        public int? Year { get; set; }
        public string Phep_Nam_Cty { get; set; }
        public string Phep_Nam_Ca_Nhan { get; set; }
       


    }
}