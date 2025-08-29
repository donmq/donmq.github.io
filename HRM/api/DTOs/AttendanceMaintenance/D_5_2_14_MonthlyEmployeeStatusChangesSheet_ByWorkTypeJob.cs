namespace API.DTOs.AttendanceMaintenance
{
    /// <summary>
    /// Param Search for 5.40 Monthly Employee Status Changes Sheet By Work Type Job
    /// </summary>
    public class Monthly_Employee_Status_Changes_Sheet_By_WorkType_Job_Param
    {
        public string Factory { get; set; }
        public string YearMonth { get; set; }
        public string Level { get; set; }
        public List<string> PermisionGroup { get; set; }
        public List<string> Work_Type { get; set; }

        public string Language { get; set; }
        public string PrintBy { get; set; }
    }

    public class Monthly_Employee_Status_Changes_Sheet_By_WorkType_Job_Data_Result
    {
        public List<string> Local_Permission_list { get; set; }
        public List<Monthly_Employee_Status_Changes_Sheet_By_WorkType_Job_Data> Data { get; set; }
        public List<ParentDepartmentModel> TotalGroups { get; set; }
    }

    public class ParentDepartmentModel
    {
        public ParentDepartmentModel() { }

        public ParentDepartmentModel(string deparmtment, string level) { Parent_Department = deparmtment; Parent_Department_Level = level; }
        public ParentDepartmentModel(string deparmtment, string deparmentName, string level)
        {
            Parent_Department = deparmtment;
            Parent_Department_Name = deparmentName;
            Parent_Department_Level = level;
        }
        public string Parent_Department { get; set; }
        public string Parent_Department_Name { get; set; }
        public string Parent_Department_Level { get; set; }
    }

    public class Monthly_Employee_Status_Changes_Sheet_By_WorkType_Job_Data : ParentDepartmentModel
    {
        public Monthly_Employee_Status_Changes_Sheet_By_WorkType_Job_Data() { }

        public Monthly_Employee_Status_Changes_Sheet_By_WorkType_Job_Data(string department, string level)
        {
            Parent_Department = department;
            Parent_Department_Level = level;
        }
        public Monthly_Employee_Status_Changes_Sheet_By_WorkType_Job_Data(string department, string departmentName, string level)
        {
            Parent_Department = department;
            Parent_Department_Name = departmentName;
            Parent_Department_Level = level;
        }

        public string Department { get; set; }
        public string Department_Name { get; set; }
        public string Org_Level { get; set; }
    }

    public class Monthly_Employee_Status_Changes_Sheet_By_WorkType_Job_Excel_Header
    {
        public string Factory { get; set; }
        public string YearMonth { get; set; }
        public string Level { get; set; }
        public List<string> PermisionGroups { get; set; }
        public List<string> Work_Types_EN { get; set; }
        public List<string> Work_Types_TW { get; set; }
        public string PrintBy { get; set; }
        public string PrintDate { get; set; }
    }

    public class Monthly_Employee_Status_Changes_Sheet_By_WorkType_Job_Excel : ParentDepartmentModel
    {
        public Monthly_Employee_Status_Changes_Sheet_By_WorkType_Job_Excel() { }
        public Monthly_Employee_Status_Changes_Sheet_By_WorkType_Job_Excel(Monthly_Employee_Status_Changes_Sheet_By_WorkType_Job_Excel group, ParentDepartmentModel parentDepartment)
        {
            Department = $"{group.Parent_Department_Level} {parentDepartment.Parent_Department}";
            Department_Name = parentDepartment.Parent_Department_Name;
            Direct_Employees = group.Direct_Employees;
            Indirect_Employees = group.Indirect_Employees;
            CodeNameList = group.CodeNameList;
        }

        public Monthly_Employee_Status_Changes_Sheet_By_WorkType_Job_Excel(string department, string departmentName)
        {
            Parent_Department = department;
            Parent_Department_Name = departmentName;
        }
        public Monthly_Employee_Status_Changes_Sheet_By_WorkType_Job_Excel(string department, string departmentName, string level)
        {
            Parent_Department = department;
            Parent_Department_Name = departmentName;
            Parent_Department_Level = level;
        }

        public string Department { get; set; }
        public string Department_Name { get; set; }
        public int Direct_Employees { get; set; }
        public int Indirect_Employees { get; set; }
        public List<int> CodeNameList { get; set; }
    }


    public class MaxEffective_Date
    {
        public string Work_Type_Code { get; set; }
        public DateTime Effective_Date { get; set; }

        public MaxEffective_Date(string work_Type_Code, DateTime effective_Date)
        {
            Work_Type_Code = work_Type_Code;
            Effective_Date = effective_Date;
        }
    }

    public class CodeNameWorkTypePosition
    {
        public string Work_Type_Code { get; set; }
        public int GroupCount { get; set; }
        public CodeNameWorkTypePosition(string work_Type_Code, int groupCount)
        {
            Work_Type_Code = work_Type_Code;
            GroupCount = groupCount;
        }
    }

    public class DepartmentHierarchy
    {
        public string Factory { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Upper_Department { get; set; }
    }
}