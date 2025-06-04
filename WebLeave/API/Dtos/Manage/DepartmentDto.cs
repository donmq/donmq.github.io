
using API.Helpers.Params;

namespace API.Dtos
{
    public class DepartmentDto : DepartmentLangParam
    {
        public int DeptID { get; set; }
        public string DeptName { get; set; }
        public int? AreaID { get; set; }
        public int? BuildingID { get; set; }
        public string DeptCode { get; set; }
        public string DeptSym { get; set; }
        public int? Number { get; set; }
        public int? Shift_Time { get; set; }
        public bool? Visible { get; set; }
        public string BuildingName { get; set; }
        public string AreaName { get; set; }
    }
}