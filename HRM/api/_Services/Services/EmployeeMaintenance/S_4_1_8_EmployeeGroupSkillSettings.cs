using API.Data;
using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.EmployeeMaintenance
{
    public class S_4_1_8_EmployeeGroupSkillSettings : BaseServices, I_4_1_8_EmployeeGroupSkillSettings
    {
        public S_4_1_8_EmployeeGroupSkillSettings(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<KeyValuePair<string, string>>> GetDropDownList(EmployeeGroupSkillSettings_Param param)
        {
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
            result.AddRange(data.Where(x => x.hbc.Type_Seq == "24").Select(x => new KeyValuePair<string, string>("PR", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            result.AddRange(data.Where(x => x.hbc.Type_Seq == "26").Select(x => new KeyValuePair<string, string>("PE", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            result.AddRange(data.Where(x => x.hbc.Type_Seq == "25").Select(x => new KeyValuePair<string, string>("TE", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            result.AddRange(data.Where(x => x.hbc.Type_Seq == "27").Select(x => new KeyValuePair<string, string>("EX", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            result.AddRange(data.Where(x => x.hbc.Type_Seq == "28").Select(x => new KeyValuePair<string, string>("SK", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            if (!string.IsNullOrWhiteSpace(param.Division))
            {
                var HBFC = _repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(x => x.Division == param.Division && x.Kind == "1").ToList();
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

        public async Task<List<KeyValuePair<string, string>>> GetEmployeeList(EmployeeGroupSkillSettings_Param param)
        {
            var data = await _repositoryAccessor.HRMS_Emp_Personal
                .FindAll(x => x.Factory == param.Factory && x.Employee_ID.Contains(param.Employee_Id))
                .Select(x => new KeyValuePair<string, string>(x.Employee_ID, x.Local_Full_Name))
                .ToListAsync();
            return data;
        }

        public async Task<PaginationUtility<EmployeeGroupSkillSettings_Main>> GetSearchDetail(PaginationParam paginationParams, EmployeeGroupSkillSettings_Param searchParam, List<string> roleList)
        {
            var predicateGroup = PredicateBuilder.New<HRMS_Emp_Group>(true);
            var predicateSkill = PredicateBuilder.New<HRMS_Emp_Skill>(true);

            if (!string.IsNullOrWhiteSpace(searchParam.Division))
            {
                predicateGroup.And(x => x.Division == searchParam.Division);
                predicateSkill.And(x => x.Division == searchParam.Division);
            }
            if (!string.IsNullOrWhiteSpace(searchParam.Factory))
            {
                predicateGroup.And(x => x.Factory == searchParam.Factory);
                predicateSkill.And(x => x.Factory == searchParam.Factory);
            }
            if (!string.IsNullOrWhiteSpace(searchParam.Employee_Id))
            {
                predicateGroup.And(x => x.Employee_ID.ToLower() == searchParam.Employee_Id.ToLower().Trim());
                predicateSkill.And(x => x.Employee_ID.ToLower() == searchParam.Employee_Id.ToLower().Trim());
            }
            if (!string.IsNullOrWhiteSpace(searchParam.Production_Line))
                predicateGroup.And(x => x.Production_Line == searchParam.Production_Line);
            if (!string.IsNullOrWhiteSpace(searchParam.Performance_Category))
                predicateGroup.And(x => x.Performance_Category == searchParam.Performance_Category);
            if (!string.IsNullOrWhiteSpace(searchParam.Technical_Type))
                predicateGroup.And(x => x.Technical_Type == searchParam.Technical_Type);
            if (!string.IsNullOrWhiteSpace(searchParam.Expertise_Category))
                predicateGroup.And(x => x.Expertise_Category == searchParam.Expertise_Category);
            var HEG = _repositoryAccessor.HRMS_Emp_Group.FindAll(predicateGroup).ToList();
            var HES = _repositoryAccessor.HRMS_Emp_Skill.FindAll(predicateSkill).ToList();
            var HEP = await Query_Permission_Data_Filter(roleList);
            var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll();
            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == searchParam.Lang.ToLower());
            var codeLang = HBC.GroupJoin(HBCL,
                x => new { x.Type_Seq, x.Code },
                y => new { y.Type_Seq, y.Code },
                    (x, y) => new { hbc = x, hbcl = y })
                .SelectMany(x => x.hbcl.DefaultIfEmpty(),
                    (x, y) => new { x.hbc, hbcl = y })
                .Select(x => new
                {
                    x.hbc.Code,
                    x.hbc.Type_Seq,
                    Code_Name = x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name
                }).Distinct();
            var data = HEG
                .GroupJoin(HES,
                    x => new { x.Division, x.Factory, x.Employee_ID },
                    y => new { y.Division, y.Factory, y.Employee_ID },
                    (x, y) => new { HEG = x, HES = y })
                .SelectMany(x => x.HES.DefaultIfEmpty(),
                    (x, y) => new { x.HEG, HES = y })
                .Join(HEP,
                    x => new { x.HEG.Division, x.HEG.Factory, x.HEG.Employee_ID },
                    y => new { y.Division, y.Factory, y.Employee_ID },
                    (x, y) => new { x.HEG, x.HES, HEP = y })
                .GroupBy(x => x.HEG);
            var result = data.Select(x => new EmployeeGroupSkillSettings_Main
            {
                Division = x.Key.Division,
                Factory = x.Key.Factory,
                Employee_Id = x.Key.Employee_ID,
                Local_Full_Name = x.FirstOrDefault(y => y.HEP != null).HEP.Local_Full_Name,
                Production_Line = x.Key.Production_Line,
                Production_Line_Name = codeLang.FirstOrDefault(y => y.Code == x.Key.Production_Line && y.Type_Seq == "24").Code_Name,
                Technical_Type = x.Key.Technical_Type,
                Technical_Type_Name = codeLang.FirstOrDefault(y => y.Code == x.Key.Technical_Type && y.Type_Seq == "25").Code_Name,
                Performance_Category = x.Key.Performance_Category,
                Performance_Category_Name = codeLang.FirstOrDefault(y => y.Code == x.Key.Performance_Category && y.Type_Seq == "26").Code_Name,
                Expertise_Category = x.Key.Expertise_Category,
                Expertise_Category_Name = codeLang.FirstOrDefault(y => y.Code == x.Key.Expertise_Category && y.Type_Seq == "27").Code_Name,
                Skill_Detail_List = x.Where(y => y.HES != null).OrderBy(y => y.HES.Seq).Select(y => new EmployeeGroupSkillSettings_SkillDetail
                {
                    Seq = y.HES.Seq.ToString(),
                    Skill_Certification = y.HES.Skill_Certification,
                    Passing_Date = y.HES.Passing_Date,
                    Passing_Date_Str = y.HES.Passing_Date.ToString("yyyy/MM/dd")
                }).GroupBy(x => new { x.Seq, x.Skill_Certification, x.Passing_Date, x.Passing_Date_Str }).Select(x => x.First()).ToList()
            })
            .OrderBy(x => x.Employee_Id)
            .ToList();
            if (searchParam.Skill_Array != null)
                result = result.Where(x => x.Skill_Detail_List.Any(y => searchParam.Skill_Array.Contains(y.Skill_Certification))).ToList();
            return PaginationUtility<EmployeeGroupSkillSettings_Main>.Create(result, paginationParams.PageNumber, paginationParams.PageSize);
        }
        
        public async Task<OperationResult> PostData(EmployeeGroupSkillSettings_Main data, string userName)
        {
            var predicateGroup = PredicateBuilder.New<HRMS_Emp_Group>(true);
            if (!string.IsNullOrWhiteSpace(data.Division))
                predicateGroup.And(x => x.Division == data.Division);
            if (!string.IsNullOrWhiteSpace(data.Factory))
                predicateGroup.And(x => x.Factory == data.Factory);
            if (!string.IsNullOrWhiteSpace(data.Employee_Id))
                predicateGroup.And(x => x.Employee_ID.ToLower() == data.Employee_Id.ToLower().Trim());
            if (await _repositoryAccessor.HRMS_Emp_Group.AnyAsync(predicateGroup))
                return new OperationResult(false, "AlreadyExitedGroup");
            HRMS_Emp_Group addGroup = new()
            {
                Division = data.Division,
                Factory = data.Factory,
                Employee_ID = data.Employee_Id,
                Production_Line = data.Production_Line,
                Technical_Type = data.Technical_Type,
                Performance_Category = data.Performance_Category,
                Expertise_Category = data.Expertise_Category,
                Update_By = userName,
                Update_Time = DateTime.Now
            };
            List<HRMS_Emp_Skill> addSkillList = new();
            foreach (EmployeeGroupSkillSettings_SkillDetail item in data.Skill_Detail_List)
            {
                HRMS_Emp_Skill addSkill = new()
                {
                    Division = data.Division,
                    Factory = data.Factory,
                    Employee_ID = data.Employee_Id,
                    Seq = Convert.ToInt32(item.Seq),
                    Skill_Certification = item.Skill_Certification,
                    Passing_Date = Convert.ToDateTime(item.Passing_Date_Str),
                    Update_By = userName,
                    Update_Time = DateTime.Now
                };
                addSkillList.Add(addSkill);
            }
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                var predicateSkill = PredicateBuilder.New<HRMS_Emp_Skill>(true);
                if (!string.IsNullOrWhiteSpace(data.Division))
                    predicateSkill.And(x => x.Division == data.Division);
                if (!string.IsNullOrWhiteSpace(data.Factory))
                    predicateSkill.And(x => x.Factory == data.Factory);
                if (!string.IsNullOrWhiteSpace(data.Employee_Id))
                    predicateSkill.And(x => x.Employee_ID.ToLower() == data.Employee_Id.ToLower().Trim());
                var removeSkillList = _repositoryAccessor.HRMS_Emp_Skill.FindAll(predicateSkill).ToList();
                if (removeSkillList != null)
                    _repositoryAccessor.HRMS_Emp_Skill.RemoveMultiple(removeSkillList);
                _repositoryAccessor.HRMS_Emp_Group.Add(addGroup);
                if (addSkillList.Any())
                    _repositoryAccessor.HRMS_Emp_Skill.AddMultiple(addSkillList);
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
        public async Task<OperationResult> PutData(EmployeeGroupSkillSettings_Main data, string userName)
        {
            var predicateGroup = PredicateBuilder.New<HRMS_Emp_Group>(true);
            if (!string.IsNullOrWhiteSpace(data.Division))
                predicateGroup.And(x => x.Division == data.Division);
            if (!string.IsNullOrWhiteSpace(data.Factory))
                predicateGroup.And(x => x.Factory == data.Factory);
            if (!string.IsNullOrWhiteSpace(data.Employee_Id))
                predicateGroup.And(x => x.Employee_ID.ToLower() == data.Employee_Id.ToLower().Trim());
            var groupData = await _repositoryAccessor.HRMS_Emp_Group.FirstOrDefaultAsync(predicateGroup);
            if (groupData == null)
                return new OperationResult(false, "NotExitedData");
            groupData.Production_Line = data.Production_Line;
            groupData.Technical_Type = data.Technical_Type;
            groupData.Performance_Category = data.Performance_Category;
            groupData.Expertise_Category = data.Expertise_Category;
            groupData.Update_By = userName;
            groupData.Update_Time = DateTime.Now;
            List<HRMS_Emp_Skill> addSkillList = new();
            foreach (EmployeeGroupSkillSettings_SkillDetail item in data.Skill_Detail_List)
            {
                HRMS_Emp_Skill addSkill = new()
                {
                    Division = data.Division,
                    Factory = data.Factory,
                    Employee_ID = data.Employee_Id,
                    Seq = Convert.ToInt32(item.Seq),
                    Skill_Certification = item.Skill_Certification,
                    Passing_Date = Convert.ToDateTime(item.Passing_Date_Str),
                    Update_By = userName,
                    Update_Time = DateTime.Now
                };
                addSkillList.Add(addSkill);
            }
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                var predicateSkill = PredicateBuilder.New<HRMS_Emp_Skill>(true);
                if (!string.IsNullOrWhiteSpace(data.Division))
                    predicateSkill.And(x => x.Division == data.Division);
                if (!string.IsNullOrWhiteSpace(data.Factory))
                    predicateSkill.And(x => x.Factory == data.Factory);
                if (!string.IsNullOrWhiteSpace(data.Employee_Id))
                    predicateSkill.And(x => x.Employee_ID.ToLower() == data.Employee_Id.ToLower().Trim());
                var removeSkillList = _repositoryAccessor.HRMS_Emp_Skill.FindAll(predicateSkill).ToList();
                if (removeSkillList != null)
                    _repositoryAccessor.HRMS_Emp_Skill.RemoveMultiple(removeSkillList);
                _repositoryAccessor.HRMS_Emp_Group.Update(groupData);
                if (addSkillList.Any())
                    _repositoryAccessor.HRMS_Emp_Skill.AddMultiple(addSkillList);
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
        public async Task<OperationResult> DeleteData(string division, string factory, string employee_Id)
        {
            if (_repositoryAccessor.HRMS_Emp_Skill.Any(x => x.Division == division && x.Factory == factory && x.Employee_ID == employee_Id))
                return new OperationResult(false, "SkillDetailRemain");
            if (!_repositoryAccessor.HRMS_Emp_Group.Any(x => x.Division == division && x.Factory == factory && x.Employee_ID == employee_Id))
                return new OperationResult(false, "NotExitedData");
            HRMS_Emp_Group removeGroup = await _repositoryAccessor.HRMS_Emp_Group.FirstOrDefaultAsync(x => x.Division == division && x.Factory == factory && x.Employee_ID == employee_Id);
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                _repositoryAccessor.HRMS_Emp_Group.Remove(removeGroup);
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

        public async Task<OperationResult> CheckExistedData(EmployeeGroupSkillSettings_Param param)
        {
            return new OperationResult(await _repositoryAccessor.HRMS_Emp_Group.AnyAsync(x => x.Division == param.Division && x.Factory == param.Factory && x.Employee_ID == param.Employee_Id));
        }
    }
}