using System.Globalization;
using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Models;
using Aspose.Cells;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance
{
    public class S_5_1_1_FactoryCalendar : BaseServices, I_5_1_1_FactoryCalendar
    {
        public S_5_1_1_FactoryCalendar(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult> CheckExistedData(string Division, string Factory, string Att_Date)
        {
            var data = await _repositoryAccessor.HRMS_Att_Calendar.FirstOrDefaultAsync(x =>
                x.Division == Division &&
                x.Factory == Factory &&
                x.Att_Date.Date == Convert.ToDateTime(Att_Date).Date);
            return new OperationResult(data is not null, data);
        }

        public async Task<List<KeyValuePair<string, string>>> GetDropDownList(FactoryCalendar_MainParam param, string userName)
        {
            var factorys = await Queryt_Factory_AddList(userName);

            var HBC = await _repositoryAccessor.HRMS_Basic_Code.FindAll().ToListAsync();
            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower()).ToList();
            var result = new List<KeyValuePair<string, string>>();
            var data = HBC.GroupJoin(HBCL,
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (x, y) => new { hbc = x, hbcl = y })
                    .SelectMany(x => x.hbcl.DefaultIfEmpty(),
                    (x, y) => new { x.hbc, hbcl = y });
            result.AddRange(data.Where(x => x.hbc.Type_Seq == "1").Select(x => new KeyValuePair<string, string>("DI", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            result.AddRange(data.Where(x => x.hbc.Type_Seq == "39").Select(x => new KeyValuePair<string, string>("TY", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            if (!string.IsNullOrWhiteSpace(param.Division))
            {
                var HBFC = _repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(x => x.Division == param.Division && x.Kind == "1" && factorys.Contains(x.Factory)).ToList();
                if (HBFC.Any())
                {
                    var dataFilter = data.Join(HBFC,
                        x => new { Factory = x.hbc.Code, x.hbc.Type_Seq },
                        y => new { y.Factory, Type_Seq = "2" },
                        (x, y) => new { x.hbc, x.hbcl, hbfc = y });
                    result.AddRange(dataFilter.Where(x => x.hbc.Type_Seq == "2").Select(x => new KeyValuePair<string, string>("FA", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
                }
            }
            return result;
        }

        public async Task<OperationResult> GetSearchDetail(PaginationParam paginationParams, FactoryCalendar_MainParam searchParam)
        {
            var result = await GetData(searchParam);
            if (!result.IsSuccess)
                return result;
            return new OperationResult(true, PaginationUtility<FactoryCalendar_Table>.Create(result.Data as List<FactoryCalendar_Table>, paginationParams.PageNumber, paginationParams.PageSize));
        }
        private async Task<OperationResult> GetData(FactoryCalendar_MainParam param)
        {
            var predicate = PredicateBuilder.New<HRMS_Att_Calendar>(true);
            if (string.IsNullOrWhiteSpace(param.Division)
             || string.IsNullOrWhiteSpace(param.Factory)
             || string.IsNullOrWhiteSpace(param.Month_Str)
             || !DateTime.TryParseExact(param.Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime dateValue))
                return new OperationResult(false, "InvalidInput");
            predicate.And(x => x.Division == param.Division);
            predicate.And(x => x.Factory == param.Factory);
            predicate.And(x => x.Att_Date.Year == dateValue.Year && x.Att_Date.Month == dateValue.Month);
            var HAC = await _repositoryAccessor.HRMS_Att_Calendar.FindAll(predicate).ToListAsync();
            if (HAC == null)
                return new OperationResult(false, "NotExitedData");
            var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "39");
            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower());
            var HBC_Lang = HBC.GroupJoin(HBCL,
                x => new { x.Type_Seq, x.Code },
                y => new { y.Type_Seq, y.Code },
                    (x, y) => new { HBC = x, HBCL = y })
                .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (x, y) => new { x.HBC, HBCL = y })
                .Select(x => new
                {
                    x.HBC.Code,
                    Code_Name = x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name
                })
                .Distinct().ToList();
            var data = HAC
                .GroupJoin(HBC_Lang,
                    x => x.Type_Code,
                    y => y.Code,
                    (x, y) => new { HAC = x, HBC_Lang = y })
                .SelectMany(x => x.HBC_Lang.DefaultIfEmpty(),
                        (x, y) => new { x.HAC, HBC_Lang = y })
                .ToList();
            var result = data.Select(x => new FactoryCalendar_Table
            {
                Division = x.HAC.Division,
                Factory = x.HAC.Factory,
                Att_Date = x.HAC.Att_Date,
                Att_Date_Str = x.HAC.Att_Date.ToString("yyyy/MM/dd"),
                Type_Code = x.HAC.Type_Code,
                Type_Code_Name = x.HBC_Lang?.Code_Name ?? "",
                Describe = x.HAC.Describe,
                Update_By = x.HAC.Update_By,
                Update_Time = x.HAC.Update_Time,
                Update_Time_Str = x.HAC.Update_Time.ToString("yyyy/MM/dd HH:mm:ss"),
            }).ToList();
            return new OperationResult(true, result);
        }
        public async Task<OperationResult> PostData(FactoryCalendar_Table input)
        {
            var predicate = PredicateBuilder.New<HRMS_Att_Calendar>(true);
            if (string.IsNullOrWhiteSpace(input.Division)
             || string.IsNullOrWhiteSpace(input.Factory)
             || string.IsNullOrWhiteSpace(input.Att_Date_Str)
             || !DateTime.TryParseExact(input.Att_Date_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime dateValue))
                return new OperationResult(false, "InvalidInput");
            predicate.And(x => x.Division == input.Division);
            predicate.And(x => x.Factory == input.Factory);
            predicate.And(x => x.Att_Date.Date == dateValue.Date);
            if (await _repositoryAccessor.HRMS_Att_Calendar.AnyAsync(predicate))
                return new OperationResult(false, "AlreadyExitedData");
            try
            {
                HRMS_Att_Calendar data = new()
                {
                    Division = input.Division,
                    Factory = input.Factory,
                    Att_Date = Convert.ToDateTime(input.Att_Date_Str),
                    Type_Code = input.Type_Code,
                    Describe = input.Describe,
                    Update_By = input.Update_By,
                    Update_Time = Convert.ToDateTime(input.Update_Time_Str)
                };
                _repositoryAccessor.HRMS_Att_Calendar.Add(data);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false);
            }
        }
        public async Task<OperationResult> PutData(FactoryCalendar_Table input)
        {
            var predicate = PredicateBuilder.New<HRMS_Att_Calendar>(true);
            if (string.IsNullOrWhiteSpace(input.Division)
             || string.IsNullOrWhiteSpace(input.Factory)
             || string.IsNullOrWhiteSpace(input.Att_Date_Str)
             || !DateTime.TryParseExact(input.Att_Date_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime dateValue))
                return new OperationResult(false, "InvalidInput");
            predicate.And(x => x.Division == input.Division);
            predicate.And(x => x.Factory == input.Factory);
            predicate.And(x => x.Att_Date.Date == dateValue.Date);
            var HAC = await _repositoryAccessor.HRMS_Att_Calendar.FirstOrDefaultAsync(predicate);
            if (HAC == null)
                return new OperationResult(false, "NotExitedData");
            try
            {
                HAC.Describe = input.Describe;
                HAC.Update_By = input.Update_By;
                HAC.Update_Time = Convert.ToDateTime(input.Update_Time_Str);
                _repositoryAccessor.HRMS_Att_Calendar.Update(HAC);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false);
            }
        }

        public async Task<OperationResult> DeleteData(FactoryCalendar_Table input)
        {
            var predicate = PredicateBuilder.New<HRMS_Att_Calendar>(true);
            if (string.IsNullOrWhiteSpace(input.Division)
            || string.IsNullOrWhiteSpace(input.Factory)
            || string.IsNullOrWhiteSpace(input.Att_Date_Str)
            || !DateTime.TryParseExact(input.Att_Date_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime dateValue))
                return new OperationResult(false, "InvalidInput");
            predicate.And(x => x.Division == input.Division);
            predicate.And(x => x.Factory == input.Factory);
            predicate.And(x => x.Att_Date.Date == dateValue.Date);

            var HAC = await _repositoryAccessor.HRMS_Att_Calendar.FirstOrDefaultAsync(predicate);

            if (HAC == null)
                return new OperationResult(false, "NotExitedData");
            try
            {
                _repositoryAccessor.HRMS_Att_Calendar.Remove(HAC);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false);
            }
        }

        #region Excel functions
        public async Task<OperationResult> DownloadExcel(FactoryCalendar_MainParam param)
        {
            var result = await GetData(param);
            if (!result.IsSuccess)
                return result;
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                result.Data as List<FactoryCalendar_Table>, 
                "Resources\\Template\\AttendanceMaintenance\\5_1_1_FactoryCalendar\\Download.xlsx"
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }
        public async Task<OperationResult> UploadExcel(IFormFile file, List<string> role_List, string username)
        {
            ExcelResult resp = ExcelUtility.CheckExcel(
                file, 
                "Resources\\Template\\AttendanceMaintenance\\5_1_1_FactoryCalendar\\Template.xlsx"
            );
            if (!resp.IsSuccess)
                return new OperationResult(false, resp.Error);
            List<HRMS_Att_Calendar> excelDataList = new();
            List<FactoryCalendar_Table> excelReportList = new();
            var roles = _repositoryAccessor.HRMS_Basic_Role.FindAll(x => role_List.Contains(x.Role)).ToList();
            bool isPassed = true;
            for (int i = resp.WsTemp.Cells.Rows.Count; i < resp.Ws.Cells.Rows.Count; i++)
            {
                bool isKeyPassed = true;
                string errorMessage = "";
                if (resp.Ws.Cells[i, 0].Value == null)
                {
                    errorMessage += $"column Division cannot be blank.\n";
                    isKeyPassed = false;
                }
                if (resp.Ws.Cells[i, 0].StringValue.Length > 25)
                {
                    errorMessage += $"column Division's length higher than required.\n";
                    isKeyPassed = false;
                }
                if (resp.Ws.Cells[i, 1].Value == null)
                {
                    errorMessage += $"column Factory cannot be blank.\n";
                    isKeyPassed = false;
                }
                else
                {
                    if (!roles.Any(x => x.Factory == resp.Ws.Cells[i, 1].StringValue))
                    {
                        errorMessage += $"uploaded [Factory] data does not match the role group.\n";
                        isKeyPassed = false;
                    }
                }
                if (resp.Ws.Cells[i, 1].StringValue.Length > 10)
                {
                    errorMessage += $"column Factory's length higher than required ";
                    isKeyPassed = false;
                }
                if (resp.Ws.Cells[i, 2].Value == null)
                    errorMessage += $"column Type_Code cannot be blank.\n";
                if (resp.Ws.Cells[i, 2].StringValue.Length > 10)
                    errorMessage += $"column Type_Code's length higher than required.\n";
                if (string.IsNullOrWhiteSpace(resp.Ws.Cells[i, 3].StringValue))
                {
                    errorMessage += $"column Att_Date cannot be blank.\n";
                    isKeyPassed = false;
                }
                if (!string.IsNullOrWhiteSpace(resp.Ws.Cells[i, 3].StringValue) && resp.Ws.Cells[i, 3].Value is not DateTime)
                {
                    errorMessage += $"column Att_Date's style is not date.\n";
                    isKeyPassed = false;
                }
                if (resp.Ws.Cells[i, 4].Value == null)
                    errorMessage += $"column Describe cannot be blank.\n";
                if (resp.Ws.Cells[i, 4].StringValue.Length > 50)
                    errorMessage += $"column Describe's length higher than required.\n";

                string division = resp.Ws.Cells[i, 0].Value != null ? resp.Ws.Cells[i, 0].StringValue.Trim() : null;
                string factory = resp.Ws.Cells[i, 1].Value != null ? resp.Ws.Cells[i, 1].StringValue.Trim() : null;
                string typeCode = resp.Ws.Cells[i, 2].Value != null ? resp.Ws.Cells[i, 2].StringValue.Trim() : null;
                string attDate = resp.Ws.Cells[i, 3].Value != null ? resp.Ws.Cells[i, 3].StringValue.Trim() : null;
                string describe = resp.Ws.Cells[i, 4].Value != null ? resp.Ws.Cells[i, 4].StringValue.Trim() : null;

                DateTime.TryParseExact(attDate, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime attDateOut);

                if (isKeyPassed)
                {
                    if (_repositoryAccessor.HRMS_Att_Calendar.Any(x => x.Division == division && x.Factory == factory && x.Att_Date.Date == attDateOut.Date))
                        errorMessage += $"Data is already existed.\n";
                    if (excelReportList.Any(x => x.Division == division && x.Factory == factory && x.Att_Date_Str == attDate))
                        errorMessage += $"Identity Conflict Data.\n";
                }

                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    HRMS_Att_Calendar excelData = new()
                    {
                        Division = division,
                        Factory = factory,
                        Att_Date = attDateOut,
                        Type_Code = typeCode,
                        Describe = describe,
                        Update_By = username,
                        Update_Time = DateTime.Now
                    };
                    excelDataList.Add(excelData);
                }
                else
                {
                    isPassed = false;
                    errorMessage = errorMessage.Remove(errorMessage.Length - 1);
                }
                FactoryCalendar_Table report = new()
                {
                    Division = division,
                    Factory = factory,
                    Att_Date_Str = attDate,
                    Type_Code = typeCode,
                    Describe = describe,
                    Error_Message = errorMessage
                };
                excelReportList.Add(report);
            }
            if (!isPassed)
            {
                MemoryStream memoryStream = new();
                string fileLocation = Path.Combine(
                    Directory.GetCurrentDirectory(), 
                    "Resources\\Template\\AttendanceMaintenance\\5_1_1_FactoryCalendar\\Report.xlsx"
                );
                WorkbookDesigner workbookDesigner = new() { Workbook = new Workbook(fileLocation) };
                Worksheet worksheet = workbookDesigner.Workbook.Worksheets[0];
                workbookDesigner.SetDataSource("result", excelReportList);
                workbookDesigner.Process();
                worksheet.AutoFitColumns(worksheet.Cells.MinDataColumn, worksheet.Cells.MaxColumn);
                worksheet.AutoFitRows(worksheet.Cells.MinDataRow + 1, worksheet.Cells.MaxRow);
                workbookDesigner.Workbook.Save(memoryStream, SaveFormat.Xlsx);
                return new OperationResult { IsSuccess = false, Data = memoryStream.ToArray(), Error = "Please check downloaded Error Report" };
            }
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                _repositoryAccessor.HRMS_Att_Calendar.AddMultiple(excelDataList);
                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                return new OperationResult { IsSuccess = true };
            }
            catch (Exception)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult { IsSuccess = false };
            }
        }

        public async Task<OperationResult> DownloadExcelTemplate()
        {
            string path = Path.Combine(
                Directory.GetCurrentDirectory(), 
                "Resources\\Template\\AttendanceMaintenance\\5_1_1_FactoryCalendar\\Template.xlsx"
            );
            if (!File.Exists(path))
                return await Task.FromResult(new OperationResult(false, "NotExitedFile"));
            byte[] bytes = File.ReadAllBytes(path);
            return await Task.FromResult(new OperationResult { IsSuccess = true, Data = $"data:xlsx;base64,{Convert.ToBase64String(bytes)}" });
        }


        #endregion
    }
}