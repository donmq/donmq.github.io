namespace API.DTOs.SalaryMaintenance
{
    public class D_7_21_MenstrualLeaveHoursAllowance
    {

    }

    public class MenstrualLeaveHoursAllowanceParam
    {
        public string Factory { get; set; }
        public List<string> Permission_Group { get; set; }
        public string Year_Month { get; set; }
        public string Employee_ID { get; set; }
        public bool Is_Delete { get; set; }
        public string UserName { get; set; }
    }
}