

namespace API.Dtos.Manage.PositionManage
{
    public partial class PositionManageDto
    {
        public int PositionID { get; set; }
        public string PositionName { get; set; }
        public string PositionSym { get; set; }
        // 
        public string PositionVN { get; set; }
        public string PositionEN { get; set; }
        public string PositionTW { get; set; }
        //
        public string PositionNameExcel { get; set; }
        public string PositionSymExcel { get; set; }
    }
}