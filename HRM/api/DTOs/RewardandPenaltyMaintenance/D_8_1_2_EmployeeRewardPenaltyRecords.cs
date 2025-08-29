namespace API.DTOs.RewardandPenaltyMaintenance
{
    public class D_8_1_2_EmployeeRewardPenaltyRecordsParam
    {
        public string Factory { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public string Date_Start_Str { get; set; }
        public string Date_End_Str { get; set; }
        public string Yearly_Month_Start_Str { get; set; }
        public string Yearly_Month_End_Str { get; set; }
        public string Language { get; set; }
    }
    public class D_8_1_2_EmployeeRewardPenaltyRecordsData
    {
        public string History_GUID { get; set; }
        public string Factory { get; set; }
        public string Division { get; set; }
        public string USER_GUID { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Department_Code { get; set; }
        public string Department_Code_Name { get; set; }
        public string Work_Type { get; set; }
        public string Work_Type_Name { get; set; }
        public DateTime Reward_Date { get; set; }
        public string Reward_Date_Str { get; set; }
        public string Reward_Penalty_Type { get; set; }
        public string Reward_Penalty_Type_Name { get; set; }
        public string Reason_Code { get; set; }
        public string Reason_Code_Name { get; set; }
        public DateTime? Yearly_Month { get; set; }
        public string Yearly_Month_Str { get; set; }
        public short Counts_of { get; set; }
        public string Remark { get; set; }
        public string SerNum { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
    }
    public class D_8_1_2_EmployeeRewardPenaltyRecordsSubParam : D_8_1_2_EmployeeRewardPenaltyRecordsData
    {
        public List<EmployeeRewardPenaltyRecordsReportFileModel> File_List { get; set; }
    }

    public class D_8_1_2_EmployeeRewardPenaltyRecordsReport
    {
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Date { get; set; }
        public string Reward_Penalty_Type { get; set; }
        public string Reason_Code { get; set; }
        public string Yearly_Month { get; set; }
        public short Counts_of { get; set; }
        public string Remark { get; set; }
        public string IsCorrect { get; set; }
        public string Error_Message { get; set; }
    }
    public class EmployeeRewardPenaltyRecordsReportFileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int Size { get; set; }
    }
    public class EmployeeRewardPenaltyRecordsReportDownloadFileModel
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string SerNum { get; set; }
        public string Employee_Id { get; set; }
        public string File_Name { get; set; }
    }
    public class EmployeeRewardPenaltyRecordsReportTypeheadKeyValue
    {
        public string Key { get; set; }
        public string USER_GUID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Department { get; set; }
        public string Work_Shift_Type { get; set; }
    }
}