namespace API.DTOs.EmployeeMaintenance
{
    public class RehireEvaluationForFormerEmployeesDto
    {
        public RehireEvaluationForFormerEmployeesPersonal Personal { get; set; }
        public RehireEvaluationForFormerEmployeesEvaluation Evaluation { get; set; }
    }
    public class RehireEvaluationForFormerEmployeesPersonal
    {
        public string USER_GUID { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Department { get; set; }
        public string EmployeeID { get; set; }
        public string Local_Full_Name { get; set; }
        public DateTime? Onboard_Date { get; set; }
        public DateTime? Date_of_Resignation { get; set; }
        public string Resign_Type { get; set; }
        public string Resign_Reason { get; set; }
        public bool? Blacklist { get; set; }
        public int Seq { get; set; }
    }
    public class RehireEvaluationForFormerEmployeesEvaluation
    {
        public string Nationality { get; set; }
        public string Identification_Number { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Department { get; set; }
        public string EmployeeID { get; set; }
        public bool Results { get; set; }
        public int Seq { get; set; }
        public string USER_GUID { get; set; }
        public string Explanation { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
    }

    public class RehireEvaluationForFormerEmployeesParam
    {
        public string Nationality { get; set; }
        private string _identification_Number;
        public string Identification_Number { get => _identification_Number; set => _identification_Number = value?.Trim(); }
    }
}