using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTierV2_API._Repositories;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T1.STF;
using eTierV2_API.DTO;
using eTierV2_API.DTO.Production.T1.STF;
using eTierV2_API.Helpers.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace eTierV2_API._Services.Services.Production.T1.C2B
{
    public class ProductionT1STFEfficiencyService : IProductionT1STFEfficiencyService
    {
        private IConfiguration _configuration;
        private readonly IRepositoryAccessor _repoAccessor;

        public ProductionT1STFEfficiencyService(
            IConfiguration configuration,
            IRepositoryAccessor repoAccessor
            )
        {
            _configuration = configuration;
            _repoAccessor = repoAccessor;
        }

        public async Task<List<EfficiencyDTO>> GetData(string deptId)
        {
            var factory = _configuration.GetSection("AppSettings:FactoryId").Value;

            var currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            var deptOfBuilding = await DeptOfBuilding(deptId);

            var data = new List<EfficiencyDTO>();
            foreach (var item in deptOfBuilding)
            {
                var data_dept = new EfficiencyDTO();
                data_dept.Current_Date = DateTime.Now.ToString("yyyy/MM/dd");
                data_dept.Data_Date = await GetMaxDate();
                data_dept.Dept_ID = item.Dept_ID;
                var deptScoreDaily = await _repoAccessor.eTM_Dept_Score_Data.FindAll(x => x.Dept_ID == item.Dept_ID && x.Data_Date == data_dept.Data_Date).FirstOrDefaultAsync();

                var deptScoreMonth = await _repoAccessor.eTM_Dept_Score_Data.FindAll(x => x.Dept_ID == item.Dept_ID &&
                        x.Data_Date.Year == data_dept.Data_Date.Year &&
                        x.Data_Date.Month == data_dept.Data_Date.Month)
                    .ToListAsync();

                var eTMSetting = await _repoAccessor.eTM_Settings
                    .FindAll(x => x.Key == "Factory_ID")
                    .Select(x => x.Value).FirstOrDefaultAsync();

                var basicData = await _repoAccessor.SM_Basic_Data
                    .FindAll(x => x.Basic_Class == "Factory.Yearly.Target" && x.Column_01 == eTMSetting)
                    .FirstOrDefaultAsync();
                
                var deptScoreOutput = await _repoAccessor.VW_HPBasis_HPMonthlyEstimateProduceTarget_i14a.FindAll(x =>
                        x.Dept_ID == item.Dept_ID &&
                        x.Produce_Date.ToString() == currentDate)
                    .FirstOrDefaultAsync();
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

                // Target Achievement Target
                data_dept.Target_Target = basicData != null && !string.IsNullOrEmpty(basicData.Column_03)
                    ? (decimal.TryParse(basicData.Column_03, out var column03Value) ? Math.Round(column03Value, 1) : 0) : 0;
                data_dept.Line_Sname = item.Line_Sname;

                data.Add(data_dept);
            }

            return await Task.FromResult(data);
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


        private async Task<List<MES_OrgDTO>> DeptOfBuilding(string Dept_ID)
        {
            var dataDate = await GetMaxDate();
            var mesOrgs = await _repoAccessor.MES_Org.FindAll(x => x.Building == Dept_ID && x.Status == 1).ToListAsync();
            var mesDepts = await _repoAccessor.MES_Dept.FindAll(x => x.PS_ID == Common.CLASS_LEVEL_STF).ToListAsync();
            var mesLines = await _repoAccessor.MES_Line.FindAll().ToListAsync();
            var etmDeptScoreData = await _repoAccessor.eTM_Dept_Score_Data
                .FindAll(x =>
                    ((x.Plan_Working_Hrs.Value > 0 && x.Dept_Output_Target.Value > 0) || x.Output_FGIN.Value > 0) && x.Data_Date == dataDate)
                .ToListAsync();

            var data = mesOrgs
                .Join(mesDepts, x => x.Dept_ID.Trim(), y => y.Dept_ID.Trim(), (x, y) => new { T1 = x, T2 = y })
                .Join(mesLines, x => x.T1.Line_ID.Trim(), y => y.Line_ID.Trim(), (x, y) => new { T1 = x.T1, T2 = x.T2, T3 = y })
                .Join(etmDeptScoreData, x => x.T2.Dept_ID.Trim(), y => y.Dept_ID.Trim(), (x, y) => new { T1 = x.T1, T2 = x.T2, T3 = x.T3, T4 = y })
                .Select(x => new MES_OrgDTO
                {
                    Block = x.T1.Block,
                    Building = x.T1.Building,
                    Dept_ID = x.T1.Dept_ID,
                    Factory_ID = x.T2.Factory_ID,
                    HP_Dept_ID = x.T1.HP_Dept_ID,
                    IsAGV = x.T1.IsAGV,
                    IsT1T3 = x.T1.IsT1T3,
                    Line_ID = x.T1.Line_ID,
                    Line_ID_2 = x.T1.Line_ID_2,
                    Line_Seq = x.T1.Line_Seq,
                    PDC_ID = x.T1.PDC_ID,
                    Status = x.T1.Status,
                    Update_Time = x.T1.Update_Time,
                    Updated_By = x.T1.Updated_By,
                    Line_Sname = x.T3.Line_Sname
                })
                .ToList();
            return data;
        }
    }
}