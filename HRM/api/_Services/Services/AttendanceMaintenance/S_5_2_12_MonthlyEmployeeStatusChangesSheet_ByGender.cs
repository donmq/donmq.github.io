

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
    public class S_5_2_12_MonthlyEmployeeStatusChangesSheet_ByGender : BaseServices, I_5_2_12_MonthlyEmployeeStatusChangesSheet_ByGender
    {
        public S_5_2_12_MonthlyEmployeeStatusChangesSheet_ByGender(DBContext dbContext) : base(dbContext)
        {
        }
        #region DownloadExcel
        public async Task<OperationResult> DownloadExcel(MonthlyEmployeeStatusChangesSheet_ByGender_Param param, string userName)
        {
            var data = await GetData(param);
            if (!data.Any())
                return new OperationResult(false, "No Data");
            List<Table> tables = new() { new("result", data) };
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
                "Resources\\Template\\AttendanceMaintenance\\5_2_12_MonthlyEmployeeStatusChangesSheet_ByGender\\Download.xlsx", 
                config
            );

            var result = new
            {
                excelResult.Result,
                data.Count
            };

            return new OperationResult(excelResult.IsSuccess, excelResult.Error, result);
        }
        #endregion

        public async Task<int> GetTotalRows(MonthlyEmployeeStatusChangesSheet_ByGender_Param param)
        {
            var data = await GetData(param);
            return data.Count;
        }

        #region GetData
        public async Task<List<MonthlyEmployeeStatusChangesSheet_ByGender_Report>> GetData(MonthlyEmployeeStatusChangesSheet_ByGender_Param param)
        {
            var paramTable = new MonthlyEmployeeStatusChangesSheet_ByGender_Data()
            {
                Employee_Dept = new List<string>(),
                HEP = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(true).ToListAsync(),
                HEIEH = await _repositoryAccessor.HRMS_Emp_IDcard_EmpID_History.FindAll(true).ToListAsync(),
                HOD = await _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Factory == param.Factory, true).ToListAsync(),
                HODL = await _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Factory == param.Factory, true).ToListAsync(),
                // Xác định danh sách các nhóm quyền hạn
                Foreign_Permission_list = await Query_Permission_Group_List(param.Factory, "Y")
            };

            // Lấy tât cả danh sách phòng ban
            var listEmployeeDept = AllDepartments(param, paramTable.HOD);

            var data = new List<MonthlyEmployeeStatusChangesSheet_ByGender_Report>();

            // Danh sách nhân viên đi kèm phòng ban
            foreach (var item in listEmployeeDept)
            {
                paramTable.currentDepartment = item.Employee_Dept;

                // Lấy thông tin phòng ban trực thuộc phòng ban cùng cấp || cấp bậc nhỏ hơn
                var deparmentsByLevel = GetDepartmentHierarchy(param, paramTable);

                item.Parent_Department_Level = item.OrgLevel;
                item.Begin_Cnt = Total_Begin_Count(param, paramTable, deparmentsByLevel);
                item.NewHires_Cnt = Total_NewHires_Count(param, paramTable, deparmentsByLevel);
                item.Male_Cnt = Total_Employee_Resignations_ByGender_Count(param, paramTable, "M", item.Employee_Dept);
                item.Female_Cnt = Total_Employee_Resignations_ByGender_Count(param, paramTable, "F", item.Employee_Dept, deparmentsByLevel);


                // Lấy Thông tin phòng ban [Code , Name]
                var department = Query_Department_Report(param, paramTable);

                // Tổng số 
                var total_Headcount = item.Begin_Cnt + item.NewHires_Cnt;
                double turnover_Rate = 0;

                // Tỷ lệ luân chuyển
                if (total_Headcount != 0 && (total_Headcount - item.Male_Cnt - item.Female_Cnt) != 0)
                    turnover_Rate = (double)(item.Male_Cnt + item.Female_Cnt) / (total_Headcount - item.Male_Cnt - item.Female_Cnt) * 100;

                if ((total_Headcount + item.Male_Cnt + item.Female_Cnt + turnover_Rate) != 0)
                    data.Add(new()
                    {
                        Department = $"{item.OrgLevel} - {department.Key}",
                        Department_Name = department.Value,
                        Total_Headcount = total_Headcount,
                        Male = item.Male_Cnt,
                        Female = item.Female_Cnt,
                        Turnover_Rate = turnover_Rate + "%",
                    });
            };

            return data;
        }

        #endregion

        #region DepartmentHierarchy
        /// <summary>
        /// Lấy danh sách các phòng ban trực thuộc phòng ban với level hiện tại
        /// </summary>
        /// <param name="param"></param>
        /// <param name="HOD"> Danh sách phòng ban</param>
        /// <param name="currentDepartment"> Phòng ban có level hiện tại </param>
        /// <returns></returns>
        private static List<MonthlyEmployeeStatusChangesSheet_ByGender_Departments> AllDepartments(MonthlyEmployeeStatusChangesSheet_ByGender_Param param, List<HRMS_Org_Department> HOD)
        {

            var departments = HOD
                .Where(d => d.IsActive)
                .Where(d => Int16.Parse(d.Org_Level) == param.Level ||
                            (Int16.Parse(d.Org_Level) >= param.Level && d.Upper_Department == null) ||
                            (Int16.Parse(d.Org_Level) > param.Level && HOD
                                    .Where(ud => ud.IsActive && Int16.Parse(ud.Org_Level) < param.Level)
                                .Select(ud => ud.Department_Code)
                                .Contains(d.Upper_Department)))
                .Select(x => new MonthlyEmployeeStatusChangesSheet_ByGender_Departments
                {
                    Employee_Dept = x.Department_Code,
                    DepartmentName = x.Department_Name,
                    Parent_Department = x.Upper_Department,
                    OrgLevel = x.Org_Level
                })
                .ToList();
            return departments.OrderBy(x => x.Employee_Dept).ToList();
        }

        /// <summary>
        /// Lấy phân cấp phòng ban con || cùng cấp bậc
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="departmentCode"></param>
        /// <returns></returns>
        private static List<MonthlyEmployeeStatusChangesSheet_ByGender_Departments> GetDepartmentHierarchy(MonthlyEmployeeStatusChangesSheet_ByGender_Param param, MonthlyEmployeeStatusChangesSheet_ByGender_Data paramTable)
        {
            var departmentHierarchy = new List<MonthlyEmployeeStatusChangesSheet_ByGender_Departments>();

            var initialDepartment = paramTable.HOD
                .Where(d => d.IsActive == true && d.Factory == param.Factory && d.Department_Code == paramTable.currentDepartment)
                .Select(x => new MonthlyEmployeeStatusChangesSheet_ByGender_Departments
                {
                    Employee_Dept = x.Department_Code,
                    DepartmentName = x.Department_Name,
                    Parent_Department = x.Upper_Department,
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

        private static void AddSubDepartments(
                        MonthlyEmployeeStatusChangesSheet_ByGender_Departments department,
                        List<HRMS_Org_Department> HOD,
                        List<MonthlyEmployeeStatusChangesSheet_ByGender_Departments> departmentHierarchy,
                        string factory)
        {
            var subDepartments = HOD
                .Where(x => x.IsActive == true && x.Factory == factory && x.Upper_Department == department.Employee_Dept)
                .Select(x => new MonthlyEmployeeStatusChangesSheet_ByGender_Departments
                {
                    Employee_Dept = x.Department_Code,
                    DepartmentName = x.Department_Name,
                    Parent_Department = x.Upper_Department,
                    OrgLevel = x.Org_Level
                })
                .ToList();

            foreach (var subDept in subDepartments)
            {
                if (subDept.Employee_Dept != subDept.Parent_Department)
                {
                    departmentHierarchy.Add(subDept);
                    AddSubDepartments(subDept, HOD, departmentHierarchy, factory);
                }
            }
        }
        #endregion

        #region Employee Resignations

        /// <summary>
        /// Tổng số nhân viên mới bắt đầu
        /// </summary>
        /// <param name="param"></param>
        /// <param name="paramTable"></param>
        /// <returns></returns>
        public int Total_Begin_Count(MonthlyEmployeeStatusChangesSheet_ByGender_Param param, MonthlyEmployeeStatusChangesSheet_ByGender_Data paramTable, List<MonthlyEmployeeStatusChangesSheet_ByGender_Departments> deparmentsByLevel)
        {
            // Số lượng danh sách nhân viên hiện tại
            var Begin_Cnt = paramTable.HEP.FindAll(x => x.Factory == param.Factory
                && deparmentsByLevel.Any(z => z.Employee_Dept == x.Department)
                && x.Onboard_Date < param.FirstDate.ToDateTime()
                && (x.Resign_Date >= param.FirstDate.ToDateTime() || x.Resign_Date == null)
                && param.PermissionGroup.Contains(x.Permission_Group))
            .Select(x => x.USER_GUID)
            .ToList();

            // Những nhân viên hiện tại đang phân công/hỗ trợ
            var Foreign_Cnt1 = paramTable.HEP.FindAll(x => x.Assigned_Factory == param.Factory
                    && deparmentsByLevel.Any(z => z.Employee_Dept == x.Assigned_Department)
                    && x.Onboard_Date < param.FirstDate.ToDateTime()
                    && (x.Resign_Date >= param.FirstDate.ToDateTime() || x.Resign_Date == null)
                    && param.PermissionGroup.Contains(x.Permission_Group)
                    && paramTable.Foreign_Permission_list.Contains(x.Permission_Group))
                .Select(x => x.USER_GUID)
                .ToList();

            // Những nhân viên trước đây từng được phân công/hỗ trợ
            var Foreign_Cnt2 = paramTable.HEIEH.FindAll(x => x.Assigned_Factory == param.Factory
                    && deparmentsByLevel.Any(z => z.Employee_Dept == x.Assigned_Department)
                    && x.Onboard_Date < param.FirstDate.ToDateTime()
                    && (x.Resign_Date >= param.FirstDate.ToDateTime() || x.Resign_Date == null))
                .Join(paramTable.HEP.Where(x => param.PermissionGroup.Contains(x.Permission_Group) && paramTable.Foreign_Permission_list.Contains(x.Permission_Group)),
                    x => x.USER_GUID,
                    y => y.USER_GUID,
                    (x, y) => new { HEIEH = x, HEP = y })
                .Select(x => x.HEIEH.USER_GUID)
                .ToList();

            return Begin_Cnt.Union(Foreign_Cnt1).Union(Foreign_Cnt2).Distinct().Count();
        }

        public int Total_NewHires_Count(
            MonthlyEmployeeStatusChangesSheet_ByGender_Param param,
            MonthlyEmployeeStatusChangesSheet_ByGender_Data paramTable,
            List<MonthlyEmployeeStatusChangesSheet_ByGender_Departments> deparmentsByLevel)
        {
            // Tính toán số lượng nhân viên mới tuyển dụng trong khoảng thời gian xác định
            var NewHires_Cnt = paramTable.HEP.FindAll(x => x.Factory == param.Factory
                    && deparmentsByLevel.Any(z => z.Employee_Dept == x.Department)
                    && x.Onboard_Date >= param.FirstDate.ToDateTime() && x.Onboard_Date <= Convert.ToDateTime(param.LastDate)
                    && param.PermissionGroup.Contains(x.Permission_Group))
                .Select(x => x.USER_GUID)
                .ToList();

            // Tính toán số lượng nhân viên đang được phân công/hỗ trợ trong khoảng thời gian xác định
            var NewForeign_Cnt1 = paramTable.HEP.FindAll(x => x.Assigned_Factory == param.Factory
                    && deparmentsByLevel.Any(z => z.Employee_Dept == x.Assigned_Department)
                    && x.Onboard_Date >= param.FirstDate.ToDateTime() && x.Onboard_Date <= Convert.ToDateTime(param.LastDate)
                    && param.PermissionGroup.Contains(x.Permission_Group)
                    && paramTable.Foreign_Permission_list.Contains(x.Permission_Group))
                .Select(x => x.USER_GUID)
                .ToList();

            // Tính toán số lượng nhân viên đã từng được phân công/hỗ trợ khoảng thời gian xác định
            var NewForeign_Cnt2 = paramTable.HEIEH.Where(x => x.Assigned_Factory == param.Factory
                    && deparmentsByLevel.Any(z => z.Employee_Dept == x.Assigned_Department)
                    && x.Onboard_Date >= param.FirstDate.ToDateTime() && x.Onboard_Date <= Convert.ToDateTime(param.LastDate))
                .Join(paramTable.HEP.Where(x => param.PermissionGroup.Contains(x.Permission_Group) && paramTable.Foreign_Permission_list.Contains(x.Permission_Group)),
                    x => x.USER_GUID,
                    y => y.USER_GUID,
                    (x, y) => new { HEIEH = x, HEP = y })
                .Select(x => x.HEIEH.USER_GUID)
                .ToList();

            return NewHires_Cnt.Union(NewForeign_Cnt1).Union(NewForeign_Cnt2).Distinct().Count();
        }

        private static int Total_Employee_Resignations_ByGender_Count(
            MonthlyEmployeeStatusChangesSheet_ByGender_Param param,
            MonthlyEmployeeStatusChangesSheet_ByGender_Data paramTable,
            string gender, string currentDepartment,
            List<MonthlyEmployeeStatusChangesSheet_ByGender_Departments> departmentHierarchys = null)
        {
            // Số lượng nhân viên đã nghỉ việc
            var Employee_Resign_Cnt = paramTable.HEP.FindAll(
                x => x.Factory == param.Factory
                && (gender == "M" ? x.Department == currentDepartment : departmentHierarchys.Any(z => z.Employee_Dept == x.Department))
                && x.Resign_Date >= param.FirstDate.ToDateTime() && x.Resign_Date <= param.LastDate.ToDateTime()
                && x.Gender == gender
                && param.PermissionGroup.Contains(x.Permission_Group))
                .Select(x => x.USER_GUID)
                .ToList();

            // Tính toán số lượng nhân viên đang được phân công/hỗ trợ trong khoảng thời gian xác định
            var Foreign_Cnt1 = paramTable.HEP.FindAll(x => x.Assigned_Factory == param.Factory
                && (gender == "M" ? paramTable.Employee_Dept.Contains(x.Assigned_Department) : departmentHierarchys.Any(z => z.Employee_Dept == x.Assigned_Department))
                && x.Resign_Date.HasValue
                && x.Resign_Date.Value >= param.FirstDate.ToDateTime() && x.Resign_Date <= param.LastDate.ToDateTime()
                && x.Gender == gender
                && param.PermissionGroup.Contains(x.Permission_Group)
                && paramTable.Foreign_Permission_list.Contains(x.Permission_Group))
            .Select(x => x.USER_GUID)
            .ToList();

            // Tính toán số lượng nhân viên đã từng được phân công/hỗ trợ khoảng thời gian xác định
            var Foreign_Cnt2 = paramTable.HEIEH.Where(x => x.Assigned_Factory == param.Factory
                        && (gender == "M" ? paramTable.Employee_Dept.Contains(x.Assigned_Department) : departmentHierarchys.Any(z => z.Employee_Dept == x.Assigned_Department))
                        && x.Resign_Date.HasValue
                        && x.Resign_Date >= param.FirstDate.ToDateTime() && x.Resign_Date <= param.LastDate.ToDateTime())
                    .Join(paramTable.HEP.Where(x => x.Gender == gender && param.PermissionGroup.Contains(x.Permission_Group)
                                                                       && paramTable.Foreign_Permission_list.Contains(x.Permission_Group)),
                        x => x.USER_GUID,
                        y => y.USER_GUID,
                        (x, y) => new { HEIEH = x, HEP = y })
                .Select(x => x.HEIEH.USER_GUID)
                .ToList();

            return Employee_Resign_Cnt.Union(Foreign_Cnt1).Union(Foreign_Cnt2).Distinct().Count();
        }
        #endregion

        #region Query_Department_Report

        /// <summary>
        /// Lấy danh sách phòng ban
        /// </summary>
        /// <param name="param"></param>
        /// <param name="paramTable"></param>
        /// <returns></returns>
        private static KeyValuePair<string, string> Query_Department_Report(
            MonthlyEmployeeStatusChangesSheet_ByGender_Param param,
            MonthlyEmployeeStatusChangesSheet_ByGender_Data paramTable)
        {
            return paramTable.HOD.Where(x => x.Department_Code == paramTable.currentDepartment)
                .GroupJoin(paramTable.HODL.Where(x => x.Language_Code.ToLower() == param.Lang.ToLower() && x.Department_Code == paramTable.currentDepartment),
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
        public async Task<List<KeyValuePair<string, string>>> GetListFactory(List<string> roleList, string language) => await Query_Factory_AddList(roleList, language);
        public async Task<List<KeyValuePair<string, string>>> GetListLevel(string lang) => await GetDataBasicCode(BasicCodeTypeConstant.Level, lang);
        public async Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string lang)
        {
            var permissionGroup = await Query_Permission_List(factory);
            var result = await Query_HRMS_Basic_Code(BasicCodeTypeConstant.PermissionGroup, permissionGroup.Select(x => x.Permission_Group).ToList(), lang);
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