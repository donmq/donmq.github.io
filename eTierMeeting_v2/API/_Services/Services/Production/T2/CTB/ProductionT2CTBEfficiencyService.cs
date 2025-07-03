using eTierV2_API._Repositories;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T2.CTB;
using eTierV2_API.DTO.Production.T2.C2B;
using eTierV2_API.Helpers.Enums;
using eTierV2_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eTierV2_API._Services.Services.Production.T2.CTB
{
    public class ProductionT2CTBEfficiencyService : IProductionT2CTBEfficiencyService
    {
        private IConfiguration _configuration;
        private readonly IRepositoryAccessor _repoAccessor;
        public ProductionT2CTBEfficiencyService(
            IRepositoryAccessor repoAccessor,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _repoAccessor = repoAccessor;
        }

        private int GetLineNameOrder(string lineSname)
        {
            string numStr = Regex.Replace(lineSname, @"[^0-9]", "");
            if (numStr.Length > 0)
                return Convert.ToInt32(numStr);
            else
                return 999;
        }

        public async Task<EfficiencyChart> GetData(string deptId, string param)
        {
            if (string.IsNullOrEmpty(deptId?.Trim()))
                return null;

            deptId = deptId.Trim();
            var dataDate = await GetMaxDate(deptId);

            if (dataDate == null)
                return null;

            eTM_Page_Item_Settings pageItemSetting;
            var data = new EfficiencyChart();
            var factoryId = _configuration.GetSection("AppSettings:FactoryId").Value.Trim();
            var currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            var firstDateOfMonth = new DateTime(dataDate.Value.Year, dataDate.Value.Month, 1);
            var VW_etmHPEfficiencyData = await _repoAccessor.VW_eTM_HP_Efficiency_Data.FindAll(m => m.Data_Date >= DateTime.Now.AddDays(-31)).ToListAsync();
            var mesDeptPlan = await _repoAccessor.MES_Dept_Plan.FindAll(m => m.Plan_Date >= DateTime.Now.AddDays(-31)).ToListAsync();
            var vwDeptFromMES = await _repoAccessor.VW_DeptFromMES.FindAll().ToListAsync();
            var mesDeptTarget = await _repoAccessor.MES_Dept_Target.FindAll().ToListAsync();
            var vwLineGroup = await _repoAccessor.VW_LineGroup.FindAll().ToListAsync();
            var pageItemSettings = await _repoAccessor.eTM_Page_Item_Settings
                .FindAll(x =>
                    x.Center_Level == Common.CENTER_LEVEL_PRODUCTION &&
                    x.Tier_Level == Common.TIER_LEVEL_T2 &&
                    x.Class_Level == Common.CLASS_LEVEL_CTB &&
                    x.Page_Name == Common.PAGE_NAME_EFFICIENCY)
                .ToListAsync();

            if (param == "Daily")
            {
                var dataQuery1 = VW_etmHPEfficiencyData
                    .Join(vwDeptFromMES,
                        x => new { Dept_ID = x.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { T1 = x, T2 = y })
                    .Join(vwLineGroup,
                        x => new { Factory_ID = x.T1.Factory_ID.Trim(), Dept_ID = x.T1.Dept_ID.Trim() },
                        y => new { Factory_ID = y.Factory_ID.Trim(), Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { x.T1, x.T2, T3 = y })
                    .Where(x =>
                        (x.T1.Data_Date == dataDate) &&
                        (x.T2.PS_ID == Common.PS_ID_ASY) &&
                        (x.T1.Factory_ID.Trim() == factoryId) &&
                        (x.T3.Line_Group.Trim() == deptId || x.T2.Building.Trim() == deptId))
                    .GroupBy(x => x.T2.Line_Sname)
                    .Where(x => (x.Sum(y => y.T1.Hour_Base) + x.Sum(y => y.T1.Hour_Overtime)) > 0)
                    .Select(x => new
                    {
                        Target_Achievement = x.Sum(y => y.T1.Target_Qty) > 0 ?
                            (decimal?)(x.Sum(y => y.T1.Actual_Qty) / (decimal)(x.Sum(y => y.T1.Target_Qty)) * 100) : 0,
                        Performance_Achievement = (x.Sum(y => y.T1.Target_Qty) + x.Sum(y => y.T1.Impact_Qty)) > 0 ?
                            (decimal?)(x.Sum(y => y.T1.Actual_Qty) / (decimal)(x.Sum(y => y.T1.Target_Qty) + x.Sum(y => y.T1.Impact_Qty)) * 100) : 0,
                        EOLR = (x.Sum(y => y.T1.Hour_Base) + x.Sum(y => y.T1.Hour_Overtime)) > 0 ?
                            (decimal?)(x.Sum(y => y.T1.Actual_Qty) / (decimal?)(x.Sum(y => y.T1.Hour_Base) + x.Sum(y => y.T1.Hour_Overtime))) : 0,
                        Line = x.Key
                    }).OrderByDescending(x => x.Line).ToList();
                var dataQueryEolr = VW_etmHPEfficiencyData
                    .Join(vwDeptFromMES,
                        x => new { Dept_ID = x.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { T1 = x, T2 = y })
                    .Join(vwLineGroup,
                        x => new { Factory_ID = x.T1.Factory_ID.Trim(), Dept_ID = x.T1.Dept_ID.Trim() },
                        y => new { Factory_ID = y.Factory_ID.Trim(), Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { x.T1, x.T2, T3 = y })
                    .Where(x =>
                        (x.T1.Data_Date == dataDate) &&
                        (x.T1.Factory_ID.Trim() == factoryId) &&
                        (x.T3.Line_Group.Trim() == deptId || x.T2.Building.Trim() == deptId))
                    .GroupBy(x => new { Line_Sname = x.T2.Line_Sname.Trim(), PS_ID = x.T2.PS_ID.Trim() })
                    .Where(x => (x.Sum(y => y.T1.Hour_Base) + x.Sum(y => y.T1.Hour_Overtime)) > 0)
                    .Select(x => new
                    {
                        Line_Sname = x.Key.Line_Sname,
                        PS_ID = x.Key.PS_ID,
                        EOLR = (x.Sum(y => y.T1.Hour_Base) + x.Sum(y => y.T1.Hour_Overtime)) > 0 ?
                            (decimal?)(x.Sum(y => y.T1.Actual_Qty) / (decimal?)(x.Sum(y => y.T1.Hour_Base) + x.Sum(y => y.T1.Hour_Overtime))) : 0,
                    }).OrderByDescending(x => x.Line_Sname).ToList();


                var dataQuery2 = VW_etmHPEfficiencyData
                    .Join(vwDeptFromMES,
                        x => new { Dept_ID = x.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { T1 = x, T2 = y })
                    .Join(vwLineGroup,
                        x => new { Factory_ID = x.T1.Factory_ID.Trim(), Dept_ID = x.T1.Dept_ID.Trim() },
                        y => new { Factory_ID = y.Factory_ID.Trim(), Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { x.T1, x.T2, T3 = y })
                    .Where(x =>
                        (x.T1.Data_Date == dataDate) &&
                        (x.T1.Factory_ID.Trim() == factoryId) &&
                        (x.T3.Line_Group.Trim() == deptId || x.T2.Building.Trim() == deptId))
                    .GroupBy(x => new { Line_Sname = x.T2.Line_Sname.Trim(), PS_ID = x.T2.PS_ID.Trim() })
                    .Where(x => (x.Sum(y => y.T1.Hour_Base) + x.Sum(y => y.T1.Hour_Overtime)) > 0)
                    .Select(x => new
                    {
                        Line_Sname = x.Key.Line_Sname,
                        PS_ID = x.Key.PS_ID,
                        IE_Achievement = (x.Sum(y => y.T1.Hour_Tot) + x.Sum(y => y.T1.Hour_In) - x.Sum(y => y.T1.Hour_Out) - x.Sum(y => y.T1.Hour_Learn) - x.Sum(y => y.T1.Hour_Transfer)) > 0 ?
                            (decimal)(x.Sum(y => y.T1.Hour_IE) / (x.Sum(y => y.T1.Hour_Tot) + x.Sum(y => y.T1.Hour_In) - x.Sum(y => y.T1.Hour_Out) - x.Sum(y => y.T1.Hour_Learn) - x.Sum(y => y.T1.Hour_Transfer))) * 100 : 0,
                    }).OrderByDescending(x => x.Line_Sname).ToList();
                var dataQuery3 = mesDeptTarget
                    .Join(vwDeptFromMES,
                        x => new { Dept_ID = x.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { T1 = x, T2 = y })
                    .Join(vwLineGroup,
                        x => new { Factory_ID = x.T1.Factory_ID.Trim(), Dept_ID = x.T1.Dept_ID.Trim() },
                        y => new { Factory_ID = y.Factory_ID.Trim(), Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { x.T1, x.T2, T3 = y })
                    .Where(x =>
                        (x.T1.Year_Target == dataDate.Value.Year) &&
                        (x.T1.Month_Target == dataDate.Value.Month) &&
                        (x.T2.PS_ID == Common.PS_ID_ASY) &&
                        (x.T1.Factory_ID.Trim() == factoryId) &&
                        (x.T3.Line_Group.Trim() == deptId || x.T2.Building.Trim() == deptId))
                    .Select(x => new
                    {
                        Line_Sname = x.T2.Line_Sname,
                        Output_Target = x.T1.Output_Target,
                        Perform_Target = x.T1.Perform_Target,
                        IE_Target = x.T1.IE_Target,
                        Building = x.T2.Building,
                        Year_Target = x.T1.Year_Target,
                        Month_Target = x.T1.Month_Target,
                    }).OrderByDescending(x => x.Line_Sname).ToList();

                #region Abnormal_Working_Hours (ASY) 

                var dataQuery_Abnormal_Working_Hours = VW_etmHPEfficiencyData
                    .Join(vwDeptFromMES,
                        x => new { Dept_ID = x.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { T1 = x, T2 = y })
                    .Join(vwLineGroup,
                        x => new { Factory_ID = x.T1.Factory_ID.Trim(), Dept_ID = x.T1.Dept_ID.Trim() },
                        y => new { Factory_ID = y.Factory_ID.Trim(), Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { x.T1, x.T2, T3 = y })
                    .Where(x =>
                        (x.T1.Data_Date == dataDate) &&
                        (x.T1.Factory_ID.Trim() == factoryId) &&
                        (x.T3.Line_Group.Trim() == deptId || x.T2.Building.Trim() == deptId))
                    .GroupBy(x => x.T2.Line_Sname)
                    .Where(x => x.Sum(y => y.T1.Hour_Tot) > 0)
                    .Select(x => new
                    {
                        Line_Sname = x.Key,
                        Abnormal_Working_Hours =(Decimal)(x.Sum(y => y.T1.Hour_Ex) / x.Sum(y => y.T1.Hour_Tot)*100)

                    }).OrderByDescending(x => x.Line_Sname).ToList();

                #endregion

                var dataQuery4 = mesDeptPlan
                    .Join(vwDeptFromMES,
                        x => new { Dept_ID = x.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { T1 = x, T2 = y })
                    .Join(vwLineGroup,
                        x => new { Factory_ID = x.T1.Factory_ID.Trim(), Dept_ID = x.T1.Dept_ID.Trim() },
                        y => new { Factory_ID = y.Factory_ID.Trim(), Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { x.T1, x.T2, T3 = y })
                    .Where(x =>
                        (x.T1.Plan_Date == dataDate) &&
                        (x.T2.PS_ID == Common.PS_ID_ASY) &&
                        (x.T1.Factory_ID.Trim() == factoryId) &&
                        (x.T3.Line_Group.Trim() == deptId || x.T2.Building.Trim() == deptId))
                    .GroupBy(x => new { Dept_ID = x.T1.Dept_ID.Trim() })
                    .Select(x => new
                    {
                        Plan_Date = x.FirstOrDefault()?.T1.Plan_Date,
                        Line_Sname = x.FirstOrDefault()?.T2.Line_Sname,
                        Plan_Day_Target = x.Sum(y => y.T1.Plan_Day_Target),
                        Working_Hour = x.Sum(y => y.T1.Working_Hour),
                    }).ToList();

                var dataQuery5 = mesDeptPlan
                    .Join(vwDeptFromMES,
                        x => new { Dept_ID = x.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { T1 = x, T2 = y })
                    .Join(vwLineGroup,
                        x => new { Factory_ID = x.T1.Factory_ID.Trim(), Dept_ID = x.T1.Dept_ID.Trim() },
                        y => new { Factory_ID = y.Factory_ID.Trim(), Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { x.T1, x.T2, T3 = y })
                    .Where(x =>
                        (x.T1.Plan_Date == dataDate) &&
                        (x.T2.PS_ID == Common.PS_ID_STI) &&
                        (x.T1.Factory_ID.Trim() == factoryId) &&
                        (x.T3.Line_Group.Trim() == deptId || x.T2.Building.Trim() == deptId))
                    .GroupBy(x => new { Dept_ID = x.T1.Dept_ID.Trim() })
                    .Select(x => new
                    {
                        Plan_Date = x.FirstOrDefault()?.T1.Plan_Date,
                        Line_Sname = x.FirstOrDefault()?.T2.Line_Sname,
                        Plan_Day_Target = x.Sum(y => y.T1.Plan_Day_Target),
                        Working_Hour = x.Sum(y => y.T1.Working_Hour),
                    }).ToList();

                if (dataQuery1.Count() > 0 && dataQuery2.Count() > 0 && dataQuery4.Count() > 0 && dataQuery_Abnormal_Working_Hours.Count > 0)
                {
                    //Target Achievement
                    var targetAchievement = new List<ChartData>();
                    pageItemSetting = pageItemSettings.Where(x => x.Item_ID == Common.ITEM_TARGET_ACHIEVEMENT).FirstOrDefault();

                    foreach (var achievement in dataQuery1)
                    {
                        var addItemAchievement = new ChartData
                        {
                            Line = achievement.Line,
                            DataLine = Math.Round(achievement.Target_Achievement ?? 0, 1),
                            Target = 0,
                            Color = "#008000",
                            Title = pageItemSetting?.Item_Name,
                            Title_LL = pageItemSetting?.Item_Name_LL,
                        };

                        var target = dataQuery3.FirstOrDefault(x => x.Line_Sname == achievement.Line);
                        if (target != null)
                        {
                            addItemAchievement.Target = Math.Round(target.Output_Target ?? 0, 1);
                            addItemAchievement.Color =
                                Math.Round(achievement.Target_Achievement ?? 0, 1) >= (target.Output_Target ?? 0) ? "#008000" :
                                Math.Round(achievement.Target_Achievement ?? 0, 1) >= ((target.Output_Target ?? 0) - 3) ? "#ffff00" :
                                Math.Round(achievement.Target_Achievement ?? 0, 1) < ((target.Output_Target ?? 0) - 3) ? "#ee0000" : "";
                        }

                        targetAchievement.Add(addItemAchievement);
                    }

                    //Performance Achievement
                    var performanceAchievement = new List<ChartData>();
                    pageItemSetting = pageItemSettings.Where(x => x.Item_ID == Common.ITEM_PERFORMANCE_ACHIEVEMENT).FirstOrDefault();

                    foreach (var performance in dataQuery1)
                    {
                        var addItemPerformance = new ChartData
                        {
                            Line = performance.Line,
                            DataLine = Math.Round(performance.Performance_Achievement ?? 0, 1),
                            Target = 0,
                            Color = "#008000",
                            Title = pageItemSetting?.Item_Name,
                            Title_LL = pageItemSetting?.Item_Name_LL,
                        };

                        var target = dataQuery3.FirstOrDefault(x => x.Line_Sname == performance.Line);
                        if (target != null)
                        {
                            addItemPerformance.Target = Math.Round(target.Perform_Target ?? 0, 1);
                            addItemPerformance.Color =
                                Math.Round(performance.Performance_Achievement ?? 0, 1) >= (target.Perform_Target ?? 0) ? "#008000" :
                                Math.Round(performance.Performance_Achievement ?? 0, 1) >= ((target.Perform_Target ?? 0) - 3) ? "#ffff00" :
                                Math.Round(performance.Performance_Achievement ?? 0, 1) < ((target.Perform_Target ?? 0) - 3) ? "#ee0000" : "";
                        }
                        performanceAchievement.Add(addItemPerformance);
                    }

                    //IE Achievement (ASY)
                    var iEAchievement = new List<ChartData>();
                    pageItemSetting = pageItemSettings.Where(x => x.Item_ID == Common.ITEM_IE_ACHIEVEMENT).FirstOrDefault();

                    foreach (var ieAchievement in dataQuery2.Where(m => m.PS_ID == "ASY"))
                    {
                        var addItemIEAchievement = new ChartData
                        {
                            Line = ieAchievement.Line_Sname,
                            DataLine = Math.Round(ieAchievement.IE_Achievement, 1),
                            Target = 0,
                            Color = "#008000",
                            Title = pageItemSetting?.Item_Name,
                            Title_LL = pageItemSetting?.Item_Name_LL,
                        };

                        var target = dataQuery3.FirstOrDefault(x => x.Line_Sname == ieAchievement.Line_Sname);
                        if (target != null)
                        {
                            addItemIEAchievement.Target = Math.Round(target.IE_Target ?? 0, 1);
                            addItemIEAchievement.Color =
                                ieAchievement.IE_Achievement >= (target.IE_Target ?? 0) ? "#008000" :
                                ieAchievement.IE_Achievement >= ((target.IE_Target ?? 0) - 3) ? "#ffff00" :
                                ieAchievement.IE_Achievement < ((target.IE_Target ?? 0) - 3) ? "#ee0000" : "";
                        }

                        iEAchievement.Add(addItemIEAchievement);
                    }

                    //IE Achievement (STI)
                    var iEAchievementSTI = new List<ChartData>();
                    pageItemSetting = pageItemSettings.Where(x => x.Item_ID == Common.ITEM_IE_ACHIEVEMENT_STI).FirstOrDefault();

                    foreach (var ieAchievement in dataQuery2.Where(m => m.PS_ID == Common.PS_ID_STI))
                    {
                        var addItemIEAchievement = new ChartData
                        {
                            Line = ieAchievement.Line_Sname,
                            DataLine = Math.Round(ieAchievement.IE_Achievement, 1),
                            Target = 0,
                            Color = "#008000",
                            Title = pageItemSetting?.Item_Name,
                            Title_LL = pageItemSetting?.Item_Name_LL,
                        };

                        var target = dataQuery3.FirstOrDefault(x => x.Line_Sname == ieAchievement.Line_Sname);
                        if (target != null)
                        {
                            addItemIEAchievement.Target = Math.Round(target.IE_Target ?? 0, 1);
                            addItemIEAchievement.Color =
                                ieAchievement.IE_Achievement >= (target.IE_Target ?? 0) ? "#008000" :
                                ieAchievement.IE_Achievement >= ((target.IE_Target ?? 0) - 3) ? "#ffff00" :
                                ieAchievement.IE_Achievement < ((target.IE_Target ?? 0) - 3) ? "#ee0000" : "";
                        }

                        iEAchievementSTI.Add(addItemIEAchievement);
                    }

                    //EOLR (ASY)
                    var eOLR = new List<ChartData>();
                    pageItemSetting = pageItemSettings.Where(x => x.Item_ID == Common.ITEM_EOLR).FirstOrDefault();

                    foreach (var eolr in dataQueryEolr.Where(m => m.PS_ID == "ASY"))
                    {
                        var addItemEOLR = new ChartData
                        {
                            Line = eolr.Line_Sname,
                            DataLine = Math.Round((decimal)eolr.EOLR, 1),
                            Title = pageItemSetting?.Item_Name,
                            Title_LL = pageItemSetting?.Item_Name_LL,
                            Target = 0,
                            Color = "#008000",
                        };

                        var target = dataQuery4.FirstOrDefault(m => m.Plan_Date == dataDate && m.Line_Sname == eolr.Line_Sname);
                        if (target != null)
                        {
                            var eolrTarget = target.Working_Hour > 0 ? target.Plan_Day_Target / target.Working_Hour : 0;
                            addItemEOLR.Target = Math.Round(eolrTarget ?? 0, 1);
                            addItemEOLR.Color =
                                eolr.EOLR >= eolrTarget ? "#008000" :
                                eolr.EOLR >= (eolrTarget - 3) ? "#ffff00" :
                                eolr.EOLR < (eolrTarget - 3) ? "#ee0000" : "";
                        }

                        eOLR.Add(addItemEOLR);
                    }

                    // EOLR (STI)
                    var eOLR_STI = new List<ChartData>();
                    pageItemSetting = pageItemSettings.Where(x => x.Item_ID == Common.ITEM_EOLR_STI).FirstOrDefault();

                    foreach (var eolr in dataQueryEolr.Where(m => m.PS_ID == Common.PS_ID_STI))
                    {
                        var addItemEOLR = new ChartData
                        {
                            Line = eolr.Line_Sname,
                            DataLine = Math.Round((decimal)eolr.EOLR, 1),
                            Title = pageItemSetting?.Item_Name,
                            Title_LL = pageItemSetting?.Item_Name_LL,
                            Target = 0,
                            Color = "#008000",
                        };

                        var target = dataQuery5.FirstOrDefault(m => m.Plan_Date == dataDate && m.Line_Sname == eolr.Line_Sname);
                        if (target != null)
                        {
                            var eolrTarget = target.Working_Hour > 0 ? target.Plan_Day_Target / target.Working_Hour : 0;
                            addItemEOLR.Target = Math.Round(eolrTarget ?? 0, 1);
                            addItemEOLR.Color =
                                eolr.EOLR >= eolrTarget ? "#008000" :
                                eolr.EOLR >= (eolrTarget - 3) ? "#ffff00" :
                                eolr.EOLR < (eolrTarget - 3) ? "#ee0000" : "";
                        }

                        eOLR_STI.Add(addItemEOLR);
                    }
                    // Abnormal_Working_Hours (ASY)
                    var Abnormal_Working_Hours = new List<ChartData>();
                    pageItemSetting = pageItemSettings.Where(x => x.Item_ID == Common.ITEM_ABNORMAL_WORKING_HOURS).FirstOrDefault();

                    foreach (var working_Hours in dataQuery_Abnormal_Working_Hours)
                    {
                        var addItemworking_Hours = new ChartData
                        {
                            Line = working_Hours.Line_Sname,
                            DataLine = Math.Round((decimal)working_Hours.Abnormal_Working_Hours, 1),
                            Title = pageItemSetting?.Item_Name,
                            Title_LL = pageItemSetting?.Item_Name_LL,
                            Target = 0,
                            Color = "#008000",
                        };
                        if(pageItemSetting.Target != null){
                            addItemworking_Hours.Target = (decimal)pageItemSetting.Target;
                            addItemworking_Hours.Color =
                            working_Hours.Abnormal_Working_Hours >= (addItemworking_Hours.Target + 0.5M) ? "#ee0000" :
                            addItemworking_Hours.Target  < working_Hours.Abnormal_Working_Hours &&
                            working_Hours.Abnormal_Working_Hours < (addItemworking_Hours.Target + 0.5M) ? "#ffff00" :
                            working_Hours.Abnormal_Working_Hours <= addItemworking_Hours.Target ? "#008000" : "";

                        }

                        Abnormal_Working_Hours.Add(addItemworking_Hours);
                    }

                    data.DataDate = dataDate.Value.ToString("yyyy/MM/dd");
                    data.EOLR = eOLR.OrderByDescending(m => GetLineNameOrder(m.Line)).ThenByDescending(m => m.Line).ToList();
                    data.EOLR_STI = eOLR_STI.OrderByDescending(m => GetLineNameOrder(m.Line)).ThenByDescending(m => m.Line).ToList();
                    data.PerformanceAchievement = performanceAchievement.OrderByDescending(m => GetLineNameOrder(m.Line)).ThenByDescending(m => m.Line).ToList();
                    data.TargetAchievement = targetAchievement.OrderByDescending(m => GetLineNameOrder(m.Line)).ThenByDescending(m => m.Line).ToList();
                    data.IEAchievement = iEAchievement.OrderByDescending(m => GetLineNameOrder(m.Line)).ThenByDescending(m => m.Line).ToList();
                    data.IEAchievement_STI = iEAchievementSTI.OrderByDescending(m => GetLineNameOrder(m.Line)).ThenByDescending(m => m.Line).ToList();
                    data.Abnormal_Working_Hours = Abnormal_Working_Hours.OrderByDescending(m=>GetLineNameOrder(m.Line)).ThenByDescending(m => m.Line).ToList();
                }
                else
                {
                    data.DataDate = dataDate.Value.ToString("yyyy/MM/dd");
                }
            }
            //Monthly
            else
            {
                var dataQuery1 = VW_etmHPEfficiencyData
                    .Join(vwDeptFromMES,
                        x => new { Dept_ID = x.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { T1 = x, T2 = y })
                    .Join(vwLineGroup,
                        x => new { Factory_ID = x.T1.Factory_ID.Trim(), Dept_ID = x.T1.Dept_ID.Trim() },
                        y => new { Factory_ID = y.Factory_ID.Trim(), Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { x.T1, x.T2, T3 = y })
                    .Where(x =>
                        (firstDateOfMonth <= x.T1.Data_Date && x.T1.Data_Date <= dataDate) &&
                        (x.T2.PS_ID.Trim() == Common.PS_ID_ASY) &&
                        (x.T3.Line_Group.Trim() == deptId || x.T2.Building.Trim() == deptId))
                    .GroupBy(x => new { Line_Sname = x.T2.Line_Sname.Trim() })
                    .Where(x => (x.Sum(y => y.T1.Hour_Base) + x.Sum(y => y.T1.Hour_Overtime)) > 0)
                    .Select(x => new
                    {
                        Target_Achievement = x.Sum(y => y.T1.Target_Qty) > 0 ?
                            (decimal?)(x.Sum(y => y.T1.Actual_Qty) / (decimal)(x.Sum(y => y.T1.Target_Qty)) * 100) : 0,
                        Performance_Achievement = (x.Sum(y => y.T1.Target_Qty) + x.Sum(y => y.T1.Impact_Qty)) > 0 ?
                            (decimal)(x.Sum(y => y.T1.Actual_Qty) / (decimal)(x.Sum(y => y.T1.Target_Qty) + x.Sum(y => y.T1.Impact_Qty)) * 100) : 0,
                        Line = x.Key.Line_Sname
                    }).OrderByDescending(x => x.Line).ToList();

                var dataQueryEolr = VW_etmHPEfficiencyData
                    .Join(vwDeptFromMES,
                        x => new { Dept_ID = x.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { T1 = x, T2 = y })
                    .Join(vwLineGroup,
                        x => new { Factory_ID = x.T1.Factory_ID.Trim(), Dept_ID = x.T1.Dept_ID.Trim() },
                        y => new { Factory_ID = y.Factory_ID.Trim(), Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { x.T1, x.T2, T3 = y })
                    .Where(x =>
                        (firstDateOfMonth <= x.T1.Data_Date && x.T1.Data_Date <= dataDate) &&
                        (x.T1.Factory_ID.Trim() == factoryId) &&
                        (x.T3.Line_Group.Trim() == deptId || x.T2.Building.Trim() == deptId))
                    .GroupBy(x => new { Line_Sname = x.T2.Line_Sname.Trim(), PS_ID = x.T2.PS_ID.Trim() })
                    .Where(x => (x.Sum(y => y.T1.Hour_Base) + x.Sum(y => y.T1.Hour_Overtime)) > 0)
                    .Select(x => new
                    {
                        Line_Sname = x.Key.Line_Sname,
                        PS_ID = x.Key.PS_ID,
                        EOLR = (x.Sum(y => y.T1.Hour_Base) + x.Sum(y => y.T1.Hour_Overtime)) > 0 ?
                            (decimal?)(x.Sum(y => y.T1.Actual_Qty) / (decimal?)(x.Sum(y => y.T1.Hour_Base) + x.Sum(y => y.T1.Hour_Overtime))) : 0,
                    }).OrderByDescending(x => x.Line_Sname).ToList();

                var dataQuery2 = VW_etmHPEfficiencyData
                    .Join(vwDeptFromMES,
                        x => new { Dept_ID = x.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { T1 = x, T2 = y })
                    .Join(vwLineGroup,
                        x => new { Factory_ID = x.T1.Factory_ID.Trim(), Dept_ID = x.T1.Dept_ID.Trim() },
                        y => new { Factory_ID = y.Factory_ID.Trim(), Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { x.T1, x.T2, T3 = y })
                    .Where(x =>
                        (firstDateOfMonth <= x.T1.Data_Date && x.T1.Data_Date <= dataDate) &&
                        (x.T3.Line_Group.Trim() == deptId || x.T2.Building.Trim() == deptId))
                    .GroupBy(x => new { Line_Sname = x.T2.Line_Sname.Trim(), PS_ID = x.T2.PS_ID.Trim() })
                    .Where(x => (x.Sum(y => y.T1.Hour_Base) + x.Sum(y => y.T1.Hour_Overtime)) > 0)
                    .Select(x => new
                    {
                        Line_Sname = x.Key.Line_Sname,
                        PS_ID = x.Key.PS_ID,
                        IE_Achievement = (x.Sum(y => y.T1.Hour_Tot) + x.Sum(y => y.T1.Hour_In) - x.Sum(y => y.T1.Hour_Out) - x.Sum(y => y.T1.Hour_Learn) - x.Sum(y => y.T1.Hour_Transfer)) > 0 ?
                            ((decimal)(x.Sum(y => y.T1.Hour_IE) / (x.Sum(y => y.T1.Hour_Tot) + x.Sum(y => y.T1.Hour_In) - x.Sum(y => y.T1.Hour_Out) - x.Sum(y => y.T1.Hour_Learn) - x.Sum(y => y.T1.Hour_Transfer))) * 100) : 0,
                    }).OrderByDescending(x => x.Line_Sname).ToList();

                var dataQuery3 = mesDeptTarget
                    .Join(vwDeptFromMES,
                        x => new { Dept_ID = x.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { T1 = x, T2 = y })
                    .Join(vwLineGroup,
                        x => new { Factory_ID = x.T1.Factory_ID.Trim(), Dept_ID = x.T1.Dept_ID.Trim() },
                        y => new { Factory_ID = y.Factory_ID.Trim(), Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { x.T1, x.T2, T3 = y })
                    .Where(x =>
                        (x.T1.Year_Target == dataDate.Value.Year) &&
                        (x.T1.Month_Target == dataDate.Value.Month) &&
                        (x.T3.Line_Group.Trim() == deptId || x.T2.Building.Trim() == deptId))
                    .Select(x => new
                    {
                        Line_Sname = x.T2.Line_Sname.Trim(),
                        Building = x.T2.Building.Trim(),
                        Output_Target = x.T1.Output_Target,
                        Perform_Target = x.T1.Perform_Target,
                        IE_Target = x.T1.IE_Target,
                        Year_Target = x.T1.Year_Target,
                        Month_Target = x.T1.Month_Target,
                    }).OrderByDescending(x => x.Line_Sname).ToList();

                var dataQuery4 = mesDeptPlan
                    .Join(vwDeptFromMES,
                        x => new { Dept_ID = x.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { T1 = x, T2 = y })
                    .Join(vwLineGroup,
                        x => new { Factory_ID = x.T1.Factory_ID.Trim(), Dept_ID = x.T1.Dept_ID.Trim() },
                        y => new { Factory_ID = y.Factory_ID.Trim(), Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { x.T1, x.T2, T3 = y })
                    .Where(x =>
                        (firstDateOfMonth <= x.T1.Plan_Date && x.T1.Plan_Date <= dataDate) &&
                        (x.T2.PS_ID == Common.PS_ID_ASY) &&
                        (x.T1.Factory_ID.Trim() == factoryId) &&
                        (x.T3.Line_Group.Trim() == deptId || x.T2.Building.Trim() == deptId))
                    .GroupBy(x => new { Dept_ID = x.T1.Dept_ID })
                    .Select(x => new
                    {
                        Line_Sname = x.FirstOrDefault()?.T2.Line_Sname,
                        Dept_ID = x.FirstOrDefault()?.T1.Dept_ID,
                        Plan_Day_Target = x.Sum(y => y.T1.Plan_Day_Target),
                        Working_Hour = x.Sum(y => y.T1.Working_Hour),
                    }).OrderByDescending(x => x.Line_Sname).ToList();

                var dataQuery5 = mesDeptPlan
                    .Join(vwDeptFromMES,
                        x => new { Dept_ID = x.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { T1 = x, T2 = y })
                    .Join(vwLineGroup,
                        x => new { Factory_ID = x.T1.Factory_ID.Trim(), Dept_ID = x.T1.Dept_ID.Trim() },
                        y => new { Factory_ID = y.Factory_ID.Trim(), Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { x.T1, x.T2, T3 = y })
                    .Where(x =>
                        (firstDateOfMonth <= x.T1.Plan_Date && x.T1.Plan_Date <= dataDate) &&
                        (x.T2.PS_ID == Common.PS_ID_STI) &&
                        (x.T1.Factory_ID.Trim() == factoryId) &&
                        (x.T3.Line_Group.Trim() == deptId || x.T2.Building.Trim() == deptId))
                    .GroupBy(x => new { Dept_ID = x.T1.Dept_ID })
                    .Select(x => new
                    {
                        Line_Sname = x.FirstOrDefault()?.T2.Line_Sname,
                        Dept_ID = x.FirstOrDefault()?.T1.Dept_ID,
                        Plan_Day_Target = x.Sum(y => y.T1.Plan_Day_Target),
                        Working_Hour = x.Sum(y => y.T1.Working_Hour),
                    }).OrderByDescending(x => x.Line_Sname).ToList();

                #region Abnormal_Working_Hours (ASY) 

                var dataQuery_Abnormal_Working_Hours = VW_etmHPEfficiencyData
                    .Join(vwDeptFromMES,
                        x => new { Dept_ID = x.Dept_ID.Trim() },
                        y => new { Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { T1 = x, T2 = y })
                    .Join(vwLineGroup,
                        x => new { Factory_ID = x.T1.Factory_ID.Trim(), Dept_ID = x.T1.Dept_ID.Trim() },
                        y => new { Factory_ID = y.Factory_ID.Trim(), Dept_ID = y.Dept_ID.Trim() },
                        (x, y) => new { x.T1, x.T2, T3 = y })
                    .Where(x =>
                        (firstDateOfMonth <= x.T1.Data_Date && x.T1.Data_Date <= dataDate) &&
                        (x.T1.Factory_ID.Trim() == factoryId) &&
                        (x.T3.Line_Group.Trim() == deptId || x.T2.Building.Trim() == deptId))
                    .GroupBy(x => x.T2.Line_Sname)
                    .Where(x => (x.Sum(y => y.T1.Hour_Ex) + x.Sum(y => y.T1.Hour_Tot)) > 0)
                    .Select(x => new
                    {
                        Line_Sname = x.Key,
                        Abnormal_Working_Hours =(Decimal)(x.Sum(y => y.T1.Hour_Ex) / x.Sum(y => y.T1.Hour_Tot)*100)

                    }).OrderByDescending(x => x.Line_Sname).ToList();

                #endregion
                if (dataQuery1.Count() > 0 && dataQuery2.Count() > 0 && dataQuery4.Count() > 0 && dataQuery_Abnormal_Working_Hours.Count>0)
                {
                    //Target Achievement
                    var targetAchievement = new List<ChartData>();
                    pageItemSetting = pageItemSettings.Where(x => x.Item_ID == Common.ITEM_TARGET_ACHIEVEMENT).FirstOrDefault();

                    foreach (var achievement in dataQuery1)
                    {
                        var addItemAchievement = new ChartData
                        {
                            Line = achievement.Line,
                            DataLine = Math.Round(achievement.Target_Achievement ?? 0, 1),
                            Title = pageItemSetting?.Item_Name,
                            Title_LL = pageItemSetting?.Item_Name_LL,
                            Target = 0,
                            Color = "#008000",
                        };

                        var target = dataQuery3.FirstOrDefault(x => x.Line_Sname == achievement.Line);
                        if (target != null)
                        {
                            addItemAchievement.Target = Math.Round(target.Output_Target ?? 0, 1);
                            addItemAchievement.Color =
                                achievement.Target_Achievement >= (target.Output_Target ?? 0) ? "#008000" :
                                achievement.Target_Achievement >= ((target.Output_Target ?? 0) - 3) ? "#ffff00" :
                                achievement.Target_Achievement < ((target.Output_Target ?? 0) - 3) ? "#ee0000" : "";
                        }

                        targetAchievement.Add(addItemAchievement);
                    }

                    //Performance Achievement
                    var performanceAchievement = new List<ChartData>();
                    pageItemSetting = pageItemSettings.Where(x => x.Item_ID == Common.ITEM_PERFORMANCE_ACHIEVEMENT).FirstOrDefault();

                    foreach (var performance in dataQuery1)
                    {
                        var addItemPerformance = new ChartData
                        {
                            Line = performance.Line,
                            DataLine = Math.Round(performance.Performance_Achievement, 1),
                            Title = pageItemSetting?.Item_Name,
                            Title_LL = pageItemSetting?.Item_Name_LL,
                            Target = 0,
                            Color = "#008000",
                        };

                        var target = dataQuery3.FirstOrDefault(x => x.Line_Sname == performance.Line);
                        if (target != null)
                        {
                            addItemPerformance.Target = Math.Round(target.Perform_Target ?? 0, 1);
                            addItemPerformance.Color =
                                performance.Performance_Achievement >= (target.Perform_Target ?? 0) ? "#008000" :
                                performance.Performance_Achievement >= ((target.Perform_Target ?? 0) - 3) ? "#ffff00" :
                                performance.Performance_Achievement < ((target.Perform_Target ?? 0) - 3) ? "#ee0000" : "";
                        }

                        performanceAchievement.Add(addItemPerformance);
                    }

                    //IE Achievement (ASY)
                    var iEAchievement = new List<ChartData>();
                    pageItemSetting = pageItemSettings.Where(x => x.Item_ID == Common.ITEM_IE_ACHIEVEMENT).FirstOrDefault();
                    foreach (var ieAchievement in dataQuery2.Where(m => m.PS_ID == "ASY"))
                    {
                        var addItemIEAchievement = new ChartData
                        {
                            Line = ieAchievement.Line_Sname,
                            DataLine = Math.Round(ieAchievement.IE_Achievement, 1),
                            Target = 0,
                            Color = "#008000",
                            Title = pageItemSetting?.Item_Name,
                            Title_LL = pageItemSetting?.Item_Name_LL
                        };

                        var target = dataQuery3.FirstOrDefault(x => x.Line_Sname.Trim() == ieAchievement.Line_Sname);
                        if (target != null)
                        {
                            addItemIEAchievement.Target = Math.Round(target.IE_Target ?? 0, 1);
                            addItemIEAchievement.Color =
                                ieAchievement.IE_Achievement >= (target.IE_Target ?? 0) ? "#008000" :
                                ieAchievement.IE_Achievement >= ((target.IE_Target ?? 0) - 3) ? "#ffff00" :
                                ieAchievement.IE_Achievement < ((target.IE_Target ?? 0) - 3) ? "#ee0000" : "";
                        }

                        iEAchievement.Add(addItemIEAchievement);
                    }

                    //IE Achievement (STI)
                    var iEAchievementSTI = new List<ChartData>();
                    pageItemSetting = pageItemSettings.Where(x => x.Item_ID == Common.ITEM_IE_ACHIEVEMENT_STI).FirstOrDefault();
                    foreach (var ieAchievement in dataQuery2.Where(m => m.PS_ID == Common.PS_ID_STI))
                    {
                        var addItemIEAchievement = new ChartData
                        {
                            Line = ieAchievement.Line_Sname,
                            DataLine = Math.Round(ieAchievement.IE_Achievement, 1),
                            Target = 0,
                            Color = "#008000",
                            Title = pageItemSetting?.Item_Name,
                            Title_LL = pageItemSetting?.Item_Name_LL
                        };

                        var target = dataQuery3.FirstOrDefault(x => x.Line_Sname.Trim() == ieAchievement.Line_Sname);
                        if (target != null)
                        {
                            addItemIEAchievement.Target = Math.Round(target.IE_Target ?? 0, 1);
                            addItemIEAchievement.Color =
                                ieAchievement.IE_Achievement >= (target.IE_Target ?? 0) ? "#008000" :
                                ieAchievement.IE_Achievement >= ((target.IE_Target ?? 0) - 3) ? "#ffff00" :
                                ieAchievement.IE_Achievement < ((target.IE_Target ?? 0) - 3) ? "#ee0000" : "";
                        }

                        iEAchievementSTI.Add(addItemIEAchievement);
                    }

                    //EOLR (ASY)
                    var eOLR = new List<ChartData>();
                    pageItemSetting = pageItemSettings.FirstOrDefault(x => x.Item_ID == Common.ITEM_EOLR);

                    foreach (var eolr in dataQueryEolr.Where(m => m.PS_ID == "ASY"))
                    {
                        var addItemEOLR = new ChartData
                        {
                            Line = eolr.Line_Sname,
                            DataLine = Math.Round((decimal)eolr.EOLR, 1),
                            Target = 0,
                            Color = "#008000",
                            Title = pageItemSetting?.Item_Name,
                            Title_LL = pageItemSetting?.Item_Name_LL,
                        };

                        var target = dataQuery4.FirstOrDefault(x => x.Line_Sname == eolr.Line_Sname);
                        if (target != null)
                        {
                            var eolrTarget = target.Working_Hour > 0 ? (target.Plan_Day_Target / (decimal)target.Working_Hour) : (decimal)0.0;
                            addItemEOLR.Target = Math.Round(eolrTarget, 1);
                            addItemEOLR.Color = eolr.EOLR >=
                                eolrTarget ? "#008000" :
                                eolr.EOLR >= (eolrTarget - 3) ? "#ffff00" :
                                eolr.EOLR < (eolrTarget - 3) ? "#ee0000" : "";
                        }

                        eOLR.Add(addItemEOLR);
                    }

                    //EOLR (STI)
                    var eOLR_STI = new List<ChartData>();
                    pageItemSetting = pageItemSettings.FirstOrDefault(x => x.Item_ID == Common.ITEM_EOLR_STI);

                    foreach (var eolr in dataQueryEolr.Where(m => m.PS_ID == Common.PS_ID_STI))
                    {
                        var addItemEOLR = new ChartData
                        {
                            Line = eolr.Line_Sname,
                            DataLine = Math.Round((decimal)eolr.EOLR, 1),
                            Target = 0,
                            Color = "#008000",
                            Title = pageItemSetting?.Item_Name,
                            Title_LL = pageItemSetting?.Item_Name_LL,
                        };

                        var target = dataQuery5.FirstOrDefault(x => x.Line_Sname == eolr.Line_Sname);
                        if (target != null)
                        {
                            var eolrTarget = target.Working_Hour > 0 ? (target.Plan_Day_Target / (decimal)target.Working_Hour) : (decimal)0.0;
                            addItemEOLR.Target = Math.Round(eolrTarget, 1);
                            addItemEOLR.Color = eolr.EOLR >=
                                eolrTarget ? "#008000" :
                                eolr.EOLR >= (eolrTarget - 3) ? "#ffff00" :
                                eolr.EOLR < (eolrTarget - 3) ? "#ee0000" : "";
                        }

                        eOLR_STI.Add(addItemEOLR);

                        
                    }

                    // Abnormal_Working_Hours (ASY)
                    var Abnormal_Working_Hours = new List<ChartData>();
                    pageItemSetting = pageItemSettings.Where(x => x.Item_ID == Common.ITEM_ABNORMAL_WORKING_HOURS).FirstOrDefault();

                    foreach (var working_Hours in dataQuery_Abnormal_Working_Hours)
                    {
                        var addItemworking_Hours = new ChartData
                        {
                            Line = working_Hours.Line_Sname,
                            DataLine = Math.Round((decimal)working_Hours.Abnormal_Working_Hours, 1),
                            Title = pageItemSetting?.Item_Name,
                            Title_LL = pageItemSetting?.Item_Name_LL,
                            Target = 0,
                            Color = "#008000",
                        };
                        if(pageItemSetting.Target != null){
                            addItemworking_Hours.Target = (decimal)pageItemSetting.Target;
                            addItemworking_Hours.Color =
                            working_Hours.Abnormal_Working_Hours >= (addItemworking_Hours.Target + 0.5M) ? "#ee0000" :
                            addItemworking_Hours.Target  < working_Hours.Abnormal_Working_Hours &&
                            working_Hours.Abnormal_Working_Hours < (addItemworking_Hours.Target + 0.5M) ? "#ffff00" :
                            working_Hours.Abnormal_Working_Hours <= addItemworking_Hours.Target ? "#008000" : "";
                        }
                        
                        Abnormal_Working_Hours.Add(addItemworking_Hours);
                    }

                    data.DataDate = dataDate.Value.ToString("yyyy/MM/dd");
                    data.FirstDayOfMonth = firstDateOfMonth.ToString("yyyy/MM/dd");
                    data.EOLR = eOLR.OrderByDescending(m => GetLineNameOrder(m.Line)).ThenByDescending(m => m.Line).ToList();
                    data.EOLR_STI = eOLR_STI.OrderByDescending(m => GetLineNameOrder(m.Line)).ThenByDescending(m => m.Line).ToList();
                    data.PerformanceAchievement = performanceAchievement.OrderByDescending(m => GetLineNameOrder(m.Line)).ThenByDescending(m => m.Line).ToList();
                    data.TargetAchievement = targetAchievement.OrderByDescending(m => GetLineNameOrder(m.Line)).ThenByDescending(m => m.Line).ToList();
                    data.IEAchievement = iEAchievement.OrderByDescending(m => GetLineNameOrder(m.Line)).ThenByDescending(m => m.Line).ToList();
                    data.IEAchievement_STI = iEAchievementSTI.OrderByDescending(m => GetLineNameOrder(m.Line)).ThenByDescending(m => m.Line).ToList();
                    data.Abnormal_Working_Hours = Abnormal_Working_Hours.OrderByDescending(m=>GetLineNameOrder(m.Line)).ThenByDescending(m => m.Line).ToList();

                }
                else
                {
                    data.DataDate = dataDate.Value.ToString("yyyy/MM/dd");
                    data.FirstDayOfMonth = firstDateOfMonth.ToString("yyyy/MM/dd");
                }
            }

            return await Task.FromResult(data);
        }

        private async Task<DateTime?> GetMaxDate(string tuCode)
        {
            var factoryId = _configuration.GetSection("AppSettings:FactoryId").Value.Trim();
            var VW_efficiencyData = await _repoAccessor.VW_eTM_HP_Efficiency_Data
                .FindAll(x => x.Factory_ID == factoryId && x.Hour_Tot > 0 && (x.Hour_Base + x.Hour_Overtime) > 0)
                .Join(_repoAccessor.VW_DeptFromMES.FindAll(x => x.PS_ID == Common.PS_ID_ASY),
                    x => new { Dept_ID = x.Dept_ID },
                    y => new { Dept_ID = y.Dept_ID },
                    (x, y) => new
                    {
                        Dept_ID = x.Dept_ID,
                        Building = y.Building,
                        Data_Date = x.Data_Date,
                        WorkHour = x.Hour_Base + x.Hour_Overtime,
                        Hour_Tot = x.Hour_Tot
                    })
                .Join(
                    _repoAccessor.VW_LineGroup.FindAll(x => x.Line_Group.Trim() == tuCode || x.Building.Trim() == tuCode),
                    x => x.Dept_ID,
                    y => y.Dept_ID,
                    (x, y) => new { Building = x.Building, LineGroup = y.Line_Group, Date = x.Data_Date, WorkHour = x.WorkHour, Hour_Tot = x.Hour_Tot }
                ).ToListAsync();

            DateTime? result = VW_efficiencyData.Any() ? VW_efficiencyData.Max(m => m.Date) : null;
            return result;
        }
    }
}