
namespace API.DTOs.EmployeeMaintenance
{
    public class EmployeeGroupSkillSettings_Param
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Employee_Id { get; set; }
        public string Production_Line { get; set; }
        public string Performance_Category { get; set; }
        public string Technical_Type { get; set; }
        public string Expertise_Category { get; set; }
        public List<string> Skill_Array { get; set; }
        public string Lang { get; set; }
        
    }
    public class EmployeeGroupSkillSettings_Main
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Employee_Id { get; set; }
        public string Local_Full_Name { get; set; }
        public string Production_Line { get; set; }
        public string Production_Line_Name { get; set; }
        public string Performance_Category { get; set; }
        public string Performance_Category_Name { get; set; }
        public string Technical_Type { get; set; }
        public string Technical_Type_Name { get; set; }
        public string Expertise_Category { get; set; }
        public string Expertise_Category_Name { get; set; }
        public List<EmployeeGroupSkillSettings_SkillDetail> Skill_Detail_List { get; set; }

    }
    public class EmployeeGroupSkillSettings_SkillDetail
    {
        public string Seq { get; set; }
        public string Skill_Certification { get; set; }
        public DateTime Passing_Date { get; set; }
        public string Passing_Date_Str { get; set; }
    }
}
