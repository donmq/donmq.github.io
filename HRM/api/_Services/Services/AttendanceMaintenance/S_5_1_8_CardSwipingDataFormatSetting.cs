using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance;

public class S_5_1_8_CardSwipingDataFormatSetting : BaseServices, I_5_1_8_CardSwipingDataFormatSetting
{

    public S_5_1_8_CardSwipingDataFormatSetting(DBContext dbContext) : base(dbContext)
    {
    }
    #region area add & edit
    public async Task<OperationResult> AddNew(HRMS_Att_Swipecard_SetDto param)
    {
        var existingData = await _repositoryAccessor.HRMS_Att_Swipecard_Set.AnyAsync(x => x.Factory == param.Factory);

        if (existingData)
            return new OperationResult(false, "Data already exists");
        var newData = new HRMS_Att_Swipecard_Set
        {
            Factory = param.Factory,
            Employee_Start = param.Employee_Start,
            Employee_End = param.Employee_End,
            Time_Start = param.Time_Start,
            Time_End = param.Time_End,
            Date_Start = param.Date_Start,
            Date_End = param.Date_End,
            Update_By = param.Update_By,
            Update_Time = DateTime.Now
        };
        _repositoryAccessor.HRMS_Att_Swipecard_Set.Add(newData);

        try
        {
            await _repositoryAccessor.Save();
            return new OperationResult(true, "Add Successfully");
        }
        catch (Exception ex)
        {
            return new OperationResult(false, $"Inner exception: {ex.InnerException?.Message ?? "No inner exception message available"}");
        }
    }

    public async Task<OperationResult> Edit(HRMS_Att_Swipecard_SetDto param)
    {
        var existingData = await _repositoryAccessor.HRMS_Att_Swipecard_Set
                              .FirstOrDefaultAsync(x => x.Factory == param.Factory, true);

        if (existingData is null)
            return new OperationResult(false, "No Data");

        existingData.Employee_Start = Convert.ToInt32(param.Employee_Start);
        existingData.Employee_End = Convert.ToInt32(param.Employee_End);
        existingData.Time_Start = Convert.ToInt32(param.Time_Start);
        existingData.Time_End = Convert.ToInt32(param.Time_End);
        existingData.Date_Start = Convert.ToInt32(param.Date_Start);
        existingData.Date_End = Convert.ToInt32(param.Date_End);
        existingData.Update_By = param.Update_By;
        existingData.Update_Time = DateTime.Now;
        _repositoryAccessor.HRMS_Att_Swipecard_Set.Update(existingData);

        try
        {
            await _repositoryAccessor.Save();
            return new OperationResult(true, "Update Successfully");
        }
        catch (Exception ex)
        {
            return new OperationResult(false, $"Inner exception: {ex.InnerException?.Message ?? "No inner exception message available"}");
        }
    }
    #endregion

    #region area getdata single and paging
    public async Task<HRMS_Att_Swipecard_SetDto> GetDataByFactory(string factory)
    {
        var existingData = await _repositoryAccessor.HRMS_Att_Swipecard_Set
                              .FirstOrDefaultAsync(x => x.Factory == factory, true);
        var data = new HRMS_Att_Swipecard_SetDto
        {
            Factory = existingData.Factory,
            Employee_Start = existingData.Employee_Start,
            Employee_End = existingData.Employee_End,
            Time_Start = existingData.Time_Start,
            Time_End = existingData.Time_End,
            Date_Start = existingData.Date_Start,
            Date_End = existingData.Date_End,
        };
        return data;
    }

    public async Task<PaginationUtility<CardSwipingDataFormatSettingMain>> GetDataPagination(PaginationParam pagination, string factory)
    {
        var data = await _repositoryAccessor.HRMS_Att_Swipecard_Set
        .FindAll(x => x.Factory == factory, true)
        .Select(x => new CardSwipingDataFormatSettingMain
        {
            Factory = x.Factory,
            Employee_Id_Card_No = $"{x.Employee_Start}-{x.Employee_End}",
            Time = $"{x.Time_Start}-{x.Time_End}",
            Date = $"{x.Date_Start}-{x.Date_End}",
        })
        .ToListAsync();
        return PaginationUtility<CardSwipingDataFormatSettingMain>.Create(data, pagination.PageNumber, pagination.PageSize);
    }
    #endregion

    #region area get factory by account role
    private async Task<List<KeyValuePair<string, string>>> GetListFactory(string language)
    {
        var factories = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory, true)
                        .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                                    x => new { x.Type_Seq, x.Code },
                                    y => new { y.Type_Seq, y.Code },
                                    (x, y) => new { x, y })
                                    .SelectMany(x => x.y.DefaultIfEmpty(),
                                    (x, y) => new { x.x, y })
                        .Select(x => new KeyValuePair<string, string>(x.x.Code, $"{x.x.Code} - {(x.y != null ? x.y.Code_Name : x.x.Code_Name)}")).ToListAsync();
        return factories;
    }

    private async Task<List<string>> GetFactoryByRole(string userName)
    {
        var hrmsBasicRoles = _repositoryAccessor.HRMS_Basic_Role.FindAll(true);
        var hrmsBasicAccountRoles = _repositoryAccessor.HRMS_Basic_Account_Role.FindAll(true);
        var query = (from a in hrmsBasicRoles
                     join b in hrmsBasicAccountRoles
                     on a.Role equals b.Role
                     where b.Account == userName
                     select a.Factory)
             .Distinct();
        return await query.ToListAsync();
    }

    public async Task<List<KeyValuePair<string, string>>> GetFactoryByAccountAndLanguage(string userName, string language)
    {
        var factoryCodesByRole = await GetFactoryByRole(userName);
        var allFactoriesWithMultilang = await GetListFactory(language);

        var filteredFactoriesWithMultilang = allFactoriesWithMultilang
            .Where(kvp => factoryCodesByRole.Contains(kvp.Key))
            .ToList();

        return filteredFactoriesWithMultilang;
    }

    public Task<List<KeyValuePair<string, string>>> GetFactoryMain(string language)
    {
        return GetListFactory(language);
    }
    #endregion
}
