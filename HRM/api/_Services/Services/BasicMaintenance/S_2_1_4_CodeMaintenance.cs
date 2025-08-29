using AgileObjects.AgileMapper;
using API.Data;
using API._Services.Interfaces.BasicMaintenance;
using API.DTOs.BasicMaintenance;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.BasicMaintenance
{
    public class S_2_1_4_CodeMaintenance : BaseServices, I_2_1_4_CodeMaintenance
    {
        public S_2_1_4_CodeMaintenance(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult> Create(HRMS_Basic_CodeDto model)
        {
            var basicCode = await _repositoryAccessor.HRMS_Basic_Code.FirstOrDefaultAsync(x => x.Type_Seq.ToLower() == model.Type_Seq.Trim().ToLower() && x.Code.ToLower() == model.Code.Trim().ToLower());
            if (basicCode != null) return new OperationResult(false, "This Type Seq and Code is existed");

            var hrmBasicCode = Mapper.Map(model).ToANew<HRMS_Basic_Code>(x => x.MapEntityKeys());
            _repositoryAccessor.HRMS_Basic_Code.Add(hrmBasicCode);
            await _repositoryAccessor.Save();
            return new OperationResult(true);
        }

        public async Task<OperationResult> Delete(string typeSeq, string code)
        {
            var hrmBasicCode = await _repositoryAccessor.HRMS_Basic_Code.FirstOrDefaultAsync(x => x.Type_Seq.ToLower() == typeSeq.Trim().ToLower() && x.Code.ToLower() == code.Trim().ToLower());
            if (hrmBasicCode == null) return new OperationResult(false, "Contract not exist");
            try
            {
                _repositoryAccessor.HRMS_Basic_Code.Remove(hrmBasicCode);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false);
            }
        }

        public async Task<OperationResult> ExportExcel(CodeMaintenanceParam param)
        {
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                await QueryData(param), 
                "Resources\\Template\\BasicMaintenance\\2_1_4_CodeMaintenance\\Download.xlsx"
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }

        public async Task<List<KeyValuePair<string, string>>> GetTypeSeqs()
        {
            return await _repositoryAccessor.HRMS_Basic_Code_Type.FindAll(true)
                            .Select(x => new KeyValuePair<string, string>(x.Type_Seq, $"{x.Type_Seq} - {x.Type_Name}")).Distinct().ToListAsync();
        }

        public async Task<PaginationUtility<HRMS_Basic_CodeDto>> GetDataPagination(PaginationParam param, CodeMaintenanceParam filter)
        {
            var query = await QueryData(filter);
            return PaginationUtility<HRMS_Basic_CodeDto>.Create(query, param.PageNumber, param.PageSize);
        }

        public async Task<List<HRMS_Basic_CodeDto>> QueryData(CodeMaintenanceParam filter)
        {
            var predicateBasicCode = PredicateBuilder.New<HRMS_Basic_Code>(true);

            if (!string.IsNullOrWhiteSpace(filter.Type_Seq))
                predicateBasicCode.And(x => x.Type_Seq.ToLower() == filter.Type_Seq.Trim().ToLower());
            if (!string.IsNullOrWhiteSpace(filter.Code))
                predicateBasicCode.And(x => x.Code.ToLower() == filter.Code.Trim().ToLower());
            if (!string.IsNullOrWhiteSpace(filter.Code_Name))
                predicateBasicCode.And(x => x.Code_Name.ToLower().Contains(filter.Code_Name.Trim().ToLower()));

            var query = await _repositoryAccessor.HRMS_Basic_Code.FindAll(predicateBasicCode, true)
                                    .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Type.FindAll(true),
                                                x => x.Type_Seq,
                                                y => y.Type_Seq,
                                                (x, y) => new { basicCode = x, basicCodeType = y })
                                    .SelectMany(x => x.basicCodeType.DefaultIfEmpty(),
                                                (x, y) => new { basicCode = x.basicCode, basicCodeType = y })
                                    .Select(x => new HRMS_Basic_CodeDto
                                    {
                                        // Data export
                                        Type_Seq = x.basicCode.Type_Seq,
                                        Seq = x.basicCode.Seq,
                                        Code = x.basicCode.Code,
                                        Code_Name = x.basicCode.Code_Name,
                                        Type_Name = x.basicCodeType.Type_Name,
                                        State = x.basicCode.IsActive ? "Y" : "N",

                                        // Data Edit
                                        Char1 = x.basicCode.Char1,
                                        Char2 = x.basicCode.Char2,
                                        Date1 = x.basicCode.Date1,
                                        Date2 = x.basicCode.Date2,
                                        Date3 = x.basicCode.Date3,
                                        Int1 = x.basicCode.Int1,
                                        Int2 = x.basicCode.Int2,
                                        Int3 = x.basicCode.Int3,
                                        Decimal1 = x.basicCode.Decimal1,
                                        Decimal2 = x.basicCode.Decimal2,
                                        Decimal3 = x.basicCode.Decimal3,
                                        Remark = x.basicCode.Remark,
                                        Remark_Code = x.basicCode.Remark_Code,
                                        IsActive = x.basicCode.IsActive,

                                        Update_By = x.basicCode.Update_By,
                                        Update_Time_String = x.basicCode.Update_Time.HasValue ? x.basicCode.Update_Time.Value.ToString("yyyy/MM/dd HH:mm:ss") : null
                                    }).ToListAsync();

            if (!string.IsNullOrWhiteSpace(filter.Type_Name))
                query = query.Where(x => !string.IsNullOrWhiteSpace(x.Type_Name) && x.Type_Name.ToLower().Contains(filter.Type_Name.Trim().ToLower())).ToList();
            return query;
        }

        public async Task<OperationResult> Update(HRMS_Basic_CodeDto model)
        {
            var basicCode = _repositoryAccessor.HRMS_Basic_Code.FirstOrDefault(x => x.Type_Seq.ToLower() == model.Type_Seq.Trim().ToLower() && x.Code.ToLower() == model.Code.Trim().ToLower());
            if (basicCode == null) return new OperationResult(false, "This Basic Code not existed");

            basicCode = Mapper.Map(model).Over(basicCode);
            try
            {
                _repositoryAccessor.HRMS_Basic_Code.Update(basicCode);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false);
            }

        }
    }
}