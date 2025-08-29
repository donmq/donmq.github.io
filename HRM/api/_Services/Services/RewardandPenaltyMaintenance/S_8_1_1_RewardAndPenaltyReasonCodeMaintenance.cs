using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API._Repositories;
using API._Services.Interfaces.RewardandPenaltyMaintenance;
using API.Data;
using API.DTOs.RewardandPenaltyMaintenance;
using API.DTOs.SalaryMaintenance;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.RewardandPenaltyMaintenance
{
    public class S_8_1_1_RewardAndPenaltyReasonCodeMaintenance : BaseServices, I_8_1_1_RewardAndPenaltyReasonCodeMaintenance
    {
        public S_8_1_1_RewardAndPenaltyReasonCodeMaintenance(DBContext dbContext) : base(dbContext)
        {
        }
        #region Thêm mới dữ liệu
        public async Task<OperationResult> Create(RewardandPenaltyMaintenance_form data)
        {

            if (string.IsNullOrWhiteSpace(data.param.Factory))
                return new OperationResult(false, "InvalidInput");
            try
            {
                var HRRC = await _repositoryAccessor.HRMS_Rew_ReasonCode.FindAll(x => x.Factory == data.param.Factory).ToListAsync();
                List<HRMS_Rew_ReasonCode> addList = new();

                foreach (var item in data.Data)
                {
                    if (string.IsNullOrWhiteSpace(item.Code)
                     || string.IsNullOrWhiteSpace(item.Code_Name)
                     || !DateTime.TryParseExact(item.Update_Time, "yyyy/MM/dd HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime updateTimeValue)
                     || string.IsNullOrWhiteSpace(item.Update_Time)
                    )
                        return new OperationResult(false, "Invalid Input");
                
                    if (HRRC.Any(x => x.Code == item.Code))
                        return new OperationResult(false, "AlreadyExitedData");

                    var dataCreate = new HRMS_Rew_ReasonCode
                    {
                        Factory = data.param.Factory,
                        Code = item.Code.Trim().ToUpper(),
                        Code_Name = item.Code_Name,
                        Update_By = item.Update_By,
                        Update_Time = updateTimeValue
                    };
                    addList.Add(dataCreate);

                }

                _repositoryAccessor.HRMS_Rew_ReasonCode.AddMultiple(addList);

                await _repositoryAccessor.Save();
                return new OperationResult(true, "System.Message.CreateOKMsg");
            }
            catch (System.Exception)
            {
                return new OperationResult(false, "System.Message.CreateErrorMsg");
            }
        }
        #endregion
        #region  Cập nhật dữ liệu
        public async Task<OperationResult> Update(RewardandPenaltyMaintenanceDTO data)
        {
            var item = await _repositoryAccessor.HRMS_Rew_ReasonCode.FirstOrDefaultAsync(x => x.Factory == data.Factory && x.Code == data.Code);

            if (item is null)
            {
                return new OperationResult(false, "Not Exited Data");
            }
            
            item.Code_Name = data.Code_Name;
            item.Update_Time = DateTime.Now;
            item.Update_By = data.Update_By;

            try
            {
                _repositoryAccessor.HRMS_Rew_ReasonCode.Update(item);
                await _repositoryAccessor.Save();
                return new OperationResult(true, "System.Message.UpdateOKMsg");
            }
            catch (System.Exception)
            {
                return new OperationResult(false, "System.Message.UpdateErrorMsg");
            }
        }
        #endregion
        #region xóa dữ liệu 
        public async Task<OperationResult> Delete(RewardandPenaltyMaintenanceDTO data)
        {
            var item = await _repositoryAccessor.HRMS_Rew_ReasonCode.FirstOrDefaultAsync(x => x.Factory == data.Factory && x.Code == data.Code);
            if (item is null)
              return new OperationResult(false, "Not Exited Data");

            try
            {
                _repositoryAccessor.HRMS_Rew_ReasonCode.Remove(item);
                await _repositoryAccessor.Save();
                return new OperationResult(true, "System.Message.DeleteOKMsg");
            }
            catch (System.Exception)
            {
                return new OperationResult(false, "System.Message.DeleteErrorMsg");
            }
        }
        #endregion
        #region download excel
        public async Task<OperationResult> DownloadFileExcel(RewardandPenaltyMaintenanceParam param, string userName)
        {
            List<RewardandPenaltyMaintenanceDTO> data = await GetData(param);
            if (!data.Any()) return new OperationResult(false, "No Data");

            // xử lí report data 
            var dataTables = new List<Table>() { new("result", data) };

            // Thông tin print [ PrintBy,  PrintDay]
            var dataCells = new List<Cell>(){

                new("B2", userName),
                new("D2", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))
            };

            ConfigDownload config = new() { IsAutoFitColumn = true };
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                dataTables,
                dataCells,
                "Resources\\Template\\RewardandPenaltyMaintenance\\8_1_1_RewardAndPenaltyReasonCodeMaintenance\\Download.xlsx",
                config
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }
        #endregion
        #region GetDataPagination 
        private async Task<List<RewardandPenaltyMaintenanceDTO>> GetData(RewardandPenaltyMaintenanceParam param)
        {
            var predicate = PredicateBuilder.New<HRMS_Rew_ReasonCode>(x => x.Factory == param.Factory);

            if (!string.IsNullOrWhiteSpace(param.Reason_Code))
                predicate.And(x => x.Code == param.Reason_Code);
            var data = await _repositoryAccessor.HRMS_Rew_ReasonCode.FindAll(predicate).ToListAsync();

            var result = data.Select(item => new RewardandPenaltyMaintenanceDTO
            {
                Factory = item.Factory,
                Code = item.Code,
                Code_Name = item.Code_Name,
                Update_By = item.Update_By,
                Update_Time = item.Update_Time.ToString("yyyy/MM/dd HH:mm:ss"),
            }).ToList();

            return result;
        }

        public async Task<PaginationUtility<RewardandPenaltyMaintenanceDTO>> GetDataPagination(PaginationParam pagination, RewardandPenaltyMaintenanceParam param)
        {

          List<RewardandPenaltyMaintenanceDTO> data = await GetData(param);
          return PaginationUtility<RewardandPenaltyMaintenanceDTO>.Create(data, pagination.PageNumber, pagination.PageSize);

        }
        #endregion
        #region Get list Data
        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string userName, string language)
        {
            List<string> factorys = await Queryt_Factory_AddList(userName);
            List<KeyValuePair<string, string>> factories = await _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory && factorys.Contains(x.Code), true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (x, y) => new { x, y })
                .SelectMany(x => x.y.DefaultIfEmpty(),
                    (x, y) => new { x.x, y })
                .Select(x => new KeyValuePair<string, string>(x.x.Code, $"{x.x.Code} - {(x.y != null ? x.y.Code_Name : x.x.Code_Name)}"))
                .ToListAsync();
            return factories;
        }

        public async Task<List<KeyValuePair<string, string>>> Query_Reason(string factory)
        {
            var data = _repositoryAccessor.HRMS_Rew_ReasonCode.FindAll(x => x.Factory == factory);
            var result = await data.Select(x => new KeyValuePair<string, string>(x.Code, $"{x.Code} - { x.Code_Name}")).ToListAsync();
            return result;
        }

        public async Task<OperationResult> IsDuplicatedData(RewardandPenaltyMaintenanceDTO param)
        {
            var result = await _repositoryAccessor.HRMS_Rew_ReasonCode.AnyAsync(x =>
                x.Factory == param.Factory &&
                x.Code == param.Code.Trim().ToUpper()
                );
            return new OperationResult(result);
        }
        #endregion
    }
}