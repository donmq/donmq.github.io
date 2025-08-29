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
    public class S_7_1_20_NightShiftSubsidyMaintenance : BaseServices, I_7_1_20_NightShiftSubsidyMaintenance
    {

        private static readonly SemaphoreSlim semaphore = new(1, 1);
        public S_7_1_20_NightShiftSubsidyMaintenance(DBContext dbContext) : base(dbContext)
        {
        }
        public async Task<OperationResult> ExcuteQuery(NightShiftSubsidyMaintenanceDto_Param param, string account)
        {
            if (!DateTime.TryParseExact(param.Year_Month, "yyyy/MM", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime yearMonth))
                return new OperationResult(false, "Invalid input Year Month ");

            DateTime monthFirstDay = yearMonth;
            DateTime monthLastDay = yearMonth.AddMonths(1).AddDays(-1);
            List<HRMS_Sal_AddDedItem_Monthly> addList = new();
            await semaphore.WaitAsync();
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {

                #region 1.1 
                // xóa data
                if (param.Is_Delete)
                {
                    var delete = await DeleteData(param);
                    if (!delete.IsSuccess)
                    {
                        await _repositoryAccessor.RollbackAsync();
                        return new OperationResult(false, "Delete failed!");
                    }
                }
                #endregion 1.1
                #region 1.2
                //1.2.1
                var preEmpPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(
                    x => x.Factory == param.Factory &&
                         x.Onboard_Date < monthLastDay &&
                        (!x.Resign_Date.HasValue || (x.Resign_Date.HasValue && x.Resign_Date.Value.Date > monthLastDay.Date)));

                var preAttMonthlyDetail = PredicateBuilder.New<HRMS_Att_Monthly_Detail>(
                    x => x.Factory == param.Factory &&
                         x.Att_Month.Date == yearMonth.Date &&
                         x.Leave_Type == "2" &&
                         x.Leave_Code == "A02"
                );
                var HAMD = _repositoryAccessor.HRMS_Att_Monthly_Detail.FindAll(preAttMonthlyDetail);
                var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll(preEmpPersonal);
                var dataList = await HEP
                .Join(HAMD,
                    a => new { a.Factory, a.USER_GUID },
                    b => new { b.Factory, b.USER_GUID },
                    (a, b) => new { HEP = a, HAMD = b })
                    .Select(x => new HRMS_Sal_AddDedItem_Monthly_Data
                    {
                        USER_GUID = x.HAMD.USER_GUID,
                        Employee_ID = x.HAMD.Employee_ID,
                        PermissionGroup = x.HEP.Permission_Group
                    }).Distinct().ToListAsync();

                if (!dataList.Any())
                {
                    await _repositoryAccessor.RollbackAsync();
                    return new OperationResult(false, "No data for add");
                }

                #region 1.2.2
                var userGuidList = dataList.Select(x => x.USER_GUID).Distinct().ToList();
                var totalDaysDictionary = await HAMD
                    .Where(x => dataList.Select(y => y.USER_GUID).Contains(x.USER_GUID) && x.Leave_Code == "A02")
                    .GroupBy(x => x.USER_GUID)
                    .Select(g => new
                    {
                        USER_GUID = g.Key,
                        TotalDays = g.Sum(x => x.Days)
                    })
                    .ToDictionaryAsync(x => x.USER_GUID, x => x.TotalDays);

                var HAM = _repositoryAccessor.HRMS_Sal_Master
                    .FindAll(x =>
                        x.Factory == param.Factory &&
                        dataList.Select(y => y.Employee_ID).Contains(x.Employee_ID))
                    .ToList();
                var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.PermissionGroup);
                decimal totalSubsidy = 0;
                int processRecord = 0;
                foreach (var item in dataList)
                {
                    if (!totalDaysDictionary.ContainsKey(item.USER_GUID))
                        continue;
                    decimal totalDays = totalDaysDictionary[item.USER_GUID];
                    int added = totalDays >= 48 ? 50000 : 0;

                   var salMasterValues = HAM.FirstOrDefault(x => x.Employee_ID == item.Employee_ID);
                    if (salMasterValues is null) continue;

                    var code = HBC.FirstOrDefault(x => x.Code == item.PermissionGroup);
                    if(code is null) continue;
                    Query_HRMS_Sal_AddDedItem_Values HSAparam = new()
                    {
                        Factory = param.Factory,
                        Year_Month = yearMonth,
                        Permission = item.PermissionGroup,
                        AddDed_Type = "A",
                        AddDed_Item = "A12",
                        Salary_Type = salMasterValues.Salary_Type
                    };

                    var amount = await Query_HRMS_Sal_AddDedItem_Values(HSAparam);

                    totalSubsidy = amount * totalDays + added;
                    addList.Add(new HRMS_Sal_AddDedItem_Monthly
                    {
                        USER_GUID = item.USER_GUID,
                        Factory = param.Factory,
                        Sal_Month = yearMonth,
                        Employee_ID = item.Employee_ID,
                        AddDed_Type = "A",
                        AddDed_Item = "A12",
                        Currency = code.Char1 ?? "",
                        Amount = (int)totalSubsidy,
                        Update_By = account,
                        Update_Time = DateTime.Now
                    });
                    processRecord += 1;
                }
                //INSERT HRMS_Sal_AddDedItem_Monthly
                #endregion
                if (addList.Any())
                    _repositoryAccessor.HRMS_Sal_AddDedItem_Monthly.AddMultiple(addList);
                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true) { Data = processRecord };
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
        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName)
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
        private async Task<decimal> Query_HRMS_Sal_AddDedItem_Values(Query_HRMS_Sal_AddDedItem_Values param)
        {
            var HSAS = _repositoryAccessor.HRMS_Sal_AddDedItem_Settings
                .FindAll(x => x.Factory == param.Factory &&
                          x.Permission_Group == param.Permission &&
                          x.Salary_Type == param.Salary_Type &&
                          x.AddDed_Type == param.AddDed_Type &&
                          x.AddDed_Item == param.AddDed_Item &&
                          x.Effective_Month <= param.Year_Month
                );
            if (!await HSAS.AnyAsync())
                return 0;
            var amountValues = await HSAS.FirstOrDefaultAsync(x => x.Effective_Month == HSAS.Max(y => y.Effective_Month));
            return amountValues?.Amount ?? 0;
        }

        public async Task<OperationResult> CheckData(NightShiftSubsidyMaintenanceDto_Param param)
        {
            if (!DateTime.TryParseExact(param.Year_Month, "yyyy/MM", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime yearMonth))
                return new OperationResult(false, "Invalid input Year Month ");
            // check wk_count
            if (!await _repositoryAccessor.HRMS_Att_Monthly.AnyAsync(x => x.Factory == param.Factory &&
             x.Att_Month == yearMonth))
            {
                return new OperationResult(false, "本月出勤資料未產生，不可執行!");
            }

            var userGUID = await _repositoryAccessor.HRMS_Emp_Personal
                        .FindAll(x => x.Factory == param.Factory &&
                        param.Permission.Contains(x.Permission_Group)).Select(x => x.USER_GUID).ToListAsync();
            var adMonth = await _repositoryAccessor.HRMS_Sal_AddDedItem_Monthly
                        .AnyAsync(x => x.Factory == param.Factory && x.Sal_Month.Date == yearMonth.Date &&
                        x.AddDed_Type == "A" &&
                        x.AddDed_Item == "A12" &&
                        userGUID.Contains(x.USER_GUID));
            if (adMonth)
                return new OperationResult(true, "DeleteData");
            return new OperationResult(true);
        }

        private async Task<OperationResult> DeleteData(NightShiftSubsidyMaintenanceDto_Param param)
        {
            if (!DateTime.TryParseExact(param.Year_Month, "yyyy/MM", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime yearMonth))
                return new OperationResult(false, "Invalid input Year Month ");
            var userGUID = await _repositoryAccessor.HRMS_Emp_Personal
                        .FindAll(x => x.Factory == param.Factory &&
                        param.Permission.Contains(x.Permission_Group)).Select(x => x.USER_GUID).ToListAsync();

            var data = await _repositoryAccessor.HRMS_Sal_AddDedItem_Monthly
                        .FindAll(x => x.Factory == param.Factory && x.Sal_Month == yearMonth &&
                        x.AddDed_Type == "A" &&
                        x.AddDed_Item == "A12" &&
                        userGUID.Contains(x.USER_GUID)).ToListAsync();
            try
            {
                _repositoryAccessor.HRMS_Sal_AddDedItem_Monthly.RemoveMultiple(data);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch
            {
                return new OperationResult(false);
            }
        }


    }
    #endregion
}