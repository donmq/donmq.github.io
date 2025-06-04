
namespace API.Dtos.Common
{
    public class AreaDto
    {
        public int AreaID { get; set; }
        public string AreaName { get; set; }
        public string AreaSym { get; set; }
        public string AreaCode { get; set; }
        public int? CompanyID { get; set; }
        public int? Number { get; set; }
        public bool? Visible { get; set; }
    }

    public class AreaInformation
    {
        public int AreaID { get; set; }
        public string AreaName { get; set; }
        public string AreaNameVi { get; set; }
        public string AreaNameEn { get; set; }
        public string AreaNameZh { get; set; }
        public string AreaSym { get; set; }
        public string AreaCode { get; set; }
        public int CompanyID { get; set; }
        public string Company { get; set; }
        public int? Number { get; set; }
        public bool? Visible { get; set; }

    }
}