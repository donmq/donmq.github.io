using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.DTO.Production.T2.CTB
{
    public class SefetyViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public List<EvaluetionCategory> Evaluetions { get; set; }
        public List<string> Lines { get; set; }
    }

    public class EvaluetionCategory
    {
        public string Item_ID { get; set; }
        public string Item_Name { get; set; }
        public string Item_Name_LL { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public string Target { get; set; }
        public List<Achievement> Achievements { get; set; }
    }

    public class Achievement
    {
        public int HSE_Score_ID { get; set; }
        public string Item_ID { get; set; }
        public string Line_Sname { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public string Score { get; set; }
        public bool IsPass { get; set; }
    }
}