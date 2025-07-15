namespace Machine_API.DTO
{
    public class Cell_PlnoDto
    {
        public int ID { get; set; }
        public string Plno { get; set; }
        public string Place { get; set; }
        public int? CellID { get; set; }
        public string CellCode { get; set; }
        public string CellName { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}