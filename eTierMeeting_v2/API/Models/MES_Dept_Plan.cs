using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    public class MES_Dept_Plan
    {
        public string Factory_ID { get; set; }
        public string Dept_ID { get; set; }
        public DateTime Plan_Date { get; set; }
        [Column(TypeName = "numeric(10, 1)")]
        public decimal? Working_Hour { get; set; }
        public int Plan_Day_Target { get; set; }
    }
}
