using AgileObjects.AgileMapper;
using API.Data;
using API._Services.Interfaces.OrganizationManagement;
using API.DTOs.OrganizationManagement;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.OrganizationManagement
{
    public class S_3_1_3_OrganizationChart : BaseServices, I_3_1_3_OrganizationChart
    {
        public S_3_1_3_OrganizationChart(DBContext dbContext) : base(dbContext)
        {
        }
        public async Task<List<KeyValuePair<string, string>>> GetDropDownList(OrganizationChartParam param)
        {
            var HBC = await _repositoryAccessor.HRMS_Basic_Code.FindAll().ToListAsync();
            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.lang.ToLower()).ToList();
            var result = new List<KeyValuePair<string, string>>();
            var data = HBC.GroupJoin(HBCL,
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (x, y) => new { hbc = x, hbcl = y })
                    .SelectMany(x => x.hbcl.DefaultIfEmpty(),
                    (x, y) => new { x.hbc, hbcl = y });
            result.AddRange(data.Where(x => x.hbc.Type_Seq == "1").Select(x => new KeyValuePair<string, string>("D", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList()); // Division
            result.AddRange(data.Where(x => x.hbc.Type_Seq == "8").Select(x => new KeyValuePair<string, string>("L", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList()); // Level
            if (!string.IsNullOrWhiteSpace(param.division))
            {
                var HBFC = _repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(x => x.Division == param.division && x.Kind == "1").ToList();
                if (!HBFC.Any())
                    result.AddRange(data.Where(x => x.hbc.Type_Seq == "2").Select(x => new KeyValuePair<string, string>("F", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList()); // Factory
                else
                {
                    var dataFilter = data.Join(HBFC,
                        x => new { Factory = x.hbc.Code, x.hbc.Type_Seq },
                        y => new { y.Factory, Type_Seq = "2" },
                        (x, y) => new { x.hbc, x.hbcl, hbfc = y });
                    result.AddRange(dataFilter.Where(x => x.hbc.Type_Seq == "2").Select(x => new KeyValuePair<string, string>("F", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList()); // Factory
                }
            }
            return result;
        }
        public async Task<List<KeyValuePair<string, string>>> GetDepartmentList(OrganizationChartParam param)
        {
            var HOD = await _repositoryAccessor.HRMS_Org_Department
                .FindAll(x =>
                    x.Factory == param.factory &&
                    x.Division == param.division)
                .OrderBy(x => x.Department_Code).ToListAsync();
            var HODL = _repositoryAccessor.HRMS_Org_Department_Language
                .FindAll(x =>
                    x.Factory == param.factory &&
                    x.Division == param.division &&
                    x.Language_Code.ToLower() == param.lang.ToLower())
                .ToList();
            var data = HOD.GroupJoin(HODL,
                    x => x.Department_Code,
                    y => y.Department_Code,
                    (x, y) => new { hod = x, hodl = y })
                    .SelectMany(x => x.hodl.DefaultIfEmpty(),
                    (x, y) => new { x.hod, hodl = y });
            List<KeyValuePair<string, string>> result = data.Select(x => new KeyValuePair<string, string>(x.hod.Department_Code, $"{x.hod.Department_Code}-{(x.hodl != null ? x.hodl.Name : x.hod.Department_Name)}")).Distinct().ToList();
            return result;
        }
        public async Task<List<INodeDtoFinal>> GetChartData(OrganizationChartParam param)
        {
            try
            {
                var predicate = PredicateBuilder.New<HRMS_Org_Department>(x => x.IsActive);
                var predicateLang = PredicateBuilder.New<HRMS_Org_Department_Language>(x => x.Language_Code.ToLower() == param.lang.ToLower());
                if (!string.IsNullOrWhiteSpace(param.division))
                {
                    predicate.And(x => x.Division.Trim().ToLower() == param.division.Trim().ToLower());
                    predicateLang.And(x => x.Division.Trim().ToLower() == param.division.Trim().ToLower());
                }
                if (!string.IsNullOrWhiteSpace(param.factory))
                {
                    predicate.And(x => x.Factory.Trim().ToLower() == param.factory.Trim().ToLower());
                    predicateLang.And(x => x.Factory.Trim().ToLower() == param.factory.Trim().ToLower());
                }
                List<HRMS_Org_Department> dataList = await _repositoryAccessor.HRMS_Org_Department.FindAll(predicate).ToListAsync();
                List<HRMS_Org_Department_Language> deplLangList = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(predicateLang).ToList();
                dataList.ForEach(data =>
                {
                    var deptLang = deplLangList.FirstOrDefault(x => x.Department_Code == data.Department_Code);
                    if (deptLang != null)
                        data.Department_Name = deptLang.Name;
                });
                List<INodeDto> sortedDataList = GetSortedData(dataList, param.lang);
                sortedDataList = GetRootDept(sortedDataList);
                if (!string.IsNullOrWhiteSpace(param.department))
                    sortedDataList = GetSelectedDataList(sortedDataList, param.department);
                List<INodeDto> data = GetOrgChartFormatData(sortedDataList);
                List<INodeDtoFinal> result = Mapper.Map(data).ToANew<List<INodeDtoFinal>>(x => x.MapEntityKeys());
                if (!string.IsNullOrWhiteSpace(param.factory))
                    result = FilterDataByLevel(result, Convert.ToInt32(param.level));
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<OperationResult> DownloadExcel(OrganizationChartParam param)
        {
            var predicate0 = PredicateBuilder.New<HRMS_Org_Department>(true);
            if (!string.IsNullOrWhiteSpace(param.division))
                predicate0.And(x => x.Division == param.division);
            if (!string.IsNullOrWhiteSpace(param.factory))
                predicate0.And(x => x.Factory == param.factory);
            List<HRMS_Org_Department> dataList = await _repositoryAccessor.HRMS_Org_Department.FindAll(predicate0).ToListAsync();
            List<string> rootList = dataList.Where(x => string.IsNullOrWhiteSpace(x.Upper_Department) && string.IsNullOrWhiteSpace(x.Virtual_Department)).Select(x => x.Department_Code.Trim().ToLower()).ToList();
            var lookup = dataList.ToLookup(x => x.Attribute == "Non-Directly" ? x.Virtual_Department : x.Upper_Department);
            IEnumerable<HRMS_Org_Department> build(string pid, int level) => lookup[pid].SelectMany(x => new[] { x }.Concat(build(x.Department_Code, level + 1)));
            IEnumerable<HRMS_Org_Department> sortedDatas = build(null, 0);
            List<HRMS_Org_Department> resultList = sortedDatas.Select(x => new HRMS_Org_Department()
            {
                Org_Level = x.Org_Level,
                Department_Code = x.Department_Code,
                Department_Name = x.Department_Name,
                Upper_Department = x.Attribute == "Non-Directly" ? x.Virtual_Department : x.Upper_Department
            }).ToList();
            if (!string.IsNullOrWhiteSpace(param.department))
            {
                List<HRMS_Org_Department> result = new();
                bool rootPicked = false;
                var tempList = dataList.ToDictionary(node => node.Department_Code);
                foreach (var node in resultList)
                {
                    if (!rootPicked && node.Department_Code == param.department)
                    {
                        node.Upper_Department = null;
                        result.Add(node);
                        rootPicked = true;
                        continue;
                    }
                    if (rootPicked)
                    {
                        if (string.IsNullOrWhiteSpace(node.Upper_Department))
                            break;
                        if (tempList.TryGetValue(node.Upper_Department, out HRMS_Org_Department parent))
                        {
                            if (result.Any(x => x.Department_Code == parent.Department_Code))
                                result.Add(node);
                            else
                                break;
                        }
                        else
                            break;
                    }
                }
                resultList = result;
            }
            resultList = resultList.Where(x => Convert.ToInt32(x.Org_Level) <= Convert.ToInt32(param.level)).ToList();
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                resultList, 
                "Resources\\Template\\OrganizationManagement\\3_1_3_OrganizationChart\\Download.xlsx"
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }
        #region Organization Chart Functions
        private static List<INodeDto> GetSortedData(List<HRMS_Org_Department> dataList, string lang)
        {
            List<string> rootList = dataList.Where(x => string.IsNullOrWhiteSpace(x.Upper_Department) && string.IsNullOrWhiteSpace(x.Virtual_Department)).Select(x => x.Department_Code.Trim().ToLower()).ToList();
            var lookup = dataList.ToLookup(x => x.Attribute == "Non-Directly" ? x.Virtual_Department : x.Upper_Department);
            IEnumerable<HRMS_Org_Department> build(string pid, int level) => lookup[pid].SelectMany(x => new[] { x }.Concat(build(x.Department_Code, level + 1)));
            IEnumerable<HRMS_Org_Department> sortedDatas = build(null, 0);
            List<INodeDto> result = sortedDatas.Select(x => new INodeDto()
            {
                department = x.Department_Code,
                factory = x.Factory,
                division = x.Division,
                parent_Code = x.Attribute == "Non-Directly" ? x.Virtual_Department : x.Upper_Department,
                name = x.Department_Name,
                level = x.Org_Level,
                approved_Headcount = x.Approved_Headcount ?? 0,
                actual_Headcount = 0,
                supervisor_Employee_ID = x.Supervisor_Employee_ID,
                supervisor_Type =
                    x.Supervisor_Type == "B" ? lang == "en" ? "B.Deputy" : "B.代理" :
                    x.Supervisor_Type == "C" ? lang == "en" ? "C.Adjunction" : "C.兼任" :
                    x.Supervisor_Type == "D" ? lang == "en" ? "D.Informal" : "D.非正式" : null,
                cssClass = x.Virtual_Department == null && x.Upper_Department == null
                        ? "layer-0"
                        : x.Attribute == "Staff"
                            ? "layer-1"
                            : x.Attribute == "Directly"
                                ? "layer-2"
                                : "layer-3",
                image = "",
                is_dept_Layer = (x.Attribute == "Non-Directly" ?
                x.Virtual_Department : x.Upper_Department) != null && rootList.Contains((x.Attribute == "Non-Directly" ?
                x.Virtual_Department : x.Upper_Department).Trim().ToLower()),
                childs = new List<INodeDto>()
            }).ToList();
            return result;
        }
        private static List<INodeDto> GetRootDept(List<INodeDto> dataList)
        {
            string tempRoot = "";
            foreach (var node in dataList)
            {
                if (string.IsNullOrWhiteSpace(node.parent_Code))
                    tempRoot = node.department;
                node.root_Code = tempRoot;
            }
            return dataList;
        }
        private static List<INodeDto> GetOrgChartFormatData(List<INodeDto> dataList)
        {
            List<INodeDto> result = new();
            var nodeMap = dataList.ToDictionary(node => node.department);
            foreach (var node in dataList)
            {
                if (string.IsNullOrWhiteSpace(node.parent_Code))
                    result.Add(node);
                else
                {
                    if (nodeMap.TryGetValue(node.parent_Code, out INodeDto parent))
                        parent.childs.Add(node);
                    else
                        result.Add(node);
                }
            }
            result.ForEach(x => { x.childs = x.childs.OrderBy(x => x.cssClass).ToList(); });
            return result;
        }
        private static List<INodeDto> GetSelectedDataList(List<INodeDto> dataList, string selectedDepartment)
        {
            List<INodeDto> result = new();
            bool rootPicked = false;
            var tempList = dataList.ToDictionary(node => node.department);
            foreach (var node in dataList)
            {
                if (!rootPicked && node.department == selectedDepartment)
                {
                    node.parent_Code = null;
                    result.Add(node);
                    rootPicked = true;
                    continue;
                }
                if (rootPicked)
                {
                    if (string.IsNullOrWhiteSpace(node.parent_Code))
                        break;
                    if (tempList.TryGetValue(node.parent_Code, out INodeDto parent))
                    {
                        if (result.Any(x => x.department == parent.department))
                            result.Add(node);
                        else
                            break;
                    }
                    else
                        break;
                }
            }
            return result;
        }
        private static List<INodeDtoFinal> FilterDataByLevel(List<INodeDtoFinal> data, int level)
        {
            List<INodeDtoFinal> removeList = new();
            for (int i = 0; i < data.Count; i++)
            {
                INodeDtoFinal temp = data[i];
                if (Convert.ToInt32(temp.level) > level)
                {
                    removeList.Add(temp);
                    continue;
                }
                if (temp.childs.Any())
                    data[i].childs = FilterDataByLevel(data[i].childs, level);
            }
            List<INodeDtoFinal> result = data.Where(x => removeList.FindIndex(a => a == x) == -1).ToList();
            return result;
        }
        #endregion
    }
}