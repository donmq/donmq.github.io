using System.Text.RegularExpressions;
using API.Data;
using API._Services.Interfaces;
using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using API.Helper.Constant;
using API.Models;
using Aspose.Cells;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.SalaryMaintenance;
public class S_7_1_8_SAPCostCenterSetting : BaseServices, I_7_1_8_SAPCostCenterSetting
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    public S_7_1_8_SAPCostCenterSetting(DBContext dbContext,IWebHostEnvironment webHostEnvironment) : base(dbContext)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    #region AddAsync
    public async Task<OperationResult> AddAsync(D_7_8_SAPCostCenterSettingDto param)
    {
        DateTime year = DateTime.Now;
        if (int.TryParse(param.Year, out int inputYear))
        {
            year = new DateTime(inputYear, 1, 1);
        }
        else
        {
            return new OperationResult(false, $"Invalid year format. Please enter a valid year.");
        }

        bool existingData = await _repositoryAccessor.HRMS_Sal_SAPCostCenter.AnyAsync(x => x.Company_Code == param.Company_Code
                                                        && x.Cost_Year == year && x.Cost_Code == param.Cost_Code);
        if (existingData)
            return new OperationResult(false, "Data already exists");
        HRMS_Sal_SAPCostCenter newData = new()
        {
            Factory = param.Factory,
            Kind = param.Kind,
            Profit_Center = param.Profit_Center,
            Group_Id = param.Group,
            Company_Code = param.Company_Code,
            Cost_Code = param.Cost_Code,
            Cost_Year = year,
            Code_Name = param.Code_Name,
            Code_Name_EN = param.Code_Name_EN,
            Update_By = param.Update_By,
            Update_Time = Convert.ToDateTime(param.Update_Time)
        };
        _repositoryAccessor.HRMS_Sal_SAPCostCenter.Add(newData);

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

    #region CheckExistedData (or check field Cost Center in spec)
    public async Task<OperationResult> CheckExistedDataOrCostCenter(D_7_8_CheckDuplicateParam param)
    {
        DateTime year = DateTime.Now;
        if (int.TryParse(param.Cost_Year, out int inputYear))
        {
            year = new DateTime(inputYear, 1, 1);
        }
        else
        {
            return new OperationResult(false, $"Invalid year format. Please enter a valid year.");
        }
        return new OperationResult(await _repositoryAccessor.HRMS_Sal_SAPCostCenter.AnyAsync(x => x.Company_Code == param.Company_Code
                                                        && x.Cost_Year == year && x.Cost_Code == param.Cost_Code));
    }
    #endregion

    #region CheckExistedData Company_Code
    public async Task<OperationResult> CheckExistedDataCompanyCode(string factory, string companyCode)
    {
        return new OperationResult(await _repositoryAccessor.HRMS_Sal_SAPCostCenter.AnyAsync(x => x.Factory == factory && x.Company_Code == companyCode));
    }
    #endregion

    #region DeleteAsync
    public async Task<OperationResult> DeleteAsync(D_7_8_DeleteParam param)
    {
        DateTime year = DateTime.Now;
        if (int.TryParse(param.Cost_Year, out int inputYear))
        {
            year = new DateTime(inputYear, 1, 1);
        }
        else
        {
            return new OperationResult(false, $"Invalid year format. Please enter a valid year.");
        }
        HRMS_Sal_SAPCostCenter data = await _repositoryAccessor.HRMS_Sal_SAPCostCenter
               .FirstOrDefaultAsync(x => x.Company_Code == param.Company_Code
                                                        && x.Cost_Year == year && x.Cost_Code == param.Cost_Code);

        if (data is null)
            return new OperationResult(false, "System.Message.NotExitedData");

        try
        {
            _repositoryAccessor.HRMS_Sal_SAPCostCenter.Remove(data);
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
    public async Task<OperationResult> UpdateAsync(D_7_8_SAPCostCenterSettingDto param)
    {
        DateTime year = DateTime.Now;
        if (int.TryParse(param.Year, out int inputYear))
        {
            year = new DateTime(inputYear, 1, 1);
        }
        else
        {
            return new OperationResult(false, $"Invalid year format. Please enter a valid year.");
        }
        HRMS_Sal_SAPCostCenter existingData = await _repositoryAccessor.HRMS_Sal_SAPCostCenter
                               .FirstOrDefaultAsync(x => x.Company_Code == param.Company_Code
                                                        && x.Cost_Year == year && x.Cost_Code == param.Cost_Code);
        if (existingData is null)
            return new OperationResult(false, "System.Message.NotExitedData");

        existingData.Kind = param.Kind;
        existingData.Profit_Center = param.Profit_Center;
        existingData.Code_Name = param.Code_Name;
        existingData.Code_Name_EN = param.Code_Name_EN;
        existingData.Update_By = param.Update_By;
        existingData.Update_Time = Convert.ToDateTime(param.Update_Time);
        _repositoryAccessor.HRMS_Sal_SAPCostCenter.Update(existingData);

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

    #region  GetListKind
    public async Task<List<KeyValuePair<string, string>>> GetListKind(string language)
    {
        return await GetDataBasicCode(BasicCodeTypeConstant.Kind_Function, language);
    }
    #endregion

    #region  GetListFactory dropdown Search
    public async Task<List<KeyValuePair<string, string>>> GetListFactory(string userName, string language)
    {
        List<string> factorys = await Queryt_Factory_AddList(userName);
        List<KeyValuePair<string, string>> factories = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory && factorys.Contains(x.Code), true)
                    .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                                x => new { x.Type_Seq, x.Code },
                                y => new { y.Type_Seq, y.Code },
                                (x, y) => new { x, y })
                                .SelectMany(x => x.y.DefaultIfEmpty(),
                                (x, y) => new { x.x, y })
                    .Select(x => new KeyValuePair<string, string>(x.x.Code, $"{x.x.Code} - {(x.y != null ? x.y.Code_Name : x.x.Code_Name)}")).ToListAsync();
        return factories;
    }
    #endregion

    #region  Search
    public async Task<PaginationUtility<D_7_8_SAPCostCenterSettingDto>> Search(PaginationParam pagination, D_7_8_SAPCostCenterSettingParam param)
    {
        List<D_7_8_SAPCostCenterSettingDto> data = await GetData(param);
        return PaginationUtility<D_7_8_SAPCostCenterSettingDto>.Create(data, pagination.PageNumber, pagination.PageSize);
    }
    #endregion

    #region  private GetData
    private async Task<List<D_7_8_SAPCostCenterSettingDto>> GetData(D_7_8_SAPCostCenterSettingParam param)
    {
        ExpressionStarter<HRMS_Sal_SAPCostCenter> pred = PredicateBuilder.New<HRMS_Sal_SAPCostCenter>(x => x.Factory == param.Factory);
        ExpressionStarter<HRMS_Emp_Personal> predPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true);

        if (!string.IsNullOrWhiteSpace(param.Factory))
            pred = pred.And(x => x.Factory == param.Factory);
        if (!string.IsNullOrWhiteSpace(param.Company_Code))
            pred = pred.And(x => x.Company_Code.Contains(param.Company_Code.Trim()));
        if (!string.IsNullOrWhiteSpace(param.CostYear))
        {
            DateTime year = DateTime.Now;
            if (int.TryParse(param.CostYear, out int inputYear))
            {
                year = new DateTime(inputYear, 1, 1);
                pred = pred.And(x => x.Cost_Year == year);
            }
            else
            {
                return new List<D_7_8_SAPCostCenterSettingDto>();
            }
        }

        List<KeyValuePair<string, string>> listKind = await GetListKind(param.Language);
        List<HRMS_Sal_SAPCostCenter> result = await _repositoryAccessor.HRMS_Sal_SAPCostCenter.FindAll(pred, true).ToListAsync();
        List<D_7_8_SAPCostCenterSettingDto> response = result.Select(x =>
           {
               KeyValuePair<string, string> kindItem = listKind.FirstOrDefault(y => y.Key == x.Kind);
               D_7_8_SAPCostCenterSettingDto result = new()
               {
                   Factory = x.Factory,
                   Company_Code = x.Company_Code,
                   Year = x.Cost_Year.ToString("yyyy"),
                   Kind = x.Kind,
                   Kind_Name = kindItem.Key != null ? kindItem.Value : string.Empty,
                   Group = x.Group_Id,
                   Cost_Code = x.Cost_Code,
                   Profit_Center = x.Profit_Center,
                   Code_Name = x.Code_Name,
                   Code_Name_EN = x.Code_Name_EN,
                   Update_By = x.Update_By,
                   Update_Time = x.Update_Time.ToString("yyyy/MM/dd HH:mm:ss")
               };
               return result;
           })
            .OrderByDescending(x => x.Year)
            .ToList();
        return response;
    }
    #endregion

    #region UploadExcel
    public async Task<OperationResult> UploadExcel(IFormFile file, List<string> role_List, string userName)
    {
        ExcelResult resp = ExcelUtility.CheckExcel(
            file, 
            "Resources\\Template\\SalaryMaintenance\\7_1_8_SAPCostCenterSetting\\Template.xlsx"
        );
        if (!resp.IsSuccess)
            return new OperationResult(false, resp.Error);

        List<HRMS_Sal_SAPCostCenter> sal_SAPCostCenterAdd = new();
        List<D_7_8_SAPCostCenterSettingReportDto> excelReportList = new();

        Dictionary<string, HRMS_Basic_Code> factoryCodes = (await _repositoryAccessor.HRMS_Basic_Code
            .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory)
            .ToListAsync())
            .ToDictionary(x => x.Code);

        Dictionary<string, HRMS_Basic_Code> kindCodes = (await _repositoryAccessor.HRMS_Basic_Code
            .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Kind_Function)
            .ToListAsync())
            .ToDictionary(x => x.Code);
        List<string> roleFactories = await _repositoryAccessor.HRMS_Basic_Role
                            .FindAll(x => role_List.Contains(x.Role))
                            .Select(x => x.Factory).Distinct()
                            .ToListAsync();
        if (!roleFactories.Any())
            return new OperationResult(false, "Recent account roles do not have any factory.");

        string yearFormat = @"^\d{4}$";
        HashSet<string> processedPrimaryKeys = new();
        Regex regexCost_Year = new(yearFormat);
        for (int i = resp.WsTemp.Cells.Rows.Count; i < resp.Ws.Cells.Rows.Count; i++)
        {
            List<string> errorMessage = new();
            bool isCorrectFormatYear = true;
            bool isCorrectFactory = true;
            string factory = resp.Ws.Cells[i, 0].StringValue?.Trim();
            string company_Code = resp.Ws.Cells[i, 1].StringValue?.Trim();
            string cost_year = resp.Ws.Cells[i, 2].StringValue?.Trim();
            string group = resp.Ws.Cells[i, 3].StringValue?.Trim();
            string cost_Code = resp.Ws.Cells[i, 4].StringValue?.Trim();
            string kind = resp.Ws.Cells[i, 5].StringValue?.Trim();
            string profit_Center = resp.Ws.Cells[i, 6].StringValue?.Trim();
            string code_Name = resp.Ws.Cells[i, 7].StringValue?.Trim();
            string code_Name_EN = resp.Ws.Cells[i, 8].StringValue?.Trim();
            string primaryKeys = $"{cost_Code}_{company_Code}_{cost_year}";
            DateTime cost_year_dt = new DateTime();
            // area validate data
            // 1. Factory
            if (string.IsNullOrWhiteSpace(factory))
            {
                isCorrectFactory = false;
                errorMessage.Add("Factory is invalid. ");
            }
            else
            {
                if (!factoryCodes.ContainsKey(factory))
                {
                    isCorrectFactory = false;
                    errorMessage.Add("Factory does not existed on Type_Seq = 2. ");
                }
                if (!roleFactories.Contains(factory))
                {
                    isCorrectFactory = false;
                    errorMessage.Add("Uploaded [Factory] data does not match the role group. ");
                }
            }

            // 2. company_Code
            if (string.IsNullOrWhiteSpace(company_Code) || company_Code.Length > 4)
                errorMessage.Add("SAPCompany Code is invalid. ");
            else
            {
                // check Each factory can only have one SAP Company Code.
                if (isCorrectFactory)
                {
                    OperationResult existedDataCompanyCode = await CheckExistedDataCompanyCode(factory, company_Code);
                    if (existedDataCompanyCode.IsSuccess)
                        errorMessage.Add("Each factory can only have one SAP Company Code. ");
                }
            }

            // 3. Year
            if (string.IsNullOrWhiteSpace(cost_year) || !regexCost_Year.IsMatch(cost_year))
            {
                isCorrectFormatYear = false;
                errorMessage.Add("Year is invalid. Expected format: YYYY. ");
            }

            // 4. Group
            if (string.IsNullOrWhiteSpace(group) || group.Length > 10)
                errorMessage.Add("Group is invalid. ");

            // 5. Cost Center
            if (string.IsNullOrWhiteSpace(cost_Code) || cost_Code.Length > 10)
                errorMessage.Add("Cost Center is invalid. ");

            // Kiểm tra trùng lặp cho cost_Code + company_Code + cost_year (on excel and database)
            if (!string.IsNullOrWhiteSpace(cost_Code) && !string.IsNullOrWhiteSpace(company_Code) && !string.IsNullOrWhiteSpace(cost_year))
            {
                if (isCorrectFormatYear)
                {
                    if (int.TryParse(cost_year, out int inputYear))
                    {
                        cost_year_dt = new DateTime(inputYear, 1, 1);
                    }
                    bool onCaseDuplicatePrimary = await _repositoryAccessor.HRMS_Sal_SAPCostCenter
                   .AnyAsync(x => x.Company_Code == company_Code
                                                           && x.Cost_Year == cost_year_dt && x.Cost_Code == cost_Code);
                    if (processedPrimaryKeys.Contains(primaryKeys) || onCaseDuplicatePrimary)
                        errorMessage.Add("SAP Company Code, Year and Cost Center cannot be repeated. ");
                    else
                        processedPrimaryKeys.Add(primaryKeys);
                }
            }

            // 6. Funtion (Kind)
            if (string.IsNullOrWhiteSpace(kind) || kind.Length > 10)
                errorMessage.Add("Funtion is invalid. ");
            else
            {
                // check exists on type_seq = 50
                if (!kindCodes.ContainsKey(kind))
                    errorMessage.Add("Kind does not existed on Type_Seq = 50. ");
            }

            // 7. Profit Center
            if (string.IsNullOrWhiteSpace(profit_Center) || profit_Center.Length > 10)
                errorMessage.Add("Profit Center is invalid. ");

            // 8. Code Name
            if (string.IsNullOrWhiteSpace(code_Name) || code_Name.Length > 50)
                errorMessage.Add("Code Name is invalid. ");

            // 9. Code Name EN
            if (string.IsNullOrWhiteSpace(code_Name_EN) || code_Name_EN.Length > 50)
                errorMessage.Add("Code Name EN is invalid. ");

            if (!errorMessage.Any())
            {
                HRMS_Sal_SAPCostCenter newData = new()
                {
                    Factory = factory,
                    Kind = kind,
                    Profit_Center = profit_Center,
                    Group_Id = group,
                    Company_Code = company_Code,
                    Cost_Code = cost_Code,
                    Cost_Year = cost_year_dt,
                    Code_Name = code_Name,
                    Code_Name_EN = code_Name_EN,
                    Update_By = userName,
                    Update_Time = DateTime.Now
                };
                sal_SAPCostCenterAdd.Add(newData);
            }

            D_7_8_SAPCostCenterSettingReportDto report = new()
            {
                Factory = factory,
                Kind = kind,
                Profit_Center = profit_Center,
                Group = group,
                Company_Code = company_Code,
                Cost_Code = cost_Code,
                Year = cost_year,
                Code_Name = code_Name,
                Code_Name_EN = code_Name_EN,
                Error_Message = !errorMessage.Any() ? "Y" : string.Join("\r\n", errorMessage)
            };
            excelReportList.Add(report);
        }

        if (excelReportList.Any())
        {
            MemoryStream memoryStream = new();
            string fileLocation = Path.Combine(
                Directory.GetCurrentDirectory(), 
                "Resources\\Template\\SalaryMaintenance\\7_1_8_SAPCostCenterSetting\\Report.xlsx"
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
            _repositoryAccessor.HRMS_Sal_SAPCostCenter.AddMultiple(sal_SAPCostCenterAdd);
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
            "Resources\\Template\\SalaryMaintenance\\7_1_8_SAPCostCenterSetting\\Template.xlsx"
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
    public async Task<OperationResult> ExcelExport(D_7_8_SAPCostCenterSettingParam param, List<string> roleList, string userName)
    {
        List<D_7_8_SAPCostCenterSettingDto> data = await GetData(param);
        if (!data.Any())
            return new OperationResult(false, "No Data");
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
            "Resources\\Template\\SalaryMaintenance\\7_1_8_SAPCostCenterSetting\\Download.xlsx", 
            config
        );
        return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
    }
    #endregion

}
