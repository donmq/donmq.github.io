namespace Machine_API.DTO
{
    public class DetailInventoryDto
    {
        public int TypeInventory { get; set; }
        public string EmpNumber { get; set; }
        public string EmpName { get; set; }
        public int? CountMachine { get; set; }
        public int? CountMatch { get; set; }
        public int? CountWrongPosition { get; set; }
        public int? CountNotScan { get; set; }
        public string PercenMatch { get; set; }
        public DateTime? DateStartInventory { get; set; }
        public DateTime? DateEndInventory { get; set; }
        public DateTime? CreateTime { get; set; }
    }

    public class DetaiHistoryInventoryDto
    {
        public string MachineID { get; set; }
        public string MachineName { get; set; }
        public string MachineName_CN { get; set; }
        public string Supplier { get; set; }
        public string Place { get; set; }
        public string ScanPlace { get; set; }
        public string State { get; set; }
        public int? StatusSoKiem { get; set; }
        public string StatusNameSoKiem { get; set; }
        public int? StatusPhucKiem { get; set; }
        public string StatusNamePhucKiem { get; set; }
        public int? StatusRutKiem { get; set; }
        public string StatusNameRutKiem { get; set; }
        public int? HistoryCheckMachineID { get; set; }
        public int TypeInventory { get; set; }
        public int? StatusInventory { get; set; }
        public DateTime? DateSoKiem { get; set; }
        public DateTime? DatePhucKiem { get; set; }
        public DateTime? DateRutKiem { get; set; }
        public string Plno { get; set; }
        public int? IDSoKiem { get; set; }
        public int? IDPhucKiem { get; set; }
        public int? IDRutKiem { get; set; }
    }

    public class ResultAllInventoryDto
    {
        public List<DetailInventoryDto> ListDetail { get; set; }
        public List<DetaiHistoryInventoryDto> ListResult { get; set; }
        public string TypeFile { get; set; }
    }

}