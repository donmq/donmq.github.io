using API.Models;

namespace API.Dtos.Report.ReportShow
{
    public class ModelDTO
    {
        public Employee Employee { get; set; }
        public Part Part { get; set; }
        public Department Department { get; set; }
        public Building Building { get; set; }
    }
}