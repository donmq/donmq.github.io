namespace API.DTOs.EmployeeMaintenance
{
    public class ContractManagementReportDto
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public int Seq { get; set; }
        public string Contract_Type { get; set; }
        public string Contract_Type_Name { get; set; }
        public string Document_Type { get; set; }
        public string Document_Type_Name { get; set; }
        public string Salary_Type { get; set; }
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Contract_Start { get; set; }
        public string Contract_End { get; set; }
        public DateTime? Onboard_Date { get; set; }
        public string Probation_Start { get; set; }
        public string Probation_End { get; set; }
        public string Assessment_Result { get; set; }
        public DateTime? Extend_To { get; set; }
        public string Reason { get; set; }
    }
    public class ContractManagementReportParam
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Contract_Type { get; set; }
        public string Salary_Type { get; set; }
        public string Document_Type { get; set; }
        public string Document_Type_Name { get; set; }
        public string Onboard_Date_From { get; set; }
        public string Onboard_Date_To { get; set; }
        public string Contract_End_From { get; set; }
        public string Contract_End_To { get; set; }
        public List<string> Department { get; set; }
        public string Department_From { get; set; }
        public string Department_To { get; set; }
        public string Employee_ID_From { get; set; }
        public string Employee_ID_To { get; set; }
        public string Lang { get; set; }
    }
}