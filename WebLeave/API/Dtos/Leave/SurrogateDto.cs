namespace API.Dtos.Leave
{
    public class SurrogateDto
    {
        public int UserID { get; set; }
        public int? EmpID { get; set; }
        public string FullName { get; set; }
        public string EmpNumber { get; set; }
        public int SurrogateId { get; set; }
    }

    public class SurrogateParam
    {
        public int UserID { get; set; }
        public int PartID { get; set; }
        public string Keyword { get; set; }
    }

    public class SurrogateRemoveDto
    {
        public int UserID { get; set; }
        public int SurrogateId { get; set; }
    }
}