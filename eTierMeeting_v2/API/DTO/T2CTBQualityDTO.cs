
using System;
using System.Collections.Generic;

namespace eTierV2_API.DTO
{
    public class T2CTBQualityDTO
    {
        public DateTime Data_Date { get; set; }
        public decimal? RFT_Target { get; set; }
        public decimal? BA_Target { get; set; }
        public List<RFTChart> RFT_Chart { get; set; }
        public List<BAChart> BA_Chart { get; set; }
    }

    public class RFTChart
    {
        public string Line_Sname { get; set; }
        public decimal? RFT { get; set; }
    }

    public class BAChart
    {
        public string Line_Sname { get; set; }
        public decimal? BA { get; set; }
    }
}