namespace API.Helpers.Params.ReportShow
{
    public class ReportShowParam
    {
        public int Index { get; set; }
        public int? Id { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string Lang { get; set; }
    }
}