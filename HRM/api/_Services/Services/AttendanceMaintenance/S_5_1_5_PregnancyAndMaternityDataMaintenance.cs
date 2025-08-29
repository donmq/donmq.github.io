using API.Data;
using API._Services.Interfaces.AttendanceMaintenance;
using API.DTOs;
using API.DTOs.AttendanceMaintenance;
using API.Helper.Constant;
using API.Models;
using Aspose.Cells;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.AttendanceMaintenance
{
    public class S_5_1_5_PregnancyAndMaternityDataMaintenance : BaseServices, I_5_1_5_PregnancyAndMaternityDataMaintenance
    {

        public S_5_1_5_PregnancyAndMaternityDataMaintenance(DBContext dbContext) : base(dbContext) { }

        #region Add
        public async Task<OperationResult> Add(PregnancyMaternityDetail dto)
        {
            try
            {
                DateTime due_date = Convert.ToDateTime(dto.Due_Date_Str);
                if (await _repositoryAccessor.HRMS_Att_Pregnancy_Data
                    .AnyAsync(x => x.Factory == dto.Factory && x.Employee_ID == dto.Employee_ID && x.Due_Date.Date == due_date.Date))
                    return new OperationResult(false, "Data existed");

                if (await _repositoryAccessor.HRMS_Att_Pregnancy_Data
                    .AnyAsync(x => x.Factory == dto.Factory && x.Employee_ID == dto.Employee_ID && x.Close_Case == false))
                    return new OperationResult(false, "The previous case has not been closed");

                var personalData = await _repositoryAccessor.HRMS_Emp_Personal
                    .FirstOrDefaultAsync(x => x.USER_GUID == dto.USER_GUID && x.Factory == dto.Factory && x.Employee_ID == dto.Employee_ID);

                if (personalData == null)
                    return new OperationResult(false, "HRMS_Emp_Personal not found");

                HRMS_Att_Pregnancy_Data data = new()
                {
                    USER_GUID = dto.USER_GUID,
                    Factory = dto.Factory,
                    Employee_ID = dto.Employee_ID,
                    Department = dto.Department_Code,
                    Due_Date = due_date,
                    Baby_Start = !string.IsNullOrWhiteSpace(dto.Baby_Start_Str) ? Convert.ToDateTime(dto.Baby_Start_Str) : null,
                    Baby_End = !string.IsNullOrWhiteSpace(dto.Baby_End_Str) ? Convert.ToDateTime(dto.Baby_End_Str) : null,
                    GoWork_Date = !string.IsNullOrWhiteSpace(dto.GoWork_Date_Str) ? Convert.ToDateTime(dto.GoWork_Date_Str) : null,
                    Estimated_Date1 = !string.IsNullOrWhiteSpace(dto.Estimated_Date1_Str) ? Convert.ToDateTime(dto.Estimated_Date1_Str) : null,
                    Estimated_Date2 = !string.IsNullOrWhiteSpace(dto.Estimated_Date2_Str) ? Convert.ToDateTime(dto.Estimated_Date2_Str) : null,
                    Estimated_Date3 = !string.IsNullOrWhiteSpace(dto.Estimated_Date3_Str) ? Convert.ToDateTime(dto.Estimated_Date3_Str) : null,
                    Estimated_Date4 = !string.IsNullOrWhiteSpace(dto.Estimated_Date4_Str) ? Convert.ToDateTime(dto.Estimated_Date4_Str) : null,
                    Estimated_Date5 = !string.IsNullOrWhiteSpace(dto.Estimated_Date5_Str) ? Convert.ToDateTime(dto.Estimated_Date5_Str) : null,
                    Insurance_Date1 = !string.IsNullOrWhiteSpace(dto.Insurance_Date1_Str) ? Convert.ToDateTime(dto.Insurance_Date1_Str) : null,
                    Insurance_Date2 = !string.IsNullOrWhiteSpace(dto.Insurance_Date2_Str) ? Convert.ToDateTime(dto.Insurance_Date2_Str) : null,
                    Insurance_Date3 = !string.IsNullOrWhiteSpace(dto.Insurance_Date3_Str) ? Convert.ToDateTime(dto.Insurance_Date3_Str) : null,
                    Insurance_Date4 = !string.IsNullOrWhiteSpace(dto.Insurance_Date4_Str) ? Convert.ToDateTime(dto.Insurance_Date4_Str) : null,
                    Insurance_Date5 = !string.IsNullOrWhiteSpace(dto.Insurance_Date5_Str) ? Convert.ToDateTime(dto.Insurance_Date5_Str) : null,
                    Leave_Date1 = !string.IsNullOrWhiteSpace(dto.Leave_Date1_Str) ? Convert.ToDateTime(dto.Leave_Date1_Str) : null,
                    Leave_Date2 = !string.IsNullOrWhiteSpace(dto.Leave_Date2_Str) ? Convert.ToDateTime(dto.Leave_Date2_Str) : null,
                    Leave_Date3 = !string.IsNullOrWhiteSpace(dto.Leave_Date3_Str) ? Convert.ToDateTime(dto.Leave_Date3_Str) : null,
                    Leave_Date4 = !string.IsNullOrWhiteSpace(dto.Leave_Date4_Str) ? Convert.ToDateTime(dto.Leave_Date4_Str) : null,
                    Leave_Date5 = !string.IsNullOrWhiteSpace(dto.Leave_Date5_Str) ? Convert.ToDateTime(dto.Leave_Date5_Str) : null,
                    Maternity_Start = !string.IsNullOrWhiteSpace(dto.Maternity_Start_Str) ? Convert.ToDateTime(dto.Maternity_Start_Str) : null,
                    Maternity_End = !string.IsNullOrWhiteSpace(dto.Maternity_End_Str) ? Convert.ToDateTime(dto.Maternity_End_Str) : null,
                    Pregnancy36Weeks = !string.IsNullOrWhiteSpace(dto.Pregnancy36Weeks_Str) ? Convert.ToDateTime(dto.Pregnancy36Weeks_Str) : null,
                    Ultrasound_Date = !string.IsNullOrWhiteSpace(dto.Ultrasound_Date_Str) ? Convert.ToDateTime(dto.Ultrasound_Date_Str) : null,
                    Work7hours = !string.IsNullOrWhiteSpace(dto.Work7hours_Str) ? Convert.ToDateTime(dto.Work7hours_Str) : null,
                    Close_Case = dto.Close_Case,
                    Pregnancy_Week = decimal.Parse(dto.Pregnancy_Week),
                    Remark = dto.Remark,
                    Work_Type_Before = dto.Work_Type_Before,
                    Work_Type_After = dto.Work_Type_After,
                    Update_By = dto.Update_By,
                    Update_Time = DateTime.Now
                };

                _repositoryAccessor.HRMS_Att_Pregnancy_Data.Add(data);
                if (!string.IsNullOrWhiteSpace(dto.Work7hours_Str))
                {
                    personalData.Work8hours = true;
                    _repositoryAccessor.HRMS_Emp_Personal.Update(personalData);
                }
                await _repositoryAccessor.Save();
                return new OperationResult(true, "Create data successfully");
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.ToString());
            }
        }
        #endregion

        #region Delete
        public async Task<OperationResult> Delete(PregnancyMaternityDetail dto)
        {
            DateTime due_date = Convert.ToDateTime(dto.Due_Date_Str);

            var data = await _repositoryAccessor.HRMS_Att_Pregnancy_Data
                .FirstOrDefaultAsync(x => x.Factory == dto.Factory && x.Employee_ID == dto.Employee_ID && x.Due_Date.Date == due_date.Date);
            if (data is null)
                return new OperationResult(false, "Data not existed");

            try
            {
                _repositoryAccessor.HRMS_Att_Pregnancy_Data.Remove(data);
                await _repositoryAccessor.Save();
                return new OperationResult(true, "Delete data successfully");
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.ToString());
            }
        }
        #endregion

        #region Edit
        public async Task<OperationResult> Edit(PregnancyMaternityDetail dto)
        {
            try
            {
                DateTime due_date = Convert.ToDateTime(dto.Due_Date_Str);
                var data = await _repositoryAccessor.HRMS_Att_Pregnancy_Data
                    .FirstOrDefaultAsync(x => x.Factory == dto.Factory && x.Employee_ID == dto.Employee_ID && x.Due_Date.Date == due_date.Date);
                if (data is null)
                    return new OperationResult(false, "Data not existed");

                var personalData = await _repositoryAccessor.HRMS_Emp_Personal
                    .FirstOrDefaultAsync(x => x.USER_GUID == dto.USER_GUID && x.Factory == dto.Factory && x.Employee_ID == dto.Employee_ID);
                if (personalData == null)
                    return new OperationResult(false, "HRMS_Emp_Personal not found");

                data.Baby_Start = !string.IsNullOrWhiteSpace(dto.Baby_Start_Str) ? Convert.ToDateTime(dto.Baby_Start_Str) : null;
                data.Baby_End = !string.IsNullOrWhiteSpace(dto.Baby_End_Str) ? Convert.ToDateTime(dto.Baby_End_Str) : null;
                data.Close_Case = string.IsNullOrWhiteSpace(dto.Close_Case_Str) ? null : dto.Close_Case_Str == "Y";
                data.GoWork_Date = !string.IsNullOrWhiteSpace(dto.GoWork_Date_Str) ? Convert.ToDateTime(dto.GoWork_Date_Str) : null;
                data.Estimated_Date1 = !string.IsNullOrWhiteSpace(dto.Estimated_Date1_Str) ? Convert.ToDateTime(dto.Estimated_Date1_Str) : null;
                data.Estimated_Date2 = !string.IsNullOrWhiteSpace(dto.Estimated_Date2_Str) ? Convert.ToDateTime(dto.Estimated_Date2_Str) : null;
                data.Estimated_Date3 = !string.IsNullOrWhiteSpace(dto.Estimated_Date3_Str) ? Convert.ToDateTime(dto.Estimated_Date3_Str) : null;
                data.Estimated_Date4 = !string.IsNullOrWhiteSpace(dto.Estimated_Date4_Str) ? Convert.ToDateTime(dto.Estimated_Date4_Str) : null;
                data.Estimated_Date5 = !string.IsNullOrWhiteSpace(dto.Estimated_Date5_Str) ? Convert.ToDateTime(dto.Estimated_Date5_Str) : null;
                data.Insurance_Date1 = !string.IsNullOrWhiteSpace(dto.Insurance_Date1_Str) ? Convert.ToDateTime(dto.Insurance_Date1_Str) : null;
                data.Insurance_Date2 = !string.IsNullOrWhiteSpace(dto.Insurance_Date2_Str) ? Convert.ToDateTime(dto.Insurance_Date2_Str) : null;
                data.Insurance_Date3 = !string.IsNullOrWhiteSpace(dto.Insurance_Date3_Str) ? Convert.ToDateTime(dto.Insurance_Date3_Str) : null;
                data.Insurance_Date4 = !string.IsNullOrWhiteSpace(dto.Insurance_Date4_Str) ? Convert.ToDateTime(dto.Insurance_Date4_Str) : null;
                data.Insurance_Date5 = !string.IsNullOrWhiteSpace(dto.Insurance_Date5_Str) ? Convert.ToDateTime(dto.Insurance_Date5_Str) : null;
                data.Leave_Date1 = !string.IsNullOrWhiteSpace(dto.Leave_Date1_Str) ? Convert.ToDateTime(dto.Leave_Date1_Str) : null;
                data.Leave_Date2 = !string.IsNullOrWhiteSpace(dto.Leave_Date2_Str) ? Convert.ToDateTime(dto.Leave_Date2_Str) : null;
                data.Leave_Date3 = !string.IsNullOrWhiteSpace(dto.Leave_Date3_Str) ? Convert.ToDateTime(dto.Leave_Date3_Str) : null;
                data.Leave_Date4 = !string.IsNullOrWhiteSpace(dto.Leave_Date4_Str) ? Convert.ToDateTime(dto.Leave_Date4_Str) : null;
                data.Leave_Date5 = !string.IsNullOrWhiteSpace(dto.Leave_Date5_Str) ? Convert.ToDateTime(dto.Leave_Date5_Str) : null;
                data.Maternity_Start = !string.IsNullOrWhiteSpace(dto.Maternity_Start_Str) ? Convert.ToDateTime(dto.Maternity_Start_Str) : null;
                data.Maternity_End = !string.IsNullOrWhiteSpace(dto.Maternity_End_Str) ? Convert.ToDateTime(dto.Maternity_End_Str) : null;
                data.Pregnancy36Weeks = !string.IsNullOrWhiteSpace(dto.Pregnancy36Weeks_Str) ? Convert.ToDateTime(dto.Pregnancy36Weeks_Str) : null;
                data.Pregnancy_Week = decimal.Parse(dto.Pregnancy_Week);
                data.Remark = dto.Remark;
                data.Ultrasound_Date = !string.IsNullOrWhiteSpace(dto.Ultrasound_Date_Str) ? Convert.ToDateTime(dto.Ultrasound_Date_Str) : null;
                data.Work_Type_Before = dto.Work_Type_Before;
                data.Work_Type_After = dto.Work_Type_After;

                // Work7hours <= Date.Nnow AND written for the first time
                if (!string.IsNullOrWhiteSpace(dto.Work7hours_Str) && Convert.ToDateTime(dto.Work7hours_Str).Date <= DateTime.Now.Date &&
                    !data.Work7hours.HasValue && personalData.Work8hours != false && personalData.Work_Shift_Type != "S0")
                {
                    personalData.Work8hours = false;
                    personalData.Work_Shift_Type = "S0";
                    _repositoryAccessor.HRMS_Emp_Personal.Update(personalData);
                }
                data.Work7hours = !string.IsNullOrWhiteSpace(dto.Work7hours_Str) ? Convert.ToDateTime(dto.Work7hours_Str) : null;
                data.Update_By = dto.Update_By;
                data.Update_Time = DateTime.Now;
                _repositoryAccessor.HRMS_Att_Pregnancy_Data.Update(data);
                await _repositoryAccessor.Save();
                return new OperationResult(true, "Delete data successfully");
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.ToString());
            }
        }
        #endregion

        #region GetListFactory
        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string language, string userName)
        {
            var factorys = await Queryt_Factory_AddList(userName);
            var factories = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory && factorys.Contains(x.Code), true)
                        .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                                    x => new { x.Type_Seq, x.Code },
                                    y => new { y.Type_Seq, y.Code },
                                    (x, y) => new { x, y })
                                    .SelectMany(x => x.y.DefaultIfEmpty(),
                                    (x, y) => new { x.x, y })
                        .Select(x => new KeyValuePair<string, string>(x.x.Code, $"{x.x.Code} - {(x.y != null ? x.y.Code_Name : x.x.Code_Name)}")).ToListAsync();
            return factories;
        }
        #endregion

        #region GetListDepartment
        public async Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language)
        {
            var pred = PredicateBuilder.New<HRMS_Org_Department>(true);
            var predCom = PredicateBuilder.New<HRMS_Basic_Factory_Comparison>(x => x.Kind == "1");

            if (!string.IsNullOrWhiteSpace(factory))
            {
                pred.And(x => x.Factory == factory);
                predCom.And(x => x.Factory == factory);
            }
            var data = await _repositoryAccessor.HRMS_Org_Department.FindAll(pred)
                .Join(_repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(predCom),
                    department => department.Division,
                    factoryComparison => factoryComparison.Division,
                    (department, factoryComparison) => department)
                .GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    department => new { department.Factory, department.Department_Code },
                    language => new { language.Factory, language.Department_Code },
                    (department, language) => new { Department = department, Language = language })
                .SelectMany(
                    x => x.Language.DefaultIfEmpty(),
                    (x, language) => new { x.Department, Language = language })
                .OrderBy(x => x.Department.Department_Code)
                .Select(
                    x => new KeyValuePair<string, string>(
                        x.Department.Department_Code,
                        $"{x.Department.Department_Code} - {(x.Language != null ? x.Language.Name : x.Department.Department_Name)}"
                    )
                ).Distinct().ToListAsync();

            return data;
        }
        #endregion

        #region GetListWorkType
        public async Task<List<KeyValuePair<string, string>>> GetListWorkType(string language, bool? isWorkShiftType)
        {
            var pred = PredicateBuilder.New<HRMS_Basic_Code>(true);
            if (isWorkShiftType != null && isWorkShiftType == true)
                pred.And(x => x.Type_Seq == BasicCodeTypeConstant.WorkShiftType);
            else
                pred.And(x => x.Type_Seq == BasicCodeTypeConstant.WorkType);

            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(pred, true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    HBC => new { HBC.Type_Seq, HBC.Code },
                    HBCL => new { HBCL.Type_Seq, HBCL.Code },
                    (HBC, HBCL) => new { HBC, HBCL })
                .SelectMany(
                    x => x.HBCL.DefaultIfEmpty(),
                    (prev, HBCL) => new { prev.HBC, HBCL })
                .Select(x => new KeyValuePair<string, string>(x.HBC.Code, $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"))
                .ToListAsync();

            return data;
        }
        #endregion

        #region Query
        public async Task<PaginationUtility<PregnancyMaternityDetail>> Query(PaginationParam pagination, PregnancyMaternityParam param)
        {
            var result = await GetData(param);
            return PaginationUtility<PregnancyMaternityDetail>.Create(result, pagination.PageNumber, pagination.PageSize);
        }
        #endregion

        #region ExportExcel
        public async Task<OperationResult> ExportExcel(PregnancyMaternityParam param, string userName)
        {
            try
            {
                var data = await GetData(param);
                if (!data.Any())
                    return new OperationResult(false, "No data for excel download");
                MemoryStream stream = new();
                var path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Resources\\Template\\AttendanceMaintenance\\5_1_5_Pregnancy_And_Maternity_Data_Maintenance\\Download.xlsx"
                );
                WorkbookDesigner designer = new() { Workbook = new Workbook(path) };
                Worksheet ws = designer.Workbook.Worksheets[0];

                ws.Cells["O2"].PutValue(userName);
                ws.Cells["Q2"].PutValue(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                designer.SetDataSource("result", data);
                designer.Process();

                designer.Workbook.Save(stream, SaveFormat.Xlsx);
                return new OperationResult(true, stream.ToArray());
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.InnerException.Message);
            }
        }
        #endregion

        private async Task<List<PregnancyMaternityDetail>> GetData(PregnancyMaternityParam param)
        {
            var predPregnancy = PredicateBuilder.New<HRMS_Att_Pregnancy_Data>(true);
            var predEmp = PredicateBuilder.New<HRMS_Emp_Personal>(true);
            var permissionGroupQuery = _repositoryAccessor.HRMS_Basic_Role.FindAll(x => x.Factory == param.Factory, true).Select(x => x.Permission_Group);

            if (!string.IsNullOrWhiteSpace(param.Factory))
            {
                predPregnancy.And(x => x.Factory == param.Factory);
                predEmp.And(x => (x.Factory == param.Factory || x.Assigned_Factory == param.Factory) && permissionGroupQuery.Contains(x.Permission_Group));
            }
            if (!string.IsNullOrWhiteSpace(param.Department_Code))
            {
                predPregnancy.And(x => x.Department == param.Department_Code);
                predEmp.And(x => x.Department == param.Department_Code || x.Assigned_Department == param.Department_Code);
            }
            if (!string.IsNullOrWhiteSpace(param.Employee_ID))
            {
                predPregnancy.And(x => x.Employee_ID.Contains(param.Employee_ID.Trim()));
                predEmp.And(x => x.Employee_ID.Contains(param.Employee_ID.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(param.DueDate_Start_Str) && !string.IsNullOrWhiteSpace(param.DueDate_End_Str))
            {
                predPregnancy.And(x => x.Due_Date >= Convert.ToDateTime(param.DueDate_Start_Str));
                predPregnancy.And(x => x.Due_Date <= Convert.ToDateTime(param.DueDate_End_Str));
            }

            if (!string.IsNullOrWhiteSpace(param.MaternityLeave_Start_Str) && !string.IsNullOrWhiteSpace(param.MaternityLeave_End_Str))
            {
                predPregnancy.And(x => x.Maternity_End >= Convert.ToDateTime(param.MaternityLeave_Start_Str));
                predPregnancy.And(x => x.Maternity_End <= Convert.ToDateTime(param.MaternityLeave_End_Str));
            }
            var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.IsActive);
            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower());
            var HBC_WorkShiftType = HBC.Where(x => x.Type_Seq == BasicCodeTypeConstant.WorkShiftType)
                .GroupJoin(HBCL.Where(x => x.Type_Seq == BasicCodeTypeConstant.WorkShiftType),
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (x, y) => new { HBC = x, HBCL = y })
                .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (x, y) => new { x.HBC, HBCL = y })
                .Select(x => new
                {
                    x.HBC.Code,
                    Code_Name = $"{x.HBC.Code}-{(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"
                });
            var HOD = _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Factory == param.Factory);
            var HODL = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower());
            var HOD_Lang = HOD
                .GroupJoin(HODL,
                    x => new { x.Department_Code, x.Factory },
                    y => new { y.Department_Code, y.Factory },
                    (x, y) => new { HOD = x, HODL = y })
                .SelectMany(x => x.HODL.DefaultIfEmpty(),
                    (x, y) => new { x.HOD, HODL = y })
                .Select(x => new
                {
                    x.HOD.Factory,
                    x.HOD.Department_Code,
                    Department_Name = x.HODL != null ? x.HODL.Name : x.HOD.Department_Name
                });

            var HAPD = _repositoryAccessor.HRMS_Att_Pregnancy_Data.FindAll(predPregnancy, true);
            var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll(predEmp, true);
            var HAWTD = _repositoryAccessor.HRMS_Att_Work_Type_Days.FindAll(x => x.Factory == param.Factory && x.Effective_State);

            var result = await HAPD
                .GroupJoin(HEP,
                    x => x.USER_GUID,
                    y => y.USER_GUID,
                    (x, y) => new { HAPD = x, HEP = y })
                .SelectMany(x => x.HEP.DefaultIfEmpty(),
                    (x, y) => new { x.HAPD, HEP = y })
                .GroupJoin(HBC_WorkShiftType,
                    x => x.HEP.Work_Shift_Type,
                    y => y.Code,
                    (x, y) => new { x.HAPD, x.HEP, HBC_WorkShiftType = y })
                .SelectMany(x => x.HBC_WorkShiftType.DefaultIfEmpty(),
                    (x, y) => new { x.HAPD, x.HEP, HBC_WorkShiftType = y })
                .GroupJoin(HOD_Lang,
                    x => new { x.HAPD.Factory, Department_Code = x.HAPD.Department },
                    y => new { y.Factory, y.Department_Code },
                    (x, y) => new { x.HAPD, x.HEP, x.HBC_WorkShiftType, HOD_Lang = y })
                .SelectMany(x => x.HOD_Lang.DefaultIfEmpty(),
                    (x, y) => new { x.HAPD, x.HEP, x.HBC_WorkShiftType, HOD_Lang = y })
                .GroupJoin(HAWTD,
                    x => x.HAPD.Work_Type_Before,
                    y => y.Work_Type,
                    (x, y) => new { x.HAPD, x.HEP, x.HBC_WorkShiftType, x.HOD_Lang, HAWTD = y })
                .SelectMany(
                    x => x.HAWTD.DefaultIfEmpty(),
                    (x, y) => new { x.HAPD, x.HEP, x.HBC_WorkShiftType, x.HOD_Lang, HAWTD = y })
                .Select(x => new PregnancyMaternityDetail
                {
                    // Seq = i + 1,
                    Factory = x.HAPD.Factory,
                    Employee_ID = x.HAPD.Employee_ID,
                    USER_GUID = x.HAPD.USER_GUID,
                    Local_Full_Name = x.HEP.Local_Full_Name,
                    Department_Code = x.HAPD.Department,
                    Department_Name = x.HOD_Lang.Department_Name,
                    Department_Code_Name = x.HOD_Lang != null && !string.IsNullOrWhiteSpace(x.HOD_Lang.Department_Name)
                        ? x.HOD_Lang.Department_Code + "-" + x.HOD_Lang.Department_Name : x.HAPD.Department,
                    Work_Shift_Type = x.HEP.Work_Shift_Type,
                    Work_Shift_Type_Name = x.HBC_WorkShiftType != null ? x.HBC_WorkShiftType.Code_Name : x.HEP.Work_Shift_Type,
                    Due_Date = x.HAPD.Due_Date,
                    Due_Date_Str = x.HAPD.Due_Date.ToString("yyyy/MM/dd"),
                    Work8hours = x.HEP.Work8hours,
                    Work8hours_Str = x.HEP.Work8hours.HasValue && x.HEP.Work8hours.Value ? "Y" : "N",
                    Work7hours = x.HAPD.Work7hours,
                    Work7hours_Str = x.HAPD.Work7hours.HasValue ? x.HAPD.Work7hours.Value.ToString("yyyy/MM/dd") : "",
                    Pregnancy36Weeks = x.HAPD.Pregnancy36Weeks,
                    Pregnancy36Weeks_Str = x.HAPD.Pregnancy36Weeks.HasValue ? x.HAPD.Pregnancy36Weeks.Value.ToString("yyyy/MM/dd") : "",
                    Maternity_Start = x.HAPD.Maternity_Start,
                    Maternity_Start_Str = x.HAPD.Maternity_Start.HasValue ? x.HAPD.Maternity_Start.Value.ToString("yyyy/MM/dd") : "",
                    Maternity_End = x.HAPD.Maternity_End,
                    Maternity_End_Str = x.HAPD.Maternity_End.HasValue ? x.HAPD.Maternity_End.Value.ToString("yyyy/MM/dd") : "",
                    GoWork_Date = x.HAPD.GoWork_Date,
                    GoWork_Date_Str = x.HAPD.GoWork_Date.HasValue ? x.HAPD.GoWork_Date.Value.ToString("yyyy/MM/dd") : "",
                    Close_Case = x.HAPD.Close_Case,
                    Close_Case_Str = x.HAPD.Close_Case.HasValue &&  x.HAPD.Close_Case.Value ? "Y" : "N",
                    Work_Type_Before = x.HAPD.Work_Type_Before,
                    Special_Regular_Work_Type = x.HAWTD != null ? "Y" : "N",
                    Work_Type_After = x.HAPD.Work_Type_After,
                    Pregnancy_Week = x.HAPD.Pregnancy_Week.ToString(),
                    Remark = x.HAPD.Remark,
                    Ultrasound_Date = x.HAPD.Ultrasound_Date,
                    Ultrasound_Date_Str = x.HAPD.Ultrasound_Date.HasValue ? x.HAPD.Ultrasound_Date.Value.ToString("yyyy/MM/dd") : "",
                    Baby_Start = x.HAPD.Baby_Start,
                    Baby_Start_Str = x.HAPD.Baby_Start.HasValue ? x.HAPD.Baby_Start.Value.ToString("yyyy/MM/dd") : "",
                    Baby_End = x.HAPD.Baby_End,
                    Baby_End_Str = x.HAPD.Baby_End.HasValue ? x.HAPD.Baby_End.Value.ToString("yyyy/MM/dd") : "",
                    Estimated_Date1 = x.HAPD.Estimated_Date1,
                    Estimated_Date1_Str = x.HAPD.Estimated_Date1.HasValue ? x.HAPD.Estimated_Date1.Value.ToString("yyyy/MM/dd") : "",
                    Estimated_Date2 = x.HAPD.Estimated_Date2,
                    Estimated_Date2_Str = x.HAPD.Estimated_Date2.HasValue ? x.HAPD.Estimated_Date2.Value.ToString("yyyy/MM/dd") : "",
                    Estimated_Date3 = x.HAPD.Estimated_Date3,
                    Estimated_Date3_Str = x.HAPD.Estimated_Date3.HasValue ? x.HAPD.Estimated_Date3.Value.ToString("yyyy/MM/dd") : "",
                    Estimated_Date4 = x.HAPD.Estimated_Date4,
                    Estimated_Date4_Str = x.HAPD.Estimated_Date4.HasValue ? x.HAPD.Estimated_Date4.Value.ToString("yyyy/MM/dd") : "",
                    Estimated_Date5 = x.HAPD.Estimated_Date5,
                    Estimated_Date5_Str = x.HAPD.Estimated_Date5.HasValue ? x.HAPD.Estimated_Date5.Value.ToString("yyyy/MM/dd") : "",
                    Insurance_Date1 = x.HAPD.Insurance_Date1,
                    Insurance_Date1_Str = x.HAPD.Insurance_Date1.HasValue ? x.HAPD.Insurance_Date1.Value.ToString("yyyy/MM/dd") : "",
                    Insurance_Date2 = x.HAPD.Insurance_Date2,
                    Insurance_Date2_Str = x.HAPD.Insurance_Date2.HasValue ? x.HAPD.Insurance_Date2.Value.ToString("yyyy/MM/dd") : "",
                    Insurance_Date3 = x.HAPD.Insurance_Date3,
                    Insurance_Date3_Str = x.HAPD.Insurance_Date3.HasValue ? x.HAPD.Insurance_Date3.Value.ToString("yyyy/MM/dd") : "",
                    Insurance_Date4 = x.HAPD.Insurance_Date4,
                    Insurance_Date4_Str = x.HAPD.Insurance_Date4.HasValue ? x.HAPD.Insurance_Date4.Value.ToString("yyyy/MM/dd") : "",
                    Insurance_Date5 = x.HAPD.Insurance_Date5,
                    Insurance_Date5_Str = x.HAPD.Insurance_Date5.HasValue ? x.HAPD.Insurance_Date5.Value.ToString("yyyy/MM/dd") : "",
                    Leave_Date1 = x.HAPD.Leave_Date1,
                    Leave_Date1_Str = x.HAPD.Leave_Date1.HasValue ? x.HAPD.Leave_Date1.Value.ToString("yyyy/MM/dd") : "",
                    Leave_Date2 = x.HAPD.Leave_Date2,
                    Leave_Date2_Str = x.HAPD.Leave_Date2.HasValue ? x.HAPD.Leave_Date2.Value.ToString("yyyy/MM/dd") : "",
                    Leave_Date3 = x.HAPD.Leave_Date3,
                    Leave_Date3_Str = x.HAPD.Leave_Date3.HasValue ? x.HAPD.Leave_Date3.Value.ToString("yyyy/MM/dd") : "",
                    Leave_Date4 = x.HAPD.Leave_Date4,
                    Leave_Date4_Str = x.HAPD.Leave_Date4.HasValue ? x.HAPD.Leave_Date4.Value.ToString("yyyy/MM/dd") : "",
                    Leave_Date5 = x.HAPD.Leave_Date5,
                    Leave_Date5_Str = x.HAPD.Leave_Date5.HasValue ? x.HAPD.Leave_Date5.Value.ToString("yyyy/MM/dd") : "",
                    Update_By = x.HAPD.Update_By,
                    Update_Time = x.HAPD.Update_Time,
                    Update_Time_Str = x.HAPD.Update_Time.ToString("yyyy/MM/dd HH:mm:ss"),
                }).OrderBy(x => x.Department_Code).ToListAsync();

            if (!string.IsNullOrWhiteSpace(param.Department_Code))
                result = result.FindAll(x => x.Department_Code == param.Department_Code);
            return result;
        }

        public async Task<object> GetSpecialRegularWorkType(string Factory, string Work_Type_Before)
        {
            return new
            {
                Special_Regular_Work_Type = await _repositoryAccessor.HRMS_Att_Work_Type_Days
                    .AnyAsync(x => x.Factory == Factory && x.Effective_State && x.Work_Type == Work_Type_Before)
                    ? "Y" : "N"
            };
        }
    }
}