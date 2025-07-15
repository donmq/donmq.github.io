namespace Machine_API.DTO
{
    public class CellDto
    {
        public int CellID { get; set; }
        public string CellCode { get; set; }
        public string CellName { get; set; }
        public int? PDCID { get; set; }
        public string PDCName { get; set; }
        public int BuildingID { get; set; }
        public string BuildingName { get; set; }
        public string BuildingCode { get; set; }
        public bool? Visible { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}