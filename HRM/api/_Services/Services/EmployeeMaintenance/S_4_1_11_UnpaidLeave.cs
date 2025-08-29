using API.Data;
using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.EmployeeMaintenance
{
    public class S_4_1_11_UnpaidLeave : BaseServices, I_4_1_11_UnpaidLeave
    {
        public S_4_1_11_UnpaidLeave(DBContext dbContext) : base(dbContext) { }

        #region GetList
        public async Task<List<KeyValuePair<string, string>>> GetListDivision(string language)
        {
            return await GetHRMS_Basic_Code(BasicCodeTypeConstant.Division, language);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string division, string language)
        {
            var pred = PredicateBuilder.New<HRMS_Basic_Factory_Comparison>(x => x.Kind == "1");

            if (!string.IsNullOrWhiteSpace(division))
                pred = pred.And(x => x.Division.ToLower().Contains(division.ToLower()));

            var data = await _repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(pred)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower()),
                    x => x.Factory,
                    y => y.Code,
                    (x, y) => new { x.Factory, CodeNameLanguage = y.Select(z => z.Code_Name).FirstOrDefault() })
                .Join(_repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "2"),
                    x => x.Factory,
                    y => y.Code,
                    (x, y) => new { x.Factory, x.CodeNameLanguage, CodeName = y.Code_Name })
                .Select(x => new KeyValuePair<string, string>(x.Factory, x.CodeNameLanguage ?? x.CodeName))
                .Distinct().ToListAsync();

            if (!data.Any())
            {
                var allFactories = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "2")
                    .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower()),
                        x => x.Code,
                        y => y.Code,
                        (x, y) => new { x.Code, NameCode = x.Code_Name, NameLanguage = y.Select(z => z.Code_Name).FirstOrDefault() })
                    .Select(x => new KeyValuePair<string, string>(x.Code, x.NameLanguage ?? x.NameCode))
                    .ToListAsync();
                return allFactories;
            }
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListLeaveReason(string language)
        {
            return await GetHRMS_Basic_Code(BasicCodeTypeConstant.LeaveReason, language);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListDepartment(string division, string factory, string language)
        {
            return await _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Division == division && x.Factory == factory, true)
                .GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                      department => new { department.Division, department.Factory, department.Department_Code },
                      language => new { language.Division, language.Factory, language.Department_Code },
                    (department, language) => new { Department = department, Language = language })
                    .SelectMany(x => x.Language.DefaultIfEmpty(),
                    (x, language) => new { x.Department, Language = language })
                .Select(x => new KeyValuePair<string, string>(x.Department.Department_Code, $"{x.Department.Department_Code} - {(x.Language != null ? x.Language.Name : x.Department.Department_Name)}"))
                .ToListAsync();
        }

        private async Task<List<KeyValuePair<string, string>>> GetHRMS_Basic_Code(string Type_Seq, string Language)
        {
            return await _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => x.Type_Seq == Type_Seq, true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == Language.ToLower(), true),
                    HBC => new { HBC.Type_Seq, HBC.Code },
                    HBCL => new { HBCL.Type_Seq, HBCL.Code },
                    (HBC, HBCL) => new { HBC, HBCL })
                    .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (prev, HBCL) => new { prev.HBC, HBCL })
                .Select(x => new KeyValuePair<string, string>(x.HBC.Code, $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"))
                .ToListAsync();
        }
        #endregion

        #region Get Employee Data
        public async Task<List<string>> GetEmployeeID()
        {
            return await _repositoryAccessor.HRMS_Emp_Personal.FindAll(true).Select(x => x.Employee_ID).ToListAsync();
        }

        public async Task<List<HRMS_Emp_Unpaid_LeaveDto>> GetEmployeeData(string factory, string employeeID, string language)
        {
            var data = await _repositoryAccessor.HRMS_Emp_Personal
                .FindAll(x => x.Factory == factory && x.Employee_ID == employeeID, true)
                .GroupJoin(
                    _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Factory == factory),
                    employee => employee.Department,
                    department => department.Department_Code,
                    (employee, department) => new { Employee = employee, Department = department.DefaultIfEmpty() })
                .SelectMany(
                    x => x.Department,
                    (employee, department) => new { employee.Employee, Department = department })
                .GroupJoin(
                    _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    x => x.Department.Department_Code,
                    departmentLanguage => departmentLanguage.Department_Code,
                    (x, departmentLanguage) => new { x.Employee, x.Department, DepartmentLanguage = departmentLanguage })
                .SelectMany(
                    x => x.DepartmentLanguage.DefaultIfEmpty(),
                    (employee, departmentLanguage) => new { employee.Employee, employee.Department, DepartmentLanguage = departmentLanguage })
                .Select(
                    x => new HRMS_Emp_Unpaid_LeaveDto
                    {
                        Local_Full_Name = x.Employee.Local_Full_Name,
                        Department = $"{x.Employee.Department} - {(x.DepartmentLanguage != null ? x.DepartmentLanguage.Name : x.Department.Department_Name)}",
                        Onboard_Date = x.Employee.Onboard_Date.ToString("yyyy/MM/dd")
                    })
                .ToListAsync();

            return data;
        }

        public async Task<OperationResult> GetSeq(string division, string factory, string employeeID)
        {
            var employee = await _repositoryAccessor.HRMS_Emp_Personal
                        .FindAll(x => x.Division == division && x.Factory == factory && x.Employee_ID == employeeID.Trim(), true).AnyAsync();

            if (!employee)
                return new OperationResult(false, "No Data");

            var seqList = await _repositoryAccessor.HRMS_Emp_Unpaid_Leave
                        .FindAll(x => x.Division == division && x.Factory == factory && x.Employee_ID == employeeID.Trim(), true)
                        .Select(x => x.Seq).OrderBy(x => x).ToListAsync();

            int nextSeq = 1;

            foreach (var i in seqList)
            {
                if (i > nextSeq)
                    break;
                nextSeq++;
            }

            return new OperationResult { IsSuccess = true, Data = nextSeq };
        }
        #endregion

        #region GetData
        private async Task<List<HRMS_Emp_Unpaid_LeaveDto>> GetData(UnpaidLeaveParam param, List<string> roleList)
        {
            var pred = PredicateBuilder.New<HRMS_Emp_Unpaid_Leave>(x => x.Division == param.Division && x.Factory == param.Factory);
            var predDepartment = PredicateBuilder.New<HRMS_Org_Department>(x => x.Division == param.Division && x.Factory == param.Factory);
            var predPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(x => x.Division == param.Division && x.Factory == param.Factory);

            if (!string.IsNullOrWhiteSpace(param.Employee_ID))
                pred.And(x => x.Employee_ID.Contains(param.Employee_ID.Trim()));
            if (!string.IsNullOrWhiteSpace(param.Local_Full_Name))
                predPersonal.And(x => x.Local_Full_Name.Contains(param.Local_Full_Name.Trim()));
            if (!string.IsNullOrWhiteSpace(param.Onboard_Date))
                predPersonal.And(x => x.Onboard_Date == Convert.ToDateTime(param.Onboard_Date));

            if (!string.IsNullOrEmpty(param.Leave_Start_From))
                pred.And(x => x.Leave_Start >= Convert.ToDateTime(param.Leave_Start_From));
            if (!string.IsNullOrEmpty(param.Leave_Start_To))
                pred.And(x => x.Leave_Start <= Convert.ToDateTime(param.Leave_Start_To));

            if (!string.IsNullOrEmpty(param.Leave_End_From))
                pred.And(x => x.Leave_End >= Convert.ToDateTime(param.Leave_End_From));
            if (!string.IsNullOrEmpty(param.Leave_End_To))
                pred.And(x => x.Leave_End <= Convert.ToDateTime(param.Leave_End_To));

            if (!string.IsNullOrWhiteSpace(param.Department))
                predDepartment.And(x => x.Department_Code.Contains(param.Department));
            if (!string.IsNullOrWhiteSpace(param.Leave_Reason))
                pred.And(x => x.Leave_Reason.Contains(param.Leave_Reason.Trim()));

            var departmentQuery = _repositoryAccessor.HRMS_Org_Department.FindAll(predDepartment, true);
            var personalQuery = await Query_Permission_Data_Filter(roleList, predPersonal);
            var depLanguageQuery = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower(), true)
                                                        .Where(x => x.Division == param.Division && x.Factory == param.Factory);

            var basicCode = _repositoryAccessor.HRMS_Basic_Code.FindAll();
            var basicLanguage = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower());
            var codeLang = await basicCode.GroupJoin(basicLanguage,
                x => new { x.Type_Seq, x.Code },
                y => new { y.Type_Seq, y.Code },
                    (x, y) => new { Code = x, Language = y })
                .SelectMany(x => x.Language.DefaultIfEmpty(),
                    (x, y) => new { x.Code, Language = y })
                .Select(x => new
                {
                    x.Code.Code,
                    x.Code.Type_Seq,
                    Code_Name = x.Language != null ? x.Language.Code_Name : x.Code.Code_Name
                }).Distinct().ToListAsync();

            var data = _repositoryAccessor.HRMS_Emp_Unpaid_Leave.FindAll(pred, true)
                .AsQueryable()
                .ToList();

            var result = data
            .Join(
                personalQuery,
                leave => leave.Employee_ID,
                personal => personal.Employee_ID,
                (leave, personal) => new { Leave = leave, Personal = personal }
                )
            .Join(
                departmentQuery,
                x => x.Personal.Department,
                department => department.Department_Code,
                (x, department) => new { x.Leave, x.Personal, Department = department }
                )
            .GroupJoin(
                depLanguageQuery,
                x => x.Department?.Department_Code,
                lang => lang.Department_Code,
                (x, lang) => new { x.Leave, x.Personal, x.Department, LanguageDepartment = lang }
                )
            .SelectMany(
                x => x.LanguageDepartment.DefaultIfEmpty(),
                (x, lang) => new { x.Leave, x.Personal, x.Department, LanguageDepartment = lang }
                )
            .Select(
                x => new HRMS_Emp_Unpaid_LeaveDto
                {
                    Division = x.Leave.Division,
                    Division_Str = $"{x.Leave.Division} - {codeLang.FirstOrDefault(y => y.Code == x.Leave.Division && y.Type_Seq == "1").Code_Name}",
                    Factory = x.Leave.Factory,
                    Factory_Str = $"{x.Leave.Division} - {codeLang.FirstOrDefault(y => y.Code == x.Leave.Factory && y.Type_Seq == "2").Code_Name}",
                    Employee_ID = x.Leave.Employee_ID,
                    Department = $"{x.Personal.Department} - {x.Department.Department_Name}",
                    Department_Code = x.Personal?.Department,
                    Department_Name = x.LanguageDepartment?.Name ?? x.Department?.Department_Name,
                    Local_Full_Name = x.Personal != null ? x.Personal.Local_Full_Name : null,
                    Ordinal_Number = x.Leave.Seq,
                    Leave_Reason = x.Leave.Leave_Reason,
                    Leave_Reason_Str = $"{x.Leave.Leave_Reason} - {codeLang.FirstOrDefault(y => y.Code == x.Leave.Leave_Reason && y.Type_Seq == "31").Code_Name}",
                    Onboard_Date = x.Personal.Onboard_Date.ToString("yyyy/MM/dd"),
                    Leave_Start = x.Leave.Leave_Start.ToString("yyyy/MM/dd"),
                    Leave_End = x.Leave.Leave_End.ToString("yyyy/MM/dd"),
                    Continuation_of_Insurance = x.Leave.Continuation_of_Insurance,
                    Continuation_of_Insurance_Str = x.Leave.Continuation_of_Insurance ? "Y" : "N",
                    Seniority_Retention = x.Leave.Seniority_Retention,
                    Seniority_Retention_Str = x.Leave.Seniority_Retention ? "Y" : "N",
                    Annual_Leave_Seniority_Retention = x.Leave.Annual_Leave_Seniority_Retention,
                    Annual_Leave_Seniority_Retention_Str = x.Leave.Annual_Leave_Seniority_Retention ? "Y" : "N",
                    Effective_Status = x.Leave.Effective_Status,
                    Effective_Status_Str = x.Leave.Effective_Status ? "Y" : "N",
                    Remark = x.Leave.Remark,
                    Update_By = x.Leave.Update_By,
                    Update_Time = x.Leave.Update_Time,
                })
                .OrderBy(x => x.Division).ThenBy(x => x.Factory)
                .ThenBy(x => x.Employee_ID).ThenBy(x => x.Ordinal_Number)
                .ToList();

            var groupedResult = result.GroupBy(x => new { x.Division, x.Factory, x.Employee_ID, x.Ordinal_Number })
              .Select(x => x.First())
              .ToList();

            return groupedResult;
        }

        public async Task<PaginationUtility<HRMS_Emp_Unpaid_LeaveDto>> GetDataPagination(PaginationParam pagination, UnpaidLeaveParam param, List<string> roleList)
        {
            var data = await GetData(param, roleList);
            return PaginationUtility<HRMS_Emp_Unpaid_LeaveDto>.Create(data, pagination.PageNumber, pagination.PageSize);
        }
        #endregion

        #region Download
        public async Task<OperationResult> DownloadExcel(UnpaidLeaveParam param, List<string> roleList, string userName)
        {
            var data = await GetData(param, roleList);
            if (!data.Any())
                return new OperationResult(false, "No Data");
            List<Table> tables = new()
            {
                new Table("result", data)
            };
            List<Cell> cells = new()
            {
                new Cell("B1", userName),
                new Cell("D1", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")),
            };
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                tables, 
                cells, 
                "Resources\\Template\\EmployeeMaintenance\\4_1_11_UnpaidLeave\\Download.xlsx"
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }
        #endregion

        #region Add & Edit
        public async Task<OperationResult> AddNew(AddAndEditParam param, string userName)
        {
            var existingData = await _repositoryAccessor.HRMS_Emp_Unpaid_Leave.AnyAsync(x => x.Division == param.Division && x.Factory == param.Factory
                                                                                        && x.Employee_ID == param.Employee_ID && x.Seq == param.Ordinal_Number);

            if (existingData)
                return new OperationResult(false, "Data already exists");

            var newData = new HRMS_Emp_Unpaid_Leave
            {
                Division = param.Division,
                Factory = param.Factory,
                Employee_ID = param.Employee_ID,
                Seq = param.Ordinal_Number,
                Leave_Reason = param.Leave_Reason,
                Leave_Start = Convert.ToDateTime(param.Leave_Start),
                Leave_End = Convert.ToDateTime(param.Leave_End),
                Continuation_of_Insurance = param.Continuation_of_Insurance,
                Seniority_Retention = param.Seniority_Retention,
                Annual_Leave_Seniority_Retention = param.Annual_Leave_Seniority_Retention,
                Effective_Status = param.Effective_Status,
                Remark = param.Remark,
                Update_By = userName,
                Update_Time = DateTime.Now
            };
            _repositoryAccessor.HRMS_Emp_Unpaid_Leave.Add(newData);

            try
            {
                await _repositoryAccessor.Save();
                return new OperationResult(true, "Add Successfully");
            }
            catch (Exception ex)
            {
                return new OperationResult(false, $"Inner exception: {ex.InnerException?.Message ?? "No inner exception message available"}");
            }
        }

        public async Task<OperationResult> Edit(AddAndEditParam param, string userName)
        {
            var existingData = await _repositoryAccessor.HRMS_Emp_Unpaid_Leave
                                    .FirstOrDefaultAsync(x => x.Division == param.Division && x.Factory == param.Factory
                                                                && x.Employee_ID == param.Employee_ID && x.Seq == param.Ordinal_Number, true);

            if (existingData == null)
                return new OperationResult(false, "No Data");

            existingData.Leave_Reason = param.Leave_Reason;
            existingData.Leave_Start = Convert.ToDateTime(param.Leave_Start);
            existingData.Leave_End = Convert.ToDateTime(param.Leave_End);
            existingData.Continuation_of_Insurance = param.Continuation_of_Insurance;
            existingData.Seniority_Retention = param.Seniority_Retention;
            existingData.Annual_Leave_Seniority_Retention = param.Annual_Leave_Seniority_Retention;
            existingData.Effective_Status = param.Effective_Status;
            existingData.Remark = param.Remark;
            existingData.Update_By = userName;
            existingData.Update_Time = DateTime.Now;
            _repositoryAccessor.HRMS_Emp_Unpaid_Leave.Update(existingData);

            try
            {
                await _repositoryAccessor.Save();
                return new OperationResult(true, "Update Successfully");
            }
            catch (Exception ex)
            {
                return new OperationResult(false, $"Inner exception: {ex.InnerException?.Message ?? "No inner exception message available"}");
            }
        }
        #endregion

        #region Delete
        public async Task<OperationResult> Delete(HRMS_Emp_Unpaid_LeaveDto data)
        {
            var existingData = await _repositoryAccessor.HRMS_Emp_Unpaid_Leave
                                .FirstOrDefaultAsync(x => x.Employee_ID == data.Employee_ID && x.Seq == data.Ordinal_Number, true);
            if (existingData == null)
                return new OperationResult(false, "Data is not exist");
            _repositoryAccessor.HRMS_Emp_Unpaid_Leave.Remove(existingData);
            if (await _repositoryAccessor.Save())
                return new OperationResult(true, "Delete Successfully");
            return new OperationResult(false, "Delete failed");
        }
        #endregion
    }
}