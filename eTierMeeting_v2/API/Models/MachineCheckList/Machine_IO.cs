// Generated at 7/15/2025, 10:11:28 AM
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Machine_API.Models.MachineCheckList
{
    [Table("Machine_IO")]
    public partial class Machine_IO
    {
        [Key]
        [Required]
        [StringLength(20)]
        public string AssnoID { get; set; }
        [StringLength(100)]
        public string Spec { get; set; }
        [StringLength(10)]
        public string Dept_ID { get; set; }
        [StringLength(20)]
        public string Plno { get; set; }
        [StringLength(10)]
        public string State { get; set; }
        [StringLength(100)]
        public string MachineName_EN { get; set; }
        [StringLength(100)]
        public string MachineName_Local { get; set; }
        [StringLength(100)]
        public string MachineName_CN { get; set; }
        [StringLength(100)]
        public string Supplier { get; set; }
        [Key]
        [Required]
        [StringLength(1)]
        public string OwnerFty { get; set; }
        [Required]
        [StringLength(1)]
        public string UsingFty { get; set; }
        [Required]
        [StringLength(1)]
        public string IO_Kind { get; set; }
        [StringLength(100)]
        public string IO_Reason { get; set; }
        [Required]
        public DateTime IO_Date { get; set; }
        [Required]
        [StringLength(1)]
        public string IO_Confirm { get; set; }
        [Required]
        public DateTime Re_Date { get; set; }
        [Required]
        [StringLength(1)]
        public string Re_Confirm { get; set; }
        [StringLength(100)]
        public string Remark { get; set; }
        [StringLength(10)]
        public string Insert_By { get; set; }
        public DateTime? Insert_At { get; set; }
        [StringLength(10)]
        public string Update_By { get; set; }
        public DateTime? Update_At { get; set; }
    }
}