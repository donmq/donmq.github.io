using System;

namespace eTierV2_API.DTO
{
    public class FRI_BA_DefectDTO
    {
        public string Factory_ID { get; set; }
        public string BA_Defect_Desc { get; set; }
        public string Updated_By { get; set; }
        public DateTime Update_Time { get; set; }

        //
        public string Reason_ID { get; set; }
        public decimal Finding_Qty { get; set; }
        public DateTime Data_Date { get; set; }
    }
}