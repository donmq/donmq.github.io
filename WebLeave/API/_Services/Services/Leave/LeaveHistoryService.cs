using System.Data.SqlTypes;
using API._Repositories;
using API._Services.Interfaces.Leave;
using API.Dtos.Common;
using API.Dtos.Leave.LeaveHistory;
using API.Helpers.Enums;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
namespace API._Services.Services.Leave
{
    public class LeaveHistoryService : ILeaveHistoryService
    {
        private readonly IRepositoryAccessor _repositoryAccessor;
        public LeaveHistoryService(IRepositoryAccessor repositoryAccessor)
        {
            _repositoryAccessor = repositoryAccessor;
        }

        public async Task<List<KeyValuePair<int, string>>> GetCategory(string lang)
        {
            if (lang == "zh")
            {
                lang = LangConstants.ZH_TW;
            }

            List<KeyValuePair<int, string>> data = await _repositoryAccessor.Category.FindAll(x => x.Visible == true)
            .Include(x => x.CatLangs)
            .Select(x => new KeyValuePair<int, string>(
                x.CateID, $"{x.CateSym} - {x.CatLangs.FirstOrDefault(z => z.CateID == x.CateID && z.LanguageID == lang).CateName}"
            )).ToListAsync();

            return data;
        }

        public async Task<LeaveDataDtos> GetLeaveData(SearchHistoryParamsDto paramsSearch, PaginationParam pagination, bool isPaging = true)
        {
            Users user = await _repositoryAccessor.Users.FirstOrDefaultAsync(x => x.UserID == paramsSearch.UserID);

            DateTime startDateTime = paramsSearch.StartTime != null ? Convert.ToDateTime(paramsSearch.StartTime + " 00:00:00") : new DateTime(DateTime.Now.Year, 1, 1, 00, 00, 00);
            DateTime endDateTime = paramsSearch.EndTime != null ? Convert.ToDateTime(paramsSearch.EndTime + " 23:59:59") : new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59);

            var leaveDataQuery = _repositoryAccessor.LeaveData
                .FindAll(x => (isPaging == true
                    ? (x.Time_Start >= startDateTime && x.Time_End <= endDateTime)
                    : ((x.Time_Start <= startDateTime && x.Time_End > startDateTime) ||
                    (x.Time_Start < endDateTime && x.Time_End >= endDateTime) ||
                    (x.Time_Start >= startDateTime && x.Time_End <= endDateTime) ||
                    (x.Time_Start <= startDateTime && x.Time_End >= endDateTime))) &&
                    x.Emp.Part.Dept.Building != null &&
                    x.Emp.Part.Dept.Area != null, true)
                .Include(x => x.Cate.CatLangs)
                .Include(x => x.Emp.Part.PartLangs)
                .Include(x => x.Emp.Part.Dept.Building)
                .Include(x => x.Emp.Part.Dept.Area)
                .Select(x => new LeaveDataDto
                {
                    Approved = x.Approved,
                    ApprovedBy = x.ApprovedBy,
                    CateID = x.Cate.CateID,
                    LeaveArchive = x.LeaveArchive,
                    PartID = x.Emp.Part.PartID,
                    PartCode = x.Emp.Part.PartCode ?? "",
                    Comment = x.Comment,
                    LeaveID = x.LeaveID,
                    DateLeave = x.DateLeave,
                    LeavePlus = x.LeavePlus,
                    EmpID = x.EmpID,
                    EmpName = x.Emp.EmpName ?? "",
                    EmpNumber = x.Emp.EmpNumber ?? "",
                    Status_Line = x.Status_Line,
                    LeaveDay = x.LeaveDay,
                    Updated = x.Updated,
                    UserID = x.UserID,
                    CateSym = x.Cate.CateSym ?? "",
                    PartSym = x.Emp.Part.PartSym ?? "",
                    DeptSym = x.Emp.Part.Dept.DeptSym ?? "",
                    AreaSym = x.Emp.Part.Dept.Area.AreaSym ?? "",
                    BuildingSym = x.Emp.Part.Dept.Building.BuildingSym ?? "",
                    LeaveDayByString = ConvertLeaveDay(x.LeaveDay.ToString()),
                    Time_End = x.Time_End,
                    Time_Start = x.Time_Start,
                    TimeLine = x.TimeLine,
                    CateNameVN = x.Cate.CatLangs.FirstOrDefault(y => y.CateID.Value == x.Cate.CateID && y.LanguageID == LangConstants.VN).CateName ?? "",
                    CateNameEN = x.Cate.CatLangs.FirstOrDefault(y => y.CateID.Value == x.Cate.CateID && y.LanguageID == LangConstants.EN).CateName ?? "",
                    CateNameZH = x.Cate.CatLangs.FirstOrDefault(y => y.CateID.Value == x.Cate.CateID && y.LanguageID == LangConstants.ZH_TW).CateName ?? "",
                    PartNameVN = x.Emp.Part.PartLangs.FirstOrDefault(y => y.PartID.Value == x.Emp.Part.PartID && y.LanguageID == LangConstants.VN).PartName ?? "",
                    PartNameEN = x.Emp.Part.PartLangs.FirstOrDefault(y => y.PartID.Value == x.Emp.Part.PartID && y.LanguageID == LangConstants.EN).PartName ?? "",
                    PartNameZH = x.Emp.Part.PartLangs.FirstOrDefault(y => y.PartID.Value == x.Emp.Part.PartID && y.LanguageID == LangConstants.ZH_TW).PartName ?? ""
                }).Distinct().ToList();


            var predicate = PredicateBuilder.New<LeaveDataDto>(true);
            // Thêm các điều kiện lọc khác
            if (paramsSearch.Status == 6)
                predicate.And(x => x.Status_Line == false && x.Comment.Contains("Đã xóa"));
            else
                predicate.And(x => x.Status_Line == true);

            if (paramsSearch.CategoryId != 0)
                predicate.And(x => x.CateID == paramsSearch.CategoryId);
            if (!string.IsNullOrEmpty(paramsSearch.EmpId))
                predicate.And(x => x.EmpNumber == paramsSearch.EmpId);
            if (paramsSearch.Status > 0 && paramsSearch.Status != 6)
                predicate.And(x => x.Approved == paramsSearch.Status);

            List<LeaveDataDto> listData = new();
            if (user.UserRank == 1 || user.UserRank == 6)
            {
                predicate.And(x => x.EmpID == user.EmpID);
                listData = leaveDataQuery.Where(predicate).ToList();
            }
            else if (user.UserRank == 2 || user.UserRank == 4)
            {
                predicate.And(x => x.UserID == user.UserID);
                listData = leaveDataQuery.Where(predicate).ToList();
            }
            else
            {
                List<KeyValuePair<string, string>> checkAllowData = await CheckAllowData(paramsSearch.UserID);
                foreach (var item in checkAllowData)
                {
                    if (paramsSearch.Status == 6)
                    {
                        List<LeaveDataDto> _listItem = item.Key switch
                        {
                            "A" => leaveDataQuery
                               .FindAll(
                                   x => x.Status_Line == false && x.Comment.Contains("Đã xóa") &&
                                   (x.AreaSym == item.Value || x.UserID == user.UserID)),
                            "B" => leaveDataQuery
                                .FindAll(
                                    x => x.Status_Line == false && x.Comment.Contains("Đã xóa") &&
                                    (x.BuildingSym == item.Value || x.UserID == user.UserID)),
                            "D" => leaveDataQuery
                                .FindAll(
                                    x => x.Status_Line == false && x.Comment.Contains("Đã xóa") &&
                                    (x.DeptSym == item.Value || x.UserID == user.UserID)),
                            _ => leaveDataQuery
                                .FindAll(
                                    x => x.Status_Line == false && x.Comment.Contains("Đã xóa") &&
                                    (x.PartSym == item.Value || x.UserID == user.UserID)),
                        };
                        listData.AddRange(_listItem);
                    }
                    else
                    {
                        List<LeaveDataDto> _listItem = item.Key switch
                        {
                            "A" => leaveDataQuery
                                .FindAll(
                                    x => x.Status_Line == true &&
                                    (x.AreaSym == item.Value || x.UserID == user.UserID)),
                            "B" => leaveDataQuery
                                .FindAll(
                                    x => x.Status_Line == true &&
                                    (x.BuildingSym == item.Value || x.UserID == user.UserID)),
                            "D" => leaveDataQuery
                                .FindAll(
                                    x => x.Status_Line == true &&
                                    (x.DeptSym == item.Value || x.UserID == user.UserID)),
                            _ => leaveDataQuery
                                .FindAll(
                                    x => x.Status_Line == true &&
                                    (x.PartSym == item.Value || x.UserID == user.UserID)),
                        };
                        listData.AddRange(_listItem);
                    }
                }

                if (paramsSearch.CategoryId != 0)
                    listData = listData.Where(x => x.CateID == paramsSearch.CategoryId).ToList();
                if (!string.IsNullOrEmpty(paramsSearch.EmpId))
                    listData = listData.Where(x => x.EmpNumber.ToUpper() == paramsSearch.EmpId.ToUpper()).ToList();
                if (paramsSearch.Status > 0 && paramsSearch.Status != 6)
                    listData = listData.Where(x => x.Approved == paramsSearch.Status).ToList();
            }

            LeaveDataDtos result = new()
            {
                LeaveData = PaginationUtility<LeaveDataDto>.Create(listData.OrderByDescending(x => x.Updated).Distinct().ToList(), pagination.PageNumber, pagination.PageSize, isPaging),
                SumLeaveDay = paramsSearch.Status != 6 ? Convert.ToInt32(listData.Where(x => x.Approved == 4).Sum(x => x.LeaveDay)) : 0
            };

            return result;
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

        private async Task<List<KeyValuePair<string, string>>> CheckAllowData(int? userID)
        {
            List<KeyValuePair<string, string>> result = new();
            var roles = await _repositoryAccessor.RolesUser.FindAll(x => x.UserID == userID)
            .Include(x => x.Role).ToListAsync();

            if (roles.Count > 0)
            {
                foreach (var item in roles)
                {
                    switch (item.Role.Ranked.Value)
                    {
                        case 1:
                            result.Add(new KeyValuePair<string, string>("A", item.Role.RoleSym));
                            break;
                        case 2:
                            result.Add(new KeyValuePair<string, string>("B", item.Role.RoleSym));
                            break;
                        case 3:
                            result.Add(new KeyValuePair<string, string>("D", item.Role.RoleSym));
                            break;
                        default:
                            result.Add(new KeyValuePair<string, string>("P", item.Role.RoleSym));
                            break;
                    }
                }
            }
            return result;
        }
        public async Task<OperationResult> ExportExcel(HistoryExportParam param, PaginationParam pagination)
        {
            var data = await GetLeaveData(param, pagination, false);
            data.LeaveData.Result = data.LeaveData.Result.OrderBy(x => x.Time_Start).ToList();

            List<HistoryExportParam> exportParams = new();
            foreach (var item in data.LeaveData.Result)
            {
                var exportParam = new HistoryExportParam
                {
                    PartName = item.PartCode,
                    DeptName = item.PartCode + "-" + item.PartNameVN + "-" + item.PartNameZH,
                    Employee = item.EmpName,
                    NumberId = item.EmpNumber,
                    Category = item.CateSym + "-" + item.CateNameVN + "-" + item.CateNameZH,
                    TimeStart = item.Time_Start.Value.ToString("HH:mm MM/dd/yyyy"),
                    TimeEnd = item.Time_End.Value.ToString("HH:mm MM/dd/yyyy"),
                    LeaveDay = item.LeaveDay.ToString(),
                    status1 = GetStatus(item.Approved.Value, item.Status_Line.Value, param.Lang),
                    Update = item.Updated.Value.ToString("HH:mm MM/dd/yyyy")
                };

                exportParams.Add(exportParam);
            }

            List<Table> dataTable = new()
            {
                new Table("result", exportParams)
            };

            List<Cell> dataTitle = new()
            {
                new Cell("A1", param.PartName),
                new Cell("B1", param.DeptName),
                new Cell("C1", param.Employee),
                new Cell("D1", param.NumberId),
                new Cell("E1", param.Category),
                new Cell("F1", param.TimeStart),
                new Cell("G1", param.TimeEnd),
                new Cell("H1", param.LeaveDay),
                new Cell("I1", param.status1),
                new Cell("J1", param.Update),
            };

            ExcelResult excelResult = ExcelUtility.DownloadExcel(dataTable, dataTitle, "Resources\\Template\\Leave\\LeaveHistoryTemplate.xlsx");
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }

        private static string GetStatus(int approved, bool status, string lang)
        {
            return approved switch
            {
                1 when status == true => lang == "vi" ? "Chờ Duyệt" : lang == "en" ? "Pending" : "待辦的",
                1 when status == false => lang == "vi" ? "Đã Xóa" : lang == "en" ? "Delete" : "已刪除",
                2 => lang == "vi" ? "Đã Duyệt" : lang == "en" ? "Approve" : "得到正式認可的",
                3 => lang == "vi" ? "Từ Chối" : lang == "en" ? "Refuse" : "拒絕",
                _ => lang == "vi" ? "Hoàn Thành" : lang == "en" ? "Complete" : "完全的",
            };
        }
    }
}