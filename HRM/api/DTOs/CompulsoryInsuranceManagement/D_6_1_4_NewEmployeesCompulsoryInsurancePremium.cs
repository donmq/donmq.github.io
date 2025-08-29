
using API.Models;

namespace API.DTOs.CompulsoryInsuranceManagement
{
    public class NewEmployeesCompulsoryInsurancePremium_Param
    {
        public string Factory { get; set; }
        public string Year_Month_Str { get; set; }
        public string Paid_Salary_Days { get; set; }
        public List<string> Permission_Group { get; set; }
        public string Lang { get; set; }
        public string Function_Type { get; set; }
        public DateTime Year_Month_Date { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
    }
    public class NewEmployeesCompulsoryInsurancePremium_Excel
    {
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public string Employee_ID	 { get; set; }
        public string Local_Full_Name { get; set; }
        public decimal Insured_Salary { get; set; }
        public decimal Paid_Salary_Days { get; set; }
        public decimal Actual_Paid_Days { get; set; }
        public decimal Unemployment_Insurance { get; set; }
        public decimal Social_Insurance { get; set; }
        public decimal Health_Insurance { get; set; }
        public int Amount { get; set; }
        public DateTime Onboard_Date { get; set; }
        public DateTime Print_Date { get; set; }
    }
    public class NewEmployeesCompulsoryInsurancePremium_Calculation
    {
        public List<NewEmployeesCompulsoryInsurancePremium_Excel> Data_Excel { get; set; }
        public List<HRMS_Sal_AddDedItem_Monthly> Data_Insert { get; set; } 
    }
    public class NewEmployeesCompulsoryInsurancePremium_CRUD
    {

        public string Function { get; set; }
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
        public NewEmployeesCompulsoryInsurancePremium_General Data { get; set; }

        public NewEmployeesCompulsoryInsurancePremium_CRUD() { }
        public NewEmployeesCompulsoryInsurancePremium_CRUD(string function, NewEmployeesCompulsoryInsurancePremium_General data)
        {
            Function = function;
            IsSuccess = true;
            Data = data;
        }
    }
    public class NewEmployeesCompulsoryInsurancePremium_General
    {
        public List<HRMS_Sal_AddDedItem_Monthly> HRMS_Sal_AddDedItem_Monthly_List { get; set; }
        public NewEmployeesCompulsoryInsurancePremium_General(List<HRMS_Sal_AddDedItem_Monthly> hRMS_Sal_AddDedItem_Monthly_List)
        {
            HRMS_Sal_AddDedItem_Monthly_List = hRMS_Sal_AddDedItem_Monthly_List;
        }
    }
}