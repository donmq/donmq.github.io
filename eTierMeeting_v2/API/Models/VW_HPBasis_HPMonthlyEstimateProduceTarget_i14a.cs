
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API.Models
{
    [Keyless]
    public class VW_HPBasis_HPMonthlyEstimateProduceTarget_i14a
    {
        [Required]
        [StringLength(10)]
        public string Factory_ID { get; set; }
        [Column(TypeName = "date")]
        public DateTime Produce_Date { get; set; }
        [Required]
        [StringLength(3)]
        public string Dept_ID { get; set; }
        [Column(TypeName = "decimal(9, 1)")]
        public decimal? Target_Yield { get; set; }
        [Column(TypeName = "decimal(9, 1)")]
        public decimal? Target_WorkHours { get; set; }
    }
}