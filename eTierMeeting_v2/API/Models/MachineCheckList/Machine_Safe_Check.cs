// Generated at 11/28/2024, 2:39:45 PM
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Machine_API.Models.MachineCheckList
{
    [Table("Machine_Safe_Check")]
    public partial class Machine_Safe_Check
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Required]
        public DateTime CheckDate { get; set; }
        [Key]
        [Required]
        [StringLength(25)]
        public string AssnoID { get; set; }
        [Required]
        [StringLength(10)]
        public string Dept_ID { get; set; }
        [Required]
        public int Check_Item { get; set; }
        [Required]
        [StringLength(10)]
        public string Resault { get; set; }
        [StringLength(128)]
        public string Pic_Path { get; set; }
        public DateTime? CreateTime { get; set; }
        [StringLength(255)]
        public string CreateBy { get; set; }
    }
}