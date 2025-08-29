using System.Collections;
using System.Drawing;
using AgileObjects.AgileMapper;
using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Models;
using Aspose.Cells;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance
{
    public class S_5_1_24_MonthlyAttendanceDataMaintenanceResignedEmployees : BaseServices, I_5_1_24_MonthlyAttendanceDataMaintenanceResignedEmployees
    {

        public S_5_1_24_MonthlyAttendanceDataMaintenanceResignedEmployees(DBContext dbContext) : base(dbContext)
        {

        }

        #region Add
        public async Task<OperationResult> Add(ResignedEmployeeDetail data, string userName)
        {
            if (await _repositoryAccessor.HRMS_Att_Resign_Monthly.AnyAsync(
                    x => x.Att_Month.Date == Convert.ToDateTime(data.Att_Month).Date &&
                    x.Factory == data.Factory &&
                    x.Employee_ID == data.Employee_ID))
            {
                string message = $"Year-Month of Attendance: {data.Att_Month}, Employee ID: {data.Employee_ID} exsited!";
                return new OperationResult { IsSuccess = false, Error = message };
            }
            var att_Resign_Monthly_Detail = await _repositoryAccessor.HRMS_Att_Resign_Monthly_Detail
                            .FindAll().ToListAsync();
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                DateTime now = DateTime.Now;
                HRMS_Att_Resign_Monthly resign = new()
                {
                    Factory = data.Factory,
                    Employee_ID = data.Employee_ID,
                    USER_GUID = data.USER_GUID,
                    Department = data.Department,
                    Division = data.Division,
                    Att_Month = Convert.ToDateTime(data.Att_Month).Date,
                    Actual_Days = data.Actual_Days,
                    Delay_Early = data.Delay_Early,
                    Food_Expenses = data.Food_Expenses,
                    Night_Eat_Times = data.Night_Eat_Times,
                    No_Swip_Card = data.No_Swip_Card,
                    Pass = data.Pass == "Y",
                    Permission_Group = data.Permission_Group,
                    Resign_Status = data.Resign_Status,
                    Salary_Days = data.Salary_Days,
                    Salary_Type = data.Salary_Type,
                    Probation = data.Probation,
                    DayShift_Food = data.DayShift_Food,
                    NightShift_Food = data.NightShift_Food,
                    Update_By = userName,
                    Update_Time = now
                };
                _repositoryAccessor.HRMS_Att_Resign_Monthly.Add(resign);

                List<HRMS_Att_Resign_Monthly_Detail> details = new();
                List<HRMS_Att_Yearly> totals = new();
                if (data.Leaves is not null && data.Leaves.Any())
                {
                    data.Leaves.ForEach(item =>
                    {
                        details.Add(new()
                        {
                            Factory = resign.Factory,
                            Employee_ID = resign.Employee_ID,
                            USER_GUID = resign.USER_GUID,
                            Division = resign.Division,
                            Att_Month = resign.Att_Month,
                            Leave_Type = "1",
                            Leave_Code = item.Code,
                            Days = decimal.Parse(item.Days),
                            Update_By = userName,
                            Update_Time = now
                        });
                    });

                    totals.AddRange(
                        await Upd_HRMS_Att_Yearly(
                            new AttYearlyUpdate()
                            {
                                Factory = resign.Factory,
                                Employee_ID = resign.Employee_ID,
                                USER_GUID = resign.USER_GUID,
                                Att_Year = new DateTime(resign.Att_Month.Year, 1, 1),
                                Leave_Type = "1",
                                Account = userName,
                                Details = data.Leaves
                            }, att_Resign_Monthly_Detail
                        ));
                }

                if (data.Allowances is not null && data.Allowances.Any())
                {
                    data.Allowances.ForEach(item =>
                    {
                        details.Add(new()
                        {
                            Factory = resign.Factory,
                            Employee_ID = resign.Employee_ID,
                            USER_GUID = resign.USER_GUID,
                            Division = resign.Division,
                            Att_Month = resign.Att_Month,
                            Leave_Type = "2",
                            Leave_Code = item.Code,
                            Days = decimal.Parse(item.Days),
                            Update_By = userName,
                            Update_Time = now
                        });
                    });

                    totals.AddRange(
                        await Upd_HRMS_Att_Yearly(
                            new AttYearlyUpdate()
                            {
                                Factory = resign.Factory,
                                Employee_ID = resign.Employee_ID,
                                USER_GUID = resign.USER_GUID,
                                Att_Year = new DateTime(resign.Att_Month.Year, 1, 1),
                                Leave_Type = "2",
                                Account = userName,
                                Details = data.Allowances
                            }, att_Resign_Monthly_Detail
                        ));
                }

                if (details.Any())
                    _repositoryAccessor.HRMS_Att_Resign_Monthly_Detail.AddMultiple(details);

                if (totals.Any())
                    _repositoryAccessor.HRMS_Att_Yearly.UpdateMultiple(totals);

                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();

                return new OperationResult { IsSuccess = true };
            }
            catch (Exception ex)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult { IsSuccess = false, Error = ex.InnerException.Message };
            }
        }
        #endregion

        #region Edit
        public async Task<OperationResult> Edit(ResignedEmployeeDetail data, string userName)
        {
            var resign = await _repositoryAccessor.HRMS_Att_Resign_Monthly.FirstOrDefaultAsync(
                    x => x.Att_Month.Date == Convert.ToDateTime(data.Att_Month).Date &&
                    x.Factory == data.Factory &&
                    x.Employee_ID == data.Employee_ID);

            var att_Resign_Monthly_Detail = _repositoryAccessor.HRMS_Att_Resign_Monthly_Detail
                            .FindAll(true).ToList();
            if (resign is null)
                return new OperationResult { IsSuccess = false, Error = "Data Resigned Employee not existed!" };

            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                resign.Salary_Days = data.Salary_Days;
                resign.Actual_Days = data.Actual_Days;
                resign.Resign_Status = data.Resign_Status;
                resign.Delay_Early = data.Delay_Early;
                resign.No_Swip_Card = data.No_Swip_Card;
                resign.Food_Expenses = data.Food_Expenses;
                resign.Night_Eat_Times = data.Night_Eat_Times;
                resign.DayShift_Food = data.DayShift_Food;
                resign.NightShift_Food = data.NightShift_Food;
                resign.Department = data.Department;
                resign.Update_By = userName;
                resign.Update_Time = DateTime.Now;

                _repositoryAccessor.HRMS_Att_Resign_Monthly.Update(resign);

                List<HRMS_Att_Resign_Monthly_Detail> details = new();
                List<HRMS_Att_Yearly> totals = new();
                if (data.Leaves is not null && data.Leaves.Any())
                {
                    data.Leaves.ForEach(item =>
                    {
                        details.Add(new()
                        {
                            Factory = resign.Factory,
                            Employee_ID = resign.Employee_ID,
                            USER_GUID = resign.USER_GUID,
                            Division = resign.Division,
                            Att_Month = resign.Att_Month,
                            Leave_Type = "1",
                            Leave_Code = item.Code,
                            Days = decimal.Parse(item.Days),
                            Update_By = userName,
                            Update_Time = resign.Update_Time
                        });
                    });

                    totals.AddRange(await Upd_HRMS_Att_Yearly(new AttYearlyUpdate()
                    {
                        Factory = resign.Factory,
                        Employee_ID = resign.Employee_ID,
                        USER_GUID = resign.USER_GUID,
                        Att_Year = resign.Att_Month,
                        Leave_Type = "1",
                        Account = userName,
                        Details = data.Leaves
                    }, att_Resign_Monthly_Detail));
                }

                if (data.Allowances is not null && data.Allowances.Any())
                {
                    data.Allowances.ForEach(item =>
                    {
                        details.Add(new()
                        {
                            Factory = resign.Factory,
                            Employee_ID = resign.Employee_ID,
                            USER_GUID = resign.USER_GUID,
                            Division = resign.Division,
                            Att_Month = resign.Att_Month,
                            Leave_Type = "2",
                            Leave_Code = item.Code,
                            Days = decimal.Parse(item.Days),
                            Update_By = userName,
                            Update_Time = resign.Update_Time
                        });
                    });

                    totals.AddRange(await Upd_HRMS_Att_Yearly(new AttYearlyUpdate()
                    {
                        Factory = resign.Factory,
                        Employee_ID = resign.Employee_ID,
                        USER_GUID = resign.USER_GUID,
                        Att_Year = resign.Att_Month,
                        Leave_Type = "2",
                        Account = userName,
                        Details = data.Allowances
                    }, att_Resign_Monthly_Detail));
                }

                if (details.Any())
                    _repositoryAccessor.HRMS_Att_Resign_Monthly_Detail.UpdateMultiple(details);

                if (totals.Any())
                    _repositoryAccessor.HRMS_Att_Yearly.UpdateMultiple(totals);

                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();

                return new OperationResult { IsSuccess = true };
            }
            catch (Exception ex)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult { IsSuccess = false, Error = ex.InnerException.Message };
            }
        }
        #endregion

        #region GetListFactory
        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string language)
        {
            var predicate = PredicateBuilder.New<HRMS_Basic_Code>(x => x.Type_Seq == BasicCodeTypeConstant.Factory);
            var data = await GetBasicCodes(language, predicate);

            return data;
        }
        #endregion

        #region GetListFactoryAdd
        public async Task<List<KeyValuePair<string, string>>> GetListFactoryAdd(string userName, string language)
        {
            var factories = await _repositoryAccessor.HRMS_Basic_Account_Role.FindAll(x => x.Account.ToLower() == userName.ToLower(), true)
                .Join(_repositoryAccessor.HRMS_Basic_Role.FindAll(true),
                    x => x.Role,
                    y => y.Role,
                    (x, y) => new { accRole = x, role = y })
                .Select(x => x.role.Factory)
                .Distinct().ToListAsync();

            if (!factories.Any())
                return new List<KeyValuePair<string, string>>();

            var predicate = PredicateBuilder.New<HRMS_Basic_Code>(x => x.Type_Seq == BasicCodeTypeConstant.Factory && factories.Contains(x.Code));
            var data = await GetBasicCodes(language, predicate);

            return data;
        }
        #endregion

        #region GetListDepartment
        public async Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language)
        {
            ExpressionStarter<HRMS_Org_Department> predDept = PredicateBuilder.New<HRMS_Org_Department>(x => x.Factory == factory);
            ExpressionStarter<HRMS_Basic_Factory_Comparison> predCom = PredicateBuilder.New<HRMS_Basic_Factory_Comparison>(x => x.Factory == factory && x.Kind == "1");
            var data = await QueryDepartment(predDept, predCom, language)
                .Select(
                    x => new KeyValuePair<string, string>(
                        x.Department.Department_Code,
                        $"{x.Department.Department_Code} - {(x.Language != null ? x.Language.Name : x.Department.Department_Name)}"
                    )
                ).ToListAsync();

            return data;
        }
        #endregion

        #region GetListPermissionGroup
        public async Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string language)
        {
            var predicate = PredicateBuilder.New<HRMS_Basic_Code>(x => x.Type_Seq == BasicCodeTypeConstant.PermissionGroup);
            var data = await GetBasicCodes(language, predicate);

            return data;
        }
        #endregion

        #region GetListSalaryType
        public async Task<List<KeyValuePair<string, string>>> GetListSalaryType(string language)
        {
            var predicate = PredicateBuilder.New<HRMS_Basic_Code>(x => x.Type_Seq == BasicCodeTypeConstant.SalaryType);
            var data = await GetBasicCodes(language, predicate);

            return data;
        }
        #endregion

        #region GetResignedDetail
        public async Task<(List<LeaveDetailDisplay> Leaves, List<LeaveDetailDisplay> Allowances)> GetResignedDetail(ResignedEmployeeDetailParam param)
        {
            var leaveCodes = await QueryCodeDetail(param, "1", BasicCodeTypeConstant.Leave);

            var predicateMonthly = PredicateBuilder
                .New<HRMS_Att_Resign_Monthly_Detail>(
                    x => x.Factory == param.Factory &&
                    x.Employee_ID == param.Employee_ID &&
                    x.Att_Month == Convert.ToDateTime(param.Att_Month).Date);

            var predicateYearly = PredicateBuilder
                .New<HRMS_Att_Yearly>(
                    x => x.Factory == param.Factory &&
                    x.Employee_ID == param.Employee_ID &&
                    x.Att_Year == new DateTime(Convert.ToDateTime(param.Att_Month).Year, 1, 1));

            List<LeaveDetailDisplay> leaves = new();
            if (leaveCodes.Any())
            {
                var predicateMTemp = PredicateBuilder.New<HRMS_Att_Resign_Monthly_Detail>(predicateMonthly);
                predicateMTemp.And(x => x.Leave_Type == "1");

                var predicateYTemp = PredicateBuilder.New<HRMS_Att_Yearly>(predicateYearly);
                predicateYTemp.And(x => x.Leave_Type == "1");

                leaves = leaveCodes
                    .GroupJoin(_repositoryAccessor.HRMS_Att_Resign_Monthly_Detail.FindAll(predicateMTemp, true),
                        x => x.Key,
                        y => y.Leave_Code,
                        (x, y) => new { code = x, leave = y })
                    .SelectMany(x => x.leave.DefaultIfEmpty(),
                        (x, y) => new { x.code, leave = y })
                    .GroupJoin(_repositoryAccessor.HRMS_Att_Yearly.FindAll(predicateYTemp, true),
                        x => x.code.Key,
                        y => y.Leave_Code,
                        (x, y) => new { x.code, x.leave, year = y })
                    .SelectMany(x => x.year.DefaultIfEmpty(),
                        (x, y) => new { x.code, x.leave, year = y })
                    .Select(x => new LeaveDetailDisplay()
                    {
                        Code = x.code.Key,
                        CodeName = x.code.Value,
                        Days = x.leave is null ? "0" : x.leave.Days.ToString(),
                        Total = x.year is null ? "0" : x.year.Days.ToString()
                    }).ToList();
            }

            var allowanceCodes = await QueryCodeDetail(param, "2", BasicCodeTypeConstant.Allowance);
            List<LeaveDetailDisplay> allowances = new();
            if (allowanceCodes.Any())
            {
                var predicateMTemp = PredicateBuilder.New<HRMS_Att_Resign_Monthly_Detail>(predicateMonthly);
                predicateMTemp.And(x => x.Leave_Type == "2");

                var predicateYTemp = PredicateBuilder.New<HRMS_Att_Yearly>(predicateYearly);
                predicateYTemp.And(x => x.Leave_Type == "2");

                allowances = allowanceCodes
                    .GroupJoin(_repositoryAccessor.HRMS_Att_Resign_Monthly_Detail.FindAll(predicateMTemp, true),
                        x => x.Key,
                        y => y.Leave_Code,
                        (x, y) => new { code = x, leave = y })
                    .SelectMany(x => x.leave.DefaultIfEmpty(),
                        (x, y) => new { x.code, leave = y })
                    .GroupJoin(_repositoryAccessor.HRMS_Att_Yearly.FindAll(predicateYTemp, true),
                        x => x.code.Key,
                        y => y.Leave_Code,
                        (x, y) => new { x.code, x.leave, year = y })
                    .SelectMany(x => x.year.DefaultIfEmpty(),
                        (x, y) => new { x.code, x.leave, year = y })
                    .Select(x => new LeaveDetailDisplay()
                    {
                        Code = x.code.Key,
                        CodeName = x.code.Value,
                        Days = x.leave is null ? "0" : x.leave.Days.ToString(),
                        Total = x.year is null ? "0" : x.year.Days.ToString()
                    }).ToList();
            }

            return (leaves, allowances);
        }

        private async Task<List<KeyValuePair<string, string>>> QueryCodeDetail(ResignedEmployeeDetailParam param, string leaveType, string typeSeq)
        {
            var monthlyLeaves = _repositoryAccessor.HRMS_Att_Use_Monthly_Leave
                .FindAll(x => x.Factory == param.Factory && x.Leave_Type == leaveType && x.Effective_Month.Date <= Convert.ToDateTime(param.Att_Month).Date);

            if (!await monthlyLeaves.AnyAsync())
                return new List<KeyValuePair<string, string>>();

            var maxLeaveEffectiveMonth = await monthlyLeaves.MaxAsync(x => x.Effective_Month);

            var codes = await _repositoryAccessor.HRMS_Att_Use_Monthly_Leave
                .FindAll(x => x.Factory == param.Factory && x.Leave_Type == leaveType && x.Effective_Month == maxLeaveEffectiveMonth)
                .OrderBy(x => x.Seq)
                .Select(x => x.Code).ToListAsync();

            var predicate = PredicateBuilder.New<HRMS_Basic_Code>(x => x.Type_Seq == typeSeq && codes.Contains(x.Code));
            return await GetBasicCodes(param.Language, predicate);
        }
        #endregion

        #region GetEmpInfo
        public async Task<OperationResult> GetEmpInfo(ResignedEmployeeParam param)
        {
            var comparison = await _repositoryAccessor.HRMS_Basic_Factory_Comparison
                .FirstOrDefaultAsync(x => x.Factory == param.Factory && x.Kind == "1");

            if (comparison is null)
                return null;

            var departments = await GetDepartmentMain(param.Language);

            var emp = await _repositoryAccessor.HRMS_Emp_Personal
                .FirstOrDefaultAsync(
                    x => (x.Factory == param.Factory || x.Assigned_Factory == param.Factory) &&
                    (x.Employee_ID == param.Employee_ID || x.Assigned_Employee_ID == param.Employee_ID) &&
                    x.Division == comparison.Division, true);

            if (emp is null)
                return new OperationResult(false, "Employee not existed.");

            var checkResignDate = await _repositoryAccessor.HRMS_Emp_Personal
                .FirstOrDefaultAsync(x => x.Factory == param.Factory
                    && x.Employee_ID == param.Employee_ID && x.Resign_Date == null);

            if (checkResignDate != null)
                return new OperationResult(false, $"{param.Employee_ID} have On job and cannot be entered.");

            var department = string.IsNullOrWhiteSpace(emp.Employment_Status)
                    ? departments.FirstOrDefault(
                        d => d.Division == emp.Division &&
                        d.Factory == emp.Factory &&
                        d.Department_Code == emp.Department)
                    : emp.Employment_Status == "A" || emp.Employment_Status == "S"
                    ? departments.FirstOrDefault(
                        d => d.Division == emp.Assigned_Division &&
                        d.Factory == emp.Assigned_Factory &&
                        d.Department_Code == emp.Assigned_Department)
                    : null;

            var result = new EmpResignedInfo
            {
                USER_GUID = emp.USER_GUID,
                Department_Name = department?.Department_Name,
                Department_Code = department?.Department_Code,
                Division = string.IsNullOrWhiteSpace(emp.Employment_Status)
                    ? emp.Division
                    : emp.Employment_Status == "A" || emp.Employment_Status == "S"
                    ? emp.Assigned_Division
                    : "",
                Local_Full_Name = emp.Local_Full_Name,
                Permission_Group = emp.Permission_Group
            };
            return new OperationResult(true, result);
        }
        #endregion

        #region GetDataPagination
        public async Task<PaginationUtility<ResignedEmployeeMain>> GetDataPagination(PaginationParam pagination, ResignedEmployeeParam param, bool isPaging = true)
        {
            var departments = await GetDepartmentMain(param.Language);
            var groups = await GetListPermissionGroup(param.Language);
            var types = await GetListSalaryType(param.Language);

            var (predResignMonthly, predProbation, predEmp) = SetPredicate(param);
            var HARM = _repositoryAccessor.HRMS_Att_Resign_Monthly
                .FindAll(predResignMonthly, true)
                .Project().To<ResignedEmployeeDto>(cfg => cfg.Map(false).To(dto => dto.isProbation));
            var HAPM = _repositoryAccessor.HRMS_Att_Probation_Monthly
                .FindAll(predProbation, true)
                .Project().To<ResignedEmployeeDto>(cfg => cfg.Map(true).To(dto => dto.isProbation));

            var HAPM_HARM = HAPM
                        .Join(HARM,
                        hapm => new { hapm.Factory, hapm.Att_Month, hapm.Employee_ID },
                        harm => new { harm.Factory, harm.Att_Month, harm.Employee_ID },
                        (hapm, harm) => hapm);
            var data = await HARM.Union(HAPM_HARM).Join(_repositoryAccessor.HRMS_Emp_Personal.FindAll(predEmp, true),
                x => new { x.USER_GUID },
                y => new { y.USER_GUID },
                (x, y) => new { HARM = x, HEP = y }).ToListAsync();
            List<ResignedEmployeeMain> result = data
                .Select(x =>
                {
                    var deptLang = departments.FirstOrDefault(d => d.Division == x.HARM.Division && d.Factory == x.HARM.Factory && d.Department_Code == x.HARM.Department);
                    return new ResignedEmployeeMain
                    {
                        USER_GUID = x.HEP.USER_GUID,
                        Factory = x.HARM.Factory,
                        Employee_ID = x.HARM.Employee_ID,
                        Att_Month = x.HARM.Att_Month.ToString("yyyy/MM"),
                        Department = x.HARM.Department,
                        Department_Name = deptLang != null ? deptLang.Department_Name : x.HARM.Department,
                        Permission_Group = x.HARM.Permission_Group,
                        Permission_Group_Name = groups.Any(g => g.Key == x.HARM.Permission_Group)
                            ? groups.FirstOrDefault(g => g.Key == x.HARM.Permission_Group).Value
                            : x.HARM.Permission_Group,
                        Salary_Type = x.HARM.Salary_Type,
                        Salary_Type_Name = types.Any(g => g.Key == x.HARM.Salary_Type)
                            ? types.FirstOrDefault(g => g.Key == x.HARM.Salary_Type).Value
                            : x.HARM.Salary_Type,
                        Local_Full_Name = x.HEP.Local_Full_Name,
                        Actual_Days = x.HARM.Actual_Days,
                        Pass = x.HARM.Pass ? "Y" : "N",
                        Resign_Status = x.HARM.Resign_Status,
                        Salary_Days = x.HARM.Salary_Days,
                        Probation = x.HARM.Probation,
                        isProbation = x.HARM.isProbation,
                        Update_By = x.HARM.Update_By,
                        Update_Time = x.HARM.Update_Time.ToString("yyyy/MM/dd HH:mm:ss")
                    };
                }).ToList();
            return PaginationUtility<ResignedEmployeeMain>.Create(result, pagination.PageNumber, pagination.PageSize, isPaging);
        }
        #endregion

        #region Query
        public async Task<ResignedEmployeeDetail> Query(ResignedEmployeeParam param)
        {
            var groups = await GetListPermissionGroup(param.Language);
            var types = await GetListSalaryType(param.Language);
            var data = !param.isProbation
                ? _repositoryAccessor.HRMS_Att_Resign_Monthly.FindAll(x => x.Factory == param.Factory
                    && x.Employee_ID == param.Employee_ID
                    && x.Att_Month == DateTime.Parse(param.Att_Month_Start), true)
                    .Project().To<ResignedEmployeeDto>(cfg => cfg.Map(false).To(dto => dto.isProbation))
                : _repositoryAccessor.HRMS_Att_Probation_Monthly.FindAll(x => x.Factory == param.Factory
                    && x.Employee_ID == param.Employee_ID
                    && x.Att_Month == DateTime.Parse(param.Att_Month_Start), true)
                    .Project().To<ResignedEmployeeDto>(cfg => cfg.Map(true).To(dto => dto.isProbation));
            var result = data.Select(x => new ResignedEmployeeDetail
            {
                USER_GUID = x.USER_GUID,
                Factory = x.Factory,
                Employee_ID = x.Employee_ID,
                Att_Month = x.Att_Month.ToString("yyyy/MM"),
                Pass = x.Pass ? "Y" : "N",
                Permission_Group = x.Permission_Group,
                Salary_Type = x.Salary_Type,
                Actual_Days = x.Actual_Days,
                Resign_Status = x.Resign_Status,
                Salary_Days = x.Salary_Days,
                Delay_Early = x.Delay_Early,
                Food_Expenses = x.Food_Expenses,
                Night_Eat_Times = x.Night_Eat_Times,
                No_Swip_Card = x.No_Swip_Card,
                DayShift_Food = x.DayShift_Food, // số ngày ăn trưa
                NightShift_Food = x.NightShift_Food, // Số ngày ăn khuya
                Probation = x.Probation,
                isProbation = x.isProbation
            }).FirstOrDefault();

            var emp = await _repositoryAccessor.HRMS_Emp_Personal.FirstOrDefaultAsync(x => x.USER_GUID == result.USER_GUID);
            if (emp is not null)
            {
                var departments = await GetDepartmentMain(param.Language);

                result.Local_Full_Name = emp.Local_Full_Name;

                if (param.isMonthly)
                {
                    var HARM = _repositoryAccessor.HRMS_Att_Resign_Monthly.FirstOrDefault(x => x.Factory == param.Factory && x.Employee_ID == param.Employee_ID && x.Att_Month == DateTime.Parse(param.Att_Month_Start));
                    result.Department = HARM is not null ? departments.FirstOrDefault(x => x.Division == HARM.Division &&
                           x.Factory == HARM.Factory &&
                           x.Department_Code == HARM.Department)?.Department_Code ?? HARM.Department
                       : null;
                    result.Department_Name = HARM is not null ? departments.FirstOrDefault(x => x.Division == HARM.Division &&
                           x.Factory == HARM.Factory &&
                           x.Department_Code == HARM.Department)?.Department_Name ?? HARM.Department
                       : null;
                }
                else
                {
                    result.Department = string.IsNullOrWhiteSpace(emp.Employment_Status)
                    ? emp.Department
                    : emp.Employment_Status == "A" || emp.Employment_Status == "S"
                    ? emp.Assigned_Department
                    : "";
                    result.Department_Name = string.IsNullOrWhiteSpace(emp.Employment_Status)
                        ? departments.FirstOrDefault(
                            d => d.Division == emp.Division &&
                            d.Factory == emp.Factory &&
                            d.Department_Code == emp.Department)?.Department_Name
                        : emp.Employment_Status == "A" || emp.Employment_Status == "S"
                        ? departments.FirstOrDefault(
                            d => d.Division == emp.Assigned_Division &&
                            d.Factory == emp.Assigned_Factory &&
                            d.Department_Code == emp.Assigned_Department)?.Department_Name
                        : result.Department;
                }


                result.Division = string.IsNullOrWhiteSpace(emp.Employment_Status)
                    ? emp.Division
                    : emp.Employment_Status == "A" || emp.Employment_Status == "S"
                    ? emp.Assigned_Division
                    : "";
            }
            var details = !param.isProbation
                ? await _repositoryAccessor.HRMS_Att_Resign_Monthly_Detail.FindAll(x =>
                    x.Factory == param.Factory &&
                    x.Employee_ID == param.Employee_ID &&
                    x.Att_Month == Convert.ToDateTime(param.Att_Month_Start).Date, true)
                    .Project().To<ResignedEmployeeDto_Detail>(cfg => cfg.Map(false).To(dto => dto.isProbation)).Distinct().ToListAsync()
                : await _repositoryAccessor.HRMS_Att_Probation_Monthly_Detail.FindAll(x =>
                    x.Factory == param.Factory &&
                    x.Employee_ID == param.Employee_ID &&
                    x.Att_Month == Convert.ToDateTime(param.Att_Month_Start).Date, true)
                    .Project().To<ResignedEmployeeDto_Detail>(cfg => cfg.Map(true).To(dto => dto.isProbation)).Distinct().ToListAsync();
            if (details.Any())
            {
                var totals = await _repositoryAccessor.HRMS_Att_Yearly
                    .FindAll(
                        x => x.Factory == param.Factory &&
                        x.Att_Year.Year == Convert.ToDateTime(param.Att_Month_Start).Year &&
                        x.Employee_ID == param.Employee_ID)
                    .Distinct().ToListAsync();

                var predicateLeave = PredicateBuilder.New<HRMS_Basic_Code>(x => x.Type_Seq == BasicCodeTypeConstant.Leave);
                var leaves = await GetBasicCodes(param.Language, predicateLeave);

                result.Leaves = details.Where(x => x.Leave_Type == "1")
                    .Select(x =>
                    {
                        var leave = leaves.FirstOrDefault(c => c.Key == x.Leave_Code).Value ?? "";
                        var total = totals.FirstOrDefault(t => t.Leave_Code == x.Leave_Code && t.Leave_Type == "1");
                        return new LeaveDetailDisplay()
                        {
                            Code = x.Leave_Code,
                            CodeName = leave,
                            Days = x.Days.ToString(),
                            Total = total != null ? total.Days.ToString() : "0"
                        };
                    }).ToList();

                var predicateAllowance = PredicateBuilder.New<HRMS_Basic_Code>(x => x.Type_Seq == BasicCodeTypeConstant.Allowance);
                var allowances = await GetBasicCodes(param.Language, predicateAllowance);

                result.Allowances = details.Where(x => x.Leave_Type == "2")
                    .Select(x =>
                    {
                        var allowance = allowances.FirstOrDefault(c => c.Key == x.Leave_Code).Value ?? "";
                        var total = totals.FirstOrDefault(t => t.Leave_Code == x.Leave_Code && t.Leave_Type == "2");
                        return new LeaveDetailDisplay()
                        {
                            Code = x.Leave_Code,
                            CodeName = allowance,
                            Days = x.Days.ToString(),
                            Total = total != null ? total.Days.ToString() : "0"
                        };
                    }).ToList();
            }

            return result;
        }
        #endregion

        #region ExportExcel
        public async Task<OperationResult> ExportExcel(PaginationParam pagination, ResignedEmployeeParam param)
        {
            ResignedEmployeeDetailParam queryCodeParam = new()
            {
                Factory = param.Factory,
                Att_Month = param.Att_Month_End,
                Language = param.Language
            };
            var leaves = await QueryCodeDetail(queryCodeParam, "1", BasicCodeTypeConstant.Leave);
            var allowances = await QueryCodeDetail(queryCodeParam, "2", BasicCodeTypeConstant.Allowance);

            var data = await GetDataExport(param, leaves, allowances);

            if (!data.Any())
                return new OperationResult(false, "No data for excel download");

            try
            {
                MemoryStream stream = new();
                var path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Resources\\Template\\AttendanceMaintenance\\5_1_24_MonthlyAttendanceDataMaintenanceResignedEmployees\\Download.xlsx"
                );
                WorkbookDesigner designer = new() { Workbook = new Workbook(path) };
                Worksheet ws = designer.Workbook.Worksheets[0];

                ws.Cells["A1"].PutValue(param.Language == "tw" ? "5.24 月份出勤資料維護-離職" : "5.24 Monthly Attendance Data Maintenance-Resigned Employees");
                ws.Cells["A2"].PutValue(param.Language == "tw" ? "廠別" : "Factory");
                ws.Cells["B2"].PutValue(param.Factory);
                ws.Cells["C2"].PutValue(param.Language == "tw" ? "出勤年月" : "Year-Month of Attendance");
                ws.Cells["E2"].PutValue(param.Att_Month_Start);
                ws.Cells["F2"].PutValue(param.Language == "tw" ? "列印人員" : "Print By");
                ws.Cells["G2"].PutValue(param.PrintBy);
                ws.Cells["H2"].PutValue(param.Language == "tw" ? "列印日期" : "Print Date");
                ws.Cells["I2"].PutValue(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                ws.Cells["A4"].PutValue(param.Language == "tw" ? "部門" : "Department");
                ws.Cells["B4"].PutValue(param.Language == "tw" ? "部門名稱" : "Department Name");
                ws.Cells["C4"].PutValue(param.Language == "tw" ? "工號" : "Employee ID");
                ws.Cells["D4"].PutValue(param.Language == "tw" ? "本地姓名" : "Local Full Name");
                ws.Cells["E4"].PutValue(param.Language == "tw" ? "新進/離職" : "New-Hired / Resigned");
                ws.Cells["F4"].PutValue(param.Language == "tw" ? "試用期" : "Probation");
                ws.Cells["G4"].PutValue(param.Language == "tw" ? "權限身分別" : "Permission Group");
                ws.Cells["H4"].PutValue(param.Language == "tw" ? "薪資計別" : "Salary Type");
                ws.Cells["I4"].PutValue(param.Language == "tw" ? "計薪天數" : "Paid Salary Days");
                ws.Cells["J4"].PutValue(param.Language == "tw" ? "實際上班天數" : "Actual Work Days");
                ws.Cells["K4"].PutValue(param.Language == "tw" ? "遲到/早退(次)" : "Delay/Early(times)");
                ws.Cells["L4"].PutValue(param.Language == "tw" ? "未刷卡(次)" : "No swip card(times)");
                ws.Cells["M4"].PutValue(param.Language == "tw" ? "白班伙食次數" : "Day Shift Meal Times");
                ws.Cells["N4"].PutValue(param.Language == "tw" ? "加班伙食費" : "Overtime Meal Times");
                ws.Cells["O4"].PutValue(param.Language == "tw" ? "夜點費次數" : "Night Shift Allowance Times");
                ws.Cells["P4"].PutValue(param.Language == "tw" ? "夜班伙食次數" : "Night Shift Meal Times");

                designer.SetDataSource("result", data);
                designer.Process();

                int range = 0;
                Style style = ws.Cells["A4"].GetStyle();
                if (leaves.Any())
                {
                    Aspose.Cells.Range leaveRange = ws.Cells.CreateRange(3, 16, 1, leaves.Count);
                    style.ForegroundColor = Color.FromArgb(255, 242, 204);
                    leaveRange.SetStyle(style);
                    leaveRange.ColumnWidth = 20;

                    ArrayList leaveCodes = new();
                    leaves.ForEach(item => { leaveCodes.Add(item.Value); });
                    ws.Cells.ImportArrayList(leaveCodes, 3, 16, false);

                    range += leaves.Count;
                }

                if (allowances.Any())
                {
                    Aspose.Cells.Range allowanceRange = ws.Cells.CreateRange(3, 16 + leaves.Count, 1, allowances.Count);
                    style.ForegroundColor = Color.FromArgb(226, 239, 218);
                    allowanceRange.SetStyle(style);
                    allowanceRange.ColumnWidth = 20;

                    ArrayList allowanceCodes = new();
                    allowances.ForEach(item => { allowanceCodes.Add(item.Value); });
                    ws.Cells.ImportArrayList(allowanceCodes, 3, 16 + leaves.Count, false);

                    range += allowances.Count;
                }

                if (range > 0)
                {
                    Style styleRange = designer.Workbook.CreateStyle();
                    styleRange = AsposeUtility.SetAllBorders(styleRange);

                    Aspose.Cells.Range allowanceRange = ws.Cells.CreateRange(4, 16, data.Count, range);
                    allowanceRange.SetStyle(styleRange);
                }

                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].LeaveCodes.Any())
                    {
                        ArrayList leaveCodeDetail = new();
                        data[i].LeaveCodes.ForEach(item => { leaveCodeDetail.Add(item); });
                        ws.Cells.ImportArrayList(leaveCodeDetail, i + 4, 16, false);
                    }

                    if (data[i].AllowanceCodes.Any())
                    {
                        ArrayList allowanceDetail = new();
                        data[i].AllowanceCodes.ForEach(item => { allowanceDetail.Add(item); });
                        ws.Cells.ImportArrayList(allowanceDetail, i + 4, 16 + data[i].LeaveCodes.Count, false);
                    }
                }

                designer.Workbook.Save(stream, SaveFormat.Xlsx);
                return new OperationResult(true, stream.ToArray());
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.InnerException.Message);
            }
        }

        private async Task<List<ResignedEmployeeDetail>> GetDataExport(ResignedEmployeeParam param, List<KeyValuePair<string, string>> leaves, List<KeyValuePair<string, string>> allowances)
        {
            var departments = await GetDepartmentMain(param.Language);
            var groups = await GetListPermissionGroup(param.Language);
            var types = await GetListSalaryType(param.Language);

            var (predResignMonthly, predProbation, predEmp) = SetPredicate(param);
            var HARM = _repositoryAccessor.HRMS_Att_Resign_Monthly
                .FindAll(predResignMonthly, true)
                .Project().To<ResignedEmployeeDto>(cfg => cfg.Map(false).To(dto => dto.isProbation));
            var HAPM = _repositoryAccessor.HRMS_Att_Probation_Monthly
                .FindAll(predProbation, true)
                .Project().To<ResignedEmployeeDto>(cfg => cfg.Map(true).To(dto => dto.isProbation));
            var HAPM_HARM = HAPM
                       .Join(HARM,
                       hapm => new { hapm.Factory, hapm.Att_Month, hapm.Employee_ID },
                       harm => new { harm.Factory, harm.Att_Month, harm.Employee_ID },
                       (hapm, harm) => hapm);
            var query = await HARM.Union(HAPM_HARM)
                .GroupJoin(_repositoryAccessor.HRMS_Emp_Personal.FindAll(predEmp, true),
                    x => x.USER_GUID,
                    y => y.USER_GUID,
                    (x, y) => new { resigned = x, emp = y })
                .SelectMany(
                    x => x.emp.DefaultIfEmpty(),
                    (x, y) => new { x.resigned, emp = y })
                .Select(x => new { x.resigned, x.emp })
                .ToListAsync();

            List<ResignedEmployeeDetail> datas = new();
            foreach (var x in query)
            {
                ResignedEmployeeDetail data = new()
                {
                    USER_GUID = x.emp?.USER_GUID,
                    Factory = x.resigned.Factory,
                    Employee_ID = x.resigned.Employee_ID,
                    Att_Month = x.resigned.Att_Month.ToString("yyyy/MM"),
                    Department = x.emp == null
                        ? ""
                        : string.IsNullOrWhiteSpace(x.emp.Employment_Status)
                        ? x.emp.Department
                        : x.emp.Employment_Status == "A" || x.emp.Employment_Status == "S"
                        ? x.emp.Assigned_Department
                        : "",
                    Department_Name = x.emp == null
                        ? ""
                        : string.IsNullOrWhiteSpace(x.emp.Employment_Status)
                        ? departments.FirstOrDefault(
                            d => d.Division == x.emp.Division &&
                            d.Factory == x.emp.Factory &&
                            d.Department_Code == x.emp.Department)?.Department_Name
                        : x.emp.Employment_Status == "A" || x.emp.Employment_Status == "S"
                        ? departments.FirstOrDefault(
                            d => d.Division == x.emp.Assigned_Division &&
                            d.Factory == x.emp.Assigned_Factory &&
                            d.Department_Code == x.emp.Assigned_Department)?.Department_Name
                        : "",
                    Permission_Group = x.resigned.Permission_Group,
                    Permission_Group_Name = groups.Any(g => g.Key == x.resigned.Permission_Group)
                        ? groups.FirstOrDefault(g => g.Key == x.resigned.Permission_Group).Value
                        : "",
                    Salary_Type = x.resigned.Salary_Type,
                    Salary_Type_Name = types.Any(g => g.Key == x.resigned.Salary_Type)
                        ? types.FirstOrDefault(g => g.Key == x.resigned.Salary_Type).Value
                        : "",
                    Local_Full_Name = x.emp?.Local_Full_Name,
                    Actual_Days = x.resigned.Actual_Days,
                    Pass = x.resigned.Pass ? "Y" : "N",
                    Resign_Status = x.resigned.Resign_Status,
                    Salary_Days = x.resigned.Salary_Days,
                    Delay_Early = x.resigned.Delay_Early,
                    Division = x.resigned.Division,
                    Food_Expenses = x.resigned.Food_Expenses,
                    Night_Eat_Times = x.resigned.Night_Eat_Times,
                    DayShift_Food = x.resigned.DayShift_Food,
                    NightShift_Food = x.resigned.NightShift_Food,
                    No_Swip_Card = x.resigned.No_Swip_Card,
                    Update_By = x.resigned.Update_By,
                    Update_Time = x.resigned.Update_Time.ToString("yyyy/MM/dd HH:mm:ss"),
                    Probation = x.resigned.Probation
                };
                var details = !param.isProbation
                    ? await _repositoryAccessor.HRMS_Att_Resign_Monthly_Detail.FindAll(y =>
                        y.Factory == data.Factory &&
                        y.Employee_ID == data.Employee_ID &&
                        y.Att_Month == Convert.ToDateTime(x.resigned.Att_Month).Date, true)
                        .Project().To<ResignedEmployeeDto_Detail>(cfg => cfg.Map(false).To(dto => dto.isProbation)).Distinct().ToListAsync()
                    : await _repositoryAccessor.HRMS_Att_Probation_Monthly_Detail.FindAll(y =>
                        y.Factory == data.Factory &&
                        y.Employee_ID == data.Employee_ID &&
                        y.Att_Month == Convert.ToDateTime(x.resigned.Att_Month).Date, true)
                        .Project().To<ResignedEmployeeDto_Detail>(cfg => cfg.Map(true).To(dto => dto.isProbation)).Distinct().ToListAsync();
                if (details.Any())
                {
                    var leaveCodes = details.Where(x => x.Leave_Type == "1").ToList();
                    foreach (var item in leaves)
                    {
                        if (leaveCodes.Any(l => l.Leave_Code == item.Key))
                            data.LeaveCodes.Add(leaveCodes.FirstOrDefault(l => l.Leave_Code == item.Key)?.Days);
                        else
                            data.LeaveCodes.Add(null);
                    }

                    var allowanceCodes = details.Where(x => x.Leave_Type == "2").ToList();
                    foreach (var item in allowances)
                    {
                        if (allowanceCodes.Any(a => a.Leave_Code == item.Key))
                            data.AllowanceCodes.Add(allowanceCodes.FirstOrDefault(a => a.Leave_Code == item.Key)?.Days);
                        else
                            data.AllowanceCodes.Add(null);
                    }
                }

                datas.Add(data);
            }

            if (!string.IsNullOrWhiteSpace(param.Department))
                datas = datas.Where(x => x.Department == param.Department).ToList();

            return datas.OrderBy(x => x.Department).ToList();
        }
        #endregion

        #region Frivate functions
        private async Task<List<KeyValuePair<string, string>>> GetBasicCodes(string language, ExpressionStarter<HRMS_Basic_Code> predicate)
        {
            var a = await _repositoryAccessor.HRMS_Basic_Code.FindAll(predicate, true).ToListAsync();
            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(predicate, true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (x, y) => new { code = x, codeLang = y })
                .SelectMany(
                    x => x.codeLang.DefaultIfEmpty(),
                    (x, y) => new { x.code, codeLang = y })
                .Select(x => new KeyValuePair<string, string>(x.code.Code, $"{x.code.Code} - {x.codeLang.Code_Name ?? x.code.Code_Name}"))
                .Distinct().ToListAsync();

            return data;
        }

        private IOrderedQueryable<DepartmentJoinResult> QueryDepartment(ExpressionStarter<HRMS_Org_Department> predDept, ExpressionStarter<HRMS_Basic_Factory_Comparison> predCom, string language)
        {
            var data = _repositoryAccessor.HRMS_Org_Department.FindAll(predDept, true)
                .Join(_repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(predCom, true),
                    department => department.Division,
                    factoryComparison => factoryComparison.Division,
                    (department, factoryComparison) => department)
                .GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    department => new { department.Factory, department.Department_Code },
                    language => new { language.Factory, language.Department_Code },
                    (department, language) => new { Department = department, Language = language })
                .SelectMany(
                    x => x.Language.DefaultIfEmpty(),
                    (x, language) => new DepartmentJoinResult { Department = x.Department, Language = language })
                .OrderBy(x => x.Department.Department_Code);

            return data;
        }

        private async Task<List<DepartmentMain>> GetDepartmentMain(string language)
        {
            ExpressionStarter<HRMS_Org_Department> predDept = PredicateBuilder.New<HRMS_Org_Department>(true);
            ExpressionStarter<HRMS_Basic_Factory_Comparison> predCom = PredicateBuilder.New<HRMS_Basic_Factory_Comparison>(x => x.Kind == "1");
            var data = await QueryDepartment(predDept, predCom, language)
                .Select(
                    x => new DepartmentMain
                    {
                        Division = x.Department.Division,
                        Factory = x.Department.Factory,
                        Department_Code = x.Department.Department_Code,
                        Department_Name = $"{x.Department.Department_Code} - {(x.Language != null ? x.Language.Name : x.Department.Department_Name)}"
                    })
                .ToListAsync();

            return data;
        }

        private (ExpressionStarter<HRMS_Att_Resign_Monthly> predResignMonthly, ExpressionStarter<HRMS_Att_Probation_Monthly> predProbation, ExpressionStarter<HRMS_Emp_Personal> predEmp) SetPredicate(ResignedEmployeeParam param)
        {
            var predResignMonthly = PredicateBuilder.New<HRMS_Att_Resign_Monthly>(true);
            var predProbation = PredicateBuilder.New<HRMS_Att_Probation_Monthly>(true);
            var predEmp = PredicateBuilder.New<HRMS_Emp_Personal>(true);
            var permissionGroupQuery = _repositoryAccessor.HRMS_Basic_Role.FindAll(x => x.Factory == param.Factory, true).Select(x => x.Permission_Group);
            if (!string.IsNullOrWhiteSpace(param.Factory))
            {
                predResignMonthly.And(x => x.Factory == param.Factory);
                predProbation.And(x => x.Factory == param.Factory);
                predEmp.And(x => (x.Factory == param.Factory || x.Assigned_Factory == param.Factory) && permissionGroupQuery.Contains(x.Permission_Group));
            }

            if (!string.IsNullOrWhiteSpace(param.Department))
            {
                predResignMonthly.And(x => x.Department == param.Department);
                predProbation.And(x => x.Department == param.Department);
            }

            if (!string.IsNullOrWhiteSpace(param.Employee_ID))
            {
                predResignMonthly.And(x => x.Employee_ID.Contains(param.Employee_ID));
                predProbation.And(x => x.Employee_ID.Contains(param.Employee_ID));
                predEmp.And(x => x.Employee_ID.Contains(param.Employee_ID) || x.Assigned_Employee_ID.Contains(param.Employee_ID));
            }

            if (param.Salary_Days.HasValue && param.Salary_Days > 0)
            {
                predResignMonthly.And(x => x.Salary_Days == param.Salary_Days);
                predProbation.And(x => x.Salary_Days == param.Salary_Days);
            }

            if (!string.IsNullOrWhiteSpace(param.Att_Month_Start) && !string.IsNullOrWhiteSpace(param.Att_Month_End))
            {
                predResignMonthly.And(x => x.Att_Month.Date >= Convert.ToDateTime(param.Att_Month_Start).Date && x.Att_Month.Date <= Convert.ToDateTime(param.Att_Month_End).Date);
                predProbation.And(x => x.Att_Month.Date >= Convert.ToDateTime(param.Att_Month_Start).Date && x.Att_Month.Date <= Convert.ToDateTime(param.Att_Month_End).Date);
            }

            return (predResignMonthly, predProbation, predEmp);
        }

        private async Task<List<HRMS_Att_Yearly>> Upd_HRMS_Att_Yearly(AttYearlyUpdate update, List<HRMS_Att_Resign_Monthly_Detail> Resign_Monthly_Detail)
        {
            var codes = update.Details.Select(x => x.Code).ToList();
            var att_Resign_Monthly_Detail = Resign_Monthly_Detail
                            .FindAll(x => x.Factory == update.Factory
                                    && x.Att_Month == update.Att_Year
                                    && x.Employee_ID == update.Employee_ID
                                && x.USER_GUID == update.USER_GUID
                                && x.Leave_Type == update.Leave_Type
                                && codes.Contains(x.Leave_Code));
            var data = await _repositoryAccessor.HRMS_Att_Yearly
                .FindAll(
                    x => x.Factory == update.Factory &&
                    x.Att_Year == new DateTime(update.Att_Year.Year, 1, 1) &&
                    x.Employee_ID == update.Employee_ID &&
                    x.USER_GUID == update.USER_GUID &&
                    x.Leave_Type == update.Leave_Type &&
                    codes.Contains(x.Leave_Code))
                .ToListAsync();

            if (!data.Any())
                return new List<HRMS_Att_Yearly>();

            DateTime current = DateTime.Now;
            data.ForEach(x =>
            {
                var detail = update.Details.FirstOrDefault(d => d.Code == x.Leave_Code);
                var detailOld = att_Resign_Monthly_Detail.FirstOrDefault(d => d.Leave_Code == x.Leave_Code);
                if (detail is not null)
                {
                    x.Days += decimal.Parse(detail.Days) - (detailOld == null ? 0 : detailOld.Days);
                    x.Update_By = update.Account;
                    x.Update_Time = current;
                }
            });

            return data;
        }
        public async Task<List<KeyValuePair<string, string>>> GetEmployeeIDByFactorys(string factory)
        {
            var pred_Personal = PredicateBuilder.New<HRMS_Emp_Personal>(true);
            if (!string.IsNullOrWhiteSpace(factory))
                pred_Personal.And(x => x.Factory == factory);
            return await _repositoryAccessor.HRMS_Emp_Personal.FindAll(pred_Personal, true)
                        .Select(x => new KeyValuePair<string, string>(x.Employee_ID.Trim(), x.Employee_ID.Trim())).Distinct().ToListAsync();
        }
        public async Task<List<KeyValuePair<string, string>>> GetListFactoryByUser(string language, string userName)
        {
            var factorys = await Queryt_Factory_AddList(userName);
            var factories = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory && factorys.Contains(x.Code), true)
                        .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                                    x => new { x.Type_Seq, x.Code },
                                    y => new { y.Type_Seq, y.Code },
                                    (x, y) => new { x, y })
                                    .SelectMany(x => x.y.DefaultIfEmpty(),
                                    (x, y) => new { x.x, y })
                        .Select(x => new KeyValuePair<string, string>(x.x.Code, $"{x.x.Code} - {(x.y != null ? x.y.Code_Name : x.x.Code_Name)}")).ToListAsync();
            return factories;
        }
        #endregion
    }
}