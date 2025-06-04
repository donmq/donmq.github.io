namespace API.Dtos.Report.ReportShow
{
    public class ReportDateDetailDTO
    {
        public string title { get; set; }
        public string lang { get; set; }
        public string leaveDate { get; set; }
        public List<ReportShowModelDTO> listReportShowDateDetail { get; set; }

    }
}