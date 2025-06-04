
using API.Dtos.SeaHr.SeaHrHistory;
using API.Helpers.Utilities;
namespace API.Dtos.SeaHr
{
    public class SeaHistorySearchDto
    {
        public PaginationUtility<LeaveDataDto> LeaveData { get; set; }
        public List<KeyValueUtility> CountEachCategory { get; set; }
    }
}