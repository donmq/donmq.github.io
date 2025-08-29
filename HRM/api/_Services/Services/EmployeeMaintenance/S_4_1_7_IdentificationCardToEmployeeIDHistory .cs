using API.Data;
using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.EmployeeMaintenance
{
    public class S_4_1_7_IdentificationCardToEmployeeIDHistory : BaseServices, I_4_1_7_IdentificationCardToEmployeeIDHistory
    {
        public S_4_1_7_IdentificationCardToEmployeeIDHistory(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<PaginationUtility<HRMS_Emp_IDcard_EmpID_HistoryDto>> GetDataPagination(PaginationParam pagination, IdentificationCardToEmployeeIDHistoryParam param)
        {
            var pred = PredicateBuilder.New<HRMS_Emp_IDcard_EmpID_History>(x => x.Division == param.Division && x.Factory == param.Factory);

            if (!string.IsNullOrWhiteSpace(param.Employee_ID))
                pred.And(x => x.Employee_ID.Contains(param.Employee_ID.Trim()));
            if (!string.IsNullOrWhiteSpace(param.Nationality))
                pred.And(x => x.Nationality == param.Nationality);
            if (!string.IsNullOrWhiteSpace(param.Identification_Number))
                pred.And(x => x.Identification_Number == param.Identification_Number.Trim());

            var department = _repositoryAccessor.HRMS_Org_Department.FindAll(true);
            var departmentLanguage = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(language => language.Language_Code.ToLower() == param.Lang.ToLower(), true);
            var codeLang = department
            .GroupJoin(departmentLanguage,
                x => new { x.Division, x.Factory, x.Department_Code },
                y => new { y.Division, y.Factory, y.Department_Code },
                (x, y) => new { Code = x, Language = y })
            .SelectMany(x => x.Language.DefaultIfEmpty(),
                (x, y) => new { x.Code, Language = y })
            .Select(x => new
            {
                x.Code.Division,
                x.Code.Factory,
                x.Code.Department_Code,
                Code_Name = x.Language != null ? x.Language.Name : x.Code.Department_Name
            }).Distinct();

            var data = await _repositoryAccessor.HRMS_Emp_IDcard_EmpID_History.FindAll(pred, true)
            .GroupJoin(
                _repositoryAccessor.HRMS_Emp_Personal.FindAll(true),
                idCard => idCard.USER_GUID,
                personal => personal.USER_GUID,
                (idCard, personal) => new { IDCard = idCard, Personal = personal }
            )
            .SelectMany(
                x => x.Personal.DefaultIfEmpty(),
                (idCard, personal) => new { idCard.IDCard, Personal = personal })
            .Select(
                x => new HRMS_Emp_IDcard_EmpID_HistoryDto
                {
                    Nationality = x.IDCard.Nationality,
                    Identification_Number = x.IDCard.Identification_Number,
                    Local_Full_Name = x.Personal != null ? x.Personal.Local_Full_Name : null,
                    Division = x.IDCard.Division,
                    Factory = x.IDCard.Factory,
                    Employee_ID = x.IDCard.Employee_ID,
                    Department = x.IDCard.Department != null ? $"{x.IDCard.Department} - {codeLang.FirstOrDefault(y => y.Division == x.IDCard.Division
                                                                                                               && y.Factory == x.IDCard.Factory
                                                                                                               && y.Department_Code == x.IDCard.Department).Code_Name}" : "",
                    Assigned_Division = x.IDCard.Assigned_Division,
                    Assigned_Factory = x.IDCard.Assigned_Factory,
                    Assigned_Employee_ID = x.IDCard.Assigned_Employee_ID,
                    Assigned_Department = x.IDCard.Assigned_Department != null
                                            ? $"{x.IDCard.Assigned_Department} - {codeLang.FirstOrDefault(y => y.Division == x.IDCard.Assigned_Division
                                                                                                       && y.Factory == x.IDCard.Assigned_Factory
                                                                                                       && y.Department_Code == x.IDCard.Assigned_Department).Code_Name}" : "",
                    Onboard_Date = x.IDCard.Onboard_Date,
                    Resign_Date = x.IDCard.Resign_Date,
                    Update_By = x.IDCard.Update_By,
                    Update_Time = x.IDCard.Update_Time,
                })
                .OrderByDescending(x => x.Nationality).ThenByDescending(x => x.Identification_Number)
                .ThenByDescending(x => x.Onboard_Date).ThenByDescending(x => x.Division)
                .ThenByDescending(x => x.Factory).ThenByDescending(x => x.Employee_ID)
                .ToListAsync();

            return PaginationUtility<HRMS_Emp_IDcard_EmpID_HistoryDto>.Create(data, pagination.PageNumber, pagination.PageSize);
        }

        #region GetList
        public async Task<List<KeyValuePair<string, string>>> GetListDivision(string language)
        {
            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "1")
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower()),
                                x => new { x.Type_Seq, x.Code },
                                y => new { y.Type_Seq, y.Code },
                                (x, y) => new { x, y })
                                .SelectMany(x => x.y.DefaultIfEmpty(),
                                (x, y) => new { BasicCode = x.x, BasicCodeLanguage = y })
            .Select(x => new KeyValuePair<string, string>(x.BasicCode.Code, $"{x.BasicCode.Code} - {(x.BasicCodeLanguage != null ? x.BasicCodeLanguage.Code_Name : x.BasicCode.Code_Name)}")).ToListAsync();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string division, string language)
        {
            var pred = PredicateBuilder.New<HRMS_Basic_Factory_Comparison>(x => x.Kind == "1");

            if (!string.IsNullOrWhiteSpace(division))
                pred = pred.And(x => x.Division.ToLower().Contains(division.ToLower()));

            var data = await _repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(pred)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower()),
                    x => x.Factory,
                    y => y.Code,
                    (x, y) => new { x.Factory, CodeNameLanguage = y.Select(z => z.Code_Name).FirstOrDefault() })
                .Join(_repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "2"),
                    x => x.Factory,
                    y => y.Code,
                    (x, y) => new { x.Factory, x.CodeNameLanguage, CodeName = y.Code_Name })
                .Select(x => new KeyValuePair<string, string>(x.Factory, x.CodeNameLanguage ?? x.CodeName))
                .Distinct().ToListAsync();

            if (!data.Any())
            {
                var allFactories = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "2")
                    .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower()),
                        x => x.Code,
                        y => y.Code,
                        (x, y) => new { x.Code, NameCode = x.Code_Name, NameLanguage = y.Select(z => z.Code_Name).FirstOrDefault() })
                    .Select(x => new KeyValuePair<string, string>(x.Code, x.NameLanguage ?? x.NameCode))
                    .ToListAsync();
                return allFactories;
            }
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListNationality(string language)
        {
            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "10")
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower()),
                                x => new { x.Type_Seq, x.Code },
                                y => new { y.Type_Seq, y.Code },
                                (x, y) => new { x, y })
                                .SelectMany(x => x.y.DefaultIfEmpty(),
                                (x, y) => new { BasicCode = x.x, BasicCodeLanguage = y })
            .Select(x => new KeyValuePair<string, string>(x.BasicCode.Code, $"{(x.BasicCodeLanguage != null ? x.BasicCodeLanguage.Code_Name : x.BasicCode.Code_Name)}")).ToListAsync();
            return data;
        }
        #endregion
    }
}