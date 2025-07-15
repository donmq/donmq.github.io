namespace Machine_API.DTO
{
    public class InventoryLineDto
    {
        public string PlnoName { get; set; }
        public string PlnoId { get; set; }
        public DateTime? TimeSoKiem { get; set; }
        public DateTime? TimePhucKiem { get; set; }
        public DateTime? TimeRutKiem { get; set; }
        public string PecenMatchPhucKiem { get; set; }
        public string PecenMatchSoKiem { get; set; }
        public string PecenMatchRutKiem { get; set; }
    }
}