
namespace Machine_API.Helpers.Params
{
    public class SearchHistoryParams
    {
        public string MachineId { get; set; }
        public int? PdcId { get; set; }
        public int? BuildingCode { get; set; }
        public int? CellCode { get; set; }
        public string PositionCode { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string Sort { get; set; }
        public bool IsPaging { get; set; } = true;
    }
}