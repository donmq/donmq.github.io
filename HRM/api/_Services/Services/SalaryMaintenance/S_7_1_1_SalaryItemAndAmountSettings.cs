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
    public class S_7_1_1_SalaryItemAndAmountSettings : BaseServices, I_7_1_1_SalaryItemAndAmountSettings
    {
        public S_7_1_1_SalaryItemAndAmountSettings(DBContext dbContext) : base(dbContext)
        {
        }
        #region Dropdown List
        public async Task<List<KeyValuePair<string, string>>> GetDropDownList(SalaryItemAndAmountSettings_MainParam param, List<string> roleList)
        {
            var HBC = await _repositoryAccessor.HRMS_Basic_Code.FindAll().ToListAsync();
            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower()).ToList();
            var result = new List<KeyValuePair<string, string>>();
            var data = HBC.GroupJoin(HBCL,
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (x, y) => new { hbc = x, hbcl = y })
                    .SelectMany(x => x.hbcl.DefaultIfEmpty(),
                    (x, y) => new { x.hbc, hbcl = y });
            var authFactories = await Queryt_Factory_AddList(roleList);
            result.AddRange(data.Where(x => x.hbc.Type_Seq == BasicCodeTypeConstant.Factory && authFactories.Contains(x.hbc.Code)).Select(x => new KeyValuePair<string, string>("FA", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            result.AddRange(data.Where(x => x.hbc.Type_Seq == BasicCodeTypeConstant.PermissionGroup).Select(x => new KeyValuePair<string, string>("PE", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            result.AddRange(data.Where(x => x.hbc.Type_Seq == BasicCodeTypeConstant.SalaryType).Select(x => new KeyValuePair<string, string>("ST", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            if (param.FormType != "Main")
            {
                result.AddRange(data.Where(x => x.hbc.Type_Seq == BasicCodeTypeConstant.SalaryItem).Select(x => new KeyValuePair<string, string>("SI", $"{x.hbc.Code}- {(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
                result.AddRange(data.Where(x => x.hbc.Type_Seq == BasicCodeTypeConstant.SalaryKind).Select(x => new KeyValuePair<string, string>("KI", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            }
            return result;
        }
        #endregion
        public async Task<OperationResult> IsExistedData(SalaryItemAndAmountSettings_MainData param)
        {
            var HSIS = await _repositoryAccessor.HRMS_Sal_Item_Settings.FindAll(x =>
                x.Factory == param.Factory &&
                x.Permission_Group == param.Permission_Group &&
                x.Salary_Type == param.Salary_Type &&
                x.Effective_Month.Date == Convert.ToDateTime(param.Effective_Month).Date
            ).ToListAsync();
            if (HSIS.Any())
            {
                var data = HSIS.Select(x => new SalaryItemAndAmountSettings_SubData
                {
                    Seq = x.Seq.ToString(),
                    Salary_Item = x.Salary_Item,
                    Kind = x.Kind,
                    Insurance = x.Insurance,
                    Amount = x.Amount.ToString(),
                    Update_By = x.Update_By,
                    Update_Time = x.Update_Time,
                    Is_Duplicate = false
                }).ToList();
                var result = new SalaryItemAndAmountSettings_Update
                {
                    Param = new SalaryItemAndAmountSettings_SubParam
                    {
                        Factory = HSIS[0].Factory,
                        Effective_Month = HSIS[0].Effective_Month.ToString("yyyy/MM/dd"),
                        Permission_Group = HSIS[0].Permission_Group,
                        Salary_Type = HSIS[0].Salary_Type,
                        Salary_Days = HSIS[0].Salary_Days.ToString(),
                    },
                    Data = data
                };
                return new OperationResult(true, result);
            }
            return new OperationResult(false);
        }
        public async Task<OperationResult> IsDuplicatedData(SalaryItemAndAmountSettings_SubParam param, string userName)
        {
            List<SalaryItemAndAmountSettings_SubData> result = new();
            var HAIS = await _repositoryAccessor.HRMS_Sal_Item_Settings.FindAll(x =>
                x.Factory == param.Factory &&
                x.Permission_Group == param.Permission_Group &&
                x.Salary_Type == param.Salary_Type
            ).ToListAsync();
            if (!HAIS.Any())
                return new OperationResult(false, result);
            var selected_HAIS = HAIS.FindAll(x => x.Effective_Month.Date == Convert.ToDateTime(param.Effective_Month_Str).Date);

            if (selected_HAIS.Any())
                return new OperationResult(true);
            var now = DateTime.Now;
            var maxEffectiveMonth = HAIS.Max(x => x.Effective_Month);
            result = HAIS.FindAll(x => x.Effective_Month.Date == maxEffectiveMonth.Date).Select(x => new SalaryItemAndAmountSettings_SubData
            {
                Seq = x.Seq.ToString(),
                Salary_Item = x.Salary_Item,
                Kind = x.Kind,
                Insurance = x.Insurance,
                Amount = x.Amount.ToString(),
                Update_By = userName,
                Update_Time = now,
                Is_Duplicate = false
            }).ToList();
            return new OperationResult(false, result);
        }
        public async Task<PaginationUtility<SalaryItemAndAmountSettings_MainData>> GetSearchDetail(PaginationParam paginationParams, SalaryItemAndAmountSettings_MainParam searchParam)
        {
            var predicate = PredicateBuilder.New<HRMS_Sal_Item_Settings>(x =>
                x.Factory == searchParam.Factory &&
                searchParam.Permission_Group.Contains(x.Permission_Group)
            );
            var predicateHSM = PredicateBuilder.New<HRMS_Sal_Monthly>(x =>
              x.Factory == searchParam.Factory &&
              searchParam.Permission_Group.Contains(x.Permission_Group) &&
              x.Lock == "Y"
            );
            if (!string.IsNullOrWhiteSpace(searchParam.Salary_Type))
            {
                predicate.And(x => x.Salary_Type == searchParam.Salary_Type);
                predicateHSM.And(x => x.Salary_Type == searchParam.Salary_Type);
            }
            if (!string.IsNullOrWhiteSpace(searchParam.Effective_Month_Str) && DateTime.TryParseExact(searchParam.Effective_Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime effectiveMonthValue))
            {
                predicate.And(x => x.Effective_Month.Date == effectiveMonthValue.Date);
                predicateHSM.And(x => x.Sal_Month.Date >= effectiveMonthValue.Date);
            }

            var HSM = _repositoryAccessor.HRMS_Sal_Monthly.FindAll(predicateHSM);
            var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll();
            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == searchParam.Lang.ToLower());
            var codLang = HBC
                .GroupJoin(HBCL,
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (x, y) => new { HBC = x, HBCL = y })
                .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (x, y) => new { x.HBC, HBCL = y })
                .Select(x => new
                {
                    x.HBC.Code,
                    x.HBC.Type_Seq,
                    Code_Name = x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name
                });
            var data = _repositoryAccessor.HRMS_Sal_Item_Settings
                .FindAll(predicate)
                .OrderBy(x => x.Permission_Group)
                .ThenBy(x => x.Salary_Type)
                .ThenByDescending(x => x.Effective_Month);
            var result = data.Select(x => new SalaryItemAndAmountSettings_MainData
            {
                Factory = x.Factory,
                Effective_Month = x.Effective_Month.ToString("yyyy/MM/dd"),
                Permission_Group = x.Permission_Group,
                Permission_Group_Name = x.Permission_Group + " - " + codLang.FirstOrDefault(y => y.Type_Seq == BasicCodeTypeConstant.PermissionGroup && y.Code == x.Permission_Group).Code_Name,
                Salary_Type = x.Salary_Type,
                Salary_Type_Name = x.Salary_Type + " - " + codLang.FirstOrDefault(y => y.Type_Seq == BasicCodeTypeConstant.SalaryType && y.Code == x.Salary_Type).Code_Name,
                Salary_Days = x.Salary_Days.ToString(),
                Seq = x.Seq.ToString(),
                Salary_Item = x.Salary_Item,
                Salary_Item_Name = x.Salary_Item + " - " + codLang.FirstOrDefault(y => y.Type_Seq == BasicCodeTypeConstant.SalaryItem && y.Code == x.Salary_Item).Code_Name,
                Kind = x.Kind,
                Kind_Name = x.Kind + " - " + codLang.FirstOrDefault(y => y.Type_Seq == BasicCodeTypeConstant.SalaryKind && y.Code == x.Kind).Code_Name,
                Insurance = x.Insurance,
                Amount = x.Amount.ToString(),
                Update_By = x.Update_By,
                Update_Time = x.Update_Time.ToString("yyyy/MM/dd HH:mm:ss"),
                Is_Editable = !HSM.Any(y =>
                    y.Factory == x.Factory &&
                    y.Permission_Group == x.Permission_Group &&
                    y.Salary_Type == x.Salary_Type &&
                    y.Sal_Month.Date >= x.Effective_Month.Date
                )
            });
            return await PaginationUtility<SalaryItemAndAmountSettings_MainData>.CreateAsync(result, paginationParams.PageNumber, paginationParams.PageSize);
        }
        public async Task<OperationResult> PostData(SalaryItemAndAmountSettings_Update input)
        {
            var predicate = PredicateBuilder.New<HRMS_Sal_Item_Settings>(true);
            if (string.IsNullOrWhiteSpace(input.Param.Factory)
             || string.IsNullOrWhiteSpace(input.Param.Permission_Group)
             || string.IsNullOrWhiteSpace(input.Param.Salary_Type)
             || string.IsNullOrWhiteSpace(input.Param.Salary_Days)
             || string.IsNullOrWhiteSpace(input.Param.Effective_Month_Str)
             || !DateTime.TryParseExact(input.Param.Effective_Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime effectiveMonthValue)
            )
                return new OperationResult(false, "InvalidInput");
            try
            {
                predicate.And(x =>
                    x.Factory == input.Param.Factory &&
                    x.Permission_Group == input.Param.Permission_Group &&
                    x.Salary_Type == input.Param.Salary_Type &&
                    x.Effective_Month.Date == effectiveMonthValue.Date
                );
                var HAIS = await _repositoryAccessor.HRMS_Sal_Item_Settings.FindAll(predicate).ToListAsync();
                List<HRMS_Sal_Item_Settings> addList = new();
                foreach (var item in input.Data)
                {
                    if (string.IsNullOrWhiteSpace(item.Seq)
                     || string.IsNullOrWhiteSpace(item.Salary_Item)
                     || string.IsNullOrWhiteSpace(item.Kind)
                     || string.IsNullOrWhiteSpace(item.Insurance)
                     || string.IsNullOrWhiteSpace(item.Amount)
                     || string.IsNullOrWhiteSpace(item.Update_By)
                     || string.IsNullOrWhiteSpace(item.Update_Time_Str)
                     || !DateTime.TryParseExact(item.Update_Time_Str, "yyyy/MM/dd HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime updateTimeValue)
                    )
                        return new OperationResult(false, "InvalidInput");
                    if (HAIS.Any(x => x.Salary_Item == item.Salary_Item))
                        return new OperationResult(false, "AlreadyExitedData");
                    HRMS_Sal_Item_Settings addData = new()
                    {
                        Factory = input.Param.Factory,
                        Permission_Group = input.Param.Permission_Group,
                        Salary_Type = input.Param.Salary_Type,
                        Effective_Month = effectiveMonthValue,
                        Salary_Days = short.Parse(input.Param.Salary_Days),
                        Seq = int.Parse(item.Seq),
                        Salary_Item = item.Salary_Item,
                        Kind = item.Kind,
                        Insurance = item.Insurance,
                        Amount = int.Parse(item.Amount),
                        Update_By = item.Update_By,
                        Update_Time = updateTimeValue,
                    };
                    addList.Add(addData);
                }
                _repositoryAccessor.HRMS_Sal_Item_Settings.AddMultiple(addList);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false, "ErrorException");
            }
        }

        public async Task<OperationResult> PutData(SalaryItemAndAmountSettings_Update input)
        {
            var predicate = PredicateBuilder.New<HRMS_Sal_Item_Settings>(true);
            if (string.IsNullOrWhiteSpace(input.Param.Factory)
             || string.IsNullOrWhiteSpace(input.Param.Permission_Group)
             || string.IsNullOrWhiteSpace(input.Param.Salary_Type)
             || string.IsNullOrWhiteSpace(input.Param.Salary_Days)
             || string.IsNullOrWhiteSpace(input.Param.Effective_Month_Str)
             || !DateTime.TryParseExact(input.Param.Effective_Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime effectiveMonthValue)
            )
                return new OperationResult(false, "InvalidInput");
            try
            {
                predicate.And(x =>
                    x.Factory == input.Param.Factory &&
                    x.Permission_Group == input.Param.Permission_Group &&
                    x.Salary_Type == input.Param.Salary_Type &&
                    x.Effective_Month.Date == effectiveMonthValue.Date
                );
                var removeList = await _repositoryAccessor.HRMS_Sal_Item_Settings.FindAll(predicate).ToListAsync();
                if (!removeList.Any())
                    return new OperationResult(false, "NotExitedData");
                List<HRMS_Sal_Item_Settings> addList = new();
                foreach (var item in input.Data)
                {
                    if (string.IsNullOrWhiteSpace(item.Seq)
                     || string.IsNullOrWhiteSpace(item.Salary_Item)
                     || string.IsNullOrWhiteSpace(item.Kind)
                     || string.IsNullOrWhiteSpace(item.Insurance)
                     || string.IsNullOrWhiteSpace(item.Amount)
                     || string.IsNullOrWhiteSpace(item.Update_By)
                     || string.IsNullOrWhiteSpace(item.Update_Time_Str)
                     || !DateTime.TryParseExact(item.Update_Time_Str, "yyyy/MM/dd HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime updateTimeValue)
                    )
                        return new OperationResult(false, "InvalidInput");
                    HRMS_Sal_Item_Settings addData = new()
                    {
                        Factory = input.Param.Factory,
                        Permission_Group = input.Param.Permission_Group,
                        Salary_Type = input.Param.Salary_Type,
                        Effective_Month = effectiveMonthValue,
                        Salary_Days = short.Parse(input.Param.Salary_Days),
                        Seq = int.Parse(item.Seq),
                        Salary_Item = item.Salary_Item,
                        Kind = item.Kind,
                        Insurance = item.Insurance,
                        Amount = int.Parse(item.Amount),
                        Update_By = item.Update_By,
                        Update_Time = updateTimeValue,
                    };
                    addList.Add(addData);
                }
                _repositoryAccessor.HRMS_Sal_Item_Settings.RemoveMultiple(removeList);
                await _repositoryAccessor.Save();

                _repositoryAccessor.HRMS_Sal_Item_Settings.AddMultiple(addList);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false, "ErrorException");
            }
        }

        public async Task<OperationResult> DeleteData(SalaryItemAndAmountSettings_MainData data)
        {
            var predicate = PredicateBuilder.New<HRMS_Sal_Item_Settings>(true);
            if (string.IsNullOrWhiteSpace(data.Factory)
             || string.IsNullOrWhiteSpace(data.Permission_Group)
             || string.IsNullOrWhiteSpace(data.Salary_Type)
             || string.IsNullOrWhiteSpace(data.Salary_Item)
             || string.IsNullOrWhiteSpace(data.Effective_Month)
             || !DateTime.TryParseExact(data.Effective_Month, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime effectiveMonthValue))
                return new OperationResult(false, "InvalidInput");
            predicate.And(x =>
                x.Factory == data.Factory &&
                x.Permission_Group == data.Permission_Group &&
                x.Salary_Type == data.Salary_Type &&
                x.Salary_Item == data.Salary_Item &&
                x.Effective_Month.Date == effectiveMonthValue.Date
            );
            var removeData = await _repositoryAccessor.HRMS_Sal_Item_Settings.FirstOrDefaultAsync(predicate);
            if (removeData == null)
                return new OperationResult(false, "NotExitedData");
            try
            {
                _repositoryAccessor.HRMS_Sal_Item_Settings.Remove(removeData);
                return new OperationResult(await _repositoryAccessor.Save());
            }
            catch (Exception)
            {
                return new OperationResult(false, "ErrorException");
            }
        }
    }
}
