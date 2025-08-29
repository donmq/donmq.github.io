
using System.Globalization;
using API.Data;
using API._Services.Interfaces.CompulsoryInsuranceManagement;
using API.DTOs.CompulsoryInsuranceManagement;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.CompulsoryInsuranceManagement
{
    public class S_6_1_2_ContributionRateSetting : BaseServices, I_6_1_2_ContributionRateSetting
    {
        public S_6_1_2_ContributionRateSetting(DBContext dbContext) : base(dbContext)
        {
        }

        #region Create
        public async Task<OperationResult> Create(ContributionRateSettingForm data)
        {
            var pred = PredicateBuilder.New<HRMS_Ins_Rate_Setting>(true);
            if (string.IsNullOrWhiteSpace(data.Param.Factory)
             || string.IsNullOrWhiteSpace(data.Param.Permission_Group)
             || string.IsNullOrWhiteSpace(data.Param.Effective_Month_Str)
             || !DateTime.TryParseExact(data.Param.Effective_Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime effectiveMonthValue)
            )
                return new OperationResult(false, "InvalidInput");

            try
            {
                pred.And(x => x.Factory == data.Param.Factory
                           && x.Permission_Group == data.Param.Permission_Group
                           && x.Effective_Month.Date == effectiveMonthValue.Date);

                var HIRS = await _repositoryAccessor.HRMS_Ins_Rate_Setting.FindAll(pred).ToListAsync();
                List<HRMS_Ins_Rate_Setting> addList = new();

                foreach (var item in data.SubData)
                {
                    if (string.IsNullOrWhiteSpace(item.Insurance_Type)
                     || string.IsNullOrWhiteSpace(item.Employer_Rate.ToString())
                     || string.IsNullOrWhiteSpace(item.Employee_Rate.ToString())
                     || string.IsNullOrWhiteSpace(item.Update_Time_Str)
                     || !DateTime.TryParseExact(item.Update_Time_Str, "yyyy/MM/dd HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime updateTimeValue)
                    )
                        return new OperationResult(false, "InvalidInput");

                    if (HIRS.Any(x => x.Insurance_Type == item.Insurance_Type))
                        return new OperationResult(false, "AlreadyExitedData");

                    HRMS_Ins_Rate_Setting addData = new()
                    {
                        Factory = data.Param.Factory,
                        Permission_Group = data.Param.Permission_Group,
                        Effective_Month = effectiveMonthValue,
                        Insurance_Type = item.Insurance_Type,
                        Employer_Rate = item.Employer_Rate,
                        Employee_Rate = item.Employee_Rate,
                        Update_By = item.Update_By,
                        Update_Time = updateTimeValue
                    };

                    addList.Add(addData);
                }

                _repositoryAccessor.HRMS_Ins_Rate_Setting.AddMultiple(addList);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false);
            }
        }
        #endregion
        #region Update

        public async Task<OperationResult> Update(ContributionRateSettingForm data)
        {
            var pred = PredicateBuilder.New<HRMS_Ins_Rate_Setting>(true);
            if (string.IsNullOrWhiteSpace(data.Param.Factory)
             || string.IsNullOrWhiteSpace(data.Param.Permission_Group)
             || string.IsNullOrWhiteSpace(data.Param.Effective_Month_Str)
             || !DateTime.TryParseExact(data.Param.Effective_Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime effectiveMonthValue)
            )
                return new OperationResult(false, "InvalidInput");

            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                pred.And(x => x.Factory == data.Param.Factory
                           && x.Permission_Group == data.Param.Permission_Group
                           && x.Effective_Month.Date == effectiveMonthValue.Date);

                var removeList = await _repositoryAccessor.HRMS_Ins_Rate_Setting.FindAll(pred).ToListAsync();
                if (!removeList.Any())
                    return new OperationResult(false, "Not Exited Data");

                List<HRMS_Ins_Rate_Setting> addList = new();

                foreach (var item in data.SubData)
                {
                    if (string.IsNullOrWhiteSpace(item.Insurance_Type)
                     || !decimal.TryParse(item.Employer_Rate.ToString(), out decimal employerRate)
                     || !decimal.TryParse(item.Employee_Rate.ToString(), out decimal employeeRate)
                     || string.IsNullOrWhiteSpace(item.Update_Time_Str)
                     || !DateTime.TryParseExact(item.Update_Time_Str, "yyyy/MM/dd HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime updateTimeValue)
                    )
                        return new OperationResult(false, "Invalid Input");

                    HRMS_Ins_Rate_Setting addData = new()
                    {
                        Factory = data.Param.Factory,
                        Permission_Group = data.Param.Permission_Group,
                        Effective_Month = effectiveMonthValue,
                        Insurance_Type = item.Insurance_Type,
                        Employer_Rate = item.Employer_Rate,
                        Employee_Rate = item.Employee_Rate,
                        Update_By = item.Update_By,
                        Update_Time = updateTimeValue
                    };

                    addList.Add(addData);
                }

                _repositoryAccessor.HRMS_Ins_Rate_Setting.RemoveMultiple(removeList);

                _repositoryAccessor.HRMS_Ins_Rate_Setting.AddMultiple(addList);
                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false);
            }
        }
        #endregion
        #region Delete
        public async Task<OperationResult> Delete(ContributionRateSettingDto data)
        {
            var item = await _repositoryAccessor.HRMS_Ins_Rate_Setting
               .FirstOrDefaultAsync(x => x.Factory == data.Factory
                           && x.Effective_Month.Date == Convert.ToDateTime(data.Effective_Month_Str).Date
                           && x.Permission_Group == data.Permission_Group
                           && x.Insurance_Type == data.Insurance_Type);

            if (item == null)
                return new OperationResult(false, "Data not exist");
            _repositoryAccessor.HRMS_Ins_Rate_Setting.Remove(item);
            if (await _repositoryAccessor.Save())
                return new OperationResult(true, "Delete Successfully");
            return new OperationResult(false, "Delete failed");
        }
        #endregion
        #region GetData
        public async Task<PaginationUtility<ContributionRateSettingDto>> GetDataPagination(PaginationParam pagination, ContributionRateSettingParam param)
        {
            var pred = PredicateBuilder.New<HRMS_Ins_Rate_Setting>(true);

            if (!string.IsNullOrWhiteSpace(param.Factory))
                pred = pred.And(x => x.Factory == param.Factory);

            if (param.Permission_Group != null)
                pred.And(x => param.Permission_Group.Contains(x.Permission_Group));

            if (!string.IsNullOrWhiteSpace(param.Effective_Month))
                pred = pred.And(x => x.Effective_Month == Convert.ToDateTime(param.Effective_Month));

            var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll();
            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower());
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

            var data = _repositoryAccessor.HRMS_Ins_Rate_Setting.FindAll(pred);
            var result = data.Select(x => new ContributionRateSettingDto
            {
                Factory = x.Factory,
                Effective_Month = x.Effective_Month,
                Permission_Group = x.Permission_Group,
                Permission_Group_Name = x.Permission_Group + " - " + codLang
                    .FirstOrDefault(y => y.Type_Seq == BasicCodeTypeConstant.PermissionGroup && y.Code == x.Permission_Group).Code_Name,
                Insurance_Type = x.Insurance_Type,
                Insurance_Type_Name = x.Insurance_Type + " - " + codLang
                    .FirstOrDefault(y => y.Type_Seq == BasicCodeTypeConstant.InsuranceType && y.Code == x.Insurance_Type).Code_Name,
                Employer_Rate = x.Employer_Rate.ToString("F3", CultureInfo.InvariantCulture),
                Employee_Rate = x.Employee_Rate.ToString("F3", CultureInfo.InvariantCulture),
                Update_By = x.Update_By,
                Update_Time = x.Update_Time.ToString("yyyy/MM/dd HH:mm:ss")
            })
            .OrderBy(x => x.Permission_Group)
            .ThenBy(x => x.Insurance_Type)
            .ThenByDescending(x => x.Effective_Month);
            return await PaginationUtility<ContributionRateSettingDto>.CreateAsync(result, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<List<ContributionRateSettingSubData>> GetDetail(ContributionRateSettingSubParam param)
        {
            var data = await _repositoryAccessor.HRMS_Ins_Rate_Setting
                .FindAll(x => x.Factory == param.Factory
                    && x.Effective_Month == Convert.ToDateTime(param.Effective_Month_Str)
                    && x.Permission_Group == param.Permission_Group)
                .Select(x => new ContributionRateSettingSubData
                {
                    Insurance_Type = x.Insurance_Type,
                    Employer_Rate = x.Employer_Rate,
                    Employee_Rate = x.Employee_Rate,
                    Update_By = x.Update_By,
                    Update_Time = x.Update_Time,
                    Update_Time_Str = x.Update_Time.ToString("yyyy/MM/dd HH:mm:ss")
                })
                .ToListAsync();
            return data;
        }
        #endregion
        #region GetList
        public async Task<List<KeyValuePair<string, string>>> GetListFactory(List<string> roleList, string language)
        {
            var factoriesByAccount = await Queryt_Factory_AddList(roleList);
            var factories = await GetDataBasicCode(BasicCodeTypeConstant.Factory, language);

            return factories.IntersectBy(factoriesByAccount, x => x.Key).ToList();
        }

        public async Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language)
        {
            var permissionGroupByFactory = await Query_Permission_Group_List(factory);
            var permissionGroup = await GetDataBasicCode(BasicCodeTypeConstant.PermissionGroup, language);
            return permissionGroup.IntersectBy(permissionGroupByFactory, x => x.Key).ToList();
        }

        public async Task<List<KeyValuePair<string, string>>> GetListInsuranceType(string language)
        {
            return await GetDataBasicCode(BasicCodeTypeConstant.InsuranceType, language);
        }
        #endregion
        #region Check Data
        public async Task<ContributionRateSettingCheckEffectiveMonth> CheckEffectiveMonth(ContributionRateSettingSubParam param)
        {
            ContributionRateSettingCheckEffectiveMonth result = new();
            var data = await _repositoryAccessor.HRMS_Ins_Rate_Setting
                .FindAll(x => x.Factory == param.Factory
                    && x.Permission_Group == param.Permission_Group
                    && x.Insurance_Type == param.Insurance_Type).ToListAsync();

            result.checkEffective_Month = data.Any(x => x.Effective_Month == Convert.ToDateTime(param.Effective_Month_Str));

            if (!result.checkEffective_Month)
            {
                var validData = data.Where(x => x.Effective_Month <= Convert.ToDateTime(param.Effective_Month_Str)).ToList();

                if (validData.Any())
                {
                    var maxEffectiveMonth = validData.Max(x => x.Effective_Month);
                    result.DataDefault = validData.Where(x => x.Effective_Month == maxEffectiveMonth)
                        .Select(x => new ContributionRateSettingSubData
                        {
                            Employer_Rate = x.Employer_Rate,
                            Employee_Rate = x.Employee_Rate
                        }).FirstOrDefault();
                }
                else
                {
                    result.DataDefault = new ContributionRateSettingSubData
                    {
                        Employer_Rate = 0,
                        Employee_Rate = 0
                    };
                }
            }
            return result;
        }

        public async Task<bool> CheckSearch(ContributionRateSettingParam param)
        {
            return await _repositoryAccessor.HRMS_Sal_Monthly
                .AnyAsync(x => x.Factory == param.Factory
                    && param.Permission_Group.Contains(x.Permission_Group)
                    && x.Sal_Month >= Convert.ToDateTime(param.Effective_Month)
                    && x.Lock == "Y");
        }
        #endregion
    }
}