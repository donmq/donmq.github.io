namespace API.DTOs.RewardAndPenaltyReport
{
    public class EmployeeRewardAndPenaltyReportParam
    {
        public string Factory { get; set; }
        public string Start_Year_Month { get; set; }
        public string End_Year_Month { get; set; }
        public List<string> Permission_Group { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public string Start_Date { get; set; }
        public string End_Date { get; set; }
        public string RewardPenaltyType { get; set; }
        public string Counts { get; set; }
        public string UserName { get; set; }
        public string Language { get; set; }
    }

    public class EmployeeRewardAndPenaltyReportDto
    {
        public string History_GUID { get; set; }
        public string USER_GUID { get; set; }
        public string Factory { get; set; }
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public string Employee_ID { get; set; }
        public string LocalFullName { get; set; }
        public DateTime Date { get; set; }
        public string Reward_Type { get; set; }
        public string Reason_Code { get; set; }
        public DateTime? Year_Month { get; set; }
        public int CountsOf { get; set; }
        public string Remark { get; set; }
    }
}