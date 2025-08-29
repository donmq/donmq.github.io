using API.Data;
using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.EmployeeMaintenance
{
    public class S_4_1_12_ResignationManagement : BaseServices, I_4_1_12_ResignationManagement
    {
        public S_4_1_12_ResignationManagement(DBContext dbContext) : base(dbContext) { }

        #region GetData
        private async Task<List<HRMS_Emp_ResignationDto>> GetData(ResignationManagementParam param, List<string> roleList)
        {
            var pred = PredicateBuilder.New<HRMS_Emp_Resignation>(x => x.Division == param.Division && x.Factory == param.Factory);
            var predPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(x => x.Division == param.Division && x.Factory == param.Factory);

            if (!string.IsNullOrWhiteSpace(param.Employee_ID))
                pred.And(x => x.Employee_ID.Contains(param.Employee_ID.Trim()));
            if (!string.IsNullOrWhiteSpace(param.Local_Full_Name))
                predPersonal.And(x => x.Local_Full_Name.Contains(param.Local_Full_Name.Trim()));
            if (!string.IsNullOrEmpty(param.StartDate))
                pred.And(x => x.Resign_Date >= Convert.ToDateTime(param.StartDate));
            if (!string.IsNullOrEmpty(param.EndDate))
                pred.And(x => x.Resign_Date <= Convert.ToDateTime(param.EndDate));

            var personalQuery = await Query_Permission_Data_Filter(roleList, predPersonal);
            var departmentQuery = _repositoryAccessor.HRMS_Org_Department.FindAll(true)
                                                        .Where(x => x.Division == param.Division && x.Factory == param.Factory);
            var depLanguageQuery = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower(), true)
                                                        .Where(x => x.Division == param.Division && x.Factory == param.Factory);

            var basicCode = _repositoryAccessor.HRMS_Basic_Code.FindAll();
            var basicLanguage = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower());
            var codeLang = await basicCode.GroupJoin(basicLanguage,
                x => new { x.Type_Seq, x.Code },
                y => new { y.Type_Seq, y.Code },
                    (x, y) => new { Code = x, Language = y })
                .SelectMany(x => x.Language.DefaultIfEmpty(),
                    (x, y) => new { x.Code, Language = y })
                .Select(x => new
                {
                    x.Code.Code,
                    x.Code.Type_Seq,
                    Code_Name = x.Language != null ? x.Language.Code_Name : x.Code.Code_Name
                }).Distinct().ToListAsync();

            var data = _repositoryAccessor.HRMS_Emp_Resignation.FindAll(pred, true).ToList();

            var result = data
                .GroupJoin(
                    personalQuery,
                    resign => resign.USER_GUID,
                    personal => personal.USER_GUID,
                    (resign, personal) => new { Resign = resign, Personal = personal }
                )
                .SelectMany(
                    x => x.Personal.DefaultIfEmpty(),
                    (x, personal) => new { x.Resign, Personal = personal }
                )
                .Where(x => x.Personal != null)
                .GroupJoin(
                    departmentQuery,
                    x => x.Personal?.Department,
                    department => department.Department_Code,
                    (x, department) => new { x.Resign, x.Personal, Department = department }
                )
                .SelectMany(
                    x => x.Department.DefaultIfEmpty(),
                    (x, department) => new { x.Resign, x.Personal, Department = department }
                )
                .GroupJoin(
                    depLanguageQuery,
                    x => x.Department?.Department_Code,
                    lang => lang.Department_Code,
                    (x, lang) => new { x.Resign, x.Personal, x.Department, LanguageDepartment = lang }
                )
                .SelectMany(
                    x => x.LanguageDepartment.DefaultIfEmpty(),
                    (x, lang) => new { x.Resign, x.Personal, x.Department, LanguageDepartment = lang }
                )
                .Select(
                    x => new HRMS_Emp_ResignationDto
                    {
                        USER_GUID = x.Resign?.USER_GUID,
                        Division = x.Resign?.Division,
                        Factory = x.Resign?.Factory,
                        Nationality = x.Resign?.Nationality,
                        Identification_Number = x.Resign?.Identification_Number,
                        Employee_ID = x.Resign?.Employee_ID,
                        Local_Full_Name = x.Personal?.Local_Full_Name,
                        Department = x.Personal?.Department != null ? $"{x.Personal.Department} - {(x.LanguageDepartment != null ? x.LanguageDepartment.Name : x.Department?.Department_Name)}" : null,
                        Department_Code = x.Personal?.Department,
                        Department_Name = x.LanguageDepartment?.Name ?? x.Department?.Department_Name,
                        Resign_Date = x.Resign?.Resign_Date.ToString("yyyy/MM/dd"),
                        Onboard_Date = x.Resign?.Onboard_Date.ToString("yyyy/MM/dd"),
                        Resignation_Type = x.Resign?.Resignation_Type,
                        Resignation_Type_Str = x.Resign?.Resignation_Type != null ? $"{x.Resign.Resignation_Type} - {codeLang.FirstOrDefault(y => y.Code == x.Resign.Resignation_Type && y.Type_Seq == "32")?.Code_Name}" : null,
                        Resign_Reason = x.Resign?.Resign_Reason,
                        Resign_Reason_Str = x.Resign?.Resign_Reason != null ? $"{x.Resign.Resign_Reason} - {codeLang.FirstOrDefault(y => y.Code == x.Resign.Resign_Reason && y.Type_Seq == "33")?.Code_Name}" : null,
                        Remark = x.Resign?.Remark,
                        Blacklist = x.Resign?.Blacklist,
                        Blacklist_Str = x.Resign?.Blacklist == true ? "Y" : "N",
                        Verifier = x.Resign?.Verifier,
                        Verifier_Name = x.Resign?.Verifier_Name,
                        Verifier_Title = x.Resign?.Verifier_Title != null ? (codeLang.FirstOrDefault(y => y.Code == x.Resign.Verifier_Title && y.Type_Seq == "3")?.Code_Name) : "",
                        Update_By = x.Resign?.Update_By,
                        Update_Time = x.Resign?.Update_Time.ToString("yyyy/MM/dd")
                    })
                    .OrderBy(x => x.Division)
                    .ThenBy(x => x.Factory)
                    .ThenBy(x => x.Employee_ID)
                    .ToList();

            var groupedResult = result.GroupBy(x => new { x.Division, x.Factory, x.Employee_ID })
                  .Select(x => x.First())
                  .ToList();

            return groupedResult;
        }

        public async Task<PaginationUtility<HRMS_Emp_ResignationDto>> GetDataPagination(PaginationParam pagination, ResignationManagementParam param, List<string> roleList)
        {
            var data = await GetData(param, roleList);
            return PaginationUtility<HRMS_Emp_ResignationDto>.Create(data, pagination.PageNumber, pagination.PageSize);
        }
        #endregion

        #region Download
        public async Task<OperationResult> DownloadExcel(ResignationManagementParam param, List<string> roleList)
        {
            var data = await GetData(param, roleList);
            if (!data.Any())
                return new OperationResult(false, "No Data");
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                data,
                "Resources\\Template\\EmployeeMaintenance\\4_1_12_ResignationManagement\\Download.xlsx"
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }
        #endregion

        #region GetList
        public async Task<List<KeyValuePair<string, string>>> GetListDivision(string language)
        {
            return await GetHRMS_Basic_Code(BasicCodeTypeConstant.Division, language);
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
                .Select(x => new KeyValuePair<string, string>(x.Factory, $"{x.Factory} - {x.CodeNameLanguage ?? x.CodeName}"))
                .Distinct().ToListAsync();

            if (!data.Any())
            {
                var allFactories = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "2")
                    .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower()),
                        x => x.Code,
                        y => y.Code,
                        (x, y) => new { x.Code, NameCode = x.Code_Name, NameLanguage = y.Select(z => z.Code_Name).FirstOrDefault() })
                    .Select(x => new KeyValuePair<string, string>(x.Code, $"{x.Code} - {x.NameLanguage ?? x.NameCode}"))
                    .ToListAsync();
                return allFactories;
            }
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListResignationType(string language)
        {
            return await GetHRMS_Basic_Code(BasicCodeTypeConstant.ResignationType, language);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListResignReason(string language, string resignationType)
        {
            return await GetHRMS_Basic_Code(BasicCodeTypeConstant.ReasonResignation, language, resignationType);
        }

        private async Task<List<KeyValuePair<string, string>>> GetHRMS_Basic_Code(string Type_Seq, string language, string resignationType = null)
        {
            var query = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == Type_Seq, true);
            if (!string.IsNullOrEmpty(resignationType))
                query = query.Where(x => x.Char1 == resignationType);
            return await query.GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                HBC => new { HBC.Type_Seq, HBC.Code },
                HBCL => new { HBCL.Type_Seq, HBCL.Code },
                (HBC, HBCL) => new { HBC, HBCL })
                .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (prev, HBCL) => new { prev.HBC, HBCL })
                .Select(x => new KeyValuePair<string, string>(x.HBC.Code, $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"))
                .ToListAsync();
        }
        #endregion

        #region Get Employee Data
        public async Task<List<string>> GetEmployeeID()
        {
            return await _repositoryAccessor.HRMS_Emp_Personal.FindAll(x => x.Employee_ID.Length <= 9, true).Select(x => x.Employee_ID).ToListAsync();
        }

        public async Task<OperationResult> GetVerifierName(string factory, string verifier)
        {
            if (!string.IsNullOrWhiteSpace(factory) && !string.IsNullOrWhiteSpace(verifier))
            {
                var data = await _repositoryAccessor.HRMS_Emp_Personal
                    .FirstOrDefaultAsync(x => x.Factory == factory && x.Employee_ID == verifier, true);
                return data != null ? new OperationResult { IsSuccess = true, Data = data.Local_Full_Name ?? "" } : new OperationResult(false);
            }
            return new OperationResult(false);
        }

        public async Task<OperationResult> GetVerifierTitle(string factory, string verifier, string language)
        {
            if (!string.IsNullOrWhiteSpace(factory) && !string.IsNullOrWhiteSpace(verifier))
            {
                var empPersonal = await _repositoryAccessor.HRMS_Emp_Personal
                    .FirstOrDefaultAsync(x => x.Factory == factory && x.Employee_ID == verifier, true);

                if (empPersonal != null)
                {
                    string positionTitle = empPersonal.Position_Title;

                    var basicCode = await _repositoryAccessor.HRMS_Basic_Code
                        .FirstOrDefaultAsync(x => x.Type_Seq == "3" && x.Code == positionTitle);

                    var basicCodeLang = await _repositoryAccessor.HRMS_Basic_Code_Language
                        .FirstOrDefaultAsync(x => x.Language_Code.ToLower() == language.ToLower() && x.Type_Seq == "3" && x.Code == positionTitle);

                    return new OperationResult { IsSuccess = true, Data = basicCodeLang != null ? basicCodeLang.Code_Name : basicCode.Code_Name ?? "" };
                }
            }
            return new OperationResult(false);
        }

        public async Task<List<HRMS_Emp_ResignationFormDto>> GetEmployeeData(string factory, string employeeID)
        {
            var data = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(x => x.Factory == factory && x.Employee_ID == employeeID, true)
                .Select(
                    x => new HRMS_Emp_ResignationFormDto
                    {
                        Nationality = x.Nationality,
                        Identification_Number = x.Identification_Number,
                        Local_Full_Name = x.Local_Full_Name,
                        Onboard_Date = x.Onboard_Date.ToString("yyyy/MM/dd"),
                    })
                    .ToListAsync();

            return data;
        }
        #endregion

        #region Add
        public async Task<OperationResult> AddNew(ResignAddAndEditParam param, string userName)
        {
            var personData = await _repositoryAccessor.HRMS_Emp_Personal
                            .FirstOrDefaultAsync(x => x.Division == param.Division && x.Factory == param.Factory && x.Employee_ID == param.Employee_ID);

            var userPersonalData = await _repositoryAccessor.HRMS_Emp_Personal
                                        .FirstOrDefaultAsync(x => x.USER_GUID == personData.USER_GUID && x.Division == param.Division
                                                               && x.Factory == param.Factory && x.Employee_ID == param.Employee_ID);

            if (userPersonalData == null)
                return new OperationResult(false, "No Data");

            var idCardHistory = await _repositoryAccessor.HRMS_Emp_IDcard_EmpID_History
                                .FirstOrDefaultAsync(x => x.USER_GUID == personData.USER_GUID && x.Division == param.Division
                                                       && x.Factory == param.Factory && x.Employee_ID == param.Employee_ID);

            var existingData = await _repositoryAccessor.HRMS_Emp_Resignation
                                .FirstOrDefaultAsync(x => x.USER_GUID == personData.USER_GUID && x.Division == param.Division
                                                       && x.Factory == param.Factory && x.Employee_ID == param.Employee_ID);

            if (existingData != null)
                return new OperationResult(false, "Data already exists");

            var blacklistData = await _repositoryAccessor.HRMS_Emp_Blacklist
                                    .FirstOrDefaultAsync(x => x.USER_GUID == userPersonalData.USER_GUID
                                                           && x.Maintenance_Date == DateTime.Now);

            if (blacklistData != null)
                return new OperationResult(false, "Blacklist data already exists");

            var verifierData = await _repositoryAccessor.HRMS_Emp_Personal
                                .FirstOrDefaultAsync(x => x.Employee_ID == param.Verifier);

            var newData = new HRMS_Emp_Resignation
            {
                USER_GUID = userPersonalData.USER_GUID,
                Division = param.Division,
                Factory = param.Factory,
                Employee_ID = param.Employee_ID,
                Nationality = param.Nationality,
                Identification_Number = param.Identification_Number,
                Onboard_Date = Convert.ToDateTime(param.Onboard_Date),
                Resign_Date = Convert.ToDateTime(param.Resign_Date),
                Resignation_Type = param.Resignation_Type,
                Resign_Reason = param.Resign_Reason,
                Remark = param.Remark,
                Blacklist = param.Blacklist,
                Verifier = param.Verifier,
                Verifier_Name = param.Verifier_Name,
                Verifier_Title = verifierData != null ? verifierData.Position_Title : "",
                Update_By = userName,
                Update_Time = DateTime.Now
            };
            _repositoryAccessor.HRMS_Emp_Resignation.Add(newData);

            userPersonalData.Deletion_Code = "N";
            userPersonalData.Resign_Date = Convert.ToDateTime(param.Resign_Date);
            userPersonalData.Resign_Reason = param.Resign_Reason;
            userPersonalData.Blacklist = param.Blacklist;
            userPersonalData.Update_By = userName;
            userPersonalData.Update_Time = DateTime.Now;

            _repositoryAccessor.HRMS_Emp_Personal.Update(userPersonalData);

            if (param.Blacklist == true)
            {
                var newBlacklistData = new HRMS_Emp_Blacklist()
                {
                    USER_GUID = userPersonalData.USER_GUID,
                    Maintenance_Date = DateTime.Now,
                    Nationality = param.Nationality,
                    Identification_Number = param.Identification_Number,
                    Resign_Reason = param.Resign_Reason,
                    Description = param.Remark,
                    Update_By = userName,
                    Update_Time = DateTime.Now
                };
                _repositoryAccessor.HRMS_Emp_Blacklist.Add(newBlacklistData);
            }

            if (idCardHistory != null)
            {
                idCardHistory.Resign_Date = userPersonalData.Resign_Date;
                idCardHistory.Update_By = userName;
                idCardHistory.Update_Time = DateTime.Now;
                _repositoryAccessor.HRMS_Emp_IDcard_EmpID_History.Update(idCardHistory);
            }

            await _repositoryAccessor.BeginTransactionAsync();
            {
                try
                {
                    await _repositoryAccessor.Save();
                    await _repositoryAccessor.CommitAsync();
                    return new OperationResult(true, "Add Successfully");
                }
                catch (Exception ex)
                {
                    await _repositoryAccessor.RollbackAsync();
                    return new OperationResult(false, $"Inner exception: {ex.InnerException?.Message ?? "No inner exception message available"}");
                }
            }
        }
        #endregion

        #region Edit
        public async Task<OperationResult> Edit(ResignAddAndEditParam param, string userName)
        {
            var userPersonalData = await _repositoryAccessor.HRMS_Emp_Personal
                                        .FirstOrDefaultAsync(x => x.USER_GUID == param.USER_GUID && x.Division == param.Division
                                                          && x.Factory == param.Factory && x.Employee_ID == param.Employee_ID);

            if (userPersonalData == null)
                return new OperationResult(false, "No Data");

            var existingData = await _repositoryAccessor.HRMS_Emp_Resignation
                                    .FirstOrDefaultAsync(x => x.USER_GUID == param.USER_GUID && x.Division == param.Division
                                                           && x.Factory == param.Factory && x.Employee_ID == param.Employee_ID);

            var blacklistData = await _repositoryAccessor.HRMS_Emp_Blacklist
                                    .FirstOrDefaultAsync(x => x.USER_GUID == param.USER_GUID);

            var resignationData = await _repositoryAccessor.HRMS_Emp_Resignation
                                    .FirstOrDefaultAsync(x => x.USER_GUID == param.USER_GUID && x.Division == param.Division
                                                           && x.Factory == param.Factory && x.Employee_ID == param.Employee_ID);

            if (existingData == null)
                return new OperationResult(false, "No Data");

            var verifierData = await _repositoryAccessor.HRMS_Emp_Personal
                                    .FirstOrDefaultAsync(x => x.Employee_ID == param.Verifier);

            existingData.Resign_Date = Convert.ToDateTime(param.Resign_Date);
            existingData.Resignation_Type = param.Resignation_Type;
            existingData.Resign_Reason = param.Resign_Reason;
            existingData.Remark = param.Remark;
            existingData.Blacklist = param.Blacklist;
            existingData.Verifier = param.Verifier;
            existingData.Verifier_Name = param.Verifier_Name;
            existingData.Verifier_Title = verifierData != null ? verifierData.Position_Title : "";
            existingData.Update_By = userName;
            existingData.Update_Time = DateTime.Now;
            _repositoryAccessor.HRMS_Emp_Resignation.Update(existingData);

            if (param.Blacklist == true)
            {
                if (blacklistData != null)
                {
                    blacklistData.Resign_Reason = param.Resign_Reason;
                    blacklistData.Description = param.Remark;
                    blacklistData.Update_By = userName;
                    blacklistData.Update_Time = DateTime.Now;
                    _repositoryAccessor.HRMS_Emp_Blacklist.Update(blacklistData);
                }
                else
                {
                    var newBlacklistData = new HRMS_Emp_Blacklist()
                    {
                        USER_GUID = userPersonalData.USER_GUID,
                        Maintenance_Date = Convert.ToDateTime(param.Update_Time),
                        Nationality = param.Nationality,
                        Identification_Number = param.Identification_Number,
                        Resign_Reason = param.Resign_Reason,
                        Description = param.Remark,
                        Update_By = userName,
                        Update_Time = DateTime.Now
                    };
                    _repositoryAccessor.HRMS_Emp_Blacklist.Add(newBlacklistData);
                }
            }
            else if (blacklistData != null)
                _repositoryAccessor.HRMS_Emp_Blacklist.Remove(blacklistData);

            userPersonalData.Deletion_Code = "N";
            userPersonalData.Resign_Date = Convert.ToDateTime(param.Resign_Date);
            userPersonalData.Resign_Reason = param.Resign_Reason;
            userPersonalData.Blacklist = param.Blacklist;
            userPersonalData.Update_By = userName;
            userPersonalData.Update_Time = DateTime.Now;

            _repositoryAccessor.HRMS_Emp_Personal.Update(userPersonalData);

            if (resignationData != null)
            {
                var idCardHistory = await _repositoryAccessor.HRMS_Emp_IDcard_EmpID_History
                                    .FirstOrDefaultAsync(x => x.USER_GUID == resignationData.USER_GUID && x.Division == resignationData.Division
                                                           && x.Factory == resignationData.Factory && x.Employee_ID == resignationData.Employee_ID);

                if (idCardHistory != null)
                {
                    idCardHistory.Resign_Date = resignationData.Resign_Date;
                    idCardHistory.Update_By = userName;
                    idCardHistory.Update_Time = DateTime.Now;
                    _repositoryAccessor.HRMS_Emp_IDcard_EmpID_History.Update(idCardHistory);
                }
            }

            await _repositoryAccessor.BeginTransactionAsync();
            {
                try
                {
                    await _repositoryAccessor.Save();
                    await _repositoryAccessor.CommitAsync();
                    return new OperationResult(true, "Update Successfully");
                }
                catch (Exception ex)
                {
                    await _repositoryAccessor.RollbackAsync();
                    return new OperationResult(false, $"Inner exception: {ex.InnerException?.Message ?? "No inner exception message available"}");
                }
            }
        }
        #endregion

        #region Delete
        public async Task<OperationResult> Delete(HRMS_Emp_ResignationDto data, string userName)
        {
            var existingData = await _repositoryAccessor.HRMS_Emp_Resignation
                                    .FirstOrDefaultAsync(x => x.USER_GUID == data.USER_GUID && x.Division == data.Division
                                                           && x.Factory == data.Factory && x.Employee_ID == data.Employee_ID);

            var userPersonalData = await _repositoryAccessor.HRMS_Emp_Personal
                                        .FirstOrDefaultAsync(x => x.USER_GUID == data.USER_GUID && x.Division == data.Division
                                                               && x.Factory == data.Factory && x.Employee_ID == data.Employee_ID);

            var resignationData = await _repositoryAccessor.HRMS_Emp_Resignation
                                        .FirstOrDefaultAsync(x => x.USER_GUID == data.USER_GUID && x.Division == data.Division
                                                          && x.Factory == data.Factory && x.Employee_ID == data.Employee_ID);

            if (data.Blacklist == true)
                return new OperationResult(false, "Cannot delete");

            if (existingData == null)
                return new OperationResult(false, "Data is not exist");

            _repositoryAccessor.HRMS_Emp_Resignation.Remove(existingData);

            if (userPersonalData != null)
            {
                userPersonalData.Deletion_Code = "Y";
                userPersonalData.Resign_Date = null;
                userPersonalData.Resign_Reason = null;
                userPersonalData.Blacklist = null;
                userPersonalData.Update_By = userName;
                userPersonalData.Update_Time = DateTime.Now;

                _repositoryAccessor.HRMS_Emp_Personal.Update(userPersonalData);
            }

            var resignationHistory = new HRMS_Emp_Resignation_History()
            {
                History_GUID = Guid.NewGuid().ToString(),
                Status = "D",
                USER_GUID = data.USER_GUID,
                Division = data.Division,
                Factory = data.Factory,
                Employee_ID = data.Employee_ID,
                Nationality = data.Nationality,
                Identification_Number = data.Identification_Number,
                Onboard_Date = Convert.ToDateTime(data.Onboard_Date),
                Resign_Date = Convert.ToDateTime(data.Resign_Date),
                Resignation_Type = data.Resignation_Type,
                Resign_Reason = data.Resign_Reason,
                Remark = data.Remark,
                Blacklist = data.Blacklist,
                Verifier = !string.IsNullOrWhiteSpace(data.Verifier) ? data.Verifier : null,
                Verifier_Name = !string.IsNullOrWhiteSpace(data.Verifier_Name) ? data.Verifier_Name : null,
                Verifier_Title = !string.IsNullOrWhiteSpace(data.Verifier_Title) ? data.Verifier_Title : null,
                Update_By = userName,
                Update_Time = DateTime.Now
            };
            _repositoryAccessor.HRMS_Emp_Resignation_History.Add(resignationHistory);

            if (resignationData != null)
            {
                var idCardHistory = await _repositoryAccessor.HRMS_Emp_IDcard_EmpID_History
                                    .FirstOrDefaultAsync(x => x.USER_GUID == resignationData.USER_GUID && x.Division == resignationData.Division
                                                           && x.Factory == resignationData.Factory && x.Employee_ID == resignationData.Employee_ID);

                if (idCardHistory != null)
                {
                    idCardHistory.Resign_Date = null;
                    idCardHistory.Update_By = userName;
                    idCardHistory.Update_Time = DateTime.Now;
                    _repositoryAccessor.HRMS_Emp_IDcard_EmpID_History.Update(idCardHistory);
                }
            }
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true, "Delete Successfully");
            }
            catch
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false, "Delete failed");

            }
        }
        #endregion
    }
}