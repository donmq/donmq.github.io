namespace API.DTOs.BasicMaintenance
{
    public class Code_LanguageDto
    {
        public string Type_Seq { get; set; }
        public string Type_Name { get; set; }
        public string Code { get; set; }
        public string Code_Name { get; set; }
        public string Update_By { get; set; }
        public string State { get; set; }
        public DateTime? Update_Time { get; set; }
    }
    public class Code_LanguageDetail
    {
        public string Type_Seq { get; set; }
        public string Type_Title { get; set; }
        public string Code { get; set; }
        public string Code_Name { get; set; }
        public List<Code_Language_Form> Detail { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Time { get; set; }
    }

    public class Code_LanguageParam
    {
        public string Type_Seq { get; set; }
        public string Type_Name { get; set; }
        public string Code { get; set; }
        public string Code_Name { get; set; }
    }

    public class Code_Language_Form
    {
        public string Language_Code { get; set; }
        public string Name { get; set; }
    }
}