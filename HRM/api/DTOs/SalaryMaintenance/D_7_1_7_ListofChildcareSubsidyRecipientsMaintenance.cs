namespace API.DTOs.SalaryMaintenance;

public class D_7_7_HRMS_Sal_Childcare_SubsidyDto
{
    public int Seq { get; set; }
    public string USER_GUID { get; set; }
    public string Factory { get; set; }
    public string Department_Code { get; set; }
    public string Department_Name { get; set; }
    public string Department_Code_Name { get; set; }
    public string Employee_ID { get; set; }
    public string Employee_ID_Old { get; set; }
    public string Local_Full_Name { get; set; }
    public DateTime Birthday_Child { get; set; }
    public DateTime Month_Start { get; set; }
    public DateTime Month_End { get; set; }
    public short Num_Children { get; set; }
    public string Update_By { get; set; }
    public string Update_Time { get; set; }
}

public class D_7_7_HRMS_Sal_Childcare_SubsidyReportDto
{
    public int Seq { get; set; }
    public string USER_GUID { get; set; }
    public string Factory { get; set; }
    public string Department_Full { get; set; }
    public string Department_Code { get; set; }
    public string Department_Name { get; set; }
    public string Department_Code_Name { get; set; }
    public string Employee_ID { get; set; }
    public string Local_Full_Name { get; set; }
    public string Birthday_Child { get; set; }
    public string Month_Start { get; set; }
    public string Month_End { get; set; }
    public short? Num_Children_Report { get; set; }
    public string Error_Message { get; set; }
}

public class D_7_7_HRMS_Sal_Childcare_SubsidyParam
{
    public string Factory { get; set; }
    public string Employee_ID { get; set; }
    public string Language { get; set; }
}

public class D_7_7_CheckDuplicateParam
{
    public string Factory { get; set; }
    public string Employee_ID { get; set; }
    public DateTime Birthday_Child { get; set; }
}
public class ListofChildcareSubsidyRecipientsMaintenance_EmployeeDataChange
{
    public string USER_GUID { get; set; }
    public string Local_Full_Name { get; set; }
    public string Department_Code { get; set; }
    public string Department_Code_Name { get; set; }
}
public class D_7_7_DeleteParam : D_7_7_CheckDuplicateParam
{
}