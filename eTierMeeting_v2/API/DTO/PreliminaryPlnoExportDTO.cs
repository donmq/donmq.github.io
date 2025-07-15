namespace Machine_API.DTO
{
    public class PreliminaryPlnoExportDTO
    {
        public string EmpName { get; set; }
        public string EmpNumber { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string ListBuilding { get; set; }
        public string ListCell { get; set; }
        public string ListHpA15 { get; set; }
        public bool Is_Manager{get; set;}
        public bool Is_Preliminary{get; set;}
    }
}