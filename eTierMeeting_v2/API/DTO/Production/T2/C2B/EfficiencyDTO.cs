using System;
using System.Collections.Generic;

namespace eTierV2_API.DTO.Production.T2.C2B
{
    public class EfficiencyChart
    {
        public string DataDate { get; set; }
        public string FirstDayOfMonth { get; set; }
        public List<ChartData> TargetAchievement { get; set; }
        public List<ChartData> PerformanceAchievement { get; set; }
        public List<ChartData> IEAchievement { get; set; }
        public List<ChartData> IEAchievement_STI { get; set; }
        public List<ChartData> EOLR { get; set; }
        public List<ChartData> EOLR_STI { get; set; }
        public List<ChartData> Abnormal_Working_Hours { get; set; }
    }


    public class ChartData
    {
        public string Title { get; set; }
        public string Title_LL { get; set; }
        public string Line { get; set; }
        public decimal DataLine { get; set; }
        public decimal Target { get; set; }
        public string Color { get; set; }
    }

    public class EfficiencyDTO
    {
        public string Dept_ID { get; set; }
        public DateTime Data_Date { get; set; }
        public string Current_Date { get; set; }
        public string Line_Sname { get; set; }

        public float TargetAchievement { get; set; }
        public float PerformanceAchievement { get; set; }
        public float IEAchievement { get; set; }
        public float EOLR { get; set; }
        public float TargetAchievement_Target { get; set; }
        public float PerformanceAchievement_Target { get; set; }
        public float IEAchievement_Target { get; set; }
        public float EOLR_Target { get; set; }
    }
}