
using AgileObjects.AgileMapper;
using API.Data;
using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.EmployeeMaintenance
{
    public class S_4_2_1_EmployeeBasicInformationReport : BaseServices, I_4_2_1_EmployeeBasicInformationReport
    {
        public S_4_2_1_EmployeeBasicInformationReport(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult> GetPagination(EmployeeBasicInformationReport_Param param, List<string> roleList)
        {
            var result = await GetData(param, roleList);
            return new OperationResult() { Data = result.Count };
        }

        private async Task<List<EmployeeBasicInformationReport_Report>> GetData(EmployeeBasicInformationReport_Param param, List<string> roleList)
        {
            #region PredicateBuilder
            var pred = PredicateBuilder.New<HRMS_Emp_Personal>(true);
            var leavePred = PredicateBuilder.New<HRMS_Emp_Unpaid_Leave>(x => x.Effective_Status);
            var departmentLanguagePred = PredicateBuilder.New<HRMS_Org_Department_Language>(x => x.Language_Code.ToLower() == param.Language.ToLower());

            if (!string.IsNullOrWhiteSpace(param.Nationality))
                pred.And(x => x.Nationality == param.Nationality);

            if (!string.IsNullOrWhiteSpace(param.IdentificationNumber))
                pred.And(x => x.Identification_Number.Contains(param.IdentificationNumber));

            if (!string.IsNullOrWhiteSpace(param.Division))
            {
                pred.And(x => x.Division == param.Division);
                leavePred.And(x => x.Division == param.Division);
                departmentLanguagePred.And(x => x.Division == param.Division);
            }

            if (!string.IsNullOrWhiteSpace(param.Factory))
            {
                pred.And(x => x.Factory == param.Factory);
                leavePred.And(x => x.Factory == param.Factory);
                departmentLanguagePred.And(x => x.Factory == param.Factory);
            }

            if (!string.IsNullOrWhiteSpace(param.EmployeeID))
            {
                pred.And(x => x.Employee_ID.Contains(param.EmployeeID));
                leavePred.And(x => x.Employee_ID.Contains(param.EmployeeID));
            }

            if (!string.IsNullOrWhiteSpace(param.Department))
                pred.And(x => x.Department == param.Department);

            if (!string.IsNullOrWhiteSpace(param.AssignedDivision))
                pred.And(x => x.Assigned_Division == param.AssignedDivision);

            if (!string.IsNullOrWhiteSpace(param.AssignedFactory))
                pred.And(x => x.Assigned_Factory == param.AssignedFactory);

            if (!string.IsNullOrWhiteSpace(param.AssignedEmployeeID))
                pred.And(x => x.Assigned_Employee_ID.Contains(param.AssignedEmployeeID));

            if (!string.IsNullOrWhiteSpace(param.PermissionGroup))
                pred.And(x => x.Permission_Group == param.PermissionGroup);

            if (!string.IsNullOrWhiteSpace(param.AssignedDepartment))
                pred.And(x => x.Assigned_Department == param.AssignedDepartment);



            var result = new List<EmployeeBasicInformationReport_Report>();

            var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll(pred).ToList();
            if (!HEP.Any()) return result;

            if (!string.IsNullOrWhiteSpace(param.OnboardDateStartStr))
                HEP = HEP.Where(x => x.Onboard_Date.Date >= Convert.ToDateTime(param.OnboardDateStartStr).Date).ToList();
            if (!string.IsNullOrWhiteSpace(param.OnboardDateEndStr))
                HEP = HEP.Where(x => x.Onboard_Date.Date <= Convert.ToDateTime(param.OnboardDateEndStr).Date).ToList();

            if (!string.IsNullOrWhiteSpace(param.DateOfGroupEmploymentStart))
                HEP = HEP.Where(x => x.Group_Date.Date >= Convert.ToDateTime(param.DateOfGroupEmploymentStart).Date).ToList();
            if (!string.IsNullOrWhiteSpace(param.DateOfGroupEmploymentEnd))
                HEP = HEP.Where(x => x.Group_Date.Date <= Convert.ToDateTime(param.DateOfGroupEmploymentEnd).Date).ToList();

            if (!string.IsNullOrWhiteSpace(param.DateOfResignationStart))
                HEP = HEP.Where(x => x.Resign_Date.HasValue && x.Resign_Date.Value.Date >= Convert.ToDateTime(param.DateOfResignationStart).Date).ToList();
            if (!string.IsNullOrWhiteSpace(param.DateOfResignationEnd))
                HEP = HEP.Where(x => x.Resign_Date.HasValue && x.Resign_Date.Value.Date <= Convert.ToDateTime(param.DateOfResignationEnd).Date).ToList();
            if (!string.IsNullOrWhiteSpace(param.PositionGrade_Start))
                HEP = HEP.Where(x => x.Position_Grade >= decimal.Parse(param.PositionGrade_Start)).ToList();
            if (!string.IsNullOrWhiteSpace(param.PositionGrade_End))
                HEP = HEP.Where(x => x.Position_Grade <= decimal.Parse(param.PositionGrade_End)).ToList();
            List<HRMS_Emp_Personal_Next> HEP_next = Mapper.Map(HEP).ToANew<List<HRMS_Emp_Personal_Next>>(x => x.MapEntityKeys());
            var leaveUnpairs = await _repositoryAccessor.HRMS_Emp_Unpaid_Leave.FindAll(leavePred).Select(x => x.Employee_ID).ToListAsync();
            foreach (var item in HEP_next)
                item.Employee_Status = item.Deletion_Code == "N" ? item.Deletion_Code : leaveUnpairs.Contains(item.Employee_ID) ? "U" : "Y";

            if (!string.IsNullOrWhiteSpace(param.EmploymentStatus) && param.EmploymentStatus != "all")
                HEP_next = HEP_next.Where(x => x.Employee_Status == param.EmploymentStatus).ToList();

            var reasonResign = await GetDataBasicCode(BasicCodeTypeConstant.ReasonResignation, param.Language);
            var workLocations = await GetDataBasicCode(BasicCodeTypeConstant.WorkLocation, param.Language);
            var educations = await GetDataBasicCode(BasicCodeTypeConstant.Education, param.Language);
            var transportationMethods = await GetDataBasicCode(BasicCodeTypeConstant.TransportationMethod, param.Language);
            var provinces = await GetDataBasicCode(BasicCodeTypeConstant.Province, param.Language);
            var cities = await GetDataBasicCode(BasicCodeTypeConstant.City, param.Language);
            var workShiftTypes = await GetDataBasicCode(BasicCodeTypeConstant.WorkShiftType, param.Language);
            var jobTitles = await GetDataBasicCode(BasicCodeTypeConstant.JobTitle, param.Language);
            var workTypes = await GetDataBasicCode(BasicCodeTypeConstant.WorkType, param.Language);

            var permissionGroups = await GetDataBasicCode(BasicCodeTypeConstant.PermissionGroup, param.Language);
            var performanceDivisions = await GetDataBasicCode(BasicCodeTypeConstant.Division, param.Language);
            var identityTypes = await GetDataBasicCode(BasicCodeTypeConstant.IdentityType, param.Language);
            var restaurants = await GetDataBasicCode(BasicCodeTypeConstant.Restaurant, param.Language);
            var religions = await GetDataBasicCode(BasicCodeTypeConstant.Religion, param.Language);

            var documents = await _repositoryAccessor.HRMS_Emp_Document.FindAll(x => x.Division == param.Division && x.Factory == param.Factory && x.Document_Type == "01").ToListAsync();

            var org_Department = _repositoryAccessor.HRMS_Org_Department.FindAll(true);
            var org_Department_Language = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(departmentLanguagePred, true);
            var depLang = org_Department
                .GroupJoin(org_Department_Language,
                    x => new { x.Factory, x.Division, x.Department_Code },
                    y => new { y.Factory, y.Division, y.Department_Code },
                    (x, y) => new { HOD = x, HODL = y })
                .SelectMany(x => x.HODL.DefaultIfEmpty(),
                    (x, y) => new { x.HOD, HODL = y })
                .Select(x => new
                {
                    x.HOD.Division,
                    x.HOD.Factory,
                    x.HOD.Department_Code,
                    Department_Name = x.HODL != null ? x.HODL.Name : x.HOD.Department_Name
                }).ToHashSet();
            var emp_Exit_History = _repositoryAccessor.HRMS_Emp_Exit_History.FindAll(true).ToList();
            var emp_Unpaid_Leave = _repositoryAccessor.HRMS_Emp_Unpaid_Leave.FindAll(leavePred);
            var data = HEP_next
                .Distinct()
                // reasonResign
                .GroupJoin(reasonResign,
                    HEP => new { HEP.Resign_Reason },
                    reason => new { Resign_Reason = reason.Key },
                    (x, reason) => new { HEP = x, reason })
                .SelectMany(x => x.reason.DefaultIfEmpty(),
                    (x, reason) => new { x.HEP, reason })
                // workLocations
                .GroupJoin(workLocations,
                    last => new { last.HEP.Work_Location },
                    workLocation => new { Work_Location = workLocation.Key },
                    (x, workLocation) => new { x.HEP, x.reason, workLocation })
                .SelectMany(x => x.workLocation.DefaultIfEmpty(),
                    (x, workLocation) => new { x.HEP, x.reason, workLocation })
                // educations
                .GroupJoin(educations,
                    last => new { last.HEP.Education },
                    education => new { Education = education.Key },
                    (x, education) => new { x.HEP, x.reason, x.workLocation, education })
                .SelectMany(x => x.education.DefaultIfEmpty(),
                    (x, education) => new { x.HEP, x.reason, x.workLocation, education })

                // transportationMethods
                .GroupJoin(transportationMethods,
                    last => new { last.HEP.Transportation_Method },
                    transportationMethod => new { Transportation_Method = transportationMethod.Key },
                    (x, transportationMethod) => new { x.HEP, x.reason, x.workLocation, x.education, transportationMethod })
                .SelectMany(x => x.transportationMethod.DefaultIfEmpty(),
                    (x, transportationMethod) => new { x.HEP, x.reason, x.workLocation, x.education, transportationMethod })
                // registeredProvinceDirectlies
                .GroupJoin(provinces,
                    last => new { last.HEP.Registered_Province_Directly },
                    registeredProvinceDirectlies => new { Registered_Province_Directly = registeredProvinceDirectlies.Key },
                    (x, registeredProvinceDirectlies) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, registeredProvinceDirectlies })
                .SelectMany(x => x.registeredProvinceDirectlies.DefaultIfEmpty(),
                    (x, registeredProvinceDirectlies) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, registeredProvinceDirectlies })
                // registeredCities
                .GroupJoin(cities,
                    last => new { last.HEP.Registered_City },
                    registeredCity => new { Registered_City = registeredCity.Key },
                    (x, registeredCity) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, registeredCity })
                .SelectMany(x => x.registeredCity.DefaultIfEmpty(),
                    (x, registeredCity) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, registeredCity })
                // workShiftTypes
                .GroupJoin(workShiftTypes,
                    last => new { last.HEP.Work_Shift_Type },
                    workShiftType => new { Work_Shift_Type = workShiftType.Key },
                    (x, workShiftType) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, x.registeredCity, workShiftType })
                .SelectMany(x => x.workShiftType.DefaultIfEmpty(),
                    (x, workShiftType) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, x.registeredCity, workShiftType })
                // jobTitles - PositionTitle
                .GroupJoin(jobTitles,
                    last => new { last.HEP.Position_Title },
                    jobTitle => new { Position_Title = jobTitle.Key },
                    (x, jobTitle) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, x.registeredCity, x.workShiftType, jobTitle })
                .SelectMany(x => x.jobTitle.DefaultIfEmpty(),
                    (x, jobTitle) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, x.registeredCity, x.workShiftType, jobTitle })
                // workTypes
                .GroupJoin(workTypes,
                    last => new { last.HEP.Work_Type },
                    workType => new { Work_Type = workType.Key },
                    (x, workType) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, x.registeredCity, x.workShiftType, x.jobTitle, workType })
                .SelectMany(x => x.workType.DefaultIfEmpty(),
                    (x, workType) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, x.registeredCity, x.workShiftType, x.jobTitle, workType })
                // permissionGroups
                .GroupJoin(permissionGroups,
                    last => new { last.HEP.Permission_Group },
                    permissionGroup => new { Permission_Group = permissionGroup.Key },
                    (x, permissionGroup) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, x.registeredCity, x.workShiftType, x.jobTitle, x.workType, permissionGroup })
                .SelectMany(x => x.permissionGroup.DefaultIfEmpty(),
                    (x, permissionGroup) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, x.registeredCity, x.workShiftType, x.jobTitle, x.workType, permissionGroup })
                // performanceDivisions
                .GroupJoin(performanceDivisions,
                    last => new { last.HEP.Performance_Division },
                    performanceDivision => new { Performance_Division = performanceDivision.Key },
                    (x, performanceDivision) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, x.registeredCity, x.workShiftType, x.jobTitle, x.workType, x.permissionGroup, performanceDivision })
                .SelectMany(x => x.performanceDivision.DefaultIfEmpty(),
                    (x, performanceDivision) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, x.registeredCity, x.workShiftType, x.jobTitle, x.workType, x.permissionGroup, performanceDivision })
                // identityTypes
                .GroupJoin(identityTypes,
                    last => new { last.HEP.Identity_Type },
                    identityType => new { Identity_Type = identityType.Key },
                    (x, identityType) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, x.registeredCity, x.workShiftType, x.jobTitle, x.workType, x.permissionGroup, x.performanceDivision, identityType })
                .SelectMany(x => x.identityType.DefaultIfEmpty(),
                    (x, identityType) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, x.registeredCity, x.workShiftType, x.jobTitle, x.workType, x.permissionGroup, x.performanceDivision, identityType })
                // restaurants
                .GroupJoin(restaurants,
                    last => new { last.HEP.Restaurant },
                    restaurant => new { Restaurant = restaurant.Key },
                    (x, restaurant) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, x.registeredCity, x.workShiftType, x.jobTitle, x.workType, x.permissionGroup, x.performanceDivision, x.identityType, restaurant })
                .SelectMany(x => x.restaurant.DefaultIfEmpty(),
                    (x, restaurant) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, x.registeredCity, x.workShiftType, x.jobTitle, x.workType, x.permissionGroup, x.performanceDivision, x.identityType, restaurant })
                // religions
                .GroupJoin(religions,
                    last => new { last.HEP.Religion },
                    religion => new { Religion = religion.Key },
                    (x, religion) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, x.registeredCity, x.workShiftType, x.jobTitle, x.workType, x.permissionGroup, x.performanceDivision, x.identityType, x.restaurant, religion })
                .SelectMany(x => x.religion.DefaultIfEmpty(),
                    (x, religion) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, x.registeredCity, x.workShiftType, x.jobTitle, x.workType, x.permissionGroup, x.performanceDivision, x.identityType, x.restaurant, religion })
                // mailingProvinceDirectlys
                .GroupJoin(provinces,
                    last => new { last.HEP.Mailing_Province_Directly },
                    mailingProvinceDirectly => new { Mailing_Province_Directly = mailingProvinceDirectly.Key },
                    (x, mailingProvinceDirectly) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, x.registeredCity, x.workShiftType, x.jobTitle, x.workType, x.permissionGroup, x.performanceDivision, x.identityType, x.restaurant, x.religion, mailingProvinceDirectly })
                .SelectMany(x => x.mailingProvinceDirectly.DefaultIfEmpty(),
                    (x, mailingProvinceDirectly) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, x.registeredCity, x.workShiftType, x.jobTitle, x.workType, x.permissionGroup, x.performanceDivision, x.identityType, x.restaurant, x.religion, mailingProvinceDirectly })
                // mailingCitys
                .GroupJoin(cities,
                    last => new { last.HEP.Mailing_City },
                    mailingCity => new { Mailing_City = mailingCity.Key },
                    (x, mailingCity) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, x.registeredCity, x.workShiftType, x.jobTitle, x.workType, x.permissionGroup, x.performanceDivision, x.identityType, x.restaurant, x.religion, x.mailingProvinceDirectly, mailingCity })
                .SelectMany(x => x.mailingCity.DefaultIfEmpty(),
                    (x, mailingCity) => new { x.HEP, x.reason, x.workLocation, x.education, x.transportationMethod, x.registeredProvinceDirectlies, x.registeredCity, x.workShiftType, x.jobTitle, x.workType, x.permissionGroup, x.performanceDivision, x.identityType, x.restaurant, x.religion, x.mailingProvinceDirectly, mailingCity })
                .ToList();
            data.ForEach(x =>
            {
                var inputData = new EmployeeBasicInformationReport_Report
                {
                    IssuedDate = x.HEP.Issued_Date,
                    IdentificationNumber = x.HEP.Identification_Number,
                    EmploymentStatus = x.HEP.Employee_Status == "N"
                                                            ? (param.Language.ToLower() == "en" ? "N.Resigned" : "N.離職")
                                                            : x.HEP.Employee_Status == "U"
                                                                ? (param.Language.ToLower() == "en" ? "U.Unpaid" : "U.留停")
                                                                : x.HEP.Employee_Status == "Y"
                                                                    ? (param.Language.ToLower() == "en" ? "Y.On job" : "Y.在職")
                                                                    : "",
                    Division = x.HEP.Division,
                    Factory = x.HEP.Factory,
                    EmployeeID = x.HEP.Employee_ID,
                    Department = x.HEP.Department,
                    DepartmentName = !string.IsNullOrWhiteSpace(x.HEP.Department)
                        ? depLang.FirstOrDefault(y => y.Division == x.HEP.Division && y.Factory == x.HEP.Factory && y.Department_Code == x.HEP.Department)?.Department_Name
                        : "",
                    AssignedDivision = x.HEP.Assigned_Division,
                    AssignedFactory = x.HEP.Assigned_Factory,
                    AssignedEmployeeID = x.HEP.Assigned_Employee_ID,
                    AssignedDepartment = x.HEP.Assigned_Department,
                    AssignedDepartmentName = !string.IsNullOrWhiteSpace(x.HEP.Assigned_Department)
                        ? depLang.FirstOrDefault(y => y.Division == x.HEP.Assigned_Division && y.Factory == x.HEP.Assigned_Factory && y.Department_Code == x.HEP.Assigned_Department)?.Department_Name
                        : "",
                    PermissionGroup = $"{x.permissionGroup.Value}",
                    CrossFactoryStatus = string.IsNullOrWhiteSpace(x.HEP.Employment_Status) ? "" :
                                                    x.HEP.Employment_Status == "A" ? (param.Language.ToLower() == "en" ? "A.Assigned" : "A.派駐")
                                                    : x.HEP.Employment_Status == "S" ? (param.Language.ToLower() == "en" ? "S.Supported" : "B.支援") : "",
                    PerformanceAssessmentResponsibilityDivision = $"{x.performanceDivision.Value}",
                    IdentityType = $"{x.identityType.Value}",
                    LocalFullName = x.HEP.Local_Full_Name,
                    PreferredEnglishFullName = x.HEP.Preferred_English_Full_Name,
                    ChineseName = x.HEP.Chinese_Name,
                    PassportFullName = GetPassportFullName(documents, x.HEP.Employee_ID),
                    Gender = x.HEP.Gender == "F" ? (param.Language.ToLower() == "en" ? "F.Female" : "F.女") : (param.Language.ToLower() == "en" ? "M.Male" : "M.男"),
                    MaritalStatus = x.HEP.Marital_Status == "F" ? (param.Language.ToLower() == "en" ? "M.Married" : "M.已婚") :
                                                        x.HEP.Marital_Status == "U" ? (param.Language.ToLower() == "en" ? "U.Unmarried" : "U.未婚")
                                                        : (param.Language.ToLower() == "en" ? "O.Other" : "O.其他"),
                    DateOfBirth = x.HEP.Birthday,
                    WorkShiftType = $"{x.workShiftType.Value}",
                    Work8hoursStr = x.HEP.Work8hours.HasValue ? x.HEP.Work8hours.Value ? "Y" : "N" : "",
                    SalaryType = "",
                    SwipeCardOptionStr = x.HEP.Swipe_Card_Option ? "Y" : "N",
                    SwipeCardNumber = x.HEP.Swipe_Card_Number,
                    PositionGradeStr = x.HEP.Position_Grade.ToString(),
                    PositionTitle = $"{x.jobTitle.Value}",
                    WorkType = $"{x.workType.Value}",
                    OnboardDate = x.HEP.Onboard_Date,
                    DateOfGroupEmployment = x.HEP.Group_Date,
                    SeniorityStartDate = x.HEP.Seniority_Start_Date,
                    AnnualLeaveSeniorityStartDate = x.HEP.Annual_Leave_Seniority_Start_Date,
                    DateOfResignation = x.HEP.Resign_Date,
                    ReasonResignation = $"{x.reason.Value}",
                    BlacklistStr = x.HEP.Blacklist.HasValue ? x.HEP.Blacklist.Value ? "Y" : "N" : "",
                    Restaurant = $"{x.restaurant.Value}",
                    WorkLocation = $"{x.workLocation.Value}",
                    UnionMembershipStr = x.HEP.Union_Membership.HasValue ? x.HEP.Union_Membership.Value ? "Y" : "N" : "",
                    PhoneNumber = x.HEP.Phone_Number,
                    MobilePhoneNumber = x.HEP.Mobile_Phone_Number,
                    Education = $"{x.education.Value}",
                    Religion = $"{x.religion.Value}",
                    TransportationMethod = $"{x.transportationMethod.Value}",
                    RegisteredProvinceDirectly = $"{x.registeredProvinceDirectlies.Value}",
                    RegisteredCity = $"{x.registeredCity.Value}",
                    RegisteredAddress = x.HEP.Registered_Address,
                    MailingProvinceDirectly = $"{x.mailingProvinceDirectly.Value}",
                    MailingCity = $"{x.mailingCity.Value}",
                    MailingAddress = x.HEP.Mailing_Address,
                    UpdateBy = x.HEP.Update_By,
                    UpdateTime = x.HEP.Update_Time
                };
                result.Add(inputData);

            });

            return result.Distinct().ToList();

            #endregion
        }

        private static string GetPassportFullName(List<HRMS_Emp_Document> documents, string employeeId)
        {
            return documents.Where(x => x.Employee_ID == employeeId)
                        .OrderByDescending(d => d.Validity_Start)
                        .Select(d => new
                        {
                            PassportName = d.Passport_Name,
                            MaxValidityDateFrom = d.Validity_Start
                        }).FirstOrDefault()?.PassportName;
        }


        public async Task<OperationResult> ExportExcel(EmployeeBasicInformationReport_Param param, List<string> roleList)
        {
            var data = await GetData(param, roleList);
            if (!data.Any()) return new OperationResult(false, "No Data");

            // xử lí report data 
            var dataTables = new List<Table>() { new("result", data) };

            // Thông tin print [Factory, PrintBy,  PrintDay]
            var dataCells = new List<Cell>(){
                new("B2", param.UserName),
                new("D2", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))
            };

            ConfigDownload config = new() { IsAutoFitColumn = true };
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                dataTables, 
                dataCells, 
                "Resources\\Template\\EmployeeMaintenance\\4_2_1_EmployeeBasicInformationReport\\Report.xlsx", 
                config
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListNationality(string language)
        {
            return await GetDataBasicCode(BasicCodeTypeConstant.Nationality, language);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListDivision(string language)
        {
            return await GetDataBasicCode(BasicCodeTypeConstant.Division, language);
        }

        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string division, List<string> roleList, string language)
        {
            var factory_Addlist = await Queryt_Factory_AddList(roleList);
            var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory && factory_Addlist.Contains(x.Code));
            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower());
            var HBFC = _repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(x => x.Kind == "1" && x.Division == division);
            var result = HBC
                .Join(HBFC,
                    x => new { Factory = x.Code },
                    y => new { y.Factory },
                    (x, y) => new { HBC = x, HBFC = y })
                .GroupJoin(HBCL,
                    x => new { x.HBC.Type_Seq, x.HBC.Code },
                    y => new { y.Type_Seq, y.Code },
                    (x, y) => new { x.HBC, x.HBFC, HBCL = y })
                .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (x, y) => new { x.HBC, x.HBFC, HBCL = y })
                .Select(x => new KeyValuePair<string, string>(x.HBC.Code, $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"))
                .ToList();
            return result;
        }


        public async Task<List<KeyValuePair<string, string>>> GetListDepartment(string division, string factory, string language)
        {
            return await _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Division == division && x.Factory == factory, true)
                .GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    HOD => new { HOD.Division, HOD.Factory, HOD.Department_Code },
                    HODL => new { HODL.Division, HODL.Factory, HODL.Department_Code },
                    (HOD, HODL) => new { HOD, HODL })
                    .SelectMany(x => x.HODL.DefaultIfEmpty(),
                    (prev, HODL) => new { prev.HOD, HODL })
                .Select(x => new KeyValuePair<string, string>(x.HOD.Department_Code, $"{x.HOD.Department_Code} - {(x.HODL != null ? x.HODL.Name : x.HOD.Department_Name)}"))
                .ToListAsync();
        }

        public async Task<List<KeyValuePair<string, string>>> GetListPermission(string language)
        {
            return await GetDataBasicCode(BasicCodeTypeConstant.PermissionGroup, language);
        }
        public async Task<List<KeyValuePair<decimal, decimal>>> GetListPositonGrade()
        {
            return await _repositoryAccessor.HRMS_Basic_Level.FindAll(true).Select(x => new KeyValuePair<decimal, decimal>(
                x.Level, x.Level
            )).Distinct().ToListAsync();
        }
    }
}