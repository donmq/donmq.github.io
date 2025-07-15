namespace Machine_API.DTO
{
    public class PreliminaryPlnoExportAllDTO
    {
        public string EmpName { get; set; }
        public string EmpNumber { get; set; }
        public string UpdateBy { get; set; }
        public string Place { get; set; }
        public string Plno { get; set; }
        public string BuildingName { get; set; }
        public string CellName { get; set; }
        public int? CellID { get; set; }
        public string CellCode { get; set; }
        public int BuildingID { get; set; }
        public string BuildingCode { get; set; }
        public string ListCell { get; set; }
        public string PDCName { get; set; }
        public List<Hp_a15Dto> ListHpA15 { get; set; }
        public bool? Is_Manager { get; set; }
        public bool? Is_Preliminary { get; set; }
        public int maxUserCount { get; set; }
    }
}