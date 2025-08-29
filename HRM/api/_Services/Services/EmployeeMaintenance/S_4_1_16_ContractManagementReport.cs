using System.Data;
using System.Data.SqlTypes;
using API.Data;
using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.EmployeeMaintenance
{
    public partial class S_4_1_16_ContractManagementReport : BaseServices, I_4_1_16_ContractManagementReport
    {
        public S_4_1_16_ContractManagementReport(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult> DownloadExcel(ContractManagementReportParam param)
        {
            var data = await GetData(param);
            if (!data.Any())
                return new OperationResult(false, "No Data");
            List<Table> tables = new()
            {
                new Table("result", data)
            };
            List<Cell> cells = new()
            {
                new Cell("B1", param.Division),
                new Cell("D1", param.Factory),
                new Cell("F1", param.Document_Type  + " - " + param.Document_Type_Name),
            };
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                tables, 
                cells, 
                "Resources\\Template\\EmployeeMaintenance\\4_1_16_Contract_Manegement_Report\\Download.xlsx"
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }

        public async Task<List<ContractManagementReportDto>> GetData(ContractManagementReportParam param)
        {
            var pred = PredicateBuilder.New<HRMS_Emp_Contract_Management>(true);

            if (!string.IsNullOrWhiteSpace(param.Division))
                pred.And(x => x.Division == param.Division);
            if (!string.IsNullOrWhiteSpace(param.Factory))
                pred.And(x => x.Factory == param.Factory);
            if (!string.IsNullOrWhiteSpace(param.Contract_Type))
                pred.And(x => x.Contract_Type == param.Contract_Type);

            var data = await _repositoryAccessor.HRMS_Emp_Contract_Management.FindAll(pred, true)
                    .GroupJoin(_repositoryAccessor.HRMS_Emp_Personal.FindAll(true),
                        x => new { x.Division, x.Factory, x.Employee_ID },
                        y => new { y.Division, y.Factory, y.Employee_ID },
                        (x, y) => new { HECM = x, HEP = y })
                    .SelectMany(x => x.HEP.DefaultIfEmpty(),
                        (x, y) => new { x.HECM, HEP = y })
                    .GroupJoin(_repositoryAccessor.HRMS_Org_Department.FindAll(true),
                        x => new { x.HEP.Department, x.HEP.Division, x.HEP.Factory },
                        y => new { Department = y.Department_Code, y.Division, y.Factory },
                        (x, y) => new { x.HECM, x.HEP, HOD = y })
                    .SelectMany(x => x.HOD.DefaultIfEmpty(),
                        (x, y) => new { x.HECM, x.HEP, HOD = y })
                    .GroupJoin(_repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "36", true),
                        x => x.HECM.Assessment_Result,
                        y => y.Code,
                        (x, y) => new { x.HECM, x.HEP, x.HOD, HBCA = y })
                    .SelectMany(x => x.HBCA.DefaultIfEmpty(),
                        (x, y) => new { x.HECM, x.HEP, x.HOD, HBCA = y })
                    .GroupJoin(_repositoryAccessor.HRMS_Emp_Contract_Type.FindAll(true),
                        x => new { x.HECM.Contract_Type, x.HECM.Division, x.HECM.Factory },
                        y => new { y.Contract_Type, y.Division, y.Factory },
                        (x, y) => new { x.HECM, x.HEP, x.HOD, x.HBCA, HECT = y })
                    .SelectMany(x => x.HECT.DefaultIfEmpty(),
                        (x, y) => new { x.HECM, x.HEP, x.HOD, x.HBCA, HECT = y })
                    .GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower(), true),
                        x => new { x.HOD.Department_Code, x.HOD.Division, x.HOD.Factory },
                        y => new { y.Department_Code, y.Division, y.Factory },
                        (x, y) => new { x.HECM, x.HEP, x.HOD, x.HBCA, x.HECT, HODL = y })
                    .SelectMany(x => x.HODL.DefaultIfEmpty(),
                        (x, y) => new { x.HECM, x.HEP, x.HOD, x.HBCA, x.HECT, HODL = y })
                    .Select((x) => new ContractManagementReportDto
                    {
                        Division = x.HECM.Division,
                        Factory = x.HECM.Factory,
                        Contract_Type = x.HECT.Contract_Type,
                        Contract_Type_Name = x.HECM.Contract_Type + (x.HECT != null ? " - " + x.HECT.Contract_Title : ""),
                        Document_Type = param.Document_Type,
                        Document_Type_Name = param.Document_Type + " - " + param.Document_Type_Name,
                        Salary_Type = "",
                        Department = x.HEP != null ? x.HEP.Department : "",
                        Department_Name = x.HODL != null ? x.HODL.Name : (x.HOD != null ? x.HOD.Department_Name : ""),
                        Employee_ID = x.HECM.Employee_ID,
                        Local_Full_Name = x.HEP != null ? x.HEP.Local_Full_Name : "",
                        Contract_Start = x.HECM.Contract_Start.ToString("yyyy/MM/dd"),
                        Contract_End = x.HECM.Contract_End.ToString("yyyy/MM/dd"),
                        Onboard_Date = x.HEP != null ? x.HEP.Onboard_Date : null,
                        Probation_Start = x.HECM.Probation_Start.Value.ToString("yyyy/MM/dd"),
                        Probation_End = x.HECM.Probation_End.Value.ToString("yyyy/MM/dd"),
                        Assessment_Result = x.HECM.Assessment_Result + (x.HBCA != null ? " - " + x.HBCA.Code_Name : ""),
                        Extend_To = x.HECM.Extend_to,
                        Reason = x.HECM.Reason,
                    })
                    .OrderBy(x => x.Division)
                    .ThenBy(x => x.Factory)
                    .ThenBy(x => x.Department)
                    .ThenBy(x => x.Contract_Type)
                    .ThenBy(x => x.Employee_ID)
                    .ToListAsync();

            if (!string.IsNullOrWhiteSpace(param.Employee_ID_From) && !string.IsNullOrWhiteSpace(param.Employee_ID_To))
            {
                data = data.Where(x => MyRegex().IsMatch(x.Employee_ID)).AsEnumerable()
                    .Where(x =>
                    {
                        var numericPart = new string(x.Employee_ID.Where(char.IsDigit).ToArray());
                        var intFrom = new string(param.Employee_ID_From.Where(char.IsDigit).ToArray());
                        var intTo = new string(param.Employee_ID_To.Where(char.IsDigit).ToArray());
                        if (int.TryParse(numericPart, out int employeeId) && int.TryParse(intFrom, out int employeeIdFrom) && int.TryParse(intTo, out int employeeIdTo))
                            return employeeId >= employeeIdFrom && employeeId <= employeeIdTo;

                        return false;
                    })
                    .ToList();
            }
            else if (!string.IsNullOrWhiteSpace(param.Employee_ID_From))
                data = data.Where(x => x.Employee_ID.Contains(param.Employee_ID_From)).ToList();
            else if (!string.IsNullOrWhiteSpace(param.Employee_ID_To))
                data = data.Where(x => x.Employee_ID.Contains(param.Employee_ID_To)).ToList();

            if (param.Department != null && param.Department.Any())
                data = data.Where(x => param.Department.Contains(x.Department.Trim())).ToList();
            else if (!string.IsNullOrWhiteSpace(param.Department_From))
                data = data.Where(x => param.Department_From.Contains(x.Department.Trim())).ToList();
            else if (!string.IsNullOrWhiteSpace(param.Department_To))
                data = data.Where(x => param.Department_To.Contains(x.Department.Trim())).ToList();

            DateTime onboardDateStart = param.Onboard_Date_From != null ? Convert.ToDateTime(param.Onboard_Date_From + " 00:00:00.000") : SqlDateTime.MinValue.Value;
            DateTime onboardDateEnd = param.Onboard_Date_To != null ? Convert.ToDateTime(param.Onboard_Date_To + " 23:59:59.997") : SqlDateTime.MaxValue.Value;

            if (!string.IsNullOrWhiteSpace(param.Onboard_Date_From) && !string.IsNullOrWhiteSpace(param.Onboard_Date_To))
                data = data.Where(x => x.Onboard_Date >= Convert.ToDateTime(param.Onboard_Date_From)
                           && x.Onboard_Date <= Convert.ToDateTime(param.Onboard_Date_To)).ToList();
            else if(!string.IsNullOrWhiteSpace(param.Onboard_Date_From) || !string.IsNullOrWhiteSpace(param.Onboard_Date_To))
                data = data.Where(x => x.Onboard_Date >= onboardDateStart && x.Onboard_Date <= onboardDateEnd).ToList();

            DateTime ContractEndFrom = param.Contract_End_From != null ? Convert.ToDateTime(param.Contract_End_From + " 00:00:00.000") : SqlDateTime.MinValue.Value;
            DateTime ContractEndTo = param.Contract_End_To != null ? Convert.ToDateTime(param.Contract_End_To + " 23:59:59.997") : SqlDateTime.MaxValue.Value;

            if (ContractEndFrom <= ContractEndTo)
                data = data.Where(x => Convert.ToDateTime(x.Contract_End) >= ContractEndFrom && Convert.ToDateTime(x.Contract_End) <= ContractEndTo)
                .ToList();

            int i = 1;
            data.ForEach(x => x.Seq = i++);
            return data;
        }

        public async Task<PaginationUtility<ContractManagementReportDto>> GetDataPagination(PaginationParam pagination, ContractManagementReportParam param)
        {
            var data = await GetData(param);
            return PaginationUtility<ContractManagementReportDto>.Create(data, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListDivision(string lang)
        {
            var listDivision = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.IsActive == true && x.Type_Seq == "1", true)
                            .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == lang.ToLower(), true),
                                x => new { x.Type_Seq, x.Code },
                                y => new { y.Type_Seq, y.Code },
                                (x, y) => new { basicCode = x, basicCodeLang = y })
                            .SelectMany(x => x.basicCodeLang.DefaultIfEmpty(),
                                (x, y) => new { x.basicCode, basicCodeLang = y })
                            .Select(x => new KeyValuePair<string, string>(
                                x.basicCode.Code,
                                x.basicCodeLang != null ? x.basicCodeLang.Code_Name : x.basicCode.Code_Name
                            ))
                            .ToListAsync();
            return listDivision;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string division, string lang)
        {
            var pred = PredicateBuilder.New<HRMS_Basic_Factory_Comparison>(x => x.Kind == "1");
            if (!string.IsNullOrWhiteSpace(division))
                pred.And(x => x.Division.ToLower() == division.Trim().ToLower());

            var data = await _repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(pred, true)
                    .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == lang.ToLower(), true),
                        x => x.Factory,
                        y => y.Code,
                        (x, y) => new { x.Factory, CodeNameLanguage = y.Select(z => z.Code_Name).FirstOrDefault() })
                    .Join(_repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "2", true),
                    x => x.Factory,
                    y => y.Code,
                    (x, y) => new { x.Factory, x.CodeNameLanguage, y.Code_Name })
                    .Select(x => new KeyValuePair<string, string>(
                        x.Factory,
                        x.CodeNameLanguage ?? x.Code_Name))
                    .Distinct()
                    .ToListAsync();
            if (!data.Any())
            {
                var allFactories = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "2", true)
                                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == lang.ToLower(), true),
                                    x => new { x.Type_Seq, x.Code },
                                    y => new { y.Type_Seq, y.Code },
                                    (x, y) => new { basicCode = x, basicCodeLang = y })
                                .SelectMany(x => x.basicCodeLang.DefaultIfEmpty(),
                                    (x, y) => new { x.basicCode, basicCodeLang = y })
                                .Select(x => new KeyValuePair<string, string>
                                (
                                    x.basicCode.Code,
                                    x.basicCodeLang != null ? x.basicCodeLang.Code_Name : x.basicCode.Code_Name
                                ))
                                .Distinct()
                                .ToListAsync();
                return allFactories;
            }
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListContractType(string division, string factory, string lang)
        {
            return await _repositoryAccessor.HRMS_Emp_Contract_Type
                .FindAll(x => (division == null || x.Division == division) && (factory == null || x.Factory == factory))
                .Select(x => new KeyValuePair<string, string>(x.Contract_Type, x.Contract_Title))
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<KeyValuePair<string, string>>> GetListDepartment(string division, string factory, string lang)
        {
            var department = _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Division == division && x.Factory == factory, true);
            var departmentLanguage = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => 
                x.Division == division && 
                x.Factory == factory && 
                x.Language_Code.ToLower() == lang.ToLower()
            , true);

            var data = await department
                 .GroupJoin(departmentLanguage,
                     x => x.Department_Code,
                     y => y.Department_Code,
                     (x, y) => new { HOD = x, HODL = y })
                 .SelectMany(x => x.HODL.DefaultIfEmpty(),
                     (x, y) => new { x.HOD, HODL = y })
                 .Select(x => new KeyValuePair<string, string>(
                         x.HOD.Department_Code,
                         $"{x.HOD.Department_Code} - {(x.HODL != null ? x.HODL.Name : x.HOD.Department_Name)}"
                     ))
                 .Distinct()
                 .ToListAsync();
            return data;
        }

        public Task<List<KeyValuePair<string, string>>> GetListSalaryType(string lang)
        {
            throw new NotImplementedException();
        }

        [System.Text.RegularExpressions.GeneratedRegex("\\d+")]
        private static partial System.Text.RegularExpressions.Regex MyRegex();
    }
}