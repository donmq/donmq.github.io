using System.Globalization;
using API._Repositories;
using API._Services.Interfaces.SeaHr;
using API.Dtos.SeaHr;
using API.Dtos.SeaHr.ViewConfirmDaily;
using API.Helpers.Enums;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
namespace API._Services.Services.SeaHr
{
    public class ViewConfirmDailyService : IViewConfirmDailyService
    {
        private readonly IRepositoryAccessor _repoAccessor;
        public ViewConfirmDailyService(IRepositoryAccessor repoAccessor)
        {
            _repoAccessor = repoAccessor;
        }

        public async Task<PaginationUtility<ViewConfirmDailyDTO>> GetViewConfirmDaily(string lang, string startTime, string endTime, PaginationParam pagination, bool isPaging)
        {
            var predViewConfirmDaily = PredicateBuilder.New<LeaveData>(true);

            DateTime startDateTime = Convert.ToDateTime(startTime + " 00:00:00");
            DateTime endDateTime = Convert.ToDateTime(endTime + " 23:59:59");

            if (startDateTime < endDateTime)
            {
                predViewConfirmDaily.And(x => x.Status_Line == true && x.Approved == 4 &&
                                   ((x.Time_Start <= startDateTime && x.Time_End > startDateTime) ||
                                   (x.Time_Start < endDateTime && x.Time_End >= endDateTime) ||
                                   (x.Time_Start >= startDateTime && x.Time_End <= endDateTime) ||
                                   (x.Time_Start <= startDateTime && x.Time_End >= endDateTime)));

            }


            CultureInfo cultureInfo = lang switch
            {
                "vi" => new CultureInfo("vi-VN"),
                "zh-TW" => new CultureInfo("zh-TW"),
                _ => new CultureInfo("en-US"),
            };

            List<ViewConfirmDailyDTO> data = await _repoAccessor.LeaveData.FindAll(predViewConfirmDaily, true)
            .Include(x => x.Cate)
            .Include(x => x.Emp)
                .ThenInclude(x => x.Part)
                    .ThenInclude(x => x.Dept)
            .Select(x => new ViewConfirmDailyDTO
            {
                LeaveArchive = x.LeaveArchive,
                DeptID = x.Emp.Part.Dept.DeptID,
                DeptCode = x.Emp.Part.Dept.DeptCode,
                DeptName = x.Emp.Part.Dept.DeptName,
                PartID = x.Emp.Part.PartID,
                EmpID = x.Emp.EmpID,
                EmpName = x.Emp.EmpName,
                EmpNumber = x.Emp.EmpNumber,
                CateID = x.Cate.CateID,
                CateSym = x.Cate.CateSym,
                Time_Start = x.Time_Start,
                Time_End = x.Time_End,
                Approved = x.Approved,
                LeaveID = x.LeaveID,
                TimeStart = x.Time_Start.Value.ToString("HH:mm"),
                TimeEnd = x.Time_End.Value.ToString("HH:mm"),
                DateStart = x.Time_Start.Value.ToString("dd/MM/yyyy"),
                DateEnd = x.Time_End.Value.ToString("dd/MM/yyyy"),
                LeaveDay = x.LeaveDay.ToString(),
                Status = x.Approved == 1 ? "Chờ duyệt" : x.Approved == 2 ? "Đã duyệt" : x.Approved == 3 ? "Từ chối" : "Hoàn thành",
                Update = x.Updated.Value.ToString(lang == "zh-TW" ? "dd/MM/yyyy tt HH:mm:ss" : "dd/MM/yyyy HH:mm:ss tt", cultureInfo),
                CateNameVN = x.Cate.CatLangs.FirstOrDefault(y => y.CateID.Value == x.Cate.CateID && y.LanguageID == LangConstants.VN).CateName,
                CateNameEN = x.Cate.CatLangs.FirstOrDefault(y => y.CateID.Value == x.Cate.CateID && y.LanguageID == LangConstants.EN).CateName,
                CateNameZH = x.Cate.CatLangs.FirstOrDefault(y => y.CateID.Value == x.Cate.CateID && y.LanguageID == LangConstants.ZH_TW).CateName,
                PartNameVN = x.Emp.Part.PartLangs.FirstOrDefault(y => y.PartID.Value == x.Emp.Part.PartID && y.LanguageID == LangConstants.VN).PartName,
                PartNameEN = x.Emp.Part.PartLangs.FirstOrDefault(y => y.PartID.Value == x.Emp.Part.PartID && y.LanguageID == LangConstants.EN).PartName,
                PartNameZH = x.Emp.Part.PartLangs.FirstOrDefault(y => y.PartID.Value == x.Emp.Part.PartID && y.LanguageID == LangConstants.ZH_TW).PartName,
                DeptNameVN = x.Emp.Part.Dept.DetpLangs.FirstOrDefault(y => y.DeptID.Value == x.Emp.Part.Dept.DeptID && y.LanguageID == LangConstants.VN).DeptName,
                DeptNameEN = x.Emp.Part.Dept.DetpLangs.FirstOrDefault(y => y.DeptID.Value == x.Emp.Part.Dept.DeptID && y.LanguageID == LangConstants.EN).DeptName,
                DeptNameZH = x.Emp.Part.Dept.DetpLangs.FirstOrDefault(y => y.DeptID.Value == x.Emp.Part.Dept.DeptID && y.LanguageID == LangConstants.ZH_TW).DeptName,
                UpdateTime = x.Updated,
            }).OrderByDescending(x => x.UpdateTime).ToListAsync();

            var result = PaginationUtility<ViewConfirmDailyDTO>.Create(data, pagination.PageNumber, pagination.PageSize, isPaging);

            return result;
        }

        public async Task<List<ViewDataModel>> GetDataViewData(IEnumerable<ViewConfirmDailyDTO> model, List<DateTime> allDates, DateTime d1, DateTime d2)
        {
            List<ViewDataModel> iniParent = new();
            List<LunchBreak> lunchs = await _repoAccessor.LunchBreak.FindAll(x => x.Visible == true, true).ToListAsync();

            foreach (ViewConfirmDailyDTO q in model)
            {
                string[] shift = q.LeaveArchive.Split('-');
                int h1 = 7;
                int m1 = 30;
                int m2 = 30;
                int h2 = 16;
                string h1m1 = "07:30";
                string h2m2 = "16:30";
                string lunch = "13,00";
                string shift2 = "";
                LunchBreak lunchBreak = new();

                if (shift.Length > 2)
                {
                    shift2 = shift[2];
                    //vao ham lay data
                    lunchBreak = lunchs.FirstOrDefault(x => x.Key == shift2);
                    h1 = lunchBreak.WorkTimeStart.Hours; ;
                    m1 = lunchBreak.WorkTimeStart.Minutes;
                    h2 = lunchBreak.WorkTimeEnd.Hours;
                    m2 = lunchBreak.WorkTimeEnd.Minutes;
                    h1m1 = $"{h1}:{m1.ToString().PadLeft(2, '0')}";
                    h2m2 = $"{h2}:{m2.ToString().PadLeft(2, '0')}";
                }
                for (int i = 0; i < allDates.Count; i++)
                {
                    var d11 = allDates[i].AddHours(h1).AddMinutes(m1);
                    var d22 = allDates[i].AddHours(h2).AddMinutes(m2);
                    d1 = allDates[i].AddTicks(lunchBreak.WorkTimeStart.Ticks);
                    d2 = allDates[i].AddTicks(lunchBreak.WorkTimeEnd.Ticks);
                    if (d2 < q.Time_Start && d2 < q.Time_End)
                    {
                        continue;
                    }
                    if (d1 > q.Time_Start && d1 > q.Time_End)
                    {
                        continue;
                    }

                    ViewDataModel child = new();

                    Employee employeeFind = await _repoAccessor.Employee.FirstOrDefaultAsync(x => x.EmpID == q.EmpID);
                    child.EmpNumber = employeeFind?.EmpNumber?.ToString();
                    Category categoryFind = await _repoAccessor.Category.FirstOrDefaultAsync(x => x.CateID == q.CateID);

                    child.CateSym = categoryFind?.CateSym;
                    child.DateStart = d1.ToString("dd/MM/yyyy");
                    child.DateEnd = d2.ToString("dd/MM/yyyy");
                    child.DeptName = q.DeptName;
                    child.EmpName = q.EmpName;
                    child.Status = q.Status;
                    child.DeptCode = q.DeptCode;
                    child.Update = q.Update;
                    child.CateNameEN = q.CateNameEN;
                    child.CateNameVN = q.CateNameVN;
                    child.CateNameZH = q.CateNameZH;

                    DateTime ds;
                    DateTime de;

                    if (q.Time_End >= d2)
                    {
                        // d1 start d2 end
                        if (q.Time_Start > d1)
                        {
                            // lấy giờ  timeStart => ds
                            child.TimeStart = Convert.ToDateTime(q.Time_Start).ToString("HH:mm");
                            ds = Convert.ToDateTime(q.Time_Start);
                        }
                        //  start d1 d2 end
                        else
                        {
                            //lấy giờ d1 => ds
                            child.TimeStart = h1m1;
                            ds = allDates[i].AddHours(h1).AddMinutes(m1);
                        }

                        child.TimeEnd = h2m2;

                        if ((ds.Hour + ds.Minute / 60) >= float.Parse(lunch)) //13
                        {
                            de = allDates[i].AddHours(h2).AddMinutes(m2);
                        }
                        else
                        {
                            if (shift2 == "C")
                            {
                                de = allDates[i].AddHours(h2 - 18).AddMinutes(m2); //-30
                            }
                            else if (shift2 == "A" || shift2 == "B")
                            {
                                de = allDates[i].AddHours(h2).AddMinutes(m2); //-30
                            }
                            else
                                // ds.Hour <=12h tinh du 8 tieng de chia 8
                                de = allDates[i].AddHours(h2 - 1).AddMinutes(m2);
                        }
                    }
                    else
                    {
                        //d1 start end d2
                        if (q.Time_Start > d1)
                        {
                            // start => ds
                            child.TimeStart = Convert.ToDateTime(q.Time_Start).ToString("HH:mm");
                            ds = Convert.ToDateTime(q.Time_Start);
                        }
                        //start d1 end d2
                        else
                        {
                            //d1 => ds
                            child.TimeStart = h1m1;
                            ds = allDates[i].AddHours(h1).AddMinutes(m1);
                        }

                        child.TimeEnd = Convert.ToDateTime(q.Time_End).ToString("HH:mm");

                        // >= 13 => -1 h => trừ ra nghỉ trưa
                        if ((Convert.ToDateTime(q.Time_End).Hour + Convert.ToDateTime(q.Time_End).Minute / 60) >= float.Parse(lunch))
                        {
                            if (shift2 == "C")
                            {
                                string hh = Convert.ToDateTime(q.Time_End).ToString("HH");
                                string mm = Convert.ToDateTime(q.Time_End).ToString("mm");
                                de = allDates[i].AddHours(long.Parse(hh) - 18).AddMinutes(long.Parse(mm)); //-30
                            }
                            else if (shift2 == "A" || shift2 == "B")
                            {
                                string hh = Convert.ToDateTime(q.Time_End).ToString("HH");
                                string mm = Convert.ToDateTime(q.Time_End).ToString("mm");
                                de = allDates[i].AddHours(long.Parse(hh)).AddMinutes(long.Parse(mm)); //-30
                            }
                            else
                            {
                                string hh = Convert.ToDateTime(q.Time_End).ToString("HH");
                                string mm = Convert.ToDateTime(q.Time_End).ToString("mm");
                                de = allDates[i].AddHours(long.Parse(hh) - 1).AddMinutes(long.Parse(mm));
                            }
                        }
                        else
                        {
                            if (shift2 == "C")
                            {
                                string hh = Convert.ToDateTime(q.Time_End).ToString("HH");
                                string mm = Convert.ToDateTime(q.Time_End).ToString("mm");
                                de = allDates[i].AddHours(long.Parse(hh)).AddMinutes(long.Parse(mm)); //-30
                            }
                            else
                            {
                                string hh = Convert.ToDateTime(q.Time_End).ToString("HH");
                                string mm = Convert.ToDateTime(q.Time_End).ToString("mm");
                                de = allDates[i].AddHours(long.Parse(hh)).AddMinutes(long.Parse(mm));
                            }
                        }
                    }

                    child.LeaveDay = Math.Round((Math.Abs((de - ds).TotalHours / 8)) > 1 ? 0 : (Math.Abs((de - ds).TotalHours / 8)), 5, MidpointRounding.AwayFromZero).ToString().Replace(",", ".");

                    iniParent.Add(child);
                }
            }
            return iniParent;
        }

        public async Task<OperationResult> ExportToData(ViewConfirmDailyParam param, PaginationParam pagination)
        {
            List<DateTime> allDates = GetAllDates(param.DateFrom, param.DateTo);
            DateTime d1 = Convert.ToDateTime(param.DateFrom + " 00:00");
            DateTime d2 = Convert.ToDateTime(param.DateTo + " 23:59");
            var data = await GetViewConfirmDaily(param.Lang, param.DateFrom, param.DateTo, pagination, false);
            if (data == null)
                return new OperationResult(false, "No Data");
            var dataView = await GetDataViewData(data.Result, allDates, d1, d2);
            List<ViewComfirmExport> exportExcel = new();
            foreach (var item in dataView)
            {
                var export = new ViewComfirmExport
                {
                    DeptCode = item.DeptCode,
                    DeptName = item.DeptName,
                    EmpName = item.EmpName,
                    EmpNumber = item.EmpNumber,
                    TimeStart = item.TimeStart,
                    TimeEnd = item.TimeEnd,
                    DateStart = item.DateStart,
                    DateEnd = item.DateEnd,
                    LeaveDay = item.LeaveDay,
                    Update = item.Update,
                    CategoryExcel = param.Lang == "vi" ? item.CateSym + " - " + item.CateNameVN : (param.Lang == "en" ? item.CateSym + " - " + item.CateNameEN : item.CateSym + " - " + item.CateNameZH),
                    Status = GetStatus(item.Status, param.Lang)
                };
                exportExcel.Add(export);
            }

            List<Table> dataTable = new()
            {
                new Table("result", exportExcel)
            };

            List<Cell> dataTitle = new()
             {
                new Cell("A1", param.Label_PartCode),
                new Cell("B1", param.Label_DeptName),
                new Cell("C1", param.Label_Employee),
                new Cell("D1", param.Label_NumberID),
                new Cell("E1", param.Label_Category),
                new Cell("F1", param.Label_TimeStart),
                new Cell("G1", param.Label_TimeEnd),
                new Cell("H1", param.Label_DateStart),
                new Cell("I1", param.Label_DateEnd),
                new Cell("J1", param.Label_LeaveDay),
                new Cell("K1", param.Label_Status),
                new Cell("L1", param.Label_UpdateTime),
            };

            ExcelResult excelResult = ExcelUtility.DownloadExcel(dataTable, dataTitle, "Resources\\Template\\SeaHr\\ViewConfirmDaily.xlsx");
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }

        private static List<DateTime> GetAllDates(string textdateS, string textdateE)
        {
            List<DateTime> allDates = new();
            DateTime from = Convert.ToDateTime(textdateS);
            DateTime to = Convert.ToDateTime(textdateE);

            for (DateTime date = from; date <= to; date = date.AddDays(1))
            {
                allDates.Add(date);
            }
            return allDates;
        }
        private static string GetStatus(string approved, string lang)
        {
            return approved switch
            {
                "Chờ Duyệt" => lang == "vi" ? "Chờ Duyệt" : lang == "en" ? "Pending" : "待辦的",
                "Đã Duyệt" => lang == "vi" ? "Đã Duyệt" : lang == "en" ? "Approve" : "得到正式認可的",
                "Từ Chối" => lang == "vi" ? "Từ Chối" : lang == "en" ? "Refuse" : "拒絕",
                _ => lang == "vi" ? "Hoàn Thành" : lang == "en" ? "Complete" : "完全的",
            };
        }
    }
}