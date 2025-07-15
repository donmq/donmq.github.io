namespace Machine_API.DTO
{
    public class HistoryInventoryExportPdfDto
    {
        public int HistoryInventoryID { get; set; }
        public string InventoryType { get; set; }
        public string IdPlno { get; set; }
        public string Place { get; set; }
        public string PlaceInventory { get; set; }
        public int? CountComplete { get; set; }
        public int? CountWrongPosition { get; set; }
        public int? CountNotScan { get; set; }
        public string MachineID { get; set; }
        public string MachineName { get; set; }
        public string Supplier { get; set; }
        public string State { get; set; }
        public int? StatusInventory { get; set; }
        public DateTime? DateTime { get; set; }
    }
}