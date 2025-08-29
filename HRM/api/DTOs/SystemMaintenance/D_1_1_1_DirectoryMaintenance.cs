namespace API.DTOs.SystemMaintenance
{
    public class DirectoryMaintenance_Data
    {
        public string Seq { get; set; }
        public string Directory_Code { get; set; }
        public string Directory_Name { get; set; }
        public string Parent_Directory_Code { get; set; }
        public string Language { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Time { get; set; }

    }
    public class DirectoryMaintenance_Param
    {
        public string Directory_Code { get; set; }
        public string Directory_Name { get; set; }
        public string Parent_Directory_Code { get; set; }
    }
}