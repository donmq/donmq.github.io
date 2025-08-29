using System.Globalization;
using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Models;
using Aspose.Cells;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance
{
    public class S_5_1_7_MaintenanceOfAnnualLeaveEntitlement : BaseServices, I_5_1_7_MaintenanceOfAnnualLeaveEntitlement
    {

        public S_5_1_7_MaintenanceOfAnnualLeaveEntitlement(DBContext dbContext) : base(dbContext)
        {
        }
        private static readonly string rootPath = Directory.GetCurrentDirectory();

        #region Add
        public async Task<OperationResult> Add(MaintenanceOfAnnualLeaveEntitlementDto datas, string userName)
        {
            DateTime now = DateTime.Now;

            if (await _repositoryAccessor.HRMS_Att_Annual_Leave.AnyAsync(
                x => x.Annual_Start == Convert.ToDateTime(datas.Annual_Start) &&
                x.Factory == datas.Factory &&
                x.Employee_ID == datas.Employee_ID &&
                x.Leave_Code == datas.Leave_Code))
            {
                string message = $"Available Range(Start): {datas.Annual_Start}, Employee ID: {datas.Employee_ID}, Leave Code: {datas.Leave_Code} exsited!";
                return new OperationResult { IsSuccess = false, Error = message };
            }

            var newData = new HRMS_Att_Annual_Leave()
            {
                Annual_Start = Convert.ToDateTime(datas.Annual_Start),
                Annual_End = Convert.ToDateTime(datas.Annual_End),
                Employee_ID = datas.Employee_ID,
                Factory = datas.Factory,
                Leave_Code = datas.Leave_Code,
                USER_GUID = datas.USER_GUID,
                Previous_Hours = decimal.Parse(datas.Previous_Hours),
                Total_Days = datas.Total_Days,
                Total_Hours = datas.Total_Hours,
                Year_Hours = decimal.Parse(datas.Year_Hours),
                Update_By = userName,
                Update_Time = now
            };

            try
            {
                _repositoryAccessor.HRMS_Att_Annual_Leave.Add(newData);
                await _repositoryAccessor.Save();
                return new OperationResult { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new OperationResult { IsSuccess = false, Error = ex.InnerException.Message };
            }
        }
        #endregion

        #region Delete
        public async Task<OperationResult> Delete(MaintenanceOfAnnualLeaveEntitlementDto dto)
        {
            var data = await _repositoryAccessor.HRMS_Att_Annual_Leave
                .FirstOrDefaultAsync(
                    x => x.Annual_Start.Date == Convert.ToDateTime(dto.Annual_Start).Date &&
                    x.Factory == dto.Factory &&
                    x.Employee_ID == dto.Employee_ID &&
                    x.Leave_Code == dto.Leave_Code
                );

            if (data is null)
                return new OperationResult(false, "Data not existed");

            try
            {
                _repositoryAccessor.HRMS_Att_Annual_Leave.Remove(data);
                await _repositoryAccessor.Save();
                return new OperationResult(true, "Delete data successfully");
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.ToString());
            }
        }
        #endregion

        #region Edit
        public async Task<OperationResult> Edit(MaintenanceOfAnnualLeaveEntitlementDto dto)
        {
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                var dataOld = await _repositoryAccessor.HRMS_Att_Annual_Leave
                    .FirstOrDefaultAsync(
                        x => x.Annual_Start.Date == Convert.ToDateTime(dto.Annual_Start).Date &&
                        x.Factory == dto.Factory &&
                        x.Employee_ID == dto.Employee_ID &&
                        x.Leave_Code == dto.Leave_Code_Old
                    );
                if (dataOld is null)
                    return new OperationResult(false, "Data not existed");
                _repositoryAccessor.HRMS_Att_Annual_Leave.Remove(dataOld);
                await _repositoryAccessor.Save();
                HRMS_Att_Annual_Leave newData = new()
                {
                    Annual_Start = Convert.ToDateTime(dto.Annual_Start),
                    Annual_End = Convert.ToDateTime(dto.Annual_End),
                    Employee_ID = dto.Employee_ID,
                    Factory = dto.Factory,
                    USER_GUID = dto.USER_GUID,
                    Leave_Code = dto.Leave_Code,
                    Previous_Hours = decimal.Parse(dto.Previous_Hours),
                    Year_Hours = decimal.Parse(dto.Year_Hours),
                    Total_Hours = dto.Total_Hours,
                    Total_Days = dto.Total_Days,
                    Update_By = dto.Update_By,
                    Update_Time = DateTime.Now,
                };
                if (await _repositoryAccessor.HRMS_Att_Annual_Leave.AnyAsync(x =>
                    x.Annual_Start.Date == newData.Annual_Start.Date &&
                    x.Factory == newData.Factory &&
                    x.Employee_ID == newData.Employee_ID &&
                    x.Leave_Code == newData.Leave_Code
                ))
                    return new OperationResult(false, "Already existed data");
                _repositoryAccessor.HRMS_Att_Annual_Leave.Add(newData);
                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false, ex.ToString());
            }
        }
        #endregion

        #region GetListFactory
        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string userName, string language)
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

            var predicate = PredicateBuilder.New<HRMS_Basic_Code>(x => x.Type_Seq == BasicCodeTypeConstant.Factory && factories.Contains(x.Code));
            var data = await GetBasicCodes(language, predicate);

            return data;
        }
        #endregion

        #region GetListDepartment
        public async Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language)
        {
            ExpressionStarter<HRMS_Org_Department> predDept = PredicateBuilder.New<HRMS_Org_Department>(x => x.Factory == factory);
            ExpressionStarter<HRMS_Basic_Factory_Comparison> predCom = PredicateBuilder.New<HRMS_Basic_Factory_Comparison>(x => x.Factory == factory && x.Kind == "1");
            var data = await QueryDepartment(predDept, predCom, language)
                .Select(
                    x => new KeyValuePair<string, string>(
                        x.Department.Department_Code,
                        $"{x.Department.Department_Code} - {(x.Language != null ? x.Language.Name : x.Department.Department_Name)}"
                    )
                ).ToListAsync();

            return data;
        }
        #endregion

        #region GetListLeaveCode
        public async Task<List<KeyValuePair<string, string>>> GetListLeaveCode(string language)
        {
            var predicate = PredicateBuilder.New<HRMS_Basic_Code>(x => x.Type_Seq == BasicCodeTypeConstant.Leave && x.Remark_Code == "Y");
            var data = await GetBasicCodes(language, predicate);

            return data;
        }
        #endregion

        #region Query
        public async Task<PaginationUtility<MaintenanceOfAnnualLeaveEntitlementDto>> Query(PaginationParam pagination, MaintenanceOfAnnualLeaveEntitlementParam param)
        {
            var result = await GetData(param);
            return PaginationUtility<MaintenanceOfAnnualLeaveEntitlementDto>.Create(result, pagination.PageNumber, pagination.PageSize);
        }
        public async Task<List<MaintenanceOfAnnualLeaveEntitlementDto>> GetData(MaintenanceOfAnnualLeaveEntitlementParam param)
        {
            var permissionGroupQuery = _repositoryAccessor.HRMS_Basic_Role.FindAll(x => x.Factory == param.Factory, true).Select(x => x.Permission_Group);
            var predAnnualLeave = PredicateBuilder.New<HRMS_Att_Annual_Leave>(x => x.Factory == param.Factory);
            var predEmp = PredicateBuilder.New<HRMS_Emp_Personal>(x => (x.Factory == param.Factory) && permissionGroupQuery.Contains(x.Permission_Group));
            if (!string.IsNullOrWhiteSpace(param.Employee_ID))
                predAnnualLeave.And(x => x.Employee_ID.Contains(param.Employee_ID.Trim()));
            if (!string.IsNullOrWhiteSpace(param.AvailableRange_Start) && !string.IsNullOrWhiteSpace(param.AvailableRange_End))
                predAnnualLeave.And(x => x.Annual_Start >= Convert.ToDateTime(param.AvailableRange_Start) && x.Annual_End <= Convert.ToDateTime(param.AvailableRange_End));
            if (!string.IsNullOrWhiteSpace(param.Leave_Code))
                predAnnualLeave.And(x => x.Leave_Code == param.Leave_Code);

            var HAAL = _repositoryAccessor.HRMS_Att_Annual_Leave.FindAll(predAnnualLeave, true);
            var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.IsActive);
            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower());
            var HBC_LeaveCode = HBC.Where(x => x.Type_Seq == BasicCodeTypeConstant.Leave)
                .GroupJoin(HBCL.Where(x => x.Type_Seq == BasicCodeTypeConstant.Leave),
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (x, y) => new { HBC = x, HBCL = y })
                .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (x, y) => new { x.HBC, HBCL = y })
                .Select(x => new
                {
                    x.HBC.Code,
                    Code_Name = $"{x.HBC.Code}-{(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"
                });


            // Get allow employee by recent account roles (Factory, Permission_Group only)
            var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll(predEmp, true).Select(x => new
            {
                x.USER_GUID,
                x.Local_Full_Name,
                Division = x.Employment_Status == "A" || x.Employment_Status == "S" ? x.Assigned_Division : x.Division,
                Factory = x.Employment_Status == "A" || x.Employment_Status == "S" ? x.Assigned_Factory : x.Factory,
                Department_Code = x.Employment_Status == "A" || x.Employment_Status == "S" ? x.Assigned_Department : x.Department,
            });

            var HOD = _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Factory == param.Factory);
            var HODL = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower());
            var HOD_Lang = HOD
                .GroupJoin(HODL,
                    x => new { x.Department_Code, x.Factory, x.Division },
                    y => new { y.Department_Code, y.Factory, y.Division },
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

            var result = await HAAL
                .Join(HEP,
                    x => x.USER_GUID,
                    y => y.USER_GUID,
                    (x, y) => new { HAAL = x, HEP = y })
                .GroupJoin(HBC_LeaveCode,
                    x => x.HAAL.Leave_Code,
                    y => y.Code,
                    (x, y) => new { x.HAAL, x.HEP, HBC_LeaveCode = y })
                .SelectMany(x => x.HBC_LeaveCode.DefaultIfEmpty(),
                    (x, y) => new { x.HAAL, x.HEP, HBC_LeaveCode = y })
                .GroupJoin(HOD_Lang,
                    x => new { x.HEP.Department_Code, x.HEP.Factory, x.HEP.Division },
                    y => new { y.Department_Code, y.Factory, y.Division },
                    (x, y) => new { x.HAAL, x.HEP, x.HBC_LeaveCode, HOD_Lang = y })
                .SelectMany(x => x.HOD_Lang.DefaultIfEmpty(),
                    (x, y) => new { x.HAAL, x.HEP, x.HBC_LeaveCode, HOD_Lang = y })
                .Select(x => new MaintenanceOfAnnualLeaveEntitlementDto
                {
                    USER_GUID = x.HAAL.USER_GUID,
                    Factory = x.HAAL.Factory,
                    Employee_ID = x.HAAL.Employee_ID,
                    Annual_Start = x.HAAL.Annual_Start.ToString("yyyy/MM/dd"),
                    Annual_End = x.HAAL.Annual_End.ToString("yyyy/MM/dd"),
                    Department_Code = x.HEP.Department_Code,
                    Department_Name = x.HOD_Lang.Department_Name,
                    Department_Code_Name = x.HOD_Lang != null && !string.IsNullOrWhiteSpace(x.HOD_Lang.Department_Name)
                        ? x.HOD_Lang.Department_Code + "-" + x.HOD_Lang.Department_Name : x.HEP.Department_Code,
                    Leave_Code = x.HAAL.Leave_Code,
                    Leave_Code_Old = x.HAAL.Leave_Code,
                    Leave_Code_Name = x.HBC_LeaveCode.Code_Name,
                    Local_Full_Name = x.HEP.Local_Full_Name,
                    Previous_Hours = x.HAAL.Previous_Hours.ToString(),
                    Total_Days = x.HAAL.Total_Days,
                    Total_Hours = x.HAAL.Total_Hours,
                    Year_Hours = x.HAAL.Year_Hours.ToString(),
                    Update_By = x.HAAL.Update_By,
                    Update_Time = x.HAAL.Update_Time.ToString("yyyy/MM/dd HH:mm:ss")
                }).ToListAsync();

            if (!string.IsNullOrWhiteSpace(param.Department))
                result = result.FindAll(x => x.Department_Code == param.Department);

            return result;
        }

        private async Task<List<DepartmentMain>> GetDepartmentMain(string language)
        {
            ExpressionStarter<HRMS_Org_Department> predDept = PredicateBuilder.New<HRMS_Org_Department>(true);
            ExpressionStarter<HRMS_Basic_Factory_Comparison> predCom = PredicateBuilder.New<HRMS_Basic_Factory_Comparison>(x => x.Kind == "1");
            var data = await QueryDepartment(predDept, predCom, language)
                .Select(
                    x => new DepartmentMain
                    {
                        Division = x.Department.Division,
                        Factory = x.Department.Factory,
                        Department_Code = x.Department.Department_Code,
                        Department_Name = $"{(x.Language != null ? x.Language.Name : x.Department.Department_Name)}"
                    }
                ).ToListAsync();

            return data;
        }
        #endregion

        #region ExportExcel
        public async Task<FileContentResult> ExportExcel()
        {
            string filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Resources\\Template\\AttendanceMaintenance\\5_1_7_MaintenanceOfAnnualLeaveEntitlement\\Template.xlsx"
            );

            if (!File.Exists(filePath)) return null;

            string fileExtension = GetContentType(filePath);

            MemoryStream memory = new();
            await using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return new FileContentResult(memory.ToArray(), fileExtension);
        }
        public async Task<OperationResult> DownloadExcel(MaintenanceOfAnnualLeaveEntitlementParam param, string userName)
        {
            var result = await GetData(param);
            ExcelResult excelResult = Download(
                result, userName, param,
                "Resources\\Template\\AttendanceMaintenance\\5_1_7_MaintenanceOfAnnualLeaveEntitlement\\Download.xlsx"
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }
        public static ExcelResult Download(List<MaintenanceOfAnnualLeaveEntitlementDto> data, string userName, MaintenanceOfAnnualLeaveEntitlementParam param, string subPath, ConfigDownload configDownload = null)
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
                worksheet.Cells["B2"].PutValue(param.Factory);
                worksheet.Cells["D2"].PutValue(param.AvailableRange_Start);
                worksheet.Cells["F2"].PutValue(param.AvailableRange_End);
                worksheet.Cells["I2"].PutValue(userName);
                worksheet.Cells["K2"].PutValue(DateTime.Now.ToString("yyyy/MM/dd  HH:mm:ss"));
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

        #region UploadExcel
        public async Task<OperationResult> UploadExcel(UploadFormData formData)
        {
            ExcelResult resp = ExcelUtility.CheckExcel(
                formData.File,
                "Resources\\Template\\AttendanceMaintenance\\5_1_7_MaintenanceOfAnnualLeaveEntitlement\\Template.xlsx"
            );
            if (!resp.IsSuccess)
                return new OperationResult(false, resp.Error);

            var leaveCodes = await GetListLeaveCode(formData.Language);

            DateTime now = DateTime.Now;

            List<HRMS_Att_Annual_Leave> annualLeaves = new();
            List<UploadReport> reports = new();

            var roles = _repositoryAccessor.HRMS_Basic_Role.FindAll(x => formData.UserRoles.Contains(x.Role)).ToList();
            for (int i = resp.WsTemp.Cells.Rows.Count; i < resp.Ws.Cells.Rows.Count; i++)
            {
                List<string> errors = new();
                string factory = resp.Ws.Cells[i, 0].StringValue ?? "";
                if (string.IsNullOrWhiteSpace(factory) || factory.Length > 10)
                    errors.Add($"Factory invalid.");

                if (!roles.Any(x => x.Factory == factory))
                    errors.Add($"Factory does not match the role group.");

                string empId = resp.Ws.Cells[i, 1].StringValue ?? "";
                if (string.IsNullOrWhiteSpace(empId) || empId.Length > 16)
                    errors.Add($"Employee ID invalid.");

                if (resp.Ws.Cells[i, 2].Value is null)
                    errors.Add($"Available Range (Start) at row {i} cannot be blank.");

                bool validateStart = DateTime.TryParseExact(resp.Ws.Cells[i, 2].StringValue, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime annual_Start);
                if (!validateStart)
                    errors.Add($"Available Range (Start) at row {i} wrong format.");

                if (resp.Ws.Cells[i, 3].Value is null)
                    errors.Add($"Available Range (End) cannot be blank.");

                bool validateEnd = DateTime.TryParseExact(resp.Ws.Cells[i, 3].StringValue, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime annual_End);
                if (!validateEnd)
                    errors.Add($"Available Range (End) at row {i} wrong format.");

                string leaveCode = resp.Ws.Cells[i, 4].StringValue ?? "";
                if (string.IsNullOrWhiteSpace(leaveCode) || leaveCode.Length > 10)
                    errors.Add($"Employee ID invalid.");

                if (!leaveCodes.Any(x => x.Key.Trim() == leaveCode.Trim()))
                    errors.Add($"Leave Code not exsit.");

                if (resp.Ws.Cells[i, 5].Value is null)
                    errors.Add($"Previous period Transfer hour(s) cannot be blank.");

                if (resp.Ws.Cells[i, 6].Value is null)
                    errors.Add($"Yearly Hours cannot be blank.");

                if (annualLeaves.Any(
                        x => x.Annual_Start == annual_Start &&
                        x.Factory == factory &&
                        x.Employee_ID == empId &&
                        x.Leave_Code == leaveCode))
                {
                    errors.Add($"Factory, Available Range(Start), Employee ID, Leave Code duplicate.");
                }


                if (await _repositoryAccessor.HRMS_Att_Annual_Leave.AnyAsync(
                    x => x.Annual_Start == annual_Start &&
                    x.Factory == factory &&
                    x.Employee_ID == empId &&
                    x.Leave_Code == leaveCode))
                {
                    errors.Add($"Factory, Available Range(Start), Employee ID, Leave Code exsited.");
                }

                string USER_GUID = (await _repositoryAccessor.HRMS_Emp_Personal.FirstOrDefaultAsync(x => x.Factory == factory && x.Employee_ID == empId))?.USER_GUID;
                if (string.IsNullOrWhiteSpace(USER_GUID))
                    errors.Add($"Employee ID not exsit.");

                if (errors.Any())
                {
                    reports.Add(new()
                    {
                        Factory = factory,
                        Employee_ID = empId,
                        AvailableRange_Start = resp.Ws.Cells[i, 2].Value,
                        AvailableRange_End = resp.Ws.Cells[i, 3].Value,
                        Leave_Code = leaveCode,
                        Previous_Hours = resp.Ws.Cells[i, 5].Value,
                        Year_Hours = resp.Ws.Cells[i, 6].Value,
                        Status = "N",
                        Error = string.Join("\n", errors)
                    });
                }
                else
                {
                    reports.Add(new()
                    {
                        Factory = factory,
                        Employee_ID = empId,
                        AvailableRange_Start = resp.Ws.Cells[i, 2].Value,
                        AvailableRange_End = resp.Ws.Cells[i, 3].Value,
                        Leave_Code = leaveCode,
                        Previous_Hours = resp.Ws.Cells[i, 5].Value,
                        Year_Hours = resp.Ws.Cells[i, 6].Value,
                        Status = "Y"
                    });

                    decimal previous_Hours = (decimal)resp.Ws.Cells[i, 5].DoubleValue;

                    decimal year_Hours = (decimal)resp.Ws.Cells[i, 6].DoubleValue;

                    annualLeaves.Add(new()
                    {
                        Annual_Start = annual_Start,
                        Annual_End = annual_End,
                        Employee_ID = empId,
                        Factory = factory,
                        Leave_Code = leaveCode,
                        USER_GUID = USER_GUID,
                        Previous_Hours = previous_Hours,
                        Total_Hours = previous_Hours + year_Hours,
                        Total_Days = (previous_Hours + year_Hours) / 8,
                        Year_Hours = year_Hours,
                        Update_By = formData.UserName,
                        Update_Time = now
                    });
                }
            }

            if (reports.Any(x => x.Status == "N"))
            {
                MemoryStream memoryStream = new();
                string template = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Resources\\Template\\AttendanceMaintenance\\5_1_7_MaintenanceOfAnnualLeaveEntitlement\\Report.xlsx"
                );
                WorkbookDesigner workbookDesigner = new() { Workbook = new Workbook(template) };
                Worksheet worksheet = workbookDesigner.Workbook.Worksheets[0];
                workbookDesigner.SetDataSource("result", reports);
                workbookDesigner.Process();
                worksheet.AutoFitColumns(worksheet.Cells.MinDataColumn, worksheet.Cells.MaxColumn);
                worksheet.AutoFitRows(worksheet.Cells.MinDataRow + 1, worksheet.Cells.MaxRow);
                workbookDesigner.Workbook.Save(memoryStream, SaveFormat.Xlsx);
                return new OperationResult { IsSuccess = false, Data = memoryStream.ToArray(), Error = "Please check downloaded Error Report" };
            }

            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                _repositoryAccessor.HRMS_Att_Annual_Leave.AddMultiple(annualLeaves);
                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();

                var uploadFile = $"Annual_Leave";
                string uploadPath = @"uploaded\excels\5.7_Maintenance_Of_Annual_Leave_Entitlement_" + now.ToString("yyyyMMdd_HHmmss");
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", uploadPath);

                await FilesUtility.SaveFile(formData.File, folder, uploadFile);

                return new OperationResult { IsSuccess = true };
            }
            catch (Exception)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult { IsSuccess = false };
            }
        }
        #endregion

        #region Frivate functions
        private async Task<List<KeyValuePair<string, string>>> GetBasicCodes(string language, ExpressionStarter<HRMS_Basic_Code> predicate)
        {
            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(predicate, true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (x, y) => new { code = x, codeLang = y })
                .SelectMany(
                    x => x.codeLang.DefaultIfEmpty(),
                    (x, y) => new { x.code, codeLang = y })
                .Select(x => new KeyValuePair<string, string>(x.code.Code, $"{x.code.Code} - {x.codeLang.Code_Name ?? x.code.Code_Name}"))
                .Distinct().ToListAsync();

            return data;
        }

        private IOrderedQueryable<DepartmentJoinResult> QueryDepartment(ExpressionStarter<HRMS_Org_Department> predDept, ExpressionStarter<HRMS_Basic_Factory_Comparison> predCom, string language)
        {
            var data = _repositoryAccessor.HRMS_Org_Department.FindAll(predDept, true)
                .Join(_repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(predCom, true),
                    department => department.Division,
                    factoryComparison => factoryComparison.Division,
                    (department, factoryComparison) => department)
                .GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    department => new { department.Factory, department.Department_Code },
                    language => new { language.Factory, language.Department_Code },
                    (department, language) => new { Department = department, Language = language })
                .SelectMany(
                    x => x.Language.DefaultIfEmpty(),
                    (x, language) => new DepartmentJoinResult { Department = x.Department, Language = language })
                .OrderBy(x => x.Department.Department_Code);

            return data;
        }

        private static string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out string contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        public async Task<OperationResult> CheckExistedData(string Annual_Start, string Factory, string Employee_ID, string Leave_Code)
        {
            return new OperationResult(await _repositoryAccessor.HRMS_Att_Annual_Leave.AnyAsync(x =>
                x.Employee_ID == Employee_ID &&
                x.Factory == Factory &&
                x.Leave_Code == Leave_Code &&
                x.Annual_Start.Date == Convert.ToDateTime(Annual_Start).Date));
        }
        #endregion
    }
}