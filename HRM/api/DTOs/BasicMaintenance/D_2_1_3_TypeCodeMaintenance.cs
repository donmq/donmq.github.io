namespace API.DTOs.BasicMaintenance
{
    public class HRMS_Basic_Code_TypeDto
    {
        public string Type_Seq { get; set; }
        public string Type_Name { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Time { get; set; }

    }
    public class HRMS_Basic_Code_TypeParam
    {
        public string Type_Seq { get; set; }
        public string Type_Name { get; set; }
    }
    public class Language_Dto
    {
        public string type_Seq { get; set; }
        public List<LanguageDetail_Dto> Detail_Dto { get; set; }
        public string userName { get; set; }
    }
    public class LanguageDetail_Dto
    {
        public string Language_Code { get; set; }
        public string Type_Name { get; set; }
    }
    public class HRMS_Basic_Code_Type_LanguageInfoDto
    {
        public bool isInt { get; set; }
        public int Seq { get; set; }
        public string Type_Seq { get; set; }
        public string Type_Name { get; set; }
        public List<Info> Info { get; set; }
    }
    public class Info
    {
        public string Language_Code { get; set; }
        public string Type_Name { get; set; }
    }
}