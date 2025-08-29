namespace API.DTOs.SalaryMaintenance
{
    public class D_7_15_ChildcareSubsidyGenerationDto
    {
        public string Department { get; set; }
        public string Factory { get; set; }
        public string DepartmentName { get; set; }
        public decimal NumberOfChildren { get; set; }
        public string EmployeeID { get; set; }
        public string LocalFullName { get; set; }
        public int SubsidyAmount { get; set; }
    }

    public class ChildcareSubsidyGenerationParam
    {
        public string Kind_Tab1 { get; set; }
        public string Kind_Tab2 { get; set; }
        public string Factory { get; set; }
        public List<string> PermissionGroupMultiple { get; set; }
        public string YearMonth { get; set; }
        public string ResignedDate_Start { get; set; }
        public string ResignedDate_End { get; set; }
        public bool Is_Delete { get; set; }
        public string Lang { get; set; }
    }

    public class EmployeeSalaryData
    {
        public string USER_GUID { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public int Amount { get; set; }
    }

    public class ChildcareSubsidyData
    {
        public string USER_GUID { get; set; }
        public string Employee_ID { get; set; }
        public string Permission_Group { get; set; }
        public string Factory { get; set; }
    }
}