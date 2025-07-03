using System.Collections.Generic;

namespace eTierV2_API.DTO.Production.T5.EfficiencyKanban
{
    public class EfficiencyExtKanbanModel
    {
        public string Title { get; set; }
        public int ShowValues { get; set; }
        public string ChartUnit { get; set; }
        public bool IsActive { get; set; }
        public int Digits { get; set; }
        public List<string> Labels { get; set; }
        public List<FactoryDataChartExt> Data { get; set; }
    }

    public class FactoryDataChartExt
    {
        public string Name { get; set; }
        public List<decimal?> Value { get; set; }
        public List<int?> ActualQty { get; set; }
    }
}