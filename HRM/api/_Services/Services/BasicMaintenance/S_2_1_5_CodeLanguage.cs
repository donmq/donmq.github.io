using API.Data;
using API._Services.Interfaces.BasicMaintenance;
using API.DTOs.BasicMaintenance;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.BasicMaintenance
{
    public class S_2_1_5_CodeLanguage : BaseServices, I_2_1_5_CodeLanguage
    {
        public S_2_1_5_CodeLanguage(DBContext dbContext) : base(dbContext)
        {
        }


        #region Add
        public async Task<OperationResult> Add(Code_LanguageDetail model)
        {
            if (string.IsNullOrWhiteSpace(model.Code) || string.IsNullOrWhiteSpace(model.Code_Name) || string.IsNullOrWhiteSpace(model.Type_Seq))
                return new OperationResult(false, "Have an empty key value!");
            if (!model.Detail.Any(x => !string.IsNullOrWhiteSpace(x.Name)))
                return new OperationResult(false, "Please add at least one Language!");
            if (await _repositoryAccessor.HRMS_Basic_Code_Language
                .AnyAsync(x =>
                    x.Type_Seq.Trim() == model.Type_Seq.Trim() &&
                    x.Code.Trim() == model.Code.Trim()))
                return new OperationResult(false, "Languages for this code already existed!");
            var addListLang = new List<HRMS_Basic_Code_Language>();
            foreach (var item in model.Detail)
            {
                HRMS_Basic_Code_Language data = new()
                {
                    Type_Seq = model.Type_Seq,
                    Code = model.Code,
                    Language_Code = item.Language_Code.ToUpper(),
                    Code_Name = item.Name,
                    Update_By = model.Update_By,
                    Update_Time = DateTime.Now
                };
                if (!string.IsNullOrWhiteSpace(data.Code_Name))
                    addListLang.Add(data);
            }
            _repositoryAccessor.HRMS_Basic_Code_Language.AddMultiple(addListLang);
            try
            {
                await _repositoryAccessor.Save();
                return new OperationResult(true, "Add Successfully");
            }
            catch
            {
                return new OperationResult(false, "Add Failure");
            }
        }
        #endregion

        public async Task<OperationResult> Delete(Code_LanguageParam param)
        {
            var lang = await _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Type_Seq.Trim() == param.Type_Seq.Trim() && x.Code.Trim() == param.Code.Trim()).ToListAsync();
            if (lang.Any())
            {
                _repositoryAccessor.HRMS_Basic_Code_Language.RemoveMultiple(lang);
                await _repositoryAccessor.Save();
                return new OperationResult(true, "Delete Successfully");
            }
            else
            {
                return new OperationResult(false, "Can not Delete");
            }

        }

        #region Edit
        public async Task<OperationResult> Edit(Code_LanguageDetail model)
        {
            if (!model.Detail.Any(x => !string.IsNullOrWhiteSpace(x.Name)))
                return new OperationResult(false, "Please add at least one Language!");
            var lang = await _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Code.Trim() == model.Code.Trim()).ToListAsync();
            _repositoryAccessor.HRMS_Basic_Code_Language.RemoveMultiple(lang);
            var addListLang = new List<HRMS_Basic_Code_Language>();
            foreach (var item in model.Detail)
            {
                HRMS_Basic_Code_Language data = new()
                {
                    Type_Seq = model.Type_Seq,
                    Code = model.Code,
                    Language_Code = item.Language_Code.ToUpper(),
                    Code_Name = item.Name,
                    Update_By = model.Update_By,
                    Update_Time = DateTime.Now
                };
                if (!string.IsNullOrWhiteSpace(data.Code_Name))
                    addListLang.Add(data);
            }
            try
            {
                _repositoryAccessor.HRMS_Basic_Code_Language.AddMultiple(addListLang);
                await _repositoryAccessor.Save();
                return new OperationResult(true, "Edit Successfully");
            }
            catch
            {
                return new OperationResult(false, "Edit Failure");
            }
        }
        #endregion

        public async Task<OperationResult> ExportExcel(Code_LanguageParam param, string language)
        {
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                await QueryData(param, language),
                "Resources\\Template\\BasicMaintenance\\2_1_5_Code_Language\\Download.xlsx"
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }

        public async Task<Code_LanguageDetail> GetDetail(Code_LanguageParam param)
        {
            var lang = await _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Code == param.Code.Trim() && x.Type_Seq == param.Type_Seq.Trim()).ToListAsync();
            var checkLang = await _repositoryAccessor.HRMS_SYS_Language.FindAll(x => x.IsActive == true).ToListAsync();

            if (!lang.Any())
                return null;

            var details = checkLang.GroupJoin(lang, x => x.Language_Code, y => y.Language_Code, (x, y) => new { CheckLang = x, Lang = y })
                                    .SelectMany(x => x.Lang.DefaultIfEmpty(), (x, y) => new { x.CheckLang, Lang = y })
            .Select(x => new Code_Language_Form()
            {
                Language_Code = x.CheckLang?.Language_Code,
                Name = x.Lang?.Code_Name != null ? x.Lang?.Code_Name : "",
            }).ToList();
            Code_LanguageDetail result = new()
            {
                Type_Seq = param.Type_Seq,
                Type_Title = param.Type_Name != null ? param.Type_Seq + "-" + param.Type_Name : param.Type_Seq,
                Code = param.Code,
                Code_Name = param.Code_Name,
                Detail = details
            };
            return result;
        }

        private async Task<List<Code_LanguageDto>> QueryData(Code_LanguageParam param, string language)
        {
            var langPred = PredicateBuilder.New<HRMS_Basic_Code_Language>(true);
            var typePred = PredicateBuilder.New<HRMS_Basic_Code_Type>(true);
            var basicPred = PredicateBuilder.New<HRMS_Basic_Code>(true);

            if (!string.IsNullOrEmpty(param.Type_Seq))
            {
                langPred.And(x => x.Type_Seq == param.Type_Seq.Trim());
            }
            if (!string.IsNullOrEmpty(param.Code))
            {
                langPred.And(x => x.Code == param.Code.Trim());
            }
            if (!string.IsNullOrEmpty(language))
            {
                langPred.And(x => x.Language_Code.ToLower() == language.ToLower());
            }

            var lang = await _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(langPred).ToListAsync();
            var type = await _repositoryAccessor.HRMS_Basic_Code_Type.FindAll().ToListAsync();
            var basic = await _repositoryAccessor.HRMS_Basic_Code.FindAll().ToListAsync();

            var data = (from T1 in basic
                        join T2 in lang on new { T1.Code, T1.Type_Seq } equals new { T2.Code, T2.Type_Seq }
                        join T3 in type on T2.Type_Seq equals T3.Type_Seq
                        group new { T1, T2, T3 } by new { T1.Code, T2.Type_Seq } into g
                        select new Code_LanguageDto()
                        {
                            Type_Seq = g.FirstOrDefault().T2.Type_Seq,
                            Type_Name = g.FirstOrDefault().T3.Type_Name != null ? g.FirstOrDefault().T3.Type_Name : "",
                            Code = g.FirstOrDefault().T2.Code,
                            Code_Name = g.FirstOrDefault().T2.Code_Name,
                            Update_By = g.FirstOrDefault().T2.Update_By,
                            Update_Time = g.FirstOrDefault().T2.Update_Time,
                            State = g.FirstOrDefault().T1.IsActive == true ? "Y" : "N"
                        }).ToList();

            if (!string.IsNullOrWhiteSpace(param.Type_Name))
            {
                data = data.Where(x => x.Type_Name.ToLower() == param.Type_Name.Trim().ToLower()).ToList();
            }

            if (!string.IsNullOrWhiteSpace(param.Code_Name))
            {
                data = data.Where(x => x.Code_Name.ToLower() == param.Code_Name.Trim().ToLower()).ToList();
            }

            return data;
        }

        public async Task<PaginationUtility<Code_LanguageDto>> Search(PaginationParam paginationParams, Code_LanguageParam param, string language)
        {
            var data = await QueryData(param, language);
            return PaginationUtility<Code_LanguageDto>.Create(data, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<List<KeyValuePair<string, string>>> GetTypeSeq()
        {
            var data = await _repositoryAccessor.HRMS_Basic_Code_Type
                       .FindAll().OrderBy(x => x.Type_Seq)
                       .Select(x => new KeyValuePair<string, string>(x.Type_Seq, $"{x.Type_Seq}-{x.Type_Name}"))
                       .ToListAsync();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetLanguage()
        {
            return await _repositoryAccessor.HRMS_SYS_Language.FindAll(x => x.IsActive == true)
            .Select(x => new KeyValuePair<string, string>(x.Language_Code, x.Language_Name))
            .Distinct().ToListAsync();
        }

        public async Task<List<KeyValuePair<string, string>>> GetCode(string type_Seq)
        {

            return await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.IsActive == true && x.Type_Seq.Trim() == type_Seq.Trim())
            .Select(x => new KeyValuePair<string, string>(x.Code, x.Code_Name))
            .Distinct().ToListAsync();
        }

        public async Task<List<string>> GetCodeName(string type_Seq, string code, string language)
        {
            var checkBasicLang = _repositoryAccessor.HRMS_Basic_Code_Language
                                              .FindAll(x => x.Type_Seq.Trim() == type_Seq.Trim()
                                              && x.Code.Trim() == code.Trim()
                                              && x.Language_Code.ToLower() == language.ToLower());
            if (checkBasicLang.Any())
            {
                return await checkBasicLang.Select(x => x.Code_Name).ToListAsync();

            }
            else
            {
                return await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.IsActive == true && x.Type_Seq.Trim() == type_Seq.Trim() && x.Code.Trim() == code.Trim())
                .Select(x => x.Code_Name).ToListAsync();
            }

        }
    }
}