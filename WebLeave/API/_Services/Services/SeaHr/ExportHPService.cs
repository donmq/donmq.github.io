using System.Text;
using API._Repositories;
using API._Services.Interfaces.SeaHr;
using API.Dtos.SeaHr.ExportHP;
using API.Helpers.Params;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
namespace API._Services.Services.SeaHr
{
    public class ExportHPService : IExportHPService
    {
        private readonly IRepositoryAccessor _repoAccessor;
        public ExportHPService(IRepositoryAccessor repoAccessor)
        {
            _repoAccessor = repoAccessor;
        }

        public async Task<List<ExportLeave>> GetData(ExportHPParam param)
        {
            List<ExportLeave> result = new();
            List<DateTime> allDates = GetAllDates(param.FromDate, param.ToDate);
            DateTime d1 = Convert.ToDateTime(param.FromDate + " 00:00:00.000");
            DateTime d2 = Convert.ToDateTime(param.ToDate + " 23:59:59.997");

            // pred Model
            var predLeaveData = PredicateBuilder.New<LeaveData>(true);

            predLeaveData.And(q => q.Status_Line == true && q.Approved == 4 &&
                    ((q.Time_Start <= d1 && q.Time_End > d1) ||
                    (q.Time_Start < d2 && q.Time_End >= d2) ||
                    (q.Time_Start >= d1 && q.Time_End <= d2) ||
                    (q.Time_Start <= d1 && q.Time_End >= d2)));

            if (param.AreaID > 0)
                predLeaveData.And(x => x.Emp.Part.Dept.AreaID == param.AreaID);

            if (param.DepartmentID > 0)
                predLeaveData.And(x => x.Emp.Part.DeptID == param.DepartmentID);

            if (param.LeaveType > 0)
            {
                if (param.LeaveType == 1)
                    predLeaveData.And(x => x.Cate.CateSym != "G" && x.Cate.CateSym != "S" && x.Cate.CateSym != "Z" && x.Cate.CateSym != "7" && x.Cate.CateSym != "H" && x.Cate.CateSym != "F");

                else if (param.LeaveType == 2)
                    predLeaveData.And(x => x.Cate.CateSym != "G" && x.Cate.CateSym != "S" && x.Cate.CateSym != "Z");

                else if (param.LeaveType == 3)
                    predLeaveData.And(x => x.Time_Applied > x.Time_Start);

                else if (param.LeaveType == 4)
                    predLeaveData.And(x => x.Cate.CateSym != "7" && x.Cate.CateSym != "H" && x.Cate.CateSym != "F");
                else
                {
                    var cateidLis = new List<int> { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
                    var catesymLis = new List<string> { "J", "A", "D", "E", "G", "O", "S", "Z", "U", "7", "W", "H", "F" };

                    if (cateidLis.Any(x => x == param.LeaveType))
                    {
                        int index = cateidLis.IndexOf(param.LeaveType);
                        predLeaveData.And(x => x.Cate.CateSym == catesymLis[index]);
                    }
                }
            }

            var data = await _repoAccessor.LeaveData.FindAll(predLeaveData)
                .Include(x => x.Emp)
                    .ThenInclude(x => x.Part)
                        .ThenInclude(x => x.Dept)
                .Include(x => x.Cate)
                .ToListAsync();

            //--------------------------- Xuất HP----------------------------------------//
            return await ExportToDataHP(data, allDates, d1, d2);
        }


        public static List<DateTime> GetAllDates(string textdateS, string textdateE)
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

        public async Task<List<ExportLeave>> ExportToDataHP(List<LeaveData> model, List<DateTime> allDates, DateTime d1, DateTime d2)
        {
            List<LunchBreak> lunchs = await _repoAccessor.LunchBreak
                .FindAll(x => x.Visible == true, true)
                .ToListAsync();

            int h1 = 7;
            int m1 = 30;
            int m2 = 30;
            int h2 = 16;
            string h1m1 = "0730";
            string h2m2 = "1630";
            string lunch = "13,00";
            string shift2 = "";
            List<ExportLeave> iniParent = new();
            foreach (LeaveData q in model)
            {
                string[] shift = q.LeaveArchive.Split('-');

                if (shift.Length > 2)
                {
                    LunchBreak lunchBreak = new();
                    shift2 = shift[2];
                    if (shift2 == "C")
                        lunchBreak = new()
                        {
                            Key = "C",
                            WorkTimeStart = new TimeSpan(0, 0, 0),
                            WorkTimeEnd = new TimeSpan(24, 0, 0),
                            LunchTimeStart = new TimeSpan(22, 0, 0),
                            LunchTimeEnd = new TimeSpan(22, 45, 0)
                        };
                    else
                        lunchBreak = lunchs.FirstOrDefault(x => x.Key == shift2);

                    h1 = lunchBreak.WorkTimeStart.Hours;
                    m1 = lunchBreak.WorkTimeStart.Minutes;
                    h2 = shift2 == "C" ? 24 : lunchBreak.WorkTimeEnd.Hours;
                    m2 = lunchBreak.WorkTimeEnd.Minutes;
                    lunch = $"{lunchBreak.LunchTimeStart.Hours}.{lunchBreak.LunchTimeStart.Minutes}";
                    h1m1 = $"{h1.ToString().PadLeft(2, '0')}{m1.ToString().PadLeft(2, '0')}";
                    h2m2 = $"{h2.ToString().PadLeft(2, '0')}{m2.ToString().PadLeft(2, '0')}";
                }
                for (int i = 0; i < allDates.Count; i++)
                {
                    d1 = allDates[i].AddHours(h1).AddMinutes(m1);
                    d2 = allDates[i].AddHours(h2).AddMinutes(m2);

                    if (d2 < q.Time_Start && d2 < q.Time_End)
                        continue;

                    if (d1 > q.Time_Start && d1 > q.Time_End)
                        continue;

                    ExportLeave child = new()
                    {
                        empno = q.Emp.EmpNumber,
                        code = q.Cate.CateSym,
                        symd = allDates[i].ToString("MM/dd/yyyy"),
                        eymd = allDates[i].ToString("MM/dd/yyyy")
                    };

                    DateTime ds;
                    DateTime de;

                    if (q.Time_End >= d2)
                    {
                        // d1 start d2 end
                        if (q.Time_Start > d1)
                        {
                            // lấy giờ  timeStart => ds
                            child.shm = Convert.ToDateTime(q.Time_Start).ToString("HHmm");
                            ds = Convert.ToDateTime(q.Time_Start);
                        }
                        //  start d1 d2 end
                        else
                        {
                            //lấy giờ d1 => ds
                            child.shm = h1m1;
                            ds = allDates[i].AddHours(h1).AddMinutes(m1);
                        }

                        child.ehm = h2m2;

                        if ((ds.Hour + ds.Minute / 60) >= float.Parse(lunch)) //13
                            de = allDates[i].AddHours(h2).AddMinutes(m2);

                        else
                        {
                            if (shift2 == "C")
                                de = allDates[i].AddHours(h2 - 18).AddMinutes(m2); //-30

                            else if (shift2 == "A" || shift2 == "B")
                                de = allDates[i].AddHours(h2).AddMinutes(m2); //-30

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
                            child.shm = Convert.ToDateTime(q.Time_Start).ToString("HHmm");
                            ds = Convert.ToDateTime(q.Time_Start);
                        }
                        //start d1 end d2
                        else
                        {
                            //d1 => ds
                            child.shm = h1m1;
                            ds = allDates[i].AddHours(h1).AddMinutes(m1);
                        }

                        child.ehm = Convert.ToDateTime(q.Time_End).ToString("HHmm");

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

                    child.dat = (Math.Abs((de - ds).TotalHours / 8) > 1 ? 0 : Math.Abs((de - ds).TotalHours / 8)).ToString().Replace(",", ".");

                    iniParent.Add(child);
                }
            }
            return iniParent;
        }

        public async Task<PaginationUtility<ExportLeave>> PaginationData(ExportHPParam param, PaginationParam paginationParam)
        {
            var data = await GetData(param);
            return PaginationUtility<ExportLeave>.Create(data, paginationParam.PageNumber, paginationParam.PageSize, false);
        }

        public async Task<OperationResult> DownloadExcel(ExportHPParam param, string typeFile)
        {
            var data = await GetData(param);
            ExcelResult excelResult;

            if (typeFile == "csv")
            {
                StringWriter sw = new();
                foreach (var index in data)
                {
                    string formatLine = string.Format("{0},{1},=\"{2}\",{3},{4},{5},{6}",
                        index.symd,
                        index.eymd,
                        index.empno,
                        index.code,
                        index.shm,
                        index.ehm,
                        index.dat);
                    sw.WriteLine(formatLine);
                }

                byte[] result = Array.Empty<byte>();

                if (data.Any())
                    result = Encoding.UTF8.GetBytes(sw.ToString());

                return new OperationResult(true, null, result);
            }
            else
            {
                excelResult = ExcelUtility.DownloadExcel(data, "Resources\\Template\\SeaHr\\ExportHP.xlsx");
                return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
            }
        }
    }
}