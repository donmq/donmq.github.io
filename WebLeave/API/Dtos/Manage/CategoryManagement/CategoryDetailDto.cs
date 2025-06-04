namespace API.Dtos.Manage.CategoryManagement
{
    public class CategoryDetailDto
    {
        public int CateID { get; set; }
        public string CateNameVN { get; set; }
        public string CateNameEN { get; set; }
        public string CateNameTW { get; set; }
        public string CateSym { get; set; }
        public bool? Visible { get; set; }
    }
}