using AgileObjects.AgileMapper;
using API.Data;
using API._Services.Interfaces.BasicMaintenance;
using API.DTOs.BasicMaintenance;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.BasicMaintenance
{
    public class S_2_1_3_TypeCodeMaintenance : BaseServices, I_2_1_3_TypeCodeMaintenance
    {
        public S_2_1_3_TypeCodeMaintenance(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult> AddNew(Language_Dto param, string user)
        {
            List<HRMS_Basic_Code_Type_Language> code_Languages = new();
            List<HRMS_Basic_Code_Type> code_Type = new();
            foreach (var item in param.Detail_Dto)
            {
                if (item.Language_Code == "TW")
                {
                    HRMS_Basic_Code_Type dataType = new()
                    {
                        Type_Seq = param.type_Seq.Trim(),
                        Type_Name = item.Type_Name.Trim(),
                        Update_By = user,
                        Update_Time = DateTime.Now,
                    };
                    code_Type.Add(dataType);
                }
                if (!string.IsNullOrWhiteSpace(item.Type_Name))
                {
                    HRMS_Basic_Code_Type_Language dataLanguage = new()
                    {
                        Type_Seq = param.type_Seq.Trim(),
                        Language_Code = item.Language_Code.ToUpper(),
                        Type_Name = item.Type_Name.Trim(),
                        Update_By = user,
                        Update_Time = DateTime.Now,
                    };
                    code_Languages.Add(dataLanguage);
                }
            }
            _repositoryAccessor.HRMS_Basic_Code_Type.AddMultiple(code_Type);
            _repositoryAccessor.HRMS_Basic_Code_Type_Language.AddMultiple(code_Languages);
            try
            {
                await _repositoryAccessor.Save();
                return new OperationResult { IsSuccess = true };
            }
            catch
            {
                return new OperationResult { IsSuccess = false };
            }
        }

        public async Task<OperationResult> EditLanguageCode(Language_Dto param, string user)
        {
            var lang = await _repositoryAccessor.HRMS_Basic_Code_Type_Language
                .FindAll(x => x.Type_Seq.Trim() == param.type_Seq.Trim())
                .ToListAsync();
            List<HRMS_Basic_Code_Type_Language> code_Languages = new();
            List<HRMS_Basic_Code_Type> code_Type = new();
            _repositoryAccessor.HRMS_Basic_Code_Type_Language.RemoveMultiple(lang);
            foreach (var item in param.Detail_Dto)
            {
                if (item.Language_Code == "TW")
                {
                    HRMS_Basic_Code_Type dataType = new()
                    {
                        Type_Seq = param.type_Seq.Trim(),
                        Type_Name = item.Type_Name.Trim(),
                        Update_By = user,
                        Update_Time = DateTime.Now,
                    };
                    code_Type.Add(dataType);
                }
                if (!string.IsNullOrWhiteSpace(item.Type_Name))
                {
                    HRMS_Basic_Code_Type_Language data = new()
                    {
                        Type_Seq = param.type_Seq.Trim(),
                        Language_Code = item.Language_Code.ToUpper(),
                        Type_Name = item.Type_Name.Trim(),
                        Update_By = user,
                        Update_Time = DateTime.Now,
                    };
                    code_Languages.Add(data);
                }
            }
            _repositoryAccessor.HRMS_Basic_Code_Type.UpdateMultiple(code_Type);
            _repositoryAccessor.HRMS_Basic_Code_Type_Language.AddMultiple(code_Languages);
            try
            {
                await _repositoryAccessor.Save();
                return new OperationResult { IsSuccess = true };
            }
            catch (Exception)
            {
                return new OperationResult { IsSuccess = false };
            }
        }

        public async Task<OperationResult> Edit(HRMS_Basic_Code_TypeDto param)
        {
            var checkdata = await _repositoryAccessor.HRMS_Basic_Code_Type.FirstOrDefaultAsync(x => x.Type_Seq == param.Type_Seq);
            if (checkdata == null)
                return new OperationResult(false);
            checkdata.Type_Seq = param.Type_Seq.Trim();
            checkdata.Type_Name = param.Type_Name.Trim();
            checkdata.Update_By = param.Update_By;
            checkdata.Update_Time = DateTime.Now;
            _repositoryAccessor.HRMS_Basic_Code_Type.Update(checkdata);

            if (await _repositoryAccessor.Save())
            {
                return new OperationResult(true, "Update Successfully");
            }
            return new OperationResult(false, "Update failed");
        }

        public async Task<OperationResult> Delete(string type_Seq)
        {
            var checkData = await _repositoryAccessor.HRMS_Basic_Code_Type.FirstOrDefaultAsync(x => x.Type_Seq == type_Seq.Trim());
            if (checkData == null)
                return new OperationResult(false, "Type Seq not exist");
            if (await _repositoryAccessor.HRMS_Basic_Code.AnyAsync(x => x.Type_Seq.Trim() == type_Seq.Trim()))
                return new OperationResult(false, "Already used in Code Maintenance");
            _repositoryAccessor.HRMS_Basic_Code_Type.Remove(checkData);
            await _repositoryAccessor.Save();
            return new OperationResult(true, "Delete Successfully");
        }

        public async Task<PaginationUtility<HRMS_Basic_Code_Type_LanguageInfoDto>> Getdata(PaginationParam pagination, HRMS_Basic_Code_TypeParam param)
        {
            ExpressionStarter<HRMS_Basic_Code_Type> predicate = PredicateBuilder.New<HRMS_Basic_Code_Type>(true);
            if (!string.IsNullOrWhiteSpace(param.Type_Seq))
                predicate = predicate.And(x => x.Type_Seq.Contains(param.Type_Seq.Trim()));
            if (!string.IsNullOrWhiteSpace(param.Type_Name))
                predicate = predicate.And(x => x.Type_Name.Contains(param.Type_Name.Trim()));
            var HBCT = _repositoryAccessor.HRMS_Basic_Code_Type.FindAll(predicate).Distinct();
            var HBCTL = _repositoryAccessor.HRMS_Basic_Code_Type_Language.FindAll();
            var data = await HBCT
                .GroupJoin(HBCTL,
                    a => a.Type_Seq,
                    b => b.Type_Seq,
                    (a, b) => new { HBCT = a, HBCTL = b })
                .SelectMany(x => x.HBCTL.DefaultIfEmpty(),
                    (x, y) => new { x.HBCT, HBCTL = y })
                .GroupBy(x => x.HBCT)
                .Select(x => new HRMS_Basic_Code_Type_LanguageInfoDto
                {
                    Type_Seq = x.Key.Type_Seq,
                    Type_Name = x.Key.Type_Name,
                    Info = x.Where(y => y.HBCTL != null)
                        .GroupBy(y => y.HBCTL)
                        .Select(y => new Info
                        {
                            Language_Code = y.Key.Language_Code,
                            Type_Name = y.Key.Type_Name,
                        }).Distinct().ToList()
                }).OrderBy(x => x.Type_Seq).ToListAsync();
            var result = data.Select(x =>
            {
                var isInt = int.TryParse(x.Type_Seq, out int Seq);
                return new HRMS_Basic_Code_Type_LanguageInfoDto
                {
                    isInt = isInt,
                    Seq = Seq,
                    Type_Seq = x.Type_Seq,
                    Type_Name = x.Type_Name,
                    Info = x.Info
                };
            }).OrderBy(x => x.isInt).ThenBy(x => x.Seq).ToList();
            return PaginationUtility<HRMS_Basic_Code_Type_LanguageInfoDto>.Create(result, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<OperationResult> AddTypeCode(HRMS_Basic_Code_TypeDto param, string user)
        {
            if (await _repositoryAccessor.HRMS_Basic_Code_Type.AnyAsync(x => x.Type_Seq == param.Type_Seq))
                return new OperationResult(false, "Type Seq already exist ");
            param.Update_By = user;
            param.Update_Time = DateTime.Now;
            var typeCodeData = Mapper.Map(param).ToANew<HRMS_Basic_Code_Type>(x => x.MapEntityKeys());
            _repositoryAccessor.HRMS_Basic_Code_Type.Add(typeCodeData);
            try
            {
                await _repositoryAccessor.Save();
                return new OperationResult { IsSuccess = true };
            }
            catch
            {
                return new OperationResult { IsSuccess = false };
            }
        }

        public async Task<List<KeyValuePair<string, string>>> GetLanguage()
        {
            return await _repositoryAccessor.HRMS_SYS_Language
                .FindAll(x => x.IsActive == true)
                .Select(x => new KeyValuePair<string, string>(x.Language_Code, x.Language_Name))
                .Distinct().ToListAsync();
        }

        public async Task<Language_Dto> GetDetail(string type_Seq)
        {
            var data = await _repositoryAccessor.HRMS_SYS_Language.FindAll(x => x.IsActive == true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Type_Language.FindAll(x => x.Type_Seq == type_Seq),
                    a => a.Language_Code,
                    b => b.Language_Code,
                    (a, b) => new { a, b })
                .SelectMany(x => x.b.DefaultIfEmpty(),
                    (x, y) => new { x.a.Language_Code, Name = y.Type_Name })
                .ToListAsync();
            if (!data.Any())
                return null;
            var detail = data.Select(x => new LanguageDetail_Dto
            {
                Language_Code = x.Language_Code,
                Type_Name = x.Name
            }).ToList();
            Language_Dto result = new()
            {
                type_Seq = type_Seq,
                Detail_Dto = detail
            };
            return result;
        }
    }
}