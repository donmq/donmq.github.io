namespace Machine_API.DTO
{
    public class EmployeeDto
    {
        public int ID { get; set; }
        public string EmpName { get; set; }
        public string EmpNumber { get; set; }
        public bool? Visible { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public List<PlnoEmployDto>  ListPlnoEmploy { get; set; }
    }
}