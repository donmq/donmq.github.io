using System.ComponentModel.DataAnnotations;

namespace Machine_API.Models.MachineCheckList
{
    public partial class hp_a04
    {
        [Key]
        [StringLength(12)]
        public string Main_Asset_Number { get; set; }
        [Key]
        [StringLength(25)]
        public string AssnoID { get; set; }
        [StringLength(18)]
        public string Spec { get; set; }
        [StringLength(10)]
        public string Dept_ID { get; set; }
        [StringLength(10)]
        public string Plno { get; set; }
        [StringLength(10)]
        public string State { get; set; }
        [StringLength(50)]
        public string MachineName_EN { get; set; }
        [StringLength(50)]
        public string MachineName_Local { get; set; }
        [StringLength(50)]
        public string MachineName_CN { get; set; }
        [StringLength(8)]
        public string Trdate { get; set; }
        public bool? Visible { get; set; }
        [StringLength(10)]
        public string Supplier { get; set; }
        [Key]
        [StringLength(1)]
        public string OwnerFty { get; set; }
        [StringLength(4)]
        public string Company_Code { get; set; }
        [StringLength(8)]
        public string Askid { get; set; }
        public bool? Is_Update_To_SAP { get; set; }
    }
}