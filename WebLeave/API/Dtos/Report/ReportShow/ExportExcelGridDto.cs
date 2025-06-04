namespace API.Dtos.Report.ReportShow
{
    public class ExportExcelGridDto
    {
        public string LeaveDate { get; set; }
        public string SEAMP { get; set; }
        public string Applied { get; set; }
        public string Approved { get; set; }
        public string Actual { get; set; }
        public string MPPoolOut { get; set; }
        public string MPPoolIn { get; set; }
        public string Total { get; set; }
        public ReportIndexViewModelDTO ReportIndexViewModelDTO { get; set; }

    }


}