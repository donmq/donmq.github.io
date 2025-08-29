using API.Models;

namespace API.DTOs.SalaryMaintenance
{
    public class SalaryAdditionsAndDeductionsInputDto
    {
        public string USER_GUID { get; set; }
        public string Factory { get; set; }
        public string Factory_Dept { get; set; }
        public string Division { get; set; }
        public DateTime Sal_Month { get; set; }
        public string Sal_Month_Str { get; set; }
        public string Department_Name { get; set; }
        public string Department_Code { get; set; }
        public string Department_Code_Name { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string AddDed_Type { get; set; }
        public string AddDed_Type_Str { get; set; }
        public string AddDed_Item { get; set; }
        public string AddDed_Item_Str { get; set; }
        public string Currency { get; set; }
        public string Currency_Str { get; set; }
        public int Amount { get; set; }
        public DateTime? Resign_Date { get; set; }
        public string Resign_Date_Str { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Update_Time_Str { get; set; }
    }

    public class SalaryAdditionsAndDeductionsInput_Param
    {
        public string Factory { get; set; }
        public string Sal_Month { get; set; }
        public string AddDed_Type { get; set; }
        public string AddDed_Item { get; set; }
        public string Employee_ID { get; set; }
        public string Department { get; set; }
        public string Language { get; set; }
    }

    public class SalaryAdditionsAndDeductionsInput_Upload
    {
        public IFormFile File { get; set; }
        public string Language { get; set; }
    }

    public class SalaryAdditionsAndDeductionsInput_Personal
    {
        public string USER_GUID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Factory { get; set; }
        public string Division { get; set; }
        public string Employee_ID { get; set; }
        public string Department { get; set; }
    }

      public class SalaryAdditionsAndDeductionsInput_Report : HRMS_Sal_AddDedItem_Monthly
    {
        public string Error_Message { get; set; }
        public string IsCorrect { get; set; }
        public string AmountStr { get; set; }
        public string Sal_MonthStr { get; set; }
    }
}