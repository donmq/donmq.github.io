using API.Dtos.Common;
using API.Helpers.Utilities;
namespace API.Dtos.SeaHr
{
    public class SeaConfirmSearchDto
    {
        public PaginationUtility<LeaveDataDto> LeaveData { get; set; }
        public List<KeyValueUtility> CountEachCategory { get; set; }
    }
}