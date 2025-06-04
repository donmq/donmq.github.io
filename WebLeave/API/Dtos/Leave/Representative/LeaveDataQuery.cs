using API.Models;

namespace API.Dtos.Leave.Representative
{
    public class LeaveDataQuery
    {
        public LeaveData Leave { get; set; }
        public Employee Employee { get; set; }
        public Area Area { get; set; }
        public Building Building { get; set; }
        public Department Department { get; set; }
        public Part Part { get; set; }
    }
}