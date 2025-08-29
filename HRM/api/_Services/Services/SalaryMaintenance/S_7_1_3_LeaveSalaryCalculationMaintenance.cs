using AgileObjects.AgileMapper;
using API.Data;
using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.SalaryMaintenance
{
    public class S_7_1_3_LeaveSalaryCalculationMaintenance : BaseServices, I_7_1_3_LeaveSalaryCalculationMaintenance
    {
        public S_7_1_3_LeaveSalaryCalculationMaintenance(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult> Create(LeaveSalaryCalculationMaintenanceDTO dto)
        {

            if (await _repositoryAccessor.HRMS_Sal_Leave_Calc_Maintenance.AnyAsync(x => x.Factory == dto.Factory
                                                                        && x.Leave_Code == dto.Leave_Code))
                return new OperationResult(false, "SalaryMaintenance.LeaveSalaryCalculationMaintenance.Duplicates");

            var dataCreate = new HRMS_Sal_Leave_Calc_Maintenance
            {
                Factory = dto.Factory,
                Leave_Code = dto.Leave_Code,
                Salary_Rate = dto.Salary_Rate,
                Update_By = dto.Update_By,
                Update_Time = DateTime.Now
            };

            try
            {
                _repositoryAccessor.HRMS_Sal_Leave_Calc_Maintenance.Add(dataCreate);
                await _repositoryAccessor.Save();
                return new OperationResult(true, "System.Message.CreateOKMsg");
            }
            catch (System.Exception)
            {
                return new OperationResult(false, "System.Message.CreateErrorMsg");
            }
        }

        public async Task<OperationResult> Delete(LeaveSalaryCalculationMaintenanceDTO data)
        {
            var item = await _repositoryAccessor.HRMS_Sal_Leave_Calc_Maintenance.FirstOrDefaultAsync(x => x.Factory == data.Factory && x.Leave_Code == data.Leave_Code);
            if (item is not null)
                _repositoryAccessor.HRMS_Sal_Leave_Calc_Maintenance.Remove(item);

            try
            {
                await _repositoryAccessor.Save();
                return new OperationResult(true, "System.Message.DeleteOKMsg");
            }
            catch (System.Exception)
            {
                return new OperationResult(false, "System.Message.DeleteErrorMsg");
            }
        }

        public async Task<PaginationUtility<LeaveSalaryCalculationMaintenanceDTO>> GetDataPagination(PaginationParam pagination, LeaveSalaryCalculationMaintenanceParam param)
        {
            var predicate = PredicateBuilder.New<HRMS_Sal_Leave_Calc_Maintenance>(true);
            if (!string.IsNullOrWhiteSpace(param.Factory))
                predicate.And(x => x.Factory == param.Factory);

            var leave = await GetListLeaveCode(param.Language);

            var data = await _repositoryAccessor.HRMS_Sal_Leave_Calc_Maintenance.FindAll(predicate).ToListAsync();

            var result = data.Select(item => new LeaveSalaryCalculationMaintenanceDTO
            {
                Factory = item.Factory,
                Leave_Code = item.Leave_Code,
                Leave_Code_Name = leave.FirstOrDefault(y => y.Key == item.Leave_Code).Value,
                Salary_Rate = item.Salary_Rate,
                Update_By = item.Update_By,
                Update_Time = item.Update_Time.ToString("yyyy/MM/dd HH:mm:ss"),
            }).ToList();

            return PaginationUtility<LeaveSalaryCalculationMaintenanceDTO>.Create(result, pagination.PageNumber, pagination.PageSize);
        }


        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string language, List<string> roleList)
        {
            var predHBC = PredicateBuilder.New<HRMS_Basic_Code>(x => x.Type_Seq == BasicCodeTypeConstant.Factory);

            var factorys = await Queryt_Factory_AddList(roleList);
            predHBC.And(x => factorys.Contains(x.Code));

            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(predHBC, true)
                        .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                           x => new { x.Type_Seq, x.Code },
                           y => new { y.Type_Seq, y.Code },
                           (x, y) => new { HBC = x, HBCL = y }
                        ).SelectMany(x => x.HBCL.DefaultIfEmpty(),
                            (x, y) => new { x.HBC, HBCL = y }
                        ).Select(x => new KeyValuePair<string, string>(
                            x.HBC.Code.Trim(),
                            x.HBC.Code.Trim() + " - " + (x.HBCL != null ? x.HBCL.Code_Name.Trim() : x.HBC.Code_Name.Trim())
                        )).Distinct().ToListAsync();
            return data;
        }
        public async Task<List<KeyValuePair<string, string>>> GetListLeaveCode(string language)
        {
             var result = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Leave && x.Char1 == "Leave" && x.IsActive, true)
                    .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                                    x => new { x.Type_Seq, x.Code },
                                    y => new { y.Type_Seq, y.Code },
                                    (x, y) => new { basicCode = x, basicCode_lang = y })
                                    .SelectMany(x => x.basicCode_lang.DefaultIfEmpty(),
                                    (x, y) => new { x = x.basicCode, basicCode_lang = y })
                .Select(x => new KeyValuePair<string, string>(
                    x.x.Code,
                    $"{x.x.Code} - {(x.basicCode_lang != null ? x.basicCode_lang.Code_Name : x.x.Code_Name)}")
                ).ToListAsync();
            return result;
        }

        public async Task<OperationResult> Update(LeaveSalaryCalculationMaintenanceDTO data)
        {
            var item = await _repositoryAccessor.HRMS_Sal_Leave_Calc_Maintenance.FirstOrDefaultAsync(x => x.Factory == data.Factory && x.Leave_Code == data.Leave_Code);

            if (item is not null)
            {
                item = Mapper.Map(data).Over(item);
                item.Update_Time = DateTime.Now;
                item.Update_By = data.Update_By;
                _repositoryAccessor.HRMS_Sal_Leave_Calc_Maintenance.Update(item);
            }

            try
            {
                await _repositoryAccessor.Save();
                return new OperationResult(true, "System.Message.UpdateOKMsg");
            }
            catch (System.Exception)
            {
                return new OperationResult(false, "System.Message.UpdateErrorMsg");
            }
        }
    }
}