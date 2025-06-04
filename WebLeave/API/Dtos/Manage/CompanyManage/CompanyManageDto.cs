

namespace API.Dtos.Manage.CompanyManage
{
    public partial class CompanyManageDto
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyInfo { get; set; }
        public string CompanySym { get; set; }
        public int? Number { get; set; }
        public bool? Visible { get; set; }
    }
}