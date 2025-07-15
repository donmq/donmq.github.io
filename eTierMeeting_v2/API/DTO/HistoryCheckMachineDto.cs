using System.ComponentModel.DataAnnotations;

namespace Machine_API.DTO
{
    public class HistoryCheckMachineDto
    {
        public int HistoryCheckMachineID { get; set; }
        public int? TotalMachine { get; set; }
        public int? TotalScans { get; set; }
        public int? TotalNotScan { get; set; }
        public int? TotalExist { get; set; }
        public int? TotalNotExist { get; set; }
        public string UserName { get; set; }
        [StringLength(20)]
        public string CreateBy { get; set; }
        public string TypeFile { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}