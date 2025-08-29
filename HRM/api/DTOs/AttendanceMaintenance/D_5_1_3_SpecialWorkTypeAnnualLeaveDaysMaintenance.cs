
namespace API.DTOs.AttendanceMaintenance
{
    public class HRMS_Att_Work_Type_DaysDto
    {
        public string Division { get; set; }

        public string Factory { get; set; }

        public string Work_Type { get; set; }

        public string Annual_leave_days { get; set; }

        public bool Effective_State { get; set; }

        public string Update_By { get; set; }

        public string Update_Time { get; set; }
        public string Status { get; set; }

    }
    public class SpecialWorkTypeAnnualLeaveDaysMaintenanceParam
    {
        public string Division { get; set; }

        public string Factory { get; set; }
        public string Lang { get; set; }
    }
}
