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
    public class S_1_1_1_DirectoryMaintenance : BaseServices, I_1_1_1_DirectoryMaintenance
    {
        private readonly IHubContext<SignalRHub> _hubContext;

        public S_1_1_1_DirectoryMaintenance(DBContext dbContext,IHubContext<SignalRHub> hubContext) : base(dbContext)
        {
            _hubContext = hubContext;
        }

        public async Task<OperationResult> Add(DirectoryMaintenance_Data param)
        {
            if (await _repositoryAccessor.HRMS_SYS_Directory.AnyAsync(x => x.Directory_Code.Trim() == param.Directory_Code.Trim()))
                return new OperationResult(false, "This Directory Code is already exist");
            if (!string.IsNullOrWhiteSpace(param.Directory_Code))
            {
                var item = Mapper.Map(param).ToANew<HRMS_SYS_Directory>(x => x.MapEntityKeys());
                _repositoryAccessor.HRMS_SYS_Directory.Add(item);
            }
            else
            {
                return new OperationResult(false, "Please add Directory Code");
            }
            try
            {
                await _repositoryAccessor.Save();
                return new OperationResult(true, "Add Successfully");
            }
            catch (Exception)
            {
                return new OperationResult(false, "Cannot Add");
            }
        }

        public async Task<OperationResult> Delete(string directoryCode)
        {
            var checkID = _repositoryAccessor.HRMS_SYS_Directory.FirstOrDefault(x => x.Directory_Code.Trim() == directoryCode.Trim());
            if (checkID == null)
                return new OperationResult(false, "Error, Supplied Directory Code is not existed");
            // Nếu Directory_Code được dùng cho HRMS_SYS_Program.Parent_Directory_Code, không được xóa
            var directory = _repositoryAccessor.HRMS_SYS_Directory.FindAll(x => x.Directory_Code.Trim() == directoryCode.Trim());
            var program = _repositoryAccessor.HRMS_SYS_Program.FindAll(x => x.Parent_Directory_Code.Trim() == directoryCode.Trim());
            var data = directory.Join(program,
                                x => x.Directory_Code,
                                y => y.Parent_Directory_Code,
                                (x, y) => new { Directory = x, Program = y })
                                .ToList().Count();
            // Kiểm tra HRMS_SYS_Program.Parent_Directory_Code >= 1, không được xóa
            if (data >= 1)
                return new OperationResult(false, "Error, This Directory Code is using by Program");
            // if (await _repositoryAccessor.HRMS_SYS_Directory.AnyAsync(x => x.Parent_Directory_Code.Trim() == directoryCode.Trim()))
            //     return new OperationResult(false, "Error, This Directory Code is using as Parent Directory Code by another Directory Code");
            _repositoryAccessor.HRMS_SYS_Directory.Remove(checkID);
            await _repositoryAccessor.Save();
            return new OperationResult(true, "Delete Successfully");
        }

        public async Task<List<KeyValuePair<string, string>>> GetParentDirectoryCode()
        {
            return await _repositoryAccessor.HRMS_SYS_Directory.FindAll().Select(x => new KeyValuePair<string, string>(x.Directory_Code, x.Directory_Code))
            .Distinct().ToListAsync();
        }

        public async Task<PaginationUtility<DirectoryMaintenance_Data>> Search(PaginationParam paginationParams, DirectoryMaintenance_Param param)
        {
            var pred = PredicateBuilder.New<HRMS_SYS_Directory>(true);

            if (!string.IsNullOrEmpty(param.Directory_Code))
            {
                pred.And(x => x.Directory_Code == param.Directory_Code.Trim());
            }
            if (!string.IsNullOrEmpty(param.Directory_Name))
            {
                pred.And(x => x.Directory_Name == param.Directory_Name.Trim());
            }
            if (!string.IsNullOrEmpty(param.Parent_Directory_Code))
            {
                pred.And(x => x.Parent_Directory_Code == param.Parent_Directory_Code.Trim());
            }

            var data = await _repositoryAccessor.HRMS_SYS_Directory.FindAll(pred).Project().To<DirectoryMaintenance_Data>().OrderBy(x => x.Seq).ToListAsync();
            return PaginationUtility<DirectoryMaintenance_Data>.Create(data, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<OperationResult> Update(DirectoryMaintenance_Data model)
        {
            if (await _repositoryAccessor.HRMS_SYS_Directory.AnyAsync(x => x.Directory_Code.Trim() == model.Directory_Code.Trim()))
            {
                var item = Mapper.Map(model).ToANew<HRMS_SYS_Directory>(x => x.MapEntityKeys());
                _repositoryAccessor.HRMS_SYS_Directory.Update(item);
            }
            else
            {
                return new OperationResult(false, "Can't find this Class_Code");
            }
            if (await _repositoryAccessor.Save())
            {
                //Get all account have selected Direction_Code
                var HBAR = _repositoryAccessor.HRMS_Basic_Account_Role.FindAll();
                var HBRPG = _repositoryAccessor.HRMS_Basic_Role_Program_Group.FindAll();
                var HSP = _repositoryAccessor.HRMS_SYS_Program.FindAll(x => x.Parent_Directory_Code == model.Directory_Code);
                var _data = HBAR
                    .Join(HBRPG,
                        x => x.Role,
                        y => y.Role,
                        (x, y) => new { HBAR = x, HBRPG = y })
                    .Join(HSP,
                        x => x.HBRPG.Program_Code,
                        y => y.Program_Code,
                        (x, y) => new { x.HBAR, x.HBRPG, HSP = y })
                    .GroupBy(x => x.HBAR);
                var accountList = _data.Where(x => x.Key.Account != model.Update_By).Select(x => x.Key.Account).Distinct().ToList();
                if (accountList.Any())
                    await _hubContext.Clients.All.SendAsync(SignalRConstants.ACCOUNT_CHANGED, accountList);
                return new OperationResult(true, "Update Successfully");
            }
            return new OperationResult(false, "Update failed");
        }
    }
}