namespace API.DTOs.EmployeeMaintenance
{
    public class ContractManagementDto
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Employee_ID { get; set; }
        public int Seq { get; set; }
        public string Contract_Type { get; set; }
        public string Contract_Start { get; set; }
        public string Contract_End { get; set; }
        public bool Effective_Status { get; set; }
        public string Probation_Start { get; set; }
        public string Probation_End { get; set; }
        public string Assessment_Result { get; set; }
        public string Assessment_Result_Name { get; set; }
        public string Extend_to { get; set; }
        public string Reason { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Local_Full_Name { get; set; }
        public string Department { get; set; }
        public DateTime? Onboard_Date { get; set; }
        public string Contract_Title { get; set; }
    }
    public class ContractManagementParam
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string EmployeeID { get; set; }
        public string LocalFullName { get; set; }
        public string Department { get; set; }
        public string ContractType { get; set; }
        public string EffectiveStatus { get; set; }
        public string Onboard_Date_From { get; set; }
        public string Onboard_Date_To { get; set; }
        public string Contract_End_From { get; set; }
        public string Contract_End_To { get; set; }
        public string Probation_End_From { get; set; }
        public string Probation_End_To { get; set; }
        public string Username { get; set; }
        
    }

    public class ProbationParam
    {
        public bool Probationary_Period { get; set; }
        public short? Probationary_Year { get; set; }
        public short? Probationary_Month { get; set; }
        public short? Probationary_Day { get; set; }
    }

    public class Personal
    {
        public string Local_Full_Name { get; set; }
        public string Nationality { get; set; }
        public string Department { get; set; }
        public List<int> Seq { get; set; }
        public DateTime? Onboard_Date { get; set; }
    }

    public class PersonalParam
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string EmployeeID{ get; set; }
        public string Lang { get; set; }
    }
}