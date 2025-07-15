namespace Machine_API.Helpers.Params
{
    public class SearchMachineParams
    {
        public string MachineId { get; set; }
        public int? PdcId { get; set; }
        public int? BuildingCode { get; set; }
        public int BuildingId { get; set; }
        public string CellCode { get; set; }
        public string PositionCode { get; set; }
        public string Category { get; set; }
        public string Sort { get; set; }
        public bool IsPaging { get; set; } = true;
    }
}