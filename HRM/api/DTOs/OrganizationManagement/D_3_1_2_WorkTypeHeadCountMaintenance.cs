namespace API.DTOs.OrganizationManagement
{
    public class HRMS_Org_Work_Type_HeadcountDto
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
        public string Effective_Date { get; set; }
        public string Work_Type_Code { get; set; }
        public string Work_Type_Name { get; set; }
        public int Approved_Headcount { get; set; }
        public int Actual_Headcount { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
    }

    public class DepartmentNameObject
    {
        public string Department_Code { get; set; }
        public string Department_Name { get; set; }
    }

    public class HRMS_Org_Work_Type_HeadcountDataMain
    {
        public int TotalApprovedHeadcount { get; set; }
        public int TotalActual { get; set; }
        public PaginationUtility<HRMS_Org_Work_Type_HeadcountDto> DataPagination { get; set; }
    }


    public class HRMS_Org_Work_Type_HeadcountUpdate
    {
        public List<HRMS_Org_Work_Type_HeadcountDto> DataUpdate { get; set; }
        public List<HRMS_Org_Work_Type_HeadcountDto> DataNewAdd { get; set; }
    }

    public class HRMS_Org_Work_Type_HeadcountParam
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Department_Code { get; set; }
        public string Effective_Date { get; set; }
        public string Language { get; set; }
    }
}