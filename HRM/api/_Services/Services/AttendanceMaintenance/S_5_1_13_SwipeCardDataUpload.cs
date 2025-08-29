using System.Text.RegularExpressions;
using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Helper.Utilities;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance
{
    /// <summary>
    /// Dựa vào cấu hình thời gian. Cho phép cập nhật dữ liệu người bấm thẻ
    /// </summary>
    public partial class S_5_1_13_SwipeCardDataUpload : BaseServices, I_5_1_13_SwipeCardDataUpload
    {
        [GeneratedRegex("\\s+")]
        private static partial Regex MyRegex();
        private readonly IWebHostEnvironment _webHostEnvironment;
        public S_5_1_13_SwipeCardDataUpload(DBContext dbContext,IWebHostEnvironment webHostEnvironment) : base(dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<KeyValuePair<string, string>>> GetFactories(string language, List<string> roleList)
        {
            var predHBC = PredicateBuilder.New<HRMS_Basic_Code>(x => x.Type_Seq == BasicCodeTypeConstant.Factory);

            var factorys = await Queryt_Factory_AddList(roleList);
            predHBC.And(x => factorys.Contains(x.Code));

            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(predHBC, true)
                        .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                           x => new { x.Type_Seq, x.Code },
                           y => new { y.Type_Seq, y.Code },
                           (x, y) => new { HBC = x, HBCL = y }
                        ).SelectMany(x => x.HBCL.DefaultIfEmpty(),
                            (x, y) => new { x.HBC, HBCL = y }
                        ).Select(x => new KeyValuePair<string, string>(
                            x.HBC.Code.Trim(),
                            x.HBC.Code.Trim() + " - " + (x.HBCL != null ? x.HBCL.Code_Name.Trim() : x.HBC.Code_Name.Trim())
                        )).Distinct().ToListAsync();
            return data;
        }
        public async Task<OperationResult> UploadExcuteSwipeData(HRMS_Att_Swipe_Card_Upload request)
        {
            #region 1. Kiểm tra Role của User upload có thuộc [Factory] hay không
            // xác thực role được cấp phép theo nhà máy [Factory] &  [Account]
            var roleAccepts = await _repositoryAccessor.HRMS_Basic_Role.FindAll(x => x.Factory == request.Factory, true)
                                .Join(_repositoryAccessor.HRMS_Basic_Account_Role.FindAll(x => x.Account == request.CurrentUser),
                                    x => x.Role,
                                    y => y.Role,
                                    (role, accountRole) => new { role, accountRole })
                                    .Select(x => x.role.Role).ToListAsync();
            if (!roleAccepts.Any())
                return new OperationResult(false, $"Your account is not rule to upload for Factory: {request.Factory}");

            #endregion

            // kiểm tra thời gian cấu hình
            var now = DateTime.Now;

            // danh sách nhân viên của nhà máy
            var employees = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(x => x.Factory == request.Factory || x.Assigned_Factory == request.Factory).Select(x => new { x.Employee_ID, x.Assigned_Employee_ID }).ToListAsync();


            // // Lấy nội dung theo Swipe Card Set theo nhà máy
            var swipeCardSets = await _repositoryAccessor.HRMS_Att_Swipecard_Set.FirstOrDefaultAsync(x => x.Factory == request.Factory);



            // // Nhà máy chưa được cấu hình
            if (swipeCardSets == null)
                return new OperationResult(false, $"Factory {request.Factory} none setting yet");

            // Kiểm tra độ dài chuỗi 
            List<int> lengths = new(){
                swipeCardSets.Employee_Start,
                swipeCardSets.Employee_End,
                swipeCardSets.Date_Start,
                swipeCardSets.Date_End,
                swipeCardSets.Time_Start,
                swipeCardSets.Time_End
            };

            int maxLength = lengths.Max();
            int minLenth = lengths.Min();
            string fileRawName = request.FileUpload.FileName.GetFileName();
            string fileName = fileRawName.Split(".")[0];
            var path = $"{_webHostEnvironment.WebRootPath}\\uploaded\\AttendanceMaintenance\\5_1_13_SwipeCardDataUpload\\{fileRawName}";
            await FilesUtility.SaveFile(request.FileUpload, "uploaded\\AttendanceMaintenance\\5_1_13_SwipeCardDataUpload", fileName);

            // Danh sách nhân viên bấm thẻ hợp lệ
            var employeeIdSwipCards = new List<HRMS_Att_Swipe_Card>();

            // Danh sách báo cáo
            var reports = new List<HRMS_Att_Swipe_Card_Report>();

            // Số lượng dữ liệu được xử lý
            int processRecords = 0;

            // danh sách bấm thẻ trong DB hiện tại
            var swipeCardOnDB = await _repositoryAccessor.HRMS_Att_Swipe_Card.FindAll(x => x.Factory == request.Factory, true).ToListAsync();

            #region 2. Lấy thông tin mã nhân viên từ file master tải lên
            try
            {
                // lấy dữ liệu file 
                var contentFileTxt = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(contentFileTxt))
                    return new OperationResult(false, $"file is empty");
                var dataSwipeCards = MyRegex().Replace(contentFileTxt.Replace("\r", " ").Replace("\n", " "), " ").Split(" ").Distinct().ToList();
                foreach (var data in dataSwipeCards)
                {
                    if (!string.IsNullOrWhiteSpace(data))
                    {
                        // Kiểm tra độ dài phù hợp
                        if (data.Length < maxLength)
                            reports.Add(CreateModelReport(data, $"Length of format invalid"));

                        // lấy data theo SwipeCardSet
                        var employeeId = data[(swipeCardSets.Employee_Start - 1)..swipeCardSets.Employee_End];
                        var timeSwipe = data[(swipeCardSets.Time_Start - 1)..swipeCardSets.Time_End];
                        var daySwipe = data[(swipeCardSets.Date_Start - 1)..swipeCardSets.Date_End];

                        // 00051=106290306
                        string employeeIdFormat = $"{request.Factory.Trim()}-{employeeId}";

                        // Kiểm tra nhân viên có thuộc nhà máy hay không ?
                        if (employees.Any(x => x.Employee_ID == employeeIdFormat || x.Assigned_Employee_ID == employeeIdFormat))
                        {
                            if (!IsTimeValid(timeSwipe))
                                reports.Add(CreateModelReport(employeeId, $"Invalid time: {timeSwipe}"));
                            else if (!IsDayValid(daySwipe, now.Year))
                                reports.Add(CreateModelReport(employeeId, $"Invalid day: {daySwipe}"));
                            else
                            {
                                if (employeeIdSwipCards.Any(x => x.Employee_ID == employeeIdFormat && x.Card_Time == timeSwipe && x.Card_Date == daySwipe))
                                    reports.Add(CreateModelReport(employeeId, $"Duplicate entry for employee {employeeId} at time {timeSwipe} and day {daySwipe}"));
                                else
                                    employeeIdSwipCards.Add(CreateModel(request.Factory, employeeId, timeSwipe, daySwipe, now, request.CurrentUser));
                            }
                        }
                        else reports.Add(CreateModelReport(employeeId, $"Employee {employeeId} none existed"));
                    }
                }
            }
            catch
            {
                FilesUtility.DeleteFile(path);
                return new OperationResult(false);
            }
            #region 3. Toàn bộ gói đã được kiểm tra và có dấu lỗi N. Toàn bộ dữ liệu gói không được lưu trữ và báo cáo lỗi được tạo và thông báo lỗi được hiển thị
            if (reports.Any())
            {
                // xử lí report data 
                var dataTables = new List<Table>() { new("result", reports) };

                // Thông tin print [Factory, PrintBy,  PrintDay]
                var dataCells = new List<Cell>(){
                    new("B3", request.Factory),
                    new("E3", request.CurrentUser),
                    new("G3", now.ToString("yyyy/MM/dd HH:mm:ss"))
                };

                ConfigDownload config = new() { IsAutoFitColumn = true };
                ExcelResult excelResult = ExcelUtility.DownloadExcel(
                    dataTables, dataCells, 
                    "Resources\\Template\\AttendanceMaintenance\\5_1_13_SwipeCardDataUpload\\Report.xlsx", 
                    config
                );
                return new OperationResult(false, excelResult.Error, excelResult.Result);
            }
            #endregion

            processRecords = employeeIdSwipCards.Count;

            // Nếu tất cả hợp lệ, Xoá hết dữ liệu trong bảng [HRMS_Att_Swipe_Card] & lưu lại dữ liệu đã kiểm tra
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                // Xoá hết dữ liệu trong bảng HRMS_Att_Swipe_Card
                _repositoryAccessor.HRMS_Att_Swipe_Card.RemoveMultiple(swipeCardOnDB);
                await _repositoryAccessor.Save();

                // lưu data mới
                if (employeeIdSwipCards.Any())
                {
                    _repositoryAccessor.HRMS_Att_Swipe_Card.AddMultiple(employeeIdSwipCards);
                    await _repositoryAccessor.Save();
                }
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true, processRecords);
            }
            catch (Exception ex)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false, ex.Message);
            }
            #endregion
        }

        private static bool IsTimeValid(string time)
        {
            if (string.IsNullOrWhiteSpace(time) || time.Length != 4 || !time.All(char.IsNumber))
                return false;
            var hour = int.Parse(time[..2]);
            var minute = int.Parse(time[2..]);
            if (hour < 0 || hour >= 24 || minute < 0 || minute >= 60)
                return false;
            return true;
        }

        private static bool IsDayValid(string daySwipe, int year)
        {
            if (string.IsNullOrWhiteSpace(daySwipe) || daySwipe.Length != 4)
                return false;

            var month = Convert.ToInt16(daySwipe[..2]); // tháng
            var day = Convert.ToInt16(daySwipe[2..]);  // ngày

            // số ngày tối đa trong tháng
            var dayInMonth = DateTime.DaysInMonth(year, month);

            if (month > 12 || month < 1 || day < 1 || day > dayInMonth)
                return false;
            return true;
        }

        private static HRMS_Att_Swipe_Card CreateModel(string factory, string employeeId, string timeSwipe, string daySwipe, DateTime time, string user) => new()
        {
            Mark = "N", //Default = N
            Factory = factory,
            Employee_ID = $"{factory}-{employeeId}",
            Work_Shift_Type = "1", //Default = 1
            Card_Time = timeSwipe,
            Card_Date = daySwipe,
            Update_Time = time,
            Update_By = user
        };

        private static HRMS_Att_Swipe_Card_Report CreateModelReport(string employeeId, string errorMessage) => new(employeeId, errorMessage);
    }
}