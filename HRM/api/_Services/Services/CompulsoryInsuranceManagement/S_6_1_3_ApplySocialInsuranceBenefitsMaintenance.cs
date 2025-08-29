
using System.Globalization;
using AgileObjects.AgileMapper;
using API.Data;
using API._Services.Interfaces.CompulsoryInsuranceManagement;
using API.DTOs.CompulsoryInsuranceManagement;
using API.Helper.Constant;
using API.Models;
using Aspose.Cells;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.CompulsoryInsuranceManagement
{
    public class S_6_1_3_ApplySocialInsuranceBenefitsMaintenance : BaseServices, I_6_1_3_ApplySocialInsuranceBenefitsMaintenance
    {
        public S_6_1_3_ApplySocialInsuranceBenefitsMaintenance(DBContext dbContext) : base(dbContext)
        {
        }
        #region GetData
        public async Task<PaginationUtility<ApplySocialInsuranceBenefitsMaintenanceDto>> GetDataPagination(PaginationParam pagination, ApplySocialInsuranceBenefitsMaintenanceParam param)
        {
            var result = await GetData(param);
            return PaginationUtility<ApplySocialInsuranceBenefitsMaintenanceDto>.Create(result, pagination.PageNumber, pagination.PageSize);
        }
        public async Task<List<ApplySocialInsuranceBenefitsMaintenanceDto>> GetData(ApplySocialInsuranceBenefitsMaintenanceParam param)
        {
            if (string.IsNullOrWhiteSpace(param.Factory)
            || string.IsNullOrWhiteSpace(param.Start_Year_Month_Str)
            || !DateTime.TryParseExact(param.Start_Year_Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime dateStartValue)
            || string.IsNullOrWhiteSpace(param.End_Year_Month_Str)
            || !DateTime.TryParseExact(param.End_Year_Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime dateEndValue))
                return null;
            var pred_HIBM = PredicateBuilder.New<HRMS_Ins_Benefits_Maintain>(x =>
                x.Factory == param.Factory &&
                x.Declaration_Month.Date >= dateStartValue.Date &&
                x.Declaration_Month.Date <= dateEndValue.Date);
            var pred_HIEM = PredicateBuilder.New<HRMS_Ins_Emp_Maintain>(x => x.Factory == param.Factory && x.Insurance_Type == "V01");

            if (param.Declaration_Seq != 0)
                pred_HIBM.And(x => x.Declaration_Seq == param.Declaration_Seq);

            if (!string.IsNullOrWhiteSpace(param.Benefits_Kind))
                pred_HIBM.And(x => x.Benefits_Kind == param.Benefits_Kind);

            if (!string.IsNullOrWhiteSpace(param.Employee_ID))
            {
                pred_HIBM.And(x => x.Employee_ID.Contains(param.Employee_ID.Trim()));
                pred_HIEM.And(x => x.Employee_ID.Contains(param.Employee_ID.Trim()));
            }

            var HEP = _repositoryAccessor.HRMS_Emp_Personal.FindAll(true).ToList();
            var HIEM = _repositoryAccessor.HRMS_Ins_Emp_Maintain.FindAll(pred_HIEM, true).ToList();
            var Benefits_list = await GetListBenefitsKind(param.Language);

            Dictionary<string, int> DaysList = new();
            var HIBM = await _repositoryAccessor.HRMS_Ins_Benefits_Maintain.FindAll(pred_HIBM, true).ToListAsync();

            List<ApplySocialInsuranceBenefitsMaintenanceDto> result = HIBM.OrderBy(x => x.Declaration_Month).ThenBy(x => x.Employee_ID).ThenBy(x => x.Benefits_Kind).ThenBy(x => x.Benefits_Start).Select(x =>
            {
                string key = $"{x.Employee_ID}{x.Declaration_Month.Year}{x.Benefits_Kind}";
                if (DaysList.ContainsKey(key))
                {
                    DaysList[key] += x.Total_Days;
                }
                else
                    DaysList[key] = x.Total_Days;
                var personal = HEP.FirstOrDefault(p => p.USER_GUID == x.USER_GUID);
                HRMS_Ins_Emp_Maintain Emp = HIEM.OrderByDescending(x => x.Insurance_Start).FirstOrDefault(m => m.Employee_ID == x.Employee_ID);
                return new ApplySocialInsuranceBenefitsMaintenanceDto
                {
                    USER_GUID = x.USER_GUID,
                    Factory = x.Factory,
                    Declaration_Month = x.Declaration_Month,
                    Declaration_Month_Str = x.Declaration_Month.ToString("yyyy/MM/dd"),
                    Declaration_Seq = x.Declaration_Seq, // Declaration Frequency
                    Benefits_Kind = x.Benefits_Kind, // Kind of Benefits
                    Benefits_Name = Benefits_list.FirstOrDefault(be => be.Key == x.Benefits_Kind).Value,
                    Employee_ID = x.Employee_ID,
                    Local_Full_Name = personal?.Local_Full_Name,
                    Compulsory_Insurance_Number = Emp?.Insurance_Num,
                    Special_Work_Type = x.Special_Work_Type,
                    Birthday_Child = x.Birth_Child,
                    Birthday_Child_Str = x.Birth_Child.HasValue ? x.Birth_Child.Value.ToString("yyyy/MM/dd") : "",
                    Benefits_Start = x.Benefits_Start,
                    Benefits_Start_Str = x.Benefits_Start.ToString("yyyy/MM/dd"),
                    Benefits_End = x.Benefits_End,
                    Benefits_End_Str = x.Benefits_End.ToString("yyyy/MM/dd"),
                    Benefits_Num = x.Benefits_Num, // Code of Benefits
                    Total_Days = x.Total_Days, //Day(s)
                    Annual_Accumulated_Days = DaysList[key],
                    Amt = x.Amt, // Total Amount
                    Update_By = x.Update_By,
                    Update_Time = x.Update_Time,
                    Update_Time_Str = x.Update_Time.ToString("yyyy/MM/dd HH:mm:ss"),
                };
            }).ToList();

            return result;
        }

        public async Task<OperationResult> GetAdditionData(ApplySocialInsuranceBenefitsMaintenanceDto data)
        {
            try
            {
                var HAWTD = await _repositoryAccessor.HRMS_Att_Work_Type_Days
                    .FirstOrDefaultAsync(x => x.Factory == data.Factory && x.Work_Type == data.Work_Type);
                var HIEM = await _repositoryAccessor.HRMS_Ins_Emp_Maintain
                    .FindAll(x => x.Factory == data.Factory && x.Employee_ID == data.Employee_ID && x.Insurance_Type == "V01")
                    .OrderByDescending(x => x.Insurance_Start)
                    .FirstOrDefaultAsync();
                var result = new ApplySocialInsuranceBenefitsMaintenanceDto
                {
                    Special_Work_Type = HAWTD is not null ? "Y" : "N",
                    Compulsory_Insurance_Number = HIEM?.Insurance_Num
                };
                return new OperationResult(true, result);
            }
            catch (Exception)
            {
                return new OperationResult(false);
            }
        }
        public async Task<OperationResult> CheckDate(ApplySocialInsuranceBenefitsMaintenanceDto data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.Factory)
                || string.IsNullOrWhiteSpace(data.Benefits_Kind)
                || string.IsNullOrWhiteSpace(data.Declaration_Month_Str)
                || !DateTime.TryParseExact(data.Declaration_Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime Declaration_Month)
                || string.IsNullOrWhiteSpace(data.Benefits_Start_Str)
                || !DateTime.TryParseExact(data.Benefits_Start_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime inputStart)
                || string.IsNullOrWhiteSpace(data.Benefits_End_Str)
                || !DateTime.TryParseExact(data.Benefits_End_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime inputEnd))
                    return new OperationResult(false, "Invalid inputs");
                var HIBM = await _repositoryAccessor.HRMS_Ins_Benefits_Maintain.FindAll(x =>
                        x.Factory == data.Factory &&
                        x.Employee_ID == data.Employee_ID &&
                        x.Declaration_Month.Date == Declaration_Month.Date &&
                        x.Benefits_Kind == data.Benefits_Kind)
                    .ToListAsync();
                if (data.is_Edit)
                {
                    if (!HIBM.Any(x => x.Benefits_Start.Date == inputStart.Date))
                        return new OperationResult(false, "Can not find recent data.");
                    HIBM = HIBM.FindAll(x => x.Benefits_Start.Date != inputStart.Date);
                }
                if (HIBM.Any(x => x.Benefits_Start.Date >= inputStart.Date && x.Benefits_Start.Date <= inputEnd.Date))
                    return new OperationResult(false, "Start Date cannot be duplicate.");
                if (HIBM.Any(x => x.Benefits_End.Date >= inputStart.Date && x.Benefits_End.Date <= inputEnd.Date))
                    return new OperationResult(false, "End Date cannot be duplicate.");
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false);
            }
        }

        #endregion
        #region GetList
        public async Task<List<KeyValuePair<string, string>>> GetListFactory(List<string> roleList, string language)
        {
            var factoriesByAccount = await Queryt_Factory_AddList(roleList);
            var factories = await GetDataBasicCode(BasicCodeTypeConstant.Factory, language);

            return factories.IntersectBy(factoriesByAccount, x => x.Key).ToList();
        }

        public async Task<List<KeyValuePair<string, string>>> GetListBenefitsKind(string language)
        {
            var result = await GetDataBasicCode(BasicCodeTypeConstant.BenefitsType, language);

            return result;
        }
        public async Task<List<string>> GetListTypeHeadEmployeeID(string factory)
        => await _repositoryAccessor.HRMS_Emp_Personal.FindAll(x => x.Factory == factory, true).Select(x => x.Employee_ID).Distinct().ToListAsync();

        #endregion
        #region Create
        public async Task<OperationResult> Create(ApplySocialInsuranceBenefitsMaintenanceDto data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.Factory)
                || string.IsNullOrWhiteSpace(data.Employee_ID)
                || string.IsNullOrWhiteSpace(data.Benefits_Kind)
                || string.IsNullOrWhiteSpace(data.Declaration_Month_Str)
                || !DateTime.TryParseExact(data.Declaration_Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime Declaration_Month)
                || string.IsNullOrWhiteSpace(data.Benefits_Start_Str)
                || !DateTime.TryParseExact(data.Benefits_Start_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime inputStart)
                || string.IsNullOrWhiteSpace(data.Benefits_End_Str)
                || !DateTime.TryParseExact(data.Benefits_End_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime inputEnd))
                    return new OperationResult(false, "CompulsoryInsuranceManagement.ApplySocialInsuranceBenefitsMaintenance.InvalidInputs");
                if (await _repositoryAccessor.HRMS_Ins_Benefits_Maintain.AnyAsync(x =>
                    x.Factory == data.Factory &&
                    x.Declaration_Month.Date == Declaration_Month.Date &&
                    x.Benefits_Kind == data.Benefits_Kind &&
                    x.Employee_ID == data.Employee_ID &&
                    x.Benefits_Start.Date == inputStart.Date))
                    return new OperationResult(false, "CompulsoryInsuranceManagement.ApplySocialInsuranceBenefitsMaintenance.Duplicates");
                var addData = new HRMS_Ins_Benefits_Maintain
                {
                    USER_GUID = data.USER_GUID,
                    Factory = data.Factory,
                    Employee_ID = data.Employee_ID,
                    Declaration_Month = Declaration_Month,
                    Declaration_Seq = data.Declaration_Seq,
                    Benefits_Kind = data.Benefits_Kind,
                    Special_Work_Type = data.Special_Work_Type,
                    Birth_Child = !string.IsNullOrWhiteSpace(data.Birthday_Child_Str) ? Convert.ToDateTime(data.Birthday_Child_Str) : null,
                    Benefits_Start = inputStart,
                    Benefits_End = inputEnd,
                    Benefits_Num = data.Benefits_Num,
                    Total_Days = data.Total_Days,
                    Amt = data.Amt,
                    Update_By = data.Update_By,
                    Update_Time = Convert.ToDateTime(data.Update_Time_Str),
                };
                _repositoryAccessor.HRMS_Ins_Benefits_Maintain.Add(addData);
                await _repositoryAccessor.Save();
                return new OperationResult(true, "System.Message.CreateOKMsg");
            }
            catch (Exception)
            {
                return new OperationResult(false, "System.Message.CreateErrorMsg");
            }
        }
        #endregion
        #region Update
        public async Task<OperationResult> Update(ApplySocialInsuranceBenefitsMaintenanceDto data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.Factory)
                || string.IsNullOrWhiteSpace(data.Employee_ID)
                || string.IsNullOrWhiteSpace(data.Benefits_Kind)
                || string.IsNullOrWhiteSpace(data.Declaration_Month_Str)
                || !DateTime.TryParseExact(data.Declaration_Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime Declaration_Month)
                || string.IsNullOrWhiteSpace(data.Benefits_Start_Str)
                || !DateTime.TryParseExact(data.Benefits_Start_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime inputStart)
                || string.IsNullOrWhiteSpace(data.Benefits_End_Str)
                || !DateTime.TryParseExact(data.Benefits_End_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime inputEnd))
                    return new OperationResult(false, "CompulsoryInsuranceManagement.ApplySocialInsuranceBenefitsMaintenance.InvalidInputs");
                var itemCheck = await _repositoryAccessor.HRMS_Ins_Benefits_Maintain.FirstOrDefaultAsync(x =>
                    x.Factory == data.Factory &&
                    x.Declaration_Month.Date == Declaration_Month.Date &&
                    x.Benefits_Kind == data.Benefits_Kind &&
                    x.Employee_ID == data.Employee_ID &&
                    x.Benefits_Start.Date == inputStart.Date);
                if (itemCheck is null)
                    return new OperationResult(false, "CompulsoryInsuranceManagement.ApplySocialInsuranceBenefitsMaintenance.NotExistedData");
                itemCheck.Birth_Child = !string.IsNullOrWhiteSpace(data.Birthday_Child_Str) ? Convert.ToDateTime(data.Birthday_Child_Str) : null;
                itemCheck.Benefits_End = inputEnd;
                itemCheck.Benefits_Num = data.Benefits_Num;
                itemCheck.Amt = data.Amt;
                itemCheck.Update_By = data.Update_By;
                itemCheck.Update_Time = Convert.ToDateTime(data.Update_Time_Str);
                await _repositoryAccessor.Save();
                return new OperationResult(true, "System.Message.UpdateOKMsg");
            }
            catch (Exception)
            {
                return new OperationResult(false, "System.Message.UpdateErrorMsg");
            }
        }
        #endregion
        #region Delete
        public async Task<OperationResult> Delete(ApplySocialInsuranceBenefitsMaintenanceDto data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.Factory)
                || string.IsNullOrWhiteSpace(data.Employee_ID)
                || string.IsNullOrWhiteSpace(data.Benefits_Kind)
                || string.IsNullOrWhiteSpace(data.Declaration_Month_Str)
                || !DateTime.TryParseExact(data.Declaration_Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime Declaration_Month)
                || string.IsNullOrWhiteSpace(data.Benefits_Start_Str)
                || !DateTime.TryParseExact(data.Benefits_Start_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime inputStart))
                    return new OperationResult(false, "CompulsoryInsuranceManagement.ApplySocialInsuranceBenefitsMaintenance.InvalidInputs");
                var itemCheck = await _repositoryAccessor.HRMS_Ins_Benefits_Maintain.FirstOrDefaultAsync(x =>
                    x.Factory == data.Factory &&
                    x.Declaration_Month.Date == Declaration_Month.Date &&
                    x.Benefits_Kind == data.Benefits_Kind &&
                    x.Employee_ID == data.Employee_ID &&
                    x.Benefits_Start.Date == inputStart.Date);
                if (itemCheck is null)
                    return new OperationResult(false, "CompulsoryInsuranceManagement.ApplySocialInsuranceBenefitsMaintenance.NotExistedData");
                _repositoryAccessor.HRMS_Ins_Benefits_Maintain.Remove(itemCheck);
                await _repositoryAccessor.Save();
                return new OperationResult(true, "System.Message.DeleteOKMsg");
            }
            catch (Exception)
            {
                return new OperationResult(false, "System.Message.DeleteErrorMsg");
            }
        }
        #endregion

        #region Calculate
        public async Task<OperationResult> Formula(ApplySocialInsuranceBenefitsMaintenanceDto data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.Factory)
                || string.IsNullOrWhiteSpace(data.Employee_ID)
                || string.IsNullOrWhiteSpace(data.Benefits_Kind)
                || string.IsNullOrWhiteSpace(data.Declaration_Month_Str)
                || !DateTime.TryParseExact(data.Declaration_Month_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime declaration_Month)
                || string.IsNullOrWhiteSpace(data.Benefits_Start_Str)
                || !DateTime.TryParseExact(data.Benefits_Start_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime inputStart)
                || string.IsNullOrWhiteSpace(data.Benefits_End_Str)
                || !DateTime.TryParseExact(data.Benefits_End_Str, "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime inputEnd))
                    return new OperationResult(false, "CompulsoryInsuranceManagement.ApplySocialInsuranceBenefitsMaintenance.InvalidInputs");
                DateTime Start_Month = ToFirstDateOfMonth(inputStart.AddMonths(-6));
                DateTime End_Month = ToFirstDateOfMonth(inputStart.AddMonths(-1));
                // DaysFormula
                switch (data.Benefits_Kind)
                {
                    // BT Bệnh thường gặp
                    case "V01":
                        data.Total_Days = (short)await Query_Leave_Sum_Days(
                            data.Factory, inputStart, inputEnd, data.Employee_ID, "B0");
                        break;
                    // CO Trẻ em bị bệnh
                    case "V02":
                        data.Total_Days = (short)await Query_Leave_Sum_Days(
                            data.Factory, inputStart, inputEnd, data.Employee_ID, "A0");
                        break;
                    // BN Bệnh kéo dài
                    case "V03":
                        List<string> Leave_Code_List = new() {
                            "A0", "C0", "D0", "E0", "F0",
                            "G0", "G1", "G2", "G3", "H0",
                            "I0", "I1", "J0", "J1", "J2",
                            "K0", "O0" };
                        int Calendar_days = (inputEnd.Date - inputStart.Date).Days + 1;
                        int Actual_Days = _repositoryAccessor.HRMS_Att_Change_Record.Count(x =>
                            x.Factory == data.Factory &&
                            x.Att_Date.Date >= inputStart.Date &&
                            x.Att_Date.Date <= inputEnd.Date &&
                            x.Employee_ID == data.Employee_ID);
                        var HALM = _repositoryAccessor.HRMS_Att_Leave_Maintain.FindAll(x =>
                            x.Factory == data.Factory &&
                            x.Leave_Date.Date >= inputStart.Date &&
                            x.Leave_Date.Date <= inputEnd.Date &&
                            x.Employee_ID == data.Employee_ID);
                        decimal Leave_Days = HALM.Where(x => Leave_Code_List.Contains(x.Leave_code)).Sum(x => x.Days);
                        decimal Sick = HALM.Where(x => x.Leave_code == "B0" && x.Days < 1).Sum(x => x.Days);
                        data.Total_Days = (short)(Calendar_days - Actual_Days - Leave_Days - Sick);

                        DateTime Salary_Month = ToFirstDateOfMonth(inputStart.AddMonths(-1));
                        var Sal_MasterBackup = _repositoryAccessor.HRMS_Sal_MasterBackup.FirstOrDefault(x =>
                            x.Factory == data.Factory &&
                            x.Sal_Month.Date == Salary_Month.Date &&
                            x.Employee_ID == data.Employee_ID
                        );
                        decimal Basic_amt = await Query_WageStandard_Sum(
                            "B", data.Factory, Salary_Month, data.Employee_ID, Sal_MasterBackup?.Permission_Group, Sal_MasterBackup?.Salary_Type);
                        data.Amt = (int)(Basic_amt / 24 * data.Total_Days * 75 / 100);
                        break;
                    // KT Kiểm tra thai kỳ
                    case "V04":
                        data.Total_Days = (short)await Query_Leave_Sum_Days(
                            data.Factory, inputStart, inputEnd, data.Employee_ID, "G2");
                        break;
                    // TS Nghỉ phép sinh con TS cho nam giới
                    case "V05":
                        data.Total_Days = (short)await Query_Leave_Sum_Days(
                            data.Factory, inputStart, inputEnd, data.Employee_ID, "G3");

                        decimal Avg_Salary = await Avg_WageStandard_Sum(data.Factory, Start_Month, End_Month, data.Employee_ID);
                        data.Amt = (int)Avg_Salary / 24 * data.Total_Days;
                        break;
                    // SC Sinh con
                    case "V06":
                        data.Total_Days = (short)((inputEnd.Date - inputStart.Date).Days + 1);

                        int Maternity_Month = (int)Math.Round((decimal)data.Total_Days / 30, 1);
                        decimal Avg_Salary_V06 = await Avg_WageStandard_Sum(data.Factory, Start_Month, End_Month, data.Employee_ID);
                        data.Amt = (int)(Avg_Salary_V06 * Maternity_Month + 23400000 * 2);
                        break;
                    // ST Phá thai, sử dụng biện pháp tránh thai
                    case "V07":
                        List<string> Leave_Code_List_V07 = new() {
                            "D0", "E0", "F0", "G1", "H0",
                            "I0", "I1", "J0", "J1", "J2",
                            "J5" };
                        int Calendar_days_V07 = (inputEnd.Date - inputStart.Date).Days + 1;
                        int Actual_Days_V07 = _repositoryAccessor.HRMS_Att_Change_Record.Count(x =>
                            x.Factory == data.Factory &&
                            x.Att_Date.Date >= inputStart.Date &&
                            x.Att_Date.Date <= inputEnd.Date &&
                            x.Employee_ID == data.Employee_ID &&
                            x.Days == 0);
                        var HALM_V07 = _repositoryAccessor.HRMS_Att_Leave_Maintain.FindAll(x =>
                            x.Factory == data.Factory &&
                            x.Leave_Date.Date >= inputStart.Date &&
                            x.Leave_Date.Date <= inputEnd.Date &&
                            x.Employee_ID == data.Employee_ID);
                        decimal Leave_Days_V07 = HALM_V07.Where(x => Leave_Code_List_V07.Contains(x.Leave_code)).Sum(x => x.Days);
                        decimal Sick_V07 = HALM_V07
                            .Where(x => new List<string>() { "G0" }.Contains(x.Leave_code))
                            .Sum(x => x.Days);
                        data.Total_Days = (short)(Calendar_days_V07 - Actual_Days_V07 - Leave_Days_V07 - Sick_V07);

                        decimal Avg_Salary_V07 = await Avg_WageStandard_Sum(data.Factory, Start_Month, End_Month, data.Employee_ID);
                        data.Amt = (int)(Avg_Salary_V07 / 30 * data.Total_Days);
                        break;
                    default:
                        break;
                }
                // Annual Accumulated Days
                int C_days = _repositoryAccessor.HRMS_Ins_Benefits_Maintain.FindAll(x =>
                        x.Factory == data.Factory &&
                        x.Employee_ID == data.Employee_ID &&
                        x.Declaration_Month.Year == declaration_Month.Year &&
                        x.Benefits_Kind == data.Benefits_Kind).Sum(x => x.Total_Days);
                data.Annual_Accumulated_Days = C_days + data.Total_Days;
                return new OperationResult(true, data);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex);
            }
        }
        private async Task<decimal> Query_Leave_Sum_Days(string factory, DateTime Benefits_Start, DateTime Benefits_End, string employee_ID, string Leave_code)
        {
            var HALM = await _repositoryAccessor.HRMS_Att_Leave_Maintain.FindAll(x =>
                x.Factory == factory &&
                x.Leave_Date.Date >= Benefits_Start.Date &&
                x.Leave_Date.Date <= Benefits_End.Date &&
                x.Leave_code == Leave_code &&
                x.Employee_ID == employee_ID).SumAsync(x => x.Days);
            return HALM;
        }
        private async Task<decimal> Avg_WageStandard_Sum(string Factory, DateTime Start_Month, DateTime End_Month, string Employee_ID)
        {
            // 1. 先取得權限身分別、薪資計別，作為條件代表
            var Sal_MasterBackup = _repositoryAccessor.HRMS_Sal_MasterBackup.FirstOrDefault(x =>
                x.Factory == Factory &&
                x.Sal_Month.Date == End_Month.Date &&
                x.Employee_ID == Employee_ID
            );
            if (Sal_MasterBackup is null)
                return 0;
            // 2.取得保險用途的核薪項目清單
            var HSIS = _repositoryAccessor.HRMS_Sal_Item_Settings.FindAll(x =>
                x.Factory == Factory &&
                x.Permission_Group == Sal_MasterBackup.Permission_Group &&
                x.Salary_Type == Sal_MasterBackup.Salary_Type
            );

            var Salary_Code_List = await HSIS.Where(x =>
                x.Insurance == "Y" &&
                x.Effective_Month.Date == HSIS.Where(y => y.Effective_Month.Date <= End_Month.Date).Max(y => y.Effective_Month).Date
            ).Select(x => x.Salary_Item).ToListAsync();
            // 3.加總核薪項目金額 
            var Amount_Values = await _repositoryAccessor.HRMS_Sal_MasterBackup_Detail
                .FindAll(x =>
                    x.Sal_Month.Date >= Start_Month.Date &&
                    x.Sal_Month.Date <= End_Month.Date &&
                    x.Employee_ID == Employee_ID &&
                    Salary_Code_List.Contains(x.Salary_Item) &&
                    x.Factory == Factory)
                .SumAsync(x => x.Amount);
            //4. 金額/月數= 每月平均薪資
            int Unit_Month = ((End_Month.Year - Start_Month.Year) * 12) + End_Month.Month - Start_Month.Month + 1;
            var Avg_Salary = Amount_Values / Unit_Month;
            return Math.Abs(Avg_Salary);
        }
        #endregion

        #region DownloadExcel
        public async Task<OperationResult> DownloadExcel(ApplySocialInsuranceBenefitsMaintenanceParam param, string userName)
        {

            var data = await GetData(param);

            if (!data.Any())
                return new OperationResult(false, "No data for excel download");

            try
            {
                MemoryStream stream = new();
                var path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Resources\\Template\\CompulsoryInsuranceManagement\\6_1_3_ApplySocialInsuranceBenefitsMaintenance\\Download.xlsx"
                );
                WorkbookDesigner designer = new() { Workbook = new Workbook(path) };
                Worksheet ws = designer.Workbook.Worksheets[0];

                ws.Cells["B2"].PutValue(userName);
                ws.Cells["D2"].PutValue(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

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
        private static DateTime ToFirstDateOfMonth(DateTime dt) => new(dt.Year, dt.Month, 1);

    }
}