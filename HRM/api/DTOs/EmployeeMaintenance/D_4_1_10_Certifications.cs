
namespace API.DTOs.EmployeeMaintenance
{
    public class Certifications_MainParam
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Employee_Id { get; set; }
        public string Local_Full_Name { get; set; }
        public string Category_Of_Certification { get; set; }
        public string Passing_Date_From { get; set; }
        public string Passing_Date_To { get; set; }
        public string Passing_Date_From_Str { get; set; }
        public string Passing_Date_To_Str { get; set; }
        public string Certification_Valid_Period_From { get; set; }
        public string Certification_Valid_Period_To { get; set; }
        public string Certification_Valid_Period_From_Str { get; set; }
        public string Certification_Valid_Period_To_Str { get; set; }
        public string Lang { get; set; }
        

    }
    public class Certifications_MainData
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Employee_Id { get; set; }
        public string Local_Full_Name { get; set; }
        public int Seq { get; set; }
        public string Category_Of_Certification { get; set; }
        public string Name_Of_Certification { get; set; }
        public string Score { get; set; }
        public string Level { get; set; }
        public bool Result { get; set; }
        public DateTime Passing_Date { get; set; }
        public string Passing_Date_Str { get; set; }
        public DateTime? Certification_Valid_Period { get; set; }
        public string Certification_Valid_Period_Str { get; set; }
        public List<Certifications_FileModel> File_List { get; set; }
        public string Remark { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Ser_Num { get; set; }
        public string Department { get; set; }
        public string Department_Name { get; set; }
        public string Result_Str { get; set; }
    }
    public class Certifications_SubParam
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Employee_Id { get; set; }
        public string Local_Full_Name { get; set; }
    }
    public class Certifications_SubData
    {
        public int Seq { get; set; }
        public string Category_Of_Certification { get; set; }
        public string Name_Of_Certification { get; set; }
        public string Score { get; set; }
        public string Level { get; set; }
        public bool Result { get; set; }
        public DateTime Passing_Date { get; set; }
        public string Passing_Date_Str { get; set; }
        public DateTime? Certification_Valid_Period { get; set; }
        public string Certification_Valid_Period_Str { get; set; }
        public List<Certifications_FileModel> File_List { get; set; }
        public string Remark { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Update_Time_Str { get; set; }
        public string Ser_Num { get; set; }
    }
    public class Certifications_SubMemory
    {
        public Certifications_SubParam Param { get; set; }
        public List<Certifications_SubData> Data { get; set; }
    }
    public class Certifications_FileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int Size { get; set; }
    }
    public class Certifications_DownloadFileModel
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Ser_Num { get; set; }
        public string Employee_Id { get; set; }
        public string File_Name { get; set; }
    }
    public class Certifications_SubModel
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Employee_Id { get; set; }
        public int Seq { get; set; }
    }

    public class Certifications_TypeheadKeyValue
    {
        public string Key { get; set; }
        public string UseR_GUID { get; set; }
        public string Local_Full_Name { get; set; }
        public int Max_Seq { get; set; }
    }
}