using System.Globalization;
using API.Data;
using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using API.Helper.Constant;
using API.Helper.Utilities;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.SalaryMaintenance
{
    public class S_7_1_2_MonthlyExchangeRateSetting : BaseServices, I_7_1_2_MonthlyExchangeRateSetting
    {
        public S_7_1_2_MonthlyExchangeRateSetting(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult> DeleteData(MonthlyExchangeRateSetting_Main data)
        {
            var predicate = PredicateBuilder.New<HRMS_Sal_Currency_Rate>(true);
            if (string.IsNullOrWhiteSpace(data.Kind)
             || string.IsNullOrWhiteSpace(data.Currency)
             || string.IsNullOrWhiteSpace(data.Exchange_Currency)
             || string.IsNullOrWhiteSpace(data.Rate_Month)
             || !DateTime.TryParseExact(data.Rate_Month, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime rateMonthValue))
                return new OperationResult(false, "InvalidInput");
            predicate.And(x =>
                x.Kind == data.Kind &&
                x.Currency == data.Currency &&
                x.Exchange_Currency == data.Exchange_Currency &&
                x.Rate_Month.Date == rateMonthValue.Date
            );
            var removeData = await _repositoryAccessor.HRMS_Sal_Currency_Rate.FirstOrDefaultAsync(predicate);
            if (removeData == null)
                return new OperationResult(false, "NotExitedData");
            try
            {
                _repositoryAccessor.HRMS_Sal_Currency_Rate.Remove(removeData);
                return new OperationResult(await _repositoryAccessor.Save());
            }
            catch (Exception)
            {
                return new OperationResult(false, "ErrorException");
            }
        }

        public async Task<List<KeyValuePair<string, string>>> GetDropDownList(MonthlyExchangeRateSetting_Param param, List<string> roleList)
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
            result.AddRange(data.Where(x => x.hbc.Type_Seq == BasicCodeTypeConstant.Kind).Select(x => new KeyValuePair<string, string>("KI", $"{x.hbc.Code}- {(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            result.AddRange(data.Where(x => x.hbc.Type_Seq == BasicCodeTypeConstant.Currency).Select(x => new KeyValuePair<string, string>("CR", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            return result;
        }

        public async Task<PaginationUtility<MonthlyExchangeRateSetting_Main>> GetSearchDetail(PaginationParam paginationParams, MonthlyExchangeRateSetting_Param searchParam)
        {
            var predicate = PredicateBuilder.New<HRMS_Sal_Currency_Rate>(x => x.Rate_Month == Convert.ToDateTime(searchParam.Rate_Month_Str));
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
            var data = _repositoryAccessor.HRMS_Sal_Currency_Rate.FindAll(predicate);
            var result = data.Select(x => new MonthlyExchangeRateSetting_Main
            {
                Rate_Month = x.Rate_Month.ToString("yyyy/MM/dd"),
                Kind = x.Kind,
                Kind_Name = x.Kind + " - " + codLang.FirstOrDefault(y => y.Type_Seq == BasicCodeTypeConstant.Kind && y.Code == x.Kind).Code_Name,
                Currency = x.Currency,
                Currency_Name = x.Currency + " - " + codLang.FirstOrDefault(y => y.Type_Seq == BasicCodeTypeConstant.Currency && y.Code == x.Currency).Code_Name,
                Exchange_Currency = x.Exchange_Currency,
                Exchange_Currency_Name = x.Exchange_Currency + " - " + codLang.FirstOrDefault(y => y.Type_Seq == BasicCodeTypeConstant.Currency && y.Code == x.Exchange_Currency).Code_Name,
                Rate = x.Rate.ToString("#.00000"),
                Rate_Date = x.Rate_Date.ToString("yyyy/MM/dd"),
                Update_By = x.Update_By,
                Update_Time = x.Update_Time.ToString("yyyy/MM/dd HH:mm:ss")
            });
            return await PaginationUtility<MonthlyExchangeRateSetting_Main>.CreateAsync(result, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<OperationResult> IsDuplicatedData(MonthlyExchangeRateSetting_Main param)
        {
            var result = await _repositoryAccessor.HRMS_Sal_Currency_Rate.AnyAsync(x =>
                x.Rate_Month.Date == Convert.ToDateTime(param.Rate_Month_Str) &&
                x.Kind == param.Kind &&
                x.Currency == param.Currency &&
                x.Exchange_Currency == param.Exchange_Currency);
            return new OperationResult(result);
        }

        public async Task<OperationResult> IsExistedData(MonthlyExchangeRateSetting_Main param)
        {
            var HSCR = await _repositoryAccessor.HRMS_Sal_Currency_Rate.FindAll(x => x.Rate_Month.Date == Convert.ToDateTime(param.Rate_Month).Date
            ).ToListAsync();
            if (HSCR.Any())
            {
                var data = HSCR.Select(x => new MonthlyExchangeRateSetting_Main
                {
                    Kind = x.Kind,
                    Currency = x.Currency,
                    Exchange_Currency = x.Exchange_Currency,
                    Rate = x.Rate.ToString(),
                    Rate_Date = x.Rate_Date.ToString("yyyy/MM/dd"),
                    Update_By = x.Update_By,
                    Update_Time = x.Update_Time.ToString("yyyy/MM/dd HH:mm:ss"),
                    Is_Duplicate = false
                }).ToList();
                var result = new MonthlyExchangeRateSetting_Update
                {
                    Param = new MonthlyExchangeRateSetting_Param
                    {
                        Rate_Month = HSCR[0].Rate_Month.ToString("yyyy/MM/dd")
                    },
                    Data = data
                };
                return new OperationResult(true, result);
            }
            return new OperationResult(false);
        }

        public async Task<OperationResult> PostData(MonthlyExchangeRateSetting_Update input)
        {
            var predicate = PredicateBuilder.New<HRMS_Sal_Currency_Rate>(true);
            if (string.IsNullOrWhiteSpace(input.Param.Rate_Month_Str)
             || !DateTime.TryParseExact(input.Param.Rate_Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime rateMonthValue))
                return new OperationResult(false, "InvalidInput");
            try
            {
                predicate.And(x => x.Rate_Month.Date == rateMonthValue.Date);
                var HAIS = await _repositoryAccessor.HRMS_Sal_Currency_Rate.FindAll(predicate).ToListAsync();
                List<HRMS_Sal_Currency_Rate> addList = new();
                foreach (var item in input.Data)
                {
                    if (string.IsNullOrWhiteSpace(item.Kind)
                     || string.IsNullOrWhiteSpace(item.Currency)
                     || string.IsNullOrWhiteSpace(item.Exchange_Currency)
                     || !item.Rate.CheckDecimalValue(9, 5)
                     || string.IsNullOrWhiteSpace(item.Rate_Date_Str)
                     || string.IsNullOrWhiteSpace(item.Update_By)
                     || string.IsNullOrWhiteSpace(item.Rate_Date_Str)
                     || !DateTime.TryParseExact(item.Rate_Date_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime rateDateValue)
                     || string.IsNullOrWhiteSpace(item.Update_Time_Str)
                     || !DateTime.TryParseExact(item.Update_Time_Str, "yyyy/MM/dd HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime updateTimeValue)
                    )
                        return new OperationResult(false, "InvalidInput");
                    if (HAIS.Any(x => x.Kind == item.Kind && x.Currency == item.Currency && x.Exchange_Currency == item.Exchange_Currency))
                        return new OperationResult(false, "AlreadyExitedData");
                    HRMS_Sal_Currency_Rate addData = new()
                    {
                        Rate_Month = rateMonthValue,
                        Kind = item.Kind,
                        Currency = item.Currency,
                        Exchange_Currency = item.Exchange_Currency,
                        Rate = decimal.Parse(item.Rate),
                        Rate_Date = rateDateValue,
                        Update_By = item.Update_By,
                        Update_Time = updateTimeValue,
                    };
                    addList.Add(addData);
                }
                _repositoryAccessor.HRMS_Sal_Currency_Rate.AddMultiple(addList);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false, "ErrorException");
            }
        }

        public async Task<OperationResult> PutData(MonthlyExchangeRateSetting_Update input)
        {
            var predicate = PredicateBuilder.New<HRMS_Sal_Currency_Rate>(true);
            if (string.IsNullOrWhiteSpace(input.Param.Rate_Month_Str)
            || !DateTime.TryParseExact(input.Param.Rate_Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime rateMonthValue))
                return new OperationResult(false, "InvalidInput");
            try
            {
                predicate.And(x => x.Rate_Month.Date == rateMonthValue.Date
                );
                var removeList = await _repositoryAccessor.HRMS_Sal_Currency_Rate.FindAll(predicate).ToListAsync();
                if (!removeList.Any())
                    return new OperationResult(false, "NotExitedData");
                List<HRMS_Sal_Currency_Rate> addList = new();
                foreach (var item in input.Data)
                {
                    if (string.IsNullOrWhiteSpace(item.Kind)
                    || string.IsNullOrWhiteSpace(item.Currency)
                    || string.IsNullOrWhiteSpace(item.Exchange_Currency)
                    || !item.Rate.CheckDecimalValue(10, 5)
                    || string.IsNullOrWhiteSpace(item.Rate_Date_Str)
                    || string.IsNullOrWhiteSpace(item.Update_By)
                    || string.IsNullOrWhiteSpace(item.Rate_Date_Str)
                    || !DateTime.TryParseExact(item.Rate_Date_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime rateDateValue)
                    || string.IsNullOrWhiteSpace(item.Update_Time_Str)
                    || !DateTime.TryParseExact(item.Update_Time_Str, "yyyy/MM/dd HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime updateTimeValue)
                   )
                        return new OperationResult(false, "InvalidInput");
                    HRMS_Sal_Currency_Rate addData = new()
                    {
                        Rate_Month = rateMonthValue,
                        Kind = item.Kind,
                        Currency = item.Currency,
                        Exchange_Currency = item.Exchange_Currency,
                        Rate = decimal.Parse(item.Rate),
                        Rate_Date = rateDateValue,
                        Update_By = item.Update_By,
                        Update_Time = updateTimeValue,
                    };
                    addList.Add(addData);
                }
                _repositoryAccessor.HRMS_Sal_Currency_Rate.RemoveMultiple(removeList);
                await _repositoryAccessor.Save();

                _repositoryAccessor.HRMS_Sal_Currency_Rate.AddMultiple(addList);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false, "ErrorException");
            }
        }
    }
}
