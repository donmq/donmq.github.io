using System.ComponentModel.DataAnnotations;

namespace Machine_API.Models.MachineCheckList
{
    public class ErrorLog
    {
        public int ID { get; set; }

        [StringLength(100)]
        public string LogType { get; set; }

        public string Content { get; set; }

        public DateTime? DateLog { get; set; }
        
    }
}