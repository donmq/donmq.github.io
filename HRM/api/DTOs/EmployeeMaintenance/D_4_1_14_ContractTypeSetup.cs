namespace API.DTOs.EmployeeMaintenance
{
    public class ContractTypeSetupDto
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Contract_Type { get; set; }
        public string Contract_Type_After { get; set; }
        public string Contract_Title { get; set; }
        public bool Probationary_Period { get; set; }
        public string Probationary_Period_Str { get; set; }
        public string Alert_Str { get; set; }
        public short? Probationary_Year { get; set; }
        public short? Probationary_Month { get; set; }
        public short? Probationary_Day { get; set; }
        public bool Alert { get; set; }
        public int? Seq { get; set; }
        public string Schedule_Frequency { get; set; }
        public short? Day_Of_Month { get; set; }
        public string Alert_Rules { get; set; }
        public short? Days_Before_Expiry_Date { get; set; }
        public short? Month_Range { get; set; }
        public short? Contract_Start { get; set; }
        public short? Contract_End { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public List<HRMSEmpContractTypeDetail> dataDetail { get; set; }
    }

    public class HRMSEmpContractTypeDetail
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Contract_Type { get; set; }
        public int Seq { get; set; }
        public string Schedule_Frequency { get; set; }
        public short? Day_Of_Month { get; set; }
        public string Alert_Rules { get; set; }
        public short? Days_Before_Expiry_Date { get; set; }
        public short Month_Range { get; set; }
        public short? Contract_Start { get; set; }
        public short? Contract_End { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
    }

    public class ContractTypeSetupParam
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Contract_Type { get; set; }
        public string Probationary_Period_Str { get; set; }
        public string Alert_Str { get; set; }
        public string Lang { get; set; }
    }
}