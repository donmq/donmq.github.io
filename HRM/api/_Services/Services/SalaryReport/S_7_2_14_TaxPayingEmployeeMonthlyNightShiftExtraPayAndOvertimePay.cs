using System.Globalization;
using API._Services.Interfaces.SalaryReport;
using API.Data;
using API.DTOs.SalaryReport;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.SalaryReport
{
    public class S_7_2_14_TaxPayingEmployeeMonthlyNightShiftExtraPayAndOvertimePay : BaseServices, I_7_2_14_TaxPayingEmployeeMonthlyNightShiftExtraPayAndOvertimePay
    {
        public S_7_2_14_TaxPayingEmployeeMonthlyNightShiftExtraPayAndOvertimePay(DBContext dbContext) : base(dbContext)
        {
        }

        private async Task<OperationResult> GetData(NightShiftExtraAndOvertimePayParam param)
        {
            if (string.IsNullOrWhiteSpace(param.Factory)
             || !param.Permission_Group.Any()
             || string.IsNullOrWhiteSpace(param.Year_Month)
             || !DateTime.TryParseExact(param.Year_Month, "yyyy/MM", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime yearMonth))

                return new OperationResult(false, "SalaryReport.MonthlySalaryAdditionsDeductionsSummaryReport.InvalidInput");

            var pred = PredicateBuilder.New<HRMS_Sal_Monthly>(x => x.Factory == param.Factory
                && x.Sal_Month.Date == yearMonth.Date
                && x.Tax > 0
                && param.Permission_Group.Contains(x.Permission_Group));

            var listEmployeeID = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(x => x.Factory == param.Factory
                && param.Permission_Group.Contains(x.Permission_Group)).Select(x => x.Employee_ID).ToListAsync();

            if (!string.IsNullOrWhiteSpace(param.Department))
                pred.And(x => x.Department == param.Department);

            if (!string.IsNullOrWhiteSpace(param.EmployeeID))
                pred.And(x => x.Employee_ID == param.EmployeeID);

            pred.And(x => listEmployeeID.Contains(x.Employee_ID));

            var data = await _repositoryAccessor.HRMS_Sal_Monthly.FindAll(pred).ToListAsync();
            return new OperationResult(true, data);
        }

        public async Task<OperationResult> Download(NightShiftExtraAndOvertimePayParam param)
        {
            var data = await GetData(param);
            throw new NotImplementedException();
        }

        public Task<OperationResult> GetTotalRows(NightShiftExtraAndOvertimePayParam param)
        {
            throw new NotImplementedException();
        }

        public async Task<List<KeyValuePair<string, string>>> GetListDepartment(string factory, string language)
        {
            var departments = await Query_Department_List(factory);
            var departmentsWithLanguage = await _repositoryAccessor.HRMS_Org_Department
                .FindAll(x => x.Factory == factory
                        && departments.Select(y => y.Key).Contains(x.Department_Code), true)
                .GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    x => new { x.Division, x.Factory, x.Department_Code },
                    y => new { y.Division, y.Factory, y.Department_Code },
                    (HOD, HODL) => new { HOD, HODL })
                .SelectMany(x => x.HODL.DefaultIfEmpty(),
                    (x, y) => new { x.HOD, HODL = y })
                .Select(x => new KeyValuePair<string, string>(x.HOD.Department_Code, $"{x.HOD.Department_Code} - {(x.HODL != null ? x.HODL.Name : x.HOD.Department_Name)}"))
                .Distinct()
                .ToListAsync();
            return departmentsWithLanguage;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListFactory(string userName, string language)
        {
            List<string> factories = await Queryt_Factory_AddList(userName);
            var factoriesWithLanguage = await _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Factory
                        && factories.Contains(x.Code), true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (HBC, HBCL) => new { HBC, HBCL })
                .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (x, y) => new { x.HBC, HBCL = y })
                .Select(x => new KeyValuePair<string, string>(x.HBC.Code, $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"))
                .ToListAsync();
            return factoriesWithLanguage;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListPermissionGroup(string factory, string language)
        {
            var permissionGroups = await Query_Permission_List(factory);

            var permissionGroupsWithLanguage = await _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => x.Type_Seq == BasicCodeTypeConstant.PermissionGroup
                           && permissionGroups.Select(y => y.Permission_Group).Contains(x.Code), true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (HBC, HBCL) => new { HBC, HBCL })
                .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (x, y) => new { x.HBC, HBCL = y })
                .Select(x => new KeyValuePair<string, string>(x.HBC.Code, $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"))
                .ToListAsync();
            return permissionGroupsWithLanguage;
        }
    }
}