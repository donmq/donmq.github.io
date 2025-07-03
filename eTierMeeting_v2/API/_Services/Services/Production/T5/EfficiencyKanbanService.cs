using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using eTierV2_API._Repositories;
using eTierV2_API._Services.Interfaces.Production.T5;
using eTierV2_API.Data;
using eTierV2_API.DTO.Production.T5.EfficiencyKanban;
using eTierV2_API.Helpers.Enums;
using eTierV2_API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace eTierV2_API._Services.Services.Production.T5
{
    public class EfficiencyKanbanService : IEfficiencyKanbanService
    {
        private readonly DateTime _maxDate;
        private readonly IServiceProvider _serviceProvider;
        private readonly HashSet<string> _qaLinePrefixList;
        private readonly HashSet<string> _quantizationPrefixList;
        private readonly IRepositoryAccessor _repoAccessor;
        public EfficiencyKanbanService(IRepositoryAccessor repoAccessor,
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
            List<EfficiencyDto> data1;
            List<EfficiencyDto> data2;
            dataPPH2 dataPPH2;
            List<eTM_HP_Efficiency_Data_External> dataModelEfficiencyFakeExt;
            List<EfficiencyDto> dataModelEfficiency;

            // Get the required DbContext instances from each scope.
            using var scope1 = _serviceProvider.CreateScope();
            using var scope2 = _serviceProvider.CreateScope();
            using var scope3 = _serviceProvider.CreateScope();
            using var scope4 = _serviceProvider.CreateScope();
            using var scope5 = _serviceProvider.CreateScope();

            // Get the required DbContext instances from each scope.
            Debug.WriteLine($"1, {stopwatch.ElapsedMilliseconds}");
            var dbContext1 = scope1.ServiceProvider.GetRequiredService<DBContext>();
            var dbContext2 = scope2.ServiceProvider.GetRequiredService<DBContext>();
            var dbContext3 = scope3.ServiceProvider.GetRequiredService<DBContext>();
            var dbContext4 = scope4.ServiceProvider.GetRequiredService<DBContext>();
            var dbContext5 = scope5.ServiceProvider.GetRequiredService<DBContext>();

            // Prepare data using async methods to utilize CPU resources fully.
            var data1Task = PrepareData1(dbContext1, param);
            var data2Task = PrepareData2(dbContext2, param);
            var dataPPH2Task = PrepareDataPPH2(dbContext3, param);
            var dataModelEfficiencyFakeExtTask = PrepareDataModelEfficiencyFakeExt(dbContext4);
            var dataModelEfficiencyTask = PrepareDataModelEfficiency(dbContext5, param);

            // Wait for all async methods to complete.
            await Task.WhenAll(data1Task, data2Task, dataPPH2Task, dataModelEfficiencyFakeExtTask, dataModelEfficiencyTask);

            data1 = data1Task.Result;
            data2 = data2Task.Result;
            dataPPH2 = dataPPH2Task.Result;
            dataModelEfficiencyFakeExt = dataModelEfficiencyFakeExtTask.Result;
            dataModelEfficiency = dataModelEfficiencyTask.Result;

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
                    case Common.ITEM_PERFORMANCE_ACHIEVEMENT:
                        var dataPerformance_Achievement = GetDataPerformanceAchievement(data1, param, item);
                        dataEfficiency.Add(dataPerformance_Achievement);
                        break;
                    case Common.ITEM_EOLR:
                        var dataEOLR = GetDataEOLRAchievement(data1, param, item);
                        dataEfficiency.Add(dataEOLR);
                        break;
                    case Common.ITEM_IE_ACHIEVEMENT:
                        var dataIE = GetDataIEAchievement(data2, param, item);
                        dataEfficiency.Add(dataIE);
                        break;
                    case Common.ITEM_CTB_IE_ACHIEVEMENT:
                        var dataCTB = GetDataCTBIEAchievement(dataModelEfficiency, param, item);
                        dataEfficiency.Add(dataCTB);
                        break;
                    case Common.ITEM_ABNORMAL_WORKING_HOUR:
                        var dataAbnormalEff = GetDataAbnormalWorkingHourEfficiency(dataModelEfficiency, param, item);
                        dataEfficiency.Add(dataAbnormalEff);
                        break;
                    case Common.ITEM_PPH:
                        var dataPPH = GetDataPPHAchievement(dataPPH2.dataEfficiency, param, dataPPH2.dataEfficiencyDataSubcon, item);
                        dataEfficiency.Add(dataPPH);
                        break;
                    default:
                        var dataModEff = param.Is_T5_External
                            ? GetDataModelEfficiencyFakeExt(dataModelEfficiencyFakeExt, param, "%", 1, item)
                            : GetDataModelEfficiency(dataModelEfficiency, param, item);
                        dataEfficiency.Add(dataModEff);
                        break;
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

        // --------------------------------------Query chung cho 3 chart --------------------------------------------------------//
        // + Target_Achievement
        // + Performance_Achievement
        // + EOLR
        //------------------------------------------------------------------------------------------------------------------------//
        // [SQL 1] Target_Achievement, Performance_Achievement, & EOLR
        public async Task<List<EfficiencyDto>> PrepareData1(DBContext context, EffiencyKanbanParam param)
        {
            System.Diagnostics.Debug.WriteLine("PrepareData1 Start");
            IQueryable<EfficiencyDto> dataQuery1_group;
            var pred_EfficiencyData = PredicateBuilder.New<VW_eTM_HP_Efficiency_Data>(x => param.Factorys.Select(y => y.Id).Contains(x.Factory_ID.TrimEnd()));
            if (param.Type == "month")
                pred_EfficiencyData.And(x => param.Months.Select(y => y.Month).Contains(x.Data_Date.Month));
            else if (param.Type == "year")
                pred_EfficiencyData.And(x => param.Years.Contains(x.Data_Date.Year));
            else if (param.Type == "week")
                pred_EfficiencyData.And(x => param.Weeks.First().WeekStart.Date <= x.Data_Date && param.Weeks.Last().WeekFinish >= x.Data_Date);
            else
                pred_EfficiencyData.And(x => param.Seasons.First().SeasonStart.Date <= x.Data_Date && param.Seasons.Last().SeasonFinish >= x.Data_Date);
            pred_EfficiencyData.And(x => x.Kind == "5");
            var vwEfficiencyData = _repoAccessor.VW_eTM_HP_Efficiency_Data.FindAllContext(context, pred_EfficiencyData);
            var deptIdOfVW = vwEfficiencyData.Select(x => x.Dept_ID.Trim()).Distinct().ToList();
            var dataHP_HP_Production_Line_ie21 = _repoAccessor.HP_Production_Line_ie21.FindAllContext(context);
            var dataQuery1 = from vwdata in vwEfficiencyData
                             join ie21 in dataHP_HP_Production_Line_ie21
                             on new { vwdata.Dept_ID, vwdata.Factory_ID }
                             equals new { ie21.Dept_ID, ie21.Factory_ID } into joinedData
                             from ie21 in joinedData.DefaultIfEmpty()
                             select new { vwdata, ie21 };

            if (param.Type == "week")
            {
                dataQuery1_group = from data in dataQuery1
                                   group new { data.vwdata, data.ie21 } by new { data.vwdata.Factory_ID, data.vwdata.Data_Date, data.ie21.Line_No } into pg
                                   select new EfficiencyDto()
                                   {
                                       Factory = pg.Key.Factory_ID.TrimEnd(),
                                       Line_No = pg.Key.Line_No,
                                       Data_Date = pg.Key.Data_Date,
                                       Actual_Qty = pg.Sum(m => m.vwdata.Actual_Qty),
                                       Target_Qty = pg.Sum(m => m.vwdata.Target_Qty),
                                       Impact_Qty = pg.Sum(m => m.vwdata.Impact_Qty),
                                       Hour_Base = pg.Sum(m => m.vwdata.Hour_Base),
                                       Hour_Overtime = pg.Sum(m => m.vwdata.Hour_Overtime),
                                       Month = pg.Max(x => x.vwdata.Data_Date.Month),
                                       Year = pg.Max(x => x.vwdata.Data_Date.Year)
                                   };
            }
            else
            {
                dataQuery1_group = from data in dataQuery1
                                   group new { data.vwdata, data.ie21 } by new { data.vwdata.Factory_ID, data.vwdata.Data_Date.Month, data.vwdata.Data_Date.Year, data.ie21.Line_No } into pg
                                   select new EfficiencyDto()
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
            }

            var result = await dataQuery1_group.ToListAsync();
            System.Diagnostics.Debug.WriteLine("PrepareData1 End");
            return result;
        }

        // --------------------------------------Query chung cho 2 chart --------------------------------------------------------//
        // + IE Achievement
        // + CTB IE Achievement
        //------------------------------------------------------------------------------------------------------------------------//
        // [SQL 2]  Full Factory IE Achievement 
        public async Task<List<EfficiencyDto>> PrepareData2(DBContext context, EffiencyKanbanParam param)
        {
            System.Diagnostics.Debug.WriteLine("PrepareData2 Start");
            IQueryable<EfficiencyDto> dataQuery2_group;
            var pred_EfficiencyData = PredicateBuilder.New<VW_eTM_HP_Efficiency_Data>(x => x.Kind != null && param.Factorys.Select(y => y.Id).Contains(x.Factory_ID.TrimEnd()));
            if (param.Type == "month")
                pred_EfficiencyData.And(x => param.Months.Select(y => y.Month).Contains(x.Data_Date.Month));
            else if (param.Type == "year")
                pred_EfficiencyData.And(x => param.Years.Contains(x.Data_Date.Year));
            else if (param.Type == "week")
                pred_EfficiencyData.And(x => param.Weeks.First().WeekStart.Date <= x.Data_Date && param.Weeks.Last().WeekFinish >= x.Data_Date);
            else
                pred_EfficiencyData.And(x => param.Seasons.First().SeasonStart.Date <= x.Data_Date && param.Seasons.Last().SeasonFinish >= x.Data_Date);
            var efficiencyData2s = _repoAccessor.VW_eTM_HP_Efficiency_Data.FindAllContext(context, pred_EfficiencyData);

            var dataQuery2 = from data in efficiencyData2s
                             select new EfficiencyDto
                             {
                                 Hour_IE = data.Hour_IE,
                                 Hour_Tot = data.Hour_Tot,
                                 Hour_Tot_All = data.Hour_Tot_All,
                                 Hour_In = data.Hour_In,
                                 Hour_Out = data.Hour_Out,
                                 Hour_Learn = data.Hour_Learn,
                                 Hour_Transfer = data.Hour_Transfer,
                                 Hour_Ex = data.Hour_Ex,
                                 Hour_Tot_2 = data.Hour_Tot_2,
                                 Hour_UT005 = data.Hour_UT005,
                                 Hour_OEM = data.Hour_OEM,
                                 Factory = data.Factory_ID.TrimEnd(),
                                 Data_Date = data.Data_Date,
                                 Month = data.Data_Date.Month,
                                 Year = data.Data_Date.Year,
                                 Kind = data.Kind.Trim(),
                                 IsDevCenterLines = data.Factory_ID == "C" && data.Dept_ID.StartsWith("B"),
                                 IsQALines = data.Factory_ID == "C" && _qaLinePrefixList.Contains(data.Dept_ID.Substring(0, 2)),
                                 IsQuantizationLines = data.Factory_ID == "C" && _quantizationPrefixList.Contains(data.Dept_ID.Substring(0, 3)),
                                 Dept_ID = data.Dept_ID,
                                 Actual_Qty = data.Actual_Qty
                             };


            if (param.Type == "week")
            {
                dataQuery2_group = from a in dataQuery2
                                   group a by new { a.Factory, a.Data_Date, a.Kind, a.IsQALines, a.IsQuantizationLines } into pg
                                   select new EfficiencyDto()
                                   {
                                       Factory = pg.Key.Factory.TrimEnd(),
                                       Data_Date = pg.Key.Data_Date,
                                       Kind = pg.Key.Kind,
                                       IsQALines = pg.Key.IsQALines,
                                       IsQuantizationLines = pg.Key.IsQuantizationLines,
                                       Hour_IE = pg.Sum(m => m.Hour_IE ?? 0),
                                       Hour_Tot = pg.Sum(m => m.Hour_Tot ?? 0),
                                       Hour_In = pg.Sum(m => m.Hour_In ?? 0),
                                       Hour_Out = pg.Sum(m => m.Hour_Out ?? 0),
                                       Hour_Learn = pg.Sum(m => m.Hour_Learn ?? 0),
                                       Hour_Transfer = pg.Sum(m => m.Hour_Transfer ?? 0),
                                       Hour_Ex = pg.Sum(m => m.Hour_Ex ?? 0),
                                       Hour_Tot_2 = pg.Sum(m => m.Hour_Tot_2 ?? 0),
                                       Hour_UT005 = pg.Sum(m => m.Hour_UT005 ?? 0),
                                       Hour_OEM = pg.Sum(m => m.Hour_OEM ?? 0),
                                       Month = pg.Max(m => m.Month),
                                       Year = pg.Max(m => m.Year),
                                       Actual_Qty = pg.Sum(m => m.Actual_Qty ?? 0)
                                   };
            }
            else
            {
                dataQuery2_group = from a in dataQuery2
                                   group a by new { a.Factory, a.Kind, a.IsQALines, a.Year, a.Month, a.IsQuantizationLines } into pg
                                   select new EfficiencyDto()
                                   {
                                       Factory = pg.Key.Factory.TrimEnd(),
                                       Data_Date = new DateTime(
                                           pg.Key.Year,
                                           pg.Key.Month,
                                           1
                                       ),
                                       Kind = pg.Key.Kind,
                                       IsQALines = pg.Key.IsQALines,
                                       IsQuantizationLines = pg.Key.IsQuantizationLines,
                                       Hour_IE = pg.Sum(m => m.Hour_IE),
                                       Hour_Tot = pg.Sum(m => m.Hour_Tot),
                                       Hour_In = pg.Sum(m => m.Hour_In),
                                       Hour_Out = pg.Sum(m => m.Hour_Out),
                                       Hour_Learn = pg.Sum(m => m.Hour_Learn),
                                       Hour_Transfer = pg.Sum(m => m.Hour_Transfer),
                                       Hour_Ex = pg.Sum(m => m.Hour_Ex),
                                       Hour_Tot_2 = pg.Sum(m => m.Hour_Tot_2),
                                       Hour_UT005 = pg.Sum(m => m.Hour_UT005),
                                       Hour_OEM = pg.Sum(m => m.Hour_OEM),
                                       Month = pg.Max(m => m.Month),
                                       Year = pg.Max(m => m.Year),
                                       Actual_Qty = pg.Sum(m => m.Actual_Qty)
                                   };
            }

            var result = await dataQuery2_group.ToListAsync();
            System.Diagnostics.Debug.WriteLine("PrepareData2 End");
            return result;
        }

        // --------------------------------------Query PPH ----------------------------------------------------------------------//
        // PPH = [Actual_Qty] / [Total Hour]
        //------------------------------------------------------------------------------------------------------------------------//

        public async Task<List<EfficiencyDto>> PrepareDataPPH1(DBContext context, ExpressionStarter<VW_eTM_HP_Efficiency_Data> pred_EfficiencyData, EffiencyKanbanParam param)
        {
            System.Diagnostics.Debug.WriteLine("PrepareDataPPH1 Start");
            IQueryable<EfficiencyDto> dataQueryPPH1;

            var vwEfficiencyData = _repoAccessor.VW_eTM_HP_Efficiency_Data.FindAllContext(context, pred_EfficiencyData);
            var deptIdOfVW = vwEfficiencyData.Select(x => x.Dept_ID.Trim()).Distinct().ToList();
            var tbl_eTM_HP_G01_Flag = _repoAccessor.eTM_HP_G01_Flag.FindAllContext(context, x => deptIdOfVW.Contains(x.Dept_ID.Trim()));
            var dataQuery2 = from a in vwEfficiencyData
                             join b in tbl_eTM_HP_G01_Flag
                             on new { a.Factory_ID, a.Dept_ID } equals new { b.Factory_ID, b.Dept_ID } into joinedData
                             from data_eTM_HP_G01_Flag in joinedData.DefaultIfEmpty()
                             select new EfficiencyDto()
                             {
                                 Hour_IE = a.Hour_IE,
                                 Hour_Tot = a.Hour_Tot,
                                 Hour_Tot_All = a.Hour_Tot_All,
                                 Hour_In = a.Hour_In,
                                 Hour_Out = a.Hour_Out,
                                 Hour_Learn = a.Hour_Learn,
                                 Hour_Transfer = a.Hour_Transfer,
                                 Hour_Ex = a.Hour_Ex,
                                 Hour_Tot_2 = a.Hour_Tot_2,
                                 Hour_UT005 = a.Hour_UT005,
                                 Hour_OEM = a.Hour_OEM,
                                 Factory = a.Factory_ID.Trim(),
                                 Data_Date = a.Data_Date,
                                 Month = a.Data_Date.Month,
                                 Year = a.Data_Date.Year,
                                 Kind = a.Kind.Trim(),
                                 IsDevCenterLines = a.Factory_ID == "C" && a.Dept_ID.StartsWith("B"),
                                 IsQALines = a.Factory_ID == "C" && _qaLinePrefixList.Contains(a.Dept_ID.Substring(0, 2)),
                                 IsQuantizationLines = a.Factory_ID == "C" && _quantizationPrefixList.Contains(a.Dept_ID.Substring(0, 3)),
                                 Dept_ID = a.Dept_ID,
                                 Actual_Qty = a.Actual_Qty
                             };

            if (param.Type == "week")
            {
                dataQueryPPH1 = dataQuery2
                    .Where(x =>
                        x.Kind.Trim() == "5" &&
                        (
                            !(x.Factory == "C" && (x.IsDevCenterLines || x.IsQALines)) ||
                            (x.Factory == "C" && x.IsQuantizationLines)
                        )
                    )
                    .GroupBy(x => new { x.Factory, x.Data_Date })
                    .Select(x => new EfficiencyDto
                    {
                        Factory = x.Key.Factory.Trim(),
                        Data_Date = x.Key.Data_Date,
                        Actual_Qty = x.Sum(m => m.Actual_Qty)
                    });

            }
            else
            {
                dataQueryPPH1 = dataQuery2
                    .Where(x =>
                        x.Kind.Trim() == "5" &&
                        (
                            !(x.Factory == "C" && (x.IsDevCenterLines || x.IsQALines)) ||
                            (x.Factory == "C" && x.IsQuantizationLines)
                        ))
                    .GroupBy(x => new { x.Factory, x.Year, x.Month })
                    .Select(x => new EfficiencyDto
                    {
                        Factory = x.Key.Factory.Trim(),
                        Data_Date = new DateTime(
                            x.Key.Year,
                            x.Key.Month,
                            1
                        ),
                        Actual_Qty = x.Sum(m => m.Actual_Qty)
                    });
            }

            var result = await dataQueryPPH1.ToListAsync();
            System.Diagnostics.Debug.WriteLine("PrepareDataPPH1 End");
            return result;
        }
        // [SQL 3]  PPH
        public async Task<dataPPH2> PrepareDataPPH2(DBContext context, EffiencyKanbanParam param)
        {
            System.Diagnostics.Debug.WriteLine("PrepareDataPPH2 Start");
            IQueryable<EfficiencyDto> dataQueryPPH2;
            var pred_EfficiencyData = PredicateBuilder.New<VW_eTM_HP_Efficiency_Data>(x => param.Factorys.Select(y => y.Id).Contains(x.Factory_ID.TrimEnd()));
            var pred_EfficiencyDataSubcon = PredicateBuilder.New<eTM_HP_Efficiency_Data_Subcon>(x => param.Factorys.Select(y => y.Id).Contains(x.Factory_ID.TrimEnd()));

            if (param.Type == "month")
            {
                pred_EfficiencyData.And(x => param.Months.Select(y => y.Month).Contains(x.Data_Date.Month));
                pred_EfficiencyDataSubcon.And(x => param.Months.Select(y => y.Month).Contains(x.Data_Date.Month));
            }
            else if (param.Type == "year")
            {
                pred_EfficiencyData.And(x => param.Years.Contains(x.Data_Date.Year));
                pred_EfficiencyDataSubcon.And(x => param.Years.Contains(x.Data_Date.Year));
            }
            else if (param.Type == "week")
            {
                pred_EfficiencyData.And(x => param.Weeks.First().WeekStart.Date <= x.Data_Date && param.Weeks.Last().WeekFinish >= x.Data_Date);
                pred_EfficiencyDataSubcon.And(x => param.Weeks.First().WeekStart.Date <= x.Data_Date && param.Weeks.Last().WeekFinish >= x.Data_Date);
            }
            else
            {
                pred_EfficiencyData.And(x => param.Seasons.First().SeasonStart.Date <= x.Data_Date && param.Seasons.Last().SeasonFinish >= x.Data_Date);
                pred_EfficiencyDataSubcon.And(x => param.Seasons.First().SeasonStart.Date <= x.Data_Date && param.Seasons.Last().SeasonFinish >= x.Data_Date);
            }
            var efficiencyData = _repoAccessor.VW_eTM_HP_Efficiency_Data.FindAllContext(context, pred_EfficiencyData);
            var dataHPG01Flag = _repoAccessor.eTM_HP_G01_Flag.FindAllContext(context);

            if (param.Type == "week")
            {
                dataQueryPPH2 = from a in efficiencyData
                                join b in dataHPG01Flag
                                on new { a.Factory_ID, a.Dept_ID } equals new { b.Factory_ID, b.Dept_ID } into joinData
                                from b in joinData.DefaultIfEmpty()
                                where (
                                    a.Factory_ID.TrimEnd() == "C" && a.Data_Date < new DateTime(2024, 02, 01) &&
                                    !a.Dept_ID.StartsWith("QA") && a.Dept_ID != "QXP" &&
                                    (!(a.Dept_ID.StartsWith("B") || _quantizationPrefixList.Contains(a.Dept_ID.Substring(0, 3)))) && b.Dept_ID == null
                                ) || a.Factory_ID.TrimEnd() != "C" || a.Data_Date >= new DateTime(2024, 02, 01)
                                group a by new { a.Factory_ID, a.Data_Date, a.Kind } into pg
                                select new EfficiencyDto()
                                {
                                    Factory = pg.Key.Factory_ID.TrimEnd(),
                                    Kind = pg.Key.Kind,
                                    Data_Date = pg.Key.Data_Date,
                                    Actual_Qty = pg.Sum(x => x.Kind == "5" ? x.Actual_Qty : 0),
                                    Hour_Tot_All = pg.Sum(m => m.Hour_Tot_All)
                                };
            }
            else
            {
                dataQueryPPH2 = from a in efficiencyData
                                join b in dataHPG01Flag
                                on new { a.Factory_ID, a.Dept_ID } equals new { b.Factory_ID, b.Dept_ID } into joinData
                                from p in joinData.DefaultIfEmpty()
                                where (
                                    a.Factory_ID == "C" && a.Data_Date < new DateTime(2024, 02, 01) &&
                                    !a.Dept_ID.StartsWith("QA") && a.Dept_ID != "QXP" &&
                                    (!(a.Dept_ID.StartsWith("B") || _quantizationPrefixList.Contains(a.Dept_ID.Substring(0, 3)))) && p.Dept_ID == null
                                ) || a.Factory_ID != "C" || a.Data_Date >= new DateTime(2024, 02, 01)
                                group a by new { a.Factory_ID, a.Data_Date.Year, a.Data_Date.Month, a.Kind } into pg
                                select new EfficiencyDto()
                                {
                                    Factory = pg.Key.Factory_ID.TrimEnd(),
                                    Kind = pg.Key.Kind,
                                    Data_Date = new DateTime(
                                        pg.Key.Year,
                                        pg.Key.Month,
                                        1
                                    ),
                                    Month = pg.Key.Month,
                                    Year = pg.Key.Year,
                                    Actual_Qty = pg.Sum(x => x.Kind == "5" ? x.Actual_Qty : 0),
                                    Hour_Tot_All = pg.Sum(m => m.Hour_Tot_All)
                                };
            }
            var dataEfficiency = await dataQueryPPH2.ToListAsync();
            var dataEfficiencyDataSubcon = await _repoAccessor.eTM_HP_Efficiency_Data_Subcon.FindAllContext(context, pred_EfficiencyDataSubcon).ToListAsync();

            dataPPH2 result = new dataPPH2
            {
                dataEfficiencyDataSubcon = dataEfficiencyDataSubcon,
                dataEfficiency = dataEfficiency
            };
            System.Diagnostics.Debug.WriteLine("PrepareDataPPH2 End");
            return result;
        }

        /* ------------------------- Query Model Efficiency ------------------------- */
        // [SQL 4]  Model Efficiency & CTB IE Achievement & Abnormal Working Hour %
        public async Task<List<EfficiencyDto>> PrepareDataModelEfficiency(DBContext context, EffiencyKanbanParam param)
        {
            System.Diagnostics.Debug.WriteLine("PrepareModel Start");
            IQueryable<EfficiencyDto> dataQueryModEffGroup;
            var pred_EfficiencyData = PredicateBuilder.New<VW_eTM_HP_Efficiency_Data>(x => x.Kind != null && x.IsCTB == 1 && param.Factorys.Select(y => y.Id).Contains(x.Factory_ID.TrimEnd()));
            if (param.Type == "month")
                pred_EfficiencyData.And(x => param.Months.Select(y => y.Month).Contains(x.Data_Date.Month));
            else if (param.Type == "year")
                pred_EfficiencyData.And(x => param.Years.Contains(x.Data_Date.Year));
            else if (param.Type == "week")
                pred_EfficiencyData.And(x => param.Weeks.First().WeekStart.Date <= x.Data_Date && param.Weeks.Last().WeekFinish >= x.Data_Date);
            else
                pred_EfficiencyData.And(x => param.Seasons.First().SeasonStart.Date <= x.Data_Date && param.Seasons.Last().SeasonFinish >= x.Data_Date);
            var dataQueryModEff = _repoAccessor.VW_eTM_HP_Efficiency_Data.FindAllContext(context, pred_EfficiencyData);
            if (param.Type == "week")
            {
                dataQueryModEffGroup =
                    from data in dataQueryModEff
                    group data by new { Factory_ID = data.Factory_ID.TrimEnd(), data.Data_Date } into pg
                    select new EfficiencyDto()
                    {
                        Factory = pg.Key.Factory_ID.TrimEnd(),
                        Data_Date = pg.Key.Data_Date,
                        Hour_IE = pg.Sum(m => m.Hour_IE),
                        Hour_Ex = pg.Sum(m => m.Hour_Ex),
                        Hour_Tot = pg.Sum(m => m.Hour_Tot),
                        Hour_Tot_2 = pg.Sum(m => m.Hour_Tot_2),
                        Hour_UT005 = pg.Sum(m => m.Hour_UT005),
                        Hour_OEM = pg.Sum(m => m.Hour_OEM),
                        Hour_Tot_CMT = pg.Sum(m => m.Hour_Tot_CMT),
                        Hour_In = pg.Sum(m => m.Hour_In),
                        Hour_Out = pg.Sum(m => m.Hour_Out),
                        Hour_Learn = pg.Sum(m => m.Hour_Learn),
                        Hour_Transfer = pg.Sum(m => m.Hour_Transfer),
                    };
            }
            else
            {
                dataQueryModEffGroup =
                    from data in dataQueryModEff
                    group data by new { Factory_ID = data.Factory_ID.TrimEnd(), Data_Year = data.Data_Date.Year, Data_Month = data.Data_Date.Month } into pg
                    select new EfficiencyDto()
                    {
                        Factory = pg.Key.Factory_ID.TrimEnd(),
                        Data_Date = new DateTime(pg.Key.Data_Year, pg.Key.Data_Month, 1),
                        Qty_2 = pg.Sum(m => m.Qty_2),
                        Hour_IE = pg.Sum(m => m.Hour_IE),
                        Hour_Tot = pg.Sum(m => m.Hour_Tot),
                        Hour_Ex = pg.Sum(m => m.Hour_Ex),
                        Hour_Tot_2 = pg.Sum(m => m.Hour_Tot_2),
                        Hour_UT005 = pg.Sum(m => m.Hour_UT005),
                        Hour_OEM = pg.Sum(m => m.Hour_OEM),
                        Un_Man = pg.Sum(m => m.Un_Man),
                        Month = pg.Max(m => m.Data_Date.Month),
                        Year = pg.Max(m => m.Data_Date.Year),
                        Hour_Tot_CMT = pg.Sum(m => m.Hour_Tot_CMT),
                        Hour_In = pg.Sum(m => m.Hour_In),
                        Hour_Out = pg.Sum(m => m.Hour_Out),
                        Hour_Learn = pg.Sum(m => m.Hour_Learn),
                        Hour_Transfer = pg.Sum(m => m.Hour_Transfer)
                    };
            }
            var result = await dataQueryModEffGroup.ToListAsync();
            System.Diagnostics.Debug.WriteLine("PrepareModel End");
            return result;
        }
        public async Task<List<eTM_HP_Efficiency_Data_External>> PrepareDataModelEfficiencyFakeExt(DBContext context, string performanceType = "model_efficiency")
        {
            var dataQueryInFactorys = await _repoAccessor.ETM_HP_Efficiency_Data_External.FindAllContext(context, x => x.Performance_Type == performanceType).ToListAsync();
            return dataQueryInFactorys;
        }
        #region Setting chart name, grouping and sort seq the Item chart at Prod > T5 > Efficiency from DB. Page 109 - eTM系統規格書 v3.0.4_20250609
        #endregion
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
                .Join(_repoAccessor.SM_Basic_Data.FindAll(x => x.Basic_Class == "Prod.T5.Item.AdditionalProperty1"),
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
        private EfficiencyKanbanModel GetDataTargetAchievement(List<EfficiencyDto> dataQuery, EffiencyKanbanParam param, DataChartTitle chartTitle)
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
                ShowValues = Convert.ToInt32(param.Factorys.Count() == 1),
                ChartUnit = "%",
                IsActive = true,
                Digits = 1,
                Labels = GetLabels(param),
                Data = targetAchievementData
            };
            return result;
        }
        private EfficiencyKanbanModel GetDataPerformanceAchievement(List<EfficiencyDto> dataQuery, EffiencyKanbanParam param, DataChartTitle chartTitle)
        {
            var performanceAchievementData = new List<FactoryDataChart>();
            foreach (var factory in param.Factorys)
            {
                var values = new List<decimal>();
                var actualQties = new List<int?>();
                var dataQueryInFactory = dataQuery.Where(x => x.Factory == factory.Id);

                if (param.Type == "month")
                {
                    foreach (var monthOfYearItem in param.Months)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Month == monthOfYearItem.Month && x.Year == monthOfYearItem.Year).ToList();
                        var valuePercent = GetValuePerformance(dataFind);
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
                        var valuePercent = GetValuePerformance(dataFind);
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
                        var valuePercent = GetValuePerformance(dataFind);
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
                        var valuePercent = GetValuePerformance(dataFind);
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
                performanceAchievementData.Add(factoryData);
            }

            var result = new EfficiencyKanbanModel()
            {
                Title = chartTitle.Chart_Display_Name,
                Group = chartTitle.Item_Group_SortSeq,
                ShowValues = Convert.ToInt32(param.Factorys.Count() == 1),
                ChartUnit = "%",
                IsActive = true,
                Digits = 1,
                Labels = GetLabels(param),
                Data = performanceAchievementData
            };
            return result;
        }
        private EfficiencyKanbanModel GetDataEOLRAchievement(List<EfficiencyDto> dataQuery, EffiencyKanbanParam param, DataChartTitle chartTitle)
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
        private EfficiencyKanbanModel GetDataIEAchievement(List<EfficiencyDto> dataQuery, EffiencyKanbanParam param, DataChartTitle chartTitle)
        {
            var iEData = new List<FactoryDataChart>();
            foreach (var factory in param.Factorys)
            {
                var values = new List<decimal>();
                var dataQueryInFactory = dataQuery.Where(x => x.Factory == factory.Id && x.Kind != null).ToList();

                if (param.Type == "month")
                {
                    foreach (var monthOfYearItem in param.Months)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Month == monthOfYearItem.Month && x.Year == monthOfYearItem.Year).ToList();
                        var valuePercent = GetValueIE(dataFind);
                        values.Add(valuePercent);
                    }
                }
                else if (param.Type == "year")
                {
                    foreach (var year in param.Years)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Year == year).ToList();
                        var valuePercent = GetValueIE(dataFind);
                        values.Add(valuePercent);
                    }
                }
                else if (param.Type == "week")
                {
                    foreach (var week in param.Weeks)
                    {
                        var dataFind = dataQueryInFactory.Where(x => (x.Data_Date >= week.WeekStart.Date && x.Data_Date <= week.WeekFinish.Date)).ToList();
                        var valuePercent = GetValueIE(dataFind);
                        values.Add(valuePercent);
                    }
                }
                else
                {
                    foreach (var season in param.Seasons)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Data_Date >= season.SeasonStart.Date && x.Data_Date <= season.SeasonFinish.Date).ToList();
                        var valuePercent = GetValueIE(dataFind);
                        values.Add(valuePercent);
                    }
                }

                var factoryData = new FactoryDataChart()
                {
                    Name = factory.Name,
                    Value = values
                };
                iEData.Add(factoryData);
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
                Data = iEData
            };
            return result;
        }
        private EfficiencyKanbanModel GetDataCTBIEAchievement(List<EfficiencyDto> dataQuery, EffiencyKanbanParam param, DataChartTitle chartTitle)
        {
            var kindCTBs = new List<string> { "8", "2", "3", "5", "7" };
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

        private EfficiencyKanbanModel GetDataPPHAchievement(List<EfficiencyDto> dataQuery, EffiencyKanbanParam param, List<eTM_HP_Efficiency_Data_Subcon> dataEfficiencyDataSubcon, DataChartTitle chartTitle)
        {
            var pPHData = new List<FactoryDataChart>();
            foreach (var factory in param.Factorys)
            {
                var values = new List<decimal>();
                var dataQueryInFactory = dataQuery.Where(x => x.Factory == factory.Id);
                var subconWhr = dataEfficiencyDataSubcon.Where(x => x.Factory_ID == factory.Id);
                if (param.Type == "month")
                {
                    foreach (var monthOfYearItem in param.Months)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Data_Date.Value.Month == monthOfYearItem.Month && x.Data_Date.Value.Year == monthOfYearItem.Year).ToList();
                        var subconFind = subconWhr.Where(x => x.Data_Date.Month == monthOfYearItem.Month && x.Data_Date.Year == monthOfYearItem.Year).ToList();
                        var valuePercent = GetValuePPH(dataFind, subconFind);
                        values.Add(valuePercent);
                    }
                }
                else if (param.Type == "year")
                {
                    foreach (var year in param.Years)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Data_Date.Value.Year == year).ToList();
                        var subconFind = subconWhr.Where(x => x.Data_Date.Year == year).ToList();
                        var valuePercent = GetValuePPH(dataFind, subconFind);
                        values.Add(valuePercent);
                    }
                }
                else if (param.Type == "week")
                {
                    foreach (var week in param.Weeks)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Data_Date >= week.WeekStart.Date && x.Data_Date <= week.WeekFinish.Date).ToList();
                        var subconFind = subconWhr.Where(x => x.Data_Date >= week.WeekStart.Date && x.Data_Date <= week.WeekFinish.Date).ToList();
                        var valuePercent = GetValuePPH(dataFind, subconFind);
                        values.Add(valuePercent);
                    }
                }
                else
                {
                    foreach (var season in param.Seasons)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Data_Date >= season.SeasonStart.Date && x.Data_Date <= season.SeasonFinish.Date).ToList();
                        var subconFind = subconWhr.Where(x => x.Data_Date >= season.SeasonStart.Date && x.Data_Date <= season.SeasonFinish.Date).ToList();
                        var valuePercent = GetValuePPH(dataFind, subconFind);
                        values.Add(valuePercent);
                    }
                }

                var factoryData = new FactoryDataChart()
                {
                    Name = factory.Name,
                    Value = values
                };
                pPHData.Add(factoryData);
            }

            var result = new EfficiencyKanbanModel()
            {
                Title = chartTitle.Chart_Display_Name,
                Group = chartTitle.Item_Group_SortSeq,
                ShowValues = 0,
                ChartUnit = "",
                IsActive = true,
                Digits = 3,
                Labels = GetLabels(param),
                Data = pPHData
            };
            return result;
        }

        private EfficiencyKanbanModel GetDataModelEfficiency(List<EfficiencyDto> dataQuery, EffiencyKanbanParam param, DataChartTitle chartTitle)
        {
            var dataChart = new List<FactoryDataChart>();
            foreach (var factory in param.Factorys)
            {
                var values = new List<decimal>();
                var dataQueryInFactory = dataQuery.Where(x => x.Factory == factory.Id).ToList();

                if (param.Type == "month")
                {
                    foreach (var monthOfYearItem in param.Months)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Month == monthOfYearItem.Month && x.Year == monthOfYearItem.Year).ToList();
                        var valuePercent = GetValueModelEfficiency(dataFind, monthOfYearItem.Year >= 2023);
                        values.Add(valuePercent);
                    }
                }
                else if (param.Type == "year")
                {
                    foreach (var year in param.Years)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Year == year).ToList();
                        var valuePercent = GetValueModelEfficiency(dataFind, year >= 2023);
                        values.Add(valuePercent);
                    }
                }
                else if (param.Type == "week")
                {
                    foreach (var week in param.Weeks)
                    {
                        var dataFind = dataQueryInFactory.Where(x => (x.Data_Date >= week.WeekStart.Date && x.Data_Date <= week.WeekFinish.Date)).ToList();
                        var valuePercent = GetValueModelEfficiency(dataFind, week.WeekStart.Year >= 2023);
                        values.Add(valuePercent);
                    }
                }
                else
                {
                    foreach (var season in param.Seasons)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Data_Date >= season.SeasonStart.Date && x.Data_Date <= season.SeasonFinish.Date).ToList();
                        var valuePercent = GetValueModelEfficiency(dataFind, season.SeasonStart.Year >= 2023);
                        values.Add(valuePercent);
                    }
                }

                var factoryData = new FactoryDataChart()
                {
                    Name = factory.Name,
                    Value = values
                };
                dataChart.Add(factoryData);
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
                Data = dataChart
            };
            return result;
        }

        private EfficiencyKanbanModel GetDataAbnormalWorkingHourEfficiency(List<EfficiencyDto> dataQuery, EffiencyKanbanParam param, DataChartTitle chartTitle)
        {
            var dataChart = new List<FactoryDataChart>().ToList();
            foreach (var factory in param.Factorys)
            {
                var values = new List<decimal>();
                var dataQueryInFactory = dataQuery.Where(x => x.Factory == factory.Id).ToList();
                var def = dataQueryInFactory.ToList();

                if (param.Type == "month")
                {
                    foreach (var monthOfYearItem in param.Months)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Month == monthOfYearItem.Month && x.Year == monthOfYearItem.Year).ToList();
                        var valuePercent = GetValueAbnormalWorkingHourEfficiency(dataFind);
                        values.Add(valuePercent);
                    }
                }
                else if (param.Type == "year")
                {
                    foreach (var year in param.Years)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Year == year).ToList();
                        var valuePercent = GetValueAbnormalWorkingHourEfficiency(dataFind);
                        values.Add(valuePercent);
                    }
                }
                else if (param.Type == "week")
                {
                    foreach (var week in param.Weeks)
                    {
                        var dataFind = dataQueryInFactory.Where(x => (x.Data_Date >= week.WeekStart.Date && x.Data_Date <= week.WeekFinish.Date)).ToList();
                        var valuePercent = GetValueAbnormalWorkingHourEfficiency(dataFind);
                        values.Add(valuePercent);
                    }
                }
                else
                {
                    foreach (var season in param.Seasons)
                    {
                        var dataFind = dataQueryInFactory.Where(x => x.Data_Date >= season.SeasonStart.Date && x.Data_Date <= season.SeasonFinish.Date).ToList();
                        var valuePercent = GetValueAbnormalWorkingHourEfficiency(dataFind);
                        values.Add(valuePercent);
                    }
                }

                var factoryData = new FactoryDataChart()
                {
                    Name = factory.Name,
                    Value = values
                };
                dataChart.Add(factoryData);
            }

            var result = new EfficiencyKanbanModel()
            {
                Title = chartTitle.Chart_Display_Name,
                Group = chartTitle.Item_Group_SortSeq,
                ShowValues = Convert.ToInt32(param.Factorys.Count() == 1),
                ChartUnit = "%",
                IsActive = true,
                Digits = 2,
                Labels = GetLabels(param),
                Data = dataChart,
            };
            return result;
        }
        #endregion

        #region query to get data by all charts 
        // Target_Achievement
        private decimal GetValueTarget(List<EfficiencyDto> data)
        {
            if (data == null || data.Count == 0)
                return 0;
            var sumTarget = (decimal)data.Sum(x => x.Target_Qty);
            var valuePercent = (sumTarget != 0) ? ((decimal)(data.Sum(x => x.Actual_Qty) * 100) / sumTarget) : 0;
            return Math.Round((decimal)valuePercent, 1);
        }

        // Performance_Achievement
        private decimal GetValuePerformance(List<EfficiencyDto> data)
        {
            if (data == null || data.Count == 0)
                return 0;
            decimal targetAndImpact = (decimal)(data.Sum(x => x.Target_Qty) + data.Sum(x => x.Impact_Qty));
            var valuePercent = (targetAndImpact != 0) ? ((decimal)(data.Sum(x => x.Actual_Qty) * 100) / targetAndImpact) : 0;
            return Math.Round((decimal)valuePercent, 1);
        }

        // EOLR
        private decimal GetValueEOLR(List<EfficiencyDto> data)
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

        // [SQL 2]  Full Factory IE Achievement 
        private decimal GetValueIE(List<EfficiencyDto> data)
        {
            if (data == null || data.Count == 0)
                return 0;
            var sumHour1 = data.Sum(x => x.Hour_Tot) + data.Sum(x => x.Hour_In) - data.Sum(x => x.Hour_Out) - data.Sum(x => x.Hour_Learn) - data.Sum(x => x.Hour_Transfer);
            var valuePercent = (sumHour1 != null && sumHour1 != 0) ? ((data.Sum(x => x.Hour_IE) * 100) / sumHour1) : 0;
            return Math.Round((decimal)valuePercent, 1);
        }

        private decimal GetValueCTB(List<EfficiencyDto> data)
        {
            var sumHour_IE = data.Sum(x => x.Hour_IE ?? 0);
            var sumHour2 = data.Sum(x => x.Hour_Tot) + data.Sum(x => x.Hour_In) - data.Sum(x => x.Hour_Out) - data.Sum(x => x.Hour_Learn) - data.Sum(x => x.Hour_Transfer);
            var valuePercent = (sumHour2 != null && sumHour2 != 0) ? (sumHour_IE * 100 / sumHour2) : 0;
            return Math.Round((decimal)valuePercent, 1);
        }
        // private decimal GetValuePPH(List<EfficiencyDto> data1, List<EfficiencyDto> data2)
        // {
        //     var valuePercent = data2.Sum(x => x.Hour_Tot_All) != 0 ? (data1.Sum(x => x.Actual_Qty) / data2.Sum(x => x.Hour_Tot_All)) : 0;
        //     return Math.Round((decimal)valuePercent, 3);
        // }

        // [SQL 3]  PPH
        private decimal GetValuePPH(List<EfficiencyDto> data, List<eTM_HP_Efficiency_Data_Subcon> subconFind)
        {
            var valuePercent = data.Sum(x => x.Hour_Tot_All) != 0 ? (data.Sum(x => x.Kind == "5" ? x.Actual_Qty : 0) / (data.Sum(x => x.Hour_Tot_All) + subconFind.Sum(x => x.Subcon_Whrs))) : 0;
            return Math.Round((decimal)valuePercent, 3);
        }

        private static decimal GetValueModelEfficiency(List<EfficiencyDto> data, bool checkTime)
        {
            if (!checkTime)
            {
                var totalQty2 = data.Sum(x => x.Qty_2);
                var totalUnMan = data.Sum(x => x.Un_Man);
                var factoryId = data.Select(m => m.Factory).FirstOrDefault() ?? "";

                // 27/March - UT005 in TSH is not "In-Line" Process, no need to exclude the total labor hours of hot pressure process.
                var totalHour = (decimal?)null;
                // TSH
                if (factoryId == "U")
                {
                    totalHour =
                        data.Sum(x => x.Hour_Tot_2) -
                        data.Sum(x => x.Hour_OEM);
                }
                else
                {
                    totalHour =
                        data.Sum(x => x.Hour_Tot_2) -
                        data.Sum(x => x.Hour_UT005) -
                        data.Sum(x => x.Hour_OEM);
                }

                if (totalQty2.GetValueOrDefault() == 0 || totalUnMan.GetValueOrDefault() == 0 || totalHour.GetValueOrDefault() == 0)
                    return 0;

                var valuePercent = Math.Round((decimal)totalQty2 / (decimal)totalHour, 3) / (233 / Math.Round((decimal)totalUnMan / (decimal)totalQty2, 1)) * 100;
                return Math.Round((decimal)valuePercent, 3);
            }
            else
            {
                /* spec
                sum(Hour_Tot_CMT) / (
                    sum(Hour_Tot_2) - 
                    sum(iif(Hour_UT005 is null, 0, Hour_UT005)) - 
                    sum(iif(Hour_OEM is null, 0, Hour_OEM)) +
                    sum(iif(Hour_In - Hour_Out is null, 0, Hour_In - Hour_Out)) 
                ) Model_Eff,
                */
                var factoryId = data.Select(m => m.Factory).FirstOrDefault() ?? "";

                var totalCMT = data.Sum(x => x.Hour_Tot_CMT);
                var totalTot2 = data.Sum(x => x.Hour_Tot_2);
                var totalUT005 = data.Sum(x => x.Hour_UT005 ?? 0);
                var totalOEM = data.Sum(x => x.Hour_OEM ?? 0);
                var totalIn = data.Sum(x => x.Hour_In ?? 0);
                var totalOut = data.Sum(x => x.Hour_Out ?? 0);

                var denominator = totalTot2 - totalUT005 - totalOEM + (totalIn - totalOut);

                var valuePercent = denominator > 0 ? (totalCMT / denominator) * 100 : 0;

                return Math.Round((decimal)valuePercent, 3);
            }
        }

        private static decimal GetValueAbnormalWorkingHourEfficiency(List<EfficiencyDto> data)
        {
            var totalHour_Ex = Math.Round(data.Sum(x => x.Hour_Ex) ?? 0, 0);
            var factoryId = data.Select(m => m.Factory).FirstOrDefault() ?? "";
            var totalHour_Tot = Math.Round(data.Sum(x => x.Hour_Tot) ?? 0, 0);

            if (totalHour_Ex == 0 || totalHour_Tot == 0)
                return 0;

            return Math.Round(totalHour_Ex / totalHour_Tot * 100, 3);
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

            if (data[data.Count() - 1].WeekFinish > _maxDate && data[data.Count() - 1].WeekStart <= _maxDate)
            {
                data[data.Count() - 1].WeekFinish = _maxDate;
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


        #region area T5_External adidas
        //get Fake data charts Model Efficiency %

        private EfficiencyKanbanModel GetDataModelEfficiencyFakeExt(List<eTM_HP_Efficiency_Data_External> dataQueryInFactorys, EffiencyKanbanParam param, string chartUnit, int digits, DataChartTitle chartTitle)
        {
            List<FactoryDataChart> dataChart = new();
            List<string> labels = new();

            foreach (var factory in param.Factorys)
            {
                var values = new List<decimal>();

                var dataFind = dataQueryInFactorys
                    .Where(x => x.Date_Range_Type == GetDateRangeType(param.Type)).OrderBy(x => x.Sequence)
                    .ToList();

                labels = dataFind.Select(x => x.Date_Range_Label).ToList();

                values = GetFactoryValues(factory.Name, dataFind);

                var factoryData = new FactoryDataChart()
                {
                    Name = factory.Name,
                    Value = values,
                    ActualQty = null,
                };
                dataChart.Add(factoryData);
            }

            var result = new EfficiencyKanbanModel()
            {
                Title = chartTitle.Chart_Display_Name,
                Group = chartTitle.Item_Group_SortSeq,
                ShowValues = (param.Factorys.Count == 1) ? 1 : 0,
                ChartUnit = chartUnit,
                IsActive = true,
                Digits = digits,
                Labels = labels,
                Data = dataChart
            };
            return result;
        }

        private string GetDateRangeType(string type)
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

        #endregion
    }
}