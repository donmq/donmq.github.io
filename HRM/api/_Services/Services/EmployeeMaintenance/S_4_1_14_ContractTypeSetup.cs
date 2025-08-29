using API.Data;
using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.EmployeeMaintenance
{
    public class S_4_1_14_ContractTypeSetup : BaseServices, I_4_1_14_ContractTypeSetup
    {
        public S_4_1_14_ContractTypeSetup(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult> Add(ContractTypeSetupDto data, string userName)
        {
            if (await _repositoryAccessor.HRMS_Emp_Contract_Type.AnyAsync(x => x.Division == data.Division &&
                                                                    x.Factory == data.Factory &&
                                                                    x.Contract_Type == data.Contract_Type))
                return new OperationResult(false, "Data ContractType exist already");

            var dataContractType = new HRMS_Emp_Contract_Type
            {
                Division = data.Division,
                Factory = data.Factory,
                Contract_Type = data.Contract_Type,
                Contract_Title = data.Contract_Title,
                Probationary_Period = data.Probationary_Period,
                Probationary_Year = data.Probationary_Year,
                Probationary_Month = data.Probationary_Month,
                Probationary_Day = data.Probationary_Day,
                Alert = data.Alert,
                Update_By = userName,
                Update_Time = DateTime.Now
            };

            if (data.Alert)
            {

                if (await _repositoryAccessor.HRMS_Emp_Contract_Type_Detail.AnyAsync(x => x.Division == data.Division &&
                                                                       x.Factory == data.Factory &&
                                                                       x.Contract_Type == data.Contract_Type))
                    return new OperationResult(false, "Data ContractTypeDetail exist already");

                List<HRMS_Emp_Contract_Type_Detail> dataContractTypeDetail = new();

                foreach (var item in data.dataDetail)
                {
                    var listDetail = new HRMS_Emp_Contract_Type_Detail
                    {
                        Division = item.Division,
                        Factory = item.Factory,
                        Contract_Type = item.Contract_Type,
                        Seq = item.Seq,
                        Schedule_Frequency = item.Schedule_Frequency,
                        Day_Of_Month = item.Day_Of_Month,
                        Alert_Rules = item.Alert_Rules,
                        Days_Before_Expiry_Date = item.Days_Before_Expiry_Date,
                        Month_Range = item.Month_Range,
                        Contract_Start = item.Contract_Start,
                        Contract_End = item.Contract_End,
                        Update_By = userName,
                        Update_Time = DateTime.Now
                    };
                    dataContractTypeDetail.Add(listDetail);
                }

                await _repositoryAccessor.BeginTransactionAsync();
                try
                {
                    _repositoryAccessor.HRMS_Emp_Contract_Type.Add(dataContractType);
                    _repositoryAccessor.HRMS_Emp_Contract_Type_Detail.AddMultiple(dataContractTypeDetail);
                    await _repositoryAccessor.Save();
                    await _repositoryAccessor.CommitAsync();
                    return new OperationResult(true);
                }
                catch
                {
                    await _repositoryAccessor.RollbackAsync();
                    return new OperationResult(false);
                }
            }
            else
            {
                await _repositoryAccessor.BeginTransactionAsync();
                try
                {
                    _repositoryAccessor.HRMS_Emp_Contract_Type.Add(dataContractType);
                    await _repositoryAccessor.Save();
                    await _repositoryAccessor.CommitAsync();
                    return new OperationResult(true);
                }
                catch
                {
                    await _repositoryAccessor.RollbackAsync();
                    return new OperationResult(false);
                }
            }
        }


        public async Task<OperationResult> Delete(ContractTypeSetupDto data)
        {
            var itemContractType = await _repositoryAccessor.HRMS_Emp_Contract_Type
                .FirstOrDefaultAsync(x => x.Division == data.Division &&
                                           x.Factory == data.Factory &&
                                           x.Contract_Type == data.Contract_Type);

            if (itemContractType == null)
                return new OperationResult(false, "Data not exist");


            var itemContractTypeDetail = await _repositoryAccessor.HRMS_Emp_Contract_Type_Detail
                    .FindAll(x => x.Division == data.Division &&
                                  x.Factory == data.Factory &&
                                  x.Contract_Type == data.Contract_Type).ToListAsync();

            var itemDetailSeq = itemContractTypeDetail.FirstOrDefault(x => x.Seq == data.Seq);

            if (itemContractTypeDetail.Count > 1)
            {
                _repositoryAccessor.HRMS_Emp_Contract_Type_Detail.Remove(itemDetailSeq);
            }
            else
            {
                if (itemContractTypeDetail.Any())
                {
                    _repositoryAccessor.HRMS_Emp_Contract_Type_Detail.Remove(itemDetailSeq);
                }
                _repositoryAccessor.HRMS_Emp_Contract_Type.Remove(itemContractType);
            }
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {



                await _repositoryAccessor.CommitAsync();
                await _repositoryAccessor.Save();
                return new OperationResult(true, "Delete Successfully");
            }
            catch
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false, "Delete failed");
            }
        }


        public async Task<OperationResult> Edit(ContractTypeSetupDto data, string userName)
        {
            var dataExistContractType = await _repositoryAccessor.HRMS_Emp_Contract_Type
                    .FirstOrDefaultAsync(x => x.Division == data.Division &&
                                              x.Factory == data.Factory &&
                                              x.Contract_Type == data.Contract_Type_After);
            if (dataExistContractType == null)
                return new OperationResult(false, "No data");

            _repositoryAccessor.HRMS_Emp_Contract_Type.Remove(dataExistContractType);

            var dataNew = new HRMS_Emp_Contract_Type
            {
                Division = data.Division,
                Factory = data.Factory,
                Contract_Type = data.Contract_Type,
                Contract_Title = data.Contract_Title,
                Probationary_Period = data.Probationary_Period,
                Probationary_Year = data.Probationary_Year,
                Probationary_Month = data.Probationary_Month,
                Probationary_Day = data.Probationary_Day,
                Alert = data.Alert,
                Update_By = userName,
                Update_Time = DateTime.Now
            };
            _repositoryAccessor.HRMS_Emp_Contract_Type.Add(dataNew);

            var listDataDetailIds = await _repositoryAccessor.HRMS_Emp_Contract_Type_Detail.
                FindAll(x => x.Division == data.Division && x.Factory == data.Factory && x.Contract_Type == data.Contract_Type).ToListAsync();

            _repositoryAccessor.HRMS_Emp_Contract_Type_Detail.RemoveMultiple(listDataDetailIds);
            await _repositoryAccessor.Save();


            List<HRMS_Emp_Contract_Type_Detail> dataContractTypeDetail = new();

            foreach (var item in data.dataDetail)
            {
                var listDetail = new HRMS_Emp_Contract_Type_Detail { };

                listDetail.Division = item.Division;
                listDetail.Factory = item.Factory;
                listDetail.Contract_Type = item.Contract_Type;
                listDetail.Seq = item.Seq;
                listDetail.Schedule_Frequency = item.Schedule_Frequency;
                listDetail.Day_Of_Month = item.Day_Of_Month;
                listDetail.Alert_Rules = item.Alert_Rules;
                listDetail.Days_Before_Expiry_Date = item.Days_Before_Expiry_Date;
                listDetail.Month_Range = item.Month_Range;
                listDetail.Contract_Start = item.Contract_Start;
                listDetail.Contract_End = item.Contract_End;
                listDetail.Update_By = item.Update_By;
                listDetail.Update_Time = item.Update_Time;
                dataContractTypeDetail.Add(listDetail);
            }
            _repositoryAccessor.HRMS_Emp_Contract_Type_Detail.AddMultiple(dataContractTypeDetail);
            try
            {
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception ex)
            {

                return new OperationResult(false, ex.Message);
            }
        }

        public async Task<List<HRMSEmpContractTypeDetail>> GetDataDetail(ContractTypeSetupParam param)
        {
            var predContractTypeDetail = PredicateBuilder.New<HRMS_Emp_Contract_Type_Detail>
                                                        (x => x.Division == param.Division &&
                                                        x.Factory == param.Factory &&
                                                        x.Contract_Type == param.Contract_Type);
            var dataDetail = await _repositoryAccessor.HRMS_Emp_Contract_Type_Detail.FindAll(predContractTypeDetail)
                    .GroupJoin(_repositoryAccessor.HRMS_Emp_Contract_Type.FindAll(),
                    x => new { x.Division, x.Factory, x.Contract_Type },
                    y => new { y.Division, y.Factory, y.Contract_Type },
                    (x, y) => new { type = x, detail = y })
                    .SelectMany(x => x.detail.DefaultIfEmpty(), (x, y) => new { type = x.type, detail = y })
                    .Select(x => new HRMSEmpContractTypeDetail
                    {
                        Seq = x.type.Seq,
                        Schedule_Frequency = x.type.Schedule_Frequency,
                        Day_Of_Month = x.type.Day_Of_Month,
                        Alert_Rules = x.type.Alert_Rules,
                        Days_Before_Expiry_Date = x.type.Days_Before_Expiry_Date,
                        Month_Range = x.type.Month_Range,
                        Contract_Start = x.type.Contract_Start,
                        Contract_End = x.type.Contract_End,
                        Update_By = x.type.Update_By,
                        Update_Time = x.type.Update_Time
                    }).AsNoTracking().ToListAsync();
            return dataDetail;
        }

        public async Task<PaginationUtility<ContractTypeSetupDto>> GetDataPagination(PaginationParam pagination, ContractTypeSetupParam param)
        {
            var data = await GetData(param);
            return PaginationUtility<ContractTypeSetupDto>.Create(data, pagination.PageNumber, pagination.PageSize);
        }

        private async Task<List<ContractTypeSetupDto>> GetData(ContractTypeSetupParam param)
        {
            var predContractType = PredicateBuilder.New<HRMS_Emp_Contract_Type>(true);
            var predContractTypeDetail = PredicateBuilder.New<HRMS_Emp_Contract_Type_Detail>(true);

            if (!string.IsNullOrWhiteSpace(param.Division))
            {
                predContractType.And(x => x.Division == param.Division);
                predContractTypeDetail.And(x => x.Division == param.Division);
            }

            if (!string.IsNullOrWhiteSpace(param.Factory))
            {
                predContractType.And(x => x.Factory == param.Factory);
                predContractTypeDetail.And(x => x.Factory == param.Factory);
            }

            if (!string.IsNullOrWhiteSpace(param.Contract_Type))
            {
                predContractType.And(x => x.Contract_Type == param.Contract_Type);
                predContractTypeDetail.And(x => x.Contract_Type == param.Contract_Type);
            }

            if (param.Alert_Str == "Y")
                predContractType.And(x => x.Alert == true);

            if (param.Alert_Str == "N")
                predContractType.And(x => x.Alert == false);

            if (param.Probationary_Period_Str == "Y")
                predContractType.And(x => x.Probationary_Period == true);

            if (param.Probationary_Period_Str == "N")
                predContractType.And(x => x.Probationary_Period == false);

            IQueryable<HRMS_Emp_Contract_Type> contractType = _repositoryAccessor.HRMS_Emp_Contract_Type.FindAll(predContractType, true);
            IQueryable<HRMS_Emp_Contract_Type_Detail> contractDetail = _repositoryAccessor.HRMS_Emp_Contract_Type_Detail.FindAll(predContractTypeDetail, true);
            IQueryable<HRMS_Basic_Code> basicCode34 = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.ScheduleFrequency, true);
            IQueryable<HRMS_Basic_Code> basicCode35 = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.AlertRules, true);
            IQueryable<HRMS_Basic_Code_Language> basicCodeLang34 = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.ScheduleFrequency && x.Language_Code.ToLower() == param.Lang.ToLower(), true);
            IQueryable<HRMS_Basic_Code_Language> basicCodeLang35 = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.AlertRules && x.Language_Code.ToLower() == param.Lang.ToLower(), true);

            var data = await contractType.GroupJoin(contractDetail,
            x => new { x.Division, x.Factory, x.Contract_Type },
            y => new { y.Division, y.Factory, y.Contract_Type },
            (x, y) => new { ct = x, cd = y })
            .SelectMany(x => x.cd.DefaultIfEmpty(), (x, y) => new { x.ct, cd = y })
            .GroupJoin(basicCode34,
            x => x.cd.Schedule_Frequency,
            code => code.Code,
            (x, code) => new { x.cd, x.ct, sFcode = code })
            .SelectMany(x => x.sFcode.DefaultIfEmpty(),
            (x, code) => new { x.cd, x.ct, sFcode = code })
            .GroupJoin(basicCodeLang34,
            x => x.sFcode.Code,
            y => y.Code,
            (x, y) => new { x.cd, x.ct, x.sFcode, lang = y })
            .SelectMany(x => x.lang.DefaultIfEmpty(),
            (x, y) => new { x.cd, x.ct, x.sFcode, lang = y })
            .GroupJoin(basicCode35,
            x => x.cd.Alert_Rules,
            y => y.Code,
            (x, y) => new { x.cd, x.ct, x.sFcode, x.lang, aRCode = y })
            .SelectMany(x => x.aRCode.DefaultIfEmpty(),
             (x, y) => new { x.cd, x.ct, x.sFcode, x.lang, aRCode = y })
            .GroupJoin(basicCodeLang35,
            x => x.aRCode.Code,
            y => y.Code,
            (x, y) => new { x.cd, x.ct, x.sFcode, x.lang, x.aRCode, langAR = y })
            .SelectMany(x => x.langAR.DefaultIfEmpty(),
            (x, y) => new { x.cd, x.ct, x.sFcode, x.lang, x.aRCode, langAR = y })
            .Select(x => new ContractTypeSetupDto
            {
                Division = x.ct.Division,
                Factory = x.ct.Factory,
                Contract_Type = x.ct.Contract_Type,
                Contract_Title = x.ct.Contract_Title,
                Probationary_Period = x.ct.Probationary_Period,
                Probationary_Period_Str = x.ct.Probationary_Period == true ? "Y" : "N",
                Probationary_Year = x.ct.Probationary_Year,
                Probationary_Month = x.ct.Probationary_Month,
                Probationary_Day = x.ct.Probationary_Day,
                Alert = x.ct.Alert,
                Alert_Str = x.ct.Alert == true ? "Y" : "N",
                Seq = x.cd != null ? x.cd.Seq : null,
                Schedule_Frequency = x.lang != null ? x.lang.Code_Name : string.Empty,
                Day_Of_Month = x.cd != null ? x.cd.Day_Of_Month : null,
                Alert_Rules = x.langAR != null ? x.langAR.Code_Name : string.Empty,
                Days_Before_Expiry_Date = x.cd != null ? x.cd.Days_Before_Expiry_Date : null,
                Month_Range = x.cd != null ? x.cd.Month_Range : null,
                Contract_Start = x.cd != null ? x.cd.Contract_Start : null,
                Contract_End = x.cd != null ? x.cd.Contract_End : null,
                Update_By = x.cd != null ? x.cd.Update_By : x.ct.Update_By,
                Update_Time = x.cd != null ? x.cd.Update_Time : x.ct.Update_Time,
            }).ToListAsync();

            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListContractType(string division, string factory, string language)
        {
            var data = await _repositoryAccessor.HRMS_Emp_Contract_Type.FindAll(x => x.Division == division && x.Factory == factory)
                    .Select(x => new KeyValuePair<string, string>(
                        x.Contract_Type,
                        x.Contract_Title
                    ))
                    .Distinct()
                    .ToListAsync();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListDivision(string language)
        {
            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "1")
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower()),
                                x => new { x.Type_Seq, x.Code },
                                y => new { y.Type_Seq, y.Code },
                                (x, y) => new { x, y })
                                .SelectMany(x => x.y.DefaultIfEmpty(),
                                (x, y) => new { BasicCode = x.x, BasicCodeLanguage = y })
            .Select(x => new KeyValuePair<string, string>(x.BasicCode.Code, $"{x.BasicCode.Code} - {(x.BasicCodeLanguage != null ? x.BasicCodeLanguage.Code_Name : x.BasicCode.Code_Name)}")).ToListAsync();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string division, string language)
        {
            var pred = PredicateBuilder.New<HRMS_Basic_Factory_Comparison>(x => x.Kind == "1");

            if (!string.IsNullOrWhiteSpace(division))
                pred = pred.And(x => x.Division.ToLower().Contains(division.ToLower()));

            var data = await _repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(pred)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower()),
                    x => x.Factory,
                    y => y.Code,
                    (x, y) => new { x.Factory, CodeNameLanguage = y.Select(z => z.Code_Name).FirstOrDefault() })
                .Join(_repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "2"),
                    x => x.Factory,
                    y => y.Code,
                    (x, y) => new { x.Factory, x.CodeNameLanguage, CodeName = y.Code_Name })
                .Select(x => new KeyValuePair<string, string>(x.Factory, x.CodeNameLanguage ?? x.CodeName))
                .Distinct().ToListAsync();

            if (!data.Any())
            {
                var allFactories = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "2")
                    .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower()),
                        x => x.Code,
                        y => y.Code,
                        (x, y) => new { x.Code, NameCode = x.Code_Name, NameLanguage = y.Select(z => z.Code_Name).FirstOrDefault() })
                    .Select(x => new KeyValuePair<string, string>(x.Code, x.NameLanguage ?? x.NameCode))
                    .ToListAsync();
                return allFactories;
            }
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListScheduleFrequency(string language)
        {
            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "34")
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower()),
                                x => new { x.Type_Seq, x.Code },
                                y => new { y.Type_Seq, y.Code },
                                (x, y) => new { x, y })
                                .SelectMany(x => x.y.DefaultIfEmpty(),
                                (x, y) => new { BasicCode = x.x, BasicCodeLanguage = y })
                .Select(x => new KeyValuePair<string, string>
                (x.BasicCode.Code, $"{x.BasicCode.Code} - {(x.BasicCodeLanguage != null ?
                x.BasicCodeLanguage.Code_Name : x.BasicCode.Code_Name)}")).ToListAsync();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListAlertRule(string language)
        {
            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "35")
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower()),
                              x => new { x.Type_Seq, x.Code },
                                y => new { y.Type_Seq, y.Code },
                                (x, y) => new { x, y })
                                .SelectMany(x => x.y.DefaultIfEmpty(),
                                (x, y) => new { BasicCode = x.x, BasicCodeLanguage = y })
                .Select(x => new KeyValuePair<string, string>
                (x.BasicCode.Code, $"{x.BasicCode.Code} - {(x.BasicCodeLanguage != null ?
                x.BasicCodeLanguage.Code_Name : x.BasicCode.Code_Name)}")).ToListAsync();
            return data;
        }

        public async Task<OperationResult> DownloadExcel(ContractTypeSetupParam param)
        {
            var data = await GetData(param);
            if (!data.Any())
                return new OperationResult(false, "No Data");
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                data, 
                "Resources\\Template\\EmployeeMaintenance\\4_1_14_ContractTypeSetup\\Download.xlsx"
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }
    }
}