namespace API.DTOs.SalaryMaintenance
{
    public class D_7_9_Sal_Dept_SAPCostCenter_MappingDTO
    {
        public string Cost_Year { get; set; }
        public string Cost_Year_Str { get; set; }
        public string Factory { get; set; }
        public string Cost_Code { get; set; }
        public string Code_Name { get; set; }
        public string Department { get; set; }
        public string Department_Old { get; set; }
        public string Department_New { get; set; }
        public string Department_Name { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
        public string Error_Message { get; set; }
    }
    public class D_7_9_Sal_Dept_SAPCostCenter_MappingParam
    {
        public string Factory { get; set; }
        public string Year { get; set; }
        public string Year_Str { get; set; }
        public string Department {get; set;}
        public string CostCenter { get; set; }
        public string Language { get; set; }
    }
}