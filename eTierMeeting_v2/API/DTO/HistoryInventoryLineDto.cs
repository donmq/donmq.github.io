namespace Machine_API.DTO
{
    public class HistoryInventoryLineDto
    {
        public int HistoryInventoryId { get; set; }
        public string PlnoId { get; set; }
        public string Place { get; set; }
        public DateTime? TimeSoKiem { get; set; }
        public DateTime? TimePhucKiem { get; set; }
        public DateTime? TimeRutKiem { get; set; }


        public string PecenMatchPhucKiem { get; set; }
        public string PecenMatchSoKiem { get; set; }
        public string PecenMatchRutKiem { get; set; }
    }
}