using System.Data.SqlTypes;
using API._Repositories;
using API._Services.Interfaces.SeaHr;
using API.Dtos.SeaHr;
using API.Dtos.SeaHr.SeaHrHistory;
using API.Helpers.Enums;
using API.Helpers.Utilities;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
namespace API._Services.Services.SeaHr
{
    public class SeaHrHistoryService : ISeaHrHistoryService
    {
        private readonly IRepositoryAccessor _accessorRepo;
        private readonly IFunctionUtility _functionUtility;

        public SeaHrHistoryService(IRepositoryAccessor accessorRepo, IFunctionUtility functionUtility)
        {
            _accessorRepo = accessorRepo;
            _functionUtility = functionUtility;
        }

        public async Task<List<KeyValuePair<int, string>>> GetCategory(string lang)
        {
            if (lang == "zh")
            {
                lang = LangConstants.ZH_TW;
            }

            List<KeyValuePair<int, string>> data = await _accessorRepo.Category.FindAll(x => x.Visible == true)
            .Include(x => x.CatLangs)
            .Select(x => new KeyValuePair<int, string>(
                x.CateID, $"{x.CateSym} - {x.CatLangs.FirstOrDefault(z => z.CateID == x.CateID && z.LanguageID == lang).CateName}"
            )).ToListAsync();

            return data;
        }

        public async Task<List<KeyValuePair<int, string>>> GetDepartments()
        {
            return await _accessorRepo.Department.FindAll()
                .Select(x => new KeyValuePair<int, string>(x.DeptID, $"{x.DeptCode} - {x.DeptName}")).ToListAsync();
        }

        public async Task<SeaHistorySearchDto> GetLeaveData(SearchHistoryParamsDto paramsSearch, PaginationParam pagination, bool isPaging = true)
        {
            SeaHistorySearchDto res = new();
            var predCheckLeaveData = PredicateBuilder.New<LeaveData>(true);
            DateTime startDateTime = paramsSearch.StartTime != null ? Convert.ToDateTime(paramsSearch.StartTime + " 00:00:00") : SqlDateTime.MinValue.Value; ;
            DateTime endDateTime = paramsSearch.EndTime != null ? Convert.ToDateTime(paramsSearch.EndTime + " 23:59:59") : SqlDateTime.MaxValue.Value;
            if (startDateTime < endDateTime)
                predCheckLeaveData.And(x => (x.Time_Start >= startDateTime && x.Time_End <= endDateTime)
                    || (x.Time_Start <= startDateTime && x.Time_Start <= endDateTime && x.Time_End >= startDateTime && x.Time_End <= endDateTime)
                    || (x.Time_End >= startDateTime && x.Time_End >= endDateTime && x.Time_Start >= startDateTime && x.Time_Start <= endDateTime));
            if (paramsSearch.CategoryId != 0)
                predCheckLeaveData.And(x => x.CateID == paramsSearch.CategoryId);
            //check Status
            if (paramsSearch.Status == 0)
                predCheckLeaveData.And(x => x.Status_Line == true);
            else if (paramsSearch.Status != 0 && paramsSearch.Status != 5 && paramsSearch.Status != 6)
                predCheckLeaveData.And(x => x.Approved == paramsSearch.Status && x.Status_Line == true);
            else if (paramsSearch.Status == 5)
                predCheckLeaveData.And(x => x.Approved == 1 && x.Status_Line == true && x.Status_Lock == true);
            else if (paramsSearch.Status == 6)
                predCheckLeaveData.And(x => x.Status_Line == false);
            else
                predCheckLeaveData.And(x => x.Status_Line == true && x.Approved == paramsSearch.Status);

            var users = await _accessorRepo.Users
                    .FindAll()
                    .GroupJoin(_accessorRepo.Employee.FindAll(),
                        x => x.EmpID,
                        y => y.EmpID,
                        (x, y) => new { Users = x, Employees = y })
                    .SelectMany(
                        x => x.Employees.DefaultIfEmpty(),
                        (x, y) => new { x.Users, Employee = y })
                    .Select(x => new { x.Employee, x.Users})
                    .ToListAsync();
            // return data.EmpName ?? data.UserName;

            var query = await _accessorRepo.LeaveData.FindAll(predCheckLeaveData)
            .Include(x => x.Cate)
                .ThenInclude(x => x.CatLangs)
            .Include(x => x.Emp)
                .ThenInclude(x => x.Part)
                    .ThenInclude(x => x.Dept)
                        .ThenInclude(x => x.DetpLangs)
            .Select(x => new LeaveDataDto
            {
                LeaveID = x.LeaveID,
                EmpID = x.EmpID,
                EmpNumber = x.Emp.EmpNumber,
                EmpName = x.Emp.EmpName,
                DeptID = x.Emp.Part.Dept.DeptID,
                DeptName = x.Emp.Part.Dept.DeptName,
                PartID = x.Emp.Part.PartID,
                Category = x.Cate.CateSym + " - " + x.Cate.CateName,
                CateID = x.CateID,
                CateSym = x.Cate.CateSym,
                DateStartOrder = x.Created.Value,
                TimeStart = x.Time_Start.Value.ToString("HH:mm"),
                TimeEnd = x.Time_End.Value.ToString("HH:mm"),
                DateStart = x.Time_Start.Value.ToString("dd/MM/yyyy"),
                DateEnd = x.Time_End.Value.ToString("dd/MM/yyyy"),
                LeaveDayByString = ConvertLeaveDay(x.LeaveDay.ToString()),
                Status = GetStatus(x.Approved, paramsSearch.Lang),
                Status_Lock = x.Status_Lock,
                Approved = x.Approved,
                ApprovedBy = x.ApprovedBy,
                LeaveArchive = x.LeaveArchive,
                Time_Start = x.Time_Start,
                Time_End = x.Time_End,
                //excel
                DateStartExcel = x.Time_Start.Value.ToString("dd/MM/yyyy"),
                DateEndExcel = x.Time_End.Value.ToString("dd/MM/yyyy"),
                PartCode = x.Emp.Part.Dept.DeptCode,
                DeptIDName = x.Emp.Part.Dept.DeptName,
                CreatedString = x.Created.Value.ToString("HH:mm dd/MM/yyyy"),
                LeaveDayString = Math.Round((double)x.LeaveDay, 5, MidpointRounding.AwayFromZero).ToString().Replace(",", "."),
                SearchDate = (paramsSearch.StartTime != null ? Convert.ToDateTime(paramsSearch.StartTime).ToString("dd/MM/yyyy") : "") + " - " + (paramsSearch.EndTime != null ? Convert.ToDateTime(paramsSearch.EndTime).ToString("dd/MM/yyyy") : ""),
                UpdatedString = x.Updated.Value.ToString("HH:mm dd/MM/yyyy"),
                CommentArchive = x.CommentArchive,
                StatusExcel = GetStatus(x.Approved, paramsSearch.Lang),
                LeaveDayByRangerDateString = Math.Round((double)x.LeaveDay, 5, MidpointRounding.AwayFromZero).ToString().Replace(",", "."),
                DeptNameLangVN = x.Emp.Part.Dept.DetpLangs.FirstOrDefault(y => y.DeptID.Value == x.Emp.Part.Dept.DeptID && y.LanguageID == LangConstants.VN).DeptName,
                DeptNameLangEN = x.Emp.Part.Dept.DetpLangs.FirstOrDefault(y => y.DeptID.Value == x.Emp.Part.Dept.DeptID && y.LanguageID == LangConstants.EN).DeptName,
                DeptNameLangZH = x.Emp.Part.Dept.DetpLangs.FirstOrDefault(y => y.DeptID.Value == x.Emp.Part.Dept.DeptID && y.LanguageID == LangConstants.ZH_TW).DeptName,
                CategoryLangVN = x.Cate.CatLangs.FirstOrDefault(y => y.CateID.Value == x.Cate.CateID && y.LanguageID == LangConstants.VN).CateName,
                CategoryLangEN = x.Cate.CatLangs.FirstOrDefault(y => y.CateID.Value == x.Cate.CateID && y.LanguageID == LangConstants.EN).CateName,
                CategoryLangZH = x.Cate.CatLangs.FirstOrDefault(y => y.CateID.Value == x.Cate.CateID && y.LanguageID == LangConstants.ZH_TW).CateName,
                Time_Applied = x.Time_Applied.Value.ToString("dd/MM/yyyy"),
            }).ToListAsync();

            if (!string.IsNullOrEmpty(paramsSearch.EmpId))
                query = query.Where(x => x.EmpNumber.ToUpper() == paramsSearch.EmpId.ToUpper()).ToList();
            if (paramsSearch.DepartmentId != 0)
                query = query.Where(x => x.DeptID == paramsSearch.DepartmentId).ToList();
            if (paramsSearch.PartList != null)
                query = query.Where(x => x.PartID.HasValue && paramsSearch.PartList.Contains(x.PartID.Value)).ToList();
            List<LeaveDataDto> data = query.OrderByDescending(x => x.DateStartOrder).ToList();

            PaginationUtility<LeaveDataDto> result = PaginationUtility<LeaveDataDto>.Create(data, pagination.PageNumber, pagination.PageSize, isPaging);
            //check Leave Day By Range Date
            foreach (var item in result.Result)
            {
                DateTime d1 = Convert.ToDateTime(paramsSearch.StartTime + " 00:00");
                DateTime d2 = Convert.ToDateTime(paramsSearch.EndTime + " 23:59");
                string[] leaveArchiveParts = item.LeaveArchive.Split('-');
                if (leaveArchiveParts.Length >= 3)
                {
                    string shift = leaveArchiveParts[2];
                    DateTime tmpTimeStart = Convert.ToDateTime(item.Time_Start);
                    DateTime tmpTimeEnd = Convert.ToDateTime(item.Time_End);
                    LunchBreak shiftWork = new();
                    bool check = false;
                    if (shift == "C")
                    {
                        d1 = Convert.ToDateTime(paramsSearch.StartTime + " 22:00");
                        d2 = Convert.ToDateTime(paramsSearch.EndTime + " 06:00").AddDays(1);

                        shiftWork = await _accessorRepo.LunchBreak.FirstOrDefaultAsync(x => x.Key.Trim() == "ZZZ");
                    }
                    else shiftWork = await _accessorRepo.LunchBreak.FirstOrDefaultAsync(x => x.Key.Trim() == shift.Trim());

                    if (item.Time_Start <= d1)
                    {
                        tmpTimeStart = new DateTime(d1.Year, d1.Month, d1.Day).Date.Add(shiftWork.WorkTimeStart);
                        check = true;
                    }
                    if (item.Time_End >= d2)
                    {
                        if (shift == "C")
                        {
                            d2 = d2.AddDays(-1);
                        }
                        tmpTimeEnd = new DateTime(d2.Year, d2.Month, d2.Day).Date.Add(shiftWork.WorkTimeEnd);
                        check = true;
                    }
                    if (check == true)
                    {
                        List<dynamic> rangerDate = RangerDate(shiftWork, tmpTimeStart, tmpTimeEnd);
                        double LeaveDayByRangerDate = (double)SumTotalLeave(shift, rangerDate) / 8;
                        item.LeaveDayByRangerDateString = Math.Round((double)LeaveDayByRangerDate, 5, MidpointRounding.AwayFromZero).ToString().Replace(",", ".");
                    }
                }
                
                if(item.ApprovedBy != null && item.ApprovedBy > 0)
                    item.Sender = users.FirstOrDefault(y => y.Users.UserID == item.ApprovedBy)?.Employee?.EmpName
                        ?? users.FirstOrDefault(y => y.Users.UserID == item.ApprovedBy)?.Users?.UserName;
                else
                    item.Sender = "Đang chờ duyệt";
            }
            var categories = await GetCategories();

            categories.ForEach(item => item.Optional = data.Where(x => x.CateID == item.Key).Count());

            // categories.Insert(0, new KeyValueUtility
            // {
            //     Key = 0,
            //     Value_en = "All types of leave",
            //     Value_vi = "Tất cả loại phép",
            //     Value_zh = "所有类型的休假",
            //     Optional = data.Count
            // });

            res.LeaveData = result;
            res.CountEachCategory = categories;

            return res;
        }

        public async Task<List<KeyValueUtility>> GetCategories()
        {
            List<Category> data = await _accessorRepo.Category.FindAll(x => x.Visible.Value).ToListAsync();
            List<CatLang> dataLangs = await _accessorRepo.CatLang.FindAll(x => data.Select(y => y.CateID).Contains(x.CateID.Value)).ToListAsync();
            List<KeyValueUtility> result = data.Select(x => new KeyValueUtility
            {
                Key = x.CateID,
                Value_en = x.CateSym + " - " + dataLangs.FirstOrDefault(y => y.LanguageID == LangConstants.EN && y.CateID == x.CateID)?.CateName.Trim(),
                Value_vi = x.CateSym + " - " + dataLangs.FirstOrDefault(y => y.LanguageID == LangConstants.VN && y.CateID == x.CateID)?.CateName.Trim(),
                Value_zh = x.CateSym + " - " + dataLangs.FirstOrDefault(y => y.LanguageID == LangConstants.ZH_TW && y.CateID == x.CateID)?.CateName.Trim(),
            }).ToList();

            return result;
        }
        public dynamic SumTotalLeave(string shift, List<dynamic> dateRanger)
        {
            dynamic totalLeave = 0;
            dynamic total;
            foreach (var item in dateRanger)
            {
                // Ca C : giờ vào làm - 24 giờ + giờ kết thúc , để tính số giờ làm việc trong ca đêm
                if (shift == "C")
                    total = (new TimeSpan(24, 0, 0) - item.TimeStartDay + item.TimeEndDay).TotalHours;
                else
                    total = (item.TimeEndDay - item.TimeStartDay).TotalHours;

                if (shift == "A" || shift == "B" || shift == "C" || shift == "C5")
                    totalLeave += total;
                else
                    totalLeave = totalLeave + total - item.TimeLunch;
            }
            return totalLeave;
        }

        public List<dynamic> RangerDate(LunchBreak shiftWork, DateTime timeLeaveStart, DateTime timeLeaveEnd)
        {
            List<dynamic> dateRanger = new();

            while (timeLeaveStart <= timeLeaveEnd)
            {
                if (timeLeaveStart.DayOfWeek != 0)
                {
                    var dateObj = new
                    {
                        TimeStartDay = shiftWork.WorkTimeStart,
                        TimeEndDay = shiftWork.WorkTimeEnd,
                        TimeLunch = 0
                    };
                    dateRanger.Add(dateObj);
                }
                timeLeaveStart = timeLeaveStart.AddDays(1);
            }
            List<dynamic> dateRanger2 = new();
            if (dateRanger.Count == 1)
            {
                var dateObj = new
                {
                    TimeStartDay = new TimeSpan(timeLeaveStart.Hour, timeLeaveStart.Minute, 0),
                    TimeEndDay = new TimeSpan(timeLeaveEnd.Hour, timeLeaveEnd.Minute, 0),
                    TimeLunch = TotalLunchByDay(shiftWork, dateRanger[0].TimeStartDay, dateRanger[0].TimeEndDay)
                };
                dateRanger2.Add(dateObj);
            }
            else
            {
                for (int i = 0; i < dateRanger.Count; i++)
                {
                    if (i == 0)
                    {
                        var dateObj = new
                        {
                            TimeStartDay = new TimeSpan(timeLeaveStart.Hour, timeLeaveStart.Minute, 0),
                            dateRanger[i].TimeEndDay,
                            TimeLunch = TotalLunchByDay(shiftWork, dateRanger[i].TimeStartDay, dateRanger[i].TimeEndDay),
                        };
                        dateRanger2.Add(dateObj);
                    }
                    else if (dateRanger.Count == i + 1)
                    {
                        if (timeLeaveEnd.TimeOfDay == dateRanger[i].TimeEndDay)
                        {
                            var dateObj = new
                            {
                                dateRanger[i].TimeStartDay,
                                dateRanger[i].TimeEndDay,
                                TimeLunch = TotalLunchByDay(shiftWork, dateRanger[i].TimeStartDay, dateRanger[i].TimeEndDay),
                            };
                            dateRanger2.Add(dateObj);
                        }
                        else
                        {
                            var dateObj = new
                            {
                                TimeStartDay = new TimeSpan(timeLeaveStart.Hour, timeLeaveStart.Minute, 0),
                                dateRanger[i].TimeEndDay,
                                TimeLunch = TotalLunchByDay(shiftWork, dateRanger[i].TimeStartDay, dateRanger[i].TimeEndDay),
                            };
                            dateRanger2.Add(dateObj);
                        }

                    }
                    else
                    {
                        var dateObj = new
                        {
                            dateRanger[i].TimeStartDay,
                            dateRanger[i].TimeEndDay,
                            TimeLunch = TotalLunchByDay(shiftWork, dateRanger[i].TimeStartDay, dateRanger[i].TimeEndDay),
                        };
                        dateRanger2.Add(dateObj);
                    }
                }
            }

            return dateRanger2;
        }

        public int TotalLunchByDay(LunchBreak shiftWork, TimeSpan timeStart, TimeSpan timeEnd)
        {
            int lunchTime;

            TimeSpan lunchTimeStart = shiftWork.LunchTimeStart;
            TimeSpan lunchTimeEnd = shiftWork.LunchTimeEnd;

            if (timeStart <= lunchTimeStart && timeEnd <= lunchTimeStart)
            {
                lunchTime = 0;
            }

            else if (timeStart >= lunchTimeEnd && timeEnd >= lunchTimeEnd)
            {
                lunchTime = 0;
            }

            else if (timeStart >= lunchTimeStart && timeEnd <= lunchTimeEnd)
            {
                lunchTime = (int)(timeEnd - timeStart).TotalHours;
            }

            else if (timeStart <= lunchTimeStart && timeEnd >= lunchTimeEnd)
            {
                lunchTime = (int)(lunchTimeEnd - lunchTimeStart).TotalHours;
            }

            else if (timeStart <= lunchTimeStart && timeEnd >= lunchTimeStart && timeEnd <= lunchTimeEnd)
            {
                lunchTime = (int)(timeEnd - lunchTimeStart).TotalHours;
            }

            else if (timeStart >= lunchTimeStart && timeStart <= lunchTimeEnd && timeEnd >= lunchTimeEnd)
            {
                lunchTime = (int)(lunchTimeEnd - timeStart).TotalHours;
            }
            else
            {
                lunchTime = 0;
            }
            return lunchTime;
        }

        public async Task<List<KeyValuePair<int, string>>> GetPart(int ID)
        {
            return await _accessorRepo.Part.FindAll(x => x.DeptID == ID)
                .Select(x => new KeyValuePair<int, string>(x.PartID, $"{x.PartName}")).ToListAsync();
        }

        public async Task<OperationResult> ExportExcel(PaginationParam pagination, SearchHistoryParamsDto dto)
        {
            var data = await GetLeaveData(dto, pagination, false);

            List<Table> dataTable = new()
            {
                new Table("result", data.LeaveData.Result)
            };

            ExcelResult excelResult = ExcelUtility.DownloadExcel(dataTable, "Resources\\Template\\SeaHr\\SeaHrHistory.xlsx");
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }

        private static string GetStatus(int? approved, string lang)
        {
            return approved switch
            {
                1 => lang == "vi" ? "Chờ Duyệt" : lang == "en" ? "Pending" : "待辦的",
                2 => lang == "vi" ? "Đã Duyệt" : lang == "en" ? "Approve" : "得到正式認可的",
                3 => lang == "vi" ? "Từ Chối" : lang == "en" ? "Refuse" : "拒絕",
                _ => lang == "vi" ? "Hoàn Thành" : lang == "en" ? "Complete" : "完全的",
            };
        }

        private static string ConvertLeaveDay(string day)
        {
            try
            {
                return Math.Round(decimal.Parse(day), 5) + "d" + " - " + Math.Round(decimal.Parse((Convert.ToDouble(day) * 8).ToString()), 5) + "h";
            }
            catch
            {
                return "0";
            }
        }
    }
}