namespace Machine_API.DTO
{
    public class ResultHistoryInventoryDto
    {
        public int HistoryInventoryID { get; set; }
        public int CountSuccess { get; set; }
        public int CountNotScan { get; set; }
        public int CountWrongPosition { get; set; }
        public DateTime? StartTimeInventory { get; set; }
        public DateTime? EndTimeInventory { get; set; }
        public List<SearchMachineInventoryDto> ListInventory { get; set; }
        public int InventoryID { get; set; }
        public string Place { get; set; }
        public int Error { get; set; }
        public string TypeFile { get; set; }
    }
}