using System.Diagnostics;
using System.Globalization;
using API.Data;
using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.SalaryMaintenance
{
    public class S_7_1_21_MenstrualLeaveHoursAllowance : BaseServices, I_7_1_21_MenstrualLeaveHoursAllowance
    {
        private static readonly SemaphoreSlim semaphore = new(1, 1);
        public S_7_1_21_MenstrualLeaveHoursAllowance(DBContext dbContext) : base(dbContext) { }

        #region Execute
        public async Task<OperationResult> CheckData(MenstrualLeaveHoursAllowanceParam param)
        {
            if (string.IsNullOrWhiteSpace(param.Factory) ||
                param.Permission_Group.Count == 0 ||
                !DateTime.TryParseExact(param.Year_Month, "yyyy/MM", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime yearMonth))
                return new OperationResult(false, "Invalid inputs");

            var HSC = _repositoryAccessor.HRMS_Sal_Close.FindAll(x => x.Factory == param.Factory);

            // 1.1
            var ct1 = await HSC.CountAsync(x => x.Sal_Month == yearMonth.AddMonths(-1));

            if (ct1 == 0)
                return new OperationResult(false, "存在前月未關帳之薪資資料,請關帳後再執行!");

            var HSM = _repositoryAccessor.HRMS_Sal_Monthly
                .FindAll(x => x.Factory == param.Factory
                           && param.Permission_Group.Contains(x.Permission_Group)
                           && x.Sal_Month == yearMonth
                           && (string.IsNullOrWhiteSpace(param.Employee_ID) || x.Employee_ID == param.Employee_ID))
                .Select(x => x.Employee_ID);

            var ct2 = await HSC.CountAsync(x => x.Sal_Month == yearMonth && HSM.Contains(x.Employee_ID));

            if (ct2 > 0)
                return new OperationResult(false, "當月薪資已有關帳資料,不可以再次執行生理期休息時數津貼產生作業!");
            var predHSAM = PredicateBuilder.New<HRMS_Sal_AddDedItem_Monthly>(x =>
                x.Factory == param.Factory &&
                x.Sal_Month == yearMonth &&
                x.AddDed_Type == "B" &&
                x.AddDed_Item == "B14"
            );
            if (!string.IsNullOrWhiteSpace(param.Employee_ID))
                predHSAM.And(x => x.Employee_ID == param.Employee_ID);
            var HSAM = _repositoryAccessor.HRMS_Sal_AddDedItem_Monthly.FindAll(predHSAM);
            var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll(x =>
                x.Factory == param.Factory &&
                param.Permission_Group.Contains(x.Permission_Group)
            );
            var ct3 = await HSAM.Join(HEP,
                x => x.Employee_ID,
                y => y.Employee_ID,
                (x, y) => x).CountAsync();
            if (ct3 > 0)
                return new OperationResult(true, "DeleteData");
            return new OperationResult(true);
        }

        private async Task<OperationResult> DeleteData(MenstrualLeaveHoursAllowanceParam param)
        {
            if (!DateTime.TryParseExact(param.Year_Month, "yyyy/MM", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime yearMonth))
                return new OperationResult(false, "Invalid input Year Month ");
            var predHSAM = PredicateBuilder.New<HRMS_Sal_AddDedItem_Monthly>(x =>
               x.Factory == param.Factory &&
               x.Sal_Month == yearMonth &&
               x.AddDed_Type == "B" &&
               x.AddDed_Item == "B14"
           );
            if (!string.IsNullOrWhiteSpace(param.Employee_ID))
                predHSAM.And(x => x.Employee_ID == param.Employee_ID);
            var HSAM = _repositoryAccessor.HRMS_Sal_AddDedItem_Monthly.FindAll(predHSAM);
            var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll(x =>
                x.Factory == param.Factory &&
                param.Permission_Group.Contains(x.Permission_Group)
            );
            var data = await HSAM.Join(HEP,
                x => x.Employee_ID,
                y => y.Employee_ID,
                (x, y) => x).ToListAsync();
            try
            {
                if (data.Any())
                {
                    _repositoryAccessor.HRMS_Sal_AddDedItem_Monthly.RemoveMultiple(data);
                    await _repositoryAccessor.Save();
                }
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false);
            }
        }

        public async Task<OperationResult> Execute(MenstrualLeaveHoursAllowanceParam param)
        {
            if (string.IsNullOrWhiteSpace(param.Factory) ||
                param.Permission_Group.Count == 0 ||
                !DateTime.TryParseExact(param.Year_Month, "yyyy/MM", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime yearMonth))
                return new OperationResult(false, "Invalid inputs");
            // Khai báo biến
            List<string> Work_Shift_Type = new() { "T0", "H0", "J0" };
            List<HRMS_Sal_AddDedItem_Monthly> dataAdd = new();
            var Month_First_day = yearMonth;
            var Month_Last_day = yearMonth.AddMonths(1).AddDays(-1);

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
                        return new OperationResult(false, "Delete failed!");
                    }
                }

                // Lấy dữ liệu dùng chung
                var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.PermissionGroup);

                var HAM = _repositoryAccessor.HRMS_Att_Monthly
                    .FindAll(x => x.Factory == param.Factory
                               && x.Att_Month == yearMonth)
                    .ToList();

                var HARM = _repositoryAccessor.HRMS_Att_Resign_Monthly
                    .FindAll(x => x.Factory == param.Factory
                               && x.Att_Month == yearMonth)
                    .Select(x => x.USER_GUID)
                    .ToList();

                var HAPD = _repositoryAccessor.HRMS_Att_Pregnancy_Data
                    .FindAll(x => x.Factory == param.Factory
                               && x.Close_Case == false)
                    .ToList();

                var HSMB = _repositoryAccessor.HRMS_Sal_MasterBackup
                    .FindAll(x => x.Factory == param.Factory
                               && x.Sal_Month == yearMonth)
                    .ToList();

                var HSIS = _repositoryAccessor.HRMS_Sal_Item_Settings
                    .FindAll(x => x.Factory == param.Factory
                               && x.Effective_Month <= yearMonth)
                    .ToList();

                var HSMBD = _repositoryAccessor.HRMS_Sal_MasterBackup_Detail
                    .FindAll(x => x.Factory == param.Factory
                               && x.Sal_Month == yearMonth)
                    .ToList();

                var HALM = _repositoryAccessor.HRMS_Att_Leave_Maintain
                    .FindAll(x => x.Factory == param.Factory
                               && x.Leave_Date >= Month_First_day
                               && x.Leave_Date <= Month_Last_day
                               && x.Leave_code == "M0")
                    .ToList();

                var HAFMH = _repositoryAccessor.HRMS_Att_Female_Menstrual_Hours
                    .FindAll(x => x.Factory == param.Factory
                               && x.Att_Month == yearMonth)
                    .ToList();

                // 1.2
                // 1.2.1
                decimal TotalDays = HAM
                    .Where(x => param.Permission_Group.Contains(x.Permission_Group))
                    .Select(x => x.Salary_Days)
                    .DefaultIfEmpty(0)
                    .Max();

                // 1.2.2

                var cr_pt1 = _repositoryAccessor.HRMS_Emp_Personal
                    .FindAll(x => x.Factory == param.Factory
                               && x.Onboard_Date <= Month_Last_day
                               && param.Permission_Group.Contains(x.Permission_Group)
                               && !HARM.Contains(x.USER_GUID)
                               && (x.Resign_Date.HasValue == false || x.Resign_Date.Value > Month_Last_day)
                               && (string.IsNullOrWhiteSpace(param.Employee_ID) || x.Employee_ID == param.Employee_ID)
                               && x.Gender == "F"
                               && x.Work_Shift_Type != "S0")
                    .OrderBy(x => x.Employee_ID);

                foreach (var Emp_Personal_Values in cr_pt1)
                {
                    // Work_Shift_Type != T0, H0, J0
                    if (!Work_Shift_Type.Contains(Emp_Personal_Values.Work_Shift_Type))
                    {
                        var tmp_cnt = HAPD.Count(x => x.Employee_ID == Emp_Personal_Values.Employee_ID);

                        if (tmp_cnt > 0)
                            continue;
                    }
                    // 1.2.3
                    var Sal_Master_Values = HSMB.FirstOrDefault(x => x.Employee_ID == Emp_Personal_Values.Employee_ID);

                    if (Sal_Master_Values is null)
                        continue;

                    // Query_WageStandard_Sum default Kind = B
                    var max_Effective_Month = HSIS
                        .Where(x => x.Permission_Group == Emp_Personal_Values.Permission_Group
                                 && x.Salary_Type == Sal_Master_Values.Salary_Type)
                        .Select(x => x.Effective_Month)
                        .DefaultIfEmpty(new DateTime())
                        .Max();

                    var Salary_Code_List = HSIS
                        .Where(x => x.Permission_Group == Emp_Personal_Values.Permission_Group
                                 && x.Salary_Type == Sal_Master_Values.Salary_Type
                                 && x.Insurance == "Y"
                                 && x.Effective_Month == max_Effective_Month)
                        .Select(x => x.Salary_Item);

                    int Amount_Values = HSMBD
                            .Where(x => x.Employee_ID == Emp_Personal_Values.Employee_ID
                                    & Salary_Code_List.Contains(x.Salary_Item))?
                            .Sum(x => x.Amount) ?? 0;

                    // End Query_WageStandard_Sum
                    var df_mc = Amount_Values;

                    // 1.2.4
                    var Att_Monthly_Values = HAM.FirstOrDefault(x => x.Employee_ID == Emp_Personal_Values.Employee_ID);

                    if (Att_Monthly_Values is null)
                        continue;

                    // Query_Leave_Sum_Days
                    decimal M0Days = HALM.Where(x => x.Employee_ID == Att_Monthly_Values.Employee_ID)?.Sum(x => x.Days) ?? 0;
                    // End Query_Leave_Sum_Days

                    if (M0Days > 0)
                        Att_Monthly_Values.Actual_Days += M0Days;

                    if (Att_Monthly_Values.Actual_Days == 0)
                        continue;

                    // 1.2.5
                    decimal df_hr = 0m;
                    if (Att_Monthly_Values.Actual_Days > 2)
                        df_hr = 1.5m;
                    else if (Att_Monthly_Values.Actual_Days > 1 && Att_Monthly_Values.Actual_Days <= 2)
                        df_hr = 1;
                    else if (Att_Monthly_Values.Actual_Days <= 1 && Att_Monthly_Values.Actual_Days > 0)
                        df_hr = 0.5m;

                    if (Att_Monthly_Values.Actual_Days == 0)
                        continue;

                    decimal tmc_hr = HAFMH.Where(x => x.Employee_ID == Emp_Personal_Values.Employee_ID)?.Sum(x => x.Breaks_Hours) ?? 0;

                    int amt = (int)(df_mc / TotalDays / 8 * (df_hr - tmc_hr));
                    if (amt <= 0)
                        continue;

                    var code = HBC.FirstOrDefault(x => x.Code == Emp_Personal_Values.Permission_Group);

                    if (code is null)
                        continue;

                    var data = new HRMS_Sal_AddDedItem_Monthly()
                    {
                        USER_GUID = Emp_Personal_Values.USER_GUID,
                        Factory = param.Factory,
                        Sal_Month = yearMonth,
                        Employee_ID = Emp_Personal_Values.Employee_ID,
                        AddDed_Type = "B",
                        AddDed_Item = "B14",
                        Currency = code.Char1 ?? "",
                        Amount = amt,
                        Update_By = param.UserName,
                        Update_Time = DateTime.Now
                    };
                    dataAdd.Add(data);
                }
                if (dataAdd.Any())
                {
                    _repositoryAccessor.HRMS_Sal_AddDedItem_Monthly.AddMultiple(dataAdd);
                    await _repositoryAccessor.Save();
                }
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true, dataAdd.Count);
            }
            catch (Exception)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false);
            }
            finally
            {
                semaphore.Release();
            }
        }
        #endregion

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
    }
}