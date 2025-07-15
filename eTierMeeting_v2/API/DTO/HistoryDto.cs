using System.ComponentModel.DataAnnotations.Schema;

namespace Machine_API.DTO
{
    public class HistoryDto
    {
        public int HistoryID { get; set; }
        public string assnoID { get; set; }
        public string Cell_New { get; set; }
        public string Cell_Old { get; set; }
        public string Position_New { get; set; }
        public string Position_Old { get; set; }
        public string EmpNumber_Old { get; set; }
        public string EmpNumber_New { get; set; }
        public string UserID { get; set; }
        public DateTime? Update_Date { get; set; }
        public string OwnerFty { get; set; }

        //////
        public string EmpName { get; set; }
        public string PDCCode { get; set; }
        public string BuildingCode { get; set; }
        public string MachineName_CN { get; set; }
        public string Place_New { get; set; }
        public string Place_Old { get; set; }
        public string CellName_New { get; set; }
        public string CellName_Old { get; set; }

        //Phân trang trong store procedure
        [NotMapped] // Ko map data từ Store sang Model
        public int? TotalRows { get; set; }
    }
}