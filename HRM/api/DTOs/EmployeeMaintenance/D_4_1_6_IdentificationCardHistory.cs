namespace API.DTOs.EmployeeMaintenance
{
    public class HRMS_Emp_Identity_Card_HistoryDto
    {

        public string Nationality_Before { get; set; }
        public string USER_GUID { get; set; }
        public string History_GUID { get; set; }
        public string Identification_Number_Before { get; set; }
        public string Local_Full_Name { get; set; }
        public DateTime Issued_Date_Before { get; set; }
        public string Nationality_After { get; set; }
        public string Identification_Number_After { get; set; }
        public DateTime Issued_Date_After { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
    }


    public class HRMS_Emp_Identity_Card_HistoryParam
    {
        private string _identification_Number;
        public string Nationality { get; set; }
        public string Identification_Number { get => _identification_Number; set => _identification_Number = value?.Trim(); }
    }

    public class HRMS_Emp_PersonalView
    {
        public string Nationality { get; set; }

        public string Identification_Number { get; set; }

        public DateTime Issued_Date { get; set; }
    }

}