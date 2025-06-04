using API.Models;

namespace API.Dtos.Report
{
    public class ReportDataDTO
    {
        public long id { get; set; }

        public int? EmpID { get; set; }

        public int? LeaveID { get; set; }

        public DateTime? LeaveDate { get; set; }

        public int? DayOfWeek { get; set; }

        public int? StatusLine { get; set; }

        public int? PartTotalEmp { get; set; }

        public int? DeptTotalEmp { get; set; }

        public int? BuildingTotalEmp { get; set; }

        public int? AreaTotalEmp { get; set; }

        public int? PartTotalEmpByEmp { get; set; }

        public int? DeptTotalEmpByEmp { get; set; }

        public int? BuildingTotalEmpByEmp { get; set; }

        public int? AreaTotalEmpByEmp { get; set; }

        public int? CompTotalEmp { get; set; }

        public int? MPPoolOut { get; set; }

        public int? MPPoolIn { get; set; }

        public int? Total { get; set; }

        public virtual Employee Employee { get; set; }
    }
}