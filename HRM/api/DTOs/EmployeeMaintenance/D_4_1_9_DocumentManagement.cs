
namespace API.DTOs.EmployeeMaintenance
{
    public class DocumentManagement_MainParam
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Employee_Id { get; set; }
        public string Local_Full_Name { get; set; }
        public string Document_Type { get; set; }
        public string Validity_Date_Start_From { get; set; }
        public string Validity_Date_Start_To { get; set; }
        public string Validity_Date_Start_From_Str { get; set; }
        public string Validity_Date_Start_To_Str { get; set; }
        public string Validity_Date_End_From { get; set; }
        public string Validity_Date_End_To { get; set; }
        public string Validity_Date_End_From_Str { get; set; }
        public string Validity_Date_End_To_Str { get; set; }
        public string Lang { get; set; }


    }
    public class DocumentManagement_MainData
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Employee_Id { get; set; }
        public string Local_Full_Name { get; set; }
        public string Document_Type { get; set; }
        public string Document_Type_Name { get; set; }
        public string Passport_Full_Name { get; set; }
        public int Seq { get; set; }
        public string Document_Number { get; set; }
        public DateTime Validity_Date_From { get; set; }
        public string Validity_Date_From_Str { get; set; }
        public DateTime Validity_Date_To { get; set; }
        public string Validity_Date_To_Str { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public List<DocumentManagement_FileModel> File_List { get; set; }
        public string Ser_Num { get; set; }
    }
    public class DocumentManagement_SubParam
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Employee_Id { get; set; }
        public string Local_Full_Name { get; set; }
    }
    public class DocumentManagement_SubData
    {
        public string Document_Type { get; set; }
        public string Passport_Full_Name { get; set; }
        public int Seq { get; set; }
        public string Document_Number { get; set; }
        public DateTime Validity_Date_From { get; set; }
        public string Validity_Date_From_Str { get; set; }
        public DateTime Validity_Date_To { get; set; }
        public string Validity_Date_To_Str { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Update_Time_Str { get; set; }
        public List<DocumentManagement_FileModel> File_List { get; set; }
        public string Ser_Num { get; set; }
    }
    public class DocumentManagement_SubMemory
    {
        public DocumentManagement_SubParam Param { get; set; }
        public List<DocumentManagement_SubData> Data { get; set; }
    }
    public class DocumentManagement_FileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int Size { get; set; }
    }
    public class DocumentManagement_DownloadFileModel
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Ser_Num { get; set; }
        public string Employee_Id { get; set; }
        public string File_Name { get; set; }
    }
    public class DocumentManagement_SubModel
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Employee_Id { get; set; }
        public string Document_Type { get; set; }
        public int Seq { get; set; }
    }
    public class DocumentManagement_TypeheadKeyValue
    {
        public string Key { get; set; }
        public string UseR_GUID { get; set; }
        public string Local_Full_Name { get; set; }
        public int Max_Seq { get; set; }
    }
}