using API.Data;
using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.EmployeeMaintenance
{
    public class S_4_1_19_ExitEmployeeMasterFileHistoricalData : BaseServices, I_4_1_19_ExitEmployeeMasterFileHistoricalData
    {
        public S_4_1_19_ExitEmployeeMasterFileHistoricalData(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<PaginationUtility<ExitEmployeeMasterFileHistoricalDataView>> GetPagination(PaginationParam pagination, ExitEmployeeMasterFileHistoricalDataParam param, string account)
        {
            #region PredicateBuilder
            var pred = PredicateBuilder.New<HRMS_Emp_Exit_History>(x => x.Nationality == param.Nationality
                            && x.Division == param.Division
                            && x.Factory == param.Factory);

            if (!string.IsNullOrWhiteSpace(param.IdentificationNumber))
                pred.And(x => x.Identification_Number.Contains(param.IdentificationNumber));

            if (!string.IsNullOrWhiteSpace(param.EmployeeID))
                pred.And(x => x.Employee_ID.Contains(param.EmployeeID));

            if (!string.IsNullOrWhiteSpace(param.LocalFullName))
                pred.And(x => x.Local_Full_Name.ToLower().Contains(param.LocalFullName));

            if (!string.IsNullOrWhiteSpace(param.OnboardDateStart_Str) && !string.IsNullOrWhiteSpace(param.OnboardDateEnd_Str))
                pred.And(x => x.Onboard_Date >= Convert.ToDateTime(param.OnboardDateStart_Str)
                           && x.Onboard_Date <= Convert.ToDateTime(param.OnboardDateEnd_Str));

            if (!string.IsNullOrWhiteSpace(param.GroupDateStart_Str) && !string.IsNullOrWhiteSpace(param.GroupDateEnd_Str))
                pred.And(x => x.Group_Date >= Convert.ToDateTime(param.GroupDateStart_Str)
                           && x.Group_Date <= Convert.ToDateTime(param.GroupDateEnd_Str));

            if (!string.IsNullOrWhiteSpace(param.ResignDateStart_Str) && !string.IsNullOrWhiteSpace(param.ResignDateEnd_Str))
                pred.And(x => x.Resign_Date >= Convert.ToDateTime(param.ResignDateStart_Str)
                           && x.Resign_Date <= Convert.ToDateTime(param.ResignDateEnd_Str));
            #endregion
            var Eul = _repositoryAccessor.HRMS_Emp_Unpaid_Leave.FindAll(x => x.Effective_Status,true);
            var data = await _repositoryAccessor.HRMS_Emp_Exit_History
                .FindAll(pred, true)
                // Factory
                .Join(_repositoryAccessor.HRMS_Basic_Code.FindAll(true),
                    HEEH => new { Code = HEEH.Factory },
                    HBC_F => new { HBC_F.Code },
                    (HEEH, HBC_F) => new { HEEH, HBC_F })
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower(), true),
                    x => new { x.HBC_F.Type_Seq, x.HBC_F.Code },
                    HBCL_F => new { HBCL_F.Type_Seq, HBCL_F.Code },
                    (x, HBCL_F) => new { x.HEEH, x.HBC_F, HBCL_F })
                .SelectMany(x => x.HBCL_F.DefaultIfEmpty(),
                    (x, HBCL_F) => new { x.HEEH, x.HBC_F, HBCL_F })

                // Department
                .Join(_repositoryAccessor.HRMS_Org_Department.FindAll(true),
                    x => new { x.HEEH.Division, x.HEEH.Factory, x.HEEH.Department },
                    HOD_D => new { HOD_D.Division, HOD_D.Factory, Department = HOD_D.Department_Code },
                    (x, HOD_D) => new { x.HEEH, x.HBC_F, x.HBCL_F, HOD_D })
                .GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower(), true),
                    x => new { x.HOD_D.Division, x.HOD_D.Factory, x.HOD_D.Department_Code },
                    HODL_D => new { HODL_D.Division, HODL_D.Factory, HODL_D.Department_Code },
                    (x, HODL_D) => new { x.HEEH, x.HBC_F, x.HBCL_F, x.HOD_D, HODL_D })
                .SelectMany(x => x.HODL_D.DefaultIfEmpty(),
                    (x, HODL_D) => new { x.HEEH, x.HBC_F, x.HBCL_F, x.HOD_D, HODL_D })

                .Select(x => new ExitEmployeeMasterFileHistoricalDataView
                {
                    USER_GUID = x.HEEH.USER_GUID,
                    Nationality = x.HEEH.Nationality,
                    IdentificationNumber = x.HEEH.Identification_Number,
                    LocalFullName = x.HEEH.Local_Full_Name,
                    Division = x.HEEH.Division,
                    Factory = $"{x.HEEH.Factory} - {(x.HBCL_F != null ? x.HBCL_F.Code_Name : x.HBC_F.Code_Name)}",
                    EmployeeID = x.HEEH.Employee_ID,
                    Department = $"{x.HEEH.Department} - {(x.HODL_D != null ? x.HODL_D.Name : x.HOD_D.Department_Name)}",
                    OnboardDate = x.HEEH.Onboard_Date.ToString("yyyy/MM/dd"),
                    GroupDate = x.HEEH.Group_Date.ToString("yyyy/MM/dd"),
                    ResignDate = x.HEEH.Resign_Date.ToString("yyyy/MM/dd"),
                    EmploymentStatus = x.HEEH.Employment_Status,
                    DeletionCode = x.HEEH.Deletion_Code == "U" ? Eul.Any(y => y.Division == x.HEEH.Division && y.Factory == x.HEEH.Factory && y.Employee_ID == x.HEEH.Employee_ID) ? "U" : "" : x.HEEH.Deletion_Code
                }).ToListAsync();

            return PaginationUtility<ExitEmployeeMasterFileHistoricalDataView>.Create(data, pagination.PageNumber, pagination.PageSize);
        }
        public async Task<ExitEmployeeMasterFileHistoricalDataDto> GetDetail(string USER_GUID, string Resign_Date)
        {
            var data = await _repositoryAccessor.HRMS_Emp_Exit_History
                .FirstOrDefaultAsync(x => x.USER_GUID == USER_GUID && x.Resign_Date == Convert.ToDateTime(Resign_Date), true);

            if (data == null)
                return new();

            var departmentSupervisor = await _repositoryAccessor.HRMS_Org_Department
                .FindAll(x => (x.Division == data.Division
                    && x.Factory == data.Factory
                    && x.Supervisor_Employee_ID == data.Employee_ID)
                    || (x.Division == data.Assigned_Division
                    && x.Factory == data.Assigned_Factory
                    && x.Supervisor_Employee_ID == data.Assigned_Employee_ID), true)
                .GroupBy(x => new { x.Supervisor_Type })
                .Select(x => new DepartmentSupervisorList
                {
                    SupervisorType = x.Key.Supervisor_Type,
                    DepartmentSupervisor = string.Join(", ", x.Select(y => new
                    {
                        DepartmentSupervisor = $"{y.Factory}-{y.Department_Code}-{y.Department_Name}"
                    }).ToList())
                })
                .ToListAsync();

            var passportFullName = await _repositoryAccessor.HRMS_Emp_Document
                .FindAll(x => x.Division == data.Division
                    && x.Factory == data.Factory
                    && x.Employee_ID == data.Employee_ID)
                .Select(x => new
                {
                    x.Validity_Start,
                    x.Passport_Name
                })
                .OrderByDescending(x => x.Validity_Start)
                .FirstOrDefaultAsync();

            var numberOfDependent = await _repositoryAccessor.HRMS_Emp_Dependent.CountAsync(x => x.USER_GUID == USER_GUID && x.Dependents == true);

            var account = await _repositoryAccessor.HRMS_Basic_Account
                .FirstOrDefaultAsync(x => x.Account == data.Update_By);

            ExitEmployeeMasterFileHistoricalDataDto result = new()
            {
                USER_GUID = data.USER_GUID,
                Nationality = data.Nationality,
                IdentificationNumber = data.Identification_Number,
                IssuedDate = data.Issued_Date,
                Company = data.Company,
                DeletionCode = data.Deletion_Code,
                Division = data.Division,
                Factory = data.Factory,
                EmployeeID = data.Employee_ID,
                Department = data.Department,
                AssignedDivision = data.Assigned_Division,
                AssignedFactory = data.Assigned_Factory,
                AssignedEmployeeID = data.Assigned_Employee_ID,
                AssignedDepartment = data.Assigned_Department,
                PermissionGroup = data.Permission_Group,
                EmploymentStatus = data.Employment_Status,
                PerformanceAssessmentResponsibilityDivision = data.Performance_Assessment_Responsibility_Division,
                IdentityType = data.Identity_Type,
                LocalFullName = data.Local_Full_Name,
                PreferredEnglishFullName = data.Preferred_English_Full_Name,
                ChineseName = data.Chinese_Name,
                PassportFullName = passportFullName?.Passport_Name ?? "",
                Gender = data.Gender,
                BloodType = data.Blood_Type,
                MaritalStatus = data.Marital_Status,
                DateOfBirth = data.Date_of_Birth,
                PhoneNumber = data.Phone_Number,
                MobilePhoneNumber = data.Mobile_Phone_Number,
                Education = data.Education,
                Religion = data.Religion,
                TransportationMethod = data.Transportation_Method,
                VehicleType = data.Vehicle_Type,
                NumberOfDependents = numberOfDependent,
                LicensePlateNumber = data.License_Plate_Number,
                RegisteredProvinceDirectly = data.Registered_Province_Directly,
                RegisteredCity = data.Registered_City,
                RegisteredAddress = data.Registered_Address,
                MailingProvinceDirectly = data.Mailing_Province_Directly,
                MailingCity = data.Mailing_City,
                MailingAddress = data.Mailing_Address,
                WorkShiftType = data.Work_Shift_Type,
                SwipeCardOption = data.Swipe_Card_Option,
                SwipeCardNumber = data.Swipe_Card_Number,
                PositionGrade = data.Position_Grade,
                PositionTitle = data.Position_Title,
                WorkType = data.Work_Type,
                Restaurant = data.Restaurant,
                WorkLocation = data.Work_Location,
                UnionMembership = data.Union_Membership,
                OnboardDate = data.Onboard_Date,
                DateOfGroupEmployment = data.Group_Date,
                AnnualLeaveSeniorityStartDate = data.Annual_Leave_Seniority_Start_Date,
                SeniorityStartDate = data.Seniority_Start_Date,
                DateOfResignation = data.Resign_Date,
                ReasonResignation = data.Resign_Reason,
                Blacklist = data.Blacklist,
                UpdateTime = data.Update_Time,
                UpdateBy = account.Name,
            };
            return result;
        }


        #region Get List
        public async Task<List<KeyValuePair<string, string>>> GetListNationality(string Language)
        {
            return await GetHRMS_Basic_Code(BasicCodeTypeConstant.Nationality, Language);
        }
        public async Task<List<KeyValuePair<string, string>>> GetListDivision(string Language)
        {
            return await GetHRMS_Basic_Code(BasicCodeTypeConstant.Division, Language);
        }
        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string Division, string Language)
        {
            return await _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory, true)
                .Join(_repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(x => x.Kind == "1" && x.Division == Division, true),
                    x => new { Factory = x.Code },
                    y => new { y.Factory },
                    (x, y) => new { HBC = x, HBFC = y })
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == Language.ToLower(), true),
                    x => new { x.HBC.Type_Seq, x.HBC.Code },
                    HBCL => new { HBCL.Type_Seq, HBCL.Code },
                    (x, y) => new { x.HBC, x.HBFC, HBCL = y })
                    .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (x, y) => new { x.HBC, x.HBFC, HBCL = y })
                .Select(x => new KeyValuePair<string, string>(x.HBC.Code, $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"))
                .ToListAsync();
        }

        public async Task<List<KeyValuePair<string, string>>> GetListDepartment(string Division, string Factory, string Language)
        {
            return await _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Division == Division && x.Factory == Factory, true)
                .GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == Language.ToLower(), true),
                      HOD => new { HOD.Division, HOD.Factory, HOD.Department_Code },
                      HODL => new { HODL.Division, HODL.Factory, HODL.Department_Code },
                    (HOD, HODL) => new { HOD, HODL })
                    .SelectMany(x => x.HODL.DefaultIfEmpty(),
                    (prev, HODL) => new { prev.HOD, HODL })
                .Select(x => new KeyValuePair<string, string>(x.HOD.Department_Code, $"{x.HOD.Department_Code} - {(x.HODL != null ? x.HODL.Name : x.HOD.Department_Name)}"))
                .ToListAsync();
        }

        public async Task<List<KeyValuePair<string, string>>> GetListPermission(string Language)
        {
            return await GetHRMS_Basic_Code(BasicCodeTypeConstant.PermissionGroup, Language);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListIdentityType(string Language)
        {
            return await GetHRMS_Basic_Code(BasicCodeTypeConstant.IdentityType, Language);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListEducation(string Language)
        {
            return await GetHRMS_Basic_Code(BasicCodeTypeConstant.Education, Language);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListReligion(string Language)
        {
            return await GetHRMS_Basic_Code(BasicCodeTypeConstant.Religion, Language);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListTransportationMethod(string Language)
        {
            return await GetHRMS_Basic_Code(BasicCodeTypeConstant.TransportationMethod, Language);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListVehicleType(string Language)
        {
            return await GetHRMS_Basic_Code(BasicCodeTypeConstant.VehicleType, Language);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListProvinceDirectly(string char1, string Language)
        {
            return await GetHRMS_Basic_Code_Char1(BasicCodeTypeConstant.Province, char1, Language);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListCity(string char1, string Language)
        {
            return await GetHRMS_Basic_Code_Char1(BasicCodeTypeConstant.City, char1, Language);
        }

        public async Task<List<KeyValuePair<decimal, string>>> GetPositionGrade()
        {
            return await _repositoryAccessor.HRMS_Basic_Level.FindAll(true)
                .Select(x => new KeyValuePair<decimal, string>(x.Level, $"{x.Level}"))
                .Distinct()
                .ToListAsync();
        }
        public async Task<List<KeyValuePair<string, string>>> GetPositionTitle(decimal level, string Language)
        {
            return await _repositoryAccessor.HRMS_Basic_Level.FindAll(x => x.Level == level, true)
                .Join(_repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.JobTitle, true),
                    HBL => HBL.Level_Code,
                    HBC => HBC.Code,
                    (HBL, HBC) => new { HBL, HBC })
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == Language.ToLower(), true),
                    prev => new { prev.HBC.Type_Seq, prev.HBC.Code },
                    HBCL => new { HBCL.Type_Seq, HBCL.Code },
                    (prev, HBCL) => new { prev.HBL, prev.HBC, HBCL })
                    .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (prev, HBCL) => new { prev.HBL, prev.HBC, HBCL })
                .Select(x => new KeyValuePair<string, string>(x.HBL.Level_Code, $"{x.HBL.Level_Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"))
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<KeyValuePair<string, string>>> GetListWorkType(string Language)
        {
            return await GetHRMS_Basic_Code(BasicCodeTypeConstant.WorkType, Language);
        }
        public async Task<List<KeyValuePair<string, string>>> GetListRestaurant(string Language)
        {
            return await GetHRMS_Basic_Code(BasicCodeTypeConstant.Restaurant, Language);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListWorkLocation(string Language)
        {
            return await GetHRMS_Basic_Code(BasicCodeTypeConstant.WorkLocation, Language);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListReasonResignation(string Language)
        {
            return await GetHRMS_Basic_Code(BasicCodeTypeConstant.ReasonResignation, Language);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListWorkTypeShift(string Language)
        {
            return await GetHRMS_Basic_Code(BasicCodeTypeConstant.WorkShiftType, Language);
        }

        #endregion

        #region Private Function

        private async Task<List<KeyValuePair<string, string>>> GetHRMS_Basic_Code(string Type_Seq, string Language)
        {
            return await _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => x.Type_Seq == Type_Seq, true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == Language.ToLower(), true),
                    HBC => new { HBC.Type_Seq, HBC.Code },
                    HBCL => new { HBCL.Type_Seq, HBCL.Code },
                    (HBC, HBCL) => new { HBC, HBCL })
                    .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (prev, HBCL) => new { prev.HBC, HBCL })
                .Select(x => new KeyValuePair<string, string>(x.HBC.Code, $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"))
                .ToListAsync();
        }

        private async Task<List<KeyValuePair<string, string>>> GetHRMS_Basic_Code_Char1(string Type_Seq, string char1, string Language)
        {
            return await _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => x.Type_Seq == Type_Seq && x.Char1.ToLower() == char1.ToLower(), true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == Language.ToLower(), true),
                    HBC => new { HBC.Type_Seq, HBC.Code },
                    HBCL => new { HBCL.Type_Seq, HBCL.Code },
                    (HBC, HBCL) => new { HBC, HBCL })
                    .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (prev, HBCL) => new { prev.HBC, HBCL })
                .Select(x => new KeyValuePair<string, string>(x.HBC.Code, $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"))
                .ToListAsync();
        }
        #endregion
    }
}