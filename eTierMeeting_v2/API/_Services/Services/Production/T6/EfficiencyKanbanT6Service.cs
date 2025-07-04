using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using eTierV2_API._Repositories;
using eTierV2_API._Services.Interfaces.Production.T6;
using eTierV2_API.Data;
using eTierV2_API.DTO.Production.T6.EfficiencyKanban;
using eTierV2_API.Helpers.Enums;
using eTierV2_API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace eTierV2_API._Services.Services.Production.T6
{
    public class EfficiencyKanbanT6Service : IEfficiencyKanbanT6Service
    {
        private readonly DateTime _maxDate;
        private readonly IServiceProvider _serviceProvider;
        private readonly HashSet<string> _qaLinePrefixList;
        private readonly HashSet<string> _quantizationPrefixList;
        private readonly IRepositoryAccessor _repoAccessor;
        public EfficiencyKanbanT6Service(IRepositoryAccessor repoAccessor,
                                        IServiceProvider serviceProvider)
        {
            _repoAccessor = repoAccessor;
            _maxDate = GetMaxDate().Result;
            _serviceProvider = serviceProvider;
            _qaLinePrefixList = new HashSet<string> { "QA", "B3", "B7", "B8", "QX" };
            _quantizationPrefixList = new HashSet<string> { "B15", "B91", "B92", "B93", "B94", "B95", "B96", "B97", "B98", "B9A" };
        }

        #region Main function get data
        public async Task<List<DataGrouped>> GetDataChart(EffiencyKanbanParam param)
        {
            param = InitData(param);

            Stopwatch stopwatch = new();
            stopwatch.Start(); // �}�l�p��

            var dataResult = new List<DataGrouped>();
            List<EfficiencyByBrandDto> data1;
            List<EfficiencyByBrandDto> data2;
            List<EfficiencyByBrandDto> data3;

            // Get the required DbContext instances from each scope.
            using var scope1 = _serviceProvider.CreateScope();
            using var scope2 = _serviceProvider.CreateScope();
            using var scope3 = _serviceProvider.CreateScope();

            // Get the required DbContext instances from each scope.
            Debug.WriteLine($"1, {stopwatch.ElapsedMilliseconds}");
            var dbContext1 = scope1.ServiceProvider.GetRequiredService<DBContext>();
            var dbContext2 = scope2.ServiceProvider.GetRequiredService<DBContext>();
            var dbContext3 = scope3.ServiceProvider.GetRequiredService<DBContext>();

            // Prepare data using async methods to utilize CPU resources fully.
            var data1Task = PrepareData1(dbContext1, param);
            var data2Task = PrepareData2(dbContext2, param);
            var data3Task = PrepareData3(dbContext3, param);

            // Wait for all async methods to complete.
            await Task.WhenAll(data1Task, data2Task, data3Task);

            data1 = data1Task.Result;
            data2 = data2Task.Result;
            data3 = data3Task.Result;

            if (param.ChartTitles is null)
                return dataResult;

            var dataEfficiency = new List<EfficiencyKanbanModel>();
            foreach (var item in param.ChartTitles)
            {
                switch (item.Item_ID)
                {
                    case Common.ITEM_TARGET_ACHIEVEMENT:
                        var dataTarget_Achievement = GetDataTargetAchievement(data1, param, item);
                        dataEfficiency.Add(dataTarget_Achievement);
                        break;
                    case Common.ITEM_EOLR:
                        var dataEOLR = GetDataEOLRAchievement(data2, param, item);
                        dataEfficiency.Add(dataEOLR);
                        break;
                    case Common.ITEM_CTB_IE_ACHIEVEMENT:
                        var dataCTB = GetDataCTBIEAchievement(data3, param, item);
                        dataEfficiency.Add(dataCTB);
                        break;
                    // default:
                    //     var dataModEff = param.Is_T5_External
                    //         ? GetDataModelEfficiencyFakeExt(dataModelEfficiencyFakeExt, param, "%", 1, item)
                    //         : GetDataModelEfficiency(dataModelEfficiency, param, item);
                    //     dataEfficiency.Add(dataModEff);
                    //     break;
                }
            }

            Debug.WriteLine($"3, {stopwatch.ElapsedMilliseconds}");

            dataResult = dataEfficiency.GroupBy(x => x.Group).Select(x => new DataGrouped
            {
                Group = x.Key,
                Data_By_Groups = x.Select(y => y).ToList()
            }).ToList();
            return dataResult;
        }
        #endregion

        //------------------------------------------------------------------------------------------------------------------------//
        // [SQL 1] SQL for get data of Item_ID = 'Target_Achievement'
        public async Task<List<EfficiencyByBrandDto>> PrepareData1(DBContext context, EffiencyKanbanParam param)
        {
            System.Diagnostics.Debug.WriteLine("PrepareData1 Start");
            IQueryable<EfficiencyByBrandDto> dataQuery1_group;
            var pred_EfficiencyByBrand = PredicateBuilder.New<VW_Efficiency_ByBrand>(x => param.Factorys.Select(y => y.Id).Contains(x.Factory_ID.TrimEnd()));
            if (param.Type == "month")
                pred_EfficiencyByBrand.And(x => param.Months.Select(y => y.Month).Contains(x.Data_Date.Month));
            else if (param.Type == "year")
                pred_EfficiencyByBrand.And(x => param.Years.Contains(x.Data_Date.Year));
            else if (param.Type == "week")
                pred_EfficiencyByBrand.And(x => param.Weeks.First().WeekStart.Date <= x.Data_Date && param.Weeks.Last().WeekFinish >= x.Data_Date);
            else
                pred_EfficiencyByBrand.And(x => param.Seasons.First().SeasonStart.Date <= x.Data_Date && param.Seasons.Last().SeasonFinish >= x.Data_Date);

            if(!string.IsNullOrEmpty(param.Brand))
                pred_EfficiencyByBrand.And(x => x.Brand == param.Brand);

            pred_EfficiencyByBrand.And(x => x.Kind == "5");

            var vwEfficiencyByBrand = _repoAccessor.VW_Efficiency_ByBrand.FindAllContext(context, pred_EfficiencyByBrand);

            if (param.Type == "week")
            {
                dataQuery1_group = from a in vwEfficiencyByBrand
                                   group a by new { a.Factory_ID, a.Data_Date } into pg
                                   select new EfficiencyByBrandDto()
                                   {
                                       Factory = pg.Key.Factory_ID.TrimEnd(),
                                       Data_Date = pg.Key.Data_Date,
                                       Actual_Qty = pg.Sum(m => m.Actual_Qty),
                                       Target_Qty = pg.Sum(m => m.Target_Qty),
                                       Month = pg.Max(x => x.Data_Date.Month),
                                       Year = pg.Max(x => x.Data_Date.Year)
                                   };
            }
            else
            {
                dataQuery1_group = from data in vwEfficiencyByBrand
                                   group data by new { data.Factory_ID, data.Data_Date.Year, data.Data_Date.Month } into pg
                                   select new EfficiencyByBrandDto()
                                   {
                                       Factory = pg.Key.Factory_ID.TrimEnd(),
                                       Data_Date = new DateTime(
                                               pg.Key.Year,
                                               pg.Key.Month,
                                               1
                                           ),
                                       Actual_Qty = pg.Sum(m => m.Actual_Qty),
                                       Target_Qty = pg.Sum(m => m.Target_Qty),
                                       Month = pg.Key.Month,
                                       Year = pg.Key.Year
                                   };
            }

            var result = await dataQuery1_group.ToListAsync();
            System.Diagnostics.Debug.WriteLine("PrepareData1 End");
            return result;
        }

        //------------------------------------------------------------------------------------------------------------------------//
        // [SQL 2]  SQL for get data of Item_ID = ‘EOLR’
        public async Task<List<EfficiencyByBrandDto>> PrepareData2(DBContext context, EffiencyKanbanParam param)
        {
            System.Diagnostics.Debug.WriteLine("PrepareData2 Start");
            IQueryable<EfficiencyByBrandDto> dataQuery_group;
            var pred_EfficiencyByBrand = PredicateBuilder.New<VW_Efficiency_ByBrand>(x => x.Kind != null && param.Factorys.Select(y => y.Id).Contains(x.Factory_ID.TrimEnd()));
            if (param.Type == "month")
                pred_EfficiencyByBrand.And(x => param.Months.Select(y => y.Month).Contains(x.Data_Date.Month));
            else if (param.Type == "year")
                pred_EfficiencyByBrand.And(x => param.Years.Contains(x.Data_Date.Year));
            else if (param.Type == "week")
                pred_EfficiencyByBrand.And(x => param.Weeks.First().WeekStart.Date <= x.Data_Date && param.Weeks.Last().WeekFinish >= x.Data_Date);
            else
                pred_EfficiencyByBrand.And(x => param.Seasons.First().SeasonStart.Date <= x.Data_Date && param.Seasons.Last().SeasonFinish >= x.Data_Date);

            if(!string.IsNullOrEmpty(param.Brand))
                pred_EfficiencyByBrand.And(x => x.Brand == param.Brand);

            var efficiencyByBrand = _repoAccessor.VW_Efficiency_ByBrand.FindAllContext(context, pred_EfficiencyByBrand);

            var deptIdOfVW = efficiencyByBrand.Select(x => x.Dept_ID.Trim()).Distinct().ToList();
            var dataHP_HP_Production_Line_ie21 = _repoAccessor.HP_Production_Line_ie21.FindAllContext(context);
            var dataQuery = from vwdata in efficiencyByBrand
                             join ie21 in dataHP_HP_Production_Line_ie21
                             on new { vwdata.Dept_ID, vwdata.Factory_ID }
                             equals new { ie21.Dept_ID, ie21.Factory_ID } into joinedData
                             from ie21 in joinedData.DefaultIfEmpty()
                             select new { vwdata, ie21 };

            if (param.Type == "week")
                dataQuery_group = from data in dataQuery
                                   group new { data.vwdata, data.ie21 } by new { data.vwdata.Factory_ID, data.vwdata.Data_Date, data.ie21.Line_No } into pg
                                   select new EfficiencyByBrandDto()
                                   {
                                       Factory = pg.Key.Factory_ID.TrimEnd(),
                                       Line_No = pg.Key.Line_No,
                                       Data_Date = pg.Key.Data_Date,
                                       Actual_Qty = pg.Sum(m => m.vwdata.Actual_Qty),
                                       Hour_Base = pg.Sum(m => m.vwdata.Hour_Base),
                                       Hour_Overtime = pg.Sum(m => m.vwdata.Hour_Overtime),
                                       Month = pg.Max(x => x.vwdata.Data_Date.Month),
                                       Year = pg.Max(x => x.vwdata.Data_Date.Year)
                                   };
            
            else
                dataQuery_group = from data in dataQuery
                                   group new { data.vwdata, data.ie21 } by new { data.vwdata.Factory_ID, data.vwdata.Data_Date.Month, data.vwdata.Data_Date.Year, data.ie21.Line_No } into pg
                                   select new EfficiencyByBrandDto()
                                   {
                                       Factory = pg.Key.Factory_ID.TrimEnd(),
                                       Line_No = pg.Key.Line_No,
                                       Data_Date = new DateTime(
                                               pg.Key.Year,
                                               pg.Key.Month,
                                               1
                                           ),
                                       Actual_Qty = pg.Sum(m => m.vwdata.Actual_Qty),
                                       Target_Qty = pg.Sum(m => m.vwdata.Target_Qty),
                                       Impact_Qty = pg.Sum(m => m.vwdata.Impact_Qty),
                                       Hour_Base = pg.Sum(m => m.vwdata.Hour_Base),
                                       Hour_Overtime = pg.Sum(m => m.vwdata.Hour_Overtime),
                                       Month = pg.Key.Month,
                                       Year = pg.Key.Year
                                   };
            

            var result = await dataQuery_group.ToListAsync();
            System.Diagnostics.Debug.WriteLine("PrepareData2 End");
            return result;
        }

        //------------------------------------------------------------------------------------------------------------------------//
        // [SQL 3]  SQL for get data of Item_ID = ‘CTB_ie_achievement’
        public async Task<List<EfficiencyByBrandDto>> PrepareData3(DBContext context, EffiencyKanbanParam param)
        {
            System.Diagnostics.Debug.WriteLine("PrepareData3 Start");
            IQueryable<EfficiencyByBrandDto> dataQueryCTB;
            var pred_EfficiencyByBrand = PredicateBuilder.New<VW_Efficiency_ByBrand>(x => param.Factorys.Select(y => y.Id).Contains(x.Factory_ID.TrimEnd()));
            if (param.Type == "month")
                pred_EfficiencyByBrand.And(x => param.Months.Select(y => y.Month).Contains(x.Data_Date.Month));
            else if (param.Type == "year")
                pred_EfficiencyByBrand.And(x => param.Years.Contains(x.Data_Date.Year));
            else if (param.Type == "week")
                pred_EfficiencyByBrand.And(x => param.Weeks.First().WeekStart.Date <= x.Data_Date && param.Weeks.Last().WeekFinish >= x.Data_Date);
            else
                pred_EfficiencyByBrand.And(x => param.Seasons.First().SeasonStart.Date <= x.Data_Date && param.Seasons.Last().SeasonFinish >= x.Data_Date);

            if (!string.IsNullOrEmpty(param.Brand))
                pred_EfficiencyByBrand.And(x => x.Brand == param.Brand);
            
            var efficiencyByBrand = _repoAccessor.VW_Efficiency_ByBrand.FindAllContext(context, pred_EfficiencyByBrand);

            if (param.Type == "week")
                dataQueryCTB =
                    from data in efficiencyByBrand
                    group data by new { Factory_ID = data.Factory_ID.TrimEnd(), data.Data_Date } into pg
                    select new EfficiencyByBrandDto()
                    {
                        Factory = pg.Key.Factory_ID.TrimEnd(),
                        Data_Date = pg.Key.Data_Date,
                        Hour_IE = pg.Sum(m => m.Hour_IE),
                        Hour_Tot = pg.Sum(m => m.Hour_Tot),
                        Hour_In = pg.Sum(m => m.Hour_In),
                        Hour_Out = pg.Sum(m => m.Hour_Out),
                        Hour_Learn = pg.Sum(m => m.Hour_Learn),
                        Hour_Transfer = pg.Sum(m => m.Hour_Transfer),
                    };
            else
                dataQueryCTB =
                    from data in efficiencyByBrand
                    group data by new { Factory_ID = data.Factory_ID.TrimEnd(), Data_Year = data.Data_Date.Year, Data_Month = data.Data_Date.Month } into pg
                    select new EfficiencyByBrandDto()
                    {
                        Factory = pg.Key.Factory_ID.TrimEnd(),
                        Data_Date = new DateTime(pg.Key.Data_Year, pg.Key.Data_Month, 1),
                        Hour_IE = pg.Sum(m => m.Hour_IE),
                        Hour_Tot = pg.Sum(m => m.Hour_Tot),
                        Month = pg.Max(m => m.Data_Date.Month),
                        Year = pg.Max(m => m.Data_Date.Year),
                        Hour_In = pg.Sum(m => m.Hour_In),
                        Hour_Out = pg.Sum(m => m.Hour_Out),
                        Hour_Learn = pg.Sum(m => m.Hour_Learn),
                        Hour_Transfer = pg.Sum(m => m.Hour_Transfer)
                    };
            
            List<EfficiencyByBrandDto> result = await dataQueryCTB.ToListAsync();

            System.Diagnostics.Debug.WriteLine("PrepareData3 End");
            return result;
        }

        private EffiencyKanbanParam InitData(EffiencyKanbanParam param)
        {
            var result = param;
            var eTMS = _repoAccessor.eTM_Settings.FirstOrDefault(x => x.Key == "Manuf")?.Value;
            var division = _repoAccessor.SM_Basic_Data.FirstOrDefault(x => x.Column_03 == eTMS)?.Column_02;
            result.Months = GetMonthly(_maxDate.Month, _maxDate.Year);
            result.Years = Enumerable.Range(division == "1" ? 2025 : 2019, _maxDate.Year - (division == "1" ? 2024 : 2018)).ToList();
            result.Weeks = GetWeeks();
            result.Seasons = GetSeasons(division);
            var dataPageItemSetting = _repoAccessor.eTM_Page_Item_Settings.FindAll(x =>
                x.Center_Level == Common.CENTER_LEVEL_PRODUCTION &&
                x.Tier_Level == Common.TIER_LEVEL_T5 &&
                x.Class_Level == Common.CLASS_LEVEL_ALL &&
                x.Page_Name == Common.PAGE_NAME_EFFICIENCY &&
                x.Is_Active);
            result.ChartTitles = dataPageItemSetting
                .Join(_repoAccessor.SM_Basic_Data.FindAll(x => x.Basic_Class == "Prod.T6.Item.AdditionalProperty1"),
                    x => x.Item_ID,
                    y => y.Column_01,
                    (x, y) => new { t1 = x, t2 = y })
                .Select(x => new DataChartTitle
                {
                    Item_ID = x.t1.Item_ID,
                    Chart_Display_Name = x.t1.Item_Name,
                    Item_Group_SortSeq = x.t2.Column_02,
                    Item_SortSeq = x.t2.Column_03
                }).OrderBy(x => x.Item_Group_SortSeq).ThenBy(x => x.Item_SortSeq).ToList();
            return result;
        }


        #region combine query data by date range of all charts
        private EfficiencyKanbanModel GetDataTargetAchievement(List<EfficiencyByBrandDto> dataQuery, EffiencyKanbanParam param, DataChartTitle chartTitle)
        {
            var targetAchievementData = new List<FactoryDataChart>();
            foreach (var factory in param.Factorys)
            {
                var values = new List<decimal>();
                var actualQties = new List<int?>();
                var dataQueryInFactory = dataQuery.Where(x => x.Factory == factory.Id).ToList();

                if (param.Type == "month")
                {
                    foreach (var monthOfYearItem in param.Months)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Month == monthOfYearItem.Month && x.Year == monthOfYearItem.Year).ToList();
                        var valuePercent = GetValueTarget(dataFind);
                        values.Add(valuePercent);
                        var actualQty = dataFind.Sum(x => x.Actual_Qty);
                        actualQties.Add(actualQty);
                    }
                }
                else if (param.Type == "year")
                {
                    foreach (var year in param.Years)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Year == year).ToList();
                        var valuePercent = GetValueTarget(dataFind);
                        values.Add(valuePercent);
                        var actualQty = dataFind.Sum(x => x.Actual_Qty);
                        actualQties.Add(actualQty);
                    }
                }
                else if (param.Type == "week")
                {
                    foreach (var week in param.Weeks)
                    {
                        var dataFind = dataQueryInFactory.Where(x => (x.Data_Date >= week.WeekStart.Date && x.Data_Date <= week.WeekFinish.Date)).ToList();
                        var valuePercent = GetValueTarget(dataFind);
                        values.Add(valuePercent);
                        var actualQty = dataFind.Sum(x => x.Actual_Qty);
                        actualQties.Add(actualQty);
                    }
                }
                else
                {
                    foreach (var season in param.Seasons)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Data_Date >= season.SeasonStart.Date && x.Data_Date <= season.SeasonFinish.Date).ToList();
                        var valuePercent = GetValueTarget(dataFind);
                        values.Add(valuePercent);
                        var actualQty = dataFind.Sum(x => x.Actual_Qty);
                        actualQties.Add(actualQty);
                    }
                }

                var factoryData = new FactoryDataChart()
                {
                    Name = factory.Name,
                    Value = values,
                    ActualQty = actualQties,
                };
                targetAchievementData.Add(factoryData);
            }

            var result = new EfficiencyKanbanModel()
            {
                Title = chartTitle.Chart_Display_Name,
                Group = chartTitle.Item_Group_SortSeq,
                ShowValues = Convert.ToInt32(param.Factorys.Count == 1),
                ChartUnit = "%",
                IsActive = true,
                Digits = 1,
                Labels = GetLabels(param),
                Data = targetAchievementData
            };
            return result;
        }
        private EfficiencyKanbanModel GetDataEOLRAchievement(List<EfficiencyByBrandDto> dataQuery, EffiencyKanbanParam param, DataChartTitle chartTitle)
        {
            var eOLRData = new List<FactoryDataChart>();
            foreach (var factory in param.Factorys)
            {
                var values = new List<decimal>();
                var dataQueryInFactory = dataQuery.Where(x => x.Factory == factory.Id).ToList();

                if (param.Type == "month")
                {
                    foreach (var monthOfYearItem in param.Months)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Month == monthOfYearItem.Month && x.Year == monthOfYearItem.Year).ToList();
                        var valuePercent = GetValueEOLR(dataFind);
                        values.Add(valuePercent);
                    }
                }
                else if (param.Type == "year")
                {
                    foreach (var year in param.Years)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Year == year).ToList();
                        var valuePercent = GetValueEOLR(dataFind);
                        values.Add(valuePercent);
                    }
                }
                else if (param.Type == "week")
                {
                    foreach (var week in param.Weeks)
                    {
                        var dataFind = dataQueryInFactory.Where(x => (x.Data_Date >= week.WeekStart.Date && x.Data_Date <= week.WeekFinish.Date)).ToList();
                        var valuePercent = GetValueEOLR(dataFind);
                        values.Add(valuePercent);
                    }
                }
                else
                {
                    foreach (var season in param.Seasons)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Data_Date >= season.SeasonStart.Date && x.Data_Date <= season.SeasonFinish.Date).ToList();
                        var valuePercent = GetValueEOLR(dataFind);
                        values.Add(valuePercent);
                    }
                }

                var factoryData = new FactoryDataChart()
                {
                    Name = factory.Name,
                    Value = values
                };
                eOLRData.Add(factoryData);
            }

            var result = new EfficiencyKanbanModel()
            {
                Title = chartTitle.Chart_Display_Name,
                Group = chartTitle.Item_Group_SortSeq,
                ShowValues = 0,
                ChartUnit = "",
                IsActive = true,
                Digits = 1,
                Labels = GetLabels(param),
                Data = eOLRData,
            };
            return result;
        }
        private EfficiencyKanbanModel GetDataCTBIEAchievement(List<EfficiencyByBrandDto> dataQuery, EffiencyKanbanParam param, DataChartTitle chartTitle)
        {
            var dataQueryCTB = dataQuery.ToList();

            var cTBData = new List<FactoryDataChart>();
            foreach (var factory in param.Factorys)
            {
                var values = new List<decimal>();
                var dataQueryInFactory = dataQueryCTB.Where(x => x.Factory == factory.Id).ToList();

                if (param.Type == "month")
                {
                    foreach (var monthOfYearItem in param.Months)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Month == monthOfYearItem.Month && x.Year == monthOfYearItem.Year).ToList();
                        var valuePercent = GetValueCTB(dataFind);
                        values.Add(valuePercent);
                    }
                }
                else if (param.Type == "year")
                {
                    foreach (var year in param.Years)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Year == year).ToList();
                        var valuePercent = GetValueCTB(dataFind);
                        values.Add(valuePercent);
                    }
                }
                else if (param.Type == "week")
                {
                    foreach (var week in param.Weeks)
                    {
                        var dataFind = dataQueryInFactory.Where(x => (x.Data_Date >= week.WeekStart.Date && x.Data_Date <= week.WeekFinish.Date)).ToList();
                        var valuePercent = GetValueCTB(dataFind);
                        values.Add(valuePercent);
                    }
                }
                else
                {
                    foreach (var season in param.Seasons)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Data_Date >= season.SeasonStart.Date && x.Data_Date <= season.SeasonFinish.Date).ToList();
                        var valuePercent = GetValueCTB(dataFind);
                        values.Add(valuePercent);
                    }
                }

                var factoryData = new FactoryDataChart()
                {
                    Name = factory.Name,
                    Value = values
                };
                cTBData.Add(factoryData);
            }

            var result = new EfficiencyKanbanModel()
            {
                Title = chartTitle.Chart_Display_Name,
                Group = chartTitle.Item_Group_SortSeq,
                ShowValues = 0,
                ChartUnit = "%",
                IsActive = true,
                Digits = 1,
                Labels = GetLabels(param),
                Data = cTBData
            };
            return result;
        }
        #endregion

        #region query to get data by all charts 
        // Target_Achievement
        private decimal GetValueTarget(List<EfficiencyByBrandDto> data)
        {
            if (data == null || data.Count == 0)
                return 0;
            var sumTarget = (decimal)data.Sum(x => x.Target_Qty);
            var valuePercent = (sumTarget != 0) ? ((decimal)(data.Sum(x => x.Actual_Qty) * 100) / sumTarget) : 0;
            return Math.Round((decimal)valuePercent, 1);
        }

        // EOLR
        private decimal GetValueEOLR(List<EfficiencyByBrandDto> data)
        {
            if (data == null || data.Count == 0)
                return 0;

            decimal sumHourBase = data.Sum(x => (x.Hour_Base ?? 0) * GetValuebyLineNo(x.Line_No?.Trim()));
            decimal sumHourOvertime = data.Sum(x => (x.Hour_Overtime ?? 0) * GetValuebyLineNo(x.Line_No?.Trim()));
            decimal sumHour = sumHourBase + sumHourOvertime;

            // Ensure Line_No is not null before using it in comparisons
            var valuePercent = (sumHour != 0) ? (data.Sum(x => x.Actual_Qty) / sumHour) : 0;
            return Math.Round((decimal)valuePercent, 1);
        }
        private decimal GetValuebyLineNo(string line_No)
        {
            var Value = line_No switch
            {
                "S" => 0.5,
                "M" => 1,
                "L" => 2,
                _ => 0
            };
            return (decimal)Value;
        }

        private decimal GetValueCTB(List<EfficiencyByBrandDto> data)
        {
            var sumHour_IE = data.Sum(x => x.Hour_IE ?? 0);
            var sumHour2 = data.Sum(x => x.Hour_Tot) + data.Sum(x => x.Hour_In) - data.Sum(x => x.Hour_Out) - data.Sum(x => x.Hour_Learn) - data.Sum(x => x.Hour_Transfer);
            var valuePercent = (sumHour2 != null && sumHour2 != 0) ? (sumHour_IE * 100 / sumHour2) : 0;
            return Math.Round((decimal)valuePercent, 1);
        }
        #endregion

        #region utility functions
        // Get all labels (year || month || week | season)
        private List<string> GetLabels(EffiencyKanbanParam param)
        {
            var labels = new List<string>();
            switch (param.Type)
            {
                case "month":
                    labels = param.Months.Select(x => x.Year + (x.Month >= 10 ? "" : "0") + x.Month).ToList();
                    break;
                case "year":
                    labels = param.Years.ConvertAll(x => x + "");
                    break;
                case "week":
                    labels = param.Weeks.Select(x => x.Name).ToList();
                    break;
                default:
                    labels = param.Seasons.Select(x => x.Name).ToList();
                    break;
            }
            return labels;
        }

        // Get months from current month to 5 month ago (6 month)
        private List<MonthOfYear> GetMonthly(int monthCurrent, int year)
        {
            var monthlyAll = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var data = new List<MonthOfYear>();
            if (monthCurrent >= 6)
            {
                var months = monthlyAll.Skip(monthCurrent - 6).Take(6).ToList();
                months.ForEach(month =>
                {
                    var monthOfYear = new MonthOfYear();
                    monthOfYear.Month = month;
                    monthOfYear.Year = year;
                    data.Add(monthOfYear);
                });
            }
            else
            {
                var monthsOfYearBefore = monthlyAll.TakeLast(6 - monthCurrent).ToList();
                monthsOfYearBefore.ForEach(month =>
                {
                    var monthOfYear = new MonthOfYear();
                    monthOfYear.Month = month;
                    monthOfYear.Year = year - 1;
                    data.Add(monthOfYear);
                });
                var monthsOfYearCurrent = monthlyAll.Take(monthCurrent).ToList();
                monthsOfYearCurrent.ForEach(month =>
                {
                    var monthOfYear = new MonthOfYear();
                    monthOfYear.Month = month;
                    monthOfYear.Year = year;
                    data.Add(monthOfYear);
                });
            }
            return data;
        }

        // Get season from current season to 5 season ago (6 season)
        private List<Season> GetSeasons(string division)
        {
            int currentMonth = _maxDate.Month, currentYear = _maxDate.Year;
            int flag = 0, count = 0, nums = 3;

            if (currentMonth >= 3 && currentMonth <= 8) flag = 1;
            else flag = 2;

            var seasons = new List<Season>();
            if (currentMonth >= 9 && currentMonth <= 12) currentYear++;
            for (int i = 0; i < nums; i++)
            {
                if (flag == 1)
                {
                    var season = new Season
                    {
                        SeasonStart = new DateTime(currentYear - i, 3, 1),
                        SeasonFinish = new DateTime(currentYear - i, 8, DateTime.DaysInMonth(currentYear - i, 8)),
                        Name = division == "1" ? (currentYear - i).ToString().Substring(2) + "FW" : "FW" + (currentYear - i).ToString().Substring(2)
                    };
                    seasons.Add(season);
                    if (seasons[0].Name.Substring(0, 2) != "SS" || count != 5)
                    {
                        var season1 = new Season
                        {
                            SeasonStart = new DateTime(currentYear - 1 - i, 9, 1),
                            SeasonFinish = new DateTime(currentYear - i, 2, DateTime.DaysInMonth(currentYear - i, 2)),
                            Name = division == "1" ? (currentYear - i).ToString().Substring(2) + "SS" : "SS" + (currentYear - i).ToString().Substring(2)
                        };
                        seasons.Add(season1);
                    }
                    flag = 1; count += 2;
                }
                else
                {
                    var season = new Season
                    {
                        SeasonStart = new DateTime(currentYear - 1 - i, 9, 1),
                        SeasonFinish = new DateTime(currentYear - i, 2, DateTime.DaysInMonth(currentYear - i, 2)),
                        Name = division == "1" ? currentYear.ToString().Substring(2) + "SS" : "SS" + currentYear.ToString().Substring(2)
                    };
                    seasons.Add(season);
                    flag = 1; count++; nums++;
                }
                if ((currentYear - i) == 2025 && division == "1")
                    break;
            }
            return seasons.OrderBy(x => x.SeasonFinish).ToList();
        }

        // Get weeks from current week to 7 weeks ago (8 week)
        public List<Week> GetWeeks()
        {
            var startOfEndWeek = _maxDate.AddDays(1 - (int)(_maxDate.DayOfWeek));
            var weeks =
                Enumerable
                    .Range(0, 8)
                    .Select(i => new
                    {
                        weekStart = startOfEndWeek.AddDays(-i * 7)
                    })
                    .Select((x, i) => new Week
                    {
                        Name = x.weekStart.ToString("MM/dd-") + x.weekStart.AddDays(6).ToString("MM/dd"),
                        WeekStart = x.weekStart,
                        WeekFinish = x.weekStart.AddDays(6)
                    });

            var data = weeks.OrderBy(x => x.WeekFinish).ToList();

            if (data[data.Count - 1].WeekFinish > _maxDate && data[data.Count - 1].WeekStart <= _maxDate)
            {
                data[data.Count - 1].WeekFinish = _maxDate;
            }

            return data.Select(m => new Week
            {
                Name = m.WeekStart.ToString("MM/dd-") + m.WeekFinish.ToString("MM/dd"),
                WeekStart = m.WeekStart,
                WeekFinish = m.WeekFinish
            }).ToList();
        }
        #endregion

        private async Task<DateTime> GetMaxDate()
        {
            DateTime maxDateDeptScore =
                _repoAccessor.eTM_HP_Efficiency_Data.FindAll().Any() ? _repoAccessor.eTM_HP_Efficiency_Data.FindAll().Max(x => x.Data_Date) : DateTime.Now.AddDays(-1);
            return await Task.FromResult(maxDateDeptScore);
        }

        private static string GetDateRangeType(string type)
        {
            return type switch
            {
                "month" => "Monthly",
                "year" => "Yearly",
                "week" => "Weekly",
                _ => "Season",
            };
        }

        private List<decimal> GetFactoryValues(string factoryName, List<eTM_HP_Efficiency_Data_External> dataFind)
        {
            return factoryName switch
            {
                "SHC" => dataFind.Select(x => x.SHC).Where(x => x.HasValue).Select(x => x.Value).ToList(),
                "CB" => dataFind.Select(x => x.CB).Where(x => x.HasValue).Select(x => x.Value).ToList(),
                _ => dataFind.Select(x => x.TSH).Where(x => x.HasValue).Select(x => x.Value).ToList(),
            };
        }
        public async Task<List<FactoryInfo>> GetListFactory()
        {
            var value = _repoAccessor.eTM_Settings.FirstOrDefault(x => x.Key == "Manuf").Value.Trim();
            var column_02 = _repoAccessor.SM_Basic_Data.FirstOrDefault(x => x.Column_03 == value && x.Basic_Class == "Organization.Chart")?.Column_02;

            return await _repoAccessor.SM_Basic_Data.FindAll(x => x.Column_02 == column_02 && x.Basic_Class == "Organization.Chart").Select(x => new FactoryInfo
            {
                Stt = x.Column_08,
                Id = x.Column_03,
                Name = x.Column_09,
                Status = false
            }).Distinct().OrderBy(x => x.Stt).ToListAsync();
        }

        public async Task<List<string>> GetListBrand()
        {
            var value = _repoAccessor.eTM_Settings.FirstOrDefault(x => x.Key == "Manuf").Value.Trim();
            var column_02 = _repoAccessor.SM_Basic_Data.FirstOrDefault(x => x.Column_03 == value && x.Basic_Class == "Organization.Chart")?.Column_02;
            var column_03 = _repoAccessor.SM_Basic_Data.FirstOrDefault(x => x.Column_02 == column_02 && x.Basic_Class == "Organization.Chart")?.Column_03;
            return await _repoAccessor.VW_Efficiency_ByBrand.FindAll(x => x.Factory_ID.Contains(column_03)).Select(x => x.Brand).Distinct().ToListAsync();
        }
    }
}