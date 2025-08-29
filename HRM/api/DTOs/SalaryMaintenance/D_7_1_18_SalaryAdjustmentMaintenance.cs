namespace API.DTOs.SalaryMaintenance
{

    public class SalaryAdjustmentMaintenanceMain
    {
        public string History_GUID { get; set; }
        public string USER_GUID { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Onboard_Date { get; set; }
        public string Onboard_Date_Str { get; set; }
        public string Employment_Status { get; set; }
        public string Reason_For_Change { get; set; }
        public string Reason_For_Change_Name { get; set; }
        public string Effective_Date { get; set; }
        public string Effective_Date_Str { get; set; }
        public int Seq { get; set; }
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public string Technical_Type { get; set; }
        public string Technical_Type_Name { get; set; }
        public string Expertise_Category { get; set; }
        public string Expertise_Category_Name { get; set; }
        public string Period_of_Acting_Position { get; set; }
        public string Acting_Position_Start { get; set; }
        public string Acting_Position_End { get; set; }
        public decimal Position_Grade { get; set; }
        public string Position_Title { get; set; }
        public string Position_Title_Name { get; set; }
        public string Permission_Group { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }

        public string Salary_Type { get; set; }
        public string Salary_Type_Name { get; set; }
        public decimal Salary_Grade { get; set; }
        public string Salary_Grade_Str { get; set; }
        public decimal Salary_Level { get; set; }
        public string Salary_Level_Str { get; set; }
        public string Currency { get; set; }
        public string Currency_Name { get; set; }
        public string Probation_Salary_Month { get; set; }
        public HistoryDetail Before { get; set; }
        public HistoryDetail After { get; set; }
        public List<SalaryAdjustmentMaintenance_SalaryItem> Salary_Item { get; set; }
        public string Error_Message { get; set; }
        public bool IsEdit { get; set; }

    }

    public class HistoryDetail
    {
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public decimal Position_Grade { get; set; }
        public string Position_Title { get; set; }
        public string Salary_Type { get; set; }
        public string Permission_Group { get; set; }
        public string Technical_Type { get; set; }
        public string Expertise_Category { get; set; }
        public string Acting_Position_Start { get; set; }
        public string Acting_Position_End { get; set; }
        public decimal Salary_Grade { get; set; }
        public decimal Salary_Level { get; set; }
        public List<SalaryAdjustmentMaintenance_SalaryItem> Item { get; set; }

    }

    public class SalaryAdjustmentMaintenance_SalaryItem
    {
        public string Salary_Item { get; set; }
        public string Salary_Item_Name { get; set; }
        public string Salary_Item_NameTW { get; set; }
        public int Amount { get; set; }
    }
    public class TransferHistory
    {
        public int? Seq { get; set; }
        public string ActingPosition_Star_After { get; set; }
        public string ActingPosition_End_After { get; set; }
    }

    public class CheckEffectiveDateResult
    {
        public bool CheckEffectiveDate { get; set; }
        public int MaxSeq { get; set; }
    }

    public class SalaryAdjustmentMaintenance_PersonalDetail
    {
        public string USER_GUID { get; set; }
        public string Division { get; set; }
        public string Local_Full_Name { get; set; }
        public string Onboard_Date { get; set; }
        public string Employment_Status { get; set; }
        public DateTime? Resign_Date { get; set; }
        public HistoryDetail Before { get; set; }
        public HistoryDetail After { get; set; }
    }
    public class SalaryAdjustmentMaintenanceParam
    {
        public string Factory { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public string Onboard_Date { get; set; }
        public string Reason_For_Change { get; set; }
        public string Effective_Date_Start { get; set; }
        public string Effective_Date_End { get; set; }
        public string Lang { get; set; }

    }
}