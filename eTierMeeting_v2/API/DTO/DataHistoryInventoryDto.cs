namespace Machine_API.DTO
{
    public class DataHistoryInventoryDto
    {
        public int ID { get; set; }
        public string MachineID { get; set; }
        public string MachineName { get; set; }
        public string Supplier { get; set; }
        public string Place { get; set; }
        public string State { get; set; }
        public int? StatusInventory { get; set; }
        public int? HistoryInventoryID { get; set; }
    }
}