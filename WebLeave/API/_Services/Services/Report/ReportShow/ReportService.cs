using API._Repositories;
using API._Services.Interfaces.Report;
using API.Dtos.Report.ReportShow;
using API.Helpers.Enums;
using API.Helpers.Params.ReportShow;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API._Services.Services.Report
{
    public class ReportService : IReportService
    {
        private readonly IRepositoryAccessor _repositoryAccessor;

        public ReportService(IRepositoryAccessor repositoryAccessor)
        {
            _repositoryAccessor = repositoryAccessor;
        }

        public async Task<ReportIndexViewModelDTO> ReportShow(ReportShowParam param)
        {
            ReportIndexViewModelDTO reportIndexViewModelDTO = new();
            List<ReportShowModelDTO> listChild = new();
            List<List<ReportShowModelDTO>> listParent = new();
            if (param.Lang == "zh") param.Lang = "zh-TW";
            if (param.From is null && param.To is null)
            {
                DateTime today = DateTime.Today;
                param.From = new DateTime(today.Year, today.Month, 1);
                param.To = param.From.Value.AddMonths(1).AddDays(-1);
            }
            else
            {
                param.From = Convert.ToDateTime(param.From.Value.ToString("yyyy/MM/dd"));
                param.To = Convert.ToDateTime(param.To.Value.ToString("yyyy/MM/dd"));
            }
            if (param.Index == 0)
            {
                reportIndexViewModelDTO.Title = GetTitleCompanyByLang();
                int compId = (await _repositoryAccessor.Company.FirstOrDefaultAsync(x => x.Visible == true)).CompanyID;
                param.Id = compId;
                reportIndexViewModelDTO.ListReportShowModel = ReportShows(param.Index, compId, param.From, param.To);
            }
            else if (param.Index == 1)
            {
                reportIndexViewModelDTO.Title = GetTitleAreaByLang(param.Id.Value);
                reportIndexViewModelDTO.ListReportShowModel = ReportShows(param.Index, param.Id.Value, param.From, param.To);
            }
            else if (param.Index == 2)
            {
                reportIndexViewModelDTO.Title = GetTitleBuildingByLang(param.Id.Value);
                reportIndexViewModelDTO.ListReportShowModel = ReportShows(param.Index, param.Id.Value, param.From, param.To);
            }
            else if (param.Index == 3)
            {
                reportIndexViewModelDTO.Title = GetTitleDeptByLang(param.Id.Value);
                reportIndexViewModelDTO.ListReportShowModel = ReportShows(param.Index, param.Id.Value, param.From, param.To);
            }
            else if (param.Index == 4)
            {
                reportIndexViewModelDTO.Title = GetTitlePartByLang(param.Id.Value);
                reportIndexViewModelDTO.ListReportShowModel = ReportShows(param.Index, param.Id.Value, param.From, param.To);
            }

            int i = 1;
            int row = 0;
            foreach (var item in reportIndexViewModelDTO.ListReportShowModel)
            {
                listChild.Add(item);
                if (item.DayOfWeek == 6 || reportIndexViewModelDTO.ListReportShowModel.Count == i)
                {
                    listParent.Add(listChild);
                    listChild = new List<ReportShowModelDTO>();
                    row++;
                }
                i++;
            }
            reportIndexViewModelDTO.ListParent = listParent;
            reportIndexViewModelDTO.StartDay = param.From.Value;
            reportIndexViewModelDTO.EndDay = param.To.Value;
            return reportIndexViewModelDTO;
        }

        private GetTitleByLang GetTitleCompanyByLang()
        {
            string name = _repositoryAccessor.Company.FirstOrDefault(x => x.Visible == true)?.CompanyName;
            return new GetTitleByLang
            {
                vi = name,
                en = name,
                zh_TW = name
            };
        }

        private GetTitleByLang GetTitleAreaByLang(int id)
        {
            var areas = _repositoryAccessor.AreaLang
                .FindAll(x => x.AreaID == id && x.Area.Visible == true)
                .Include(x => x.Area)
                .ToList();
            return new GetTitleByLang
            {
                vi = areas.FirstOrDefault(x => x.LanguageID == LangConstants.VN)?.AreaName,
                en = areas.FirstOrDefault(x => x.LanguageID == LangConstants.EN)?.AreaName,
                zh_TW = areas.FirstOrDefault(x => x.LanguageID == LangConstants.ZH_TW)?.AreaName
            };
        }

        private GetTitleByLang GetTitleBuildingByLang(int id)
        {
            var buildings = _repositoryAccessor.BuildLang
                .FindAll(x => x.BuildingID == id && x.Building.Visible == true)
                .Include(x => x.Building)
                .ToList();
            return new GetTitleByLang
            {
                vi = buildings.FirstOrDefault(x => x.LanguageID == LangConstants.VN)?.BuildingName,
                en = buildings.FirstOrDefault(x => x.LanguageID == LangConstants.EN)?.BuildingName,
                zh_TW = buildings.FirstOrDefault(x => x.LanguageID == LangConstants.ZH_TW)?.BuildingName,
            };
        }

        private GetTitleByLang GetTitleDeptByLang(int id)
        {
            var depts = _repositoryAccessor.DetpLang
                .FindAll(x => x.DeptID == id && x.Dept.Visible == true)
                .Include(x => x.Dept)
                .ToList();
            return new GetTitleByLang
            {
                vi = depts.FirstOrDefault(x => x.LanguageID == LangConstants.VN)?.DeptName,
                en = depts.FirstOrDefault(x => x.LanguageID == LangConstants.EN)?.DeptName,
                zh_TW = depts.FirstOrDefault(x => x.LanguageID == LangConstants.ZH_TW)?.DeptName,
            };
        }

        private GetTitleByLang GetTitlePartByLang(int id)
        {
            var parts = _repositoryAccessor.PartLang
                .FindAll(x => x.PartID == id && x.Part.Visible == true)
                .Include(x => x.Part)
                .ToList();
            return new GetTitleByLang
            {
                vi = parts.FirstOrDefault(x => x.LanguageID == LangConstants.VN)?.PartName,
                en = parts.FirstOrDefault(x => x.LanguageID == LangConstants.EN)?.PartName,
                zh_TW = parts.FirstOrDefault(x => x.LanguageID == LangConstants.ZH_TW)?.PartName,
            };
        }

        private static List<DateTime> EachDays(DateTime from, DateTime to)
        {
            List<DateTime> allDates = new();
            for (DateTime date = from; date <= to; date = date.AddDays(1))
            {
                allDates.Add(date);
            }
            return allDates;
        }

        private List<ReportShowModelDTO> ReportShows(int index, int? id, DateTime? From, DateTime? To)
        {
            List<ReportShowModelDTO> listReportShowModel = new();
            List<DateTime> allDay = EachDays(From.Value, To.Value);

            int getTotalEmployee(int index)
            {
                if (index == 0)
                    return _repositoryAccessor.Employee.Count(x => x.Visible == true);
                else if (index == 1)
                    return TotalEmployeeArea(id.Value);
                else if (index == 2)
                    return TotalEmployeeBuilding(id.Value);
                else if (index == 3)
                    return TotalEmployeeDept(id.Value);
                else if (index == 4)
                    return _repositoryAccessor.Employee.Count(x => x.PartID == id && x.Visible == true);
                return 0;
            }

            foreach (var day in allDay)
            {
                List<ReportData> listReportDataRaw = _repositoryAccessor.ReportData.FindAll(
                    x => x.LeaveDate.Value.Year == day.Year
                    && x.LeaveDate.Value.Month == day.Month
                    && x.LeaveDate.Value.Day == day.Day
                    && x.LeaveID != 0
                    && (
                        (index == 1 && x.Emp.Part.Dept.AreaID == id) ||
                        (index == 2 && x.Emp.Part.Dept.BuildingID == id) ||
                        (index == 3 && x.Emp.Part.Dept.DeptID == id) ||
                        (index == 4 && x.Emp.Part.PartID == id) ||
                        (index != 1 && index != 2 && index != 3 && index != 4)
                    ))
                .Include(x => x.Emp.Part)
                .Select(x => new ReportData
                {
                    StatusLine = x.StatusLine,
                    MPPoolIn = x.MPPoolIn,
                    MPPoolOut = x.MPPoolOut
                }).AsNoTracking().ToList();

                ReportShowModelDTO reportShowModel = new()
                {
                    LeaveDate = day,
                    DayOfWeek = (int)day.DayOfWeek
                };

                RenderReportGrid(listReportDataRaw, reportShowModel, getTotalEmployee(index));
                listReportShowModel.Add(reportShowModel);
            }

            return listReportShowModel;
        }

        private int TotalEmployeeArea(int AreaId)
        {
            int totalEmp = _repositoryAccessor.Employee.FindAll(x => x.Part.Dept.AreaID == AreaId && x.Visible == true)
            .Include(x => x.Part.Dept)
            .Count();
            return totalEmp;
        }

        private int TotalEmployeeBuilding(int BuildingId)
        {
            int totalEmp = _repositoryAccessor.Employee
                .FindAll(x => x.Part.Dept.Building.BuildingID == BuildingId && x.Visible == true)
                .Include(x => x.Part.Dept.Building)
                .Count();
            return totalEmp;
        }

        private int TotalEmployeeDept(int DeptId)
        {
            int totalEmp = _repositoryAccessor.Employee
                .FindAll(x => x.Part.Dept.DeptID == DeptId && x.Visible == true)
                .Include(x => x.Part.Dept)
                .Count();
            return totalEmp;
        }

        private static void RenderReportGrid(List<ReportData> listReportDataRaw, ReportShowModelDTO reportShowModel, int totalEmp)
        {
            if (listReportDataRaw.Any())
            {
                reportShowModel.SEAMP = totalEmp;
                reportShowModel.Applied = listReportDataRaw.Count(x => x.StatusLine == 1);
                reportShowModel.Approved = listReportDataRaw.Count(x => x.StatusLine == 2);
                reportShowModel.MPPoolOut = listReportDataRaw.Sum(x => x.MPPoolIn.Value);
                reportShowModel.MPPoolIn = listReportDataRaw.Sum(x => x.MPPoolIn.Value);
                reportShowModel.Actual = reportShowModel.SEAMP - reportShowModel.Approved;
                reportShowModel.Total = reportShowModel.Actual - reportShowModel.MPPoolOut + reportShowModel.MPPoolIn;
            }
            else
            {
                reportShowModel.SEAMP = totalEmp;
                reportShowModel.Applied = 0;
                reportShowModel.Approved = 0;
                reportShowModel.MPPoolOut = 0;
                reportShowModel.MPPoolIn = 0;
                reportShowModel.Actual = reportShowModel.SEAMP - reportShowModel.Approved;
                reportShowModel.Total = reportShowModel.Actual - reportShowModel.MPPoolOut + reportShowModel.MPPoolIn;
            }
        }

        // Hiển thị chi tiết khi click ReportShow ở dạng Grid
        public async Task<List<ReportShowModelDTO>> ReportGridDetail(ReportGridDetailParam param)
        {
            List<ReportData> listRepoData = new();
            if (param.language == "zh") param.language = "zh-TW";
            DateTime leaveDay = new();
            listRepoData = await _repositoryAccessor.ReportData
                .FindAll(
                    x => x.LeaveDate.Value.Year == param.year
                    && x.LeaveDate.Value.Month == param.month
                    && x.LeaveDate.Value.Day == param.day
                    && x.LeaveID != 0
                    && (
                        (param.index == 0 && x.Emp.Part.Dept.Area.CompanyID == param.id) ||
                        (param.index == 1 && x.Emp.Part.Dept.AreaID == param.id) ||
                        (param.index == 2 && x.Emp.Part.Dept.BuildingID == param.id) ||
                        (param.index == 3 && x.Emp.Part.Dept.DeptID == param.id) ||
                        (param.index == 4 && x.Emp.PartID == param.id)))
                .Include(x => x.Emp.Part.Dept.Area)
                .Select(x => new ReportData
                {
                    LeaveID = x.LeaveID,
                    StatusLine = x.StatusLine,
                    LeaveDate = x.LeaveDate
                }).OrderBy(x => x.StatusLine).AsNoTracking().ToListAsync();
            // lấy ngày làm tiều đề trang
            if (listRepoData.Any())
            {
                leaveDay = listRepoData[0].LeaveDate.Value;
            }
            // lấy tên công ty làm tiêu đề trang
            // Title = await _repositoryAccessor.Company.FindAll(x => x.CompanyID == param.id).Select(x => x.CompanyName).FirstOrDefaultAsync();

            // Lấy dữ liệu từ bảng LeaveData với điều kiện LeaveID == LeaveID của bảng ReportData
            List<int?> leaveIds = listRepoData.Select(x => x.LeaveID).Distinct().ToList();
            var listLeave = await _repositoryAccessor.LeaveData
                .FindAll(x => leaveIds.Contains(x.LeaveID) && x.Status_Line == true)
                .Include(x => x.Emp.Part.Dept)
                .AsNoTracking().ToListAsync();

            var listReportModel = listRepoData
                    .Join(listLeave,
                        x => x.LeaveID,
                        y => y.LeaveID,
                        (x, y) => new ReportShowModelDTO
                        {
                            leaveData = y,
                            LeaveStatus = x.StatusLine.Value
                        })
                    .ToList();

            var dataCat = await _repositoryAccessor.CatLang.FindAll(x => x.LanguageID == param.language).AsNoTracking().ToListAsync();
            var dataDept = await _repositoryAccessor.DetpLang
                .FindAll(x => x.LanguageID == param.language && x.Dept.Visible == true)
                .Include(x => x.Dept).AsNoTracking().ToListAsync();
            var dataPost = await _repositoryAccessor.PosLang.FindAll(x => x.LanguageID == param.language).AsNoTracking().ToListAsync();

            var listReportShowModels = listReportModel
                .Select(item => new ReportShowModelDTO
                {
                    LeaveStatus = item.LeaveStatus,
                    LeaveDate = leaveDay,
                    Time_Start = FormatDate(item.leaveData.Time_Start.Value, param.language),
                    Time_End = FormatDate(item.leaveData.Time_End.Value, param.language),
                    LeaveDay = item.LeaveDay,
                    Hour = item.LeaveDay * 8,
                    LeaveType = item.leaveData.CateID + "." + dataCat.FirstOrDefault(x => x.CateID == item.leaveData.CateID)?.CateName,
                    PartCode = listLeave.FirstOrDefault(x => x.LeaveID == item.leaveData.LeaveID)?.Emp?.Part?.PartCode,
                    DeptCode = listLeave.FirstOrDefault(x => x.LeaveID == item.leaveData.LeaveID)?.Emp?.Part?.Dept?.DeptCode + "-"
                             + dataDept.FirstOrDefault(x => x.DeptID == listLeave.FirstOrDefault(x => x.LeaveID == item.leaveData.LeaveID).Emp.Part.Dept.DeptID)?.Dept?.DeptName,
                    EmployeeNumber = listLeave.FirstOrDefault(x => x.LeaveID == item.leaveData.LeaveID)?.Emp?.EmpNumber,
                    EmployeeName = listLeave.FirstOrDefault(x => x.LeaveID == item.leaveData.LeaveID)?.Emp?.EmpName,
                    EmployeePostition = dataPost.FirstOrDefault(x => x.PositionID == listLeave.FirstOrDefault(x => x.LeaveID == item.leaveData.LeaveID).Emp.PositionID)?.PositionName
                }).ToList();

            return listReportShowModels;
        }

        private static string FormatDate(DateTime date, string language)
        {
            string strDate = "";
            if (language.Equals("vi"))
            {
                strDate = date.ToString("dd/MM/yyyy HH:mm");
            }
            else if (language.Equals("en"))
            {
                strDate = date.ToString("MM/dd/yyyy HH:mm");
            }
            else if (language.Equals("zh-TW"))
            {
                strDate = date.ToString("yyyy/MM/dd HH:mm");
            }
            return strDate;
        }

        /// <summary>
        /// Hiển thi report theo ngày
        /// statusline = 1 => button Applied click
        /// statusline = 2 => buttun Approved click
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="index"></param>
        /// <returns></returns>

        public async Task<List<ReportShowModelDTO>> ReportDateDetail(ReportDateDetailParam param)
        {
            DateTime leaveDay = new();
            // string Title = "";
            // Lấy dữ liệu từ bảng ReportData với LeaveDate == date và Statusline == statusline va theo Id tương ứng 
            List<ReportData> listRepoData = new();
            listRepoData = await _repositoryAccessor.ReportData
                .FindAll(
                    x => x.LeaveDate.Value.Year == param.year
                    && x.LeaveDate.Value.Month == param.month
                    && x.LeaveDate.Value.Day == param.day
                    && x.LeaveID != 0
                    && (
                        (param.index == 0 && x.Emp.Part.Dept.Area.CompanyID == param.id) ||
                        (param.index == 1 && x.Emp.Part.Dept.AreaID == param.id) ||
                        (param.index == 2 && x.Emp.Part.Dept.BuildingID == param.id) ||
                        (param.index == 3 && x.Emp.Part.Dept.DeptID == param.id) ||
                        (param.index == 4 && x.Emp.PartID == param.id)
                    ))
                .Include(x => x.Emp.Part.Dept.Area)
                .Select(x => new ReportData
                {
                    LeaveID = x.LeaveID,
                    StatusLine = x.StatusLine,
                    LeaveDate = x.LeaveDate

                }).AsNoTracking().ToListAsync();

            if (listRepoData.Any())
            {
                leaveDay = listRepoData[0].LeaveDate.Value;
            }
            // Title = await _repositoryAccessor.Company.FindAll(x => x.CompanyID == param.id).Select(x => x.CompanyName).FirstOrDefaultAsync();

            // Lấy dữ liệu từ bảng LeaveData với điều kiện là LeaveID == với những LeaveID từ bảng ReportData
            List<LeaveData> listLeaveData = await _repositoryAccessor.LeaveData
                .FindAll(
                    x => listRepoData.Select(x => x.LeaveID).ToList().Contains(x.LeaveID) &&
                    x.Status_Line == true)
                .Include(x => x.Emp.Part.Dept)
                .AsNoTracking().ToListAsync();

            var dataCat = await _repositoryAccessor.CatLang.FindAll(x => x.LanguageID == param.language).AsNoTracking().ToListAsync();
            var dataDept = await _repositoryAccessor.DetpLang
                .FindAll(x => x.LanguageID == param.language && x.Dept.Visible == true)
                .Include(x => x.Dept).AsNoTracking().ToListAsync();
            var dataPost = await _repositoryAccessor.PosLang.FindAll(x => x.LanguageID == param.language).AsNoTracking().ToListAsync();

            // Lấy dữ liệu liên quan hiển thi ra model
            var listReportShowModels = listLeaveData
                .Select(item => new ReportShowModelDTO
                {
                    LeaveStatus = param.statusline,
                    LeaveDate = leaveDay,
                    Time_Start = FormatDate(item.Time_Start.Value, param.language),
                    Time_End = FormatDate(item.Time_End.Value, param.language),
                    LeaveDay = item.LeaveDay,
                    // Title = Title,
                    Hour = item.LeaveDay * 8,
                    LeaveType = item.CateID + "." + dataCat.FirstOrDefault(x => x.CateID == item.CateID)?.CateName,
                    PartCode = listLeaveData.FirstOrDefault(x => x.LeaveID == item.LeaveID)?.Emp?.Part?.PartCode,
                    DeptCode = listLeaveData.FirstOrDefault(x => x.LeaveID == item.LeaveID)?.Emp?.Part?.Dept?.DeptCode + "-"
                            + dataDept.FirstOrDefault(x => x.Dept.DeptID == listLeaveData.FirstOrDefault(x => x.LeaveID == item.LeaveID).Emp.Part.Dept.DeptID)?.Dept?.DeptName,
                    EmployeeNumber = listLeaveData.FirstOrDefault(x => x.LeaveID == item.LeaveID)?.Emp?.EmpNumber,
                    EmployeeName = listLeaveData.FirstOrDefault(x => x.LeaveID == item.LeaveID)?.Emp?.EmpName,
                    EmployeePostition = dataPost.FirstOrDefault(x => x.PositionID == listLeaveData.FirstOrDefault(x => x.LeaveID == item.LeaveID).Emp.PositionID)?.PositionName
                }).ToList();
            return listReportShowModels;

        }

        public async Task<OperationResult> ExportDateDetail(ExportExcelDateDto model)
        {
            List<Cell> listCell = new()
            {
                   new Cell ("A1", model.No),
                   new Cell ("B1", model.EmployeeNumber),
                   new Cell ("C1", model.EmployeeName),
                   new Cell ("D1", model.PartCode),
                   new Cell ("E1", model.EmployeePostition),
                   new Cell ("F1", model.LeaveType),
                   new Cell ("G1", model.Time_Of_Leave),
                   new Cell ("I1", model.Status),
                   new Cell ("G2", model.FromDate),
                   new Cell ("H2", model.EndDate),
            };

            var dataExportList = model.ReportDateDetailDTO.listReportShowDateDetail
            .Select((item, index) => new ReportShowModelDTO
            {
                Index = index + 1,
                EmployeeNumber = item.EmployeeNumber.ToString(),
                EmployeeName = item.EmployeeName,
                PartCode = item.PartCode,
                EmployeePostition = item.EmployeePostition,
                LeaveType = item.LeaveType,
                Time_Start = item.Time_Start,
                Time_End = item.Time_End,
                LeaveStatus_Str = item.LeaveStatus != 1 ? "Approved" : "Applied",
            })
            .ToList();

            List<Table> dataTable = new()
            {
                new Table("result", dataExportList)
            };

            ExcelResult result = await Task.Run(() => ExcelUtility.DownloadExcel(dataTable, listCell, "Resources\\Template\\Report\\ReportShow\\ReportDateDetailTemplate.xlsx"));
            return new OperationResult(result.IsSuccess, result.Error, result.Result);
        }

        public async Task<OperationResult> ExportGridDetail(ExportExcelGridDto model)
        {
            List<Cell> listCell = new()
            {
                model.ReportIndexViewModelDTO.Lang == "vi"
                    ? new Cell("A2", "Từ ngày: " + model.ReportIndexViewModelDTO.StartDay.Value.ToString("yyyy/MM/dd") + "  Đến ngày: " + model.ReportIndexViewModelDTO.EndDay.Value.ToString("yyyy/MM/dd"))
                    : model.ReportIndexViewModelDTO.Lang == "en"
                        ? new Cell("A2", "Từ ngày: " + model.ReportIndexViewModelDTO.StartDay.Value.ToString("yyyy/MM/dd") + "  Đến ngày: " + model.ReportIndexViewModelDTO.EndDay.Value.ToString("yyyy/MM/dd"))
                        : new Cell("A2", "休假時間: " + model.ReportIndexViewModelDTO.StartDay.Value.ToString("yyyy/MM/dd") + "  自日期訖: " + model.ReportIndexViewModelDTO.EndDay.Value.ToString("yyyy/MM/dd")),

                   new Cell ("A3", model.LeaveDate),
                   new Cell ("B3", model.SEAMP),
                   new Cell ("C3", model.Applied),
                   new Cell ("D3", model.Approved),
                   new Cell ("E3", model.Actual),
                   new Cell ("F3", model.MPPoolOut),
                   new Cell ("G3", model.MPPoolIn),
                   new Cell ("H3", model.Total),
            };

            List<ExcelExportReportShowModel> dataExportList = new();
            foreach (var item in model.ReportIndexViewModelDTO.ListReportShowModel)
            {
                ExcelExportReportShowModel dataExport = new()
                {
                    LeaveDate = item.LeaveDate.ToString("yyyy/MM/dd"),
                    SEAMP = item.SEAMP,
                    Applied = item.Applied,
                    Approved = item.Approved,
                    MPPoolOut = item.MPPoolOut,
                    MPPoolIn = item.MPPoolIn,
                    Total = item.Total,
                };

                dataExportList.Add(dataExport);
            }

            List<Table> dataTable = new()
            {
                new Table("result", dataExportList)
            };

            ExcelResult result = await Task.Run(() => ExcelUtility.DownloadExcel(dataTable, listCell, "Resources\\Template\\Report\\ReportShow\\ReportShowGridTemplate.xlsx"));
            return new OperationResult(result.IsSuccess, result.Error, result.Result);
        }

    }
}