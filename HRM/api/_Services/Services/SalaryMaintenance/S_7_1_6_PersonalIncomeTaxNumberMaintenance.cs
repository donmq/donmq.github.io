using System.Text.RegularExpressions;
using API.Data;
using API._Services.Interfaces.SalaryMaintenance;
using API.DTOs.SalaryMaintenance;
using API.Helper.Constant;
using API.Helper.Params.SalaryMaintenance;
using API.Models;
using Aspose.Cells;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.SalaryMaintenance
{
  public class S_7_1_6_PersonalIncomeTaxNumberMaintenance : BaseServices, I_7_1_6_PersonalIncomeTaxNumberMaintenance
  {
    private readonly IWebHostEnvironment _webHostEnvironment;
    public S_7_1_6_PersonalIncomeTaxNumberMaintenance(DBContext dbContext, IWebHostEnvironment webHostEnvironment) : base(dbContext)
    {
      _webHostEnvironment = webHostEnvironment;
    }

    #region GetEmployeeData
    public async Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName)
    {
      var factorys = await Queryt_Factory_AddList(userName);
      var factories = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory && factorys.Contains(x.Code), true)
                  .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                              x => new { x.Type_Seq, x.Code },
                              y => new { y.Type_Seq, y.Code },
                              (x, y) => new { x, y })
                              .SelectMany(x => x.y.DefaultIfEmpty(),
                              (x, y) => new { x.x, y })
                  .Select(x => new KeyValuePair<string, string>(x.x.Code, $"{x.x.Code} - {(x.y != null ? x.y.Code_Name : x.x.Code_Name)}")).ToListAsync();
      return factories;
    }
    public async Task<OperationResult> CheckDuplicate(string factory, string employeeID, string year)
    {
      var result = await _repositoryAccessor.HRMS_Sal_Tax_Number.AnyAsync(x =>
        x.Factory == factory &&
        x.Year.Year == Convert.ToInt32(year) &&
        x.Employee_ID == employeeID
      );
      return new OperationResult(result);
    }
    #endregion

    #region GetData
    private async Task<List<D_7_6_PersonalIncomeTaxNumberMaintenanceDto>> GetData(PersonalIncomeTaxNumberMaintenanceParam param)
    {
      var pred = PredicateBuilder.New<HRMS_Sal_Tax_Number>(x => x.Factory == param.Factory);

      if (!string.IsNullOrWhiteSpace(param.Year))
        pred.And(x => x.Year.Year == Convert.ToInt32(param.Year));

      if (!string.IsNullOrWhiteSpace(param.Employee_ID))
        pred.And(x => x.Employee_ID.Contains(param.Employee_ID.Trim()));

      var HOD = _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Factory == param.Factory);
      var HODL = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower());
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

      var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll()
        .Select(x => new
        {
          x.USER_GUID,
          x.Local_Full_Name,
          Division = x.Employment_Status == "A" || x.Employment_Status == "S" ? x.Assigned_Division : x.Division,
          Factory = x.Employment_Status == "A" || x.Employment_Status == "S" ? x.Assigned_Factory : x.Factory,
          Department = x.Employment_Status == "A" || x.Employment_Status == "S" ? x.Assigned_Department : x.Department,
        });

      var HSTN = _repositoryAccessor.HRMS_Sal_Tax_Number
        .FindAll(pred, true)
        .OrderBy(x => x.Year).ThenBy(x => x.Employee_ID);

      var result = await HSTN
        .GroupJoin(HEP,
          x => x.USER_GUID,
          y => y.USER_GUID,
           (x, y) => new { HSTN = x, HEP = y })
        .SelectMany(x => x.HEP.DefaultIfEmpty(),
         (x, y) => new { x.HSTN, HEP = y })
        .GroupJoin(HOD_Lang,
          x => new { x.HEP.Factory, x.HEP.Division, Department_Code = x.HEP.Department },
          y => new { y.Factory, y.Division, y.Department_Code },
          (x, y) => new { x.HSTN, x.HEP, HOD_Lang = y })
        .SelectMany(x => x.HOD_Lang.DefaultIfEmpty(),
           (x, y) => new { x.HSTN, x.HEP, HOD_Lang = y })
        .Select(x => new D_7_6_PersonalIncomeTaxNumberMaintenanceDto
        {
          USER_GUID = x.HSTN.USER_GUID,
          Factory = x.HSTN.Factory,
          Year = x.HSTN.Year.ToString("yyyy"),
          Employee_ID = x.HSTN.Employee_ID,
          Local_Full_Name = x.HEP.Local_Full_Name,
          Department_Code = x.HEP.Department,
          Department_Name = x.HOD_Lang.Department_Name,
          Department_Code_Name = x.HOD_Lang != null && !string.IsNullOrWhiteSpace(x.HOD_Lang.Department_Name)
            ? x.HEP.Department + "-" + x.HOD_Lang.Department_Name : x.HEP.Department,
          TaxNo = x.HSTN.TaxNo,
          Dependents = x.HSTN.Dependents.ToString(),
          Update_By = x.HSTN.Update_By,
          Update_Time = x.HSTN.Update_Time.ToString("yyyy/MM/dd HH:mm:ss")
        }).ToListAsync();
      return result;
    }

    public async Task<PaginationUtility<D_7_6_PersonalIncomeTaxNumberMaintenanceDto>> GetDataPagination(PaginationParam pagination, PersonalIncomeTaxNumberMaintenanceParam param)
    {
      var data = await GetData(param);
      return PaginationUtility<D_7_6_PersonalIncomeTaxNumberMaintenanceDto>.Create(data, pagination.PageNumber, pagination.PageSize);
    }
    #endregion

    #region DownloadExcel
    public async Task<OperationResult> DownloadExcel(PersonalIncomeTaxNumberMaintenanceParam param, string userName, bool isTemplate)
    {
      if (isTemplate)
      {
        var path = Path.Combine(
          _webHostEnvironment.ContentRootPath,
          "Resources\\Template\\SalaryMaintenance\\7_1_6_PersonalIncomeTaxNumberMaintenance\\Template.xlsx"
        );
        var workbook = new Workbook(path);
        var design = new WorkbookDesigner(workbook);
        using MemoryStream stream = new();
        design.Workbook.Save(stream, SaveFormat.Xlsx);
        var result = stream.ToArray();
        return new OperationResult(true, null, result);
      }
      else
      {
        var data = await GetData(param);
        if (!data.Any())
          return new OperationResult(false, "No Data");

        List<Table> tables = new()
                {
                    new Table("result", data)
                };

        List<SDCores.Cell> cells = new()
                {
                    new SDCores.Cell("B1", userName),
                    new SDCores.Cell("D1", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")),
                };

        ExcelResult excelResult = ExcelUtility.DownloadExcel(
          tables,
          cells,
          "Resources\\Template\\SalaryMaintenance\\7_1_6_PersonalIncomeTaxNumberMaintenance\\Download.xlsx"
        );
        return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
      }
    }
    #endregion

    #region Upload
    public async Task<OperationResult> UploadData(IFormFile file, string userName)
    {
      ExcelResult resp = ExcelUtility.CheckExcel(
        file,
        "Resources\\Template\\SalaryMaintenance\\7_1_6_PersonalIncomeTaxNumberMaintenance\\Template.xlsx"
      );
      if (!resp.IsSuccess)
        return new OperationResult(false, resp.Error);

      List<HRMS_Sal_Tax_Number> dataAdd = new List<HRMS_Sal_Tax_Number>();
      List<D_7_6_PersonalIncomeTaxNumberMaintenanceReport> excelReportList = new List<D_7_6_PersonalIncomeTaxNumberMaintenanceReport>();

      var allowFactories = await GetListFactory("en", userName);
      var factoryCodes = allowFactories.Select(x => x.Key).ToHashSet();

      var empPersonals = (await _repositoryAccessor.HRMS_Emp_Personal.FindAll(true)
         .ToListAsync())
         .GroupBy(x => x.Factory)
         .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.USER_GUID, y => y.Employee_ID));

      bool isPassed = true;

      for (int i = resp.WsTemp.Cells.Rows.Count; i < resp.Ws.Cells.Rows.Count; i++)
      {
        string errorMessage = "";
        string factory = resp.Ws.Cells[i, 0].StringValue?.Trim();
        string year = resp.Ws.Cells[i, 1].StringValue?.Trim();
        int parsedYear = 0;
        string employeeID = resp.Ws.Cells[i, 2].StringValue?.Trim();
        string taxNo = resp.Ws.Cells[i, 3].StringValue?.Trim();
        string dependents = resp.Ws.Cells[i, 4].StringValue?.Trim();
        short dependentValue = 0;
        string userGUID = null;

        if (!factoryCodes.Contains(factory))
          errorMessage += $"Factory '{factory}' is not valid for user {userName}.\n";

        if (string.IsNullOrWhiteSpace(employeeID) || employeeID.Length > 16)
          errorMessage += $"Employee ID is invalid.\n";
        else
        {
          if (!employeeID.Contains('-'))
            employeeID = $"{factory}-{employeeID}";
        }

        if (empPersonals.TryGetValue(factory, out Dictionary<string, string> _dict1) && _dict1.ContainsValue(employeeID))
          userGUID = _dict1.FirstOrDefault(x => x.Value == employeeID).Key;
        else
          errorMessage += $"Employee ID not found with Factory {factory}.\n";

        if (string.IsNullOrWhiteSpace(year) || !Regex.IsMatch(year, @"^\d{4}$") || !int.TryParse(year, out parsedYear) || parsedYear < 1900 || parsedYear > DateTime.Now.Year)
          errorMessage += $"Year is invalid.\n";

        if (string.IsNullOrWhiteSpace(taxNo) || taxNo.Length > 50)
          errorMessage += $"Tax No. is invalid.\n";

        if (string.IsNullOrWhiteSpace(dependents) || !short.TryParse(dependents, out dependentValue) || dependentValue < 0 || dependents.Length > 1)
          errorMessage += $"Dependents is invalid.\n";

        var existingData = await _repositoryAccessor.HRMS_Sal_Tax_Number.AnyAsync(x => x.Factory == factory && x.Year.Year == parsedYear && x.Employee_ID == employeeID);
        if (existingData)
          errorMessage += $"Data already exists.\n";

        if (dataAdd.Any(x => x.Factory == factory && x.Year.Year == parsedYear && x.Employee_ID == employeeID)
        || excelReportList.Any(x => x.Factory == factory && x.Year == year && x.Employee_ID == employeeID))
          errorMessage += $"Data are duplicated.\n";

        if (string.IsNullOrWhiteSpace(errorMessage))
        {
          var newData = new HRMS_Sal_Tax_Number
          {
            USER_GUID = userGUID,
            Factory = factory,
            Year = new DateTime(parsedYear, 1, 1),
            Employee_ID = employeeID,
            TaxNo = taxNo,
            Dependents = dependentValue,
            Update_Time = DateTime.Now,
            Update_By = userName,
          };
          dataAdd.Add(newData);
        }
        else
        {
          isPassed = false;
          errorMessage = errorMessage.Trim();
        }

        D_7_6_PersonalIncomeTaxNumberMaintenanceReport report = new()
        {
          Factory = factory,
          Year = year,
          Employee_ID = employeeID,
          TaxNo = taxNo,
          Dependents = dependents,
          IsCorrect = string.IsNullOrEmpty(errorMessage) ? "Y" : "N",
          Error_Message = errorMessage
        };
        excelReportList.Add(report);
      }

      if (!isPassed)
      {
        MemoryStream memoryStream = new();
        string fileLocation = Path.Combine(
          Directory.GetCurrentDirectory(),
          "Resources\\Template\\SalaryMaintenance\\7_1_6_PersonalIncomeTaxNumberMaintenance\\Report.xlsx"
        );
        WorkbookDesigner workbookDesigner = new() { Workbook = new Workbook(fileLocation) };
        Worksheet worksheet = workbookDesigner.Workbook.Worksheets[0];
        workbookDesigner.SetDataSource("result", excelReportList);
        workbookDesigner.Process();
        worksheet.AutoFitColumns(worksheet.Cells.MinDataColumn, worksheet.Cells.MaxColumn);
        worksheet.AutoFitRows(worksheet.Cells.MinDataRow + 1, worksheet.Cells.MaxRow);
        workbookDesigner.Workbook.Save(memoryStream, SaveFormat.Xlsx);
        return new OperationResult { IsSuccess = false, Data = memoryStream.ToArray(), Error = "Please check Error Report" };
      }

      await _repositoryAccessor.BeginTransactionAsync();
      try
      {
        _repositoryAccessor.HRMS_Sal_Tax_Number.AddMultiple(dataAdd);
        await _repositoryAccessor.Save();
        await _repositoryAccessor.CommitAsync();
        string folder = "uploaded\\excels\\SalaryMaintenance\\7_1_6_PersonalIncomeTaxNumberMaintenance\\Creates";
        await FilesUtility.SaveFile(file, folder, $"PayslipDeliveryByEmailMaintenance_{DateTime.Now:yyyyMMddHHmmss}");
        return new OperationResult(true);
      }
      catch (Exception ex)
      {
        await _repositoryAccessor.RollbackAsync();
        return new OperationResult(false, ex.Message);
      }
    }
    #endregion

    #region Add
    public async Task<OperationResult> AddNew(D_7_6_PersonalIncomeTaxNumberMaintenanceDto data, string userName)
    {
      if (data == null)
        return new OperationResult(false, "Data list is empty");

      int parsedYear = Convert.ToInt32(data.Year);
      var existingData = await _repositoryAccessor.HRMS_Sal_Tax_Number
                               .FirstOrDefaultAsync(x => x.Factory == data.Factory && x.Year.Year == parsedYear && x.Employee_ID == data.Employee_ID);

      if (existingData != null)
        return new OperationResult(false, $"Data already exists in HRMS_Sal_Tax_Number \n Factory: {data.Factory} \n Employee ID: {data.Employee_ID} \n");

      var newData = new HRMS_Sal_Tax_Number
      {
        USER_GUID = data.USER_GUID,
        Factory = data.Factory,
        Year = new DateTime(parsedYear, 1, 1),
        Employee_ID = data.Employee_ID,
        TaxNo = data.TaxNo,
        Dependents = short.Parse(data.Dependents),
        Update_By = userName,
        Update_Time = DateTime.Now
      };

      try
      {
        _repositoryAccessor.HRMS_Sal_Tax_Number.Add(newData);
        await _repositoryAccessor.Save();
        return new OperationResult(true, "Add Successfully");
      }
      catch (Exception ex)
      {
        return new OperationResult(false, $"Inner exception: {ex.InnerException?.Message ?? "No inner exception message available"}");
      }
    }
    #endregion

    #region Edit
    public async Task<OperationResult> Edit(D_7_6_PersonalIncomeTaxNumberMaintenanceDto data, string userName)
    {
      if (data == null)
        return new OperationResult(false, "Data list is empty");

      int parsedYear = Convert.ToInt32(data.Year);
      var existingData = await _repositoryAccessor.HRMS_Sal_Tax_Number
                               .FirstOrDefaultAsync(x => x.Factory == data.Factory && x.Year.Year == parsedYear && x.Employee_ID == data.Employee_ID);

      if (existingData == null)
        return new OperationResult(false, $"No data in HRMS_Sal_Tax_Number");

      existingData.USER_GUID = data.USER_GUID;
      existingData.Factory = data.Factory;
      existingData.Year = new DateTime(parsedYear, 1, 1);
      existingData.Employee_ID = data.Employee_ID;
      existingData.TaxNo = data.TaxNo;
      existingData.Dependents = short.Parse(data.Dependents);
      existingData.Update_By = userName;
      existingData.Update_Time = DateTime.Now;

      try
      {
        _repositoryAccessor.HRMS_Sal_Tax_Number.Update(existingData);
        await _repositoryAccessor.Save();
        return new OperationResult(true, "Add Successfully");
      }
      catch (Exception ex)
      {
        return new OperationResult(false, $"Inner exception: {ex.InnerException?.Message ?? "No inner exception message available"}");
      }
    }
    #endregion

    #region Delete
    public async Task<OperationResult> Delete(D_7_6_PersonalIncomeTaxNumberMaintenanceDto data, string userName)
    {
      int parsedYear = Convert.ToInt32(data.Year);
      var existingData = await _repositoryAccessor.HRMS_Sal_Tax_Number
                               .FirstOrDefaultAsync(x => x.Factory == data.Factory && x.Year.Year == parsedYear && x.Employee_ID == data.Employee_ID);

      if (existingData == null)
        return new OperationResult(false, $"No data in HRMS_Sal_Tax_Number");
      _repositoryAccessor.HRMS_Sal_Tax_Number.Remove(existingData);

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
  }
}