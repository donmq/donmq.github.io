using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTierV2_API._Repositories;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace eTierV2_API._Services.Services.Production.T1.C2B
{
    public class EfficiencyService : IEfficiencyService
    {
        private IConfiguration _configuration;
        private readonly IRepositoryAccessor _repoAccessor;
        public EfficiencyService(
            IConfiguration configuration,
            IRepositoryAccessor repoAccessor)
        {
            _configuration = configuration;
            _repoAccessor = repoAccessor;
        }

        public async Task<EfficiencyDTO> GetData(string deptId)
        {
            var factory = _configuration.GetSection("AppSettings:FactoryId").Value;
            var Data_Date = await GetMaxDate();
            var FirstDateOfMonth = Data_Date.AddDays(-Data_Date.Day + 1);
            var currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            // if(!listDate.Any())

            EfficiencyDTO data = new EfficiencyDTO();
            data.Current_Date = DateTime.Now.ToString("yyyy/MM/dd");
            data.Data_Date = await GetMaxDate();

            var deptScoreDaily = _repoAccessor.eTM_Dept_Score_Data.FindAll(x => x.Dept_ID == deptId && x.Data_Date == data.Data_Date).FirstOrDefault();

            var deptScoreMonth = _repoAccessor.eTM_Dept_Score_Data.FindAll(x => x.Dept_ID == deptId &&
                x.Data_Date.Year == data.Data_Date.Year &&
                x.Data_Date.Month == data.Data_Date.Month);

            var eTMSetting = await _repoAccessor.eTM_Settings
                    .FindAll(x => x.Key == "Factory_ID")
                    .Select(x => x.Value).FirstOrDefaultAsync();

            var basicData = await _repoAccessor.SM_Basic_Data
                .FindAll(x => x.Basic_Class == "Factory.Yearly.Target" && x.Column_01 == eTMSetting)
                .FirstOrDefaultAsync();

            var cstWorkCenterPlans = _repoAccessor.CST_WorkCenter_Plan
                .FindAll(x => x.Work_Date >= FirstDateOfMonth && x.Work_Date <= Data_Date && x.Dept_ID == deptId).ToList();
            var vwDeptFromMES = _repoAccessor.VW_DeptFromMES.FindAll(x => x.Dept_ID == deptId).ToList();

            var deptPlanData = cstWorkCenterPlans
                .Join(vwDeptFromMES,
                    x => x.Dept_ID,
                    y => y.Dept_ID,
                    (x, y) => new { CSTWP = x, DeptFromMES = y })
                .GroupBy(m => new { m.CSTWP.Dept_ID })
                .Select(x => new
                {
                    Working_Hour = x.Sum(n => n.CSTWP.Working_Hour),
                    Plan_Day_Target = x.Sum(n => n.CSTWP.Plan_Day_Target)
                })
                .FirstOrDefault();

            var deptScoreOutput = _repoAccessor.VW_HPBasis_HPMonthlyEstimateProduceTarget_i14a.FindAll(x => x.Dept_ID == deptId && x.Produce_Date.ToString() == currentDate).FirstOrDefault();
            if (deptScoreDaily == null)
            {
                data.Target_Daily = 0;
                data.EOLR_Daily = 0;
            }
            else
            {
                data.Target_Daily = deptScoreDaily.Dept_Output_Target == 0 ? 0
                    : Math.Round(((decimal)deptScoreDaily.Output_FGIN / (decimal)deptScoreDaily.Dept_Output_Target) * 100, 1);

                data.EOLR_Daily = deptScoreDaily.Actual_Working_Hrs == 0 ? 0
                    : Math.Round(((decimal)deptScoreDaily.Output_FGIN / (decimal)deptScoreDaily.Actual_Working_Hrs), 1);
            }

            if (!deptScoreMonth.Any())
            {
                data.Target_Monthly = 0;
                data.EOLR_Monthly = 0;
            }
            else
            {
                data.Target_Monthly = deptScoreMonth.Sum(x => x.Dept_Output_Target) == 0 ? 0
                    : Math.Round((((decimal)deptScoreMonth.Sum(x => x.Output_FGIN) / (decimal)deptScoreMonth.Sum(x => x.Dept_Output_Target))) * 100, 1);

                data.EOLR_Monthly = deptScoreMonth.Sum(x => x.Actual_Working_Hrs) == 0 ? 0
                    : Math.Round(((decimal)deptScoreMonth.Sum(x => x.Output_FGIN) / (decimal)deptScoreMonth.Sum(x => x.Actual_Working_Hrs)), 1);
            }

            if (deptScoreOutput == null)
            {
                data.Perform_Daily = 0;
                data.Perform_Monthly = 0;
                data.Perform_Target = 0;
            }
            else
            {
                data.Perform_Daily = Math.Round((decimal)deptScoreOutput.Target_Yield, 1);
                data.Perform_Monthly = deptScoreOutput.Target_WorkHours == 0 ? 0 :
                    Math.Round(((decimal)deptScoreOutput.Target_Yield / (decimal)deptScoreOutput.Target_WorkHours), 1);
                data.Perform_Target = Math.Round((decimal)deptScoreOutput.Target_WorkHours, 1);
            }
            // Target Achievement Target
            data.Target_Target = basicData != null && !string.IsNullOrEmpty(basicData.Column_03)
                ? (decimal.TryParse(basicData.Column_03, out var column03Value) ? Math.Round(column03Value, 1) : 0) : 0;

            // EOLR Target
            // Requirtment from DHO - SHC-R220500028
            // Old Logic
            // - data.EOLR_Target = deptTargetData == null ? 0 : Math.Round((decimal)deptTargetData.EOLR_Target, 1);
            // New Logic
            if (deptPlanData?.Working_Hour > 0)
                data.EOLR_Target = Math.Round((decimal)(deptPlanData.Plan_Day_Target / deptPlanData.Working_Hour), 1);
            else
                data.EOLR_Target = 0;

            return await Task.FromResult(data);
        }

        public async Task<List<eTM_MES_PT1_SummaryDTO>> GetDataChart(string deptId)
        {
            var Data_Date = await GetMaxDate();

            var ListMESPT1Summary = await _repoAccessor.eTM_MES_PT1_Summary.FindAll(x => x.Dept_ID.Trim() == deptId.Trim() && x.Data_Date == Data_Date).Distinct().AsNoTracking()
                .GroupBy(x => new { x.Dept_ID, x.Data_Date, x.In_Ex, x.Reason_Code })
                .Select(x => new eTM_MES_PT1_SummaryDTO
                {
                    Dept_ID = x.Key.Dept_ID.Trim(),
                    Data_Date = x.Key.Data_Date,
                    In_Ex = x.Key.In_Ex.Trim(),
                    Reason_Code = x.Key.Reason_Code.Trim(),
                    Impact_Qty = x.Sum(y => Math.Abs(y.Impact_Qty))
                }).ToListAsync();

            var Total_Impact_Qty = ListMESPT1Summary.Sum(x => x.Impact_Qty);

            foreach (var item in ListMESPT1Summary)
            {

                item.Desc_Local = _repoAccessor.VW_MES_4MReason.FirstOrDefault(x => x.Reason_Type == item.In_Ex && x.Code == item.Reason_Code).Desc_Local;
                item.Total_Impact_Qty = Total_Impact_Qty;
            }

            return ListMESPT1Summary;
        }

        private async Task<DateTime> GetMaxDate()
        {

            DateTime? maxDateDeptScore = _repoAccessor.eTM_Dept_Score_Data.FindAll().Any() ? _repoAccessor.eTM_Dept_Score_Data.FindAll().Max(x => x.Data_Date) : null;
            DateTime? maxDateQualityDefect = _repoAccessor.eTM_MES_Quality_Defect_Data.FindAll().Any() ? _repoAccessor.eTM_MES_Quality_Defect_Data.FindAll().Max(x => x.Data_Date) : null;
            DateTime? maxDateMORecord = _repoAccessor.eTM_MES_MO_Record.FindAll().Any() ? _repoAccessor.eTM_MES_MO_Record.FindAll().Max(x => x.Data_Date) : null;

            List<DateTime> listDate = new List<DateTime>();
            if (maxDateDeptScore.HasValue)
                listDate.Add(maxDateDeptScore.Value);
            if (maxDateQualityDefect.HasValue)
                listDate.Add(maxDateQualityDefect.Value);
            if (maxDateMORecord.HasValue)
                listDate.Add(maxDateMORecord.Value);

            return await Task.FromResult(listDate.Max(x => x));
        }
    }
}