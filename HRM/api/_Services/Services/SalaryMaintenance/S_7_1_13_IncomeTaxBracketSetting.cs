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
    public class S_7_1_13_IncomeTaxBracketSetting : BaseServices, I_7_1_13_IncomeTaxBracketSetting
    {
        public S_7_1_13_IncomeTaxBracketSetting(DBContext dbContext) : base(dbContext)
        {
        }

        #region Get Pagination
        public async Task<PaginationUtility<IncomeTaxBracketSettingMain>> GetDataPagination(PaginationParam pagination, IncomeTaxBracketSettingParam param)
        {
            var pred = PredicateBuilder.New<HRMS_Sal_Taxbracket>(x => x.Nation == param.Nationality);

            if (!string.IsNullOrWhiteSpace(param.Tax_Code))
                pred.And(x => x.Tax_Code == param.Tax_Code);
            if (!string.IsNullOrWhiteSpace(param.Start_Effective_Month) && !string.IsNullOrWhiteSpace(param.End_Effective_Month))
                pred.And(x => x.Effective_Month.Date >= Convert.ToDateTime(param.Start_Effective_Month).Date
                           && x.Effective_Month.Date <= Convert.ToDateTime(param.End_Effective_Month).Date);

            var Type = _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.TaxCode, true)
                .ToList();

            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language
                .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.TaxCode && x.Language_Code.ToLower() == param.Language.ToLower(), true)
                .ToList();

            var HSTB = await _repositoryAccessor.HRMS_Sal_Taxbracket.FindAll(true).ToListAsync();

            var data = HSTB
                .Where(pred)
                .Select(x => new IncomeTaxBracketSettingMain
                {
                    Nation = x.Nation,
                    Effective_Month = x.Effective_Month,
                    Tax_Code = x.Tax_Code,
                    Tax_Code_Name = HBCL.FirstOrDefault(y => y.Code == x.Tax_Code) != null ? HBCL.FirstOrDefault(y => y.Code == x.Tax_Code).Code_Name : Type.FirstOrDefault(y => y.Code == x.Tax_Code).Code_Name,
                    Type = Type.FirstOrDefault(y => y.Code == x.Tax_Code).Char1,
                    Tax_Level = x.Tax_Level,
                    Income_Start = x.Income_Start,
                    Income_End = x.Income_End,
                    Rate = x.Rate,
                    Deduction = x.Deduction,
                    Update_By = x.Update_By,
                    Update_Time = x.Update_Time,
                    Is_Disabled = HSTB.Any(y => y.Nation == x.Nation
                                          && y.Tax_Code == x.Tax_Code
                                          && y.Effective_Month.Date > x.Effective_Month.Date)
                })
                .OrderBy(x => x.Effective_Month)
                .ThenBy(x => x.Tax_Code)
                .ThenBy(x => x.Tax_Level)
                .ToList();

            return PaginationUtility<IncomeTaxBracketSettingMain>.Create(data, pagination.PageNumber, pagination.PageSize);
        }
        #endregion

        #region GetDetail
        public async Task<IncomeTaxBracketSettingDto> GetDetail(IncomeTaxBracketSettingDto dto)
        {
            var data = await _repositoryAccessor.HRMS_Sal_Taxbracket
                .FindAll(x => x.Nation == dto.Nation
                            && x.Effective_Month.Date == Convert.ToDateTime(dto.Effective_Month_Str).Date
                            && x.Tax_Code == dto.Tax_Code).ToListAsync();

            if (data is null)
                return dto;
            dto.SubData = data.Select(x => new IncomeTaxBracketSetting_SubData()
            {
                Tax_Level = x.Tax_Level,
                Income_Start = x.Income_Start,
                Income_End = x.Income_End,
                Rate = x.Rate,
                Deduction = x.Deduction,
                Update_By = x.Update_By,
                Update_Time = x.Update_Time,
                Update_Time_Str = x.Update_Time.ToString("yyyy/MM/dd HH:mm:ss")
            }).ToList();
            return dto;
        }
        #endregion

        #region Create
        public async Task<OperationResult> Create(IncomeTaxBracketSettingDto dto, string userName)
        {
            List<HRMS_Sal_Taxbracket> data = new();
            foreach (var item in dto.SubData)
            {
                if (await _repositoryAccessor.HRMS_Sal_Taxbracket
                    .AnyAsync(x => x.Nation == dto.Nation
                           && x.Effective_Month.Date == Convert.ToDateTime(dto.Effective_Month_Str).Date
                           && x.Tax_Code == dto.Tax_Code
                           && x.Tax_Level == item.Tax_Level))
                    return new OperationResult(false, "DuplicateInput");

                DateTime.TryParseExact(item.Update_Time_Str, "yyyy/MM/dd HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime updateTimeValue);
                data.Add(new HRMS_Sal_Taxbracket()
                {
                    Nation = dto.Nation,
                    Effective_Month = Convert.ToDateTime(dto.Effective_Month_Str),
                    Tax_Code = dto.Tax_Code,
                    Tax_Level = item.Tax_Level,
                    Income_Start = item.Income_Start,
                    Income_End = item.Income_End,
                    Rate = item.Rate,
                    Deduction = item.Deduction,
                    Update_By = userName,
                    Update_Time = updateTimeValue,
                });
            }

            try
            {
                _repositoryAccessor.HRMS_Sal_Taxbracket.AddMultiple(data);
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
        public async Task<OperationResult> Update(IncomeTaxBracketSettingDto dto, string userName)
        {
            var dataCheck = await _repositoryAccessor.HRMS_Sal_Taxbracket
                .FindAll(x => x.Nation == dto.Nation
                            && x.Effective_Month.Date == Convert.ToDateTime(dto.Effective_Month_Str).Date
                            && x.Tax_Code == dto.Tax_Code).ToListAsync();
            if (dataCheck is null)
                return new OperationResult(false, "NotExitedData");

            _repositoryAccessor.HRMS_Sal_Taxbracket.RemoveMultiple(dataCheck);

            List<HRMS_Sal_Taxbracket> data = new();
            foreach (var item in dto.SubData)
            {
                DateTime.TryParseExact(item.Update_Time_Str, "yyyy/MM/dd HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime updateTimeValue);
                data.Add(new HRMS_Sal_Taxbracket()
                {
                    Nation = dto.Nation,
                    Effective_Month = Convert.ToDateTime(dto.Effective_Month_Str),
                    Tax_Code = dto.Tax_Code,
                    Tax_Level = item.Tax_Level,
                    Income_Start = item.Income_Start,
                    Income_End = item.Income_End,
                    Rate = item.Rate,
                    Deduction = item.Deduction,
                    Update_By = userName,
                    Update_Time = updateTimeValue,
                });
            }

            try
            {
                _repositoryAccessor.HRMS_Sal_Taxbracket.AddMultiple(data);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false);
            }
        }
        #endregion


        #region Delete
        public async Task<OperationResult> Delete(IncomeTaxBracketSettingMain dto)
        {
            var data = await _repositoryAccessor.HRMS_Sal_Taxbracket
               .FirstOrDefaultAsync(x => x.Nation == dto.Nation
                           && x.Effective_Month.Date == Convert.ToDateTime(dto.Effective_Month_Str).Date
                           && x.Tax_Code == dto.Tax_Code
                           && x.Tax_Level == dto.Tax_Level);

            if (data is null)
                return new OperationResult(false, "NotExitedData");

            try
            {
                _repositoryAccessor.HRMS_Sal_Taxbracket.Remove(data);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false);
            }
        }
        #endregion

        public async Task<List<KeyValuePair<string, string>>> GetListNationality(string Language)
        {
            return await GetDataBasicCode(BasicCodeTypeConstant.Nationality, Language);
        }

        public async Task<List<KeyValuePair<string, IncomeTaxBracketSettingSub>>> GetListTaxCode(string Language)
        {
            return await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.TaxCode && x.IsActive, true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == Language.ToLower(), true),
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (x, y) => new { basicCode = x, basicCode_lang = y })
                .SelectMany(x => x.basicCode_lang.DefaultIfEmpty(),
                    (x, y) => new { x = x.basicCode, basicCode_lang = y })
                .Select(x => new KeyValuePair<string, IncomeTaxBracketSettingSub>(
                    x.x.Code,
                    new IncomeTaxBracketSettingSub()
                    {
                        Tax_Code = $"{x.x.Code} - {(x.basicCode_lang != null ? x.basicCode_lang.Code_Name : x.x.Code_Name)}",
                        Type = x.x.Char1
                    })
                ).ToListAsync();
        }
        public async Task<OperationResult> IsDuplicatedData(string Nation, string Tax_Code, int Tax_Level, string Effective_Month)
        {
            var result = await _repositoryAccessor.HRMS_Sal_Taxbracket.AnyAsync(x => x.Nation == Nation
                            && x.Effective_Month.Date == Convert.ToDateTime(Effective_Month).Date
                            && x.Tax_Code == Tax_Code
                            && x.Tax_Level == Tax_Level);
            return new OperationResult(result);
        }
    }
}