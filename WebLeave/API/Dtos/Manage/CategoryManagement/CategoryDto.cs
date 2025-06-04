namespace API.Dtos.Manage.CategoryManagement
{
    public class CategoryDto
    {
        public int CateID { get; set; }
        public string CateName { get; set; }
        public string CateSym { get; set; }
        public bool? Visible { get; set; }
    }
}