
namespace API.DTOs.SalaryMaintenance
{
    public class FinSalaryAttributionCategoryMaintenance_Param
    {
        public string Factory { get; set; }
        public string Department { get; set; }
        public string Kind { get; set; }
        public string Kind_Code { get; set; }
        public List<string> Kind_Code_List { get; set; }
        public string Salary_Category { get; set; }
        public string Lang { get; set; }
    }
    public class FinSalaryAttributionCategoryMaintenance_Data
    {
        public string Factory { get; set; }
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public string Department_Code_Name { get; set; }
        public string Kind { get; set; }
        public string Kind_Name { get; set; }
        public string Kind_Code { get; set; }
        public string Kind_Code_Name { get; set; }
        public string Salary_Category { get; set; }
        public string Salary_Category_Name { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
        public string Error_Message { get; set; }
    }
    public class FinSalaryAttributionCategoryMaintenance_Update
    {
        public FinSalaryAttributionCategoryMaintenance_Param Param { get; set; }
        public List<FinSalaryAttributionCategoryMaintenance_Data> Data { get; set; }
    }
}