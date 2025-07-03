using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTierV2_API._Repositories;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T1.UPF;
using eTierV2_API.DTO;
using eTierV2_API.DTO.Production.T1.STF;
using eTierV2_API.Helpers.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace eTierV2_API._Services.Services.Production.T1.C2B
{
    public class ProductionT1UPFEfficiencyService : IProductionT1UPFEfficiencyService
    {
        private IConfiguration _configuration;
        private readonly IRepositoryAccessor _repoAccessor;

        public ProductionT1UPFEfficiencyService(
                 IConfiguration configuration,
                 IRepositoryAccessor repoAccessor)
        {
            _configuration = configuration;
            _repoAccessor = repoAccessor;
        }

        public async Task<List<EfficiencyDTO>> GetData(string deptId)
        {
            var factory = _configuration.GetSection("AppSettings:FactoryId").Value;
            var data_Date = await GetMaxDate();
            var firstDayOfMonth = new DateTime(data_Date.Year, data_Date.Month, 1);
            var currentDate = DateTime.Now.Date;
            var deptOfBuilding = await DeptOfBuilding(deptId);

            var data = new List<EfficiencyDTO>();
            foreach (var item in deptOfBuilding)
            {
                var data_dept = new EfficiencyDTO();
                data_dept.Current_Date = currentDate.ToString("yyyy/MM/dd");
                data_dept.Data_Date = data_Date;
                data_dept.Dept_ID = item.Dept_ID;

                var deptScoreDaily = await _repoAccessor.eTM_Dept_Score_Data.FindAll(x => x.Dept_ID == item.Dept_ID && x.Data_Date == data_Date).FirstOrDefaultAsync();

                var deptScoreMonth = await _repoAccessor.eTM_Dept_Score_Data.FindAll(x => x.Dept_ID == item.Dept_ID &&
                                                                           x.Data_Date.Year == data_Date.Year &&
                                                                           x.Data_Date.Month == data_Date.Month)
                                                                    .ToListAsync();

                var deptTargetData = await _repoAccessor.MES_Dept_Target.FindAll(x => x.Factory_ID == factory &&
                                                                           x.Dept_ID == item.Dept_ID &&
                                                                           x.Year_Target == data_Date.Year &&
                                                                           x.Month_Target == data_Date.Month)
                                                                    .FirstOrDefaultAsync();

                var deptScoreOutput = await _repoAccessor.VW_HPBasis_HPMonthlyEstimateProduceTarget_i14a.FindAll(x =>
                                                                                x.Dept_ID == item.Dept_ID &&
                                                                                x.Produce_Date == currentDate)
                                                                         .FirstOrDefaultAsync();

                var vWDeptFromMES = await _repoAccessor.VW_DeptFromMES.FindAll(x => x.Dept_ID == item.Dept_ID).ToListAsync();

                var mESDeptPlan = await _repoAccessor.MES_Dept_Plan.FindAll(x => x.Dept_ID == item.Dept_ID &&
                                       x.Plan_Date <= data_Date && x.Plan_Date >= firstDayOfMonth).ToListAsync();

                var EOLRTarget = mESDeptPlan
                   .Join(vWDeptFromMES,
                         x => x.Dept_ID.Trim(), y => y.Dept_ID.Trim(), (x, y) => new { T1 = x, T2 = y })
                          .Select(x => new
                          {
                              Plan_Day_Target = x.T1.Plan_Day_Target,
                              Working_Hour = x.T1.Working_Hour
                          })
                   .ToList();

                if (deptScoreDaily == null)
                {
                    data_dept.Target_Daily = 0;
                    data_dept.EOLR_Daily = 0;
                }
                else
                {
                    data_dept.Target_Daily = deptScoreDaily.Dept_Output_Target == 0 ? 0
                                      : Math.Round(((decimal)deptScoreDaily.Output_FGIN / (decimal)deptScoreDaily.Dept_Output_Target) * 100, 1);

                    data_dept.EOLR_Daily = deptScoreDaily.Actual_Working_Hrs == 0 ? 0
                                    : Math.Round(((decimal)deptScoreDaily.Output_FGIN / (decimal)deptScoreDaily.Actual_Working_Hrs), 1);
                }

                if (!deptScoreMonth.Any())
                {
                    data_dept.Target_Monthly = 0;
                    data_dept.EOLR_Monthly = 0;
                }
                else
                {
                    data_dept.Target_Monthly = deptScoreMonth.Sum(x => x.Dept_Output_Target) == 0 ? 0
                                        : Math.Round((((decimal)deptScoreMonth.Sum(x => x.Output_FGIN) / (decimal)deptScoreMonth.Sum(x => x.Dept_Output_Target))) * 100, 1);

                    data_dept.EOLR_Monthly = deptScoreMonth.Sum(x => x.Actual_Working_Hrs) == 0 ? 0
                                      : Math.Round(((decimal)deptScoreMonth.Sum(x => x.Output_FGIN) / (decimal)deptScoreMonth.Sum(x => x.Actual_Working_Hrs)), 1);
                }

                if (deptScoreOutput == null)
                {
                    data_dept.Perform_Daily = 0;
                    data_dept.Perform_Monthly = 0;
                    data_dept.Perform_Target = 0;
                }
                else
                {
                    data_dept.Perform_Daily = Math.Round((decimal)deptScoreOutput.Target_Yield, 1);
                    data_dept.Perform_Monthly = deptScoreOutput.Target_WorkHours == 0 ? 0 :
                        Math.Round(((decimal)deptScoreOutput.Target_Yield / (decimal)deptScoreOutput.Target_WorkHours), 1);
                    data_dept.Perform_Target = Math.Round((decimal)deptScoreOutput.Target_WorkHours, 1);
                }

                data_dept.Target_Target = deptTargetData == null ? 0 : Math.Round((decimal)deptTargetData.Output_Target, 1);
                data_dept.EOLR_Target = EOLRTarget.Sum(x => x.Working_Hour) == 0 ? 0
                                      : Math.Round(((decimal)EOLRTarget.Sum(x => x.Plan_Day_Target) / (decimal)EOLRTarget.Sum(x => x.Working_Hour)), 1);

                data_dept.Line_Sname = item.Dept_Sname;

                data.Add(data_dept);
            }

            return data;
        }

        private async Task<List<VW_DeptFromMESDTO>> DeptOfBuilding(string deptId)
        {
            var dataDate = await GetMaxDate();
            var data = await _repoAccessor.VW_DeptFromMES.FindAll(x => x.Building == deptId && x.PS_ID == "STI")
                       .Join(_repoAccessor.eTM_Dept_Score_Data.FindAll(x => x.Plan_Working_Hrs > 0 && x.Dept_Output_Target > 0 && x.Data_Date == dataDate),
                             x => x.Dept_ID.Trim(), y => y.Dept_ID.Trim(), (x, y) => new { T1 = x, T2 = y })
                       .Select(x => new VW_DeptFromMESDTO
                       {
                           Dept_ID = x.T1.Dept_ID,
                           Dept_Sname = x.T1.Line_Sname
                       })
                       .ToListAsync();
            return data;
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