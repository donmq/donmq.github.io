
using API.Models;

namespace API.DTOs.SalaryMaintenance
{
    public class MonthlySalaryGenerationExitedEmployees_Param
    {
        public string Factory { get; set; }
        public string Employee_Id { get; set; }
        public string Year_Month { get; set; }
        public string Year_Month_Str { get; set; }
        public List<string> Permission_Group { get; set; }
        public string Lang { get; set; }
        public string Tab_Type { get; set; }
        public int Total_Rows { get; set; }
        public string salary_Lock { get; set; }
        public decimal Salary_Days { get; set; }
    }
    public class MonthlySalaryGenerationExitedEmployees_Temp
    {
        public string USER_GUID { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public DateTime Sal_Month { get; set; }
        public string Employee_ID { get; set; }
        public string Type_Seq { get; set; }
        public string AddDed_Type { get; set; }
        public string Item { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string Department { get; set; }
    }

    public class MonthlySalaryGenerationExitedEmployees_PTemp
    {
        public string USER_GUID { get; set; }
        public string Division { get; set; }
        public string Factory { get; set; }
        public DateTime Sal_Month { get; set; }
        public string Employee_ID { get; set; }
        public string Probation { get; set; }
        public string Type_Seq { get; set; }
        public string AddDed_Type { get; set; }
        public string Item { get; set; }
        public int Amount { get; set; }
    }

    public class MonthlySalaryGenerationExitedEmployees_CRUD
    {

        public string Function { get; set; }
        public string Step { get; set; }
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
        public MonthlySalaryGenerationExitedEmployees_General Data { get; set; }

        public MonthlySalaryGenerationExitedEmployees_CRUD() { }
        public MonthlySalaryGenerationExitedEmployees_CRUD(string function, string step, MonthlySalaryGenerationExitedEmployees_General data)
        {
            Function = function;
            Step = step;
            IsSuccess = true;
            Data = data;
        }
    }
    public class MonthlySalaryGenerationExitedEmployees_General
    {
        public HRMS_Sal_Close HRMS_Sal_Close { get; set; }
        public List<HRMS_Sal_Close> HRMS_Sal_Close_List { get; set; }
        public HRMS_Sal_Resign_Monthly HRMS_Sal_Resign_Monthly { get; set; }
        public List<HRMS_Sal_Resign_Monthly> HRMS_Sal_Resign_Monthly_List { get; set; }
        public HRMS_Sal_Tax HRMS_Sal_Tax { get; set; }

        public MonthlySalaryGenerationExitedEmployees_General() { }

        public MonthlySalaryGenerationExitedEmployees_General(HRMS_Sal_Close hRMS_Sal_Close)
        {
            HRMS_Sal_Close = hRMS_Sal_Close;
        }
        public MonthlySalaryGenerationExitedEmployees_General(List<HRMS_Sal_Close> hRMS_Sal_Close_List)
        {
            HRMS_Sal_Close_List = hRMS_Sal_Close_List;
        }

        public MonthlySalaryGenerationExitedEmployees_General(HRMS_Sal_Resign_Monthly hRMS_Sal_Resign_Monthly)
        {
            HRMS_Sal_Resign_Monthly = hRMS_Sal_Resign_Monthly;
        }
        public MonthlySalaryGenerationExitedEmployees_General(List<HRMS_Sal_Resign_Monthly> hRMS_Sal_Resign_Monthly_List)
        {
            HRMS_Sal_Resign_Monthly_List = hRMS_Sal_Resign_Monthly_List;
        }
        public MonthlySalaryGenerationExitedEmployees_General(HRMS_Sal_Tax hRMS_Sal_Tax)
        {
            HRMS_Sal_Tax = hRMS_Sal_Tax;
        }
    }
    public static class MonthlySalaryGenerationExitedEmployeesExtensions
    {
        public static void Ins_Temp(this List<MonthlySalaryGenerationExitedEmployees_Temp> temp, string USER_GUID, string Division, string Factory, DateTime InputYearMonth, string Employee_ID, string Type_Seq, string AddDed_Type, string Item, int Amount, string Currency, string Department)
        {
            MonthlySalaryGenerationExitedEmployees_Temp data = new()
            {
                USER_GUID = USER_GUID,
                Division = Division,
                Factory = Factory,
                Sal_Month = InputYearMonth,
                Employee_ID = Employee_ID,
                Type_Seq = Type_Seq,
                AddDed_Type = AddDed_Type,
                Item = Item,
                Amount = Amount,
                Currency = Currency,
                Department = Department
            };
            temp.Add(data);
        }

        public static void Ins_PTemp(this List<MonthlySalaryGenerationExitedEmployees_PTemp> pTemp, string USER_GUID, string Division, string Factory, DateTime InputYearMonth, string Employee_ID, string Probation, string Type_Seq, string AddDed_Type, string Item, int Amount)
        {
            MonthlySalaryGenerationExitedEmployees_PTemp data = new()
            {
                USER_GUID = USER_GUID,
                Division = Division,
                Factory = Factory,
                Sal_Month = InputYearMonth,
                Employee_ID = Employee_ID,
                Probation = Probation,
                Type_Seq = Type_Seq,
                AddDed_Type = AddDed_Type,
                Item = Item,
                Amount = Amount
            };
            pTemp.Add(data);
        }

        public static int Query_Sal_Temp(this List<MonthlySalaryGenerationExitedEmployees_Temp> temp, string Factory, DateTime Year_Month, string Employee_ID, string Seq, string Type, string Item)
        {
            var Amount = temp.FirstOrDefault(x =>
                x.Factory == Factory &&
                x.Sal_Month == Year_Month.Date &&
                x.Type_Seq == Seq &&
                x.Employee_ID == Employee_ID &&
                x.AddDed_Type == Type &&
                x.Item == Item
            )?.Amount ?? 0;
            return Amount;
        }
    }
}