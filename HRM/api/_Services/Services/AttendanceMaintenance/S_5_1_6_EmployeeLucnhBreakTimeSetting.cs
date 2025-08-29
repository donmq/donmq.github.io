using System.Globalization;
using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Models;
using Aspose.Cells;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SDCores;

namespace API._Services.Services.AttendanceMaintenance
{
    public class S_5_1_6_EmployeeLucnhBreakTimeSetting : BaseServices, I_5_1_6_EmployeeLucnhBreakTimeSetting
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public S_5_1_6_EmployeeLucnhBreakTimeSetting(DBContext dbContext,IWebHostEnvironment webHostEnvironment) : base(dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        private static readonly string rootPath = Directory.GetCurrentDirectory();

        #region GetList
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

        public async Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language)
        {
            return await _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Factory == factory, true)
                .Join(
                    _repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(b => b.Kind == "1" && b.Factory == factory, true),
                    department => department.Division,
                    factoryComparison => factoryComparison.Division,
                    (department, factoryComparison) => department
                )
                .GroupJoin(
                    _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    department => new { department.Factory, department.Department_Code },
                    language => new { language.Factory, language.Department_Code },
                    (department, language) => new { Department = department, Language = language }
                )
                .SelectMany(
                    x => x.Language.DefaultIfEmpty(),
                    (x, language) => new { x.Department, Language = language }
                )
                .OrderBy(x => x.Department.Department_Code)
                .Select(
                    x => new KeyValuePair<string, string>(
                        x.Department.Department_Code,
                        $"{x.Department.Department_Code} - {(x.Language != null ? x.Language.Name : x.Department.Department_Name)}"
                    )
                )
                .ToListAsync();
        }
        #endregion

        #region GetData
        private async Task<List<HRMS_Att_LunchtimeDto>> GetData(EmployeeLunchBreakTimeSettingParam param, List<string> roleList)
        {
            var pred = PredicateBuilder.New<HRMS_Att_Lunchtime>(x => x.Factory == param.Factory);
            var predPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(x => x.Factory == param.Factory);

            if (!string.IsNullOrWhiteSpace(param.Employee_ID))
                pred = pred.And(x => x.Employee_ID.Contains(param.Employee_ID.Trim()));
            if (param.In_Service == "Y")
                predPersonal = predPersonal.And(x => x.Resign_Date == null);
            if (param.In_Service == "N")
                predPersonal = predPersonal.And(x => x.Resign_Date != null);
            if (!string.IsNullOrWhiteSpace(param.Department_From) && !string.IsNullOrWhiteSpace(param.Department_To))
                predPersonal = predPersonal.And(x => x.Department.CompareTo(param.Department_From) >= 0 && x.Department.CompareTo(param.Department_To) <= 0);

            var HAL = await _repositoryAccessor.HRMS_Att_Lunchtime.FindAll(pred, true).ToListAsync();

            // Get allow employee by recent account roles
            var HEP_info = Query_Permission_Data_Filter(roleList, predPersonal).Result.Select(x => new
            {
                x.USER_GUID,
                x.Local_Full_Name,
                Division = x.Employment_Status == "A" || x.Employment_Status == "S" ? x.Assigned_Division : x.Division,
                Factory = x.Employment_Status == "A" || x.Employment_Status == "S" ? x.Assigned_Factory : x.Factory,
                Department_Code = x.Employment_Status == "A" || x.Employment_Status == "S" ? x.Assigned_Department : x.Department,
            });
            var HOD = _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Factory == param.Factory);
            var HODL = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower());
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
                }).ToList();
            var result = HAL
                .Join(HEP_info,
                    x => x.USER_GUID,
                    y => y.USER_GUID,
                    (x, y) => new { HAL = x, HEP_info = y })
                .GroupJoin(HOD_Lang,
                    x => new { x.HEP_info.Department_Code, x.HEP_info.Factory, x.HEP_info.Division },
                    y => new { y.Department_Code, y.Factory, y.Division },
                    (x, y) => new { x.HAL, x.HEP_info, HOD_Lang = y })
                .SelectMany(x => x.HOD_Lang.DefaultIfEmpty(),
                    (x, y) => new { x.HAL, x.HEP_info, HOD_Lang = y })
                .Select(x => new HRMS_Att_LunchtimeDto
                {
                    USER_GUID = x.HAL.USER_GUID,
                    Factory = x.HAL.Factory,
                    Department_Code = x.HEP_info.Department_Code,
                    Department_Name = x.HOD_Lang?.Department_Name,
                    Department_Code_Name = x.HOD_Lang != null && !string.IsNullOrWhiteSpace(x.HOD_Lang.Department_Name)
                        ? x.HOD_Lang.Department_Code + "-" + x.HOD_Lang.Department_Name : x.HEP_info.Department_Code,
                    Employee_ID = x.HAL.Employee_ID,
                    Local_Full_Name = x.HEP_info.Local_Full_Name ?? "",
                    Lunch_Start = x.HAL.Lunch_Start,
                    Lunch_End = x.HAL.Lunch_End,
                    Update_By = x.HAL.Update_By,
                    Update_Time = x.HAL.Update_Time.ToString("yyyy/MM/dd  HH:mm:ss")
                })
               .OrderBy(x => x.Factory).ThenBy(x => x.Department_Code).ThenBy(x => x.Employee_ID)
               .ToList();
            return result;
        }

        public async Task<PaginationUtility<HRMS_Att_LunchtimeDto>> GetDataPagination(PaginationParam pagination, EmployeeLunchBreakTimeSettingParam param, List<string> roleList)
        {
            var data = await GetData(param, roleList);
            return PaginationUtility<HRMS_Att_LunchtimeDto>.Create(data, pagination.PageNumber, pagination.PageSize);
        }
        #endregion

        #region Download
        public Task<OperationResult> DownloadExcelTemplate()
        {
            var path = Path.Combine(
                _webHostEnvironment.ContentRootPath,
                "Resources\\Template\\AttendanceMaintenance\\5_1_6_EmployeeLunchBreakTimeSetting\\Template.xlsx"
            );
            var workbook = new Workbook(path);
            var design = new WorkbookDesigner(workbook);
            MemoryStream stream = new();
            design.Workbook.Save(stream, SaveFormat.Xlsx);
            var result = stream.ToArray();
            return Task.FromResult(new OperationResult(true, null, result));
        }
        public async Task<OperationResult> DownloadExcel(EmployeeLunchBreakTimeSettingParam param, List<string> roleList, string userName)
        {
            var result = await GetData(param, roleList);
            ExcelResult excelResult = DownloadExcel(
                result, userName, param.Factory,
                "Resources\\Template\\AttendanceMaintenance\\5_1_6_EmployeeLunchBreakTimeSetting\\Download.xlsx"
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }
        public static ExcelResult DownloadExcel<T>(List<T> data, string userName, string factory, string subPath, ConfigDownload configDownload = null)
        {
            configDownload ??= new ConfigDownload();
            if (!data.Any())
                return new ExcelResult(isSuccess: false, "No data for excel download");
            try
            {
                MemoryStream memoryStream = new();
                string file = Path.Combine(rootPath, subPath);
                WorkbookDesigner obj = new()
                {
                    Workbook = new Workbook(file)
                };
                Worksheet worksheet = obj.Workbook.Worksheets[0];
                worksheet.Cells["E2"].PutValue(userName);
                worksheet.Cells["B2"].PutValue(factory);
                worksheet.Cells["H2"].PutValue(DateTime.Now.ToString("yyyy/MM/dd  HH:mm:ss"));
                obj.SetDataSource("result", data);
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

        #region Upload
        public async Task<OperationResult> UploadData(IFormFile file, List<string> role_List, string userName)
        {
            ExcelResult resp = ExcelUtility.CheckExcel(
                file,
                "Resources\\Template\\AttendanceMaintenance\\5_1_6_EmployeeLunchBreakTimeSetting\\Template.xlsx"
            );
            if (!resp.IsSuccess)
                return new OperationResult(false, resp.Error);

            List<HRMS_Att_Lunchtime> lunchTimeAdd = new();
            List<HRMS_Att_Lunchtime> lunchTimeUpdate = new();
            List<HRMS_Att_LunchtimeReport> excelReportList = new();

            var roles = await _repositoryAccessor.HRMS_Basic_Role
                .FindAll(x => role_List.Contains(x.Role))
                .ToListAsync();

            var factoryCodes = (await _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory)
                .ToListAsync())
                .ToDictionary(x => x.Code);

            //  Query_EmpPersonal_Add(Factory,Employee_ID)   
            var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll(true).Select(x => new
            {
                HEP = x,
                Actual_Employee_ID = x.Employment_Status == "A" || x.Employment_Status == "S" ? x.Assigned_Employee_ID : x.Employee_ID,
                Actual_Division = x.Employment_Status == "A" || x.Employment_Status == "S" ? x.Assigned_Division : x.Division,
                Actual_Factory = x.Employment_Status == "A" || x.Employment_Status == "S" ? x.Assigned_Factory : x.Factory,
            });
            var HBFC = _repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(x => x.Kind == "1");
            var empPersonals = (await HEP
                .Join(HBFC,
                    x => new { Division = x.Actual_Division, Factory = x.Actual_Factory },
                    HBFC => new { HBFC.Division, HBFC.Factory },
                    (x, y) => new { x.HEP.USER_GUID, x.Actual_Employee_ID, x.Actual_Factory })
                .ToListAsync())
                .GroupBy(x => x.Actual_Factory)
                .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.USER_GUID, y => y.Actual_Employee_ID));

            var existingLunchTimes = (await _repositoryAccessor.HRMS_Att_Lunchtime.FindAll(true)
                .ToListAsync())
                .GroupBy(x => x.Factory)
                .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.Employee_ID, y => y));

            bool isPassed = true;

            for (int i = resp.WsTemp.Cells.Rows.Count; i < resp.Ws.Cells.Rows.Count; i++)
            {
                string errorMessage = "";
                string factory = resp.Ws.Cells[i, 0].StringValue?.Trim();
                string employeeID = resp.Ws.Cells[i, 1].StringValue?.Trim();
                string lunchStartTime = resp.Ws.Cells[i, 2].StringValue?.Trim();
                string lunchEndTime = resp.Ws.Cells[i, 3].StringValue?.Trim();

                if (!factoryCodes.ContainsKey(factory))
                    errorMessage += $"Factory is not valid for user {userName}. ";

                if (!roles.Any(x => x.Factory == factory))
                    errorMessage += $"uploaded [Factory] data does not match the role group. ";

                if (string.IsNullOrWhiteSpace(employeeID) || employeeID.Length > 16)
                    errorMessage += $"Employee ID is invalid. ";

                bool isValidLunchStartTime = !string.IsNullOrEmpty(lunchStartTime) &&
                                            (lunchStartTime.Length == 4 || lunchStartTime.ToUpper() == "ZZZZ") &&
                                            (lunchStartTime.ToUpper() == "ZZZZ" || DateTime.TryParseExact(lunchStartTime, "HHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _));

                bool isValidLunchEndTime = !string.IsNullOrEmpty(lunchEndTime) &&
                                           (lunchEndTime.Length == 4 || lunchEndTime.ToUpper() == "ZZZZ") &&
                                           (lunchEndTime.ToUpper() == "ZZZZ" || DateTime.TryParseExact(lunchEndTime, "HHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _));

                if (!isValidLunchStartTime)
                    errorMessage += $"Lunch Start is not in the correct format (HHMM). ";

                if (!isValidLunchEndTime)
                    errorMessage += $"Lunch End is not in the correct format (HHMM). ";

                if (isValidLunchStartTime && isValidLunchEndTime && lunchStartTime.ToUpper() != "ZZZZ" && lunchEndTime.ToUpper() != "ZZZZ")
                    if (DateTime.ParseExact(lunchEndTime, "HHmm", CultureInfo.InvariantCulture) < DateTime.ParseExact(lunchStartTime, "HHmm", CultureInfo.InvariantCulture))
                        errorMessage += $"Lunch Start is later than Lunch End. ";

                string userGUID = null;

                if (empPersonals.TryGetValue(factory, out Dictionary<string, string> _dict1) && _dict1.ContainsValue(employeeID))
                    userGUID = _dict1.FirstOrDefault(x => x.Value == employeeID).Key;
                else
                    errorMessage += $"USER_GUID not found. ";

                if (lunchTimeAdd.Any(x => x.Factory == factory && x.Employee_ID == employeeID)
                || excelReportList.Any(x => x.Factory == factory && x.Employee_ID == employeeID))
                    errorMessage += $"Data are duplicated. ";

                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    if (existingLunchTimes.TryGetValue(factory, out Dictionary<string, HRMS_Att_Lunchtime> _dict2) &&
                        _dict2.TryGetValue(employeeID, out HRMS_Att_Lunchtime _HAL))
                    {
                        var existingData = _HAL;
                        if (existingData != null)
                        {
                            existingData.USER_GUID = userGUID;
                            existingData.Factory = factory;
                            existingData.Employee_ID = employeeID;
                            existingData.Lunch_Start = lunchStartTime;
                            existingData.Lunch_End = lunchEndTime;
                            existingData.Update_Time = DateTime.Now;
                            existingData.Update_By = userName;
                            lunchTimeUpdate.Add(existingData);
                        }
                    }
                    else
                    {
                        var newData = new HRMS_Att_Lunchtime
                        {
                            USER_GUID = userGUID,
                            Factory = factory,
                            Employee_ID = employeeID,
                            Lunch_Start = lunchStartTime,
                            Lunch_End = lunchEndTime,
                            Update_Time = DateTime.Now,
                            Update_By = userName,
                        };
                        lunchTimeAdd.Add(newData);
                    }
                }
                else
                {
                    isPassed = false;
                    errorMessage = errorMessage.Trim();
                }

                HRMS_Att_LunchtimeReport report = new()
                {
                    Factory = factory,
                    Employee_ID = employeeID,
                    Lunch_Start = lunchStartTime,
                    Lunch_End = lunchEndTime,
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
                    "Resources\\Template\\AttendanceMaintenance\\5_1_6_EmployeeLunchBreakTimeSetting\\Report.xlsx"
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
                    _repositoryAccessor.HRMS_Att_Lunchtime.AddMultiple(lunchTimeAdd);
                    _repositoryAccessor.HRMS_Att_Lunchtime.UpdateMultiple(lunchTimeUpdate);
                    await _repositoryAccessor.Save();
                    await _repositoryAccessor.CommitAsync();
                    string folder = "uploaded\\excels\\AttendanceMaintenance\\5_1_6_EmployeeLunchBreakTimeSetting\\Creates";
                    await FilesUtility.SaveFile(file, folder, $"EmployeeLunchBreakTimeSetting_{DateTime.Now:yyyyMMddHHmmss}");
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

        #region Delete
        public async Task<OperationResult> Delete(HRMS_Att_LunchtimeDto data)
        {
            var existingData = await _repositoryAccessor.HRMS_Att_Lunchtime
                                    .FirstOrDefaultAsync(x => x.Factory == data.Factory && x.Employee_ID == data.Employee_ID);

            if (existingData == null)
                return new OperationResult(false, "Data is not exist");

            _repositoryAccessor.HRMS_Att_Lunchtime.Remove(existingData);

            try
            {
                await _repositoryAccessor.Save();
                return new OperationResult(true, "Delete Successfully");
            }
            catch
            {
                return new OperationResult(false, "Delete failed");
            }
        }
        #endregion
    }
}