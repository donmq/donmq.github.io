using System.Data.SqlTypes;
using System.Globalization;
using API.Data;
using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.EmployeeMaintenance
{
    public class S_4_1_15_ContractManagement : BaseServices, I_4_1_15_ContractManagement
    {
        public S_4_1_15_ContractManagement(DBContext dbContext) : base(dbContext) { }
        public async Task<OperationResult> Create(ContractManagementDto data)
        {
            var predicate = PredicateBuilder.New<HRMS_Emp_Contract_Management>(true);
            if (string.IsNullOrWhiteSpace(data.Division)
              || string.IsNullOrWhiteSpace(data.Factory)
              || string.IsNullOrWhiteSpace(data.Employee_ID)
              || string.IsNullOrWhiteSpace(data.Contract_Type)
              || data.Seq == 0
              || string.IsNullOrWhiteSpace(data.Contract_Start)
              || !DateTime.TryParseExact(data.Contract_Start, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime Cont_StartValue)
              || string.IsNullOrWhiteSpace(data.Contract_End)
              || !DateTime.TryParseExact(data.Contract_End, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime Cont_EndValue))
                return new OperationResult(false, "Invalid Input");
            predicate.And(x => x.Division == data.Division && x.Factory == data.Factory && x.Employee_ID == data.Employee_ID && x.Seq == data.Seq);
            if (await _repositoryAccessor.HRMS_Emp_Contract_Management.AnyAsync(predicate))
                return new OperationResult(false, "Already Exited Data");
            try
            {
                HRMS_Emp_Contract_Management item = new()
                {
                    Division = data.Division,
                    Factory = data.Factory,
                    Employee_ID = data.Employee_ID,
                    Seq = data.Seq,
                    Contract_Type = data.Contract_Type,
                    Contract_Start = Cont_StartValue,
                    Contract_End = Cont_EndValue,
                    Effective_Status = data.Effective_Status,
                    Probation_Start = DateTime.TryParseExact(data.Probation_Start, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime startValue) ? startValue : null,
                    Probation_End = DateTime.TryParseExact(data.Probation_End, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime endValue) ? endValue : null,
                    Assessment_Result = data.Assessment_Result,
                    Extend_to = DateTime.TryParseExact(data.Extend_to, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime toValue) ? toValue : null,
                    Reason = !string.IsNullOrWhiteSpace(data.Reason) ? data.Reason.Trim() : null,
                    Update_By = data.Update_By,
                    Update_Time = data.Update_Time
                };
                _repositoryAccessor.HRMS_Emp_Contract_Management.Add(item);
                await _repositoryAccessor.Save();
                return new OperationResult(true, "Create Successfully");
            }
            catch (Exception)
            {
                return new OperationResult(false, "Create failed");
            }
        }

        public async Task<OperationResult> Update(ContractManagementDto data)
        {
            var predicate = PredicateBuilder.New<HRMS_Emp_Contract_Management>(true);
            if (string.IsNullOrWhiteSpace(data.Division)
              || string.IsNullOrWhiteSpace(data.Factory)
              || string.IsNullOrWhiteSpace(data.Employee_ID)
              || string.IsNullOrWhiteSpace(data.Contract_Type)
              || data.Seq == 0
              || string.IsNullOrWhiteSpace(data.Contract_Start)
              || !DateTime.TryParseExact(data.Contract_Start, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime Cont_StartValue)
              || string.IsNullOrWhiteSpace(data.Contract_End)
              || !DateTime.TryParseExact(data.Contract_End, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime Cont_EndValue))
                return new OperationResult(false, "Invalid Input");
            predicate.And(x => x.Division == data.Division && x.Factory == data.Factory && x.Employee_ID == data.Employee_ID && x.Seq == data.Seq);
            var result = await _repositoryAccessor.HRMS_Emp_Contract_Management.FirstOrDefaultAsync(predicate);
            if (result == null)
                return new OperationResult(false, "Data not exist");
            try
            {
                result.Contract_Type = data.Contract_Type;
                result.Contract_Start = Cont_StartValue;
                result.Contract_End = Cont_EndValue;
                result.Effective_Status = data.Effective_Status;
                result.Probation_Start = DateTime.TryParseExact(data.Probation_Start, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime startValue) ? startValue : null;
                result.Probation_End = DateTime.TryParseExact(data.Probation_End, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime endValue) ? endValue : null;
                result.Assessment_Result = data.Assessment_Result;
                result.Extend_to = DateTime.TryParseExact(data.Extend_to, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime toValue) ? toValue : null;
                result.Reason = !string.IsNullOrWhiteSpace(data.Reason) ? data.Reason.Trim() : null;
                result.Update_By = data.Update_By;
                result.Update_Time = data.Update_Time;
                _repositoryAccessor.HRMS_Emp_Contract_Management.Update(result);
                await _repositoryAccessor.Save();
                return new OperationResult(true, "Update Successfully");
            }
            catch (Exception)
            {
                return new OperationResult(false, "Update failed");
            }
        }

        public async Task<OperationResult> Delete(ContractManagementDto data)
        {
            var item = await _repositoryAccessor.HRMS_Emp_Contract_Management.FirstOrDefaultAsync(x =>
                    x.Division == data.Division && x.Factory == data.Factory && x.Employee_ID == data.Employee_ID && x.Seq == data.Seq);
            if (item == null)
                return new OperationResult(false, "Data not exist");
            _repositoryAccessor.HRMS_Emp_Contract_Management.Remove(item);
            if (await _repositoryAccessor.Save())
                return new OperationResult(true, "Delete Successfully");
            return new OperationResult(false, "Delete failed");
        }
        #region GetData
        public async Task<PaginationUtility<ContractManagementDto>> GetData(PaginationParam pagination, ContractManagementParam param, List<string> roleList)
        {
            var pred = PredicateBuilder.New<HRMS_Emp_Contract_Management>(true);
            var predEmpPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true);

            if (param.EffectiveStatus != "All")
                pred = pred.And(x => x.Effective_Status == (param.EffectiveStatus == "Y"));
            if (!string.IsNullOrWhiteSpace(param.Division))
            {
                pred = pred.And(x => x.Division == param.Division);
                // predEmpPersonal = predEmpPersonal.And(x => x.Division == param.Division);
            }
            if (!string.IsNullOrWhiteSpace(param.Factory))
            {
                pred = pred.And(x => x.Factory == param.Factory);
                // predEmpPersonal = predEmpPersonal.And(x => x.Factory == param.Factory);
            }
            if (!string.IsNullOrWhiteSpace(param.EmployeeID))
                pred = pred.And(x => x.Employee_ID.ToLower().Contains(param.EmployeeID.Trim().ToLower()));
            if (!string.IsNullOrWhiteSpace(param.ContractType))
                pred = pred.And(x => x.Contract_Type == param.ContractType);

            DateTime ContractEndFrom = param.Contract_End_From != null ? Convert.ToDateTime(param.Contract_End_From + " 00:00:00.000") : SqlDateTime.MinValue.Value;
            DateTime ContractEndTo = param.Contract_End_To != null ? Convert.ToDateTime(param.Contract_End_To + " 23:59:59.997") : SqlDateTime.MaxValue.Value;

            if (ContractEndFrom <= ContractEndTo)
                pred = pred.And(x => x.Contract_End >= ContractEndFrom && x.Contract_End <= ContractEndTo);

            DateTime ProbationEndFrom = param.Probation_End_From != null ? Convert.ToDateTime(param.Probation_End_From + " 00:00:00.000") : SqlDateTime.MinValue.Value;
            DateTime ProbationEndTo = param.Probation_End_To != null ? Convert.ToDateTime(param.Probation_End_To + " 23:59:59.997") : SqlDateTime.MaxValue.Value;

            if (!string.IsNullOrWhiteSpace(param.Probation_End_From) || !string.IsNullOrWhiteSpace(param.Probation_End_To))
            {
                if (ProbationEndFrom <= ProbationEndTo)
                    pred = pred.And(x => x.Probation_End >= ProbationEndFrom && x.Probation_End <= ProbationEndTo);
            }

            var dataContract = _repositoryAccessor.HRMS_Emp_Contract_Management.FindAll(pred, true).ToList();
            var emp_Personal = await Query_Permission_Data_Filter(roleList, predEmpPersonal);

            var data = dataContract
                    .Join(emp_Personal.Distinct(),
                        x => new { x.Employee_ID, x.Division, x.Factory },
                        y => new { y.Employee_ID, y.Division, y.Factory },
                        (x, y) => new { HECM = x, HEP = y })
                    .GroupJoin(_repositoryAccessor.HRMS_Emp_Contract_Type.FindAll(true),
                        x => new { x.HECM.Contract_Type, x.HECM.Division, x.HECM.Factory },
                        y => new { y.Contract_Type, y.Division, y.Factory },
                        (x, y) => new { x.HECM, x.HEP, HECT = y })
                    .SelectMany(x => x.HECT.DefaultIfEmpty(),
                        (x, y) => new { x.HECM, x.HEP, HECT = y })
                    .GroupJoin(_repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "36", true),
                        x => x.HECM.Assessment_Result,
                        y => y.Code,
                        (x, y) => new { x.HECM, x.HEP, x.HECT, HBC = y })
                    .SelectMany(x => x.HBC.DefaultIfEmpty(),
                        (x, y) => new { x.HECM, x.HEP, x.HECT, HBC = y })
                    .Select(x => new ContractManagementDto
                    {
                        Division = x.HECM.Division,
                        Factory = x.HECM.Factory,
                        Employee_ID = x.HECM.Employee_ID,
                        Local_Full_Name = x.HEP != null ? x.HEP.Local_Full_Name : "",
                        Onboard_Date = x.HEP?.Onboard_Date,
                        Department = x.HEP != null ? x.HEP.Department : "",
                        Seq = x.HECM.Seq,
                        Contract_Type = x.HECM.Contract_Type,
                        Contract_Title = x.HECT != null ? x.HECT.Contract_Title : "",
                        Contract_Start = x.HECM.Contract_Start.ToString("yyyy/MM/dd"),
                        Contract_End = x.HECM.Contract_End.ToString("yyyy/MM/dd"),
                        Effective_Status = x.HECM.Effective_Status,
                        Probation_Start = x.HECM.Probation_Start?.ToString("yyyy/MM/dd"),
                        Probation_End = x.HECM.Probation_End?.ToString("yyyy/MM/dd"),
                        Assessment_Result = x.HECM.Assessment_Result,
                        Assessment_Result_Name = x.HECM.Assessment_Result + (x.HBC != null ? (" - " + x.HBC.Code_Name) : ""),
                        Extend_to = x.HECM.Extend_to?.ToString("yyyy/MM/dd"),
                        Reason = x.HECM.Reason,
                        Update_By = x.HECM.Update_By,
                        Update_Time = x.HECM.Update_Time,
                    })
                    .ToList();


            if (!string.IsNullOrWhiteSpace(param.LocalFullName))
                data = data.Where(x => x.Local_Full_Name.ToLower().Contains(param.LocalFullName.Trim().ToLower())).ToList();

            if (!string.IsNullOrWhiteSpace(param.Department))
                data = data.Where(x => x.Department == param.Department).ToList();

            DateTime onboardDateStart = param.Onboard_Date_From != null ? Convert.ToDateTime(param.Onboard_Date_From + " 00:00:00.000") : SqlDateTime.MinValue.Value;
            DateTime onboardDateEnd = param.Onboard_Date_To != null ? Convert.ToDateTime(param.Onboard_Date_To + " 23:59:59.997") : SqlDateTime.MaxValue.Value;

            if (onboardDateStart <= onboardDateEnd)
                data = data.Where(x => x.Onboard_Date >= onboardDateStart && x.Onboard_Date <= onboardDateEnd).ToList();

            return PaginationUtility<ContractManagementDto>.Create(data, pagination.PageNumber, pagination.PageSize);
        }
        #endregion

        #region GetList
        public async Task<List<KeyValuePair<string, string>>> GetListDivision(string lang)
        {
            var listDivision = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.IsActive == true && x.Type_Seq == "1", true)
                            .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == lang.ToLower(), true),
                                x => new { x.Type_Seq, x.Code },
                                y => new { y.Type_Seq, y.Code },
                                (x, y) => new { basicCode = x, basicCodeLang = y })
                            .SelectMany(x => x.basicCodeLang.DefaultIfEmpty(),
                                (x, y) => new { x.basicCode, basicCodeLang = y })
                            .Select(x => new KeyValuePair<string, string>(
                                x.basicCode.Code,
                                $"{(x.basicCodeLang != null ? x.basicCodeLang.Code_Name : x.basicCode.Code_Name)}"
                            ))
                            .ToListAsync();
            return listDivision;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string division, string lang)
        {
            var pred = PredicateBuilder.New<HRMS_Basic_Factory_Comparison>(x => x.Kind == "1");
            if (!string.IsNullOrWhiteSpace(division))
                pred.And(x => x.Division.ToLower() == division.Trim().ToLower());

            var data = await _repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(pred, true)
                    .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == lang.ToLower(), true),
                        x => x.Factory,
                        y => y.Code,
                        (x, y) => new { x.Factory, CodeNameLanguage = y.Select(z => z.Code_Name).FirstOrDefault() })
                    .Join(_repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "2", true),
                    x => x.Factory,
                    y => y.Code,
                    (x, y) => new { x.Factory, x.CodeNameLanguage, CodeName = y.Code_Name })
                    .Select(x => new KeyValuePair<string, string>(
                        x.Factory,
                        x.CodeNameLanguage ?? x.CodeName))
                    .Distinct()
                    .ToListAsync();
            if (!data.Any())
            {
                var allFactories = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "2", true)
                                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == lang.ToLower(), true),
                                    x => new { x.Type_Seq, x.Code },
                                    y => new { y.Type_Seq, y.Code },
                                    (x, y) => new { basicCode = x, basicCodeLang = y })
                                .SelectMany(x => x.basicCodeLang.DefaultIfEmpty(),
                                    (x, y) => new { x.basicCode, basicCodeLang = y })
                                .Select(x => new KeyValuePair<string, string>
                                (
                                    x.basicCode.Code,
                                    x.basicCodeLang != null ? x.basicCodeLang.Code_Name : x.basicCode.Code_Name
                                ))
                                .Distinct()
                                .ToListAsync();
                return allFactories;
            }

            return data;
        }
        public async Task<List<KeyValuePair<string, string>>> GetListDepartment(string division, string factory, string lang)
        {
            var department = _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Division == division && x.Factory == factory, true);
            var departmentLanguage = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x =>
                x.Division == division &&
                x.Factory == factory &&
                x.Language_Code.ToLower() == lang.ToLower()
            , true);

            var data = await department
                .GroupJoin(departmentLanguage,
                    x => x.Department_Code,
                    y => y.Department_Code,
                    (x, y) => new { HOD = x, HODL = y })
                .SelectMany(x => x.HODL.DefaultIfEmpty(),
                    (x, y) => new { x.HOD, HODL = y })
                .Select(x => new KeyValuePair<string, string>(
                        x.HOD.Department_Code,
                        $"{x.HOD.Department_Code} - {(x.HODL != null ? x.HODL.Name : x.HOD.Department_Name)}"
                    ))
                .Distinct()
                .ToListAsync();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListContractType(string division, string factory, string lang)
        {
            return await _repositoryAccessor.HRMS_Emp_Contract_Type.FindAll(x => (division == null || x.Division == division) && (factory == null || x.Factory == factory), true)
                    .Select(x => new KeyValuePair<string, string>
                    (
                        x.Contract_Type,
                        x.Contract_Title
                    ))
                    .Distinct()
                    .ToListAsync();
        }

        public async Task<List<KeyValuePair<string, string>>> GetListAssessmentResult(string lang)
        {
            var listDivision = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.IsActive == true && x.Type_Seq == "36", true)
                            .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == lang.ToLower(), true),
                                x => new { x.Type_Seq, x.Code },
                                y => new { y.Type_Seq, y.Code },
                                (x, y) => new { basicCode = x, basicCodeLang = y })
                            .SelectMany(x => x.basicCodeLang.DefaultIfEmpty(),
                                (x, y) => new { x.basicCode, basicCodeLang = y })
                            .Select(x => new KeyValuePair<string, string>(
                                x.basicCode.Code,
                                $"{(x.basicCodeLang != null ? x.basicCodeLang.Code_Name : x.basicCode.Code_Name)}"
                            ))
                            .ToListAsync();
            return listDivision;
        }
        public async Task<ProbationParam> GetProbationDate(string division, string factory, string contractType)
        {
            ProbationParam dataContract = new();
            HRMS_Emp_Contract_Type data = await _repositoryAccessor.HRMS_Emp_Contract_Type
                .FirstOrDefaultAsync(x => (division == null || x.Division == division) && (factory == null || x.Factory == factory) && x.Contract_Type == contractType);
            if (data != null)
            {
                dataContract = new ProbationParam
                {
                    Probationary_Period = data.Probationary_Period,
                    Probationary_Year = data.Probationary_Year,
                    Probationary_Month = data.Probationary_Month,
                    Probationary_Day = data.Probationary_Day,
                };
            }
            return dataContract;
        }

        public async Task<Personal> GetPerson(PersonalParam paramPersonal)
        {
            var listSeq = _repositoryAccessor.HRMS_Emp_Contract_Management
                .FindAll(x => x.Division == paramPersonal.Division && x.Factory == paramPersonal.Factory && x.Employee_ID == paramPersonal.EmployeeID, true).Select(x => x.Seq).ToList();

            return await _repositoryAccessor.HRMS_Emp_Personal.FindAll(x => x.Division == paramPersonal.Division && x.Factory == paramPersonal.Factory && x.Employee_ID == paramPersonal.EmployeeID, true)
                .GroupJoin(_repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Division == paramPersonal.Division && x.Factory == paramPersonal.Factory, true),
                    x => x.Department,
                    y => y.Department_Code,
                    (x, y) => new { HEP = x, HOD = y })
                .SelectMany(x => x.HOD.DefaultIfEmpty(),
                        (x, y) => new { x.HEP, HOD = y })
                .GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(x =>
                    x.Division == paramPersonal.Division &&
                    x.Factory == paramPersonal.Factory &&
                    x.Language_Code.ToLower() == paramPersonal.Lang.ToLower()
                 , true),
                    x => x.HOD.Department_Code,
                    y => y.Department_Code,
                    (x, y) => new { x.HEP, x.HOD, HODL = y })
                .SelectMany(x => x.HODL.DefaultIfEmpty(),
                        (x, y) => new { x.HEP, x.HOD, HODL = y })
                .Select(x => new Personal
                {
                    Local_Full_Name = x.HEP.Local_Full_Name,
                    Nationality = x.HEP.Nationality,
                    Department = $"{x.HEP.Department} - {(x.HODL != null ? x.HODL.Name : x.HOD.Department_Name)}",
                    Seq = listSeq,
                    Onboard_Date = x.HEP.Onboard_Date
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<KeyValuePair<string, string>>> GetEmployeeID()
        {
            var data = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(true).Select(x => new KeyValuePair<string, string>(x.Employee_ID, x.Employee_ID)).Distinct().ToListAsync();
            return data;
        }
    }
    #endregion
}