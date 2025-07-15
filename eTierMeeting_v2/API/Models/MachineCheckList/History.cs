using System.ComponentModel.DataAnnotations;

namespace Machine_API.Models.MachineCheckList
{
    public class History
    {
        public int HistoryID { get; set; }

        [StringLength(20)]
        public string assnoID { get; set; }

        [StringLength(50)]
        public string Cell_New { get; set; }

        [StringLength(50)]
        public string Cell_Old { get; set; }

        [StringLength(100)]
        public string Position_New { get; set; }

        [StringLength(100)]
        public string Position_Old { get; set; }

        [StringLength(20)]
        public string EmpNumber_Old { get; set; }

        [StringLength(20)]
        public string EmpNumber_New { get; set; }

        [StringLength(20)]
        public string UserID { get; set; }

        public DateTime? Update_Date { get; set; }

        [StringLength(1)]
        public string OwnerFty { get; set; }
    }
}