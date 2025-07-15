namespace Machine_API.DTO
{
    public class HistoryInventoryDto
    {
        public int HistoryInventoryID { get; set; }
        public int? InventoryType { get; set; }
        public string IdPlno { get; set; }
        public string Place { get; set; }
        public int? CountComplete { get; set; }
        public int? CountWrongPosition { get; set; }
        public int? CountNotScan { get; set; }
        public string UserName { get; set; }
        public string EmpName { get; set; }
        public DateTime? StartTimeInventory { get; set; }
        public DateTime? EndTimeInventory { get; set; }
        public DateTime? DateTime { get; set; }
        public string TypeFile { get; set; }
    }
}