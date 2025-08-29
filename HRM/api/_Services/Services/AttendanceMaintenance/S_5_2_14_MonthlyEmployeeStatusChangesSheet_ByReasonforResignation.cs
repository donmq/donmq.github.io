using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance
{
    public class S_5_2_14_MonthlyEmployeeStatusChangesSheet_ByReasonforResignation : BaseServices, I_5_2_14_MonthlyEmployeeStatusChangesSheet_ByReasonforResignation
    {
        public S_5_2_14_MonthlyEmployeeStatusChangesSheet_ByReasonforResignation(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Result> GetTotalRecords(Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Param param)
        {
            return await GetTableInformation(param);
        }

        public async Task<OperationResult> Export(Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Param param)
        {
            // 1. Lấy dữ liệu chính
            var result = await GetTableInformation(param, true);

            // 2. Lấy thông tin theo danh sách Reasons
            var header = new Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Excel_Header
            {
                Factory = param.Factory,
                PrintBy = param.PrintBy,
                YearMonth = param.YearMonth,
                PrintDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                PermisionGroups = await GetListCodeName(param.PermisionGroups, param.Language, BasicCodeTypeConstant.PermissionGroup),
            };

            // // 3. Chuyển đổi dữ liệu thành dữ liệu In (CELL)
            var cellHeader = ConvertToExcelTable(header, result);

            // 4. in Dữ liệu
            var table = new List<Table>() { new("result", result.Exports) };
            ConfigDownload config = new() { IsAutoFitColumn = true };
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                table, 
                cellHeader, 
                "Resources\\Template\\AttendanceMaintenance\\5_2_14_MonthlyEmployeeStatusChangesSheetByReasonforResignation\\Download.xlsx", 
                config
            );
            // 5. Trả data in
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }

        #region Get Dropdown List
        public async Task<List<KeyValuePair<string, string>>> GetFactories(List<string> roleList, string language) => await Query_Factory_AddList(roleList, language);
        public async Task<List<KeyValuePair<string, string>>> GetPermistionGroups(string factory, string language) => await Query_BasicCode_PermissionGroup(factory, language);
        #endregion

        #region GetData
        private async Task<Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Result> GetTableInformation(Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Param param, bool isExport = false)
        {
            // Phần chung
            var predicatePermission = PredicateBuilder.New<HRMS_Emp_Permission_Group>(x => x.Foreign_Flag == "N" && param.PermisionGroups.Any(z => z == x.Permission_Group));
            if (!string.IsNullOrWhiteSpace(param.Factory))
                predicatePermission.And(x => x.Factory == param.Factory);
            var local_Permission_list = await _repositoryAccessor.HRMS_Emp_Permission_Group.FindAll(predicatePermission, true).Select(x => x.Permission_Group).ToListAsync();

            var firstDateOfMonth = GetDateTimeOfMonth(param.YearMonth);
            var lastDateOfMonth = GetDateTimeOfMonth(param.YearMonth, true);

            var queryParam = new Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Query_Param()
            {
                Factory = param.Factory,
                EmployeeDept = await GetEmployeeDept(param.Factory, local_Permission_list),
                Personals = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(x => x.Assigned_Factory == param.Factory, true).ToListAsync(),
                EmpIdHistory = await _repositoryAccessor.HRMS_Emp_IDcard_EmpID_History.FindAll(x => x.Assigned_Factory == param.Factory, true).ToListAsync(),

                Local_Permission_list = local_Permission_list
            };

            var data = new List<Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Value>();
            for (int row = 1; row <= 2; row++)
            {
                var value = new Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Value();
                if (row == 1)
                {
                    value.HeaderTitle = "AttendanceMaintenance.MonthlyEmployeeStatusChangesSheetByReasonforResignation.TWExpatriate";
                    value.NumberOfEmployeesAt = await GetNumberOfEmployeesAt(param.Factory, firstDateOfMonth);
                    value.NewHiresThisMonth = await GetNewHiresThisMonth(param.Factory, firstDateOfMonth, lastDateOfMonth);
                    value.ResignationsThisMonth = await GetResignationsThisMonth(param.Factory, firstDateOfMonth, lastDateOfMonth);
                    value.TotalNumberOfEmployeesAt = value.NumberOfEmployeesAt + value.NewHiresThisMonth - value.ResignationsThisMonth;
                }
                else
                {
                    value.HeaderTitle = "AttendanceMaintenance.MonthlyEmployeeStatusChangesSheetByReasonforResignation.CNExpatriate";
                    value.NumberOfEmployeesAt = await GetNumberOfEmployeesAt(param.Factory, firstDateOfMonth, false);
                    value.NewHiresThisMonth = await GetNewHiresThisMonth(param.Factory, firstDateOfMonth, lastDateOfMonth, false);
                    value.ResignationsThisMonth = await GetResignationsThisMonth(param.Factory, firstDateOfMonth, lastDateOfMonth, false);
                    value.TotalNumberOfEmployeesAt = value.NumberOfEmployeesAt + value.NewHiresThisMonth - value.ResignationsThisMonth;
                }
                data.Add(value);
            }

            data.Add(new Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Value()
            {
                HeaderTitle = "AttendanceMaintenance.MonthlyEmployeeStatusChangesSheetByReasonforResignation.TotalNumberOfExpatriate",
                NumberOfEmployeesAt = data[0].NumberOfEmployeesAt + data[1].NumberOfEmployeesAt,
                NewHiresThisMonth = data[0].NewHiresThisMonth + data[1].NewHiresThisMonth,
                ResignationsThisMonth = data[0].ResignationsThisMonth + data[1].ResignationsThisMonth,
                TotalNumberOfEmployeesAt = data[0].TotalNumberOfEmployeesAt + data[1].TotalNumberOfEmployeesAt
            });

            // danh sách lý do nghỉ việc 
            var totalRecords = await GetReasonForResignations(param.Factory, firstDateOfMonth, lastDateOfMonth, param.Language);

            // danh sách Báo cáo Excel
            var exports = new List<Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Excel>();

            #region Lấy thông tin Exports
            if (isExport && totalRecords.Any())
            {
                foreach (var reason in totalRecords)
                {
                    var excel = new Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Excel()
                    {
                        ReasonForResignation = $"{reason.Key} - {reason.Value}",
                        TotalNumberOfResignations = await GetExportTotalNumberOfResignations(param.Factory, firstDateOfMonth, lastDateOfMonth, param.PermisionGroups, reason.Key),
                        NumberOfResignationsWithoutSignedLaborContracts = await Query_Export_NumberOfResignation(param.Factory, firstDateOfMonth, lastDateOfMonth, param.PermisionGroups, reason.Key),
                        NumberOfResignationsWithSignedLaborContracts = await Query_Export_NumberOfResignation(param.Factory, firstDateOfMonth, lastDateOfMonth, param.PermisionGroups, reason.Key, false),
                    };
                    exports.Add(excel);
                }
            }
            #endregion

            var result = new Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Result()
            {
                TotalRecords = totalRecords.Count,
                Data = data,
                Exports = exports
            };

            return result;
        }

        private async Task<List<KeyValuePair<string, string>>> GetReasonForResignations(string factory, DateTime firstDateOfMonth, DateTime lastDateOfMonth, string language)
        {
            var reasons = await GetDataBasicCode(BasicCodeTypeConstant.ReasonResignation, language, true);

            var query = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(x => x.Factory == factory
                    && x.Resign_Date.HasValue
                    && x.Resign_Date.Value >= firstDateOfMonth.Date
                    && x.Resign_Date.Value <= lastDateOfMonth.Date
                    , true).ToListAsync();


            return query.Join(reasons, x => new { x.Resign_Reason }, y => new { Resign_Reason = y.Key },
                                    (x, y) => new KeyValuePair<string, string>(x.Resign_Reason, y.Value))
                                .Distinct()
                                .ToList();
        }

        /// <summary>
        /// Lấy danh sách nhân viên hiện tại (cũ)
        /// </summary>
        /// <param name="personals">Danh sách nhân viên </param>
        /// <param name="firstDateOfMonth"> Ngày đầu tiên của tháng được chọn (YearMonth) </param>
        /// <param name="isTaiwan">Default: A01 , CN: A02 </param>
        /// <returns></returns>
        private async Task<int> GetNumberOfEmployeesAt(string factory,
                        DateTime firstDateOfMonth,
                        bool isTaiwan = true)
        {
            var personals = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(x => x.Permission_Group.Contains(isTaiwan ? "A01" : "A02"))
                                                        .ToListAsync();

            var beginCnt1 = personals.Where(x => x.Assigned_Factory == factory
                                                && x.Onboard_Date.Date < firstDateOfMonth.Date
                                                && (x.Resign_Date.HasValue && x.Resign_Date.Value >= firstDateOfMonth.Date
                                                || !x.Resign_Date.HasValue))
                                    .Select(x => x.USER_GUID).ToList();

            var empIdCardHistories = await _repositoryAccessor.HRMS_Emp_IDcard_EmpID_History.FindAll(a => a.Assigned_Factory == factory && a.Onboard_Date.Date < firstDateOfMonth.Date
                                        && (a.Resign_Date.HasValue && a.Resign_Date.Value >= firstDateOfMonth.Date
                                        || !a.Resign_Date.HasValue))
                                        .Select(x => x.USER_GUID)
                                        .ToListAsync();

            var beginCnt2 = empIdCardHistories.Join(personals, USER_GUID => USER_GUID, y => y.USER_GUID, (USER_GUID, y) => new { USER_GUID, y }).Select(x => x.USER_GUID).ToList();

            return beginCnt1.Union(beginCnt2).Count();
        }

        /// <summary>
        /// Lấy danh sách thuê mới trong tháng này
        /// </summary>
        /// <param name="personals">Danh sách nhân viên theo nhà máy </param>
        /// <param name="empIdHistory"> Danh sách lịch sử thay đổi thẻ nhân viên </param>
        /// <param name="firstDateOfMonth"> Ngày đầu tiên của tháng hiện tại </param>
        /// <param name="isTaiwan">Default: A01 , CN: A02 </param>
        /// <returns> Số lượng thuê mới trong tháng này </returns>
        private async Task<int> GetNewHiresThisMonth(string factory, DateTime firstDateOfMonth, DateTime lastDateOfMonth, bool isTaiwan = true)
        {

            var personals = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(x => x.Permission_Group.Contains(isTaiwan ? "A01" : "A02")).ToListAsync();

            var new_Cnt1 = personals.Where(a => a.Assigned_Factory == factory &&
                                                a.Onboard_Date.Date >= firstDateOfMonth.Date &&
                                                a.Onboard_Date.Date <= lastDateOfMonth.Date
                                            ).Select(x => x.USER_GUID)
                                            .ToList();

            var empIdCardHistories = await _repositoryAccessor.HRMS_Emp_IDcard_EmpID_History.FindAll(a =>
                                                a.Assigned_Factory == factory &&
                                                a.Onboard_Date.Date >= firstDateOfMonth.Date &&
                                                a.Onboard_Date.Date <= lastDateOfMonth.Date)
                                            .Select(x => x.USER_GUID)
                                            .ToListAsync();

            var new_Cnt2 = empIdCardHistories.Join(personals,
                            USER_GUID => USER_GUID,
                            y => y.USER_GUID,
                            (USER_GUID, y) => new { USER_GUID, y })
                            .Select(x => x.USER_GUID)
                            .ToList();

            return new_Cnt1.Union(new_Cnt2).Count();
        }

        /// <summary>
        /// Lấy danh sách đăng ký mới trong tháng này
        /// </summary>
        /// <param name="param"></param>
        /// <param name="firstDateOfMonth"></param>
        /// <param name="lastDateOfMonth"></param>
        /// <param name="permissionGroups"></param>
        /// <returns> New_Cnt2 </returns>
        private async Task<int> GetResignationsThisMonth(string factory, DateTime firstDateOfMonth, DateTime lastDateOfMonth, bool isTaiwan = true)
        {
            var personals = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(x => x.Permission_Group.Contains(isTaiwan ? "A01" : "A02")).ToListAsync();

            var resign_Cnt1 = personals.Where(a => a.Assigned_Factory == factory &&
                                                a.Resign_Date.HasValue &&
                                                a.Resign_Date.Value.Date >= firstDateOfMonth.Date &&
                                                a.Resign_Date.Value.Date <= lastDateOfMonth.Date
                                            ).Select(x => x.USER_GUID)
                                            .ToList();

            var empIdCardHistories = await _repositoryAccessor.HRMS_Emp_IDcard_EmpID_History.FindAll(a =>
                                                a.Assigned_Factory == factory &&
                                                a.Resign_Date.HasValue &&
                                                a.Resign_Date.Value.Date >= firstDateOfMonth.Date &&
                                                a.Resign_Date.Value.Date <= lastDateOfMonth.Date)
                                            .Select(x => x.USER_GUID)
                                            .ToListAsync();

            var resign_Cnt2 = empIdCardHistories.Join(personals,
                            USER_GUID => USER_GUID,
                            y => y.USER_GUID,
                            (USER_GUID, y) => new { USER_GUID, y })
                            .Select(x => x.USER_GUID)
                            .ToList();

            return resign_Cnt1.Union(resign_Cnt2).Count();
        }

        private async Task<List<Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Dept>> GetEmployeeDept(string factory, List<string> local_Permission_list)
        {
            var predicate = PredicateBuilder.New<HRMS_Emp_Personal>(true);
            var predicateDepartment = PredicateBuilder.New<HRMS_Org_Department>(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(factory))
                predicate.And(x => x.Factory == factory);

            predicate.And(x => local_Permission_list.Any(z => z == x.Permission_Group));

            var query = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(predicate, true)
                        .Join(_repositoryAccessor.HRMS_Org_Department.FindAll(predicateDepartment, true),
                            x => new { x.Division, x.Factory, Department_Code = x.Department },
                            y => new { y.Division, y.Factory, y.Department_Code },
                            (personal, department) => new Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Dept()
                            {
                                Department = personal.Department,
                                Department_Name = department.Department_Name,
                                Org_Level = department.Org_Level,
                            })
                        .Distinct()
                        .ToListAsync();
            return query;
        }

        /// <summary>
        /// Lấy ngày bắt đầu hoặc ngày kết thúc của tháng trong năm
        /// </summary>
        /// <param name="yearMonth">Thời gian năm và tháng</param>
        /// <param name="isEndDateOfMonth"> Mặc định lấy ngày đầu tháng, True: Ngày cuối tháng </param>
        /// <returns>DateTime ngày đầu tháng hoặc cuối tháng </returns>
        private static DateTime GetDateTimeOfMonth(string yearMonth, bool isEndDateOfMonth = false)
        {
            var time = yearMonth.Split("/");
            int year = int.Parse(time[0]);
            int month = int.Parse(time[1]);
            return new DateTime(year, month, !isEndDateOfMonth ? 1 : DateTime.DaysInMonth(year, month));
        }

        /// <summary>
        /// Tổng số nhân viên từ chức
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetExportTotalNumberOfResignations(string factory, DateTime firstDateOfMonth, DateTime lastDateOfMonth, List<string> local_Permission_list, string resign_Reason)
        {
            var foreign_Permission_list = await Query_Permission_Group_List(factory, "N");

            var count = await _repositoryAccessor.HRMS_Emp_Personal
                                .FindAll(x => x.Factory == factory
                                            && x.Resign_Reason == resign_Reason
                                            && x.Resign_Date.HasValue
                                            && x.Resign_Date.Value.Date >= firstDateOfMonth.Date
                                            && x.Resign_Date.Value.Date <= lastDateOfMonth.Date
                                            && local_Permission_list.Any(p => p == x.Permission_Group)
                                            && foreign_Permission_list.Any(p => p == x.Permission_Group)
                                            , true).CountAsync();
            return count;
        }
        
        /// <summary>
        /// True: ExportNumberOfResignationsWithoutSignedLaborContracts
        /// False: GetExportNumberOfResignationsWithSignedLaborContracts
        /// </summary>
        /// <param name="queryParam"></param>
        /// <param name="resign_Reason"></param>
        /// <param name="isSignedLaborContracts">Default: ExportNumberOfResignationsWithoutSignedLaborContracts</param>
        /// <returns></returns>
        private async Task<int> Query_Export_NumberOfResignation(string factory, DateTime firstDateOfMonth, DateTime lastDateOfMonth, List<string> local_Permission_list, string resign_Reason, bool isSignedLaborContracts = true)
        {
            var foreign_Permission_list = await Query_Permission_Group_List(factory, "N");

           // Quản lý danh sách hợp đồng thử việc của nhân viên nghỉ việc trong tháng này
           var results = await _repositoryAccessor.HRMS_Emp_Contract_Type.FindAll(true)
                               .Join(_repositoryAccessor.HRMS_Emp_Contract_Management.FindAll(true),
                                   a => new { a.Division, a.Factory, a.Contract_Type },
                                   b => new { b.Division, b.Factory, b.Contract_Type },
                                   (a, b) => new { a, b })
                               // Số lượng nhân viên nghỉ việc [Từ đầu tháng -> cuối tháng được chọn ]
                               .Join(_repositoryAccessor.HRMS_Emp_Personal.FindAll(f => f.Factory == factory
                                   && f.Resign_Reason == resign_Reason
                                   && f.Resign_Date >= firstDateOfMonth && f.Resign_Date <= lastDateOfMonth
                                   && local_Permission_list.Any(x => x == f.Permission_Group)
                                   && foreign_Permission_list.Contains(f.Permission_Group), true),
                                   x => new { x.b.Division, x.b.Factory, x.b.Employee_ID },
                                   c => new { c.Division, c.Factory, c.Employee_ID },
                                   (x, c) => new { x.a, x.b, c })
                               .GroupBy(g => new { g.c.Resign_Reason, g.c.Employee_ID, g.a.Probationary_Period })
                               .Select(g => new { Contract_Start = g.Max(x => x.b.Contract_Start), g.Key.Employee_ID, g.Key.Probationary_Period })
                               .Where(y => y.Probationary_Period == isSignedLaborContracts)
                               .CountAsync();

            return results;
        }

        private async Task<List<string>> GetListCodeName(List<string> listCode, string language, string basicCodeType)
        {
            if (!listCode.Any()) return listCode;

            var result = new List<string>();
            var data = await GetDataBasicCode(basicCodeType, language);
            foreach (var code in listCode)
            {
                var item = data.FirstOrDefault(x => x.Key == code);
                result.Add(!string.IsNullOrWhiteSpace(item.Key) ? item.Value : code);
            }
            return result;
        }

        #endregion

        #region Convert To Table Excel

        private static List<Cell> ConvertToExcelTable(Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Excel_Header header, Monthly_Employee_Status_Changes_Sheet_By_Reason_for_Resignation_Result data)
        {
            // Header
            var result = new List<Cell>()
            {
                new("B2", header.Factory),
                new("B3", header.PrintBy),
                new("D2", header.YearMonth),
                new("D3", header.PrintDate),
                new("F2", string.Join(" / ", header.PermisionGroups)),

                new("B7", data.Data[0].NumberOfEmployeesAt),
                new("B8", data.Data[1].NumberOfEmployeesAt),
                new("B9", data.Data[2].NumberOfEmployeesAt),

                new("C7", data.Data[0].NewHiresThisMonth),
                new("C8", data.Data[1].NewHiresThisMonth),
                new("C9", data.Data[2].NewHiresThisMonth),

                new("D7", data.Data[0].ResignationsThisMonth),
                new("D8", data.Data[1].ResignationsThisMonth),
                new("D9", data.Data[2].ResignationsThisMonth),

                new("E7", data.Data[0].TotalNumberOfEmployeesAt),
                new("E8", data.Data[1].TotalNumberOfEmployeesAt),
                new("E9", data.Data[2].TotalNumberOfEmployeesAt),
            };

            // Bảng dữ liệu

            return result;
        }
        #endregion
    }
}