using API.Dtos.Common;

namespace API.Dtos.SeaHr
{
    public class SeaConfirmEmpDetailDto
    {
        public List<LeaveDataDto> LeaveData { get; set; }
        public HistoryEmpDto HistoryEmp { get; set; }
    }
}