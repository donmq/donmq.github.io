namespace API.DTOs.OrganizationManagement
{
    public class Org_Direct_DepartmentResult
    {

        public string Division { get; set; }
        public string Factory { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Line_Code { get; set; }
        public string Line_Name { get; set; }
        public string Direct_Department_Attribute { get; set; }
        public string Direct_Department_Attribute_Name { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
    }
    public class Org_Direct_DepartmentParam
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Department_Code { get; set; }
        public string Lang { get; set; }

    }
    public class Org_Direct_DepartmentParamQuery
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Department_Code { get; set; }
        public string Line_Code { get; set; }
        public string Direct_Department_Attribute { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
    }
}