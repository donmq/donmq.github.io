using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Helper.Utilities;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance
{
    public class S_5_2_1_NewResignedEmployeeDataPrinting : BaseServices, I_5_2_1_NewResignedEmployeeDataPrinting
    {
        public S_5_2_1_NewResignedEmployeeDataPrinting(DBContext dbContext) : base(dbContext)
        {
        }

        #region GetList
        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName)
        {
            var factoryList = await Queryt_Factory_AddList(userName);

            var query = _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory && factoryList.Contains(x.Code), true);

            var data = await query
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language
                    .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory && x.Language_Code.ToLower() == language.ToLower(), true),
                    code => code.Code,
                    language => language.Code,
                    (code, language) => new { Code = code, Language = language })
                .SelectMany(
                    x => x.Language.DefaultIfEmpty(),
                    (x, language) => new { x.Code, Language = language })
                .Select(x => new KeyValuePair<string, string>(x.Code.Code, $"{x.Code.Code} - {x.Language.Code_Name ?? x.Code.Code_Name}"))
                .Distinct()
                .ToListAsync();

            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListDepartment(string language, string factory)
        {
            return await _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Factory == factory, true)
                .Join(
                    _repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(b => b.Kind == "1" && b.Factory == factory, true),
                    department => department.Division,
                    factoryComparison => factoryComparison.Division,
                    (department, factoryComparison) => department
                )
                .GroupJoin(
                    _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    department => new { department.Division, department.Factory, department.Department_Code },
                    language => new { language.Division, language.Factory, language.Department_Code },
                    (department, language) => new { Department = department, Language = language }
                )
                .SelectMany(
                    x => x.Language.DefaultIfEmpty(),
                    (x, language) => new { x.Department, Language = language }
                )
                .OrderBy(x => x.Department.Department_Code)
                .Select(
                    x => new KeyValuePair<string, string>(
                        x.Department.Department_Code,
                        $"{x.Department.Department_Code} - {(x.Language != null ? x.Language.Name : x.Department.Department_Name)}"
                    )
                )
                .ToListAsync();
        }
        #endregion

        #region GetData-Total-Excel
        public async Task<List<NewResignedEmployeeDataPrintingDto>> GetData(NewResignedEmployeeDataPrintingParam param)
        {
            var pred = PredicateBuilder.New<HRMS_Emp_Personal>(x => x.Factory == param.Factory || x.Assigned_Factory == param.Factory);

            if (!string.IsNullOrWhiteSpace(param.Department))
                pred.And(x => x.Department == param.Department || x.Assigned_Department == param.Department);

            if (param.Kind == "NewHired")
                pred.And(x => x.Onboard_Date >= param.Date_From.ToDateTime()
                           && x.Onboard_Date <= param.Date_To.ToDateTime());

            if (param.Kind == "Resigned")
                pred.And(x => x.Resign_Date != null
                           && x.Resign_Date >= param.Date_From.ToDateTime()
                           && x.Resign_Date <= param.Date_To.ToDateTime());

            var personalQuery = _repositoryAccessor.HRMS_Emp_Personal.FindAll(pred, true).ToList();
            var departmentQuery = _repositoryAccessor.HRMS_Org_Department.FindAll(true);
            var departmentLanguage = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower(), true);
            var contractQuery = _repositoryAccessor.HRMS_Emp_Contract_Management.FindAll(true);
            var insEmpQuery = _repositoryAccessor.HRMS_Ins_Emp_Maintain.FindAll(x => x.Factory == param.Factory, true).ToList();


            var basicCode = _repositoryAccessor.HRMS_Basic_Code.FindAll(true);
            var basicLanguage = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower(), true);
            var codeLang = await basicCode
                .GroupJoin(basicLanguage,
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
                })
                .Distinct().ToListAsync();

            var maxContractDates = contractQuery
                .GroupBy(x => new { x.Division, x.Factory, x.Employee_ID })
                .Select(y => new
                {
                    y.Key.Division,
                    y.Key.Factory,
                    y.Key.Employee_ID,
                    MaxContractStart = y.Max(c => c.Contract_Start)
                }).ToList();

            var result = personalQuery
                .GroupJoin(departmentQuery,
                    personal => personal.Employment_Status == "A" || personal.Employment_Status == "S" ?
                        new { Division = personal.Assigned_Division, Factory = personal.Assigned_Factory, Department = personal.Assigned_Department } :
                        new { personal.Division, personal.Factory, personal.Department },
                    department => new { department.Division, department.Factory, Department = department.Department_Code },
                    (personal, department) => new { Personal = personal, Department = department })
                .SelectMany(x => x.Department.DefaultIfEmpty(),
                    (x, department) => new { x.Personal, Department = department })
                .GroupJoin(
                    departmentLanguage,
                    x => new { x.Department?.Division, x.Department?.Factory, x.Department?.Department_Code },
                    lang => new { lang.Division, lang.Factory, lang.Department_Code },
                    (x, lang) => new { x.Personal, x.Department, LanguageDepartment = lang }
                )
                .SelectMany(
                    x => x.LanguageDepartment.DefaultIfEmpty(),
                    (x, lang) => new { x.Personal, x.Department, LanguageDepartment = lang }
                )
                .GroupJoin(
                    maxContractDates,
                    x => new { x.Personal.Division, x.Personal.Factory, x.Personal.Employee_ID },
                    y => new { y.Division, y.Factory, y.Employee_ID },
                    (x, maxContract) => new { x.Personal, x.Department, x.LanguageDepartment, MaxContract = maxContract.FirstOrDefault() }
                )
                .Select(x => new NewResignedEmployeeDataPrintingDto
                {
                    Department = x.Department?.Department_Code,
                    Department_Name = x.LanguageDepartment?.Name ?? x.Department?.Department_Name,
                    Employee_ID = x.Personal.Employee_ID,
                    Local_Full_Name = x.Personal.Local_Full_Name,
                    Position_Title = CheckValue(x.Personal.Position_Title, codeLang.FirstOrDefault(y => y.Code == x.Personal.Position_Title && y.Type_Seq == BasicCodeTypeConstant.JobTitle)?.Code_Name ?? string.Empty),
                    Onboard_Date = x.Personal?.Onboard_Date,
                    Resign_Date = x.Personal?.Resign_Date,
                    Resign_Reason = CheckValue(x.Personal.Resign_Reason, codeLang.FirstOrDefault(y => y.Code == x.Personal.Resign_Reason && y.Type_Seq == BasicCodeTypeConstant.ReasonResignation)?.Code_Name ?? string.Empty),
                    Education = CheckValue(x.Personal.Education, codeLang.FirstOrDefault(y => y.Code == x.Personal.Education && y.Type_Seq == BasicCodeTypeConstant.Education)?.Code_Name ?? string.Empty),
                    Registered_Province_Directly = CheckValue(x.Personal.Registered_Province_Directly, codeLang.FirstOrDefault(y => y.Code == x.Personal.Registered_Province_Directly && y.Type_Seq == BasicCodeTypeConstant.Province)?.Code_Name
                                            ?? string.Empty),
                    Registered_City = CheckValue(x.Personal.Registered_City, codeLang.FirstOrDefault(y => y.Code == x.Personal.Registered_City && y.Type_Seq == BasicCodeTypeConstant.City)?.Code_Name
                                            ?? string.Empty),
                    Registered_Address = x.Personal.Registered_Address,
                    Birthday = x.Personal?.Birthday,
                    Transportation_Method = CheckValue(x.Personal.Transportation_Method, codeLang.FirstOrDefault(y => y.Code == x.Personal.Transportation_Method && y.Type_Seq == BasicCodeTypeConstant.TransportationMethod)?.Code_Name ?? string.Empty),
                    Work_Type = CheckValue(x.Personal.Work_Type, codeLang.FirstOrDefault(y => y.Code == x.Personal.Work_Type && y.Type_Seq == BasicCodeTypeConstant.WorkType)?.Code_Name ?? string.Empty),
                    Phone_Number = x.Personal.Phone_Number,
                    Mobile_Phone_Number = x.Personal.Mobile_Phone_Number,
                    Gender = x.Personal.Gender == "F"
                            ? (param.Lang == "en" ? "F.Female" : "F.女")
                            : x.Personal.Gender == "M"
                            ? (param.Lang == "en" ? "M.Male" : "M.男")
                            : x.Personal.Gender,
                    Contract_Date = x.MaxContract?.MaxContractStart,
                    Insurance_Date = insEmpQuery.Where(ins => ins.Employee_ID == x.Personal.Employee_ID && ins.Insurance_Type == "V02").Max(ins => ins?.Insurance_Start),
                    Seniority = Math.Round((DateTime.Now - x.Personal.Onboard_Date).TotalDays / 365, 1)
                })
                .OrderBy(x => x.Department).ThenBy(x => x.Employee_ID).ToList();

            var groupedResult = result.GroupBy(x => new { x.Department, x.Employee_ID }).Select(x => x.First()).ToList();

            return groupedResult;
        }



        public async Task<int> GetTotal(NewResignedEmployeeDataPrintingParam param)
        {
            var data = await GetData(param);
            return data.Count;
        }

        public async Task<OperationResult> DownloadExcel(NewResignedEmployeeDataPrintingParam param, string userName)
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
                new Cell("A1", param.Lang == "en" ? "5.2.1 New/Resigned Employee Data Printing" : "5.2.1 新進/離職員工資料列印"),
                new Cell("A2", param.Lang == "en" ? "Factory" : "廠別"),
                new Cell("A4", param.Lang == "en" ? "Print By" : "列印人員"),
                new Cell("B2", param.Factory),
                new Cell("B4", userName),
                new Cell("C4", param.Lang == "en" ? "Print Date" : "列印日期"),
                new Cell("D2", param.Lang == "en" ? "Kind" : "類別"),
                new Cell("D4", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")),
                new Cell("E2", param.Kind == "NewHired"
                            ? (param.Lang == "en" ? "New-hired" : "新進")
                            : (param.Lang == "en" ? "Resigned" : "離職")),
                new Cell("G2", param.Lang == "en" ? "Date" : "資料日期"),
                new Cell("H2", $"{param.Date_From} ~ {param.Date_To}"),
            };
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                tables, cells, 
                "Resources\\Template\\AttendanceMaintenance\\5_2_1_NewResignedEmployeeDataPrinting\\Download.xlsx"
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }
        #endregion
    }
}