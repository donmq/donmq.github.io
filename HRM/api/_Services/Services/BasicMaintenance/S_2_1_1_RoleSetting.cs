using AgileObjects.AgileMapper;
using API.Data;
using API._Services.Interfaces.BasicMaintenance;
using API.DTOs.BasicMaintenance;
using API.Helper.Enums;
using API.Helper.SignalR;
using API.Models;
using LinqKit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.BasicMaintenance
{
    public class S_2_1_1_RoleSetting : BaseServices, I_2_1_1_RoleSetting
    {
        private readonly IHubContext<SignalRHub> _hubContext;

        public S_2_1_1_RoleSetting(DBContext dbContext,IHubContext<SignalRHub> hubContext) : base(dbContext)
        {
            _hubContext = hubContext;
        }

        public async Task<List<KeyValuePair<string, string>>> GetDropDownList(string lang)
        {
            var HBC = await _repositoryAccessor.HRMS_Basic_Code.FindAll().ToListAsync();
            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == lang.ToLower()).ToList();
            var level = await _repositoryAccessor.HRMS_Basic_Level.FindAll().ToListAsync();
            var result = new List<KeyValuePair<string, string>>();
            var data = HBC.GroupJoin(HBCL,
                  x => new { x.Type_Seq, x.Code },
                  y => new { y.Type_Seq, y.Code },
                  (x, y) => new { hbc = x, hbcl = y })
                  .SelectMany(x => x.hbcl.DefaultIfEmpty(),
                  (x, y) => new { x.hbc, hbcl = y });
            result.AddRange(data.Where(x => x.hbc.Type_Seq == "2").Select(x => new KeyValuePair<string, string>("F", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList()); // Factory
            result.AddRange(data.Where(x => x.hbc.Type_Seq == "4").Select(x => new KeyValuePair<string, string>("S", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList()); // Salary_Code
            result.AddRange(level.Select(x => new KeyValuePair<string, string>("L", x.Level.ToString())).Distinct().ToList()); // Level

            return result;
        }

        public async Task<PaginationUtility<RoleSettingDetail>> GetSearchDetail(PaginationParam paginationParams, RoleSettingParam searchParam)
        {
            var data = await GetData(searchParam);
            return PaginationUtility<RoleSettingDetail>.Create(data, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<OperationResult> DownloadExcel(RoleSettingParam param)
        {

            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                await GetData(param),
                "Resources\\Template\\BasicMaintenance\\2_1_1_RoleSetting\\Download.xlsx"
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }

        private async Task<List<RoleSettingDetail>> GetData(RoleSettingParam searchParam)
        {
            var predicateRoleSetting = PredicateBuilder.New<HRMS_Basic_Role>(true);
            if (!string.IsNullOrWhiteSpace(searchParam.Role))
                predicateRoleSetting.And(x => x.Role.Replace(" ", "").ToLower().Contains(searchParam.Role.Replace(" ", "").ToLower()));
            if (!string.IsNullOrWhiteSpace(searchParam.Description))
                predicateRoleSetting.And(x => x.Description.Replace(" ", "").ToLower().Contains(searchParam.Description.Replace(" ", "").ToLower()));
            if (!string.IsNullOrWhiteSpace(searchParam.Factory))
                predicateRoleSetting.And(x => x.Factory.Replace(" ", "").ToLower() == searchParam.Factory.Replace(" ", "").ToLower());
            if (!string.IsNullOrWhiteSpace(searchParam.Permission_Group))
                predicateRoleSetting.And(x => x.Permission_Group.Replace(" ", "").ToLower() == searchParam.Permission_Group.Replace(" ", "").ToLower());
            if (!string.IsNullOrWhiteSpace(searchParam.Direct))
                predicateRoleSetting.And(x => x.Direct.Replace(" ", "").ToLower() == searchParam.Direct.Replace(" ", "").ToLower());
            var basicRole = await _repositoryAccessor.HRMS_Basic_Role
                .FindAll(predicateRoleSetting)
                .Select(x => new RoleSettingDetail
                {
                    Role = x.Role,
                    Description = x.Description,
                    Factory = x.Factory,
                    Permission_Group = x.Permission_Group,
                    Direct = x.Direct,
                    Level_Start = x.Level_Start.ToString(),
                    Level_End = x.Level_End.ToString(),
                    Lang = searchParam.Lang,
                    Update_By = x.Update_By,
                    Update_Time = x.Update_Time
                }).ToListAsync();
            return basicRole;
        }

        private async Task<List<TreeviewItem>> GetProgramGroup(RoleSettingParam param, string onForm)
        {
            var HSD = _repositoryAccessor.HRMS_SYS_Directory.FindAll();
            var HSP = _repositoryAccessor.HRMS_SYS_Program.FindAll();
            var HSPF = _repositoryAccessor.HRMS_SYS_Program_Function.FindAll();
            var HSPFC = _repositoryAccessor.HRMS_SYS_Program_Function_Code.FindAll();
            var HSPL_Directory = _repositoryAccessor.HRMS_SYS_Program_Language
                .FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower() && x.Kind == "D");
            var HSPL_Program = _repositoryAccessor.HRMS_SYS_Program_Language
                .FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower() && x.Kind == "P");
            var HBRPG = _repositoryAccessor.HRMS_Basic_Role_Program_Group.FindAll(x => x.Role == param.Role);
            var directoryDataList = await HSD
                .GroupJoin(HSPL_Directory,
                    x => x.Directory_Code,
                    y => y.Code,
                    (x, y) => new { HSD = x, HSPL_Dir_Lang = y })
                .SelectMany(x => x.HSPL_Dir_Lang.DefaultIfEmpty(),
                    (x, y) => new { x.HSD, HSPL_Dir_Lang = y })
                .GroupJoin(HSP,
                    x => x.HSD.Directory_Code,
                    y => y.Parent_Directory_Code,
                    (x, y) => new { x.HSD, x.HSPL_Dir_Lang, HSP = y })
                .SelectMany(x => x.HSP.DefaultIfEmpty(),
                    (x, y) => new { x.HSD, x.HSPL_Dir_Lang, HSP = y })
                .GroupJoin(HSPL_Program,
                    x => x.HSP.Program_Code,
                    y => y.Code,
                    (x, y) => new { x.HSD, x.HSPL_Dir_Lang, x.HSP, HSPL_Pro_Lang = y })
                .SelectMany(x => x.HSPL_Pro_Lang.DefaultIfEmpty(),
                    (x, y) => new { x.HSD, x.HSPL_Dir_Lang, x.HSP, HSPL_Pro_Lang = y })
                .GroupJoin(HSPF,
                    x => x.HSP.Program_Code,
                    y => y.Program_Code,
                    (x, y) => new { x.HSD, x.HSPL_Dir_Lang, x.HSP, x.HSPL_Pro_Lang, HSPF = y })
                .SelectMany(x => x.HSPF.DefaultIfEmpty(),
                    (x, y) => new { x.HSD, x.HSPL_Dir_Lang, x.HSP, x.HSPL_Pro_Lang, HSPF = y })
                .GroupJoin(HSPFC,
                    x => x.HSPF.Fuction_Code,
                    y => y.Fuction_Code,
                    (x, y) => new { x.HSD, x.HSPL_Dir_Lang, x.HSP, x.HSPL_Pro_Lang, x.HSPF, HSPFC_Lang = y })
                .SelectMany(x => x.HSPFC_Lang.DefaultIfEmpty(),
                    (x, y) => new { x.HSD, x.HSPL_Dir_Lang, x.HSP, x.HSPL_Pro_Lang, x.HSPF, HSPFC_Lang = y })
                .GroupJoin(HBRPG,
                    x => new { x.HSP.Program_Code, x.HSPF.Fuction_Code },
                    y => new { y.Program_Code, y.Fuction_Code },
                    (x, y) => new { x.HSD, x.HSPL_Dir_Lang, x.HSP, x.HSPL_Pro_Lang, x.HSPF, x.HSPFC_Lang, HBRPG_Role = y })
                .SelectMany(x => x.HBRPG_Role.DefaultIfEmpty(),
                    (x, y) => new { x.HSD, x.HSPL_Dir_Lang, x.HSP, x.HSPL_Pro_Lang, x.HSPF, x.HSPFC_Lang, HBRPG_Role = y })
                .GroupBy(x => new { x.HSD.Seq, x.HSD.Directory_Code, x.HSD.Directory_Name })
                .OrderBy(x => x.Key.Seq)
                .Select(x => new TreeviewItem
                {
                    text = $"{x.Key.Seq}. {x.FirstOrDefault().HSPL_Dir_Lang.Name ?? x.Key.Directory_Name}",
                    value = x.Key.Directory_Code,
                    @disabled = onForm == "Main" || !x.Any(y => y.HSP != null),
                    children = x.Where(y => y.HSP != null).GroupBy(y => new { y.HSP.Seq, y.HSP.Program_Code, y.HSP.Program_Name })
                        .OrderBy(y => y.Key.Seq).Select(y => new TreeviewItem
                        {
                            text = $"{y.Key.Program_Code}. {y.FirstOrDefault().HSPL_Pro_Lang.Name ?? y.Key.Program_Name}",
                            value = y.Key.Program_Code,
                            @disabled = onForm == "Main" || !y.Any(z => z.HSPF != null),
                            @collapsed = onForm != "Main",
                            children = y.Where(z => z.HSPF != null).GroupBy(z => new { z.HSPF.Fuction_Code })
                                .OrderBy(z => z.Key.Fuction_Code).Select(z => new TreeviewItem
                                {
                                    text = param.Lang.ToLower() == "tw"
                                        ? z.FirstOrDefault().HSPFC_Lang.Fuction_Name_TW
                                        : z.FirstOrDefault().HSPFC_Lang.Fuction_Name_EN,
                                    value = $"{y.Key.Program_Code}/{z.Key.Fuction_Code}",
                                    @checked = !string.IsNullOrWhiteSpace(param.Role) && z.Any(w => w.HBRPG_Role != null)
                                })
                        })
                }).ToListAsync();
            if (onForm == "Main")
                return directoryDataList;
            List<TreeviewItem> roleList = new()
            {
                new TreeviewItem{
                    text = param.Lang.ToLower() == "tw" ? "全選" :"All" ,
                    value = "All",
                    children = directoryDataList,
                    @disabled =  onForm == "Main" || !directoryDataList.Any(),
                }
            };
            return roleList;
        }
        public async Task<List<TreeviewItem>> GetProgramGroupDetail(RoleSettingParam param)
        {
            var programGroup = await GetProgramGroup(param, "Main");
            return programGroup;
        }
        public async Task<List<TreeviewItem>> GetProgramGroupTemplate(string lang)
        {
            var param = new RoleSettingParam() { Lang = lang };
            var programGroup = await GetProgramGroup(param, "Add");
            return programGroup;
        }
        public async Task<RoleSettingDto> GetRoleSettingEdit(RoleSettingParam param)
        {
            var basicRole = await _repositoryAccessor.HRMS_Basic_Role.FirstOrDefaultAsync(x => x.Role.Trim() == param.Role.Trim());
            var programGroup = await GetProgramGroup(param, "Edit");
            var result = new RoleSettingDto()
            {
                Role_Setting = Mapper.Map(basicRole).ToANew<RoleSetting>(x => x.MapEntityKeys()),
                Role_List = programGroup
            };
            return result;
        }

        public async Task<OperationResult> PostRole(RoleSettingDto data, string userName)
        {
            var predicateRoleSetting = PredicateBuilder.New<HRMS_Basic_Role>(true);
            if (!string.IsNullOrWhiteSpace(data.Role_Setting.Role))
                predicateRoleSetting.And(x => x.Role.Replace(" ", "").ToLower() == data.Role_Setting.Role.Replace(" ", "").ToLower());
            if (!string.IsNullOrWhiteSpace(data.Role_Setting.Factory))
                predicateRoleSetting.And(x => x.Factory.Replace(" ", "").ToLower() == data.Role_Setting.Factory.Replace(" ", "").ToLower());
            if (await _repositoryAccessor.HRMS_Basic_Role.AnyAsync(predicateRoleSetting))
                return new OperationResult { IsSuccess = false, Error = "Already exist role with selected factory" };
            List<HRMS_Basic_Role_Program_Group> addList = new();
            HRMS_Basic_Role basicRole = new()
            {
                Role = data.Role_Setting.Role.Replace(" ", ""),
                Description = data.Role_Setting.Description.Trim(),
                Factory = data.Role_Setting.Factory,
                Permission_Group = data.Role_Setting.Permission_Group,
                Level_Start = decimal.Parse(data.Role_Setting.Level_Start),
                Level_End = decimal.Parse(data.Role_Setting.Level_End),
                Direct = data.Role_Setting.Direct,
                Update_By = userName,
                Update_Time = DateTime.Now
            };
            var selectedRoles = data.Role_List.Select(x => x.value).ToList();
            foreach (var item in selectedRoles)
            {
                string[] temp = item.Split('/');
                HRMS_Basic_Role_Program_Group add = new()
                {
                    Role = basicRole.Role,
                    Program_Code = temp[0],
                    Fuction_Code = temp[1],
                    Update_By = basicRole.Update_By,
                    Update_Time = basicRole.Update_Time
                };
                addList.Add(add);
            }
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                var removeList = _repositoryAccessor.HRMS_Basic_Role_Program_Group.FindAll(x => x.Role.ToLower().Trim() == basicRole.Role.ToLower().Trim()).ToList();
                if (removeList.Any())
                    _repositoryAccessor.HRMS_Basic_Role_Program_Group.RemoveMultiple(removeList);
                _repositoryAccessor.HRMS_Basic_Role.Add(basicRole);
                _repositoryAccessor.HRMS_Basic_Role_Program_Group.AddMultiple(addList);
                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false);
            }
        }

        public async Task<OperationResult> PutRole(RoleSettingDto data, string userName)
        {
            var predicateRoleSetting = PredicateBuilder.New<HRMS_Basic_Role>(true);
            if (!string.IsNullOrWhiteSpace(data.Role_Setting.Role))
                predicateRoleSetting.And(x => x.Role.Replace(" ", "").ToLower() == data.Role_Setting.Role.Replace(" ", "").ToLower());
            if (!string.IsNullOrWhiteSpace(data.Role_Setting.Factory))
                predicateRoleSetting.And(x => x.Factory.Replace(" ", "").ToLower() == data.Role_Setting.Factory.Replace(" ", "").ToLower());
            var role = await _repositoryAccessor.HRMS_Basic_Role.FirstOrDefaultAsync(predicateRoleSetting);
            if (role == null)
                return new OperationResult { IsSuccess = false, Error = "Not Exits Role" };
            role.Description = data.Role_Setting.Description;
            role.Direct = data.Role_Setting.Direct;
            role.Level_Start = decimal.Parse(data.Role_Setting.Level_Start);
            role.Level_End = decimal.Parse(data.Role_Setting.Level_End);
            role.Permission_Group = data.Role_Setting.Permission_Group;
            role.Update_By = userName;
            role.Update_Time = DateTime.Now;
            List<HRMS_Basic_Role_Program_Group> addList = new();
            var selectedRoles = data.Role_List.Select(x => x.value).ToList();
            foreach (var item in selectedRoles)
            {
                if (!item.Contains('/'))
                    return new OperationResult { IsSuccess = false, Error = $"No function on selected role : {item}" };
                string[] temp = item.Split('/');
                HRMS_Basic_Role_Program_Group add = new()
                {
                    Role = data.Role_Setting.Role.Trim(),
                    Program_Code = temp[0],
                    Fuction_Code = temp[1],
                    Update_By = role.Update_By,
                    Update_Time = role.Update_Time
                };
                addList.Add(add);
            }
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                _repositoryAccessor.HRMS_Basic_Role.Update(role);
                var removeList = await _repositoryAccessor.HRMS_Basic_Role_Program_Group.FindAll(x => x.Role.Trim() == data.Role_Setting.Role.Trim()).ToListAsync();
                if (removeList.Any())
                    _repositoryAccessor.HRMS_Basic_Role_Program_Group.RemoveMultiple(removeList);
                await _repositoryAccessor.Save();
                if (addList.Any())
                    _repositoryAccessor.HRMS_Basic_Role_Program_Group.AddMultiple(addList);
                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                var accountList = _repositoryAccessor.HRMS_Basic_Account_Role.FindAll(x => x.Role == role.Role && x.Account != userName).Select(x => x.Account).ToList();
                if (accountList.Any())
                    await _hubContext.Clients.All.SendAsync(SignalRConstants.ACCOUNT_CHANGED, accountList);
                return new OperationResult(true);
            }
            catch (Exception)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false);
            }
        }

        public async Task<OperationResult> DeleteRole(string role, string factory)
        {
            var predicateRoleSetting = PredicateBuilder.New<HRMS_Basic_Role>(true);
            if (!string.IsNullOrWhiteSpace(role))
                predicateRoleSetting.And(x => x.Role.Trim() == role.Trim());
            if (!string.IsNullOrWhiteSpace(factory))
                predicateRoleSetting.And(x => x.Factory.Trim() == factory.Trim());
            var roleData = await _repositoryAccessor.HRMS_Basic_Role.FirstOrDefaultAsync(predicateRoleSetting);
            if (await _repositoryAccessor.HRMS_Basic_Account_Role.AnyAsync(x => x.Role.Trim() == role.Trim()))
                return new OperationResult { IsSuccess = false, Error = "The role has been authorized\nIt cannot be deleted" };
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                var removeList = _repositoryAccessor.HRMS_Basic_Role_Program_Group.FindAll(x => x.Role.Trim() == role.Trim()).ToList();
                if (removeList.Any())
                    _repositoryAccessor.HRMS_Basic_Role_Program_Group.RemoveMultiple(removeList);
                _repositoryAccessor.HRMS_Basic_Role.Remove(roleData);
                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false);
            }
        }

        public async Task<OperationResult> CheckRole(string role, List<string> roleList)
        {
            return await Task.FromResult(new OperationResult(roleList.Any(x => x == role)));
        }
    }
}