using API.Models;

namespace API.DTOs.AttendanceMaintenance;
public partial class HRDailyReportParam
{
    public string Factory { get; set; }
    public string Date { get; set; }
    public string Level { get; set; }
    public string Level_Name { get; set; }
    public List<string> PermissionGroups { get; set; }
    public string Lang { get; set; }
}

public partial class HRDailyReportResult
{
    public List<HRDailyReport> HRDailyReport { get; set; }
    public int HeadCount { get; set; }
}

public partial class HRDailyReport
{
    public string GroupBy { get; set; }
    public string Parent_Department_Level { get; set; }
    public string Department { get; set; }
    public string Department_Name { get; set; }
    public int ExpectedAttendance { get; set; }
    public int ExpectedAttendanceExcluding5DaysAbsenteeism { get; set; }
    public int ExpectedAttendanceExcluding5DaysAbsenteeismAndMaternityLeave { get; set; }
    public int Supervisor { get; set; }
    public int Staff { get; set; }
    public int Technicians { get; set; }
    public int WaterSpiders { get; set; }
    public int Assistants { get; set; }
    public int PersonalLeave { get; set; } // Personal_Cnt
    public int UnpaidLeave { get; set; }
    public int SickLeave { get; set; }
    public int Absenteeism { get; set; }
    public int WorkStoppage { get; set; }
    public int AnnualLeaveCompany { get; set; }
    public int AnnualLeaveEmployee { get; set; }
    public int OtherLeave { get; set; }
    public int MaternityLeave { get; set; }
    public int PrenatalCheckupLeave { get; set; }
    public int CompensatoryMaternityLeave { get; set; }
    public int BusinessTrip { get; set; }
    public int ActualAttendance { get; set; }
    public int NewRecruit { get; set; }
    public int Resigned { get; set; }
    public int ExpectedAttendanceTomorrow { get; set; }
    public int ExpectedAttendanceTomorrowExcluding5days { get; set; }
    public int _8HourWorkForPregnantEmployees { get; set; }
    public int _7HourWorkForPregnantEmployees { get; set; }
    public int _7HourWorkForEmployeesWithBabiesUnder12Months { get; set; }
    public int _8HourWorkForEmployeesWithBabiesUnder12Months { get; set; }
    public int Employee_Absences_5Days { get; set; }
    public int TotalPersonalSickAndAbsenteeismLeave { get; set; }
    public double AverageLeaveCountPersonalSickAndAbsenteeism { get; set; }
    public int HeadCount { get; set; }
}

public partial class HRDailyReportCount
{
    public decimal QueryResult { get; set; }
    public decimal HeadCount { get; set; }
    public int MonthlyAbsenteeism { get; set; }
}

public partial class HRDailyReportDept_values_Emp_Cnt
{
    public string Factory { get; set; }
    public string Division { get; set; }
    public string Dept_values { get; set; }
    public int Emp_Cnt { get; set; }
}

public partial class HRDailyReportStaffSum
{
    public string Department { get; set; }
    public int Sum { get; set; }
}

public partial class HRDailyReportD
{
    public string Division { get; set; }
    public string Factory { get; set; }

    public string Department { get; set; }
    public string UpperDepartment { get; set; }
    public string OrgLevel { get; set; }

}

public partial class HRDailyReportTableRequest
{
    public List<string> Employee_Dept { get; set; }
    public List<HRMS_Emp_Personal> HEP { get; set; }
    public List<HRMS_Emp_Personal> HEP_All { get; set; }
    public List<HRMS_Emp_Transfer_History> ETH { get; set; }
    public List<HRMS_Basic_Level> HBL { get; set; }
    public List<HRMS_Att_Leave_Maintain> HLM { get; set; }
    public List<HRMS_Org_Department> HOD { get; set; }
}