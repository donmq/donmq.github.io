
namespace API.Dtos.Report.ReportShow
{
    public class ReportIndexViewModelDTO
    {
        public string TitleExportExcel { get; set; }
        public GetTitleByLang Title { get; set; }
        public string Lang { get; set; }
        public DateTime? StartDay { get; set; }
        public DateTime? EndDay { get; set; }
        public List<ReportShowModelDTO> ListReportShowModel { get; set; }
        public List<List<ReportShowModelDTO>> ListParent { get; set; }
    }
}