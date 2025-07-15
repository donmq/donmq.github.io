namespace Machine_API.Helpers.Params
{
    public class ReportHistoryInventoryParams
    {
        public string PlnoId { get; set; }
        public string TimeSokiem { get; set; }
        public string TimeRutKiem { get; set; }
        public string TimePhucKiem { get; set; }
        public string TypeFile { get; set; }
        public string Lang { get; set; }
    }

    public class ReportKiemKeParam
    {
        public string Lang { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}