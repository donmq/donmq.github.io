
namespace API.DTOs.EmployeeMaintenance
{
    public class HRMS_Emp_EducationalDto
    {
        public string USER_GUID { get; set; }

        public string Nationality { get; set; }
        public string Identification_Number { get; set; }
        public string Local_Full_Name { get; set; }


        public string Degree { get; set; }
        public string DegreeName { get; set; }

        public string Academic_System { get; set; }
        public string Academic_SystemName { get; set; }

        public string Major { get; set; }
        public string MajorName { get; set; }

        public string School { get; set; }

        public string Department { get; set; }

        public string Period_Start { get; set; }

        public string Period_End { get; set; }

        public bool Graduation { get; set; }

        public string Update_By { get; set; }

        public DateTime Update_Time { get; set; }

    }

    public class HRMS_Emp_Educational_FileUpload
    {
        public string USER_GUID { get; set; }

        public string SerNum { get; set; }

        public int FileID { get; set; }

        public string FileName { get; set; }

        public double FileSize { get; set; }

        public string Update_By { get; set; }

        public DateTime Update_Time { get; set; }
    }

    public class HRMS_Emp_EducationalDownload
    {
        public string FileName { get; set; }
        public string File { get; set; }
    }

    public class HRMS_Emp_EducationalParam
    {
        public string USER_GUID { get; set; }
        public string Language { get; set; }
    }

    public class HRMS_Emp_Educational_QueryData
    {
        public string Type_Seq { get; set; }
        public string Code { get; set; }
        public string Code_Name { get; set; }
        public string Code_Lang { get; set; }
    }

    public class EducationUpload
    {
        public string USER_GUID { get; set; }
        public List<EducationFile> Files { get; set; }
        public string UpdateBy { get; set; }
    }

    public class EducationFile
    {
        public string USER_GUID { get; set; }
        public string SerNum { get; set; }
        public int FileID { get; set; }


        public string FileName { get; set; }
        public double FileSize { get; set; }
        public IFormFile File { get; set; }
    }
}