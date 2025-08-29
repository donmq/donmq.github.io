
using AgileObjects.AgileMapper;
using API.Data;
using API._Services.Interfaces.BasicMaintenance;
using API.DTOs.BasicMaintenance;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.BasicMaintenance
{
    public class S_2_1_6_LevelMaintenance : BaseServices, I_2_1_6_LevelMaintenance
    {
        public S_2_1_6_LevelMaintenance(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult> Add(HRMS_Basic_LevelDto model)
        {
            if (await _repositoryAccessor.HRMS_Basic_Level.AnyAsync(x => x.Level == model.Level && x.Level_Code.Trim().ToUpper() == model.Level_Code.Trim().ToUpper()))
                return new OperationResult(false, "Data already exists.");
            var newItem = Mapper.Map(model).ToANew<HRMS_Basic_Level>(x => x.MapEntityKeys());
            _repositoryAccessor.HRMS_Basic_Level.Add(newItem);
            if (await _repositoryAccessor.Save())
                return new OperationResult(true);
            return new OperationResult(false);
        }

        public async Task<OperationResult> Delete(HRMS_Basic_LevelDto model)
        {
            var item = await _repositoryAccessor.HRMS_Basic_Level
                .FirstOrDefaultAsync(x => x.Level == model.Level && x.Level_Code.Trim() == model.Level_Code.Trim());
            if (item != null)
            {
                _repositoryAccessor.HRMS_Basic_Level.Remove(item);
                if (await _repositoryAccessor.Save())
                    return new OperationResult(true);
                return new OperationResult(false);
            }
            return new OperationResult(false);
        }

        public async Task<OperationResult> Edit(HRMS_Basic_LevelDto model)
        {
            var item = await _repositoryAccessor.HRMS_Basic_Level
                .FirstOrDefaultAsync(x => x.Level == model.Level && x.Level_Code.Trim() == model.Level_Code.Trim());
            if (item != null)
            {
                item = Mapper.Map(model).Over(item);
                _repositoryAccessor.HRMS_Basic_Level.Update(item);
                if (await _repositoryAccessor.Save())
                    return new OperationResult(true);
                return new OperationResult(false);
            }
            return new OperationResult(false);
        }

        public async Task<OperationResult> ExportExcel(LevelMaintenanceParam param, string lang)
        {
            var res = await QueryData(param);
            res.ForEach(item =>
            {
                item.Status = item.IsActive ? (lang == "en" ? "Y.Enabled" : "Y.啟用") : (lang == "en" ? "N.Disabled" : "N.停用");
            });
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                res,
                "Resources\\Template\\BasicMaintenance\\2_1_6_LevelMaintenance\\Download.xlsx"
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }

        private async Task<List<HRMS_Basic_LevelDto>> QueryData(LevelMaintenanceParam param)
        {
            var pred_HRMS_Basic_Level = PredicateBuilder.New<HRMS_Basic_Level>(true);
            if (!string.IsNullOrWhiteSpace(param.Type_Code))
                pred_HRMS_Basic_Level.And(x => x.Type_Code == param.Type_Code.Trim());
            if (!string.IsNullOrWhiteSpace(param.Level))
                pred_HRMS_Basic_Level.And(x => x.Level.ToString().Contains(param.Level.Trim()));
            if (!string.IsNullOrWhiteSpace(param.Level_Code))
                pred_HRMS_Basic_Level.And(x => x.Level_Code.ToLower().Contains(param.Level_Code.Trim().ToLower()));
            var codeLangType = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower() && x.Type_Seq == "3", true);
            var codeLangPositionTitle = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower() && x.Type_Seq == "23", true);
            var data = await _repositoryAccessor.HRMS_Basic_Level.FindAll(pred_HRMS_Basic_Level, true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "3", true),
                    x => x.Level_Code,
                    y => y.Code,
                    (x, y) => new { T1 = x, T2 = y }
                ).SelectMany(x => x.T2.DefaultIfEmpty(),
                    (x, y) => new { x.T1, T2 = y }
                ).GroupJoin(_repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "23", true),
                    x => x.T1.Type_Code,
                    y => y.Code,
                    (x, y) => new { x.T1, x.T2, T3_Type = y }
                ).SelectMany(x => x.T3_Type.DefaultIfEmpty(),
                    (x, y) => new { x.T1, x.T2, T3_Type = y }
                ).Select(x => new HRMS_Basic_LevelDto()
                {
                    Level = x.T1.Level,
                    Type_Code = x.T1.Type_Code,
                    Type_Code_Name = x.T1.Type_Code + "-" + (!string.IsNullOrEmpty(codeLangPositionTitle.FirstOrDefault(o => o.Code == x.T1.Type_Code).Code_Name) ? codeLangPositionTitle.FirstOrDefault(o => o.Code == x.T1.Type_Code).Code_Name : x.T3_Type.Code_Name),
                    Level_Code = x.T1.Level_Code,
                    Level_Code_Name = x.T1.Level_Code + "-" + (!string.IsNullOrEmpty(codeLangType.FirstOrDefault(o => o.Code == x.T1.Level_Code).Code_Name) ? codeLangType.FirstOrDefault(o => o.Code == x.T1.Level_Code).Code_Name : x.T2.Code_Name),
                    Update_By = x.T1.Update_By,
                    Update_Time = x.T1.Update_Time,
                    IsActive = x.T1.IsActive
                }).ToListAsync();
            return data;
        }
        public async Task<PaginationUtility<HRMS_Basic_LevelDto>> GetData(PaginationParam pagination, LevelMaintenanceParam param, bool isPaging = true)
        {
            var data = await QueryData(param);
            return PaginationUtility<HRMS_Basic_LevelDto>.Create(data, pagination.PageNumber, pagination.PageSize, isPaging);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListLevelCode(string type, string language)
        {
            var basicCodeLang = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower() && x.Type_Seq == "3", true);
            var pred_HRMS_Basic_Code = PredicateBuilder.New<HRMS_Basic_Code>(x => x.Type_Seq == "3");
            if (!string.IsNullOrWhiteSpace(type) && type.Trim() == "add")
                pred_HRMS_Basic_Code.And(x => x.IsActive == true);

            var data = await _repositoryAccessor.HRMS_Basic_Code
                .FindAll(pred_HRMS_Basic_Code, true)
                .GroupJoin(basicCodeLang,
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (x, y) => new { basicCode = x, basicCodeLang = y }
                ).SelectMany(x => x.basicCodeLang.DefaultIfEmpty(), (x, y) => new { x.basicCode, basicCodeLang = y })
                .Select(x => new KeyValuePair<string, string>(x.basicCode.Code.Trim(), x.basicCode.Code.Trim() + " - " + (x.basicCodeLang != null ? x.basicCodeLang.Code_Name.Trim() : x.basicCode.Code_Name.Trim())))
                .Distinct().ToListAsync();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetTypes(string language)
        {
            var basicCodeLang = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower() && x.Type_Seq == "23", true);
            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "23", true)
                .GroupJoin(basicCodeLang,
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (x, y) => new { basicCode = x, basicCodeLang = y }
                ).SelectMany(x => x.basicCodeLang.DefaultIfEmpty(), (x, y) => new { x.basicCode, basicCodeLang = y })
                .Select(x => new KeyValuePair<string, string>(x.basicCode.Code.Trim(), x.basicCode.Code.Trim() + " - " + (x.basicCodeLang != null ? x.basicCodeLang.Code_Name.Trim() : x.basicCode.Code_Name.Trim())))
                .Distinct().ToListAsync();
            return data;
        }
    }
}