using System.Drawing;
using System.Text.RegularExpressions;
using AgileObjects.AgileMapper.Extensions;
using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Models;
using Aspose.Cells;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance
{
    public partial class S_5_2_24_ResignationAnnualLeaveWorktypeAnalysisReport : BaseServices, I_5_2_24_ResignationAnnualLeaveWorktypeAnalysisReport
    {
        [GeneratedRegex("[^A-Z]+")]
        private static partial Regex MyRegex();
        public S_5_2_24_ResignationAnnualLeaveWorktypeAnalysisReport(DBContext dbContext) : base(dbContext) { }
        private static readonly string rootPath = Directory.GetCurrentDirectory();

        public async Task<OperationResult> DownloadExcel(ResignationAnnualLeaveWorktypeAnalysisReportParam param, string userName)
        {
            var data = await Getdata(param);
            if (!data.Any())
            {
                return new OperationResult(isSuccess: false, "No data for excel download");
            }
            var listlevel = await GetListLevel(param.Lang);
            var updatedPermissionGroup = new List<string>();
            var listPermissionGroup = await GetListPermissionGroup(param.Factory, param.Lang);
            MemoryStream memoryStream = new();
            string file = Path.Combine(
                rootPath,
                "Resources\\Template\\AttendanceMaintenance\\5_2_24_ResignationAnnualLeaveWorktypeAnalysisReport\\Download.xlsx"
            );
            WorkbookDesigner obj = new()
            {
                Workbook = new Workbook(file)
            };
            foreach (var item in param.Permission_Group)
            {
                var updatedItem = listPermissionGroup.FirstOrDefault(x => x.Key == item).Value;
                updatedPermissionGroup.Add(updatedItem);
            }
            Worksheet worksheet = obj.Workbook.Worksheets[0];

            worksheet.Cells["B2"].PutValue(param.Factory);
            worksheet.Cells["D2"].PutValue(param.DateStart.ToString("yyyy/MM/dd"));
            worksheet.Cells["F2"].PutValue(param.DateEnd.ToString("yyyy/MM/dd"));
            worksheet.Cells["K2"].PutValue(string.Join(",", updatedPermissionGroup));
            worksheet.Cells["N2"].PutValue(listlevel.FirstOrDefault(x => x.Key == param.Level).Value);
            worksheet.Cells["B3"].PutValue(userName);
            worksheet.Cells["D3"].PutValue(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

            worksheet.Cells["N5"].PutValue("職務/工種分析未滿一年");
            worksheet.Cells.CreateRange(4, 13, 1, data[0].WorkTypeList.Count + 1).SetStyle(GetStyle(obj, 255, 242, 204));

            worksheet.Cells[4, data[0].WorkTypeList.Count + 14].PutValue("滿一年以上離職總人數(工種)");
            worksheet.Cells.CreateRange(4, 13 + data[0].WorkTypeList.Count, 1, data[0].WorkTypeListThan1.Count + 2).SetStyle(GetStyle(obj, 226, 239, 218));

            // Merge Columns
            worksheet.Cells.Merge(4, 13, 1, data[0].WorkTypeList.Count + 1);
            worksheet.Cells.Merge(4, 14 + data[0].WorkTypeList.Count, 1, data[0].WorkTypeListThan1.Count + 1);

            worksheet.Cells[5, data[0].WorkTypeList.Count + 14].PutValue("滿一年以上離職總人數");
            worksheet.Cells[5, data[0].WorkTypeList.Count + 14].SetStyle(GetStyle(obj, 226, 239, 218));

            var WorkTypeList = Math.Max(data[0].WorkTypeList.Count, data[0].WorkTypeListThan1.Count);

            for (int i = 0; i < WorkTypeList; i++)
            {
                if (i < data[0].WorkTypeList.Count)
                {
                    worksheet.Cells[5, i + 14].PutValue(data[0].WorkTypeList[i].WorkTypeName);
                    worksheet.Cells[5, i + 14].SetStyle(GetStyle(obj, 255, 242, 204));
                }

                if (i < data[0].WorkTypeListThan1.Count)
                {
                    worksheet.Cells[5, i + data[0].WorkTypeList.Count + 15].PutValue(data[0].WorkTypeListThan1[i].WorkTypeName);
                    worksheet.Cells[5, i + data[0].WorkTypeList.Count + 15].SetStyle(GetStyle(obj, 226, 239, 218));
                }
            }
            for (int i = 0; i < data.Count; i++)
            {
                worksheet.Cells["A" + (i + 7)].PutValue(data[i].Department);
                worksheet.Cells["B" + (i + 7)].PutValue(data[i].DepartmentName);
                worksheet.Cells["C" + (i + 7)].PutValue(data[i].w_outs);
                worksheet.Cells["D" + (i + 7)].PutValue(data[i].w_news);
                worksheet.Cells["E" + (i + 7)].PutValue(data[i].w_y01);
                worksheet.Cells["F" + (i + 7)].PutValue(data[i].w_y12);
                worksheet.Cells["G" + (i + 7)].PutValue(data[i].w_y23);
                worksheet.Cells["H" + (i + 7)].PutValue(data[i].w_y34);
                worksheet.Cells["I" + (i + 7)].PutValue(data[i].w_y45);
                worksheet.Cells["J" + (i + 7)].PutValue(data[i].w_y56);
                worksheet.Cells["K" + (i + 7)].PutValue(data[i].w_y6);
                worksheet.Cells["L" + (i + 7)].PutValue(data[i].w_all);
                worksheet.Cells["M" + (i + 7)].PutValue(data[i].w_mh);
                worksheet.Cells["N" + (i + 7)].PutValue(data[i].w_n1);
                worksheet.Cells[i + 6, data[0].WorkTypeList.Count + 14].PutValue(data[i].w_m1);

                int columnIndex = 14;
                for (int j = 0; j < WorkTypeList; j++)
                {
                    if (j < data[i].WorkTypeList.Count)
                        worksheet.Cells[i + 6, columnIndex].PutValue(data[i].WorkTypeList[j].Value);
                    if (j < data[i].WorkTypeListThan1.Count)
                        worksheet.Cells[i + 6, columnIndex + data[i].WorkTypeList.Count + 1].PutValue(data[i].WorkTypeListThan1[j].Value);
                    columnIndex++;
                }
            }
            var totalRowPos = worksheet.Cells.MaxRow + 2;
            Style totalStyle = obj.Workbook.CreateStyle();
            totalStyle.IsTextWrapped = true;
            worksheet.Cells["B" + totalRowPos].Value = "合計:\nTotal:";
            worksheet.Cells["B" + totalRowPos].SetStyle(totalStyle);
            for (int i = 2; i <= worksheet.Cells.MaxDataColumn; i++)
            {
                var cellPos = MyRegex().Replace(worksheet.Cells[5, i].Name, "");
                worksheet.Cells[cellPos + totalRowPos].Formula = "=SUM(" + cellPos + (totalRowPos - data.Count) + ":" + cellPos + (totalRowPos - 1) + ")";
            }
            CellArea area = new()
            {
                StartRow = 1,
                StartColumn = 8,
                EndRow = 6,
                EndColumn = data[0].WorkTypeList.Count + data[0].WorkTypeListThan1.Count + 14
            };
            worksheet.AutoFitColumns(area.StartColumn, area.EndColumn);
            worksheet.AutoFitRows(area.StartRow, area.EndRow);

            obj.Workbook.Save(memoryStream, SaveFormat.Xlsx);
            return new OperationResult(true, new { TotalRows = data.Count, Excel = memoryStream.ToArray() });
        }

        public async Task<List<KeyValuePair<string, string>>> GetListLevel(string language) => await GetDataBasicCode(BasicCodeTypeConstant.Level, language);

        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName)
        {
            var factoryList = await Queryt_Factory_AddList(userName);

            var query = _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory && factoryList.Contains(x.Code), true);

            var data = await query
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language
                    .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory && x.Language_Code.ToLower() == language.ToLower(), true),
                    code => code.Code,
                    lang => lang.Code,
                    (code, lang) => new { Code = code, Lang = lang })
                .SelectMany(
                    x => x.Lang.DefaultIfEmpty(),
                    (x, lang) => new { x.Code, Lang = lang })
                .Select(x => new KeyValuePair<string, string>(x.Code.Code, $"{x.Code.Code} - {x.Lang.Code_Name ?? x.Code.Code_Name}"))
                .Distinct()
                .ToListAsync();

            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string Language)
        {
            return await Query_BasicCode_PermissionGroup(factory, Language);
        }

        public async Task<int> Search(ResignationAnnualLeaveWorktypeAnalysisReportParam param)
        {
            var result = await Getdata(param);

            return result.Count;
        }
        public async Task<List<ResignationAnnualLeaveWorktypeAnalysisReportExcelResult>> Getdata(ResignationAnnualLeaveWorktypeAnalysisReportParam param)
        {
            DateTime StartDate = param.DateStart;
            DateTime EndDate = param.DateEnd;

            // Local_Permission_list
            List<string> Local_Permission_list = await _repositoryAccessor.HRMS_Emp_Permission_Group.FindAll(x =>
                param.Permission_Group.Contains(x.Permission_Group) &&
                x.Factory == param.Factory &&
                x.Foreign_Flag == "N")
                .Select(x => x.Permission_Group)
                .ToListAsync();

            var preHEP = PredicateBuilder.New<HRMS_Emp_Personal>(x =>
                x.Factory == param.Factory &&
                x.Resign_Date.HasValue &&
                x.Resign_Date.Value.Date >= StartDate.Date &&
                x.Resign_Date.Value.Date <= EndDate.Date &&
                Local_Permission_list.Contains(x.Permission_Group));

            var HEP = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(preHEP, true).ToListAsync();
            if (!HEP.Any())
                return new();

            var HOD = await _repositoryAccessor.HRMS_Org_Department.FindAll(x =>
                x.Factory == param.Factory &&
                x.IsActive, true).ToListAsync();
            var HECT = await _repositoryAccessor.HRMS_Emp_Contract_Type.FindAll(x =>
                x.Probationary_Period == false).ToListAsync();
            var HCM = await _repositoryAccessor.HRMS_Emp_Contract_Management.FindAll(true).ToListAsync();
            var _HEP = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(x => x.Factory == param.Factory && Local_Permission_list.Contains(x.Permission_Group)).ToListAsync();

            var HECT_HCM_HEP = HECT
                .Join(HCM,
                    x => new { x.Division, x.Factory, x.Contract_Type },
                    y => new { y.Division, y.Factory, y.Contract_Type },
                    (hect, hcm) => new { hect, hcm })
                .Join(_HEP,
                    x => new { x.hcm.Employee_ID, x.hcm.Division, x.hcm.Factory },
                    y => new { y.Employee_ID, y.Division, y.Factory },
                    (x, hep) => new { x.hect, x.hcm, hep })
                .Select(x => x.hep.Employee_ID)
                .Distinct()
                .ToList();

            // qry
            HEP = HEP.Join(HOD,
                x => new { x.Division, x.Factory, Department_Code = x.Department },
                y => new { y.Division, y.Factory, y.Department_Code },
                (x, y) => new { hep = x, hod = y })
                .Select(x => x.hep).ToList();

            var HBC = GetListWorkType(param.Lang);

            var HES = await _repositoryAccessor.HRMS_Emp_Skill.FindAll(x => x.Factory == param.Factory).ToListAsync();

            var departmentByLevel = GetDepartmentByLevel(param.Level, HOD);

            var deptLang = HOD
                .GroupJoin(
                    _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x =>
                        x.Factory == param.Factory &&
                        x.Language_Code.ToLower() == param.Lang.ToLower()),
                    x => new { x.Division, x.Factory, x.Department_Code },
                    y => new { y.Division, y.Factory, y.Department_Code },
                    (x, y) => new { dept = x, hodl = y })
                .SelectMany(x => x.hodl.DefaultIfEmpty(),
                    (x, y) => new { x.dept, hodl = y })
                .Select(x => new
                {
                    Code = x.dept.Department_Code,
                    Name = x.hodl != null ? x.hodl.Name : x.dept.Department_Name
                }).Distinct().ToHashSet();

            List<ResignationAnnualLeaveWorktypeAnalysisReportExcelResult> result = new();

            var workTypeList = HEP.Select(x => new WorkTypeList
            {
                WorkType = x.Work_Type,
                Year = (x.Resign_Date.Value.Date - x.Onboard_Date).Days / 365,
                WorkTypeName = HBC.FirstOrDefault(h => h.Key == x.Work_Type).Value,
                Value = 0
            }).ToList();

            var WorkTypeList = workTypeList.DeepClone().Where(x => x.Year < 1).DistinctBy(x => x.WorkType);
            var WorkTypeListThan1 = workTypeList.DeepClone().Where(x => x.Year >= 1).DistinctBy(x => x.WorkType);

            foreach (var dept in departmentByLevel)
            {
                var deptList = GetDepartmentHierarchy(dept.Department_Code, HOD);

                // Danh sách nhân viên theo phòng ban
                List<HRMS_Emp_Personal> listEmpByDepartment = HEP.FindAll(x =>
                    deptList.Any(d => d.Department_Code == x.Department));

                int w_outs = listEmpByDepartment.Count;
                if (w_outs == 0)
                    continue;
                int w_news = listEmpByDepartment.Count(x => !HECT_HCM_HEP.Contains(x.Employee_ID));

                int w_y12 = 0;
                int w_y23 = 0;
                int w_y34 = 0;
                int w_y45 = 0;
                int w_y56 = 0;
                int w_all = 0;
                int w_mh = 0;
                int w_n1 = 0;
                int w_m1 = 0;
                int w_y6 = 0;
                int w_y01 = 0;
                var _WorkTypeList = WorkTypeList.DeepClone();
                var _WorkTypeListThan1 = WorkTypeListThan1.DeepClone();

                foreach (var PH in listEmpByDepartment)
                {
                    decimal year = (decimal)(PH.Resign_Date.Value.Date - PH.Onboard_Date.Date).Days / 365;
                    var skill = HES.Where(x =>
                        x.Division == PH.Division &&
                        x.Employee_ID == PH.Employee_ID);

                    if (year < 1)
                    {
                        w_n1++;
                        var workType = _WorkTypeList.FirstOrDefault(x => x.WorkType == PH.Work_Type);
                        if (workType != null) workType.Value++;
                        if (HECT_HCM_HEP.Contains(PH.Employee_ID))
                            w_y01++;
                    }
                    else if (year >= 1 && year <= 2)
                    {
                        w_y12++;
                        var workType = _WorkTypeListThan1.FirstOrDefault(x => x.WorkType == PH.Work_Type);
                        if (workType != null) workType.Value++;
                    }
                    else if (year > 2 && year <= 3)
                    {
                        w_y23++;
                        var workType = _WorkTypeListThan1.FirstOrDefault(x => x.WorkType == PH.Work_Type);
                        if (workType != null) workType.Value++;
                    }
                    else if (year > 3 && year <= 4)
                    {
                        w_y34++;
                        var workType = _WorkTypeListThan1.FirstOrDefault(x => x.WorkType == PH.Work_Type);
                        if (workType != null) workType.Value++;
                    }
                    else if (year > 4 && year <= 5)
                    {
                        w_y45++;
                        var workType = _WorkTypeListThan1.FirstOrDefault(x => x.WorkType == PH.Work_Type);
                        if (workType != null) workType.Value++;
                    }
                    else if (year > 5 && year <= 6)
                    {
                        w_y56++;
                        var workType = _WorkTypeListThan1.FirstOrDefault(x => x.WorkType == PH.Work_Type);
                        if (workType != null) workType.Value++;
                    }
                    else if (year > 6)
                    {
                        w_y6++;
                        var workType = _WorkTypeListThan1.FirstOrDefault(x => x.WorkType == PH.Work_Type);
                        if (workType != null) workType.Value++;
                    }

                    if (skill.Any(x => x.Skill_Certification == "01"))
                        w_mh++;

                    if (skill.Any(x => x.Skill_Certification == "02"))
                        w_all++;
                }

                w_m1 = w_y12 + w_y23 + w_y34 + w_y45 + w_y56 + w_y6;
                result.Add(new ResignationAnnualLeaveWorktypeAnalysisReportExcelResult
                {
                    Department = deptLang.FirstOrDefault(x => x.Code == dept.Department_Code)?.Code,
                    DepartmentName = deptLang.FirstOrDefault(x => x.Code == dept.Department_Code)?.Name,
                    w_outs = w_outs,
                    w_y12 = w_y12,
                    w_y23 = w_y23,
                    w_y34 = w_y34,
                    w_y45 = w_y45,
                    w_y56 = w_y56,
                    w_y6 = w_y6,
                    w_all = w_all,
                    w_mh = w_mh,
                    w_n1 = w_n1,
                    w_m1 = w_m1,
                    w_news = w_news,
                    w_y01 = w_y01,
                    WorkTypeList = _WorkTypeList.ToList(),
                    WorkTypeListThan1 = _WorkTypeListThan1.ToList()
                });
            }

            var workTypeList1 = WorkTypeList.Where(x =>
                result.Any(y => y.WorkTypeList.FirstOrDefault(z => z.WorkType == x.WorkType)?.Value > 0));

            var workTypeListThan1 = WorkTypeListThan1.Where(x =>
                result.Any(y => y.WorkTypeListThan1.FirstOrDefault(z => z.WorkType == x.WorkType)?.Value > 0));

            result.ForEach(x =>
            {
                x.WorkTypeList = x.WorkTypeList.Where(wt =>
                    workTypeList1.Any(y => y.WorkType == wt.WorkType)).ToList();

                x.WorkTypeListThan1 = x.WorkTypeListThan1.Where(wt =>
                    workTypeListThan1.Any(y => y.WorkType == wt.WorkType)).ToList();
            });

            return result.OrderBy(x => x.Department).ToList();
        }

        private List<HRMS_Org_Department> GetDepartmentByLevel(string level, List<HRMS_Org_Department> departments)
        {
            if (departments.Count == 0 || !int.TryParse(level, out int levelInt))
                return new List<HRMS_Org_Department>();
            var result = departments.Where(x =>
                int.TryParse(x.Org_Level, out int orgLevelInt) &&
                (orgLevelInt == levelInt ||
                (orgLevelInt >= levelInt && string.IsNullOrWhiteSpace(x.Upper_Department)) ||
                (orgLevelInt > levelInt && departments.Any(y => int.TryParse(y.Org_Level, out int yOrgLevelInt) && yOrgLevelInt < levelInt && y.Department_Code == x.Upper_Department)))
            ).ToList();
            return result;
        }
        private List<HRMS_Org_Department> GetDepartmentHierarchy(string department, List<HRMS_Org_Department> departments)
        {
            List<HRMS_Org_Department> result = new();
            var rootDepartment = departments.FirstOrDefault(x => x.Department_Code == department);
            if (rootDepartment == null) return result;
            result.Add(rootDepartment);
            var addedDepartments = new HashSet<string> { department };
            var queue = new Queue<string>();
            queue.Enqueue(department);
            while (queue.Count > 0)
            {
                var currentDepartmentCode = queue.Dequeue();
                var childDepartments = departments.Where(x => x.Upper_Department == currentDepartmentCode && !addedDepartments.Contains(x.Department_Code)).ToList();
                foreach (var child in childDepartments)
                {
                    result.Add(child);
                    addedDepartments.Add(child.Department_Code);
                    queue.Enqueue(child.Department_Code);
                }
            }
            return result;
        }
        private List<KeyValuePair<string, string>> GetListWorkType(string language)
        {
            return _repositoryAccessor.HRMS_Basic_Code
                   .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.WorkType, true)
                   .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                       HBC => new { HBC.Type_Seq, HBC.Code },
                       HBCL => new { HBCL.Type_Seq, HBCL.Code },
                       (HBC, HBCL) => new { HBC, HBCL })
                       .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                       (prev, HBCL) => new { prev.HBC, HBCL })
                   .Select(x => new KeyValuePair<string, string>(x.HBC.Code, $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"))
                   .ToList();
        }
        private Style GetStyle(WorkbookDesigner obj, int color1, int color2, int color3)
        {
            Style style = obj.Workbook.CreateStyle();
            style.ForegroundColor = Color.FromArgb(color1, color2, color3);
            style.Pattern = BackgroundType.Solid;
            style.IsTextWrapped = true;
            style.HorizontalAlignment = TextAlignmentType.Center;
            style.VerticalAlignment = TextAlignmentType.Center;
            style = AsposeUtility.SetAllBorders(style);
            return style;
        }
    }
}