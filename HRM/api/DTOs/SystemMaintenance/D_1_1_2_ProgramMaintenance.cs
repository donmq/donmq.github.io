namespace API.DTOs.SystemMaintenance
{
    public class ProgramMaintenance_Data
    {
        public int? Seq { get; set; }
        public string Program_Code { get; set; }
        public string Program_Name { get; set; }
        public string Parent_Directory_Code { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Time { get; set; }
        public List<string> Functions { get; set; }
    }

    public class ProgramMaintenance_Param
    {
        public string Program_Code { get; set; }
        public string Program_Name { get; set; }
        public string Parent_Directory_Code { get; set; }
    }
}