
using API.Data;
using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.EmployeeMaintenance
{
    public class S_4_2_2_EmergencyContactsSheetReport : BaseServices, I_4_2_2_EmergencyContactsSheetReport
    {
        public S_4_2_2_EmergencyContactsSheetReport(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult> DownloadExcel(EmergencyContactsReportParam param, List<string> roleList, string userName)
        {
            var data = await GetData(param, roleList);
            if (!data.Any())
                return new OperationResult(false, "No Data");
            List<Table> tables = new()
                    {
                        new Table("result", data)
                    };
            List<Cell> cells = new()
                    {
                        new Cell("B1", userName),
                        new Cell("D1", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")),
                    };
            ConfigDownload config = new() { IsAutoFitColumn = true };
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                tables,
                cells,
                "Resources\\Template\\EmployeeMaintenance\\4_2_2_EmergencyContactsSheetReport\\Report.xlsx",
                config
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }

        public async Task<int> GetTotalRows(EmergencyContactsReportParam param, List<string> roleList)
        {
            var data = await GetData(param, roleList);
            return data.Count;
        }

        public async Task<List<EmergencyContactsReportExport>> GetData(EmergencyContactsReportParam param, List<string> roleList)
        {
            var pred = PredicateBuilder.New<HRMS_Emp_Emergency_Contact>(true);
            var predEmpPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true);
            var predEmp_Unpaid_Leave = PredicateBuilder.New<HRMS_Emp_Unpaid_Leave>(x => x.Effective_Status);

            if (!string.IsNullOrWhiteSpace(param.Division))
            {
                pred.And(x => x.Division == param.Division);
                predEmp_Unpaid_Leave.And(x => x.Division == param.Division);
            }

            if (!string.IsNullOrWhiteSpace(param.Factory))
            {
                pred.And(x => x.Factory == param.Factory);
                predEmp_Unpaid_Leave.And(x => x.Factory == param.Factory);
            }

            if (!string.IsNullOrWhiteSpace(param.EmployeeID))
            {
                pred.And(x => x.Employee_ID.Contains(param.EmployeeID.Trim()));
                predEmp_Unpaid_Leave.And(x => x.Employee_ID.Contains(param.EmployeeID.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(param.Department))
                predEmpPersonal.And(x => x.Department == param.Department);

            if (!string.IsNullOrWhiteSpace(param.AssignedDivision))
                predEmpPersonal.And(x => x.Assigned_Division == param.AssignedDivision);

            if (!string.IsNullOrWhiteSpace(param.AssignedFactory))
                predEmpPersonal.And(x => x.Assigned_Factory == param.AssignedFactory);

            if (!string.IsNullOrWhiteSpace(param.AssignedEmployeeID))
                predEmpPersonal.And(x => x.Assigned_Employee_ID.Trim().Contains(param.AssignedEmployeeID.Trim()));

            if (!string.IsNullOrWhiteSpace(param.AssignedDepartment))
                predEmpPersonal.And(x => x.Assigned_Department == param.AssignedDepartment);

            var HEP = await Query_Permission_Data_Filter(roleList, predEmpPersonal);
            if (!string.IsNullOrWhiteSpace(param.EmploymentStatus))
            {
                var leaveUnpairs = await _repositoryAccessor.HRMS_Emp_Unpaid_Leave.FindAll(predEmp_Unpaid_Leave).Select(x => x.Employee_ID).ToListAsync();
                HEP = HEP.Where(x => x.Deletion_Code == "N"
                    ? x.Deletion_Code == param.EmploymentStatus
                    : leaveUnpairs.Contains(x.Employee_ID)
                        ? param.EmploymentStatus == "U"
                        : param.EmploymentStatus == "Y").ToList();
            }
            var HEEC = await _repositoryAccessor.HRMS_Emp_Emergency_Contact.FindAll(pred, true).ToListAsync();
            var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "22", true).ToList();
            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower(), true).ToList();

            var data = HEEC
                .Join(HEP,
                    x => x.USER_GUID,
                    y => y.USER_GUID,
                    (x, y) => new { HEEC = x, HEP = y })
                .GroupJoin(HBC,
                    x => x.HEEC.Relationship,
                    y => y.Code,
                    (x, y) => new { x.HEEC, x.HEP, HBC = y })
                .SelectMany(x => x.HBC.DefaultIfEmpty(),
                    (x, y) => new { x.HEEC, x.HEP, HBC = y })
                .GroupJoin(HBCL,
                    x => new { x.HBC?.Type_Seq, x.HBC?.Code },
                    y => new { y.Type_Seq, y.Code },
                    (x, y) => new { x.HEEC, x.HEP, x.HBC, HBCL = y })
                .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (x, y) => new { x.HEEC, x.HEP, x.HBC, HBCL = y })
                .GroupBy(x => new { x.HEEC.USER_GUID, x.HEEC.Seq })
                .Select(x => x.First())
                .Select(x => new EmergencyContactsReportExport()
                {
                    Division = x.HEEC.Division,
                    Factory = x.HEEC.Factory,
                    EmployeeID = x.HEEC.Employee_ID,
                    LocalFullName = x.HEP.Local_Full_Name,
                    Seq = x.HEEC.Seq,
                    EmergencyContact = x.HEEC.Emergency_Contact,
                    Relationship = x.HEEC.Relationship + (x.HBCL != null ? " - " + x.HBCL.Code_Name : x.HBC != null ? x.HBC.Code_Name : ""),
                    EmergencyContactPhone = x.HEEC.Emergency_Contact_Phone,
                    TemporaryAddress = x.HEEC.Temporary_Address,
                    EmergencyContactAddress = x.HEEC.Emergency_Contact_Address
                })
                .ToList();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListDivision(string lang) => await GetDataBasicCode(BasicCodeTypeConstant.Division, lang);

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

        public async Task<List<KeyValuePair<string, string>>> GetListDepartment(string division, string factory, string lang)
        {
            return await _repositoryAccessor.HRMS_Org_Department.FindAll(x => x.Division == division && x.Factory == factory, true)
                .GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == lang.ToLower(), true),
                      HOD => new { HOD.Division, HOD.Factory, HOD.Department_Code },
                      HODL => new { HODL.Division, HODL.Factory, HODL.Department_Code },
                    (HOD, HODL) => new { HOD, HODL })
                    .SelectMany(x => x.HODL.DefaultIfEmpty(),
                    (prev, HODL) => new { prev.HOD, HODL })
                .Select(x => new KeyValuePair<string, string>(
                    x.HOD.Department_Code,
                    $"{x.HOD.Department_Code} - {(x.HODL != null ? x.HODL.Name : x.HOD.Department_Name)}"))
                .ToListAsync();
        }

    }
}