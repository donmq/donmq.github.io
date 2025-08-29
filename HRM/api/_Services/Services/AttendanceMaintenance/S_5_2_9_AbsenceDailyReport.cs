using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance;
public class S_5_2_9_AbsenceDailyReport : BaseServices, I_5_2_9_AbsenceDailyReport
{
    public S_5_2_9_AbsenceDailyReport(DBContext dbContext) : base(dbContext)
    {
    }

    public async Task<AbsenceDailyReportCount> GetTotalRows(AbsenceDailyReportParam param, List<string> roleList)
    {
        List<AbsenceDailyReport> data = await GetData(param, roleList);
        // Recruits
        List<AbsenceDailyReportTodays> dataRecruits = await GetDataTodays(param, roleList, "Onboard");
        // Resigning
        List<AbsenceDailyReportTodays> dataResigning = await GetDataTodays(param, roleList, "Resigning");
        return new AbsenceDailyReportCount { QueryResult = data.Count, Recruits = dataRecruits.Count, Resigning = dataResigning.Count };
    }

    public async Task<OperationResult> DownloadExcel(AbsenceDailyReportParam param, List<string> roleList, string userName)
    {
        List<AbsenceDailyReport> data = await GetData(param, roleList);

        // Recruits
        List<AbsenceDailyReportTodays> dataRecruits = await GetDataTodays(param, roleList, "Onboard");
        // Resigning
        List<AbsenceDailyReportTodays> dataResigning = await GetDataTodays(param, roleList, "Resigning");
        if (!data.Any() && !dataRecruits.Any() && !dataResigning.Any())
            return new OperationResult(false, "No Data");

        List<Table> tables = new()
        {
            new Table("result", data),
            new Table("resultRecruits", dataRecruits),
            new Table("resultResigning", dataResigning)
        };
        List<Cell> cells = new()
        {
            new Cell("B1", param.Factory),
            new Cell("E1", param.Date),
            new Cell("B3", userName),
            new Cell("D3", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")),
        };
        ConfigDownload config = new() { IsAutoFitColumn = true };
        ExcelResult excelResult = ExcelUtility.DownloadExcel(
            tables, 
            cells, 
            "Resources\\Template\\AttendanceMaintenance\\5_2_9_AbsenceDailyReport\\Download.xlsx", 
            config
        );
        return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
    }

    #region get Data
    private async Task<List<AbsenceDailyReport>> GetData(AbsenceDailyReportParam param, List<string> roleList)
    {
        // Initialize the predicates
        ExpressionStarter<HRMS_Emp_Personal> predEmpPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true);
        ExpressionStarter<HRMS_Att_Leave_Maintain> leavePred = PredicateBuilder.New<HRMS_Att_Leave_Maintain>();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(param.Factory))
        {
            predEmpPersonal.And(x => x.Factory == param.Factory);
            leavePred.And(x => x.Factory == param.Factory);
        }

        if (!string.IsNullOrWhiteSpace(param.Date))
        {
            leavePred.And(x => x.Leave_Date == Convert.ToDateTime(param.Date));
        }

        // Query the leave maintain records
        List<HRMS_Att_Leave_Maintain> leave_Maintains = await _repositoryAccessor.HRMS_Att_Leave_Maintain.FindAll(leavePred, true).ToListAsync();

        // Fetch the filtered employee data
        List<HRMS_Emp_Personal> HEP = await Query_Permission_Data_Filter_Factory(roleList, predEmpPersonal);

        // Perform the join operation between leave_Maintains and HEP
        var joinedData = leave_Maintains
        .Join(
            HEP,
            leave => leave.USER_GUID,
            personal => personal.USER_GUID,
            (leave, personal) => new { Leave = leave, Personal = personal }
        );

        // Prepare the result list
        List<AbsenceDailyReport> absenceDailyReports = new();

        foreach (var item in joinedData)
        {
            HRMS_Att_Leave_Maintain leaveMain = item.Leave;
            HRMS_Emp_Personal personal = item.Personal;

            // Fetch department report asynchronously
            (string department_Code, string department_Name) = await Query_Department_Report(personal.Factory, personal.Department, param.Lang);

            // Fetch leave codes asynchronously
            string leave_Codes = await GetListBasicCode(BasicCodeTypeConstant.Leave, leaveMain.Leave_code, param.Lang);

            // Calculate leave and absence totals asynchronously
            (decimal monthlyLeaveTotal, decimal monthlyAbsentTotal) = await CalculateLeaveAndAbsenceTotals(leaveMain.Leave_Date, personal.Factory, personal.Employee_ID, leaveMain.Leave_code);

            // Add the new AbsenceDailyReport to the list
            absenceDailyReports.Add(new AbsenceDailyReport
            {
                Department = department_Code,
                Department_Name = department_Name,
                Employee_ID = leaveMain.Employee_ID ?? string.Empty,
                Local_Full_Name = personal.Local_Full_Name ?? string.Empty,
                Leave = leave_Codes,
                Days = leaveMain.Days,
                MonthlyAbsentTotal = monthlyAbsentTotal,
                MonthlyLeaveTotal = monthlyLeaveTotal
            });
        }

        // // Group the data before returning
        List<AbsenceDailyReport> groupedData = absenceDailyReports.GroupBy(report => new
        {
            report.Department,
            report.Department_Name,
            report.Employee_ID,
            report.Local_Full_Name,
            report.Leave,
            report.Days,
            report.MonthlyAbsentTotal,
            report.MonthlyLeaveTotal
        })
        .Select(group => new AbsenceDailyReport
        {
            Department = group.Key.Department,
            Department_Name = group.Key.Department_Name,
            Employee_ID = group.Key.Employee_ID,
            Local_Full_Name = group.Key.Local_Full_Name,
            Leave = group.Key.Leave,
            Days = group.Key.Days,
            MonthlyAbsentTotal = group.Key.MonthlyAbsentTotal,
            MonthlyLeaveTotal = group.Key.MonthlyLeaveTotal
        }).OrderBy(x => x.Department).ThenBy(x => x.Employee_ID)
        .ToList();

        return groupedData;
    }

    private async Task<List<AbsenceDailyReportTodays>> GetDataTodays(AbsenceDailyReportParam param, List<string> roleList, string type)
    {
        // Initialize the predicates
        ExpressionStarter<HRMS_Emp_Personal> predEmpPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true);

        // Apply filters
        if (!string.IsNullOrWhiteSpace(param.Factory))
        {
            ExpressionStarter<HRMS_Emp_Personal> predicate = PredicateBuilder.New<HRMS_Emp_Personal>();
            predicate = predicate.Or(x => x.Factory == param.Factory)
                                .Or(x => x.Assigned_Factory == param.Factory);
            predEmpPersonal.And(predicate);
        }

        if (!string.IsNullOrWhiteSpace(param.Date))
        {
            if (type == "Resigning")
                predEmpPersonal.And(x => x.Resign_Date == Convert.ToDateTime(param.Date));
            else
                predEmpPersonal.And(x => x.Onboard_Date == Convert.ToDateTime(param.Date));
        }

        // Fetch the filtered employee data
        List<HRMS_Emp_Personal> HEP = await Query_Permission_Data_Filter_Factory(roleList, predEmpPersonal);

        // Prepare the result list
        List<AbsenceDailyReportTodays> absenceDailyReports = new();
        foreach (var personal in HEP)
        {
            // Fetch department report asynchronously
            (string department_Code, string department_Name) = await Query_Department_Report(personal.Factory, personal.Department, param.Lang);

            absenceDailyReports.Add(new AbsenceDailyReportTodays
            {
                Department = department_Code,
                Department_Name = department_Name,
                Employee_ID = personal.Employee_ID ?? string.Empty,
                Local_Full_Name = personal.Local_Full_Name ?? string.Empty
            });
        }

        // the list data maintained
        List<AbsenceDailyReportTodays> groupedData = absenceDailyReports.GroupBy(report => new
        {
            report.Department,
            report.Department_Name,
            report.Employee_ID,
            report.Local_Full_Name,
        })
        .Select(group => new AbsenceDailyReportTodays
        {
            Department = group.Key.Department,
            Department_Name = group.Key.Department_Name,
            Employee_ID = group.Key.Employee_ID,
            Local_Full_Name = group.Key.Local_Full_Name
        }).OrderBy(x => x.Department).ThenBy(x => x.Employee_ID)
        .ToList();

        return groupedData;
    }

    public async Task<(decimal monthlyLeaveTotal, decimal monthlyAbsentTotal)> CalculateLeaveAndAbsenceTotals(
        DateTime inputDate, string factory, string employeeId, string leaveCode)
    {
        DateTime firstDateOfMonth = new(inputDate.Year, inputDate.Month, 1);
        DateTime lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);

        decimal monthlyLeaveTotal = await _repositoryAccessor.HRMS_Att_Leave_Maintain
           .FindAll(leave =>
               leave.Factory == factory &&
               leave.Employee_ID == employeeId &&
               leave.Leave_code == leaveCode &&
               leave.Leave_Date >= firstDateOfMonth &&
               leave.Leave_Date <= lastDateOfMonth
           )
           .SumAsync(leave => leave.Days);

        decimal monthlyAbsentTotal = await _repositoryAccessor.HRMS_Att_Leave_Maintain
            .FindAll(leave =>
                leave.Factory == factory &&
                leave.Employee_ID == employeeId &&
                leave.Leave_code == "C0" && // Mã nghỉ phép vắng mặt
                leave.Leave_Date >= firstDateOfMonth &&
                leave.Leave_Date <= lastDateOfMonth
            )
            .SumAsync(leave => leave.Days);

        return (monthlyLeaveTotal, monthlyAbsentTotal);
    }

    #region Query_Department_Report
    private async Task<(string, string)> Query_Department_Report(string factory, string department, string lang)
    {
        var result = await _repositoryAccessor.HRMS_Org_Department
            .FindAll(x => x.Factory == factory && x.Department_Code == department, true)
            .GroupJoin(
                _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == lang.ToLower() && x.Factory == factory && x.Department_Code == department, true),
                x => new { x.Department_Code },
                y => new { y.Department_Code },
                (x, y) => new { HOD = x, HODL = y }
            )
            .SelectMany(x => x.HODL.DefaultIfEmpty(), (x, y) => new { x.HOD, HBCL = y })
           .Select(x => new
           {
               x.HOD.Department_Code,
               Department_Name = x.HBCL != null ? x.HBCL.Name : x.HOD.Department_Name
           })
            .FirstOrDefaultAsync();

        return (result?.Department_Code, result?.Department_Name);
    }
    #endregion

    // 3.1 Query_HRMS_Basic_Code
    // input: language, Type_Seq, code
    private async Task<string> GetListBasicCode(string typeSeq, string code, string language)
    {
        var data = await _repositoryAccessor.HRMS_Basic_Code
            .FindAll(x => x.Type_Seq == typeSeq && x.Code == code, true)
            .GroupJoin(
                _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                HBC => new { HBC.Type_Seq, HBC.Code },
                HBCL => new { HBCL.Type_Seq, HBCL.Code },
                (HBC, HBCL) => new { HBC, HBCL }
            )
            .SelectMany(x => x.HBCL.DefaultIfEmpty(), (prev, HBCL) => new { prev.HBC, HBCL })
             .Select(x =>
                  $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"
             )
            .FirstOrDefaultAsync();
        return data ?? string.Empty;
    }
    #endregion

    #region Get dropdownList condition search
    public async Task<List<KeyValuePair<string, string>>> Queryt_Factory_AddList(string userName, string language)
    {
        List<string> factoriesByAccount = await GetFactoryByAccount(userName);
        List<KeyValuePair<string, string>> factories = await GetDataBasicCode(BasicCodeTypeConstant.Factory, language);

        return factories.IntersectBy(factoriesByAccount, x => x.Key).ToList();
    }

    private async Task<List<string>> GetFactoryByAccount(string userName)
    {
        return await _repositoryAccessor.HRMS_Basic_Role.FindAll(true)
            .Join(_repositoryAccessor.HRMS_Basic_Account_Role.FindAll(x => x.Account == userName, true),
             HBR => HBR.Role,
             HBAR => HBAR.Role,
             (x, y) => new { HBR = x, HBAR = y })
            .Select(x => x.HBR.Factory)
            .Distinct()
            .ToListAsync();
    }

    private async Task<List<HRMS_Emp_Personal>> Query_Permission_Data_Filter_Factory(List<string> accountRoles, ExpressionStarter<HRMS_Emp_Personal> predicate = null)
    {
        List<HRMS_Emp_Personal> result = new();
        foreach (string accountRole in accountRoles)
        {
            ExpressionStarter<HRMS_Emp_Personal> predicatePersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true);
            if (predicate != null && predicate.IsStarted)
                predicatePersonal.And(predicate);
            HRMS_Basic_Role role = await _repositoryAccessor.HRMS_Basic_Role.FirstOrDefaultAsync(x => x.Role == accountRole, true);
            if (role is null)
                continue;
            predicatePersonal.And(x =>
                x.Factory == role.Factory || x.Assigned_Factory == role.Factory
            );
            List<HRMS_Emp_Personal> HEP = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(predicatePersonal, true).ToListAsync();
            result.AddRange(HEP);
        }
        return result;
    }
    #endregion
}
