using AgileObjects.AgileMapper.Extensions;
using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Helper.Utilities;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance;
public class S_5_2_10_HRDailyReport : BaseServices, I_5_2_10_HRDailyReport
{
    public S_5_2_10_HRDailyReport(DBContext dbContext) : base(dbContext)
    {
    }


    #region Base Getdata

    private async Task<OperationResult> BaseGetdata(HRDailyReportParam param, List<string> roleList)
    {
        if (string.IsNullOrWhiteSpace(param.Date) || string.IsNullOrWhiteSpace(param.Factory) || param.PermissionGroups == null || string.IsNullOrWhiteSpace(param.Level))
            return new OperationResult("Factory, Date, Level, PermissionGroup is Required");
        
        DateTime inputDate = Convert.ToDateTime(param.Date);

        var predEmpPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true)
                                            .And(emp => emp.Factory == param.Factory)
                                            .And(emp => emp.Onboard_Date <= inputDate)
                                            .And(emp => emp.Resign_Date >= inputDate || emp.Resign_Date == null)
                                            .And(emp => param.PermissionGroups.Contains(emp.Permission_Group));

        List<HRMS_Emp_Personal> HEP = await Query_Permission_Data_Filter_Factory(roleList, predEmpPersonal);
        List<HRMS_Emp_Personal> HEP_All = await Query_Permission_Data_Filter_Factory(roleList); // Số lượng tất cả nhân viên nằm trong nhà máy
        List<HRMS_Emp_Transfer_History> ETH = await _repositoryAccessor.HRMS_Emp_Transfer_History.FindAll(true).ToListAsync();
        List<HRMS_Basic_Level> HBL = await _repositoryAccessor.HRMS_Basic_Level.FindAll(true).ToListAsync();
        List<HRMS_Att_Leave_Maintain> HLM = await _repositoryAccessor.HRMS_Att_Leave_Maintain.FindAll(true).ToListAsync();
        List<HRMS_Org_Department> HOD = await _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Factory == param.Factory, true).ToListAsync();

        HRDailyReportTableRequest paramTable = new()
        {
            Employee_Dept = new List<string>(),
            HEP = HEP,
            HEP_All = HEP_All,
            ETH = ETH,
            HBL = HBL,
            HLM = HLM,
            HOD = HOD
        };

        var departmentCode = paramTable.HOD.Where(x => x.IsActive == true
                                                    && x.Factory == param.Factory 
                                                    && int.Parse(x.Org_Level) < int.Parse(param.Level))
                                            .Select(x => x.Department_Code)
                                            .ToList();

        var result = paramTable.HOD
            .Where(x => x.IsActive == true
                && (int.Parse(x.Org_Level) == int.Parse(param.Level)
                || (int.Parse(x.Org_Level) >= int.Parse(param.Level) && x.Upper_Department is null)
                || (int.Parse(x.Org_Level) > int.Parse(param.Level) && departmentCode.Contains(x.Upper_Department))))
            .Select(x => new HRDailyReportD()
            {
                Division = x.Division,
                Department = x.Department_Code,
                Factory = x.Factory,
                UpperDepartment = x.Upper_Department
            })
            .ToList();


        List<HRDailyReport> deptResult = new();
        foreach (var item in result)
        {
            paramTable.Employee_Dept.Clear();
            paramTable.Employee_Dept.Add(item.Department);
            var depart = DepartmentHierarchy(param, paramTable);
            paramTable.Employee_Dept.Clear();
            paramTable.Employee_Dept.AddRange(depart.Select(x => x.Department).ToList());
            // Check department
            if (paramTable.HEP.Any(x => paramTable.Employee_Dept.Contains(x.Department)))
            {
                List<HRDailyReport> hrDailyReport = await CalculateReport(param, paramTable, item);
                deptResult.AddRange(hrDailyReport);
            }
        }
        if (!deptResult.Any())
            return new OperationResult(false, "No Data");

        HRDailyReportResult resultResponse = new()
        {
            HeadCount = await Get_Dept_values_Emp_Cnt(param),
            HRDailyReport = deptResult
        };
        return new OperationResult(true, resultResponse);
    }

    #region  CalculateReport
    private async Task<List<HRDailyReport>> CalculateReport(HRDailyReportParam param, HRDailyReportTableRequest paramTable, HRDailyReportD item)
    {
        DateTime inputDate = Convert.ToDateTime(param.Date);
        List<HRDailyReport> hrDailyReport = new();
        (string org_Level, string department_Code, string department_Name) = await Query_Department_Report(param, item);
        int expectedAttendance = GetExpectedAttendanceField(param, paramTable, inputDate, item);
        int newRecruit = GetNewRecruitField(param, paramTable, inputDate, paramTable.Employee_Dept);
        int resigned = GetResignedField(param, paramTable, inputDate, paramTable.Employee_Dept);
        int employee_Absences_5Days = GetEmployee_Absences_5DaysField(param, paramTable, inputDate, paramTable.Employee_Dept);
        int personal_Cnt = GetPersonalLeaveField(param, paramTable, paramTable.Employee_Dept);
        int unpaidLeave = GetUnpaidLeaveField(param, paramTable, paramTable.Employee_Dept);
        int sickLeave = GetSickLeaveField(param, paramTable, paramTable.Employee_Dept);
        int absenteeism = GetAbsenteeismField(param, paramTable, paramTable.Employee_Dept);
        int workStoppage = GetWorkStoppageField(param, paramTable, paramTable.Employee_Dept);
        int annualLeaveCompany = GetAnnualLeaveCompanyField(param, paramTable, paramTable.Employee_Dept);
        int annualLeaveEmployee = GetAnnualLeaveEmployeeField(param, paramTable, paramTable.Employee_Dept);
        int otherLeave = GetOtherLeaveField(param, paramTable, paramTable.Employee_Dept);
        int maternityLeave = GetMaternityLeaveField(param, paramTable, paramTable.Employee_Dept);
        int prenatalCheckupLeave = GetPrenatalCheckupLeaveField(param, paramTable, paramTable.Employee_Dept);
        int compensatoryMaternityLeave = GetCompensatoryMaternityLeaveField(param, paramTable, paramTable.Employee_Dept);
        int businessTrip = GetBusinessTripField(param, paramTable, paramTable.Employee_Dept);
        int expectedAttendanceTomorrow = GetExpectedAttendanceTomorrowField(expectedAttendance, newRecruit, resigned);

        hrDailyReport.Add(new HRDailyReport
        {
            GroupBy = param.Level,
            Department = $"{org_Level} {department_Code}",
            Department_Name = department_Name,
            ExpectedAttendance = expectedAttendance,
            ExpectedAttendanceExcluding5DaysAbsenteeism = expectedAttendance - employee_Absences_5Days,
            ExpectedAttendanceExcluding5DaysAbsenteeismAndMaternityLeave = expectedAttendance - employee_Absences_5Days - maternityLeave,
            Supervisor = GetSupervisorField(param, paramTable, paramTable.Employee_Dept),
            Staff = GetStaffField(param, paramTable, paramTable.Employee_Dept),
            Technicians = GetTechniciansField(param, paramTable, paramTable.Employee_Dept),
            WaterSpiders = GetWaterSpidersField(param, paramTable, paramTable.Employee_Dept),
            Assistants = GetAssistantsField(param, paramTable, paramTable.Employee_Dept),
            PersonalLeave = personal_Cnt,
            UnpaidLeave = unpaidLeave,
            SickLeave = sickLeave,
            Absenteeism = absenteeism,
            WorkStoppage = workStoppage,
            AnnualLeaveCompany = annualLeaveCompany,
            AnnualLeaveEmployee = annualLeaveEmployee,
            OtherLeave = otherLeave,
            MaternityLeave = maternityLeave,
            PrenatalCheckupLeave = prenatalCheckupLeave,
            CompensatoryMaternityLeave = compensatoryMaternityLeave,
            BusinessTrip = businessTrip,
            ActualAttendance = GetActualAttendanceField(expectedAttendance, personal_Cnt, unpaidLeave, sickLeave, absenteeism, workStoppage, annualLeaveCompany, annualLeaveEmployee, otherLeave, maternityLeave, prenatalCheckupLeave, compensatoryMaternityLeave, resigned, newRecruit),
            NewRecruit = newRecruit,
            Resigned = resigned,
            ExpectedAttendanceTomorrow = expectedAttendanceTomorrow,
            Employee_Absences_5Days = employee_Absences_5Days,
            ExpectedAttendanceTomorrowExcluding5days = GetExpectedAttendanceTomorrowExcluding5daysField(expectedAttendanceTomorrow, employee_Absences_5Days),
            _8HourWorkForPregnantEmployees = Get8HourWorkForPregnantEmployeesField(param, paramTable, paramTable.Employee_Dept),
            _7HourWorkForPregnantEmployees = Get7HourWorkForPregnantEmployeesField(param, paramTable, inputDate, paramTable.Employee_Dept),
            _7HourWorkForEmployeesWithBabiesUnder12Months = Get7HourWorkForEmployeesWithBabiesUnder12MonthsField(param, paramTable, inputDate, paramTable.Employee_Dept),
            _8HourWorkForEmployeesWithBabiesUnder12Months = Get8HourWorkForEmployeesWithBabiesUnder12MonthsField(param, paramTable, inputDate, paramTable.Employee_Dept),
            TotalPersonalSickAndAbsenteeismLeave = GetTotalPersonalSickAndAbsenteeismLeaveField(personal_Cnt, sickLeave, absenteeism, employee_Absences_5Days),
            AverageLeaveCountPersonalSickAndAbsenteeism = GetAverageLeaveCountField(personal_Cnt, sickLeave, absenteeism, employee_Absences_5Days, expectedAttendance, maternityLeave),
        });

        return hrDailyReport;
    }
    #endregion


    #region DepartmentHierarchy
    private static List<HRDailyReportD> DepartmentHierarchy(HRDailyReportParam param, HRDailyReportTableRequest paramTable)
    {
        var departmentHierarchy = new List<HRDailyReportD>();

        var initialDepartment = paramTable.HOD
            .Where(d => d.IsActive == true && d.Factory == param.Factory && paramTable.Employee_Dept.Contains(d.Department_Code))
            .Select(x => new HRDailyReportD
            {
                Department = x.Department_Code,
                OrgLevel = x.Org_Level
            })
            .FirstOrDefault();

        if (initialDepartment != null)
        {
            departmentHierarchy.Add(initialDepartment);
            AddSubDepartments(initialDepartment, paramTable.HOD, departmentHierarchy, param.Factory);
        }

        return departmentHierarchy;
    }

    private static void AddSubDepartments(HRDailyReportD department, List<HRMS_Org_Department> HOD, List<HRDailyReportD> departmentHierarchy, string factory)
    {
        var subDepartments = HOD
            .Where(x => x.IsActive == true && x.Factory == factory && x.Upper_Department == department.Department)
            .Select(x => new HRDailyReportD
            {
                Department = x.Department_Code,
                UpperDepartment = x.Upper_Department,
                OrgLevel = x.Org_Level
            })
            .ToList();

        foreach (var subDept in subDepartments)
        {
            if (subDept.Department != subDept.UpperDepartment)
            {
                departmentHierarchy.Add(subDept);
                AddSubDepartments(subDept, HOD, departmentHierarchy, factory);
            }
        }
    }
    #endregion

    #endregion

    #region Excel area
    public async Task<OperationResult> DownloadExcel(HRDailyReportParam param, List<string> roleList, string userName)
    {
        OperationResult result = await BaseGetdata(param, roleList);

        if (!result.IsSuccess)
            return result;
        HRDailyReportResult res = result.Data as HRDailyReportResult;
        List<HRDailyReport> hrDailyReport = res.HRDailyReport;
        // totals

        int totalExpectedAttendance = hrDailyReport.Sum(x => x.ExpectedAttendance);
        int totalExpectedAttendanceExcluding5DaysAbsenteeism = hrDailyReport.Sum(x => x.ExpectedAttendanceExcluding5DaysAbsenteeism);
        int totalExpectedAttendanceExcluding5DaysAbsenteeismAndMaternityLeave = hrDailyReport.Sum(x => x.ExpectedAttendanceExcluding5DaysAbsenteeismAndMaternityLeave);
        int totalSupervisor = hrDailyReport.Sum(x => x.Supervisor);
        int totalStaff = hrDailyReport.Sum(x => x.Staff);
        int totalTechnicians = hrDailyReport.Sum(x => x.Technicians);
        int totalWaterSpiders = hrDailyReport.Sum(x => x.WaterSpiders);
        int totalAssistants = hrDailyReport.Sum(x => x.Assistants);
        int totalPersonalLeave = hrDailyReport.Sum(x => x.PersonalLeave);
        int totalUnpaidLeave = hrDailyReport.Sum(x => x.UnpaidLeave);
        int totalSickLeave = hrDailyReport.Sum(x => x.SickLeave);
        int totalAbsenteeism = hrDailyReport.Sum(x => x.Absenteeism);
        int totalWorkStoppage = hrDailyReport.Sum(x => x.WorkStoppage);
        int totalAnnualLeaveCompany = hrDailyReport.Sum(x => x.AnnualLeaveCompany);
        int totalAnnualLeaveEmployee = hrDailyReport.Sum(x => x.AnnualLeaveEmployee);
        int totalOtherLeave = hrDailyReport.Sum(x => x.OtherLeave);
        int totalMaternityLeave = hrDailyReport.Sum(x => x.MaternityLeave);
        int totalPrenatalCheckupLeave = hrDailyReport.Sum(x => x.PrenatalCheckupLeave);
        int totalCompensatoryMaternityLeave = hrDailyReport.Sum(x => x.CompensatoryMaternityLeave);
        int totalBusinessTrip = hrDailyReport.Sum(x => x.BusinessTrip);
        int totalActualAttendance = hrDailyReport.Sum(x => x.ActualAttendance);
        int totalNewRecruit = hrDailyReport.Sum(x => x.NewRecruit);
        int totalResigned = hrDailyReport.Sum(x => x.Resigned);
        int totalExpectedAttendanceTomorrow = hrDailyReport.Sum(x => x.ExpectedAttendanceTomorrow);
        int totalExpectedAttendanceTomorrowExcluding5days = hrDailyReport.Sum(x => x.ExpectedAttendanceTomorrowExcluding5days);
        int total_8HourWorkForPregnantEmployees = hrDailyReport.Sum(x => x._8HourWorkForPregnantEmployees);
        int total_7HourWorkForPregnantEmployees = hrDailyReport.Sum(x => x._7HourWorkForPregnantEmployees);
        int total_8HourWorkForEmployeesWithBabiesUnder12Months = hrDailyReport.Sum(x => x._8HourWorkForEmployeesWithBabiesUnder12Months);
        int total_7HourWorkForEmployeesWithBabiesUnder12Months = hrDailyReport.Sum(x => x._7HourWorkForEmployeesWithBabiesUnder12Months);
        int totalEmployee_Absences_5Days = hrDailyReport.Sum(x => x.Employee_Absences_5Days);
        int total_TotalPersonalSickAndAbsenteeismLeave = hrDailyReport.Sum(x => x.TotalPersonalSickAndAbsenteeismLeave);
        double total_AverageLeaveCountPersonalSickAndAbsenteeism = hrDailyReport.Sum(x => x.AverageLeaveCountPersonalSickAndAbsenteeism);

        List<HRDailyReport> hrDailyReportTotals = new()
        {
            new HRDailyReport()
            {
                ExpectedAttendance = totalExpectedAttendance,
                ExpectedAttendanceExcluding5DaysAbsenteeism = totalExpectedAttendanceExcluding5DaysAbsenteeism,
                ExpectedAttendanceExcluding5DaysAbsenteeismAndMaternityLeave = totalExpectedAttendanceExcluding5DaysAbsenteeismAndMaternityLeave,
                Supervisor = totalSupervisor,
                Staff = totalStaff,
                Technicians = totalTechnicians,
                WaterSpiders = totalWaterSpiders,
                Assistants = totalAssistants,
                PersonalLeave = totalPersonalLeave,
                UnpaidLeave = totalUnpaidLeave,
                SickLeave = totalSickLeave,
                Absenteeism = totalAbsenteeism,
                WorkStoppage = totalWorkStoppage,
                AnnualLeaveCompany = totalAnnualLeaveCompany,
                AnnualLeaveEmployee = totalAnnualLeaveEmployee,
                OtherLeave = totalOtherLeave,
                MaternityLeave = totalMaternityLeave,
                PrenatalCheckupLeave = totalPrenatalCheckupLeave,
                CompensatoryMaternityLeave = totalCompensatoryMaternityLeave,
                BusinessTrip = totalBusinessTrip,
                ActualAttendance = totalActualAttendance,
                NewRecruit = totalNewRecruit,
                Resigned = totalResigned,
                ExpectedAttendanceTomorrow = totalExpectedAttendanceTomorrow,
                ExpectedAttendanceTomorrowExcluding5days = totalExpectedAttendanceTomorrowExcluding5days,
                _8HourWorkForPregnantEmployees = total_8HourWorkForPregnantEmployees,
                _7HourWorkForPregnantEmployees = total_7HourWorkForPregnantEmployees,
                _8HourWorkForEmployeesWithBabiesUnder12Months = total_8HourWorkForEmployeesWithBabiesUnder12Months,
                _7HourWorkForEmployeesWithBabiesUnder12Months = total_7HourWorkForEmployeesWithBabiesUnder12Months,
                Employee_Absences_5Days = totalEmployee_Absences_5Days,
                TotalPersonalSickAndAbsenteeismLeave = total_TotalPersonalSickAndAbsenteeismLeave,
                AverageLeaveCountPersonalSickAndAbsenteeism = total_AverageLeaveCountPersonalSickAndAbsenteeism,
            }
        };
        List<Table> tables = new()
        {
            new Table("result", hrDailyReport),
            new Table("totals", hrDailyReportTotals),
        };
        List<Cell> cells = new()
        {
            new Cell("B2", param.Factory),
            new Cell("B3", userName),
            new Cell("D2", param.Date),
            new Cell("D3", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")),
            new Cell("F2", param.Level_Name),
        };
        ConfigDownload config = new() { IsAutoFitColumn = true };
        ExcelResult excelResult = ExcelUtility.DownloadExcel(
            tables, 
            cells, 
            "Resources\\Template\\AttendanceMaintenance\\5_2_10_HRDailyReport\\Download.xlsx", 
            config
        );
        return new OperationResult(excelResult.IsSuccess, excelResult.Error, new
        {
            excelResult.Result,
            QueryResult = hrDailyReport.Count,
            res.HeadCount,
            MonthlyAbsenteeism = totalAbsenteeism
        });
    }

    #region Query_Department_Report
    private async Task<(string, string, string)> Query_Department_Report(HRDailyReportParam param, HRDailyReportD item)
    {
        var result = await _repositoryAccessor.HRMS_Org_Department
            .FindAll(x => x.Factory == param.Factory && item.Department.Contains(x.Department_Code) && x.Division == item.Division, true)
            .GroupJoin(
                _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower() && x.Factory == param.Factory && x.Department_Code == item.Department && x.Division == item.Division, true),
                x => new { x.Department_Code },
                y => new { y.Department_Code },
                (x, y) => new { HOD = x, HODL = y }
            )
            .SelectMany(x => x.HODL.DefaultIfEmpty(), (x, y) => new { x.HOD, HBCL = y })
           .Select(x => new
           {
               x.HOD.Department_Code,
               Department_Name = x.HBCL != null ? x.HBCL.Name : x.HOD.Department_Name,
               x.HOD.Org_Level
           })
            .FirstOrDefaultAsync();

        return (result?.Org_Level, result?.Department_Code, result?.Department_Name);
    }
    #endregion

    // out_cnt, in_cnt base (ETH, HEP)
    public (int Out_Cnt, int In_Cnt) Get_out_cnt_in_cnt_base(HRDailyReportTableRequest paramTable, 
                                                                string inputFactory, 
                                                                DateTime inputDate, 
                                                                List<string> permissionGroups, 
                                                                List<string> department_Codes)
    {
        var query = paramTable.ETH
            .Join(paramTable.HEP_All,
                ETH => ETH.USER_GUID,
                emp => emp.USER_GUID,
                (ETH, emp) => new { ETH, emp })
          .Where(x => !x.ETH.Effective_Status
                    && x.ETH.Data_Source == "01"
                    && x.ETH.Effective_Date <= inputDate
                    && permissionGroups.Contains(x.emp.Permission_Group)).ToList();

        int out_Cnt = query.FindAll(j => j.ETH.Factory_Before == inputFactory && department_Codes.Contains(j.ETH.Department_Before)).Select(x => x.ETH.USER_GUID).Distinct().Count();
        int in_Cnt = query.FindAll(j => j.ETH.Factory_After == inputFactory && department_Codes.Contains(j.ETH.Department_After)).Select(x => x.ETH.USER_GUID).Distinct().Count();

        return (out_Cnt, in_Cnt);
    }

    // 3.
    private int GetExpectedAttendanceField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, DateTime inputDate, HRDailyReportD item)
    {
        // Tính toán số lượng nhân viên dự kiến có mặt 
        var department_Codes = _repositoryAccessor.HRMS_Org_Department
                                        .FindAll(x => x.Factory == param.Factory && x.Upper_Department  == item.Department
                                                    || x.Factory == param.Factory && x.Department_Code == item.Department)
                                        . Select(x => x.Department_Code)
                                        .ToList();

        int relevantEmployeeCount = paramTable.HEP_All.FindAll(emp => department_Codes.Contains(emp.Department)
                                                                && emp.Onboard_Date.Date < inputDate.Date
                                                                && (emp.Resign_Date is null || emp.Resign_Date.Value.Date >= inputDate.Date)
                                                                && param.PermissionGroups.Contains(emp.Permission_Group))
                                                        .Select(x => x.USER_GUID)
                                                        .Distinct()
                                                        .Count();

        (int outCount, int inCount) = Get_out_cnt_in_cnt_base(paramTable, param.Factory, inputDate, param.PermissionGroups, department_Codes);
        int expectedAttendance = relevantEmployeeCount - outCount + inCount;

        return expectedAttendance;
    }

    // (HEP, HBL)  // for 6, 7, 8 field
    private int GetFieldCount(HRDailyReportParam param,
                            HRDailyReportTableRequest paramTable,
                            List<string> dept_values, List<string> roleTypeCode)
    {
        DateTime inputDate = Convert.ToDateTime(param.Date);

        // Fetch employee and basic level data
        IEnumerable<HRMS_Emp_Personal> HEP = paramTable.HEP_All
        .Where(emp => emp.Factory == param.Factory && dept_values.Contains(emp.Department)
        && emp.Onboard_Date < inputDate && (emp.Resign_Date >= inputDate || emp.Resign_Date == null)
        && param.PermissionGroups.Contains(emp.Permission_Group));
        IEnumerable<HRMS_Basic_Level> HBL = paramTable.HBL
            .Where(x => roleTypeCode.Contains(x.Type_Code));

        // Grouping and counting
        int totalCnt = HEP.Join(HBL, HEP => HEP.Position_Title, HBL => HBL.Level_Code, (HEP, HBL) => new { HEP, HBL })
            .Select(x => new { x.HEP.Department, x.HEP.USER_GUID, x.HBL.Type_Code, x.HBL.Level }).Distinct().Count();
        (int out_Cnt, int in_Cnt) = Sum_Staff_Cnt(paramTable, param.Factory, inputDate, dept_values, roleTypeCode);

        totalCnt = totalCnt - out_Cnt + in_Cnt;

        return totalCnt;
    }

    // 6.
    private int GetSupervisorField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, List<string> dept_values)
    {
        return GetFieldCount(param, paramTable, dept_values, new List<string>() { "B" });
    }

    // 7.
    private int GetStaffField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, List<string> dept_values)
    {
        return GetFieldCount(param, paramTable, dept_values, new List<string>() { "A" });
    }

    // 8.
    private int GetTechniciansField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, List<string> dept_values)
    {
        // Danh sách Codes
        var roleTypeCode = new List<string>() { "C", "D" };
        return GetFieldCount(param, paramTable, dept_values, roleTypeCode);
    }

    // 9. , 10., 11.
    private static int GetFieldSum(
        HRDailyReportParam param,
        HRDailyReportTableRequest paramTable,
        List<string> dept_values,
        List<string> workTypes
    )
    {
        DateTime inputDate = Convert.ToDateTime(param.Date);

        int fieldSum = paramTable.HEP_All.FindAll(emp =>
            emp.Factory == param.Factory
            && dept_values.Contains(emp.Department)
            && emp.Onboard_Date < inputDate
            && (emp.Resign_Date == null || emp.Resign_Date >= inputDate)
            && param.PermissionGroups.Contains(emp.Permission_Group)
            && workTypes.Contains(emp.Work_Type)
        ).Select(x => x.USER_GUID).Distinct().Count();


        var query = paramTable.ETH
            .Join(paramTable.HEP_All,
                ETH => ETH.USER_GUID,
                emp => emp.USER_GUID,
                (ETH, emp) => new { ETH, emp })
          .Where(joined =>
              !joined.ETH.Effective_Status
              && joined.ETH.Data_Source == "01"
              && joined.ETH.Effective_Date <= inputDate
              && param.PermissionGroups.Contains(joined.emp.Permission_Group)).ToList();

        int out_Cnt = query.FindAll(j => j.ETH.Factory_Before == param.Factory && dept_values.Contains(j.ETH.Department_Before) && workTypes.Contains(j.ETH.Work_Type_Before)).Select(x => x.ETH.USER_GUID).Distinct().Count();
        int in_Cnt = query.FindAll(j => j.ETH.Factory_After == param.Factory && dept_values.Contains(j.ETH.Department_After) && workTypes.Contains(j.ETH.Work_Type_After)).Select(x => x.ETH.USER_GUID).Distinct().Count();

        fieldSum = fieldSum - out_Cnt + in_Cnt;
        return fieldSum;
    }
    private static int GetFieldSumAssistants(
        HRDailyReportParam param,
        HRDailyReportTableRequest paramTable,
        List<string> dept_values,
        List<string> position_Title
    )
    {
        DateTime inputDate = Convert.ToDateTime(param.Date);

        int fieldSum = paramTable.HEP_All.FindAll(emp =>
            emp.Factory == param.Factory
            && dept_values.Contains(emp.Department)
            && emp.Onboard_Date < inputDate
            && (emp.Resign_Date == null || emp.Resign_Date >= inputDate)
            && param.PermissionGroups.Contains(emp.Permission_Group)
            && position_Title.Contains(emp.Position_Title)
        ).Select(x => x.USER_GUID).Distinct().Count();


        var query = paramTable.ETH
            .Join(paramTable.HEP_All,
                ETH => ETH.USER_GUID,
                emp => emp.USER_GUID,
                (ETH, emp) => new { ETH, emp })
          .Where(joined =>
              !joined.ETH.Effective_Status
              && joined.ETH.Data_Source == "01"
              && joined.ETH.Effective_Date <= inputDate
              && param.PermissionGroups.Contains(joined.emp.Permission_Group)).ToList();

        int out_Cnt = query.FindAll(j => j.ETH.Factory_Before == param.Factory && dept_values.Contains(j.ETH.Department_Before) && position_Title.Contains(j.ETH.Position_Title_Before)).Select(x => x.ETH.USER_GUID).Distinct().Count();
        int in_Cnt = query.FindAll(j => j.ETH.Factory_After == param.Factory && dept_values.Contains(j.ETH.Department_After) && position_Title.Contains(j.ETH.Position_Title_After)).Select(x => x.ETH.USER_GUID).Distinct().Count();

        fieldSum = fieldSum - out_Cnt + in_Cnt;
        return fieldSum;
    }

    // 9.
    private static int GetWaterSpidersField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, List<string> dept_values)
    {
        List<string> workTypes = new() { "A00", "F12" };
        return GetFieldSum(
            param,
            paramTable,
            dept_values,
            workTypes
        );
    }

    // 10.
    private static int GetAssistantsField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, List<string> dept_values)
    {
        List<string> position_Title = new() { "B150" };
        return GetFieldSumAssistants(
            param,
            paramTable,
            dept_values,
            position_Title
        );
    }

    // 11.
    private int GetPersonalLeaveField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, List<string> dept_values)
    {
        List<string> leaveList = new() { "A0", "O0", "G3" };
        return Sum_Leave_Cnt(paramTable, param, dept_values, leaveList);
    }

    // 12.
    private int GetUnpaidLeaveField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, List<string> dept_values)
    {
        List<string> leaveList = new() { "J3", "J4" };
        return Sum_Leave_Cnt(paramTable, param, dept_values, leaveList);
    }

    // 13.
    private int GetSickLeaveField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, List<string> dept_values)
    {
        List<string> leaveList = new() { "B0" };
        return Sum_Leave_Cnt(paramTable, param, dept_values, leaveList);
    }

    // 14.
    private int GetAbsenteeismField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, List<string> dept_values)
    {
        List<string> leaveList = new() { "C0" };
        int absent = Sum_Leave_Cnt(paramTable, param, dept_values, leaveList);
        return absent;
    }

    // 15.
    private int GetWorkStoppageField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, List<string> dept_values)
    {
        List<string> leaveList = new() { "J0", "J1", "J2", "J5" };
        return Sum_Leave_Cnt(paramTable, param, dept_values, leaveList);
    }

    // 16.
    private int GetAnnualLeaveCompanyField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, List<string> dept_values)
    {
        List<string> leaveList = new() { "I1" };
        return Sum_Leave_Cnt(paramTable, param, dept_values, leaveList);
    }

    // 17.
    private int GetAnnualLeaveEmployeeField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, List<string> dept_values)
    {
        List<string> leaveList = new() { "I0" };
        return Sum_Leave_Cnt(paramTable, param, dept_values, leaveList);
    }

    // 18.
    private int GetOtherLeaveField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, List<string> dept_values)
    {
        List<string> leaveList = new() { "E0", "F0", "H0", "K0" };
        return Sum_Leave_Cnt(paramTable, param, dept_values, leaveList);
    }

    // 19.
    private int GetMaternityLeaveField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, List<string> dept_values)
    {
        List<string> leaveList = new() { "G0" };
        return Sum_Leave_Cnt(paramTable, param, dept_values, leaveList);
    }

    // 20.
    private int GetPrenatalCheckupLeaveField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, List<string> dept_values)
    {
        List<string> leaveList = new() { "G2" };
        return Sum_Leave_Cnt(paramTable, param, dept_values, leaveList);
    }

    // 21.
    private int GetCompensatoryMaternityLeaveField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, List<string> dept_values)
    {
        List<string> leaveList = new() { "G1" };
        return Sum_Leave_Cnt(paramTable, param, dept_values, leaveList);
    }

    // 22.
    private int GetBusinessTripField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, List<string> dept_values)
    {
        List<string> leaveList = new() { "M0" };
        return Sum_Leave_Cnt(paramTable, param, dept_values, leaveList);
    }

    // 23.
    private static int GetActualAttendanceField(int expected_Attendancet, int personal_Cnt, int unpaid, int sick, int absent,
     int work_Stoppage, int annual_Company, int annual_Employee, int otherLeave,
     int maternity, int prenatal_Checkup, int compensatory_Maternit, int resigned, int new_Recruit)
    {
        int actual = expected_Attendancet - personal_Cnt - unpaid - sick - absent - work_Stoppage - annual_Company - annual_Employee - otherLeave
        - maternity - prenatal_Checkup - compensatory_Maternit - resigned - new_Recruit;
        return actual;
    }

    // 24.
    private static int GetNewRecruitField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, DateTime inputDate, List<string> dept_values)
    {
        ExpressionStarter<HRMS_Emp_Personal> predEmpPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true)
         .And(emp => emp.Factory == param.Factory)
         .And(emp => dept_values.Contains(emp.Department))
         .And(emp => emp.Resign_Date >= inputDate || emp.Resign_Date == null)
         .And(emp => param.PermissionGroups.Contains(emp.Permission_Group))
         .And(emp => emp.Onboard_Date == inputDate);
        var HEP = paramTable.HEP_All.Where(predEmpPersonal);
        int new_Recruit = HEP.Select(x => x.USER_GUID).Distinct().Count();

        (int out_Cnt, int in_Cnt) = In_Out_Cnt_2Table(param, paramTable, dept_values, emp => emp.Onboard_Date);
        new_Recruit = new_Recruit - out_Cnt - in_Cnt;
        return new_Recruit;
    }

    // 25.
    private static int GetResignedField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, DateTime inputDate, List<string> dept_values)
    {
        ExpressionStarter<HRMS_Emp_Personal> predEmpPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true)
         .And(emp => emp.Factory == param.Factory)
         .And(emp => dept_values.Contains(emp.Department))
         .And(emp => emp.Resign_Date >= inputDate || emp.Resign_Date == null)
         .And(emp => param.PermissionGroups.Contains(emp.Permission_Group))
         .And(emp => emp.Resign_Date == inputDate);
        var HEP = paramTable.HEP_All.Where(predEmpPersonal);
        int resigned = HEP.Select(x => x.USER_GUID).Distinct().Count();

        (int out_Cnt, int in_Cnt) = In_Out_Cnt_2Table(param, paramTable, dept_values, emp => emp.Resign_Date);
        resigned = resigned - out_Cnt - in_Cnt;
        return resigned;
    }

    private static (int Out_Cnt, int In_Cnt) In_Out_Cnt_2Table(
        HRDailyReportParam param,
        HRDailyReportTableRequest paramTable,
        List<string> dept_values,
        Func<HRMS_Emp_Personal, DateTime?> getDateField, bool work8hours = false, bool work7hours = false, List<string> employee_ids = null, bool work7hours_BB = false
    )
    {
        DateTime inputDate = Convert.ToDateTime(param.Date);

        // Predicate for outgoing transfers
        ExpressionStarter<HRMS_Emp_Transfer_History> predETH = PredicateBuilder.New<HRMS_Emp_Transfer_History>(true)
            .And(eth => !eth.Effective_Status)
            .And(eth => eth.Data_Source == "01")
            .And(eth => eth.Effective_Date <= inputDate);
        IEnumerable<HRMS_Emp_Transfer_History> ETH = paramTable.ETH.Where(predETH);

        ExpressionStarter<HRMS_Emp_Personal> predEmpPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true)
            .And(emp => param.PermissionGroups.Contains(emp.Permission_Group));

        if (work8hours)
        {
            predEmpPersonal = predEmpPersonal.And(emp => emp.Resign_Date >= inputDate || emp.Resign_Date == null)
            .And(emp => emp.Work8hours == true);
        }
        else if (work7hours)
        {
            predEmpPersonal = predEmpPersonal.And(emp => emp.Resign_Date >= inputDate || emp.Resign_Date == null)
            .And(emp => emp.Work_Shift_Type == "L0")
            .And(emp => !employee_ids.Contains(emp.Employee_ID));
        }
        else if (work7hours_BB)
        {
            predEmpPersonal = predEmpPersonal.And(emp => emp.Resign_Date >= inputDate || emp.Resign_Date == null)
            .And(emp => emp.Work_Shift_Type == "T0");
        }
        else
        {
            // New Recruit
            predEmpPersonal = predEmpPersonal.And(emp => getDateField(emp) == inputDate);
        }
        IEnumerable<HRMS_Emp_Personal> HEP = paramTable.HEP.Where(predEmpPersonal);

        var query = ETH
            .Join(HEP,
                ETH => ETH.USER_GUID,
                emp => emp.USER_GUID,
                (ETH, emp) => new { ETH, emp }).ToList();

        int out_Cnt = query.FindAll(joined => joined.ETH.Factory_Before == param.Factory && dept_values.Contains(joined.ETH.Department_Before) && joined.emp.Work8hours == true).Select(x => x.ETH.USER_GUID).Distinct().Count();

        int in_Cnt = query.FindAll(joined => joined.ETH.Factory_After == param.Factory && dept_values.Contains(joined.ETH.Department_After) && joined.emp.Work8hours == true).Select(x => x.ETH.USER_GUID).Distinct().Count();

        return (out_Cnt, in_Cnt);
    }

    // 26.
    private static int GetExpectedAttendanceTomorrowField(int expected_Attendancet, int new_Recruit, int resigned)
    {
        return expected_Attendancet + new_Recruit - resigned;
    }

    // 27.
    private static int GetExpectedAttendanceTomorrowExcluding5daysField(int attendance_Tomorrow, int employee_Absences_5Days)
    {
        return attendance_Tomorrow - employee_Absences_5Days;
    }

    // 28.
    private static int Get8HourWorkForPregnantEmployeesField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, List<string> dept_values)
    {
        DateTime inputDate = Convert.ToDateTime(param.Date);
        ExpressionStarter<HRMS_Emp_Personal> predEmpPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true)
        .And(emp => emp.Factory == param.Factory)
        .And(emp => dept_values.Contains(emp.Department))
        .And(emp => param.PermissionGroups.Contains(emp.Permission_Group))
        .And(emp => emp.Resign_Date >= inputDate || emp.Resign_Date == null)
        .And(emp => emp.Work8hours == true);
        IEnumerable<HRMS_Emp_Personal> HEP = paramTable.HEP_All.Where(predEmpPersonal);
        int work8hours_Cnt = HEP.Select(x => x.USER_GUID).Distinct().Count();
        (int out_Cnt, int in_Cnt) = In_Out_Cnt_2Table(param, paramTable, dept_values, null, true);

        work8hours_Cnt = work8hours_Cnt - out_Cnt + in_Cnt;
        return work8hours_Cnt;
    }

    // 29.
    private static int Get7HourWorkForPregnantEmployeesField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, DateTime inputDate, List<string> dept_values)
    {
        List<string> employee_ids = paramTable.HLM.Where(x => x.Factory == param.Factory && x.Leave_Date == inputDate && x.Leave_code == "G0").Select(x => x.Employee_ID).ToList();
        ExpressionStarter<HRMS_Emp_Personal> predEmpPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true)
        .And(emp => emp.Factory == param.Factory)
        .And(emp => dept_values.Contains(emp.Department))
        .And(emp => param.PermissionGroups.Contains(emp.Permission_Group))
        .And(emp => emp.Resign_Date >= inputDate || emp.Resign_Date == null)
        .And(emp => emp.Work_Shift_Type == "L0")
        .And(emp => !employee_ids.Contains(emp.Employee_ID));
        IEnumerable<HRMS_Emp_Personal> HEP = paramTable.HEP_All.Where(predEmpPersonal);
        int work7hours_Cnt = HEP.Select(x => x.USER_GUID).Distinct().Count();
        (int out_Cnt, int in_Cnt) = In_Out_Cnt_2Table(param, paramTable, dept_values, null, false, true, employee_ids);

        work7hours_Cnt = work7hours_Cnt - out_Cnt + in_Cnt;
        return work7hours_Cnt;
    }

    // 30.
    private static int Get7HourWorkForEmployeesWithBabiesUnder12MonthsField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, DateTime inputDate, List<string> dept_values)
    {
        ExpressionStarter<HRMS_Emp_Personal> predEmpPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true)
        .And(emp => emp.Factory == param.Factory)
        .And(emp => dept_values.Contains(emp.Department))
        .And(emp => param.PermissionGroups.Contains(emp.Permission_Group))
        .And(emp => emp.Resign_Date >= inputDate || emp.Resign_Date == null)
        .And(emp => emp.Work_Shift_Type == "T0");
        IEnumerable<HRMS_Emp_Personal> HEP = paramTable.HEP_All.Where(predEmpPersonal);
        int work7hours_BB = HEP.Select(x => x.USER_GUID).Distinct().Count();
        (int out_Cnt, int in_Cnt) = In_Out_Cnt_2Table(param, paramTable, dept_values, null, false, false, null, true);

        work7hours_BB = work7hours_BB - out_Cnt + in_Cnt;
        return work7hours_BB;
    }

    // 31.
    private static int Get8HourWorkForEmployeesWithBabiesUnder12MonthsField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, DateTime inputDate, List<string> dept_values)
    {
        List<string> work_shift_type = new() { "J0", "H0" };
        ExpressionStarter<HRMS_Emp_Personal> predEmpPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true)
        .And(emp => emp.Factory == param.Factory)
        .And(emp => dept_values.Contains(emp.Department))
        .And(emp => param.PermissionGroups.Contains(emp.Permission_Group))
        .And(emp => emp.Resign_Date >= inputDate || emp.Resign_Date == null)
        .And(emp => work_shift_type.Contains(emp.Work_Shift_Type));
        IEnumerable<HRMS_Emp_Personal> HEP = paramTable.HEP_All.Where(predEmpPersonal);
        int work8hours_BB = HEP.Select(x => x.USER_GUID).Distinct().Count();
        return work8hours_BB;
    }

    // 32.
    private static int GetEmployee_Absences_5DaysField(HRDailyReportParam param, HRDailyReportTableRequest paramTable, DateTime inputDate, List<string> dept_values)
    {
        ExpressionStarter<HRMS_Emp_Personal> predEmpPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true)
        .And(emp => emp.Factory == param.Factory)
        .And(emp => emp.Resign_Date >= inputDate || emp.Resign_Date == null)
        .And(emp => param.PermissionGroups.Contains(emp.Permission_Group))
        .And(emp => dept_values.Contains(emp.Department))
        .And(emp => emp.Resign_Reason == BasicCodeTypeConstant.Resign_Reason);
        IEnumerable<HRMS_Emp_Personal> HEP = paramTable.HEP_All.Where(predEmpPersonal);
        return HEP.Select(x => x.USER_GUID).Distinct().Count();
    }

    // 33. Tổng số ngày nghỉ phép cá nhân, nghỉ ốm và vắng mặt không phép
    private static int GetTotalPersonalSickAndAbsenteeismLeaveField(int personal_Cnt, int sickLeave, int absenteeism, int employee_Absences_5Days)
    {
        int total = personal_Cnt + sickLeave + absenteeism - employee_Absences_5Days;
        return total;
    }

    // 34. Trung bình số người xin nghỉ phép  (nghỉ việc riêng, nghỉ ốm và vắng mặt không phép)
    private static double GetAverageLeaveCountField(int sum_Personal_Cnt, int sum_Sick, int sum_Absent, int sum_Employee_Absences_5Days, int sum_Expected_Attendancet, int sum_Maternity)
    {
        double total = 0;
        int denominator = sum_Expected_Attendancet - sum_Employee_Absences_5Days - sum_Maternity;

        // Kiểm tra mẫu số có bằng 0 hay không
        if (denominator == 0)
            return total;

        total = (double)(sum_Personal_Cnt + sum_Sick + sum_Absent - sum_Employee_Absences_5Days) / denominator * 100;
        return total;
    }
    // 35  tổng số người dự kiến có mặt bằng cách cộng số người dự kiến có mặt của từng bộ phận lại với nhau
    #endregion

    #region get totall rows
    public async Task<OperationResult> GetTotalRows(HRDailyReportParam param, List<string> roleList, string userName)
    {
        OperationResult result = await BaseGetdata(param, roleList);

        if (!result.IsSuccess)
            return result;
        HRDailyReportResult res = result.Data as HRDailyReportResult;
        List<HRDailyReport> hrDailyReport = res.HRDailyReport;
        int totalAbsenteeism = hrDailyReport.Sum(x => x.Absenteeism);

        return new OperationResult
        {
            IsSuccess = true,
            Data = new HRDailyReportCount { QueryResult = hrDailyReport.Count, HeadCount = res.HeadCount, MonthlyAbsenteeism = totalAbsenteeism }
        };
    }

    private async Task<int> Get_Dept_values_Emp_Cnt(HRDailyReportParam param)
    {
        var result = await _repositoryAccessor.HRMS_Emp_Personal
                                    .FindAll(x => x.Factory == param.Factory &&
                                                x.Onboard_Date.Date <= param.Date.ToDateTime().Date &&
                                                (x.Resign_Date.HasValue && x.Resign_Date.Value.Date >= param.Date.ToDateTime().Date || !x.Resign_Date.HasValue) &&
                                                param.PermissionGroups.Any(per => per == x.Permission_Group))
                                    .ToListAsync();
        return result.Count;
    }

    // 3 table  ETH, HEP, HBL
    public (int Out_Cnt, int In_Cnt) Sum_Staff_Cnt(HRDailyReportTableRequest paramTable, string inputFactory, DateTime inputDate, List<string> dept_values, List<string> type_Code = null)
    {
        var query = paramTable.ETH
            .Join(paramTable.HEP,
                ETH => ETH.USER_GUID,
                emp => emp.USER_GUID,
                (ETH, emp) => new { ETH, emp })
            .Join(paramTable.HBL,
                joined => joined.emp.Position_Title,
                level => level.Level_Code,
                (joined, level) => new { joined.ETH, joined.emp, level })
            .Where(joined => !joined.ETH.Effective_Status &&
                            joined.ETH.Data_Source == "01" &&
                            joined.ETH.Effective_Date <= inputDate &&
                            type_Code.Contains(joined.level.Type_Code));

        int out_Cnt = query.Filter(j => j.ETH.Factory_Before == inputFactory && dept_values.Contains(j.ETH.Department_Before)).Select(x => new { x.ETH.USER_GUID, x.level.Level }).Distinct().Count();
        int in_Cnt = query.Filter(j => j.ETH.Factory_After == inputFactory && dept_values.Contains(j.ETH.Department_After)).Select(x => new { x.ETH.USER_GUID, x.level.Level }).Distinct().Count();


        return (out_Cnt, in_Cnt);
    }

    public int Sum_Leave_Cnt(HRDailyReportTableRequest paramTable, HRDailyReportParam param, List<string> dept_values, List<string> leave_List)
    {
        DateTime inputDate = Convert.ToDateTime(param.Date);
        IEnumerable<HRMS_Att_Leave_Maintain> leaveData = paramTable.HLM
            .Where(leave => leave.Leave_Date == inputDate);
        // Data Reading SQL 4.
        int leave_Sum = paramTable.HEP_All
        .FullOuterJoin(
            leaveData,
            a => a.USER_GUID,
            b => b.USER_GUID,
            (a, b, key) => new { HEP = a, LeaveMain = b }
        )
         .Where(x => x.HEP != null && x.LeaveMain != null &&
                    x.HEP.Factory == param.Factory &&
                    dept_values.Contains(x.HEP.Department) &&
                    x.HEP.Onboard_Date < inputDate &&
                    (x.HEP.Resign_Date >= inputDate || x.HEP.Resign_Date == null) &&
                    param.PermissionGroups.Contains(x.HEP.Permission_Group) &&
                    x.LeaveMain.Leave_Date == inputDate).
        Select(l => new
        {
            user_gui = l.HEP.USER_GUID,
            leaveCode = l.LeaveMain.Leave_code
        }).Distinct()
        .GroupBy(x => x.leaveCode)
        .Select(g => new
        {
            Leave_Sum = g.Sum(x => leave_List.Contains(x?.leaveCode) ? 1 : 0)
        })
        .Sum(x => x.Leave_Sum);

        var query = paramTable.ETH
            .Join(paramTable.HEP,
                ETH => ETH.USER_GUID,
                HEP => HEP.USER_GUID,
                (ETH, HEP) => new { ETH, HEP })
            .Join(paramTable.HLM,
                joined => joined.ETH.USER_GUID,
                leave => leave.USER_GUID,
                (joined, leave) => new { joined.ETH, joined.HEP, leave })
            .Where(joined =>
                            !joined.ETH.Effective_Status
                             && joined.ETH.Data_Source == "01"
                             && joined.ETH.Effective_Date <= inputDate
                             && param.PermissionGroups.Contains(joined.HEP.Permission_Group)
                             && joined.leave.Leave_Date == inputDate
                             && leave_List.Contains(joined.leave.Leave_code));
        int out_Cnt = query.Count(joined => joined.ETH.Factory_Before == param.Factory && dept_values.Contains(joined.ETH.Department_Before));
        int in_Cnt = query.Count(joined => joined.ETH.Factory_After == param.Factory && dept_values.Contains(joined.ETH.Department_After));

        leave_Sum = leave_Sum - out_Cnt + in_Cnt;
        return leave_Sum;
    }
    #endregion

    #region Get dropdownList condition search
    public async Task<List<KeyValuePair<string, string>>> GetListLevel(string language)
    {
        return await GetDataBasicCode(BasicCodeTypeConstant.Level, language);
    }

    public async Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language)
    {
        List<string> permissionGroups = await Query_Permission_GroupList(factory);
        List<KeyValuePair<string, string>> permisstionGroupsInBasicCode = await GetDataBasicCode(BasicCodeTypeConstant.PermissionGroup, language);
        return permisstionGroupsInBasicCode.Join(permissionGroups,
                        x => new { permissionGroup = x.Key },
                        permissionGroup => new { permissionGroup },
                        (x, y) => new KeyValuePair<string, string>(x.Key, x.Value)).ToList();
    }

    private async Task<List<string>> Query_Permission_GroupList(string factory)
      => await _repositoryAccessor.HRMS_Emp_Permission_Group.FindAll(x => x.Factory == factory).Select(x => x.Permission_Group).ToListAsync();

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