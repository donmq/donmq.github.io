using System.Globalization;
using API.Data;
using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using API.Helper.Constant;
using API.Models;
using Aspose.Cells;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.SalaryMaintenance;
public class S_7_1_7_ListofChildcareSubsidyRecipientsMaintenance : BaseServices, I_7_1_7_ListofChildcareSubsidyRecipientsMaintenance
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    public S_7_1_7_ListofChildcareSubsidyRecipientsMaintenance(DBContext dbContext,IWebHostEnvironment webHostEnvironment) : base(dbContext)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    #region AddAsync
    public async Task<OperationResult> AddAsync(D_7_7_HRMS_Sal_Childcare_SubsidyDto param)
    {
        bool existingData = await _repositoryAccessor.HRMS_Sal_Childcare_Subsidy.AnyAsync(x =>
            x.Factory == param.Factory &&
            x.Employee_ID == param.Employee_ID &&
            x.Birthday_Child == param.Birthday_Child.Date);
        if (existingData)
            return new OperationResult(false, "Data already exists");
        HRMS_Sal_Childcare_Subsidy newData = new()
        {
            USER_GUID = param.USER_GUID,
            Factory = param.Factory,
            Employee_ID = param.Employee_ID,
            Birthday_Child = param.Birthday_Child.Date,
            Num_Children = param.Num_Children,
            Month_Start = param.Month_Start,
            Month_End = param.Month_End,
            Update_By = param.Update_By,
            Update_Time = Convert.ToDateTime(param.Update_Time)
        };
        _repositoryAccessor.HRMS_Sal_Childcare_Subsidy.Add(newData);

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
    #endregion

    #region CheckExistedData
    public async Task<OperationResult> CheckExistedData(string Factory, string Employee_ID, string Birthday_Child)
    {
        DateTime birthday = Convert.ToDateTime(Birthday_Child);
        return new OperationResult(await _repositoryAccessor.HRMS_Sal_Childcare_Subsidy.AnyAsync(x => x.Factory == Factory
                                                        && x.Employee_ID == Employee_ID && x.Birthday_Child == birthday.Date));
    }
    #endregion

    #region DeleteAsync
    public async Task<OperationResult> DeleteAsync(D_7_7_DeleteParam param)
    {
        HRMS_Sal_Childcare_Subsidy data = await _repositoryAccessor.HRMS_Sal_Childcare_Subsidy.FirstOrDefaultAsync(x =>
            x.Factory == param.Factory &&
            x.Employee_ID == param.Employee_ID &&
            x.Birthday_Child == param.Birthday_Child.Date);
        if (data is null)
            return new OperationResult(false, "System.Message.NotExitedData");
        try
        {
            _repositoryAccessor.HRMS_Sal_Childcare_Subsidy.Remove(data);
            await _repositoryAccessor.Save();
            return new OperationResult(true, "System.Message.DeleteOKMsg");
        }
        catch (Exception ex)
        {
            return new OperationResult(false, ex.ToString());
        }
    }
    #endregion

    #region UpdateAsync
    public async Task<OperationResult> UpdateAsync(D_7_7_HRMS_Sal_Childcare_SubsidyDto param)
    {
        HRMS_Sal_Childcare_Subsidy existingData = await _repositoryAccessor.HRMS_Sal_Childcare_Subsidy.FirstOrDefaultAsync(x =>
            x.Factory == param.Factory &&
            x.Employee_ID == param.Employee_ID_Old &&
            x.Birthday_Child == param.Birthday_Child.Date);
        if (existingData is null)
            return new OperationResult(false, "System.Message.NotExitedData");
        if (param.Employee_ID != param.Employee_ID_Old)
        {
            // remove data old
            _repositoryAccessor.HRMS_Sal_Childcare_Subsidy.Remove(existingData);
            bool existingDataAdd = await _repositoryAccessor.HRMS_Sal_Childcare_Subsidy.AnyAsync(x =>
                x.Factory == param.Factory &&
                x.Employee_ID == param.Employee_ID &&
                x.Birthday_Child == param.Birthday_Child.Date);
            if (existingDataAdd)
                return new OperationResult(false, "Data already exists");
            HRMS_Sal_Childcare_Subsidy newData = new()
            {
                USER_GUID = param.USER_GUID,
                Factory = param.Factory,
                Employee_ID = param.Employee_ID,
                Birthday_Child = param.Birthday_Child.Date,
                Num_Children = param.Num_Children,
                Month_Start = param.Month_Start,
                Month_End = param.Month_End,
                Update_By = param.Update_By,
                Update_Time = Convert.ToDateTime(param.Update_Time)
            };
            _repositoryAccessor.HRMS_Sal_Childcare_Subsidy.Add(newData);
        }
        else
        {
            existingData.Num_Children = param.Num_Children;
            existingData.Update_By = param.Update_By;
            existingData.Update_Time = Convert.ToDateTime(param.Update_Time);
            _repositoryAccessor.HRMS_Sal_Childcare_Subsidy.Update(existingData);
        }

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

    #region  GetListFactory dropdown Search
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
    #endregion

    #region  Search
    public async Task<PaginationUtility<D_7_7_HRMS_Sal_Childcare_SubsidyDto>> Search(PaginationParam pagination, D_7_7_HRMS_Sal_Childcare_SubsidyParam param)
    {
        List<D_7_7_HRMS_Sal_Childcare_SubsidyDto> data = await GetData(param);
        return PaginationUtility<D_7_7_HRMS_Sal_Childcare_SubsidyDto>.Create(data, pagination.PageNumber, pagination.PageSize);
    }
    #endregion

    #region  private GetData
    private async Task<List<D_7_7_HRMS_Sal_Childcare_SubsidyDto>> GetData(D_7_7_HRMS_Sal_Childcare_SubsidyParam param)
    {
        var pred = PredicateBuilder.New<HRMS_Sal_Childcare_Subsidy>(x => x.Factory == param.Factory);
        if (!string.IsNullOrWhiteSpace(param.Employee_ID))
            pred = pred.And(x => x.Employee_ID.Contains(param.Employee_ID.Trim()));

        var HSCS = _repositoryAccessor.HRMS_Sal_Childcare_Subsidy.FindAll(pred, true).OrderBy(x => x.Employee_ID);
        var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll();

        var HOD = _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Factory == param.Factory);
        var HODL = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower());
        var HOD_Lang = HOD
            .GroupJoin(HODL,
                x => new { x.Department_Code, x.Factory },
                y => new { y.Department_Code, y.Factory },
                (x, y) => new { HOD = x, HODL = y })
            .SelectMany(x => x.HODL.DefaultIfEmpty(),
                (x, y) => new { x.HOD, HODL = y })
            .Select(x => new
            {
                x.HOD.Factory,
                x.HOD.Division,
                x.HOD.Department_Code,
                Department_Name = x.HODL != null ? x.HODL.Name : x.HOD.Department_Name
            });

        var result = await HSCS
            .GroupJoin(HEP,
                x => x.USER_GUID,
                y => y.USER_GUID,
                (x, y) => new { HSCS = x, HEP = y })
            .SelectMany(x => x.HEP.DefaultIfEmpty(),
                (x, y) => new { x.HSCS, HEP = y })
            .GroupJoin(HOD_Lang,
                x => new { x.HEP.Factory, x.HEP.Division, Department_Code = x.HEP.Department },
                y => new { y.Factory, y.Division, y.Department_Code },
                (x, y) => new { x.HSCS, x.HEP, HOD_Lang = y })
            .SelectMany(x => x.HOD_Lang.DefaultIfEmpty(),
                (x, y) => new { x.HSCS, x.HEP, HOD_Lang = y })
            .Select(x => new D_7_7_HRMS_Sal_Childcare_SubsidyDto
            {
                USER_GUID = x.HSCS.USER_GUID,
                Factory = x.HSCS.Factory,
                Employee_ID = x.HSCS.Employee_ID,
                Local_Full_Name = x.HEP.Local_Full_Name,
                Department_Code = x.HEP.Department,
                Department_Name = x.HOD_Lang.Department_Name,
                Department_Code_Name = x.HOD_Lang != null && !string.IsNullOrWhiteSpace(x.HOD_Lang.Department_Name)
                    ? x.HEP.Department + "-" + x.HOD_Lang.Department_Name : x.HEP.Department,
                Birthday_Child = x.HSCS.Birthday_Child,
                Month_Start = x.HSCS.Month_Start,
                Month_End = x.HSCS.Month_End,
                Num_Children = x.HSCS.Num_Children,
                Update_By = x.HSCS.Update_By,
                Update_Time = x.HSCS.Update_Time.ToString("yyyy/MM/dd HH:mm:ss")
            }).ToListAsync();
        return result;
    }
    #endregion

    #region UploadExcel
    public async Task<OperationResult> UploadExcel(IFormFile file, List<string> role_List, string userName)
    {
        ExcelResult resp = ExcelUtility.CheckExcel(
            file,
            "Resources\\Template\\SalaryMaintenance\\7_1_7_ListofChildcareSubsidyRecipientsMaintenance\\Template.xlsx"
        );
        if (!resp.IsSuccess)
            return new OperationResult(false, resp.Error);

        List<HRMS_Sal_Childcare_Subsidy> sal_Childcare_SubsidyAdd = new();
        List<D_7_7_HRMS_Sal_Childcare_SubsidyReportDto> excelReportList = new();

        Dictionary<string, HRMS_Basic_Code> factoryCodes = (await _repositoryAccessor.HRMS_Basic_Code
            .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory)
            .ToListAsync())
            .ToDictionary(x => x.Code);
        List<string> roleFactories = await _repositoryAccessor.HRMS_Basic_Role
            .FindAll(x => role_List.Contains(x.Role))
            .Select(x => x.Factory).Distinct()
            .ToListAsync();
        if (!roleFactories.Any())
            return new OperationResult(false, "Recent account roles do not have any factory.");
        var empPersonals = await _repositoryAccessor.HRMS_Emp_Personal
            .FindAll(x => roleFactories.Contains(x.Factory))
            .Select(x => new { x.Division, x.Factory, x.Employee_ID, x.USER_GUID })
            .ToListAsync();

        HashSet<string> processedPrimaryKeys = new();

        for (int i = resp.WsTemp.Cells.Rows.Count; i < resp.Ws.Cells.Rows.Count; i++)
        {
            List<string> errorMessage = new();
            string factory = resp.Ws.Cells[i, 0].StringValue?.Trim();
            string employee_ID = resp.Ws.Cells[i, 1].StringValue?.Trim();
            string birthday_Child = resp.Ws.Cells[i, 2].StringValue?.Trim();
            string month_Start = resp.Ws.Cells[i, 3].StringValue?.Trim();
            string month_End = resp.Ws.Cells[i, 4].StringValue?.Trim();
            string num_Children = resp.Ws.Cells[i, 5].StringValue?.Trim();
            string primaryKeys = $"{factory}_{employee_ID}_{birthday_Child}";

            // area validate data
            // 1. Factory
            if (string.IsNullOrWhiteSpace(factory))
                errorMessage.Add("Factory is invalid. ");
            if (!string.IsNullOrWhiteSpace(factory))
            {
                if (!factoryCodes.ContainsKey(factory))
                    errorMessage.Add("Factory does not existed on Type_Seq=2. ");
                if (!roleFactories.Contains(factory))
                    errorMessage.Add("Uploaded [Factory] data does not match the role group. ");
            }

            // 2. Employee ID
            if (string.IsNullOrWhiteSpace(employee_ID) || employee_ID.Length > 16)
                errorMessage.Add("Employee ID is invalid. ");
            else
            {
                if (!employee_ID.Contains('-'))
                    employee_ID = $"{factory}-{employee_ID}";
                else
                {
                    string[] parts = employee_ID.Split('-');
                    if (parts[0] != factory)
                        errorMessage.Add("Cannot find the corresponding factory with upload Employee ID. ");
                }

            }

            // 3. birthday_Child
            var validBirthdayChild = DateTime.TryParseExact(birthday_Child, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime parsedDateBirthday);
            if (!validBirthdayChild)
                errorMessage.Add("Birthday_Child is Invalid date format (yyyy/MM/dd). ");

            // Kiểm tra trùng lặp cho Factory + Employee_ID + Birthday_Child (on excel and database)
            if (!string.IsNullOrWhiteSpace(factory) && !string.IsNullOrWhiteSpace(employee_ID) && validBirthdayChild)
            {
                bool onCaseDuplicatePrimary = await _repositoryAccessor.HRMS_Sal_Childcare_Subsidy
                .AnyAsync(x => x.Factory == factory
                        && x.Employee_ID == employee_ID && x.Birthday_Child == parsedDateBirthday);
                if (processedPrimaryKeys.Contains(primaryKeys) || onCaseDuplicatePrimary)
                    errorMessage.Add("Factory, and Employee ID, Birthday_Child cannot be repeated. ");
                else
                    processedPrimaryKeys.Add(primaryKeys);
            }

            // 4. YearMonth_Start
            var validYearMonth_Start = DateTime.TryParseExact(month_Start, "yyyy/MM", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime parsedDateMonth_Start);
            if (!validYearMonth_Start)
                errorMessage.Add("YearMonth_Start is Invalid date format (yyyy/MM). ");

            // 5. YearMonth_End
            var validYearMonth_End = DateTime.TryParseExact(month_End, "yyyy/MM", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime parsedDateMonth_End);
            if (!validYearMonth_End)
                errorMessage.Add("YearMonth_End is Invalid date format (yyyy/MM). ");

            // Validate month_Start and month_End in "yyyy/MM" format
            if (validYearMonth_Start && validYearMonth_End)
            {
                if (parsedDateMonth_Start > parsedDateMonth_End)
                    errorMessage.Add("YearMonth_Start cannot be greater than YearMonth_End. ");
            }

            // 6. Num_Children
            if (string.IsNullOrWhiteSpace(num_Children) || num_Children.Length > 2)
                errorMessage.Add("Num_Children is null or invalid. ");

            string userGUID = null;
            string empPersonal_USER_GUID = empPersonals.FirstOrDefault(x => x.Factory == factory && x.Employee_ID == employee_ID)?.USER_GUID;

            if (empPersonal_USER_GUID != null)
                userGUID = empPersonal_USER_GUID;
            else
                errorMessage.Add("Employee ID is not existed. ");

            if (!errorMessage.Any())
            {
                HRMS_Sal_Childcare_Subsidy newData = new()
                {
                    USER_GUID = userGUID,
                    Factory = factory,
                    Employee_ID = employee_ID,
                    Birthday_Child = parsedDateBirthday,
                    Month_Start = parsedDateMonth_Start,
                    Month_End = parsedDateMonth_End,
                    Num_Children = Convert.ToInt16(num_Children),
                    Update_Time = DateTime.Now,
                    Update_By = userName,
                };
                sal_Childcare_SubsidyAdd.Add(newData);
            }

            D_7_7_HRMS_Sal_Childcare_SubsidyReportDto report = new()
            {
                Factory = factory,
                Employee_ID = employee_ID,
                Birthday_Child = birthday_Child,
                Month_Start = month_Start,
                Month_End = month_End,
                Num_Children_Report = string.IsNullOrWhiteSpace(num_Children) ? null : Convert.ToInt16(num_Children),
                Error_Message = !errorMessage.Any() ? "Y" : string.Join("\r\n", errorMessage)
            };
            excelReportList.Add(report);
        }

        if (excelReportList.Any())
        {
            MemoryStream memoryStream = new();
            string fileLocation = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Resources\\Template\\SalaryMaintenance\\7_1_7_ListofChildcareSubsidyRecipientsMaintenance\\Report.xlsx"
            );
            WorkbookDesigner workbookDesigner = new() { Workbook = new Workbook(fileLocation) };
            Worksheet worksheet = workbookDesigner.Workbook.Worksheets[0];
            workbookDesigner.SetDataSource("result", excelReportList);
            workbookDesigner.Process();
            worksheet.AutoFitColumns(worksheet.Cells.MinDataColumn, worksheet.Cells.MaxColumn);
            worksheet.AutoFitRows(worksheet.Cells.MinDataRow + 1, worksheet.Cells.MaxRow);
            workbookDesigner.Workbook.Save(memoryStream, SaveFormat.Xlsx);
            if (excelReportList.Exists(x => x.Error_Message != "Y"))
                return new OperationResult { IsSuccess = false, Data = memoryStream.ToArray(), Error = "Please check Error Report" };
        }

        await _repositoryAccessor.BeginTransactionAsync();
        try
        {
            _repositoryAccessor.HRMS_Sal_Childcare_Subsidy.AddMultiple(sal_Childcare_SubsidyAdd);
            await _repositoryAccessor.Save();
            await _repositoryAccessor.CommitAsync();
            return new OperationResult(true, "System.Message.UploadOKMsg");
        }
        catch (Exception ex)
        {
            await _repositoryAccessor.RollbackAsync();
            return new OperationResult(false, ex.Message);
        }
    }
    #endregion

    #region DownloadExcelTemplate
    public Task<OperationResult> DownloadExcelTemplate()
    {
        string path = Path.Combine(
            _webHostEnvironment.ContentRootPath,
            "Resources\\Template\\SalaryMaintenance\\7_1_7_ListofChildcareSubsidyRecipientsMaintenance\\Template.xlsx"
        );
        Workbook workbook = new(path);
        WorkbookDesigner design = new(workbook);
        MemoryStream stream = new();
        design.Workbook.Save(stream, SaveFormat.Xlsx);
        byte[] result = stream.ToArray();
        return Task.FromResult(new OperationResult(true, null, result));
    }
    #endregion

    #region ExcelExport
    public async Task<OperationResult> ExcelExport(D_7_7_HRMS_Sal_Childcare_SubsidyParam param, string userName)
    {
        List<D_7_7_HRMS_Sal_Childcare_SubsidyDto> data = await GetData(param);
        if (!data.Any())
            return new OperationResult(false, "No Data");
        data.ForEach(x => x.Seq = data.IndexOf(x) + 1);
        List<Table> tables = new() { new Table("result", data) };
        List<SDCores.Cell> cells = new()
        {
            new SDCores.Cell("B1", userName),
            new SDCores.Cell("D1", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")),
        };
        ConfigDownload config = new() { IsAutoFitColumn = true };
        ExcelResult excelResult = ExcelUtility.DownloadExcel(
            tables,
            cells,
            "Resources\\Template\\SalaryMaintenance\\7_1_7_ListofChildcareSubsidyRecipientsMaintenance\\Download.xlsx",
            config
        );
        return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
    }
    #endregion
}
