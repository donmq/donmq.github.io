namespace Machine_API.DTO
{
    public class ReportMachineDto
    {
        public List<ReportMachineItem> ListReportMachineItem { get; set; }
        public int? TotalIdle { get; set; }
        public int? TotalInuse { get; set; }
    }

    public class ReportMachineItem
    {
        public int? BuildingID { get; set; }

        public string BuildingName { get; set; }
        public int? InUse { get; set; }

        public int? Idle { get; set; }
    }
}