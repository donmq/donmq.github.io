using System.ComponentModel.DataAnnotations;

namespace eTierV2_API.Models
{
    public class eTM_HP_G01_Flag
    {
        [StringLength(5)]
        public string Factory_ID { get; set; }
        [StringLength(5)]
        public string Dept_ID { get; set; }
    }
}