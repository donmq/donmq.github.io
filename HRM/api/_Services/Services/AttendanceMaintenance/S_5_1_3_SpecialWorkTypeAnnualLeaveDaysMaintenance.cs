using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Models;
using Aspose.Cells;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance;

public class S_5_1_3_SpecialWorkTypeAnnualLeaveDaysMaintenance : BaseServices, I_5_1_3_SpecialWorkTypeAnnualLeaveDaysMaintenance
{
    public S_5_1_3_SpecialWorkTypeAnnualLeaveDaysMaintenance(DBContext dbContext) : base(dbContext)
    {
    }
    private static readonly string rootPath = Directory.GetCurrentDirectory();
    #region area add & edit
    public async Task<OperationResult> AddNew(HRMS_Att_Work_Type_DaysDto param)
    {
        bool existingData = await _repositoryAccessor.HRMS_Att_Work_Type_Days.AnyAsync(x => x.Division == param.Division && x.Factory == param.Factory
                                                                                       && x.Work_Type == param.Work_Type);

        if (existingData)
            return new OperationResult(false, "Data already exists");
        var newData = new HRMS_Att_Work_Type_Days
        {
            Division = param.Division,
            Factory = param.Factory,
            Work_Type = param.Work_Type,
            Annual_leave_days = decimal.Parse(param.Annual_leave_days),
            Effective_State = param.Effective_State,
            Update_By = param.Update_By,
            Update_Time = Convert.ToDateTime(param.Update_Time)
        };
        _repositoryAccessor.HRMS_Att_Work_Type_Days.Add(newData);

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

    public async Task<OperationResult> Edit(HRMS_Att_Work_Type_DaysDto param)
    {
        var existingData = await _repositoryAccessor.HRMS_Att_Work_Type_Days
                                .FirstOrDefaultAsync(x => x.Division == param.Division && x.Factory == param.Factory
                                                            && x.Work_Type == param.Work_Type, true);

        if (existingData is null)
            return new OperationResult(false, "No Data");

        existingData.Annual_leave_days = Convert.ToDecimal(param.Annual_leave_days);
        existingData.Effective_State = param.Effective_State;
        existingData.Update_By = param.Update_By;
        existingData.Update_Time = Convert.ToDateTime(param.Update_Time);
        _repositoryAccessor.HRMS_Att_Work_Type_Days.Update(existingData);

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

    #region area search
    public async Task<PaginationUtility<HRMS_Att_Work_Type_DaysDto>> GetDataPagination(PaginationParam pagination, SpecialWorkTypeAnnualLeaveDaysMaintenanceParam param)
    {
        var data = await Getdata(param);

        return PaginationUtility<HRMS_Att_Work_Type_DaysDto>.Create(data, pagination.PageNumber, pagination.PageSize);
    }
    public async Task<List<HRMS_Att_Work_Type_DaysDto>> Getdata(SpecialWorkTypeAnnualLeaveDaysMaintenanceParam param)
    {
        ExpressionStarter<HRMS_Att_Work_Type_Days> pred = PredicateBuilder.New<HRMS_Att_Work_Type_Days>(x => x.Division == param.Division && x.Factory == param.Factory);

        IQueryable<HRMS_Basic_Code> workTypeNameQuery = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.WorkType, true);
        IQueryable<HRMS_Basic_Code_Language> basiccode_Language = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower(), true);
        List<HRMS_Att_Work_Type_DaysDto> data = await _repositoryAccessor.HRMS_Att_Work_Type_Days.FindAll(pred, true)
                                      .GroupJoin(workTypeNameQuery,
                                              x => new { Code = x.Work_Type },
                                              y => new { y.Code },
                                          (x, y) => new { wordTypeDays = x, workTypeName = y })
                                          .SelectMany(x => x.workTypeName.DefaultIfEmpty(),
                                          (x, y) => new { x.wordTypeDays, workTypeName = y })
                                      // Join vs Department 
                                      .GroupJoin(basiccode_Language,
                                              x => new { x.workTypeName.Type_Seq, x.workTypeName.Code },
                                              y => new { y.Type_Seq, y.Code },
                                          (x, y) => new { x.wordTypeDays, x.workTypeName, basiccode_Language = y })
                                          .SelectMany(x => x.basiccode_Language.DefaultIfEmpty(),
                                          (x, y) => new { x.wordTypeDays, x.workTypeName, basiccode_Language = y }).Select(x => new HRMS_Att_Work_Type_DaysDto
                                          {
                                              Work_Type = $"{x.workTypeName.Code} - {(x.basiccode_Language != null ? x.basiccode_Language.Code_Name : x.workTypeName.Code_Name)}",
                                              Annual_leave_days = x.wordTypeDays.Annual_leave_days.ToString(),
                                              Factory = x.wordTypeDays.Factory,
                                              Division = x.wordTypeDays.Division,
                                              Update_By = x.wordTypeDays.Update_By,
                                              Update_Time = x.wordTypeDays.Update_Time.ToString("yyyy/MM/dd HH:mm:ss"),
                                              Effective_State = x.wordTypeDays.Effective_State,
                                              Status = param.Lang == "tw" ? x.wordTypeDays.Effective_State ? "啟用" : "停用" : x.wordTypeDays.Effective_State ? "Enabled" : "Disabled"
                                          }).ToListAsync();

        return data;
    }
    #endregion

    #region area GetListDivision & GetListFactory
    public async Task<List<KeyValuePair<string, string>>> GetListDivision(string language)
    {
        var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Division)
            .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower()),
                            x => new { x.Type_Seq, x.Code },
                            y => new { y.Type_Seq, y.Code },
                            (x, y) => new { x, y })
                            .SelectMany(x => x.y.DefaultIfEmpty(),
                            (x, y) => new { BasicCode = x.x, BasicCodeLanguage = y })
        .Select(x => new KeyValuePair<string, string>(x.BasicCode.Code, $"{x.BasicCode.Code} - {(x.BasicCodeLanguage != null ? x.BasicCodeLanguage.Code_Name : x.BasicCode.Code_Name)}")).ToListAsync();
        return data;
    }

    public async Task<List<KeyValuePair<string, string>>> GetListFactory(string division, string language, string userName)
    {
        var factorys = await Queryt_Factory_AddList(userName);
        var data = await _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory && factorys.Contains(x.Code), true)
                .Join(_repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(x => x.Kind == "1" && x.Division == division && factorys.Contains(x.Factory), true),
                    x => new { Factory = x.Code },
                    y => new { y.Factory },
                    (x, y) => new { HBC = x, HBFC = y })
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    x => new { x.HBC.Type_Seq, x.HBC.Code },
                    HBCL => new { HBCL.Type_Seq, HBCL.Code },
                    (x, y) => new { x.HBC, x.HBFC, HBCL = y })
                    .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (x, y) => new { x.HBC, x.HBFC, HBCL = y })
                .Select(x => new KeyValuePair<string, string>(x.HBC.Code, $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"))
                .ToListAsync();

        return data;
    }

    public async Task<List<KeyValuePair<string, string>>> GetListWorkType(string language)
    {
        return await _repositoryAccessor.HRMS_Basic_Code
               .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.WorkType, true)
               .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                   HBC => new { HBC.Type_Seq, HBC.Code },
                   HBCL => new { HBCL.Type_Seq, HBCL.Code },
                   (HBC, HBCL) => new { HBC, HBCL })
                   .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                   (prev, HBCL) => new { prev.HBC, HBCL })
               .Select(x => new KeyValuePair<string, string>(x.HBC.Code, $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"))
               .ToListAsync();
    }
    public async Task<OperationResult> DownloadExcel(SpecialWorkTypeAnnualLeaveDaysMaintenanceParam param, string userName)
    {
        var result = await Getdata(param);
        ExcelResult excelResult = DownloadExcel(
            result, userName, param.Factory, 
            "Resources\\Template\\AttendanceMaintenance\\5_1_3_SpecialWorkTypeAnnualLeaveDaysMaintenance\\Download.xlsx"
        );
        return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
    }
    public static ExcelResult DownloadExcel<T>(List<T> data, string userName, string factory, string subPath, ConfigDownload configDownload = null)
    {
        if (configDownload == null)
        {
            configDownload = new ConfigDownload();
        }

        if (!data.Any())
        {
            return new ExcelResult(isSuccess: false, "No data for excel download");
        }

        try
        {
            MemoryStream memoryStream = new MemoryStream();
            string file = Path.Combine(rootPath, subPath);
            WorkbookDesigner obj = new WorkbookDesigner
            {
                Workbook = new Workbook(file)
            };
            Worksheet worksheet = obj.Workbook.Worksheets[0];
            worksheet.Cells["E2"].PutValue(userName);
            worksheet.Cells["B2"].PutValue(factory);
            worksheet.Cells["H2"].PutValue(DateTime.Now.ToString("yyyy/MM/dd  HH:mm:ss"));
            obj.SetDataSource("result", (object)data);
            obj.Process();

            obj.Workbook.Save(memoryStream, configDownload.SaveFormat);
            return new ExcelResult(isSuccess: true, memoryStream.ToArray());
        }
        catch (Exception ex)
        {
            return new ExcelResult(isSuccess: false, ex.InnerException.Message);
        }
    }
    #endregion
}
