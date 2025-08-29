using API.Models;

namespace API.DTOs.AttendanceMaintenance
{
    /// <summary>
    /// Param Search for 5.41 Monthly Employee Status Changes Sheet By Reason for Resignation
    /// </summary>
    public class Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Param
    {
        public string Factory { get; set; }
        public string YearMonth { get; set; }
        public List<string> PermisionGroups { get; set; }
        public string Language { get; set; }

        public string PrintBy { get; set; }
    }

    public class Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Query_Param
    {
        public string Factory { get; set; }
        public List<string> PermissionGroups { get; set; }
        public List<HRMS_Emp_Personal> Personals { get; set; }
        public List<HRMS_Emp_IDcard_EmpID_History> EmpIdHistory { get; set; }
        public DateTime FirstDateOfMonth { get; set; }
        public DateTime LastDateOfMonth { get; set; }
        public List<Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Dept> EmployeeDept { get; set; }
        public List<string> Local_Permission_list { get; set; }
    }

    public class Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Dept
    {
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public string Org_Level { get; set; }
    }

    public class Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Result
    {
        public int TotalRecords { get; set; }
        public List<Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Value> Data { get; set; }
        public List<Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Excel> Exports { get; set; }
    }

    public class Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Value
    {
        /// <summary>
        /// Tiêu đề 
        /// </summary>
        /// <value></value>
        public string HeaderTitle { get; set; }

        /// <summary>
        /// Số lượng nhân viên hiện tại
        /// </summary>
        /// <value></value>
        public int NumberOfEmployeesAt { get; set; }

        /// <summary>
        /// Số lượng thuê lại nhân viên trong tháng này
        /// </summary>
        /// <value></value>
        public int NewHiresThisMonth { get; set; }

        /// <summary>
        /// Số lượng nhân viên mới trong tháng này
        /// </summary>
        /// <value></value>
        public int ResignationsThisMonth { get; set; }
        public int TotalNumberOfEmployeesAt { get; set; }
    }

    public class Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Excel_Header
    {
        public string Factory { get; set; }
        public string YearMonth { get; set; }
        public List<string> PermisionGroups { get; set; }
        public string PrintBy { get; set; }
        public string PrintDate { get; set; }
    }

    public class Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Excel
    {
        /// <summary>
        /// Lý do nghỉ việc 
        /// </summary>
        /// <value></value>
        public string ReasonForResignation { get; set; }

        /// <summary>
        /// Tổng số lượng nghỉ việc
        /// </summary>
        /// <value></value>
        public int TotalNumberOfResignations { get; set; }

        /// <summary>
        /// Số lượng từ chức không ký HDLD
        /// </summary>
        /// <value></value>
        public int NumberOfResignationsWithoutSignedLaborContracts { get; set; }

        /// <summary>
        /// Số lượng xin việc có ký HDLD
        /// </summary>
        /// <value></value>
        public int NumberOfResignationsWithSignedLaborContracts { get; set; }
    }
}
