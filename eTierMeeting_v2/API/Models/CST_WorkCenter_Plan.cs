using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API.Models
{
    [Table("CST_WorkCenter_Plan")]
    public partial class CST_WorkCenter_Plan
    {
        [Key]
        [Required]
        public DateTime Work_Date { get; set; }
        [Key]
        [Required]
        [StringLength(20)]
        public string OPER { get; set; }
        [StringLength(10)]
        public string Work_Center { get; set; }
        [Key]
        [Required]
        [StringLength(10)]
        public string Dept_ID { get; set; }
        public int? Plan_Day_Target { get; set; }
        public int? Plan_Hour_Target { get; set; }
        public int? Plan_Grand_Total { get; set; }
        public int? WT_Head { get; set; }
        public int? UTN_Yield_Qty { get; set; }
        public double? UTN_Yield_Rate { get; set; }
        public int? UTN_Defect_Qty { get; set; }
        public double? UTN_RFT { get; set; }
        public int? UTN_Total_Head { get; set; }
        public double? UTN_PPH { get; set; }
        public int? Total_Head_Count { get; set; }
        public double? Working_Hour { get; set; }
        public double? UTN_Total_Minute { get; set; }
        public double? Tag { get; set; }
        [StringLength(50)]
        public string User_ID { get; set; }
        public DateTime? Update_Time { get; set; }
    }
}