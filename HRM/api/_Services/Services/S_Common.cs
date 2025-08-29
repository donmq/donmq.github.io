using System.Data;
using API.Data;
using API._Services.Interfaces;
using API.Dtos.Auth;
using API.DTOs;
using API.Helper.Constant;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services;
public class S_Common : BaseServices, I_Common
{
    public S_Common(DBContext dbContext) : base(dbContext)
    {
    }
    public async Task<AuthProgram> GetAuthProgram(string username)
    {
        var user = _repositoryAccessor.HRMS_Basic_Account.FindAll(x => x.Account.Trim() == username.Trim());
        var roleUsers = _repositoryAccessor.HRMS_Basic_Account_Role.FindAll(x => x.Account.Trim() == username.Trim());
        var roles = _repositoryAccessor.HRMS_Basic_Role.FindAll();
        var groupRoles = _repositoryAccessor.HRMS_Basic_Role_Program_Group.FindAll();
        var sys_Directory = _repositoryAccessor.HRMS_SYS_Directory.FindAll();
        var sys_Program = _repositoryAccessor.HRMS_SYS_Program.FindAll();
        var sys_Function = _repositoryAccessor.HRMS_SYS_Program_Function.FindAll();

        var data = await user
           .GroupJoin(roleUsers,
               x => x.Account,
               y => y.Account,
               (x, y) => new { user = x, roleUsers = y })
           .SelectMany(x => x.roleUsers.DefaultIfEmpty(),
               (x, y) => new { x.user, roleUsers = y })
           .GroupJoin(roles,
               x => x.roleUsers.Role,
               y => y.Role,
               (x, y) => new { x.user, x.roleUsers, roles = y })
           .SelectMany(x => x.roles.DefaultIfEmpty(),
               (x, y) => new { x.user, x.roleUsers, roles = y })
           .GroupJoin(groupRoles,
               x => x.roles.Role,
               y => y.Role,
               (x, y) => new { x.user, x.roleUsers, x.roles, groupRoles = y })
           .SelectMany(x => x.groupRoles.DefaultIfEmpty(),
               (x, y) => new { x.user, x.roleUsers, x.roles, groupRoles = y })
           .GroupJoin(sys_Program,
               x => x.groupRoles.Program_Code,
               y => y.Program_Code,
               (x, y) => new { x.user, x.roleUsers, x.roles, x.groupRoles, sys_Program = y })
           .SelectMany(x => x.sys_Program.DefaultIfEmpty(),
               (x, y) => new { x.user, x.roleUsers, x.roles, x.groupRoles, sys_Program = y })
           .GroupJoin(sys_Directory,
               x => x.sys_Program.Parent_Directory_Code,
               y => y.Directory_Code,
               (x, y) => new { x.user, x.roleUsers, x.roles, x.groupRoles, x.sys_Program, sys_Directory = y })
           .SelectMany(x => x.sys_Directory.DefaultIfEmpty(),
               (x, y) => new { x.user, x.roleUsers, x.roles, x.groupRoles, x.sys_Program, sys_Directory = y })
           .GroupJoin(sys_Function,
               x => new { x.groupRoles.Program_Code, x.groupRoles.Fuction_Code },
               y => new { y.Program_Code, y.Fuction_Code },
               (x, y) => new { x.user, x.roleUsers, x.roles, x.groupRoles, x.sys_Program, x.sys_Directory, sys_Function = y })
           .SelectMany(x => x.sys_Function.DefaultIfEmpty(),
               (x, y) => new { x.user, x.roleUsers, x.roles, x.groupRoles, x.sys_Program, x.sys_Directory, sys_Function = y })
           .GroupBy(x => x.user)
           .ToListAsync();
        var userLogged = data
            .Select(x => new AuthProgram
            {
                Directories = x.Where(y => y.sys_Directory != null)
                    .GroupBy(y => y.sys_Directory)
                    .OrderBy(y => y.Key.Seq)
                    .Select(y => new DirectoryInfomation
                    {
                        Seq = y.Key.Seq,
                        Directory_Code = y.Key.Directory_Code,
                        Directory_Name = y.Key.Directory_Name
                    }),
                Programs = x.Where(y => y.sys_Program != null)
                    .GroupBy(y => y.sys_Program).OrderBy(y => y.Key.Parent_Directory_Code).ThenBy(y => y.Key.Seq)
                    .Select(y => new ProgramInfomation
                    {
                        Seq = y.Key.Seq != null ? (int)y.Key.Seq : 0,
                        Parent_Directory_Code = y.Key.Parent_Directory_Code,
                        Program_Code = y.Key.Program_Code,
                        Program_Name = y.Key.Program_Name,
                    }),
                Functions = x.Where(y => y.sys_Function != null)
                    .GroupBy(y => y.sys_Function).OrderBy(y => y.Key.Program_Code).ThenBy(y => y.Key.Fuction_Code)
                    .Select(y => new FunctionInfomation
                    {
                        Program_Code = y.Key.Program_Code,
                        Function_Code = y.Key.Fuction_Code,
                    })
            }).FirstOrDefault();
        if (!userLogged.Programs.Any(x => x.Program_Code == "2.1.8"))
        {
            var resetPasswordRole = sys_Program.Where(x => x.Program_Code == "2.1.8").Select(x => new ProgramInfomation
            {
                Seq = x.Seq != null ? (int)x.Seq : 0,
                Parent_Directory_Code = x.Parent_Directory_Code,
                Program_Code = x.Program_Code,
                Program_Name = x.Program_Name,
            }).FirstOrDefault();
            userLogged.Programs = userLogged.Programs.Append(resetPasswordRole);
        }
        return userLogged;
    }
    public async Task<bool?> GetPasswordReset(string username)
    {
        var user = await _repositoryAccessor.HRMS_Basic_Account.FirstOrDefaultAsync(x => x.Account == username);
        return user.Password_Reset;
    }
    // code = 2 (2.2.Query_HRMS_Basic_Code_List )
    public async Task<List<KeyValuePair<string, string>>> GetListFactoryMain(string language)
    {
        return await GetBasicCodeList(language, BasicCodeTypeConstant.Factory);
    }

    // code = 4 (2.2.Query_HRMS_Basic_Code_List )
    public async Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string language)
    {
        return await GetBasicCodeList(language, BasicCodeTypeConstant.PermissionGroup);
    }

    // code = 40 (2.2.Query_HRMS_Basic_Code_List )
    public async Task<List<KeyValuePair<string, string>>> GetListAttendanceOrLeave(string language)
    {
        return await GetBasicCodeList(language, BasicCodeTypeConstant.Leave);
    }

    // code = 41 (2.2.Query_HRMS_Basic_Code_List )
    public async Task<List<KeyValuePair<string, string>>> GetListWorkShiftType(string language)
    {
        return await GetBasicCodeList(language, BasicCodeTypeConstant.WorkShiftType);
    }

    // code = 43 (2.2.Query_HRMS_Basic_Code_List )
    public async Task<List<KeyValuePair<string, string>>> GetListReasonCode(string language)
    {
        return await GetBasicCodeList(language, BasicCodeTypeConstant.ReasonCode);
    }

    // code = 45 (2.2.Query_HRMS_Basic_Code_List )
    public async Task<List<KeyValuePair<string, string>>> GetListSalaryItems(string language)
    {
        return await GetBasicCodeList(language, BasicCodeTypeConstant.SalaryItem);
    }



    // 2.11.Query_Department_List
    public async Task<List<KeyValuePair<string, string>>> GetListDepartment(string language, string factory)
    {
        var data = await _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Factory == factory, true)
            .Join(_repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(b => b.Kind == "1" && b.Factory == factory, true),
                department => department.Division,
                factoryComparison => factoryComparison.Division,
                (department, factoryComparison) => department)
            .GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                department => new { department.Factory, department.Department_Code },
                language => new { language.Factory, language.Department_Code },
                (department, language) => new { Department = department, Language = language })
            .SelectMany(
                x => x.Language.DefaultIfEmpty(),
                (x, language) => new { x.Department, Language = language })
            .OrderBy(x => x.Department.Department_Code)
            .Select(
                x => new KeyValuePair<string, string>(
                    x.Department.Department_Code,
                    $"{x.Department.Department_Code} - {(x.Language != null ? x.Language.Name : x.Department.Department_Name)}"
                )
            ).Distinct().ToListAsync();

        return data;
    }


    // 2.3.Query_HRMS_Basic_Code_Char1
    // input: language, Type_Seq, kind, inputChar (Leave | Attendance,.....)
    public async Task<List<KeyValuePair<string, string>>> GetListBasicCodeListChar1(string language, string typeSeq, int kind, string inputChar)
    {
        return await _repositoryAccessor.HRMS_Basic_Code
            .FindAll(x => x.Type_Seq == typeSeq && ((kind == 1 && x.Char1 == inputChar) || (kind != 1 && x.Char2 == inputChar)), true)
            .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                HBC => new { HBC.Type_Seq, HBC.Code },
                HBCL => new { HBCL.Type_Seq, HBCL.Code },
                (HBC, HBCL) => new { HBC, HBCL })
                .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                (prev, HBCL) => new { prev.HBC, HBCL })
            .Select(x => new KeyValuePair<string, string>(x.HBC.Code, $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"))
            .ToListAsync();
    }

    #region GetListAccountAdd
    public async Task<List<KeyValuePair<string, string>>> GetListAccountAdd(string userName, string language)
    {
        var factories = await _repositoryAccessor.HRMS_Basic_Account_Role.FindAll(x => x.Account.ToLower() == userName.ToLower(), true)
            .Join(_repositoryAccessor.HRMS_Basic_Role.FindAll(true),
                x => x.Role,
                y => y.Role,
                (x, y) => new { accRole = x, role = y })
            .Select(x => x.role.Factory)
            .Distinct().ToListAsync();

        if (!factories.Any())
            return new List<KeyValuePair<string, string>>();

        var data = await _repositoryAccessor.HRMS_Basic_Code
            .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory && factories.Contains(x.Code), true)
            .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                x => x.Code,
                y => y.Code,
                (x, y) => new { code = x, codeLang = y })
            .SelectMany(
                x => x.codeLang.DefaultIfEmpty(),
                (x, y) => new { x.code, codeLang = y })
            .Select(x => new KeyValuePair<string, string>(x.code.Code, $"{x.code.Code} - {x.codeLang.Code_Name ?? x.code.Code_Name}"))
            .Distinct().ToListAsync();

        return data;
    }
    #endregion

    #region private handle
    // base query basic code list (2.2.Query_HRMS_Basic_Code_List )
    // input: language, Type_Seq
    //
    private async Task<List<KeyValuePair<string, string>>> GetBasicCodeList(string language, string typeSeq)
    {
        return await _repositoryAccessor.HRMS_Basic_Code
            .FindAll(x => x.Type_Seq == typeSeq, true)
            .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                HBC => new { HBC.Type_Seq, HBC.Code },
                HBCL => new { HBCL.Type_Seq, HBCL.Code },
                (HBC, HBCL) => new { HBC, HBCL })
                .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                (prev, HBCL) => new { prev.HBC, HBCL })
            .Select(x => new KeyValuePair<string, string>(x.HBC.Code, $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"))
            .ToListAsync();
    }
    #region GetListEmployeeAdd
    public async Task<List<EmployeeCommonInfo>> GetListEmployeeAdd(string factory, string language)
    {
        var result = await Query_EmpPersonal_Add(factory, language);
        return result;
    }
    #endregion

    #endregion
}
