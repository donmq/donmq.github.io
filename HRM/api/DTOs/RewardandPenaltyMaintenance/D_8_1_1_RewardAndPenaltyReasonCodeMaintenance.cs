using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.RewardandPenaltyMaintenance
{
    public class RewardandPenaltyMaintenanceDTO
    {
        public string Factory { get; set; }
        public string Code { get; set; }
        public string Code_Name { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
    }
    public class RewardandPenaltyMaintenanceParam
    {
        public string Factory { get; set; }
        public string Reason_Code { get; set; }

    }
    public class RewardandPenaltyMaintenance_form
    {
        public List<RewardandPenaltyMaintenanceDTO> Data { get; set; }
        public RewardandPenaltyMaintenanceParam  param { get; set; }
    }
}