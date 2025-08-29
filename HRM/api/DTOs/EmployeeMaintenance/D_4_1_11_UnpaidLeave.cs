namespace API.DTOs.EmployeeMaintenance
{
    public class UnpaidLeaveParam
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Onboard_Date { get; set; }
        public string Leave_Reason { get; set; }
        public string Leave_Start { get; set; }
        public string Leave_Start_From { get; set; }
        public string Leave_Start_To { get; set; }
        public string Leave_End { get; set; }
        public string Leave_End_From { get; set; }
        public string Leave_End_To { get; set; }
        public string Lang { get; set; }
        public List<string> Role_List { get; set; }
    }

    public class AddAndEditParam : UnpaidLeaveParam
    {
        public int Ordinal_Number { get; set; }
        public bool Continuation_of_Insurance { get; set; }
        public bool Seniority_Retention { get; set; }
        public bool Annual_Leave_Seniority_Retention { get; set; }
        public bool Effective_Status { get; set; }
        public string Remark { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
    }
    public partial class HRMS_Emp_Unpaid_LeaveDto
    {
        public string Division { get; set; }
        public string Division_Str { get; set; }
        public string Factory { get; set; }
        public string Factory_Str { get; set; }
        public string Employee_ID { get; set; }
        public string Department { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Local_Full_Name { get; set; }
        public int Ordinal_Number { get; set; }
        public string Leave_Reason { get; set; }
        public string Leave_Reason_Str { get; set; }
        public string Onboard_Date { get; set; }
        public string Leave_Start { get; set; }
        public string Leave_End { get; set; }
        public bool Continuation_of_Insurance { get; set; }
        public string Continuation_of_Insurance_Str { get; set; }
        public bool Seniority_Retention { get; set; }
        public string Seniority_Retention_Str { get; set; }
        public bool Annual_Leave_Seniority_Retention { get; set; }
        public string Annual_Leave_Seniority_Retention_Str { get; set; }
        public bool Effective_Status { get; set; }
        public string Effective_Status_Str { get; set; }
        public string Remark { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Download_By { get; set; }
        public string Download_Time { get; set; }
    }
}
