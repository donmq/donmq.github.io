namespace API.Dtos.Leave.LeaveHistory
{
    public class LeaveDataDtos
    {
        public PaginationUtility<Dtos.Common.LeaveDataDto> LeaveData { get; set; }
        public double? SumLeaveDay { get; set; }
    }
}