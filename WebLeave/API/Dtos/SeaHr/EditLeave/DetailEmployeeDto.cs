using API.Dtos.Common;

namespace API.Dtos.SeaHr.EditLeave
{
    public class DetailEmployeeDto
    {
        public HistoryEmpDto Employee { get; set; }
        public List<LeaveDataDto> ListLeave { get; set; }
    }
}
