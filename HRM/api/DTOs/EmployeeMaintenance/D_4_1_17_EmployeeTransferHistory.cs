namespace API.DTOs.EmployeeMaintenance
{
    public class EmployeeTransferHistoryParam
    {
        public string Division_After { get; set; }
        public string Factory_After { get; set; }
        private string _employee_ID;
        public string Employee_ID_After { get => _employee_ID; set => _employee_ID = value?.Trim(); }
        public string Department_After { get; set; }
        private string _local_Full_Name;
        public string Local_Full_Name { get => _local_Full_Name; set => _local_Full_Name = value?.Trim(); }
        public string USER_GUID { get; set; }
        public DateTime? Effective_Date_Start { get; set; }
        public DateTime? Effective_Date_End { get; set; }
        public string Assigned_Division_After { get; set; }
        public string Assigned_Factory_After { get; set; }
        public string Assigned_Department_After { get; set; }
        private string _assigned_Employee_ID;
        public string Assigned_Employee_ID_After { get => _assigned_Employee_ID; set => _assigned_Employee_ID = value?.Trim(); }
        public string Lang { get; set; }
        public int Effective_Status { get; set; }

    }
    public class EmployeeTransferHistoryDTO
    {
        public string USER_GUID { get; set; }
        public string History_GUID { get; set; }
        public string Data_Source { get; set; }
        public string Data_Source_Name { get; set; }
        public string Nationality_Before { get; set; }
        public string Nationality_After { get; set; }
        public string Identification_Number_Before { get; set; }
        public string Identification_Number_After { get; set; }
        public string Local_Full_Name_Before { get; set; }
        public string Local_Full_Name_After { get; set; }
        public string Division_Before { get; set; }
        public string Division_After { get; set; }
        public string Factory_Before { get; set; }
        public string Factory_After { get; set; }
        public string Employee_ID_Before { get; set; }
        public string Employee_ID_After { get; set; }
        public string Department_Before { get; set; }
        public string Department_After { get; set; }
        public string Assigned_Division_Before { get; set; }
        public string Assigned_Division_After { get; set; }
        public string Assigned_Factory_Before { get; set; }
        public string Assigned_Factory_After { get; set; }
        public string Assigned_Employee_ID_Before { get; set; }
        public string Assigned_Employee_ID_After { get; set; }
        public string Assigned_Department_Before { get; set; }
        public string Assigned_Department_After { get; set; }
        public decimal Position_Grade_Before { get; set; }
        public decimal Position_Grade_After { get; set; }
        public string Position_Title_Before { get; set; }
        public string Position_Title_After { get; set; }
        public string Work_Type_Before { get; set; }
        public DateTime? ActingPosition_Start_Before { get; set; }
        public string ActingPosition_Start_Before_Str { get; set; }
        public DateTime? ActingPosition_End_Before { get; set; }
        public string ActingPosition_End_Before_Str { get; set; }
        public string Work_Type_After { get; set; }
        public DateTime? ActingPosition_Start_After { get; set; }
        public string ActingPosition_Start_After_Str { get; set; }
        public DateTime? ActingPosition_End_After { get; set; }
        public string ActingPosition_End_After_Str { get; set; }
        public string Reason_for_Change { get; set; }
        public int Seq { get; set; }
        public DateTime Effective_Date { get; set; }
        public string Effective_Date_Str { get; set; }
        public bool Effective_Status { get; set; }
        public string Effective_Status_Str { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
    }

    public class EmployeeTransferHistoryDetele
    {
        public string History_GUID { get; set; }
        public bool Effective_Status { get; set; }
    }

    public class EmployeeTransferHistoryEffectiveConfirm
    {
        public string USER_GUID { get; set; }
        public string History_GUID { get; set; }
        public bool Effective_Status { get; set; }
        public string Effective_Date { get; set; }
    }
}