namespace Machine_API.DTO
{
    public class EmployAdminDto
    {
        public int ID { get; set; }
        public string EmpName { get; set; }
        public string EmpNumber { get; set; }
        public bool? Visible { get; set; }
        public int? CellID { get; set; }
        public List<PlnoEmployDto> ListPlnoEmploy { get; set; }
    }

    public class PlnoEmployDto
    {
        public string Name { get; set; }
        public string PlnoID { get; set; }
        public string Place { get; set; }
    }
}