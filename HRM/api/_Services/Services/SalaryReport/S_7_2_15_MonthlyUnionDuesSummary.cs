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
    public class S_7_2_15_MonthlyUnionDuesSummary : BaseServices, I_7_2_15_MonthlyUnionDuesSummary
    {
        public S_7_2_15_MonthlyUnionDuesSummary(DBContext dbContext) : base(dbContext)
        {
        }

        private async Task<OperationResult> GetData(MonthlyUnionDuesSummaryParam param)
        {
            if (string.IsNullOrWhiteSpace(param.Factory)
                || string.IsNullOrWhiteSpace(param.Year_Month)
                || !DateTime.TryParseExact(param.Year_Month, "yyyy/MM", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime yearMonth))
                return new OperationResult(false, "SalaryReport.MonthlySalaryAdditionsDeductionsSummaryReport.InvalidInput");

            var pred = PredicateBuilder.New<HRMS_Sal_Monthly>(x => x.Factory == param.Factory
                && x.Sal_Month.Date == yearMonth.Date);

            var listEmployeeID = await _repositoryAccessor.HRMS_Emp_Personal
                .FindAll(x => x.Factory == param.Factory)
                .Select(x => x.Employee_ID)
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(param.Department))
                pred.And(x => x.Department == param.Department);

            pred.And(x => listEmployeeID.Contains(x.Employee_ID));

            var wk_sql = await _repositoryAccessor.HRMS_Sal_Monthly.FindAll(pred)
                .GroupJoin(_repositoryAccessor.HRMS_Org_Department_Language.FindAll(),
                    x => new { x.Factory, x.Department },
                    y => new { y.Factory, Department = y.Department_Code },
                    (x, y) => new { HSM = x, HODL = y })
                .SelectMany(x => x.HODL.DefaultIfEmpty(),
                    (x, y) => new { x.HSM, HODL = y })
                .OrderBy(x => x.HSM.Department)
                .ThenBy(x => x.HSM.Employee_ID)
                .ToListAsync();

            var result = new List<MonthlyUnionDuesSummaryParamReport>();

            foreach (var item in wk_sql)
            {
                var dataHSAM = await _repositoryAccessor.HRMS_Sal_AddDedItem_Monthly.FindAll(x => x.Factory == item.HSM.Factory
                        && x.Sal_Month == item.HSM.Sal_Month
                        && x.Employee_ID == item.HSM.Employee_ID)
                    .ToListAsync();

                var unionDues = dataHSAM.Where(x => x.AddDed_Item == "D12").Sum(x => x.Amount);
                var insurance = dataHSAM.Where(x => x.AddDed_Item == "D11").Sum(x => x.Amount);

                result.Add(new MonthlyUnionDuesSummaryParamReport
                {
                    Factory = item.HSM.Factory,
                    Department = item.HSM.Department,
                    DepartmentName = item.HODL.Name,
                    Union_fee = unionDues,
                    Medical_Insurance_Fee = insurance,
                    TotalAmount = unionDues + insurance
                });
            }

            return new OperationResult(true, result);
        }

        public async Task<OperationResult> Download(MonthlyUnionDuesSummaryParam param)
        {
            var result = await GetData(param);
            if (!result.IsSuccess)
                return result;

            var data = (List<MonthlyUnionDuesSummaryParamReport>)result.Data;

            if (data.Count == 0)
                return new OperationResult(false, "System.Message.Nodata");
            throw new NotImplementedException();
        }

        public async Task<OperationResult> GetTotalRows(MonthlyUnionDuesSummaryParam param)
        {
            var result = await GetData(param);
            if (!result.IsSuccess)
                return result;
            var data = (List<MonthlyUnionDuesSummaryParamReport>)result.Data;
            return new OperationResult(true, data.Count);
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
    }
}