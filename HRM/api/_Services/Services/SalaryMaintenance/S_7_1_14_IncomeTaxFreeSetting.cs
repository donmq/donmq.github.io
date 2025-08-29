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
    public class S_7_1_14_IncomeTaxFreeSetting : BaseServices, I_7_1_14_IncomeTaxFreeSetting
    {
        public S_7_1_14_IncomeTaxFreeSetting(DBContext dbContext) : base(dbContext)
        {
        }

        #region GetDataPagination
        public async Task<PaginationUtility<IncomeTaxFreeSetting_MainData>> GetDataPagination(PaginationParam pagination, IncomeTaxFreeSetting_MainParam param)
        {
            var pred = PredicateBuilder.New<HRMS_Sal_TaxFree>(x => x.Factory == param.Factory);

            if (!string.IsNullOrWhiteSpace(param.Type))
                pred.And(x => x.Type == param.Type);

            if (!string.IsNullOrWhiteSpace(param.Salary_Type))
                pred.And(x => x.Salary_Type == param.Salary_Type);

            if (!string.IsNullOrWhiteSpace(param.Start_Effective_Month) && !string.IsNullOrWhiteSpace(param.End_Effective_Month))
                pred.And(x => x.Effective_Month >= Convert.ToDateTime(param.Start_Effective_Month)
                           && x.Effective_Month <= Convert.ToDateTime(param.End_Effective_Month));

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

            var HSTF = await _repositoryAccessor.HRMS_Sal_TaxFree.FindAll(true).ToListAsync();

            var data = HSTF
                .Where(pred)
                .Select(x => new IncomeTaxFreeSetting_MainData
                {
                    Factory = x.Factory,
                    Type = x.Type,
                    Type_Name = x.Type + " - " + codLang.FirstOrDefault(y => y.Type_Seq == BasicCodeTypeConstant.Type && y.Code == x.Type).Code_Name,
                    Salary_Type = x.Salary_Type,
                    Salary_Type_Name = x.Salary_Type + " - " + codLang.FirstOrDefault(y => y.Type_Seq == BasicCodeTypeConstant.SalaryType && y.Code == x.Salary_Type).Code_Name,
                    Effective_Month = x.Effective_Month.ToString("yyyy/MM/dd"),
                    Amount = x.Amount,
                    Update_By = x.Update_By,
                    Update_Time = x.Update_Time,
                    Is_Disabled = HSTF.Any(y => y.Factory == x.Factory
                                           && y.Type == x.Type
                                           && y.Salary_Type == x.Salary_Type
                                           && y.Effective_Month > x.Effective_Month)
                })
                .OrderBy(x => x.Effective_Month)
                .ThenBy(x => x.Type)
                .ToList();

            return PaginationUtility<IncomeTaxFreeSetting_MainData>.Create(data, pagination.PageNumber, pagination.PageSize);
        }
        #endregion

        #region GetDetail
        public async Task<List<IncomeTaxFreeSetting_SubData>> GetDetail(IncomeTaxFreeSetting_SubParam dto)
        {

            var HSTF = _repositoryAccessor.HRMS_Sal_TaxFree
                .FindAll(x => x.Factory == dto.Factory && x.Salary_Type == dto.Salary_Type);
            var data = await HSTF
                .Where(x => x.Effective_Month.Date == Convert.ToDateTime(dto.Effective_Month).Date)
                .ToListAsync();
            if (data is null)
                return new();
            var result = data.Select(x => new IncomeTaxFreeSetting_SubData()
            {
                Type = x.Type,
                Amount = x.Amount.ToString(),
                Update_By = x.Update_By,
                Update_Time = x.Update_Time,
                Update_Time_Str = x.Update_Time.ToString("yyyy/MM/dd HH:mm:ss"),
                Is_Disabled_Edit = HSTF.Any(y => y.Type == x.Type && y.Effective_Month > x.Effective_Month)
            }).ToList();
            return result;
        }
        #endregion

        public async Task<OperationResult> IsDuplicatedData(IncomeTaxFreeSetting_SubParam param, string userName)
        {
            var result = new List<IncomeTaxFreeSetting_SubData>();
            var HSTF = await _repositoryAccessor.HRMS_Sal_TaxFree
                .FindAll(x => x.Factory == param.Factory
                           && x.Salary_Type == param.Salary_Type
                           && x.Effective_Month.Date <= Convert.ToDateTime(param.Effective_Month_Str).Date)
                .ToListAsync();
            if (!HSTF.Any())
                return new OperationResult(false, result);

            var selected_HSTF = HSTF.FindAll(x => x.Effective_Month.Date == Convert.ToDateTime(param.Effective_Month_Str).Date);
            if (selected_HSTF.Any())
                return new OperationResult(true);
            var now = DateTime.Now;
            var maxEffectiveMonth = HSTF.Max(x => x.Effective_Month);
            result = HSTF
            .FindAll(x => x.Effective_Month.Date == maxEffectiveMonth.Date)
            .Select(x => new IncomeTaxFreeSetting_SubData()
            {
                Type = x.Type,
                Amount = x.Amount.ToString(),
                Update_By = userName,
                Update_Time = now,
                Is_Duplicate = false
            }).ToList();

            return new OperationResult(false, result);
        }

        #region Create
        public async Task<OperationResult> Create(IncomeTaxFreeSetting_Form dto)
        {
            var pred = PredicateBuilder.New<HRMS_Sal_TaxFree>(true);
            if (string.IsNullOrWhiteSpace(dto.Param.Factory)
             || string.IsNullOrWhiteSpace(dto.Param.Salary_Type)
             || string.IsNullOrWhiteSpace(dto.Param.Effective_Month_Str)
             || !DateTime.TryParseExact(dto.Param.Effective_Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime effectiveMonthValue)
            )
                return new OperationResult(false, "InvalidInput");

            try
            {
                pred.And(x => x.Factory == dto.Param.Factory
                           && x.Salary_Type == dto.Param.Salary_Type
                           && x.Effective_Month.Date == effectiveMonthValue.Date);

                var HSTF = await _repositoryAccessor.HRMS_Sal_TaxFree.FindAll(pred).ToListAsync();
                List<HRMS_Sal_TaxFree> addList = new();

                foreach (var item in dto.Data)
                {
                    if (string.IsNullOrWhiteSpace(item.Type)
                     || string.IsNullOrWhiteSpace(item.Amount)
                     || string.IsNullOrWhiteSpace(item.Update_By)
                     || string.IsNullOrWhiteSpace(item.Update_Time_Str)
                     || !DateTime.TryParseExact(item.Update_Time_Str, "yyyy/MM/dd HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime updateTimeValue)
                    )
                        return new OperationResult(false, "InvalidInput");

                    if (HSTF.Any(x => x.Type == item.Type))
                        return new OperationResult(false, "AlreadyExitedData");

                    HRMS_Sal_TaxFree addData = new()
                    {
                        Factory = dto.Param.Factory,
                        Effective_Month = effectiveMonthValue,
                        Salary_Type = dto.Param.Salary_Type,
                        Type = item.Type,
                        Amount = int.Parse(item.Amount),
                        Update_By = item.Update_By,
                        Update_Time = updateTimeValue
                    };

                    addList.Add(addData);
                }

                _repositoryAccessor.HRMS_Sal_TaxFree.AddMultiple(addList);
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
        public async Task<OperationResult> Update(IncomeTaxFreeSetting_Form dto)
        {
            var pred = PredicateBuilder.New<HRMS_Sal_TaxFree>(true);
            if (string.IsNullOrWhiteSpace(dto.Param.Factory)
             || string.IsNullOrWhiteSpace(dto.Param.Salary_Type)
             || string.IsNullOrWhiteSpace(dto.Param.Effective_Month_Str)
             || !DateTime.TryParseExact(dto.Param.Effective_Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime effectiveMonthValue)
            )
                return new OperationResult(false, "InvalidInput");

            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                pred.And(x => x.Factory == dto.Param.Factory
                           && x.Salary_Type == dto.Param.Salary_Type
                           && x.Effective_Month.Date == effectiveMonthValue.Date);

                var removeList = await _repositoryAccessor.HRMS_Sal_TaxFree.FindAll(pred).ToListAsync();
                if (!removeList.Any())
                    return new OperationResult(false, "NotExitedData");

                List<HRMS_Sal_TaxFree> addList = new();

                foreach (var item in dto.Data)
                {
                    if (string.IsNullOrWhiteSpace(item.Type)
                     || string.IsNullOrWhiteSpace(item.Amount)
                     || string.IsNullOrWhiteSpace(item.Update_By)
                     || string.IsNullOrWhiteSpace(item.Update_Time_Str)
                     || !DateTime.TryParseExact(item.Update_Time_Str, "yyyy/MM/dd HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime updateTimeValue)
                    )
                        return new OperationResult(false, "InvalidInput");

                    HRMS_Sal_TaxFree addData = new()
                    {
                        Factory = dto.Param.Factory,
                        Effective_Month = effectiveMonthValue,
                        Salary_Type = dto.Param.Salary_Type,
                        Type = item.Type,
                        Amount = int.Parse(item.Amount),
                        Update_By = item.Update_By,
                        Update_Time = updateTimeValue
                    };

                    addList.Add(addData);
                }

                _repositoryAccessor.HRMS_Sal_TaxFree.RemoveMultiple(removeList);
                await _repositoryAccessor.Save();

                _repositoryAccessor.HRMS_Sal_TaxFree.AddMultiple(addList);
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
        public async Task<OperationResult> Delete(IncomeTaxFreeSetting_MainData dto)
        {
            var data = await _repositoryAccessor.HRMS_Sal_TaxFree
                .FirstOrDefaultAsync(x => x.Factory == dto.Factory
                            && x.Effective_Month.Date == Convert.ToDateTime(dto.Effective_Month).Date
                            && x.Salary_Type == dto.Salary_Type
                            && x.Type == dto.Type);
            if (data is null)
                return new OperationResult(false, "DataNotFound");

            try
            {
                _repositoryAccessor.HRMS_Sal_TaxFree.Remove(data);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false);
            }
        }
        #endregion

        #region Get List
        public async Task<List<KeyValuePair<string, string>>> GetListFactoryByUser(string language, List<string> roleList)
        {
            var factoriesByAccount = await Queryt_Factory_AddList(roleList);
            var factories = await GetDataBasicCode(BasicCodeTypeConstant.Factory, language);

            return factories.IntersectBy(factoriesByAccount, x => x.Key).ToList();
        }

        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string language)
        {
            return await GetDataBasicCode(BasicCodeTypeConstant.Factory, language);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListType(string language)
        {
            return await GetDataBasicCode(BasicCodeTypeConstant.Type, language);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListSalaryType(string language)
        {
            return await GetDataBasicCode(BasicCodeTypeConstant.SalaryType, language);
        }
        #endregion
    }
}