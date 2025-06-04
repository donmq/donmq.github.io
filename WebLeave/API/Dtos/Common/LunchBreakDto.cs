namespace API.Dtos.Common
{
    public class LunchBreakDto
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public object WorkTimeStart { get; set; } // TimeSpan to string or DateTime
        public object WorkTimeEnd { get; set; } // TimeSpan to string or DateTime
        public object LunchTimeStart { get; set; } // TimeSpan to string or DateTime
        public object LunchTimeEnd { get; set; } // TimeSpan to string or DateTime
        public string Value_vi { get; set; }
        public string Value_en { get; set; }
        public string Value_zh { get; set; }
        public int? Seq { get; set; }
        public bool? Visible { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }

    public class WorkShiftDto
    {
        public string Key { get; set; }
        public TimeDto WorkTimeStart { get; set; }
        public TimeDto WorkTimeEnd { get; set; }
        public TimeDto LunchTimeStart { get; set; }
        public TimeDto LunchTimeEnd { get; set; }
    }

    public class TimeDto
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
    }
}