
namespace API.Dtos.Leave
{
    public class HistoryEmployeeDTO
    {
        public string EmpNumber { get; set; }
        public string EmpName { get; set; }
        public string DeptCode { get; set; }
        public int PartID { get; set; }
        public string PartName { get; set; }

        public string DateIn { get; set; }
        public double? TotalDay { get; set; }
        public double? CountTotal { get; set; }
        public double? CountLeave { get; set; }
    }
}