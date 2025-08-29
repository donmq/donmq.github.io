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
    public class S_7_1_5_PayslipDeliveryByEmailMaintenance : BaseServices, I_7_1_5_PayslipDeliveryByEmailMaintenance
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public S_7_1_5_PayslipDeliveryByEmailMaintenance(DBContext dbContext,IWebHostEnvironment webHostEnvironment) : base(dbContext)
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

        public async Task<OperationResult> CheckDuplicate(string factory, string employeeID)
        {
            var result = await _repositoryAccessor.HRMS_Sal_Payslip_Email.AnyAsync(x =>
                x.Factory == factory &&
                x.Employee_ID == employeeID
            );
            return new OperationResult(result);
        }
        #endregion

        #region GetData
        private async Task<List<D_7_5_PayslipDeliveryByEmailMaintenanceDto>> GetData(PayslipDeliveryByEmailMaintenanceParam param)
        {
            var pred = PredicateBuilder.New<HRMS_Sal_Payslip_Email>(x => x.Factory == param.Factory);
            if (!string.IsNullOrWhiteSpace(param.Employee_ID))
                pred.And(x => x.Employee_ID.Contains(param.Employee_ID.Trim()));
            var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll();
            var HSPE = _repositoryAccessor.HRMS_Sal_Payslip_Email.FindAll(pred, true).OrderBy(x => x.Employee_ID);
            var result = await HSPE
                .GroupJoin(HEP,
                    x => new { x.USER_GUID },
                    y => new { y.USER_GUID },
                    (x, y) => new { HSPE = x, HEP = y })
                .SelectMany(x => x.HEP.DefaultIfEmpty(),
                    (x, y) => new { x.HSPE, HEP = y })
                .Select(x => new D_7_5_PayslipDeliveryByEmailMaintenanceDto
                {
                    USER_GUID = x.HSPE.USER_GUID,
                    Factory = x.HSPE.Factory,
                    Employee_ID = x.HSPE.Employee_ID,
                    Local_Full_Name = x.HEP.Local_Full_Name,
                    Email = x.HSPE.Email,
                    Status = x.HSPE.Status,
                    Update_By = x.HSPE.Update_By,
                    Update_Time = x.HSPE.Update_Time.ToString("yyyy/MM/dd HH:mm:ss"),
                    isDeleteDisable = x.HEP.Resign_Date < DateTime.Now.AddMonths(-1)
                }).ToListAsync();
            return result;
        }

        public async Task<PaginationUtility<D_7_5_PayslipDeliveryByEmailMaintenanceDto>> GetDataPagination(PaginationParam pagination, PayslipDeliveryByEmailMaintenanceParam param)
        {
            var data = await GetData(param);
            return PaginationUtility<D_7_5_PayslipDeliveryByEmailMaintenanceDto>.Create(data, pagination.PageNumber, pagination.PageSize);
        }
        #endregion

        #region DownloadExcel
        public async Task<OperationResult> DownloadExcel(PayslipDeliveryByEmailMaintenanceParam param, string userName, bool isTemplate)
        {
            if (isTemplate)
            {
                var path = Path.Combine(
                    _webHostEnvironment.ContentRootPath, 
                    "Resources\\Template\\SalaryMaintenance\\7_1_5_PayslipDeliveryByEmailMaintenance\\Template.xlsx"
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
                    "Resources\\Template\\SalaryMaintenance\\7_1_5_PayslipDeliveryByEmailMaintenance\\Download.xlsx"
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
                "Resources\\Template\\SalaryMaintenance\\7_1_5_PayslipDeliveryByEmailMaintenance\\Template.xlsx"
            );
            if (!resp.IsSuccess)
                return new OperationResult(false, resp.Error);

            List<HRMS_Sal_Payslip_Email> dataAdd = new List<HRMS_Sal_Payslip_Email>();
            List<D_7_5_PayslipDeliveryByEmailMaintenanceReport> excelReportList = new List<D_7_5_PayslipDeliveryByEmailMaintenanceReport>();

            var allowFactories = await GetListFactory("en", userName);
            var factoryCodes = allowFactories.Select(x => x.Key).ToHashSet();

            var empPersonals = (await _repositoryAccessor.HRMS_Emp_Personal.FindAll(true)
                .ToListAsync())
                .GroupBy(x => x.Factory)
                .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.USER_GUID, y => y.Employee_ID));

            bool isPassed = true;
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            List<string> allowStatus = new() { "Y", "N" };

            for (int i = resp.WsTemp.Cells.Rows.Count; i < resp.Ws.Cells.Rows.Count; i++)
            {
                string errorMessage = "";
                string factory = resp.Ws.Cells[i, 0].StringValue?.Trim();
                string employeeID = resp.Ws.Cells[i, 1].StringValue?.Trim();
                string email = resp.Ws.Cells[i, 2].StringValue?.Trim();
                string status = resp.Ws.Cells[i, 3].StringValue?.Trim();
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

                if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, emailPattern))
                    errorMessage += $"Email is invalid.\n";

                if (string.IsNullOrWhiteSpace(status) || !allowStatus.Contains(status))
                    errorMessage += $"Status must be 'Y' or 'N'.\n";

                var existingData = await _repositoryAccessor.HRMS_Sal_Payslip_Email.AnyAsync(x => x.Factory == factory && x.Employee_ID == employeeID);
                if (existingData)
                    errorMessage += $"Data already exists.\n";

                if (dataAdd.Any(x => x.Factory == factory && x.Employee_ID == employeeID)
                || excelReportList.Any(x => x.Factory == factory && x.Employee_ID == employeeID))
                    errorMessage += $"Data are duplicated.\n";

                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    var newData = new HRMS_Sal_Payslip_Email
                    {
                        USER_GUID = userGUID,
                        Factory = factory,
                        Employee_ID = employeeID,
                        Email = email,
                        Status = status,
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

                D_7_5_PayslipDeliveryByEmailMaintenanceReport report = new()
                {
                    Factory = factory,
                    Employee_ID = employeeID,
                    Email = email,
                    Status = status,
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
                    "Resources\\Template\\SalaryMaintenance\\7_1_5_PayslipDeliveryByEmailMaintenance\\Report.xlsx"
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
            {
                try
                {
                    _repositoryAccessor.HRMS_Sal_Payslip_Email.AddMultiple(dataAdd);
                    await _repositoryAccessor.Save();
                    await _repositoryAccessor.CommitAsync();
                    string folder = "uploaded\\excels\\SalaryMaintenance\\7_1_5_PayslipDeliveryByEmailMaintenance\\Creates";
                    await FilesUtility.SaveFile(file, folder, $"PayslipDeliveryByEmailMaintenance_{DateTime.Now:yyyyMMddHHmmss}");
                    return new OperationResult(true);
                }
                catch (Exception ex)
                {
                    await _repositoryAccessor.RollbackAsync();
                    return new OperationResult(false, ex.Message);
                }
            }
        }
        #endregion

        #region Add
        public async Task<OperationResult> AddNew(D_7_5_PayslipDeliveryByEmailMaintenanceDto data, string userName)
        {
            if (data == null)
                return new OperationResult(false, "Data list is empty");

            var existingData = await _repositoryAccessor.HRMS_Sal_Payslip_Email
                                     .FirstOrDefaultAsync(x => x.Factory == data.Factory && x.Employee_ID == data.Employee_ID);

            if (existingData != null)
                return new OperationResult(false, $"Data already exists in HRMS_Sal_Payslip_Email \n Factory: {data.Factory} \n Employee ID: {data.Employee_ID} \n");

            var newData = new HRMS_Sal_Payslip_Email
            {
                USER_GUID = data.USER_GUID,
                Factory = data.Factory,
                Employee_ID = data.Employee_ID,
                Email = data.Email,
                Status = data.Status,
                Update_By = userName,
                Update_Time = DateTime.Now
            };

            try
            {
                _repositoryAccessor.HRMS_Sal_Payslip_Email.Add(newData);
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
        public async Task<OperationResult> Edit(D_7_5_PayslipDeliveryByEmailMaintenanceDto data, string userName)
        {
            if (data == null)
                return new OperationResult(false, "Data list is empty");

            var existingData = await _repositoryAccessor.HRMS_Sal_Payslip_Email
                                     .FirstOrDefaultAsync(x => x.Factory == data.Factory && x.Employee_ID == data.Employee_ID);

            if (existingData == null)
                return new OperationResult(false, $"No data in HRMS_Sal_Payslip_Email");

            existingData.USER_GUID = data.USER_GUID;
            existingData.Factory = data.Factory;
            existingData.Employee_ID = data.Employee_ID;
            existingData.Email = data.Email;
            existingData.Status = data.Status;
            existingData.Update_By = userName;
            existingData.Update_Time = DateTime.Now;

            try
            {
                _repositoryAccessor.HRMS_Sal_Payslip_Email.Update(existingData);
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
        public async Task<OperationResult> Delete(D_7_5_PayslipDeliveryByEmailMaintenanceDto data, string userName)
        {
            var existingData = await _repositoryAccessor.HRMS_Sal_Payslip_Email
                                     .FirstOrDefaultAsync(x => x.Factory == data.Factory && x.Employee_ID == data.Employee_ID);

            if (existingData == null)
                return new OperationResult(false, $"No data in HRMS_Sal_Payslip_Email");

            var empData = await _repositoryAccessor.HRMS_Emp_Personal.FirstOrDefaultAsync(x => x.USER_GUID == existingData.USER_GUID);

            if (empData != null && empData.Resign_Date.HasValue && empData.Resign_Date.Value < DateTime.Now.AddMonths(-1))
                return new OperationResult(false, "Cannot delete record. Employee resigned more than one month ago.");

            _repositoryAccessor.HRMS_Sal_Payslip_Email.Remove(existingData);

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