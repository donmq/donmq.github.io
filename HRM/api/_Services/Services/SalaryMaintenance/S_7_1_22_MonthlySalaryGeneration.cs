using System.Collections;
using API.Data;
using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.SalaryMaintenance
{
    public class S_7_1_22_MonthlySalaryGeneration : BaseServices, I_7_1_22_MonthlySalaryGeneration
    {
        private static readonly SemaphoreSlim semaphore = new(1, 1);
        public S_7_1_22_MonthlySalaryGeneration(DBContext dbContext) : base(dbContext)
        {
        }

        #region Get List
        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string userName, string language)
        {
            var factories = await Queryt_Factory_AddList(userName);
            var factoriesWithLanguage = await _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory && factories.Contains(x.Code), true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (HBC, HBCL) => new { HBC, HBCL })
                .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (x, y) => new { x.HBC, HBCL = y })
                .Select(x => new KeyValuePair<string, string>(x.HBC.Code, $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}")).ToListAsync();
            return factoriesWithLanguage;
        }
        public async Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language)
        {
            var permissionGroups = await Query_Permission_List(factory);
            var permissionGroupsWithLanguage = await _repositoryAccessor.HRMS_Basic_Code
                            .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.PermissionGroup && permissionGroups.Select(x => x.Permission_Group).Contains(x.Code), true)
                            .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                                x => new { x.Type_Seq, x.Code },
                                y => new { y.Type_Seq, y.Code },
                                (HBC, HBCL) => new { HBC, HBCL })
                            .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                                (x, y) => new { x.HBC, HBCL = y })
                            .Select(x => new KeyValuePair<string, string>(x.HBC.Code, $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}")).ToListAsync();
            return permissionGroupsWithLanguage;
        }
        #endregion

        #region Monthly Salary Generation
        public async Task<OperationResult> CheckData(MonthlySalaryGenerationParam param)
        {
            var year_Month = DateTime.Parse(param.Year_Month);
            var reuslt = await Query_SalaryClose_Status("Y", param.Factory, year_Month, param.Permission_Group, param.Employee_ID);
            return reuslt;
        }

        public async Task<OperationResult> MonthlySalaryGenerationExecute(MonthlySalaryGenerationParam param)
        {
            List<Temp> temps = new();
            List<PTemp> pTemps = new();
            List<HRMS_Sal_Monthly> addDataHSM = new();
            List<HRMS_Sal_Tax> addDataHST = new();
            List<HRMS_Sal_Monthly_Detail> addDataHSMD = new();
            List<HRMS_Sal_Probation_Monthly_Detail> addDataHSPMD = new();
            List<HRMS_Sal_Probation_Monthly> addDataHSPM = new();
            var year_Month = DateTime.Parse(param.Year_Month);

            await semaphore.WaitAsync();
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                // Xoá dữ liệu trước đó
                if (param.Is_Delete)
                {
                    var delete = await DeleteData(param);
                    if (!delete.IsSuccess)
                    {
                        await _repositoryAccessor.RollbackAsync();
                        return new OperationResult(false, delete.Error);
                    }
                }

                // 2.1 Ngày trả lương
                var salary_Day = await _repositoryAccessor.HRMS_Att_Monthly
                .FindAll(x => x.Factory == param.Factory
                        && x.Att_Month == year_Month
                        && param.Permission_Group.Contains(x.Permission_Group))
                .AnyAsync()
                ? await _repositoryAccessor.HRMS_Att_Monthly
                    .FindAll(x => x.Factory == param.Factory
                            && x.Att_Month == year_Month
                            && param.Permission_Group.Contains(x.Permission_Group))
                    .MaxAsync(x => x.Salary_Days)
                : 0;

                // 2.2
                var first_Date = new DateTime(year_Month.Year, year_Month.Month, 1);
                var last_Date = year_Month.AddMonths(1).AddDays(-1);
                var previous_Month = year_Month.AddMonths(-1);
                var cnt = 0;

                var Employee_Resigned = _repositoryAccessor.HRMS_Sal_Resign_Monthly
                    .FindAll(x => x.Factory == param.Factory
                               && x.Sal_Month == year_Month)
                    .Select(x => x.Employee_ID)
                    .ToList();

                var predHEP = PredicateBuilder.New<HRMS_Emp_Personal>(x => x.Factory == param.Factory
                        && param.Permission_Group.Contains(x.Permission_Group)
                        && x.Onboard_Date <= last_Date
                        && (!x.Resign_Date.HasValue || x.Resign_Date > last_Date)
                        && !Employee_Resigned.Contains(x.Employee_ID));

                if (!string.IsNullOrWhiteSpace(param.Employee_ID))
                    predHEP.And(x => x.Employee_ID.Contains(param.Employee_ID));

                var cr_pt1 = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(predHEP).OrderBy(x => x.Employee_ID).ToListAsync();
                if (!cr_pt1.Any())
                    return new OperationResult(false, "No data Emp_Personal");

                var HSMBD = _repositoryAccessor.HRMS_Sal_MasterBackup_Detail
                    .FindAll(x => x.Factory == param.Factory
                               && x.Sal_Month == year_Month);

                var HSMB = _repositoryAccessor.HRMS_Sal_MasterBackup
                    .FindAll(x => x.Factory == param.Factory
                               && x.Sal_Month == year_Month);

                var HAMD = _repositoryAccessor.HRMS_Att_Monthly_Detail
                    .FindAll(x => x.Factory == param.Factory
                               && x.Att_Month == year_Month);

                var HAM = _repositoryAccessor.HRMS_Att_Monthly
                    .FindAll(x => x.Factory == param.Factory
                               && x.Att_Month == year_Month);

                var HALM = _repositoryAccessor.HRMS_Att_Leave_Maintain
                    .FindAll(x => x.Factory == param.Factory
                               && x.Leave_Date >= first_Date
                               && x.Leave_Date <= last_Date);

                var HECM = _repositoryAccessor.HRMS_Emp_Contract_Management
                    .FindAll(x => x.Contract_Start > first_Date)
                    .AsEnumerable();

                var HECT = _repositoryAccessor.HRMS_Emp_Contract_Type
                    .FindAll(x => !x.Probationary_Period)
                    .AsEnumerable();

                var HAPM = _repositoryAccessor.HRMS_Att_Probation_Monthly
                    .FindAll(x => x.Factory == param.Factory
                               && x.Att_Month == year_Month);

                var HSBA = _repositoryAccessor.HRMS_Sal_Bank_Account
                    .FindAll(x => x.Factory == param.Factory)
                    .Select(x => x.Employee_ID);

                var HSP = _repositoryAccessor.HRMS_Sal_Parameter
                    .FindAll(x => x.Factory == param.Factory);

                var HIEM = _repositoryAccessor.HRMS_Ins_Emp_Maintain
                    .FindAll(x => x.Factory == param.Factory
                        && x.Insurance_Type == "V01"
                        && (x.Insurance_Start <= first_Date ||
                            (x.Insurance_Start >= first_Date && x.Insurance_Start <= last_Date))
                        && x.Insurance_End == null)
                    .Select(x => x.Employee_ID);

                var HSADIM = _repositoryAccessor.HRMS_Sal_AddDedItem_Monthly
                    .FindAll(x => x.Factory == param.Factory
                               && x.Sal_Month == year_Month);

                var HSAIS = _repositoryAccessor.HRMS_Sal_AddDedItem_Settings
                    .FindAll(x => x.Factory == param.Factory);

                var HSPMD = _repositoryAccessor.HRMS_Sal_Probation_MasterBackup_Detail
                    .FindAll(x => x.Factory == param.Factory
                        && x.Sal_Month == year_Month);

                var HSPM = _repositoryAccessor.HRMS_Sal_Probation_MasterBackup
                    .FindAll(x => x.Factory == param.Factory
                        && x.Sal_Month == year_Month);
                var salary_Leave_Code = HSP.Where(x => x.Seq == "6").Select(x => x.Code).ToList();
                string _err = "";
                foreach (var Emp_Personal in cr_pt1)
                {
                    // 2.3 
                    // (1) Kiểm tra file sao lưu hằng tháng
                    var MasterBackup_Detail = HSMBD.Where(x => x.Employee_ID == Emp_Personal.Employee_ID).ToList();

                    if (MasterBackup_Detail.Count == 0)
                    {
                        _err += string.Format(
                                    "尚未產生7.1.17 月份薪資主檔備份查詢 HRMS_Sal_MasterBackup:\n{0}, {1:yyyy/MM}, {2}\n",
                                    param.Factory, year_Month, Emp_Personal.Employee_ID);
                        continue;
                    }

                    var MasterBackup = HSMB.FirstOrDefault(x => x.Employee_ID == Emp_Personal.Employee_ID);

                    if (MasterBackup is null)
                    {
                        _err += string.Format(
                                    "尚未產生7.1.17 月份薪資主檔備份查詢 HRMS_Sal_MasterBackup:\n{0}, {1:yyyy/MM}, {2}\n",
                                    param.Factory, year_Month, Emp_Personal.Employee_ID);
                        continue;
                    }

                    // (2) Kiểm tra hồ sơ chấm công hằng tháng
                    var Att_Monthly_Detail = HAMD.Where(x => x.Employee_ID == Emp_Personal.Employee_ID).ToList();

                    if (Att_Monthly_Detail.Count == 0)
                    {
                        _err += string.Format(
                                    "尚未產生5.1.22 月份出勤資料維護HRMS_Att_Monthly:\n{0}, {1:yyyy/MM}, {2}\n",
                                    param.Factory, year_Month, Emp_Personal.Employee_ID);
                        continue;
                    }

                    var Att_Monthly = HAM.FirstOrDefault(x => x.Employee_ID == Emp_Personal.Employee_ID);

                    if (Att_Monthly is null)
                    {
                        _err += string.Format(
                                    "尚未產生5.1.22 月份出勤資料維護HRMS_Att_Monthly:\n{0}, {1:yyyy/MM}, {2}\n",
                                    param.Factory, year_Month, Emp_Personal.Employee_ID);
                        continue;
                    }

                    // (3) Xác định có tạo dữ liệu cho số ngày nghỉ phép hay không
                    
                    var salary_Check_Days = HAMD
                        .Where(x => x.Employee_ID == Emp_Personal.Employee_ID && salary_Leave_Code.Contains(x.Leave_Code))?.Sum(x => x.Days) ?? 0;

                    // 2.4
                    var contact_Start = HECM
                        .Where(x => x.Employee_ID == Emp_Personal.Employee_ID)
                        .Join(HECT,
                            A => new { A.Factory, A.Contract_Type },
                            B => new { B.Factory, B.Contract_Type },
                            (A, B) => new { A, B })
                        .Select(x => x.A.Contract_Start);

                    var probation_Flag = "N";
                    var Att_Probation_Monthly = new HRMS_Att_Probation_Monthly();
                    var Probation_MasterBackup_Detail = HSPMD.Where(x => x.Employee_ID == Emp_Personal.Employee_ID).ToList();
                    var Probation_MasterBackup = HSPM.FirstOrDefault(x => x.Employee_ID == Emp_Personal.Employee_ID);

                    if (contact_Start.Any())
                    {
                        DateTime? contract_StartDate = contact_Start.Max();
                        if (contract_StartDate?.ToString("yyyyMM") == year_Month.ToString("yyyyMM") && contract_StartDate > first_Date)
                            probation_Flag = "Y";

                        Att_Probation_Monthly = HAPM.FirstOrDefault(x => x.Employee_ID == Emp_Personal.Employee_ID);
                        if (!Probation_MasterBackup_Detail.Any())
                        {
                            _err += string.Format(
                                    "尚未產生試用期備份 HRMS_Sal_Probation_MasterBackup_Detail:\n{0}, {1:yyyy/MM}, {2}\n",
                                    param.Factory, year_Month, Emp_Personal.Employee_ID);
                            continue;
                        }

                        if (Probation_MasterBackup == null)
                        {
                            _err += string.Format(
                                    "尚未產生試用期備份 HRMS_Sal_Probation_MasterBackup_Detail:\n{0}, {1:yyyy/MM}, {2}\n",
                                    param.Factory, year_Month, Emp_Personal.Employee_ID);
                            continue;
                        }
                    }

                    // 2.5
                    Dictionary<string, VariableCombine> BValue = await Query_Sal_Detail_Variable_Combine(
                        "B", "45", "Bkvalue", "Bkamt", "Bamt",
                        param.Factory, year_Month, Emp_Personal.Employee_ID);
                    Dictionary<string, VariableCombine> Leave = await Query_Att_Monthly_Detail_Variable_Combine(
                        TableSourceType.HRMS_Att_Monthly_Detail, "40", "Leave", "LeaveDays", "Amt",
                        param.Factory, year_Month, Emp_Personal.Employee_ID, "1");
                    Dictionary<string, VariableCombine> Overtime = await Query_Att_Monthly_Detail_Variable_Combine(
                        TableSourceType.HRMS_Att_Monthly_Detail, "42", "Overtime", "OvertimeHours", "Amt",
                        param.Factory, year_Month, Emp_Personal.Employee_ID, "2");

                    Dictionary<string, VariableCombine> PValue = new();
                    Dictionary<string, VariableCombine> FValue = new();
                    Dictionary<string, VariableCombine> PLeave = new();
                    Dictionary<string, VariableCombine> POvertime = new();
                    Dictionary<string, VariableCombine> FOvertime = new();

                    if (probation_Flag == "Y")
                    {
                        PValue = await Query_Sal_Detail_Variable_Combine(
                            "P", "45", "Pkvalue", "Pkamt", "Pamt",
                            param.Factory, year_Month, Emp_Personal.Employee_ID);
                        FValue = await Query_Sal_Detail_Variable_Combine(
                            "B", "45", "Fkvalue", "Fkamt", "Famt",
                            param.Factory, year_Month, Emp_Personal.Employee_ID);
                        PLeave = await Query_Att_Monthly_Detail_Variable_Combine(
                            TableSourceType.HRMS_Att_Probation_Monthly_Detail, "40", "PLeave", "PLeaveDays", "PAmt",
                            param.Factory, year_Month, Emp_Personal.Employee_ID, "1");
                        POvertime = await Query_Att_Monthly_Detail_Variable_Combine(
                            TableSourceType.HRMS_Att_Probation_Monthly_Detail, "42", "POvertime", "POvertimeHours", "PAmt",
                            param.Factory, year_Month, Emp_Personal.Employee_ID, "2");
                        FOvertime = await Query_Att_Monthly_Detail_Variable_Combine(
                            TableSourceType.HRMS_Att_Monthly_Detail, "42", "FOvertime", "FOvertimeHours", "FAmt",
                            param.Factory, year_Month, Emp_Personal.Employee_ID, "2");
                    }

                    // BValue
                    decimal A01_Bkamt_45 = BValue.GetValueOrDefault("A01_Bkamt_45")?.Value as decimal? ?? 0m;
                    decimal A02_Bkamt_45 = BValue.GetValueOrDefault("A02_Bkamt_45")?.Value as decimal? ?? 0m;
                    decimal B01_Bkamt_45 = BValue.GetValueOrDefault("B01_Bkamt_45")?.Value as decimal? ?? 0m;
                    decimal B02_Bkamt_45 = BValue.GetValueOrDefault("B02_Bkamt_45")?.Value as decimal? ?? 0m;
                    decimal B04_Bkamt_45 = BValue.GetValueOrDefault("B04_Bkamt_45")?.Value as decimal? ?? 0m;
                    decimal B05_Bkamt_45 = BValue.GetValueOrDefault("B05_Bkamt_45")?.Value as decimal? ?? 0m;
                    decimal B06_Bkamt_45 = BValue.GetValueOrDefault("B06_Bkamt_45")?.Value as decimal? ?? 0m;
                    decimal B07_Bkamt_45 = BValue.GetValueOrDefault("B07_Bkamt_45")?.Value as decimal? ?? 0m;
                    decimal B09_Bkamt_45 = BValue.GetValueOrDefault("B09_Bkamt_45")?.Value as decimal? ?? 0m;
                    decimal B10_Bkamt_45 = BValue.GetValueOrDefault("B10_Bkamt_45")?.Value as decimal? ?? 0m;
                    decimal C01_Bkamt_45 = BValue.GetValueOrDefault("C01_Bkamt_45")?.Value as decimal? ?? 0m;
                    decimal C02_Bkamt_45 = BValue.GetValueOrDefault("C02_Bkamt_45")?.Value as decimal? ?? 0m;
                    decimal D01_Bkamt_45 = BValue.GetValueOrDefault("D01_Bkamt_45")?.Value as decimal? ?? 0m;
                    decimal AV2_Bkamt_45 = BValue.GetValueOrDefault("AV2_Bkamt_45")?.Value as decimal? ?? 0m;

                    decimal A01_Bamt_45 = BValue.GetValueOrDefault("A01_Bamt_45")?.Value as decimal? ?? 0m;
                    decimal A02_Bamt_45 = BValue.GetValueOrDefault("A02_Bamt_45")?.Value as decimal? ?? 0m;
                    decimal B01_Bamt_45 = BValue.GetValueOrDefault("B01_Bamt_45")?.Value as decimal? ?? 0m;
                    decimal B02_Bamt_45 = BValue.GetValueOrDefault("B02_Bamt_45")?.Value as decimal? ?? 0m;
                    decimal B04_Bamt_45 = BValue.GetValueOrDefault("B04_Bamt_45")?.Value as decimal? ?? 0m;
                    decimal B05_Bamt_45 = BValue.GetValueOrDefault("B05_Bamt_45")?.Value as decimal? ?? 0m;
                    decimal B06_Bamt_45 = BValue.GetValueOrDefault("B06_Bamt_45")?.Value as decimal? ?? 0m;
                    decimal B07_Bamt_45 = BValue.GetValueOrDefault("B07_Bamt_45")?.Value as decimal? ?? 0m;
                    decimal B09_Bamt_45 = BValue.GetValueOrDefault("B09_Bamt_45")?.Value as decimal? ?? 0m;
                    decimal B10_Bamt_45 = BValue.GetValueOrDefault("B10_Bamt_45")?.Value as decimal? ?? 0m;
                    decimal C01_Bamt_45 = BValue.GetValueOrDefault("C01_Bamt_45")?.Value as decimal? ?? 0m;
                    decimal C02_Bamt_45 = BValue.GetValueOrDefault("C02_Bamt_45")?.Value as decimal? ?? 0m;
                    decimal AV2_Bamt_45 = BValue.GetValueOrDefault("AV2_Bamt_45")?.Value as decimal? ?? 0m;

                    // Leave
                    decimal D0_LeaveDays_40 = Leave.GetValueOrDefault("D0_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal H0_LeaveDays_40 = Leave.GetValueOrDefault("H0_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal I0_LeaveDays_40 = Leave.GetValueOrDefault("I0_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal I1_LeaveDays_40 = Leave.GetValueOrDefault("I1_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal K0_LeaveDays_40 = Leave.GetValueOrDefault("K0_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal E0_LeaveDays_40 = Leave.GetValueOrDefault("E0_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal F0_LeaveDays_40 = Leave.GetValueOrDefault("F0_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal J2_LeaveDays_40 = Leave.GetValueOrDefault("J2_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal G1_LeaveDays_40 = Leave.GetValueOrDefault("G1_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal J1_LeaveDays_40 = Leave.GetValueOrDefault("J1_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal J5_LeaveDays_40 = Leave.GetValueOrDefault("J5_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal W0_LeaveDays_40 = Leave.GetValueOrDefault("W0_LeaveDays_40")?.Value as decimal? ?? 0m;

                    decimal C0_LeaveDays_40 = Leave.GetValueOrDefault("C0_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal A0_LeaveDays_40 = Leave.GetValueOrDefault("A0_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal B0_LeaveDays_40 = Leave.GetValueOrDefault("B0_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal G0_LeaveDays_40 = Leave.GetValueOrDefault("G0_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal O0_LeaveDays_40 = Leave.GetValueOrDefault("O0_LeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal J4_LeaveDays_40 = Leave.GetValueOrDefault("J4_LeaveDays_40")?.Value as decimal? ?? 0m;

                    // Overtime
                    decimal A01_OvertimeHours_42 = Overtime.GetValueOrDefault("A01_OvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal A02_OvertimeHours_42 = Overtime.GetValueOrDefault("A02_OvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal A03_OvertimeHours_42 = Overtime.GetValueOrDefault("A03_OvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal A04_OvertimeHours_42 = Overtime.GetValueOrDefault("A04_OvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal B01_OvertimeHours_42 = Overtime.GetValueOrDefault("B01_OvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal B03_OvertimeHours_42 = Overtime.GetValueOrDefault("B03_OvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal B04_OvertimeHours_42 = Overtime.GetValueOrDefault("B04_OvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal C01_OvertimeHours_42 = Overtime.GetValueOrDefault("C01_OvertimeHours_42")?.Value as decimal? ?? 0m;

                    decimal A01_Amt_42 = Overtime.GetValueOrDefault("A01_Amt_42")?.Value as decimal? ?? 0m;
                    decimal A02_Amt_42 = Overtime.GetValueOrDefault("A02_Amt_42")?.Value as decimal? ?? 0m;
                    decimal A03_Amt_42 = Overtime.GetValueOrDefault("A03_Amt_42")?.Value as decimal? ?? 0m;
                    decimal B01_Amt_42 = Overtime.GetValueOrDefault("B01_Amt_42")?.Value as decimal? ?? 0m;
                    decimal B03_Amt_42 = Overtime.GetValueOrDefault("B03_Amt_42")?.Value as decimal? ?? 0m;
                    decimal C01_Amt_42 = Overtime.GetValueOrDefault("C01_Amt_42")?.Value as decimal? ?? 0m;

                    // PValue
                    decimal A01_Pkamt_45 = PValue.GetValueOrDefault("A01_Pkamt_45")?.Value as decimal? ?? 0m;
                    decimal A02_Pkamt_45 = PValue.GetValueOrDefault("A02_Pkamt_45")?.Value as decimal? ?? 0m;
                    decimal B01_Pkamt_45 = PValue.GetValueOrDefault("B01_Pkamt_45")?.Value as decimal? ?? 0m;
                    decimal B02_Pkamt_45 = PValue.GetValueOrDefault("B02_Pkamt_45")?.Value as decimal? ?? 0m;
                    decimal B04_Pkamt_45 = PValue.GetValueOrDefault("B04_Pkamt_45")?.Value as decimal? ?? 0m;
                    decimal B05_Pkamt_45 = PValue.GetValueOrDefault("B05_Pkamt_45")?.Value as decimal? ?? 0m;
                    decimal B06_Pkamt_45 = PValue.GetValueOrDefault("B06_Pkamt_45")?.Value as decimal? ?? 0m;
                    decimal B07_Pkamt_45 = PValue.GetValueOrDefault("B07_Pkamt_45")?.Value as decimal? ?? 0m;
                    decimal B09_Pkamt_45 = PValue.GetValueOrDefault("B09_Pkamt_45")?.Value as decimal? ?? 0m;
                    decimal B10_Pkamt_45 = PValue.GetValueOrDefault("B10_Pkamt_45")?.Value as decimal? ?? 0m;

                    decimal A01_Pamt_45 = PValue.GetValueOrDefault("A01_Pamt_45")?.Value as decimal? ?? 0m;
                    decimal A02_Pamt_45 = PValue.GetValueOrDefault("A02_Pamt_45")?.Value as decimal? ?? 0m;
                    decimal B01_Pamt_45 = PValue.GetValueOrDefault("B01_Pamt_45")?.Value as decimal? ?? 0m;
                    decimal B02_Pamt_45 = PValue.GetValueOrDefault("B02_Pamt_45")?.Value as decimal? ?? 0m;
                    decimal B04_Pamt_45 = PValue.GetValueOrDefault("B04_Pamt_45")?.Value as decimal? ?? 0m;
                    decimal B05_Pamt_45 = PValue.GetValueOrDefault("B05_Pamt_45")?.Value as decimal? ?? 0m;
                    decimal B06_Pamt_45 = PValue.GetValueOrDefault("B06_Pamt_45")?.Value as decimal? ?? 0m;
                    decimal B09_Pamt_45 = PValue.GetValueOrDefault("B09_Pamt_45")?.Value as decimal? ?? 0m;
                    decimal B10_Pamt_45 = PValue.GetValueOrDefault("B10_Pamt_45")?.Value as decimal? ?? 0m;

                    // FValue
                    decimal A01_Famt_45 = FValue.GetValueOrDefault("A01_Famt_45")?.Value as decimal? ?? 0m;
                    decimal A02_Famt_45 = FValue.GetValueOrDefault("A02_Famt_45")?.Value as decimal? ?? 0m;
                    decimal B01_Famt_45 = FValue.GetValueOrDefault("B01_Famt_45")?.Value as decimal? ?? 0m;
                    decimal B02_Famt_45 = FValue.GetValueOrDefault("B02_Famt_45")?.Value as decimal? ?? 0m;
                    decimal B04_Famt_45 = FValue.GetValueOrDefault("B04_Famt_45")?.Value as decimal? ?? 0m;
                    decimal B05_Famt_45 = FValue.GetValueOrDefault("B05_Famt_45")?.Value as decimal? ?? 0m;
                    decimal B06_Famt_45 = FValue.GetValueOrDefault("B06_Famt_45")?.Value as decimal? ?? 0m;
                    decimal B09_Famt_45 = FValue.GetValueOrDefault("B09_Famt_45")?.Value as decimal? ?? 0m;
                    decimal B10_Famt_45 = FValue.GetValueOrDefault("B10_Famt_45")?.Value as decimal? ?? 0m;

                    // PLeave
                    decimal D0_PLeaveDays_40 = PLeave.GetValueOrDefault("D0_PLeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal H0_PLeaveDays_40 = PLeave.GetValueOrDefault("H0_PLeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal I0_PLeaveDays_40 = PLeave.GetValueOrDefault("I0_PLeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal I1_PLeaveDays_40 = PLeave.GetValueOrDefault("I1_PLeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal K0_PLeaveDays_40 = PLeave.GetValueOrDefault("K0_PLeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal E0_PLeaveDays_40 = PLeave.GetValueOrDefault("E0_PLeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal F0_PLeaveDays_40 = PLeave.GetValueOrDefault("F0_PLeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal J2_PLeaveDays_40 = PLeave.GetValueOrDefault("J2_PLeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal G1_PLeaveDays_40 = PLeave.GetValueOrDefault("G1_PLeaveDays_40")?.Value as decimal? ?? 0m;
                    decimal J1_PLeaveDays_40 = PLeave.GetValueOrDefault("J1_PLeaveDays_40")?.Value as decimal? ?? 0m;

                    // POvertime
                    decimal A01_POvertimeHours_42 = POvertime.GetValueOrDefault("A01_POvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal A02_POvertimeHours_42 = POvertime.GetValueOrDefault("A02_POvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal A04_POvertimeHours_42 = POvertime.GetValueOrDefault("A04_POvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal B01_POvertimeHours_42 = POvertime.GetValueOrDefault("B01_POvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal B04_POvertimeHours_42 = POvertime.GetValueOrDefault("B04_POvertimeHours_42")?.Value as decimal? ?? 0m;
                    decimal C01_POvertimeHours_42 = POvertime.GetValueOrDefault("C01_POvertimeHours_42")?.Value as decimal? ?? 0m;

                    decimal A01_PAmt_42 = POvertime.GetValueOrDefault("A01_PAmt_42")?.Value as decimal? ?? 0m;
                    decimal A02_PAmt_42 = POvertime.GetValueOrDefault("A02_PAmt_42")?.Value as decimal? ?? 0m;
                    decimal B01_PAmt_42 = POvertime.GetValueOrDefault("B01_PAmt_42")?.Value as decimal? ?? 0m;
                    decimal C01_PAmt_42 = POvertime.GetValueOrDefault("C01_PAmt_42")?.Value as decimal? ?? 0m;

                    decimal A01_FAmt_42 = FOvertime.GetValueOrDefault("A01_FAmt_42")?.Value as decimal? ?? 0m;
                    decimal A02_FAmt_42 = FOvertime.GetValueOrDefault("A02_FAmt_42")?.Value as decimal? ?? 0m;
                    decimal B01_FAmt_42 = FOvertime.GetValueOrDefault("B01_FAmt_42")?.Value as decimal? ?? 0m;
                    decimal C01_FAmt_42 = FOvertime.GetValueOrDefault("C01_FAmt_42")?.Value as decimal? ?? 0m;

                    // 2.6 Kiểm tra số ngày làm việc thực tế và ngày nghỉ = 0 thì bỏ qua
                    if (Att_Monthly.Actual_Days == 0 && salary_Check_Days == 0)
                        continue;

                    // 2.7
                    // Sal_Monthly
                    var sal_Monthly = new HRMS_Sal_Monthly
                    {
                        USER_GUID = Emp_Personal.USER_GUID,
                        Division = Emp_Personal.Division,
                        Factory = Emp_Personal.Factory,
                        Sal_Month = year_Month,
                        Employee_ID = Emp_Personal.Employee_ID,
                        Department = Att_Monthly.Department,
                        Currency = MasterBackup.Currency,
                        Permission_Group = MasterBackup.Permission_Group,
                        Salary_Type = MasterBackup.Salary_Type,
                        Lock = "N",
                        BankTransfer = "N",
                        Tax = 0,
                        Update_By = param.UserName,
                        Update_Time = DateTime.Now
                    };

                    // 2.7.1
                    // PSal_Monthly
                    var pSal_Monthly = new HRMS_Sal_Probation_Monthly
                    {
                        USER_GUID = Emp_Personal.USER_GUID,
                        Division = Emp_Personal.Division,
                        Factory = Emp_Personal.Factory,
                        Sal_Month = year_Month,
                        Employee_ID = Emp_Personal.Employee_ID,
                        Probation = "Y",
                        Department = Att_Monthly.Department,
                        Currency = Probation_MasterBackup?.Currency ?? "",
                        Permission_Group = Probation_MasterBackup?.Permission_Group ?? "",
                        Salary_Type = Probation_MasterBackup?.Salary_Type ?? "",
                        Lock = "N",
                        BankTransfer = "N",
                        Tax = 0,
                        Update_By = param.UserName,
                        Update_Time = DateTime.Now
                    };

                    // 2.7.2
                    // FSal_Monthly
                    var fSal_Monthly = new HRMS_Sal_Probation_Monthly
                    {
                        USER_GUID = Emp_Personal.USER_GUID,
                        Division = Emp_Personal.Division,
                        Factory = Emp_Personal.Factory,
                        Sal_Month = year_Month,
                        Employee_ID = Emp_Personal.Employee_ID,
                        Probation = "N",
                        Department = Att_Monthly.Department,
                        Currency = MasterBackup.Currency,
                        Permission_Group = MasterBackup.Permission_Group,
                        Salary_Type = MasterBackup.Salary_Type,
                        Lock = "N",
                        BankTransfer = "N",
                        Tax = 0,
                        Update_By = param.UserName,
                        Update_Time = DateTime.Now
                    };

                    // 2.8 Kiểm tra có tài khoản ngân hàng hay không
                    var has_Bank_Account = HSBA.Contains(Emp_Personal.Employee_ID);

                    if (has_Bank_Account)
                        sal_Monthly.BankTransfer = "Y";

                    // 2.9
                    var paid_Days = Att_Monthly.Actual_Days + D0_LeaveDays_40 + H0_LeaveDays_40 + I0_LeaveDays_40 + I1_LeaveDays_40
                        + K0_LeaveDays_40 + E0_LeaveDays_40 + F0_LeaveDays_40 + J2_LeaveDays_40 + G1_LeaveDays_40 + J1_LeaveDays_40;

                    // 2.10 Paid_Days(P)
                    var paid_Days_P = 0m;
                    if (probation_Flag == "Y")
                    {
                        paid_Days_P = Att_Probation_Monthly?.Actual_Days ?? 0 + D0_PLeaveDays_40 + H0_PLeaveDays_40 + I0_PLeaveDays_40 + I1_PLeaveDays_40
                                  + K0_PLeaveDays_40 + E0_PLeaveDays_40 + F0_PLeaveDays_40 + J2_PLeaveDays_40 + G1_PLeaveDays_40 + J1_PLeaveDays_40;
                    }

                    // 2.13 
                    string new_Employee = "N";

                    if (Emp_Personal.Onboard_Date > first_Date && Emp_Personal.Onboard_Date <= last_Date)
                    {
                        new_Employee = "Y";
                        A02_Bamt_45 = 0;
                    }

                    // 2.14
                    if (probation_Flag == "Y")
                    {
                        // 2.14.1 Lương thử việc
                        // 2.14.2 A01 Lương cơ bản
                        A01_Pamt_45 = (int)Math.Round(A01_Pkamt_45 / salary_Day * paid_Days_P);
                        // 2.14.3 A02 Lương cơ bản theo thâm niên
                        A02_Pamt_45 = (int)Math.Round(A02_Pkamt_45 / salary_Day * paid_Days_P);
                        // 2.14.4 B01 Phụ cấp giám sát
                        B01_Pamt_45 = (int)Math.Round(B01_Pkamt_45 / salary_Day * paid_Days_P);
                        // 2.14.5 B02 Phụ cấp chuyên môn
                        B02_Pamt_45 = (int)Math.Round(B02_Pkamt_45 / salary_Day * paid_Days_P);
                        // 2.14.6 B04 Phụ cấp kỹ thuật
                        B04_Pamt_45 = (int)Math.Round(B04_Pkamt_45 / salary_Day * paid_Days_P);
                        // 2.14.7 B05 Phụ cấp kỹ thuật đặc biệt
                        B05_Pamt_45 = (int)Math.Round(B05_Pkamt_45 / salary_Day * paid_Days_P);
                        // 2.14.8 B06 Phụ cấp đặc biệt
                        B06_Pamt_45 = (int)Math.Round(B06_Pkamt_45 / salary_Day * paid_Days_P);
                        // 2.14.9 B07 Phụ cấp công tác

                        // 2.14.10 B09 Phụ cấp QLLTSX
                        B09_Pamt_45 = (int)Math.Round(B09_Pkamt_45 / salary_Day * paid_Days_P);
                        // 2.14.11 B10 Phụ cấp TDSX
                        B10_Pamt_45 = (int)Math.Round(B10_Pkamt_45 / salary_Day * paid_Days_P);

                        // 2.14.12 Lương đã ký hợp đồng chính thức
                        // 2.14.13 A01 Lương cơ bản
                        A01_Famt_45 = (int)Math.Round(A01_Bkamt_45 / salary_Day * (paid_Days - paid_Days_P));
                        A01_Bamt_45 = A01_Famt_45 + A01_Pamt_45;
                        // 2.14.14 A02 Lương cơ bản theo thâm niên
                        A02_Famt_45 = (int)Math.Round(A02_Bkamt_45 / salary_Day * (paid_Days - paid_Days_P));
                        A02_Bamt_45 = A02_Famt_45 + A02_Pamt_45;
                        // 2.14.15 B01 Phụ cấp giám sát
                        B01_Famt_45 = (int)Math.Round(B01_Bkamt_45 / salary_Day * (paid_Days - paid_Days_P));
                        B01_Bamt_45 = B01_Famt_45 + B01_Pamt_45;
                        // 2.14.16 B02 Phụ cấp chuyên môn
                        B02_Famt_45 = (int)Math.Round(B02_Bkamt_45 / salary_Day * (paid_Days - paid_Days_P));
                        B02_Bamt_45 = B02_Famt_45 + B02_Pamt_45;
                        // 2.14.17 B04 Phụ cấp kỹ thuật
                        B04_Famt_45 = (int)Math.Round(B04_Bkamt_45 / salary_Day * (paid_Days - paid_Days_P));
                        B04_Bamt_45 = B04_Famt_45 + B04_Pamt_45;
                        // 2.14.18 B05 Phụ cấp kỹ thuật đặc biệt
                        B05_Famt_45 = (int)Math.Round(B05_Bkamt_45 / salary_Day * (paid_Days - paid_Days_P));
                        B05_Bamt_45 = B05_Famt_45 + B05_Pamt_45;
                        // 2.14.19 B06 Phụ cấp đặc biệt
                        B06_Famt_45 = (int)Math.Round(B06_Bkamt_45 / salary_Day * (paid_Days - paid_Days_P));
                        B06_Bamt_45 = B06_Famt_45 + B06_Pamt_45;
                        // 2.14.20 B07 Phụ cấp công tác

                        // 2.14.21 B09 Phụ cấp QLLTSX
                        B09_Famt_45 = (int)Math.Round(B09_Bkamt_45 / salary_Day * (paid_Days - paid_Days_P));
                        B09_Bamt_45 = B09_Famt_45 + B09_Pamt_45;
                        // 2.14.22 B10 Phụ cấp TDSX
                        B10_Famt_45 = (int)Math.Round(B10_Bkamt_45 / salary_Day * (paid_Days - paid_Days_P));
                        B10_Bamt_45 = B10_Famt_45 + B10_Pamt_45;
                        // 2.14.23 C01 Tiền làm thêm giờ
                        C01_Bamt_45 = (int)Math.Round(C01_Bkamt_45 / salary_Day * paid_Days);
                    }
                    else // 2.15
                    {
                        // 2.15.1 A01 Lương cơ bản
                        A01_Bamt_45 = (int)Math.Round(A01_Bkamt_45 / salary_Day * paid_Days);
                        // 2.15.2 A01 Lương cơ bản theo thâm niên
                        A02_Bamt_45 = (int)Math.Round(A02_Bkamt_45 / salary_Day * paid_Days);
                        // 2.15.3 B01 Phụ cấp giám sát
                        B01_Bamt_45 = (int)Math.Round(B01_Bkamt_45 / salary_Day * paid_Days);
                        // 2.15.4 B02 Phụ cấp chuyên môn
                        B02_Bamt_45 = (int)Math.Round(B02_Bkamt_45 / salary_Day * paid_Days);
                        // 2.15.5 B04 Phụ cấp kỹ thuật
                        B04_Bamt_45 = (int)Math.Round(B04_Bkamt_45 / salary_Day * paid_Days);
                        // 2.15.6 B05 Phụ cấp kỹ thuật đặc biệt
                        B05_Bamt_45 = (int)Math.Round(B05_Bkamt_45 / salary_Day * paid_Days);
                        // 2.15.7 B06 Phụ cấp đặc biệt
                        B06_Bamt_45 = (int)Math.Round(B06_Bkamt_45 / salary_Day * paid_Days);
                        // 2.15.8 
                        // 2.15.9 B09 Phụ cấp QLLTSX
                        B09_Bamt_45 = (int)Math.Round(B09_Bkamt_45 / salary_Day * paid_Days);
                        // 2.15.10 B10 Phụ cấp TDSX
                        B10_Bamt_45 = (int)Math.Round(B10_Bkamt_45 / salary_Day * paid_Days);
                        // 2.15.11 C01 Tiền làm thêm giờ
                        C01_Bamt_45 = (int)Math.Round(C01_Bkamt_45 / salary_Day * paid_Days);
                    }

                    // 2.16 AV2 Tạm dừng để đàm phán
                    AV2_Bamt_45 = (int)Math.Round(AV2_Bkamt_45 / salary_Day * J5_LeaveDays_40);

                    // 2.17 B07 Phụ cấp công tác
                    B07_Bamt_45 = (int)Math.Round(B07_Bkamt_45 / salary_Day * paid_Days);

                    // 2.18 C02 Trợ cấp sinh hoạt
                    C02_Bamt_45 = (int)Math.Round(C02_Bkamt_45 / salary_Day * (paid_Days + W0_LeaveDays_40));

                    // 2.19 D01 Chuyên cần
                    Dictionary<string, decimal> LeaveDays_40 = new()
                    {
                        ["C0_LeaveDays_40"] = C0_LeaveDays_40,
                        ["A0_LeaveDays_40"] = A0_LeaveDays_40,
                        ["B0_LeaveDays_40"] = B0_LeaveDays_40,
                        ["H0_LeaveDays_40"] = H0_LeaveDays_40,
                        ["G0_LeaveDays_40"] = G0_LeaveDays_40,
                        ["O0_LeaveDays_40"] = O0_LeaveDays_40,
                        ["J4_LeaveDays_40"] = J4_LeaveDays_40
                    };

                    D01_Bkamt_45 = Sal_Perfect_Att_Bonus(HSP, D01_Bkamt_45, first_Date, last_Date, Emp_Personal, Att_Monthly, LeaveDays_40);

                    // 2.20

                    var basic_Amt = await Query_WageStandard_Sum("B", param.Factory, year_Month, Emp_Personal.Employee_ID, MasterBackup.Permission_Group, MasterBackup.Salary_Type);

                    // 2.21
                    var oneday_Hour = 8;

                    if (probation_Flag == "Y")
                    {
                        var basic_PKAmt = Math.Round(await Query_WageStandard_Sum("P", param.Factory, year_Month, Emp_Personal.Employee_ID, Probation_MasterBackup.Permission_Group, Probation_MasterBackup.Salary_Type));

                        // 2.21.1
                        var Sal_Parameter_A01 = Query_Sal_Parameter(HSP, param.Factory, "1", "A01");

                        A01_PAmt_42 = (int)Math.Round(basic_PKAmt / salary_Day / oneday_Hour * Sal_Parameter_A01.Code_Amt
                                    * (A01_POvertimeHours_42 + A04_POvertimeHours_42));

                        A01_FAmt_42 = (int)Math.Round(basic_Amt / salary_Day / oneday_Hour * Sal_Parameter_A01.Code_Amt
                                    * ((A01_OvertimeHours_42 + A04_OvertimeHours_42) - (A01_POvertimeHours_42 + A04_POvertimeHours_42)));

                        A01_Amt_42 = A01_FAmt_42 + A01_PAmt_42;

                        // 2.21.2
                        var Sal_Parameter_A02 = Query_Sal_Parameter(HSP, param.Factory, "1", "A02");

                        A02_PAmt_42 = (int)Math.Round(basic_PKAmt / salary_Day / oneday_Hour * Sal_Parameter_A02.Code_Amt * A02_POvertimeHours_42);

                        A02_FAmt_42 = (int)Math.Round(basic_Amt / salary_Day / oneday_Hour * Sal_Parameter_A02.Code_Amt
                                                * (A02_OvertimeHours_42 - A02_POvertimeHours_42));

                        A02_Amt_42 = A02_FAmt_42 + A02_PAmt_42;

                        // 2.21.3
                        var Sal_Parameter_B01 = Query_Sal_Parameter(HSP, param.Factory, "1", "B01");

                        B01_PAmt_42 = (int)Math.Round(basic_PKAmt / salary_Day / oneday_Hour * Sal_Parameter_B01.Code_Amt
                                                 * (B01_POvertimeHours_42 + B04_POvertimeHours_42));

                        B01_FAmt_42 = (int)Math.Round(basic_Amt / salary_Day / oneday_Hour * Sal_Parameter_B01.Code_Amt
                                                 * ((B01_OvertimeHours_42 + B04_OvertimeHours_42) - (B01_POvertimeHours_42 + B04_POvertimeHours_42)));

                        B01_Amt_42 = B01_FAmt_42 + B01_PAmt_42;

                        // 2.21.4
                        var Sal_Parameter_C01 = Query_Sal_Parameter(HSP, param.Factory, "1", "C01");

                        C01_PAmt_42 = (int)Math.Round(basic_PKAmt / salary_Day / oneday_Hour * Sal_Parameter_C01.Code_Amt * C01_POvertimeHours_42);

                        C01_FAmt_42 = (int)Math.Round(basic_Amt / salary_Day / oneday_Hour * Sal_Parameter_C01.Code_Amt
                                                * (C01_OvertimeHours_42 - C01_POvertimeHours_42));

                        C01_Amt_42 = C01_FAmt_42 + C01_PAmt_42;
                    }
                    else // 2.22 
                    {
                        // 2.22.1
                        var Sal_Parameter_A01 = Query_Sal_Parameter(HSP, param.Factory, "1", "A01");

                        A01_Amt_42 = (int)Math.Round(basic_Amt / salary_Day / oneday_Hour * Sal_Parameter_A01.Code_Amt
                                   * (A01_OvertimeHours_42 + A04_OvertimeHours_42));

                        // 2.22.2
                        var Sal_Parameter_A02 = Query_Sal_Parameter(HSP, param.Factory, "1", "A02");

                        A02_Amt_42 = (int)Math.Round(basic_Amt / salary_Day / oneday_Hour * Sal_Parameter_A02.Code_Amt
                                   * A02_OvertimeHours_42);

                        // 2.22.3
                        var Sal_Parameter_B01 = Query_Sal_Parameter(HSP, param.Factory, "1", "B01");

                        B01_Amt_42 = (int)Math.Round(basic_Amt / salary_Day / oneday_Hour * Sal_Parameter_B01.Code_Amt
                                   * (B01_OvertimeHours_42 + B04_OvertimeHours_42));

                        // 2.22.4
                        var Sal_Parameter_C01 = Query_Sal_Parameter(HSP, param.Factory, "1", "C01");

                        C01_Amt_42 = (int)Math.Round(basic_Amt / salary_Day / oneday_Hour * Sal_Parameter_C01.Code_Amt
                                   * C01_OvertimeHours_42);
                    } // 2.23 End if

                    // 2.24
                    if (C01_Bamt_45 > 0 && (A01_Amt_42 < C01_Bamt_45))
                        A01_Amt_42 = C01_Bamt_45;

                    // 2.25
                    if (new_Employee == "Y" && Emp_Personal.Work_Shift_Type == "00")
                        A01_Amt_42 = (int)Math.Round(C01_Bkamt_45 / salary_Day * paid_Days);

                    // 2.26 Tăng ca đêm bình thường
                    if (A01_OvertimeHours_42 > 0)
                    {
                        var Sal_Parameter_A03 = Query_Sal_Parameter(HSP, param.Factory, "1", "A03");

                        A03_Amt_42 = (int)Math.Round(basic_Amt / salary_Day / oneday_Hour * Sal_Parameter_A03.Code_Amt * A03_OvertimeHours_42);
                    }
                    else
                    {
                        var Sal_Parameter_A03_1 = Query_Sal_Parameter(HSP, param.Factory, "1", "A03-1");

                        A03_Amt_42 = (int)Math.Round(basic_Amt / salary_Day / oneday_Hour * Sal_Parameter_A03_1.Code_Amt * A03_OvertimeHours_42);
                    }

                    // 2.27 B03 Tăng ca đêm ngày lễ
                    var Sal_Parameter_B03 = Query_Sal_Parameter(HSP, param.Factory, "1", "B03");

                    B03_Amt_42 = (int)Math.Round(basic_Amt / salary_Day / oneday_Hour * Sal_Parameter_B03.Code_Amt * B03_OvertimeHours_42);


                    // 2.28 Mục các khoản thêm
                    // 2.29 B54 Phụ cấp ăn ca ngày
                    var amt_B54 = Query_HRMS_Sal_AddDedItem_Values(HSAIS, param.Factory, year_Month, MasterBackup.Permission_Group, MasterBackup.Salary_Type, "B", "B54");
                    var B54_Amt_49 = amt_B54 * (Att_Monthly.DayShift_Food ?? 0);

                    // 2.30 
                    var amt_B55 = Query_HRMS_Sal_AddDedItem_Values(HSAIS, param.Factory, year_Month, MasterBackup.Permission_Group, MasterBackup.Salary_Type, "B", "B55");
                    var B55_Amt_49 = amt_B55 * Att_Monthly.Night_Eat_Times;

                    // 2.31 B56 Phụ cấp ăn tăng ca
                    var amt_B56 = Query_HRMS_Sal_AddDedItem_Values(HSAIS, param.Factory, year_Month, MasterBackup.Permission_Group, MasterBackup.Salary_Type, "B", "B56");
                    var B56_Amt_49 = amt_B56 * Att_Monthly.Food_Expenses;

                    // 2.32 B57 Phụ cấp ăn ca đêm
                    var amt_B57 = Query_HRMS_Sal_AddDedItem_Values(HSAIS, param.Factory, year_Month, MasterBackup.Permission_Group, MasterBackup.Salary_Type, "B", "B57");
                    var B57_Amt_49 = amt_B57 * (Att_Monthly.NightShift_Food ?? 0);

                    // 2.33 Mục các khoản giảm
                    // 2.34
                    var D12_Amt_49 = Query_HRMS_Sal_AddDedItem_Values(HSAIS, param.Factory, year_Month, MasterBackup.Permission_Group, MasterBackup.Salary_Type, "D", "D12");

                    // 2.35 Mục bảo hiểm
                    // 2.36
                    var ins_Emp_Maintain = HIEM.Contains(Emp_Personal.Employee_ID);

                    Dictionary<string, VariableCombine> Insurance = await Query_Ins_Rate_Variable_Combine("57", "Insurance", "EmployerRate", "EmployeeRate", "Amt", param.Factory, year_Month, MasterBackup.Permission_Group);

                    decimal V01_Amt_57 = Insurance.GetValueOrDefault("V01_Amt_57")?.Value as decimal? ?? 0m;
                    decimal V02_Amt_57 = Insurance.GetValueOrDefault("V02_Amt_57")?.Value as decimal? ?? 0m;
                    decimal V03_Amt_57 = Insurance.GetValueOrDefault("V03_Amt_57")?.Value as decimal? ?? 0m;

                    if (ins_Emp_Maintain)
                    {
                        decimal V01_EmployeeRate_57 = Insurance.GetValueOrDefault("V01_EmployeeRate_57")?.Value as decimal? ?? 0m;
                        decimal V02_EmployeeRate_57 = Insurance.GetValueOrDefault("V02_EmployeeRate_57")?.Value as decimal? ?? 0m;
                        decimal V03_EmployeeRate_57 = Insurance.GetValueOrDefault("V03_EmployeeRate_57")?.Value as decimal? ?? 0m;

                        var Sal_Parameter_V01 = Query_Sal_Parameter(HSP, param.Factory, "4", "V01");
                        if (basic_Amt < Sal_Parameter_V01.Code_Amt)
                            V01_Amt_57 = (int)Math.Round(basic_Amt * V01_EmployeeRate_57);
                        else
                            V01_Amt_57 = (int)Math.Round(Sal_Parameter_V01.Code_Amt * V01_EmployeeRate_57);

                        V02_Amt_57 = (int)Math.Round(basic_Amt * V02_EmployeeRate_57);
                        V03_Amt_57 = (int)Math.Round(basic_Amt * V03_EmployeeRate_57);
                    }
                    else
                    {
                        V01_Amt_57 = 0;
                        V02_Amt_57 = 0;
                        V03_Amt_57 = 0;
                    }

                    // 2.37 Thêm dữ liệu vào Temp
                    // 2.38 Lưu trữ mục lương
                    string A01_Bkvalue_45 = BValue.GetValueOrDefault("A01_Bkvalue_45")?.Value as string ?? "";
                    string A02_Bkvalue_45 = BValue.GetValueOrDefault("A02_Bkvalue_45")?.Value as string ?? "";
                    string B01_Bkvalue_45 = BValue.GetValueOrDefault("B01_Bkvalue_45")?.Value as string ?? "";
                    string B02_Bkvalue_45 = BValue.GetValueOrDefault("B02_Bkvalue_45")?.Value as string ?? "";
                    string B04_Bkvalue_45 = BValue.GetValueOrDefault("B04_Bkvalue_45")?.Value as string ?? "";
                    string B05_Bkvalue_45 = BValue.GetValueOrDefault("B05_Bkvalue_45")?.Value as string ?? "";
                    string B06_Bkvalue_45 = BValue.GetValueOrDefault("B06_Bkvalue_45")?.Value as string ?? "";
                    string B07_Bkvalue_45 = BValue.GetValueOrDefault("B07_Bkvalue_45")?.Value as string ?? "";
                    string B09_Bkvalue_45 = BValue.GetValueOrDefault("B09_Bkvalue_45")?.Value as string ?? "";
                    string B10_Bkvalue_45 = BValue.GetValueOrDefault("B10_Bkvalue_45")?.Value as string ?? "";
                    string AV2_Bkvalue_45 = BValue.GetValueOrDefault("AV2_Bkvalue_45")?.Value as string ?? "";
                    string C02_Bkvalue_45 = BValue.GetValueOrDefault("C02_Bkvalue_45")?.Value as string ?? "";
                    string D01_Bkvalue_45 = BValue.GetValueOrDefault("D01_Bkvalue_45")?.Value as string ?? "";
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "45", "A", A01_Bkvalue_45, (int)A01_Bamt_45, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "45", "A", A02_Bkvalue_45, (int)A02_Bamt_45, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "45", "A", B01_Bkvalue_45, (int)B01_Bamt_45, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "45", "A", B02_Bkvalue_45, (int)B02_Bamt_45, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "45", "A", B04_Bkvalue_45, (int)B04_Bamt_45, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "45", "A", B05_Bkvalue_45, (int)B05_Bamt_45, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "45", "A", B06_Bkvalue_45, (int)B06_Bamt_45, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "45", "A", B09_Bkvalue_45, (int)B09_Bamt_45, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "45", "A", B10_Bkvalue_45, (int)B10_Bamt_45, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "45", "A", AV2_Bkvalue_45, (int)AV2_Bamt_45, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "45", "A", B07_Bkvalue_45, (int)B07_Bamt_45, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "45", "A", C02_Bkvalue_45, (int)C02_Bamt_45, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "45", "A", D01_Bkvalue_45, (int)D01_Bkamt_45, sal_Monthly.Currency, sal_Monthly.Department));

                    // 2.38.1
                    // --試用期
                    string A01_Pkvalue_45 = PValue.GetValueOrDefault("A01_Pkvalue_45")?.Value as string ?? "";
                    string A02_Pkvalue_45 = PValue.GetValueOrDefault("A02_Pkvalue_45")?.Value as string ?? "";
                    string B01_Pkvalue_45 = PValue.GetValueOrDefault("B01_Pkvalue_45")?.Value as string ?? "";
                    string B02_Pkvalue_45 = PValue.GetValueOrDefault("B02_Pkvalue_45")?.Value as string ?? "";
                    string B04_Pkvalue_45 = PValue.GetValueOrDefault("B04_Pkvalue_45")?.Value as string ?? "";
                    string B05_Pkvalue_45 = PValue.GetValueOrDefault("B05_Pkvalue_45")?.Value as string ?? "";
                    string B06_Pkvalue_45 = PValue.GetValueOrDefault("B06_Pkvalue_45")?.Value as string ?? "";
                    string B09_Pkvalue_45 = PValue.GetValueOrDefault("B09_Pkvalue_45")?.Value as string ?? "";
                    string B10_Pkvalue_45 = PValue.GetValueOrDefault("B10_Pkvalue_45")?.Value as string ?? "";

                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "Y", "45", "A", A01_Pkvalue_45, (int)A01_Pamt_45));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "Y", "45", "A", A02_Pkvalue_45, (int)A02_Pamt_45));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "Y", "45", "A", B01_Pkvalue_45, (int)B01_Pamt_45));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "Y", "45", "A", B02_Pkvalue_45, (int)B02_Pamt_45));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "Y", "45", "A", B04_Pkvalue_45, (int)B04_Pamt_45));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "Y", "45", "A", B05_Pkvalue_45, (int)B05_Pamt_45));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "Y", "45", "A", B06_Pkvalue_45, (int)B06_Pamt_45));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "Y", "45", "A", B09_Pkvalue_45, (int)B09_Pamt_45));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "Y", "45", "A", B10_Pkvalue_45, (int)B10_Pamt_45));

                    // --轉正
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "N", "45", "A", A01_Bkvalue_45, (int)A01_Famt_45));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "N", "45", "A", A02_Bkvalue_45, (int)A02_Famt_45));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "N", "45", "A", B01_Bkvalue_45, (int)B01_Famt_45));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "N", "45", "A", B02_Bkvalue_45, (int)B02_Famt_45));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "N", "45", "A", B04_Bkvalue_45, (int)B04_Famt_45));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "N", "45", "A", B05_Bkvalue_45, (int)B05_Famt_45));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "N", "45", "A", B06_Bkvalue_45, (int)B06_Famt_45));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "N", "45", "A", B09_Bkvalue_45, (int)B09_Famt_45));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "N", "45", "A", B10_Bkvalue_45, (int)B10_Famt_45));

                    // 2.39 Lưu trữ mục tăng ca
                    string A01_Overtime_42 = Overtime.GetValueOrDefault("A01_Overtime_42")?.Value as string ?? "";
                    string A02_Overtime_42 = Overtime.GetValueOrDefault("A02_Overtime_42")?.Value as string ?? "";
                    string B01_Overtime_42 = Overtime.GetValueOrDefault("B01_Overtime_42")?.Value as string ?? "";
                    string C01_Overtime_42 = Overtime.GetValueOrDefault("C01_Overtime_42")?.Value as string ?? "";
                    string A03_Overtime_42 = Overtime.GetValueOrDefault("A03_Overtime_42")?.Value as string ?? "";
                    string B03_Overtime_42 = Overtime.GetValueOrDefault("B03_Overtime_42")?.Value as string ?? "";
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "42", "A", A01_Overtime_42, (int)A01_Amt_42, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "42", "A", A02_Overtime_42, (int)A02_Amt_42, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "42", "A", B01_Overtime_42, (int)B01_Amt_42, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "42", "A", C01_Overtime_42, (int)C01_Amt_42, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "42", "A", A03_Overtime_42, (int)A03_Amt_42, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "42", "A", B03_Overtime_42, (int)B03_Amt_42, sal_Monthly.Currency, sal_Monthly.Department));

                    // 2.39.1
                    // --試用期
                    string A01_POvertime_42 = POvertime.GetValueOrDefault("A01_POvertime_42")?.Value as string ?? "";
                    string A02_POvertime_42 = POvertime.GetValueOrDefault("A02_POvertime_42")?.Value as string ?? "";
                    string B01_POvertime_42 = POvertime.GetValueOrDefault("B01_POvertime_42")?.Value as string ?? "";
                    string C01_POvertime_42 = POvertime.GetValueOrDefault("C01_POvertime_42")?.Value as string ?? "";

                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "Y", "42", "A", A01_POvertime_42, (int)A01_PAmt_42));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "Y", "42", "A", A02_POvertime_42, (int)A02_PAmt_42));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "Y", "42", "A", B01_POvertime_42, (int)B01_PAmt_42));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "Y", "42", "A", C01_POvertime_42, (int)C01_PAmt_42));

                    // --轉正
                    string A01_FOvertime_42 = FOvertime.GetValueOrDefault("A01_FOvertime_42")?.Value as string ?? "";
                    string A02_FOvertime_42 = FOvertime.GetValueOrDefault("A02_FOvertime_42")?.Value as string ?? "";
                    string B01_FOvertime_42 = FOvertime.GetValueOrDefault("B01_FOvertime_42")?.Value as string ?? "";
                    string C01_FOvertime_42 = FOvertime.GetValueOrDefault("C01_FOvertime_42")?.Value as string ?? "";

                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "N", "42", "A", A01_FOvertime_42, (int)A01_FAmt_42));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "N", "42", "A", A02_FOvertime_42, (int)A02_FAmt_42));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "N", "42", "A", B01_FOvertime_42, (int)B01_FAmt_42));
                    pTemps.Add(new PTemp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "N", "42", "A", C01_FOvertime_42, (int)C01_FAmt_42));

                    // 2.40 Lưu trữ mục các khoản tăng
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "49", "B", "B54", B54_Amt_49, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "49", "B", "B55", B55_Amt_49, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "49", "B", "B56", B56_Amt_49, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "49", "B", "B57", B57_Amt_49, sal_Monthly.Currency, sal_Monthly.Department));

                    // 2.41 Lưu trữ mục các khoản giảm
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "49", "D", "D12", D12_Amt_49, sal_Monthly.Currency, sal_Monthly.Department));

                    // 2.42 Lưu trữ mục bảo hiểm
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "57", "D", "V01", (int)V01_Amt_57, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "57", "D", "V02", (int)V02_Amt_57, sal_Monthly.Currency, sal_Monthly.Department));
                    temps.Add(new Temp(Emp_Personal.USER_GUID, Emp_Personal.Division, Emp_Personal.Factory, year_Month, Emp_Personal.Employee_ID, "57", "D", "V03", (int)V03_Amt_57, sal_Monthly.Currency, sal_Monthly.Department));

                    // 2.43
                    var codeList = HSP.Where(x => x.Seq == "5").Select(x => x.Code).ToList();
                    var addDedItem = HSADIM
                        .Where(x => x.Employee_ID == Emp_Personal.Employee_ID &&
                                    !codeList.Contains(x.AddDed_Item))
                        .Select(x => new Temp()
                        {
                            USER_GUID = x.USER_GUID,
                            Division = Emp_Personal.Division,
                            Factory = Emp_Personal.Factory,
                            Sal_Month = year_Month,
                            Employee_ID = Emp_Personal.Employee_ID,
                            Type_Seq = "49",
                            AddDed_Type = x.AddDed_Type,
                            Item = x.AddDed_Item,
                            Amount = x.Amount,
                        });

                    temps.AddRange(addDedItem);

                    // 2.44 
                    var sal_Tax = Sal_Tax_VN(param.Factory, year_Month, Emp_Personal.Employee_ID, A01_OvertimeHours_42, temps, sal_Monthly);

                    sal_Monthly.Tax = sal_Tax.Tax;

                    sal_Tax.Update_By = param.UserName;
                    sal_Tax.Update_Time = DateTime.Now;

                    // 2.45
                    var sal_Monthly_Detail = temps.Where(x =>
                       x.Factory == param.Factory &&
                       x.Sal_Month.Date == year_Month.Date &&
                       x.Employee_ID == Emp_Personal.Employee_ID &&
                       !string.IsNullOrEmpty(x.Item)
                    ).Select(x => new HRMS_Sal_Monthly_Detail()
                    {
                        USER_GUID = x.USER_GUID,
                        Division = x.Division,
                        Factory = x.Factory,
                        Sal_Month = year_Month,
                        Employee_ID = x.Employee_ID,
                        Type_Seq = x.Type_Seq,
                        AddDed_Type = x.AddDed_Type,
                        Item = x.Item,
                        Amount = x.Amount,
                        Update_By = param.UserName,
                        Update_Time = DateTime.Now
                    });

                    if (probation_Flag == "Y")
                    {
                        // 2.46.1
                        var probation_Monthly_Detail = pTemps.Where(x =>
                           x.Factory == param.Factory &&
                           x.Sal_Month.Date == year_Month.Date &&
                           x.Employee_ID == Emp_Personal.Employee_ID &&
                           !string.IsNullOrEmpty(x.Item)
                        ).Select(x => new HRMS_Sal_Probation_Monthly_Detail()
                        {
                            USER_GUID = x.USER_GUID,
                            Division = x.Division,
                            Factory = x.Factory,
                            Sal_Month = year_Month,
                            Employee_ID = x.Employee_ID,
                            Probation = x.Probation,
                            Type_Seq = x.Type_Seq,
                            AddDed_Type = x.AddDed_Type,
                            Item = x.Item,
                            Amount = x.Amount,
                            Update_By = param.UserName,
                            Update_Time = DateTime.Now
                        });

                        addDataHSPMD.AddRange(probation_Monthly_Detail);

                        // 2.46.2
                        addDataHSPM.Add(pSal_Monthly);
                        addDataHSPM.Add(fSal_Monthly);
                    }

                    addDataHSMD.AddRange(sal_Monthly_Detail);
                    addDataHSM.Add(sal_Monthly);
                    addDataHST.Add(sal_Tax);
                    cnt++;
                }

                if (addDataHSMD.Any() || addDataHSM.Any() || addDataHST.Any())
                {
                    _repositoryAccessor.HRMS_Sal_Monthly_Detail.AddMultiple(addDataHSMD);
                    _repositoryAccessor.HRMS_Sal_Monthly.AddMultiple(addDataHSM);
                    _repositoryAccessor.HRMS_Sal_Tax.AddMultiple(addDataHST);
                    if (addDataHSPM.Any())
                        _repositoryAccessor.HRMS_Sal_Probation_Monthly.AddMultiple(addDataHSPM);
                    if (addDataHSPMD.Any())
                        _repositoryAccessor.HRMS_Sal_Probation_Monthly_Detail.AddMultiple(addDataHSPMD);
                }

                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true, new { Count = cnt, Error = _err });
            }
            catch (Exception ex)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false, ex.InnerException.Message);
            }
            finally
            {
                semaphore.Release();
            }
        }
        #endregion

        #region Monthly Data Lock
        public async Task<OperationResult> MonthlyDataLockExecute(MonthlyDataLockParam param)
        {
            int count = 0;
            var year_Month = DateTime.Parse(param.Year_Month);
            var HSM = _repositoryAccessor.HRMS_Sal_Monthly
                    .FindAll(x => x.Factory == param.Factory
                               && x.Sal_Month == year_Month
                               && param.Permission_Group.Contains(x.Permission_Group))
                    .ToList();

            if (param.Salary_Lock == "Y")
            {
                List<HRMS_Sal_Close> insert_HSC = new();

                var cnt = await _repositoryAccessor.HRMS_Sal_Close
                    .AnyAsync(x => x.Factory == param.Factory
                                && x.Sal_Month == year_Month
                                && param.Permission_Group.Contains(x.Permission_Group)
                                && HSM.Select(x => x.Employee_ID).Contains(x.Employee_ID));

                if (cnt)
                    return new OperationResult(false, "AlreadyLocked");

                foreach (var item in HSM)
                {
                    insert_HSC.Add(new HRMS_Sal_Close
                    {
                        USER_GUID = item.USER_GUID,
                        Division = item.Division,
                        Factory = item.Factory,
                        Sal_Month = item.Sal_Month,
                        Employee_ID = item.Employee_ID,
                        Permission_Group = item.Permission_Group,
                        Close_Status = "N",
                        Close_End = null,
                        Update_By = param.UserName,
                        Update_Time = DateTime.Now
                    });

                    item.Lock = "Y";
                    item.Update_By = param.UserName;
                    item.Update_Time = DateTime.Now;
                }

                await _repositoryAccessor.BeginTransactionAsync();
                try
                {
                    _repositoryAccessor.HRMS_Sal_Close.AddMultiple(insert_HSC);
                    await _repositoryAccessor.Save();

                    count = insert_HSC.Count;
                }
                catch (Exception)
                {
                    await _repositoryAccessor.RollbackAsync();
                    return new OperationResult(false, "LockFailed");
                }

                try
                {
                    _repositoryAccessor.HRMS_Sal_Monthly.UpdateMultiple(HSM);
                    await _repositoryAccessor.Save();
                    await _repositoryAccessor.CommitAsync();
                }
                catch (Exception)
                {
                    await _repositoryAccessor.RollbackAsync();
                    return new OperationResult(false, "ErrorHSM");
                }
            }

            if (param.Salary_Lock == "N")
            {
                var cnt = await _repositoryAccessor.HRMS_Sal_Close
                    .AnyAsync(x => x.Factory == param.Factory
                                && x.Sal_Month == year_Month
                                && param.Permission_Group.Contains(x.Permission_Group)
                                && HSM.Select(x => x.Employee_ID).Contains(x.Employee_ID)
                                && x.Close_Status == "Y");

                if (cnt)
                    return new OperationResult(false, "FNAAlreadyLocked");

                var delete_HSC = _repositoryAccessor.HRMS_Sal_Close
                    .FindAll(x => x.Factory == param.Factory
                                && x.Sal_Month == year_Month
                                && param.Permission_Group.Contains(x.Permission_Group)
                                && HSM.Select(x => x.Employee_ID).Contains(x.Employee_ID))
                    .ToList();

                await _repositoryAccessor.BeginTransactionAsync();
                try
                {
                    _repositoryAccessor.HRMS_Sal_Close.RemoveMultiple(delete_HSC);
                    await _repositoryAccessor.Save();

                    count = delete_HSC.Count;
                }
                catch (Exception)
                {
                    await _repositoryAccessor.RollbackAsync();
                    return new OperationResult(false, "ErrorHSC");
                }

                foreach (var item in HSM)
                {
                    item.Lock = "N";
                    item.Update_By = param.UserName;
                    item.Update_Time = DateTime.Now;
                }

                try
                {
                    _repositoryAccessor.HRMS_Sal_Monthly.UpdateMultiple(HSM);
                    await _repositoryAccessor.Save();
                    await _repositoryAccessor.CommitAsync();
                }
                catch (Exception)
                {
                    await _repositoryAccessor.RollbackAsync();
                    return new OperationResult(false, "ErrorHSM");
                }
            }

            return new OperationResult(true, count);
        }
        #endregion

        private async Task<OperationResult> DeleteData(MonthlySalaryGenerationParam param)
        {
            var year_Month = DateTime.Parse(param.Year_Month);

            var check_Exist_HSMB = await _repositoryAccessor.HRMS_Sal_MasterBackup
                .AnyAsync(x => x.Factory == param.Factory && x.Sal_Month == year_Month);

            if (!check_Exist_HSMB)
                return new OperationResult(false, "7.1.17月份薪資主檔備份查詢 Monthly Salary Master File Backup 需要產生當月資料");

            var HSM = _repositoryAccessor.HRMS_Sal_Monthly
                .FindAll(x => x.Factory == param.Factory
                           && x.Sal_Month == year_Month
                           && param.Permission_Group.Contains(x.Permission_Group)
                           && (string.IsNullOrWhiteSpace(param.Employee_ID) || x.Employee_ID.Contains(param.Employee_ID)))
                .Select(x => x.Employee_ID);

            var HSRM = _repositoryAccessor.HRMS_Sal_Resign_Monthly
                .FindAll(x => x.Factory == param.Factory
                           && x.Sal_Month == year_Month
                           && param.Permission_Group.Contains(x.Permission_Group)
                           && (string.IsNullOrWhiteSpace(param.Employee_ID) || x.Employee_ID.Contains(param.Employee_ID)))
                .Select(x => x.Employee_ID);

            try
            {
                var HST_Delete = await _repositoryAccessor.HRMS_Sal_Tax
                    .FindAll(x => x.Factory == param.Factory
                               && x.Sal_Month == year_Month
                               && (string.IsNullOrWhiteSpace(param.Employee_ID) || x.Employee_ID.Contains(param.Employee_ID))
                               && HSM.Contains(x.Employee_ID)
                               && !HSRM.Contains(x.Employee_ID))
                    .ToListAsync();

                _repositoryAccessor.HRMS_Sal_Tax.RemoveMultiple(HST_Delete);
                await _repositoryAccessor.Save();
            }
            catch (Exception)
            {
                return new OperationResult(false, "刪除員工所得稅HRMS_Sal_Tax 失敗");
            }

            var probation_Count = await _repositoryAccessor.HRMS_Sal_Probation_Monthly
                                .CountAsync(x => x.Factory == param.Factory
                                              && x.Sal_Month == year_Month
                                              && HSM.Contains(x.Employee_ID));
            if (probation_Count > 0)
            {
                try
                {
                    {
                        var HSPMD_Delete = await _repositoryAccessor.HRMS_Sal_Probation_Monthly_Detail
                            .FindAll(x => x.Factory == param.Factory
                                       && x.Sal_Month == year_Month
                                       && HSM.Contains(x.Employee_ID))
                            .ToListAsync();

                        _repositoryAccessor.HRMS_Sal_Probation_Monthly_Detail.RemoveMultiple(HSPMD_Delete);
                        await _repositoryAccessor.Save();
                    }
                }
                catch (Exception)
                {
                    return new OperationResult(false, "刪除月份薪資明細HRMS_Sal_Probation_Monthly_Detail 失敗");
                }

                try
                {
                    {
                        var HSPM_Delete = await _repositoryAccessor.HRMS_Sal_Probation_Monthly
                            .FindAll(x => x.Factory == param.Factory
                                       && x.Sal_Month == year_Month
                                       && HSM.Contains(x.Employee_ID))
                            .ToListAsync();

                        _repositoryAccessor.HRMS_Sal_Probation_Monthly.RemoveMultiple(HSPM_Delete);
                        await _repositoryAccessor.Save();

                    }
                }
                catch (Exception)
                {
                    return new OperationResult(false, "刪除月份薪資明細HRMS_Sal_Probation_Monthly 失敗");
                }
            }

            try
            {
                var HSMD_Delete = await _repositoryAccessor.HRMS_Sal_Monthly_Detail
                    .FindAll(x => x.Factory == param.Factory
                               && x.Sal_Month == year_Month
                               && HSM.Contains(x.Employee_ID))
                    .ToListAsync();

                _repositoryAccessor.HRMS_Sal_Monthly_Detail.RemoveMultiple(HSMD_Delete);
                await _repositoryAccessor.Save();
            }
            catch (Exception)
            {
                return new OperationResult(false, "刪除月份薪資明細HRMS_Sal_Monthly_Detail 失敗");
            }

            try
            {
                var HSM_Delete = await _repositoryAccessor.HRMS_Sal_Monthly
                    .FindAll(x => x.Factory == param.Factory
                               && x.Sal_Month == year_Month
                               && param.Permission_Group.Contains(x.Permission_Group)
                               && (string.IsNullOrWhiteSpace(param.Employee_ID) || x.Employee_ID.Contains(param.Employee_ID)))
                    .ToListAsync();

                _repositoryAccessor.HRMS_Sal_Monthly.RemoveMultiple(HSM_Delete);
                await _repositoryAccessor.Save();
            }
            catch (Exception)
            {
                return new OperationResult(false, "刪除月份薪資主檔HRMS_Sal_Monthly 失敗");
            }

            return new OperationResult(true);
        }

        private async Task<OperationResult> Query_SalaryClose_Status(string Kind, string Factory, DateTime Sal_Month, List<string> Permission_Groups, string Employee_ID)
        {
            var before_Month = Sal_Month.AddMonths(-1);

            // 1. Kiểm tra dữ liệu đã tồn tại trong lương đã đóng hay chưa
            var cnt = await _repositoryAccessor.HRMS_Sal_Close.AnyAsync(x => x.Factory == Factory && x.Sal_Month == before_Month);

            if (!cnt)
                return new OperationResult(false, "前月沒有產生關帳檔,請關帳後再執行");


            if (Kind == "Y")
            {
                // 2. Kiểm tra dữ liệu phòng SEA/Quản trong tháng đó đã khoá chưa
                cnt = await _repositoryAccessor.HRMS_Sal_Monthly.AnyAsync(x => x.Factory == Factory && x.Sal_Month == Sal_Month && x.Lock == "Y");

                if (cnt)
                    return new OperationResult(false, "當月薪資已鎖定資料,不可以再次執行薪資產生作業!");

                // 3. Kiểm tra có lương tháng đó hay chưa
                cnt = await _repositoryAccessor.HRMS_Sal_Monthly
                    .AnyAsync(x => x.Factory == Factory
                                && x.Sal_Month == Sal_Month
                                && Permission_Groups.Contains(x.Permission_Group)
                                && (string.IsNullOrWhiteSpace(Employee_ID) || x.Employee_ID == Employee_ID));

                return new OperationResult(true, cnt ? "DeleteData" : null);
            }
            else
            {
                // 3. Kiểm tra có lương tháng đó hay chưa
                cnt = await _repositoryAccessor.HRMS_Sal_Resign_Monthly
                    .AnyAsync(x => x.Factory == Factory
                                && x.Sal_Month == Sal_Month
                                && Permission_Groups.Contains(x.Permission_Group)
                                && (string.IsNullOrWhiteSpace(Employee_ID) || x.Employee_ID == Employee_ID));

                return new OperationResult(true, cnt ? "DeleteData" : null);
            }
        }

        private decimal Sal_Perfect_Att_Bonus(IEnumerable<HRMS_Sal_Parameter> Sal_Parameter, decimal D01_Bkamt_45, DateTime First_Date, DateTime Last_Date, HRMS_Emp_Personal Emp_Personal, HRMS_Att_Monthly Att_Monthly, Dictionary<string, decimal> LeaveDays_40)
        {
            // 1.1 Nhân viên mới làm việc trong tháng này
            if (Emp_Personal.Onboard_Date > First_Date)
                return 0;

            // 1.2 C0 Vắng mặt
            if (LeaveDays_40["C0_LeaveDays_40"] > 0)
                return 0;

            // 1.3 Đi muộn về sớm
            var sub_Amt = Sal_Parameter
                .Where(x => x.Factory == Att_Monthly.Factory
                         && x.Seq == "2"
                         && x.Num_Times <= Att_Monthly.Delay_Early)
                .Select(x => x.Code_Amt)
                .DefaultIfEmpty(0)
                .Max();
            D01_Bkamt_45 -= sub_Amt;

            // 1.4 Quên bấm thẻ
            sub_Amt = Sal_Parameter
                .Where(x => x.Factory == Att_Monthly.Factory
                         && x.Seq == "3"
                         && x.Num_Times <= Att_Monthly.No_Swip_Card)
                .Select(x => x.Code_Amt)
                .DefaultIfEmpty(0)
                .Max();

            D01_Bkamt_45 -= sub_Amt;

            var leave_Hr = (LeaveDays_40["A0_LeaveDays_40"] + LeaveDays_40["B0_LeaveDays_40"] + LeaveDays_40["H0_LeaveDays_40"]
                         + LeaveDays_40["G0_LeaveDays_40"] + LeaveDays_40["O0_LeaveDays_40"] + LeaveDays_40["J4_LeaveDays_40"]) * 8;

            sub_Amt = _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => x.Type_Seq == "55"
                           && x.Int1 < leave_Hr
                           && x.Int2 >= leave_Hr)
                .Sum(x => x.Int3 ?? 0);

            D01_Bkamt_45 -= sub_Amt;

            return D01_Bkamt_45 >= 0 ? D01_Bkamt_45 : 0m;
        }

        private HRMS_Sal_Tax Sal_Tax_VN(string Factory, DateTime YearMonth, string Employee_ID, decimal A01_OvertimeHours_42, List<Temp> Temp, HRMS_Sal_Monthly Sal_Monthly)
        {
            List<string> Type_Seq = new() { "42", "45", "49" };
            List<string> AddDed_Type = new() { "A", "B" };
            // 1.1 Tính tổng các mục cộng
            var A1 = Temp
                .Where(x => x.Factory == Factory
                         && x.Sal_Month == YearMonth
                         && Type_Seq.Contains(x.Type_Seq)
                         && x.Employee_ID == Employee_ID
                         && AddDed_Type.Contains(x.AddDed_Type))
                ?.Sum(x => x.Amount) ?? 0;

            // 1.2.1 Tính tổng các mục trừ
            var D1 = Query_Sal_Temp(Temp, Sal_Monthly.Sal_Month, Sal_Monthly.Factory, Sal_Monthly.Employee_ID, "49", "B", "B14")
                   + Query_Sal_Temp(Temp, Sal_Monthly.Sal_Month, Sal_Monthly.Factory, Sal_Monthly.Employee_ID, "49", "B", "B44")
                   + Query_Sal_Temp(Temp, Sal_Monthly.Sal_Month, Sal_Monthly.Factory, Sal_Monthly.Employee_ID, "49", "B", "B50");

            var D8 = Query_Sal_Temp(Temp, Sal_Monthly.Sal_Month, Sal_Monthly.Factory, Sal_Monthly.Employee_ID, "49", "B", "B49");

            var D10 = Query_Sal_Temp(Temp, Sal_Monthly.Sal_Month, Sal_Monthly.Factory, Sal_Monthly.Employee_ID, "49", "A", "A06") / 3;

            // 1.2.2 Phụ cấp làm thêm giờ
            var D3 = Query_Sal_Temp(Temp, Sal_Monthly.Sal_Month, Sal_Monthly.Factory, Sal_Monthly.Employee_ID, "42", "A", "A02");

            var D9 = Query_Sal_Temp(Temp, Sal_Monthly.Sal_Month, Sal_Monthly.Factory, Sal_Monthly.Employee_ID, "42", "A", "A01") / 3;

            var D11 = Query_Sal_Temp(Temp, Sal_Monthly.Sal_Month, Sal_Monthly.Factory, Sal_Monthly.Employee_ID, "42", "A", "B01");

            var D12 = Query_Sal_Temp(Temp, Sal_Monthly.Sal_Month, Sal_Monthly.Factory, Sal_Monthly.Employee_ID, "42", "A", "C01");

            var D14 = 0;
            if (A01_OvertimeHours_42 <= 0)
                D14 = Query_Sal_Temp(Temp, Sal_Monthly.Sal_Month, Sal_Monthly.Factory, Sal_Monthly.Employee_ID, "42", "A", "A03") * 100 / 210;

            var D15 = 0;
            if (A01_OvertimeHours_42 > 0)
                D15 = Query_Sal_Temp(Temp, Sal_Monthly.Sal_Month, Sal_Monthly.Factory, Sal_Monthly.Employee_ID, "42", "A", "A03") * 110 / 210;

            var D16 = Query_Sal_Temp(Temp, Sal_Monthly.Sal_Month, Sal_Monthly.Factory, Sal_Monthly.Employee_ID, "42", "A", "C01");

            // 1.2.4 Chi phí ăn uống
            var D4 = Query_Sal_Temp(Temp, Sal_Monthly.Sal_Month, Sal_Monthly.Factory, Sal_Monthly.Employee_ID, "49", "B", "B54")
                   + Query_Sal_Temp(Temp, Sal_Monthly.Sal_Month, Sal_Monthly.Factory, Sal_Monthly.Employee_ID, "49", "B", "B55")
                   + Query_Sal_Temp(Temp, Sal_Monthly.Sal_Month, Sal_Monthly.Factory, Sal_Monthly.Employee_ID, "49", "B", "B56")
                   + Query_Sal_Temp(Temp, Sal_Monthly.Sal_Month, Sal_Monthly.Factory, Sal_Monthly.Employee_ID, "49", "B", "B57");

            var D5 = Query_Sal_Temp(Temp, Sal_Monthly.Sal_Month, Sal_Monthly.Factory, Sal_Monthly.Employee_ID, "57", "D", "V01");

            var D6 = Query_Sal_Temp(Temp, Sal_Monthly.Sal_Month, Sal_Monthly.Factory, Sal_Monthly.Employee_ID, "57", "D", "V02");

            var D7 = Query_Sal_Temp(Temp, Sal_Monthly.Sal_Month, Sal_Monthly.Factory, Sal_Monthly.Employee_ID, "57", "D", "V03");

            // 1.3 Lương chịu thuế
            var wageTax = A1 - (D1 + D3 + D4 + D5 + D6 + D7 + D8 + D9 + D10 + D11 + D12 + D14 + D15 + D16);

            // 2. Tính thuế thu nhập
            // 2.1 Thống kê về khấu trừ bắt buộc ()
            var dependents = _repositoryAccessor.HRMS_Sal_Tax_Number
                .FirstOrDefault(x => x.Factory == Sal_Monthly.Factory
                    && x.Year.Year == Sal_Monthly.Sal_Month.Year
                    && x.Employee_ID == Employee_ID)?.Dependents ?? 0;

            var HSTF = _repositoryAccessor.HRMS_Sal_TaxFree
                .FindAll(x => x.Factory == Sal_Monthly.Factory
                           && x.Salary_Type == Sal_Monthly.Salary_Type
                           && x.Effective_Month <= Sal_Monthly.Sal_Month)
                .AsEnumerable();

            var max_Effective_Month = HSTF
                .Where(x => x.Type == "A")
                .Select(x => x.Effective_Month)
                .DefaultIfEmpty(new())
                .Max();

            var taxFee = HSTF.FirstOrDefault(x => x.Type == "A" && x.Effective_Month == max_Effective_Month)?.Amount ?? 0m;

            var max_Effective_Month_K = HSTF
                .Where(x => x.Type == "K")
                .Select(x => x.Effective_Month)
                .DefaultIfEmpty(new())
                .Max();

            var dependentTaxFree = HSTF.FirstOrDefault(x => x.Type == "K" && x.Effective_Month == max_Effective_Month_K)?.Amount ?? 0m;

            // 2.2 Tính thuế thu nhập
            var tax = 0;
            var totalAmount = 0m;

            if (wageTax >= taxFee)
            {
                totalAmount = wageTax - taxFee - (dependentTaxFree * dependents);

                // Thừa
                if (totalAmount < 0)
                    tax = 0;
            }

            // 2.2.1 Lấy biểu thuế thu nhập
            var HSTB = _repositoryAccessor.HRMS_Sal_Taxbracket
                .FindAll(x => x.Nation == "VN"
                           && x.Tax_Code == "ZZ"
                           && x.Effective_Month <= Sal_Monthly.Sal_Month)
                .AsEnumerable();

            var taxbracket_Max_Effective_Month = HSTB
                .Where(x => x.Nation == "VN"
                         && x.Tax_Code == "ZZ"
                         && x.Effective_Month <= Sal_Monthly.Sal_Month)
                .Select(x => x.Effective_Month)
                .DefaultIfEmpty(new())
                .Max();

            var Value = HSTB
                .FirstOrDefault(x => x.Income_Start < totalAmount
                                  && x.Income_End >= totalAmount
                                  && x.Effective_Month == taxbracket_Max_Effective_Month);
            // 2.2.2 
            tax = Value != null ? ((int)(totalAmount * (Value.Rate / 100) - Value.Deduction)) : 0;
            var temp = Temp.FirstOrDefault(x => x.Factory == Factory && x.Sal_Month == YearMonth && x.Employee_ID == Employee_ID);
            HRMS_Sal_Tax sal_Tax = new()
            {
                USER_GUID = temp.USER_GUID,
                Factory = Factory,
                Sal_Month = YearMonth,
                Employee_ID = temp.Employee_ID,
                Department = temp.Department,
                Currency = temp.Currency,
                Num_Dependents = dependents,
                Add_Total = A1,
                Ded_Total = D1 + D3 + D4 + D5 + D6 + D7 + D8 + D9 + D10 + D11 + D12 + D14 + D15 + D16,
                Salary_Amt = wageTax,
                Rate = Value?.Rate ?? 0,
                Tax = tax,
            };
            return sal_Tax;
        }
        private static int Query_Sal_Temp(List<Temp> Temp, DateTime YearMonth, string Factory, string Employee_ID, string Seq, string Type, string Item)
        {
            return Temp
                .Where(x => x.Factory == Factory
                         && x.Sal_Month == YearMonth
                         && x.Type_Seq == Seq
                         && x.Employee_ID == Employee_ID
                         && x.AddDed_Type == Type
                         && x.Item == Item)
                ?.Sum(x => x.Amount) ?? 0;
        }

        private static HRMS_Sal_Parameter Query_Sal_Parameter(IEnumerable<HRMS_Sal_Parameter> HSP, string Factory, string Seq, string Code)
        {
            return HSP.FirstOrDefault(x => x.Factory == Factory && x.Seq == Seq && x.Code == Code);
        }

        private static int Query_HRMS_Sal_AddDedItem_Values(IEnumerable<HRMS_Sal_AddDedItem_Settings> HSAIS, string Factory, DateTime YearMonth, string Permission_Group, string Salary_Type, string AddDed_Type, string AddDed_Item)
        {
            var max_Effective_Month = HSAIS
                .Where(x => x.Factory == Factory
                         && x.Permission_Group == Permission_Group
                         && x.Salary_Type == Salary_Type
                         && x.AddDed_Type == AddDed_Type
                         && x.AddDed_Item == AddDed_Item
                         && x.Effective_Month <= YearMonth)
                .Select(x => x.Effective_Month)
                .DefaultIfEmpty(new DateTime())
                .Max();

            var result = HSAIS
                .Where(x => x.Factory == Factory
                         && x.Permission_Group == Permission_Group
                         && x.Salary_Type == Salary_Type
                         && x.AddDed_Type == AddDed_Type
                         && x.AddDed_Item == AddDed_Item
                         && x.Effective_Month == max_Effective_Month)
                ?.Sum(x => x.Amount) ?? 0;

            return result;
        }

        public class Query_Sal_Detail_Variable_Combine_Select
        {
            public string Salary_Item_values { get; set; }
            public int Amount_Item_values { get; set; }
        }

        public class Query_Att_Monthly_Detail_Variable_Combine_Select
        {
            public string Leave_Code_Values { get; set; }
            public decimal Day_Values { get; set; }
        }


        private class Temp
        {
            public string USER_GUID { get; set; }
            public string Division { get; set; }
            public string Factory { get; set; }
            public DateTime Sal_Month { get; set; }
            public string Employee_ID { get; set; }
            public string Probation { get; set; }
            public string Type_Seq { get; set; }
            public string AddDed_Type { get; set; }
            public string Item { get; set; }
            public int Amount { get; set; }
            public string Currency { get; set; }
            public string Department { get; set; }

            public Temp() { }

            public Temp(string USER_GUID, string Division, string Factory, DateTime Sal_Month, string Employee_ID, string Type_Seq, string AddDed_Type, string Item, int Amount, string Currency, string Department)
            {
                this.USER_GUID = USER_GUID;
                this.Division = Division;
                this.Factory = Factory;
                this.Sal_Month = Sal_Month;
                this.Employee_ID = Employee_ID;
                this.Type_Seq = Type_Seq;
                this.AddDed_Type = AddDed_Type;
                this.Item = Item;
                this.Amount = Amount;
                this.Currency = Currency;
                this.Department = Department;
            }
        }

        private class PTemp
        {
            public string USER_GUID { get; set; }
            public string Division { get; set; }
            public string Factory { get; set; }
            public DateTime Sal_Month { get; set; }
            public string Employee_ID { get; set; }
            public string Probation { get; set; }
            public string Type_Seq { get; set; }
            public string AddDed_Type { get; set; }
            public string Item { get; set; }
            public int Amount { get; set; }

            public PTemp() { }

            public PTemp(string USER_GUID, string Division, string Factory, DateTime Sal_Month,
                         string Employee_ID, string Probation, string Type_Seq, string AddDed_Type,
                         string Item, int Amount)
            {
                this.USER_GUID = USER_GUID;
                this.Division = Division;
                this.Factory = Factory;
                this.Sal_Month = Sal_Month;
                this.Employee_ID = Employee_ID;
                this.Probation = Probation;
                this.Type_Seq = Type_Seq;
                this.AddDed_Type = AddDed_Type;
                this.Item = Item;
                this.Amount = Amount;
            }
        }
    }
}