namespace API.Dtos.Manage.GroupBaseManage
{
    public class GroupBaseAndGroupLangDto
    {
        public int GBID { get; set; }
        public string GBVN { get; set; }
        public string GBEN { get; set; }
        public string GBTW { get; set; }
        public string BaseName { get; set; }
        public string BaseSym { get; set; }
    }
    public class GroupBaseTitleExcel
    {
        public string Label_BaseName { get; set; }
        public string Label_BaseSym { get; set; }
    }
}