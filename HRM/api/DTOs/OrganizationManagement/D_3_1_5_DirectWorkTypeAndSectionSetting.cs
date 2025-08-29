namespace API.DTOs.OrganizationManagement
{
    public class DirectWorkTypeAndSectionSettingParam
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Effective_Date { get; set; }
        public string Work_Type_Code { get; set; }
        public string Section_Code { get; set; }
        public string Lang { get; set; }
        public string Direct_Section { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
    }
    public class HRMS_Org_Direct_SectionDto
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Effective_Date { get; set; }
        public string Section_Code_Name { get; set; }
        public string Work_Type_Code { get; set; }
        public string Work_Type_Code_Name { get; set; }
        public string Section_Code { get; set; }
        public string Direct_Section { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }

    }

    public class BasicCodeLanguageView
    {
        public string Name { get; set; }
        public string Code { get; set; }

    }
}