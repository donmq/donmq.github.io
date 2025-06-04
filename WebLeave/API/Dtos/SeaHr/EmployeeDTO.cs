namespace API.Dtos.SeaHr
{
    public class EmployeeDTO
    {
        public int EmpID { get; set; }
        public string EmpName { get; set; }
        public string EmpNumber { get; set; }
        public string DateIn { get; set; }
        public int? PositionID { get; set; }
        public string Descript { get; set; }
        public int? GBID { get; set; }
        public bool? Visible { get; set; }
        public int? PartID { get; set; }
        public int HisrotyID { get; set; }
        public int? YearIn { get; set; }
        public double? TotalDay { get; set; }
        public double? Arrange { get; set; }
        public double? Agent { get; set; }
        public double? CountArran { get; set; }
        public double? CountRestArran { get; set; }
        public double? CountAgent { get; set; }
        public double? CountRestAgent { get; set; }
        public double? CountTotal { get; set; }
        public double? CountLeave { get; set; }

        public int UserID { get; set; }
        public string UserName { get; set; }
        public string HashPass { get; set; }
        public string HashImage { get; set; }
        public string EmailAddress { get; set; }
        public int? UserRank { get; set; }
        public bool? ISPermitted { get; set; }
        public DateTime? Updated { get; set; }
        public string FullName { get; set; }

        public bool IsCreateAccount { get; set; }
    }
    public class AddHistoryEmp
    {
        public int EmpID { get; set; }
        public double Leave { get; set; }
        public int? Year { get; set; }
        public double? CountArrange { get; set; }
        public double? CountRestArrange { get; set; }
        public double? CountAgent { get; set; }
        public double? CountRestAgent { get; set; }
        public double? CountLeave { get; set; }
        public string Factory { get; set; }
        public string EmpNumber { get; set; }
        public string EmpName { get; set; }
        public string Email { get; set; }
        public DateTime DateIn { get; set; }
        public DateTime Updated { get; set; }
        public int Disable { get; set; }
        public int CreateUser { get; set; }
        public int? PartID { get; set; }
        public int? PositionID { get; set; }
        public int? GBID { get; set; }
        public double Arrange { get; set; }
        public double Agent { get; set; }
    }

    public class ResultDataUploadEmp
    {
        public string Ignore { get; set; }
        public int CountCreateEmp { get; set; }
        public int CountUpdateEmp { get; set; }
        public int TotalEmp { get; set; }
    }
}