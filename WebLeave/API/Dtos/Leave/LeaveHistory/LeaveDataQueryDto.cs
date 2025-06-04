using API.Models;

namespace API.Dtos.Leave.LeaveHistory
{
    public class LeaveDataQueryDto
    {
        public LeaveData LeaveData { get; set; }
        public Employee Employee { get; set; }
        public Category Category { get; set; }
        public Part Part { get; set; }
        public Building Building { get; set; }
        public Department Department { get; set; }
        public Area Area { get; set; }
    }
}