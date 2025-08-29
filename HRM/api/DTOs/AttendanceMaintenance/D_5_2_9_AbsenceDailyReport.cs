
namespace API.DTOs.AttendanceMaintenance;
public partial class AbsenceDailyReportParam
{
    public string Factory { get; set; }
    public string Date { get; set; }
    public string Lang { get; set; }
}

public partial class AbsenceDailyReport
{
    public string Department { get; set; }
    public string Department_Name { get; set; }
    public string Employee_ID { get; set; }
    public string Local_Full_Name { get; set; }
    public string Leave { get; set; }
    public decimal? Days { get; set; }
    public decimal MonthlyLeaveTotal { get; set; }
    public decimal MonthlyAbsentTotal { get; set; }
}

public partial class AbsenceDailyReportTodays
{
    public string Department { get; set; }
    public string Department_Name { get; set; }
    public string Employee_ID { get; set; }
    public string Local_Full_Name { get; set; }
}

public partial class AbsenceDailyReportCount
{
    public decimal QueryResult { get; set; }
    public decimal Recruits { get; set; }
    public decimal Resigning { get; set; }
}
