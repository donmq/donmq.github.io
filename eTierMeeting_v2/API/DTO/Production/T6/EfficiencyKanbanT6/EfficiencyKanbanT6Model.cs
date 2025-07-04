using System;
using System.Collections.Generic;
using eTierV2_API.Models;

namespace eTierV2_API.DTO.Production.T6.EfficiencyKanban
{
    public class EfficiencyKanbanModel
    {
        public string Title { get; set; }
        public string Group { get; set; }
        public int ShowValues { get; set; }
        public string ChartUnit { get; set; }
        public bool IsActive { get; set; }
        public int Digits { get; set; }
        public List<string> Labels { get; set; }
        public List<FactoryDataChart> Data { get; set; }
    }
    public class DataGrouped
    {
        public string Group { get; set; }
        public List<EfficiencyKanbanModel> Data_By_Groups { get; set; }
    }

    public class FactoryDataChart
    {
        public string Name { get; set; }
        public List<decimal> Value { get; set; }
        public List<int?> ActualQty { get; set; }
    }

    public class EfficiencyByBrandDto
    {
        public string Factory { get; set; }
        public string Dept_ID { get; set; }
        public string Kind { get; set; }
        public int? Actual_Qty { get; set; }
        public int? Target_Qty { get; set; }
        public int? Impact_Qty { get; set; }
        public decimal? Hour_Base { get; set; }
        public decimal? Hour_Overtime { get; set; }
        public decimal? Hour_IE { get; set; }
        public decimal? Hour_Tot { get; set; }
        public decimal? Hour_Learn { get; set; }
        public decimal? Hour_Transfer { get; set; }
        public decimal? Hour_In { get; set; }
        public decimal? Hour_Out { get; set; }
        public string Line_No { get; set; }
        public DateTime? Data_Date { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Factory_ID { get; set; }
        public DateTime? Update_Time { get; set; }
    }
    public class FactoryInfo
    {
        public string Stt { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }

    public class MonthOfYear
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class EffiencyKanbanParam
    {
        public bool Is_T5_External { get; set; }
        public string Type { get; set; }
        public List<FactoryInfo> Factorys { get; set; }
        public string Brand { get; set; }
        public List<MonthOfYear> Months { get; set; }
        public List<int> Years { get; set; }
        public List<Week> Weeks { get; set; }
        public List<Season> Seasons { get; set; } 
        public List<DataChartTitle> ChartTitles { get; set; }
    }

    public class Week
    {
        public string Name { get; set; }
        public DateTime WeekStart { get; set; }
        public DateTime WeekFinish { get; set; }
    }

    public class Season
    {
        public string Name { get; set; }
        public DateTime SeasonStart { get; set; }
        public DateTime SeasonFinish { get; set; }
    }

    public class DataChartTitle
    {
        public string Item_ID { get; set; }
        public string Chart_Display_Name { get; set; }
        public string Item_Group_SortSeq { get; set; }
        public string Item_SortSeq { get; set; }
    }
}