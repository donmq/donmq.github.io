namespace API.Dtos.Report.ReportChart
{
    public class ReportChartDto
    {
        public List<NameLang> names {get;set;}
        public int Id {get;set;}
        public List<ReportChartDto> children {get;set;}
    }

    public class NameLang {
        public string name {get;set;}
        public string lang {get;set;}
    }
}