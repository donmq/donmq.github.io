using System.Globalization;
using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Helper.Utilities;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance
{
    public class S_5_2_11_MonthlyEmployeeStatusChangesSheetByDepartment : BaseServices, I_5_2_11_MonthlyEmployeeStatusChangesSheet_ByDepartment
    {
        public S_5_2_11_MonthlyEmployeeStatusChangesSheetByDepartment(DBContext dbContext) : base(dbContext)
        {
        }
        #region DownloadExcel
        public async Task<OperationResult> DownloadExcel(MonthlyEmployeeStatusParam param, string userName)
        {
            var data = await GetData(param);
            if (!data.Any())
                return new OperationResult(false, "No Data");

            int totalBeginCnt = data.Sum(x => x.Begin_Cnt);
            int totalNewHiresCnt = data.Sum(x => x.NewHires_Cnt);
            int totalResignCnt = data.Sum(x => x.Resign_Cnt);
            int totalEndMonthCnt = data.Sum(x => x.End_Month_Cnt);
            double totalTurnoverRate = 0;
            if (totalEndMonthCnt != 0)
                totalTurnoverRate = (double)totalResignCnt / totalEndMonthCnt * 100;

            var totalRow = new MonthlyEmployeeStatus_ByDepartmentReport
            {
                Department = "總計",
                Department_Name = "TOTAL",
                Begin_Cnt = totalBeginCnt,
                NewHires_Cnt = totalNewHiresCnt,
                Resign_Cnt = totalResignCnt,
                End_Month_Cnt = totalEndMonthCnt,
                Turnover_Rate = totalTurnoverRate.ToString("0.##") + "%"
            };

            data.Add(totalRow);

            List<Table> tables = new()
            {
                new Table("result", data)
            };

            // Chuyển chuỗi thành đối tượng DateTime và Format lại theo định dạng "yyyy/MM"
            DateTime date = DateTime.ParseExact(param.YearMonth, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            string formattedYearMonth = date.ToString("yyyy/MM");

            List<Cell> cells = new()
                {
                    new Cell("B1", param.Factory),
                    new Cell("D1", formattedYearMonth),
                    new Cell("F1", param.LevelName),
                    new Cell("H1", string.Join(", ", param.PermissionName)),
                    new Cell("B2", userName),
                    new Cell("D2", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")),
                };
            ConfigDownload config = new() { IsAutoFitColumn = true };
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                tables, 
                cells, 
                "Resources\\Template\\AttendanceMaintenance\\5_2_11_MonthlyEmployeeStatusChangesSheet_ByDepartment\\Download.xlsx", 
                config
            );
            var dataResult = new
            {
                excelResult.Result,
                Count = data.Count - 1
            };
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, dataResult);
        }
        #endregion

        public async Task<int> GetTotalRows(MonthlyEmployeeStatusParam param)
        {
            List<MonthlyEmployeeStatus_ByDepartmentReport> data = await GetData(param);
            return data.Count;
        }

        #region GetData
        public async Task<List<MonthlyEmployeeStatus_ByDepartmentReport>> GetData(MonthlyEmployeeStatusParam param)
        {
            TableParam paramTable = new()
            {
                Employee_Dept = new List<string>(),
                HEP = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(true).ToListAsync(),
                HEIEH = await _repositoryAccessor.HRMS_Emp_IDcard_EmpID_History.FindAll(true).ToListAsync(),
                HOD = await _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Factory == param.Factory, true).ToListAsync(),
                HODL = await _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Factory == param.Factory, true).ToListAsync(),
                // Xác định danh sách các nhóm quyền hạn
                Foreign_Permission_list = await Query_Permission_Group_List(param.Factory, "Y"),
            };

            List<Department> listEmployeeDept = Employee_Dept(param, paramTable);

            List<MonthlyEmployeeStatus_ByDepartmentReport> data = new();

            foreach (Department item in listEmployeeDept)
            {
                paramTable.Employee_Dept.Add(item.Employee_Dept);

                // Lấy thông tin cấu trúc phòng ban cho Employee_Dept
                List<Department> departmentHierarchy = DepartmentHierarchy(param, paramTable);

                paramTable.Employee_Dept = departmentHierarchy.Select(x => x.Employee_Dept).ToList();
                item.Begin_Cnt = Total_Begin_Count(param, paramTable);
                item.NewHires_Cnt = Total_NewHires_Count(param, paramTable);
                item.Resign_Cnt = Total_Resign_Count(param, paramTable);
                paramTable.Employee_Dept = new List<string> { item.Employee_Dept };
                KeyValuePair<string, string> department = Query_Department_Report(param, paramTable, item.OrgLevel);

                double turnover_Rate = 0;
                int end_Month_Cnt = item.Begin_Cnt + item.NewHires_Cnt - item.Resign_Cnt;
                // if (item.TotalNewHiresCnt != 0)
                if (end_Month_Cnt != 0)
                {
                    turnover_Rate = (double)item.Resign_Cnt / end_Month_Cnt * 100;
                }

                if (item.Begin_Cnt + item.NewHires_Cnt + item.Resign_Cnt + turnover_Rate + end_Month_Cnt != 0)
                {
                    data.Add(new()
                    {
                        Department = $"{item.OrgLevel} - {item.Employee_Dept}",
                        Department_Name = department.Value,
                        Begin_Cnt = item.Begin_Cnt,
                        NewHires_Cnt = item.NewHires_Cnt,
                        Resign_Cnt = item.Resign_Cnt,
                        Turnover_Rate = turnover_Rate.ToNumberHasTwoDecimal() + "%",
                        End_Month_Cnt = end_Month_Cnt,
                    });
                }
                paramTable.Employee_Dept.Clear();
            };



            return data.OrderBy(x => x.Department).ToList();
        }
        #endregion

        #region Number Of Employees At The Beginning Of The Month
        public int Total_Begin_Count(MonthlyEmployeeStatusParam param, TableParam paramTable)
        {
            // Số lượng danh sách nhân viên hiện tại
            List<string> Begin_Cnt = paramTable.HEP.FindAll(x => x.Factory == param.Factory
                && paramTable.Employee_Dept.Contains(x.Department)
                && x.Onboard_Date < Convert.ToDateTime(param.FirstDate)
                && (x.Resign_Date >= Convert.ToDateTime(param.FirstDate) || x.Resign_Date == null)
                && param.PermissionGroup.Contains(x.Permission_Group))
            .Select(x => x.USER_GUID)
            .ToList();

            // Những nhân viên hiện tại đang phân công/hỗ trợ
            List<string> Foreign_Cnt1 = paramTable.HEP.FindAll(x => x.Assigned_Factory == param.Factory
                    && paramTable.Employee_Dept.Contains(x.Assigned_Department)
                    && x.Onboard_Date < Convert.ToDateTime(param.FirstDate)
                    && (x.Resign_Date >= Convert.ToDateTime(param.FirstDate) || x.Resign_Date == null)
                    && param.PermissionGroup.Contains(x.Permission_Group)
                    && paramTable.Foreign_Permission_list.Contains(x.Permission_Group))
                .Select(x => x.USER_GUID)
                .ToList();

            // Những nhân viên trước đây từng được phân công/hỗ trợ
            List<string> Foreign_Cnt2 = paramTable.HEIEH.FindAll(x => x.Assigned_Factory == param.Factory
                    && paramTable.Employee_Dept.Contains(x.Assigned_Department)
                    && x.Onboard_Date < Convert.ToDateTime(param.FirstDate)
                    && (x.Resign_Date >= Convert.ToDateTime(param.FirstDate) || x.Resign_Date == null))
                .Join(paramTable.HEP.Where(x => param.PermissionGroup.Contains(x.Permission_Group) && paramTable.Foreign_Permission_list.Contains(x.Permission_Group)),
                    x => x.USER_GUID,
                    y => y.USER_GUID,
                    (x, y) => new { HEIEH = x, HEP = y })
                .Select(x => x.HEP.USER_GUID)
                .ToList();

            return Begin_Cnt.Union(Foreign_Cnt1).Union(Foreign_Cnt2).Distinct().Count();
        }
        #endregion

        #region New Hires This Month
        public int Total_NewHires_Count(MonthlyEmployeeStatusParam param, TableParam paramTable)
        {
            // Tính toán số lượng nhân viên mới tuyển dụng trong khoảng thời gian xác định
            List<string> NewHires_Cnt = paramTable.HEP.FindAll(x => x.Factory == param.Factory
                    && paramTable.Employee_Dept.Contains(x.Department)
                    && x.Onboard_Date >= Convert.ToDateTime(param.FirstDate) && x.Onboard_Date <= Convert.ToDateTime(param.LastDate)
                    && param.PermissionGroup.Contains(x.Permission_Group))
                .Select(x => x.USER_GUID)
                .ToList();

            // Tính toán số lượng nhân viên đang được phân công/hỗ trợ trong khoảng thời gian xác định
            List<string> NewForeign_Cnt1 = paramTable.HEP.FindAll(x => x.Assigned_Factory == param.Factory
                    && paramTable.Employee_Dept.Contains(x.Assigned_Department)
                    && x.Onboard_Date >= Convert.ToDateTime(param.FirstDate) && x.Onboard_Date <= Convert.ToDateTime(param.LastDate)
                    && param.PermissionGroup.Contains(x.Permission_Group)
                    && paramTable.Foreign_Permission_list.Contains(x.Permission_Group))
                .Select(x => x.USER_GUID)
                .ToList();

            // Tính toán số lượng nhân viên đã từng được phân công/hỗ trợ khoảng thời gian xác định
            List<string> NewForeign_Cnt2 = paramTable.HEIEH.Where(x => x.Assigned_Factory == param.Factory
                    && paramTable.Employee_Dept.Contains(x.Assigned_Department)
                    && x.Onboard_Date >= Convert.ToDateTime(param.FirstDate) && x.Onboard_Date <= Convert.ToDateTime(param.LastDate))
                .Join(paramTable.HEP.Where(x => param.PermissionGroup.Contains(x.Permission_Group) && paramTable.Foreign_Permission_list.Contains(x.Permission_Group)),
                    x => x.USER_GUID,
                    y => y.USER_GUID,
                    (x, y) => new { HEIEH = x, HEP = y })
                .Select(x => x.HEP.USER_GUID)
                .ToList();

            return NewHires_Cnt.Union(NewForeign_Cnt1).Union(NewForeign_Cnt2).Distinct().Count();
        }
        #endregion

        #region Resignations This Month
        private static int Total_Resign_Count(MonthlyEmployeeStatusParam param, TableParam paramTable)
        {
            // Tính toán số lượng nhân viên nghỉ việc trong khoảng thời gian xác định
            List<string> Resign_Cnt = paramTable.HEP.FindAll(x => x.Factory == param.Factory
                    && paramTable.Employee_Dept.Contains(x.Department)
                    && x.Resign_Date >= Convert.ToDateTime(param.FirstDate) && x.Resign_Date <= Convert.ToDateTime(param.LastDate)
                    && param.PermissionGroup.Contains(x.Permission_Group))
                .Select(x => x.USER_GUID)
                .ToList();

            // Tính toán số lượng nhân viên đang được phân công/hỗ trợ
            List<string> ResignForeign_Cnt1 = paramTable.HEP.FindAll(x => x.Assigned_Factory == param.Factory
                    && paramTable.Employee_Dept.Contains(x.Assigned_Department)
                    && x.Resign_Date >= Convert.ToDateTime(param.FirstDate) && x.Resign_Date <= Convert.ToDateTime(param.LastDate)
                    && param.PermissionGroup.Contains(x.Permission_Group)
                    && paramTable.Foreign_Permission_list.Contains(x.Permission_Group))
                .Select(x => x.USER_GUID)
                .ToList();

            // Tính toán số lượng nhân viên đã từng phân công/hỗ trợ
            List<string> ResignForeign_Cnt2 = paramTable.HEIEH.Where(x => x.Assigned_Factory == param.Factory
                    && paramTable.Employee_Dept.Contains(x.Assigned_Department)
                    && x.Resign_Date >= Convert.ToDateTime(param.FirstDate) && x.Resign_Date <= Convert.ToDateTime(param.LastDate))
                .Join(paramTable.HEP.Where(x => param.PermissionGroup.Contains(x.Permission_Group) && paramTable.Foreign_Permission_list.Contains(x.Permission_Group)),
                    x => x.USER_GUID,
                    y => y.USER_GUID,
                    (x, y) => new { HEIEH = x, HEP = y })
                .Select(x => x.HEP.USER_GUID)
                .ToList();

            return Resign_Cnt.Union(ResignForeign_Cnt1).Union(ResignForeign_Cnt2).Distinct().Count();
        }
        #endregion

        #region DepartmentHierarchy
        private static List<Department> DepartmentHierarchy(MonthlyEmployeeStatusParam param, TableParam paramTable)
        {
            List<Department> departmentHierarchy = new List<Department>();

            Department initialDepartment = paramTable.HOD
                .Where(d => d.IsActive && d.Factory == param.Factory && paramTable.Employee_Dept.Contains(d.Department_Code))
                .Select(x => new Department
                {
                    Employee_Dept = x.Department_Code,
                    DepartmentName = x.Department_Name,
                    UpperDepartment = x.Upper_Department,
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

        private static void AddSubDepartments(Department department, List<HRMS_Org_Department> HOD, List<Department> departmentHierarchy, string factory)
        {
            List<Department> subDepartments = HOD
                .Where(x => x.IsActive && x.Factory == factory && x.Upper_Department == department.Employee_Dept)
                .Select(x => new Department
                {
                    Employee_Dept = x.Department_Code,
                    DepartmentName = x.Department_Name,
                    UpperDepartment = x.Upper_Department,
                    OrgLevel = x.Org_Level
                })
                .ToList();

            foreach (Department subDept in subDepartments)
            {
                if (subDept.Employee_Dept != subDept.UpperDepartment)
                {
                    departmentHierarchy.Add(subDept);
                    AddSubDepartments(subDept, HOD, departmentHierarchy, factory);
                }
            }
        }
        #endregion

        #region Employee_Dept
        public List<Department> Employee_Dept(MonthlyEmployeeStatusParam param, TableParam paramTable)
        {
            List<string> departmentCode = paramTable.HOD
                .Where(x => x.IsActive && x.Factory == param.Factory && int.Parse(x.Org_Level) < param.Level)
                .Select(x => x.Department_Code)
                .ToList();

            List<Department> result = paramTable.HOD
                .Where(x => x.IsActive && (int.Parse(x.Org_Level) == param.Level
                    || (int.Parse(x.Org_Level) >= param.Level && x.Upper_Department is null)
                    || (int.Parse(x.Org_Level) > param.Level && departmentCode.Contains(x.Upper_Department))))
                .Select(x => new Department
                {
                    Employee_Dept = x.Department_Code,
                    DepartmentName = x.Department_Name,
                    UpperDepartment = x.Upper_Department,
                    OrgLevel = x.Org_Level
                })
                .ToList();

            return result;
        }
        #endregion

        #region Query_Department_Report
        private static KeyValuePair<string, string> Query_Department_Report(MonthlyEmployeeStatusParam param, TableParam paramTable, string Level)
        {
            return paramTable.HOD.Where(x => paramTable.Employee_Dept.Contains(x.Department_Code) && x.Org_Level == Level)
                .GroupJoin(paramTable.HODL.Where(x => x.Language_Code.ToLower() == param.Lang.ToLower() && paramTable.Employee_Dept.Contains(x.Department_Code)),
                    x => new { x.Department_Code },
                    y => new { y.Department_Code },
                    (x, y) => new { HOD = x, HODL = y })
                    .SelectMany(x => x.HODL.DefaultIfEmpty(),
                    (x, y) => new { x.HOD, HBCL = y })
                .Select(x => new KeyValuePair<string, string>(x.HOD.Department_Code, x.HBCL != null ? x.HBCL.Name : x.HOD.Department_Name))
                .FirstOrDefault();
        }
        #endregion

        #region GetList
        public async Task<List<KeyValuePair<string, string>>> GetListFactory(List<string> roleList, string language)
        {
            return await Query_Factory_AddList(roleList, language);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListLevel(string lang)
        {
            return await GetDataBasicCode(BasicCodeTypeConstant.Level, lang);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string lang)
        {
            List<HRMS_Emp_Permission_Group> permissionGroup = await Query_Permission_List(factory);
            List<KeyValuePair<string, string>> result = await Query_HRMS_Basic_Code(BasicCodeTypeConstant.PermissionGroup, permissionGroup.Select(x => x.Permission_Group).ToList(), lang);
            return result;
        }

        private async Task<List<KeyValuePair<string, string>>> Query_HRMS_Basic_Code(string type_Seq, List<string> permissionGroup, string lang)
        {
            return await _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => x.Type_Seq == type_Seq && permissionGroup.Contains(x.Code), true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == lang.ToLower(), true),
                    HBC => new { HBC.Type_Seq, HBC.Code },
                    HBCL => new { HBCL.Type_Seq, HBCL.Code },
                    (HBC, HBCL) => new { HBC, HBCL })
                    .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (prev, HBCL) => new { prev.HBC, HBCL })
                .Select(x => new KeyValuePair<string, string>(x.HBC.Code, $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"))
                .ToListAsync();
        }
        #endregion
    }
}