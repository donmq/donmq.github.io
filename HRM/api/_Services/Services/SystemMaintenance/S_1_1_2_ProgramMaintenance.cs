using AgileObjects.AgileMapper;
using API.Data;
using API._Services.Interfaces.SystemMaintenance;
using API.DTOs.SystemMaintenance;
using API.Helper.Enums;
using API.Helper.SignalR;
using API.Models;
using LinqKit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.SystemMaintenance
{
    public class S_1_1_2_ProgramMaintenance : BaseServices, I_1_1_2_ProgramMaintenance
    {
        private readonly IHubContext<SignalRHub> _hubContext;

        public S_1_1_2_ProgramMaintenance(DBContext dbContext,IHubContext<SignalRHub> hubContext) : base(dbContext)
        {
            _hubContext = hubContext;
        }

        public async Task<PaginationUtility<ProgramMaintenance_Data>> Getdata(PaginationParam pagination, ProgramMaintenance_Param param)
        {
            var predicate = PredicateBuilder.New<HRMS_SYS_Program>(true);
            if (!string.IsNullOrEmpty(param.Program_Code))
                predicate = predicate.And(x => x.Program_Code.Trim().ToUpper().Contains(param.Program_Code.ToUpper()));
            if (!string.IsNullOrEmpty(param.Program_Name))
                predicate = predicate.And(x => x.Program_Name.Trim().ToUpper().Contains(param.Program_Name.Trim().ToUpper()));
            if (!string.IsNullOrEmpty(param.Parent_Directory_Code))
                predicate = predicate.And(x => x.Parent_Directory_Code.Contains(param.Parent_Directory_Code));
            var data = await _repositoryAccessor.HRMS_SYS_Program
                .FindAll(predicate)
                .Project()
                .To<ProgramMaintenance_Data>()
                .OrderBy(x => x.Parent_Directory_Code)
                .ThenBy(x => x.Seq)
                .ToListAsync();
            var programCodes = data.Select(item => item.Program_Code).Distinct().ToList();
            var functions = await _repositoryAccessor.HRMS_SYS_Program_Function
                .FindAll(x => programCodes.Contains(x.Program_Code))
                .GroupJoin(
                    _repositoryAccessor.HRMS_SYS_Program_Function_Code.FindAll(),
                    x => x.Fuction_Code,
                    y => y.Fuction_Code,
                    (x, y) => new { function = x, functionCode = y }
                )
                .ToListAsync();
            data.ForEach(item =>
            {
                var itemFunctions = functions
                    .Where(x => x.function.Program_Code == item.Program_Code)
                    .SelectMany(x => x.functionCode.DefaultIfEmpty(), (x, y) => y?.Fuction_Name_EN)
                    .ToList();

                item.Functions = itemFunctions;
            });
            return PaginationUtility<ProgramMaintenance_Data>.Create(data, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<List<KeyValuePair<string, string>>> GetDirectory()
        {
            return await _repositoryAccessor.HRMS_SYS_Directory.FindAll().OrderBy(x => x.Seq).Select(x => new KeyValuePair<string, string>(x.Directory_Code, x.Directory_Name)).ToListAsync();
        }
        public async Task<List<KeyValuePair<string, string>>> GetFunction_ALL()
        {
            return await _repositoryAccessor.HRMS_SYS_Program_Function_Code.FindAll().Select(x => new KeyValuePair<string, string>(x.Fuction_Code, x.Fuction_Name_EN + " - " + x.Fuction_Name_TW)).ToListAsync();
        }
        public async Task<OperationResult> AddNew(ProgramMaintenance_Data model)
        {
            if (await _repositoryAccessor.HRMS_SYS_Program.AnyAsync(x => x.Program_Code.Trim().ToUpper() == model.Program_Code.Trim().ToUpper()))
                return new OperationResult(false, "Already exist");
            // Add bảng HRMS_SYS_Program
            model.Update_Time = DateTime.Now;
            var hRMS_SYS_Program = Mapper.Map(model).ToANew<HRMS_SYS_Program>(x => x.MapEntityKeys());
            _repositoryAccessor.HRMS_SYS_Program.Add(hRMS_SYS_Program);
            // Add bảng HRMS_SYS_Program_Function
            foreach (var i in model.Functions)
            {
                if (await _repositoryAccessor.HRMS_SYS_Program_Function.AnyAsync(x => x.Program_Code.ToUpper() == model.Program_Code.Trim().ToUpper() && x.Fuction_Code == i))
                    return new OperationResult(false);
                HRMS_SYS_Program_Function rs = new()
                {
                    Program_Code = model.Program_Code.Trim(),
                    Fuction_Code = i
                };
                _repositoryAccessor.HRMS_SYS_Program_Function.Add(rs);
            }

            if (await _repositoryAccessor.Save())
            {
                return new OperationResult(true, "Add Successfully");
            }
            return new OperationResult(false, "Add failed");
        }

        public async Task<OperationResult> Edit(ProgramMaintenance_Data model)
        {
            var kiemtra = await _repositoryAccessor.HRMS_SYS_Program.FirstOrDefaultAsync(
                x => x.Program_Code.Trim().ToUpper() == model.Program_Code.Trim().ToUpper());
            if (kiemtra == null)
            {
                return new OperationResult(false, "Does not exist");
            }
            else
            {
                var now = DateTime.Now;
                // Check exits bảng HRMS_Basic_Role_Program_Group
                var Bs_Role_Program_Group = await _repositoryAccessor.HRMS_Basic_Role_Program_Group.FindAll(x => x.Program_Code.ToUpper() == model.Program_Code.Trim().ToUpper())
                .GroupBy(x => x.Fuction_Code).Select(x => x.Key).ToListAsync();
                if (!Bs_Role_Program_Group.All(model.Functions.Contains))
                {
                    return new OperationResult(false, "SystemMaintenance.ProgramMaintenance.UpdateFunctionError");
                }
                List<HRMS_SYS_Program_Function> functionUpdate = new();
                foreach (var i in model.Functions)
                {
                    HRMS_SYS_Program_Function rs = new()
                    {
                        Fuction_Code = i,
                        Program_Code = model.Program_Code.Trim(),
                        Update_By = model.Update_By,
                        Update_Time = now
                    };
                    functionUpdate.Add(rs);
                }
                // Update bảng HRMS_SYS_Program_Function
                var originalItem = await _repositoryAccessor.HRMS_SYS_Program_Function.FindAll(x => x.Program_Code.ToUpper() == model.Program_Code.Trim().ToUpper()).ToListAsync();
                if (originalItem != null)
                {
                    _repositoryAccessor.HRMS_SYS_Program_Function.RemoveMultiple(originalItem);
                }
                _repositoryAccessor.HRMS_SYS_Program_Function.AddMultiple(functionUpdate);
                // Update bảng HRMS_SYS_Program
                kiemtra.Update_Time = now;
                kiemtra.Program_Code = model.Program_Code.Trim();
                kiemtra.Program_Name = model.Program_Name.Trim();
                kiemtra.Seq = model.Seq;
                kiemtra.Update_By = model.Update_By;
                kiemtra.Parent_Directory_Code = model.Parent_Directory_Code;
                _repositoryAccessor.HRMS_SYS_Program.Update(kiemtra);
            }
            if (await _repositoryAccessor.Save())
            {
                //Get all account have selected Program_Code
                var HBAR = _repositoryAccessor.HRMS_Basic_Account_Role.FindAll();
                var HBRPG = _repositoryAccessor.HRMS_Basic_Role_Program_Group.FindAll(x => x.Program_Code == model.Program_Code);
                var _data = HBAR
                    .Join(HBRPG,
                        x => x.Role,
                        y => y.Role,
                        (x, y) => new { HBAR = x, HBRPG = y })
                    .GroupBy(x => x.HBAR);
                var accountList = _data.Where(x => x.Key.Account != model.Update_By).Select(x => x.Key.Account).Distinct().ToList();
                if (accountList.Any())
                    await _hubContext.Clients.All.SendAsync(SignalRConstants.ACCOUNT_CHANGED, accountList);
                return new OperationResult(true, "Update Successfully");
            }
            return new OperationResult(false, "System.Caption.Error");
        }
        public async Task<OperationResult> Delete(string Program_Code)
        {
            if (await _repositoryAccessor.HRMS_SYS_Program_Function.AnyAsync(x => x.Program_Code.ToUpper() == Program_Code.Trim().ToUpper()))
                return new OperationResult(false, "SystemMaintenance.ProgramMaintenance.DeleteProgramError");
            var kiemtra_HRMS_SYS_Program = await _repositoryAccessor.HRMS_SYS_Program.FirstOrDefaultAsync(
            x => x.Program_Code.Trim().ToUpper() == Program_Code.Trim().ToUpper());
            if (kiemtra_HRMS_SYS_Program == null)
                return new OperationResult(false, "SystemMaintenance.ProgramMaintenance.ProgramNotExistedError");
            _repositoryAccessor.HRMS_SYS_Program.Remove(kiemtra_HRMS_SYS_Program);
            await _repositoryAccessor.Save();
            return new OperationResult(true);
        }
    }
}