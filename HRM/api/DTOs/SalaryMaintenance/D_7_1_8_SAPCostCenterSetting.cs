namespace API.DTOs.SalaryMaintenance;

public class D_7_8_SAPCostCenterSettingDto
{
    public string Factory { get; set; }
    public string Company_Code { get; set; }
    public string Year { get; set; } // cost_year
    public string Group { get; set; }
    public string Cost_Code { get; set; } // CostCenter
    public string Kind { get; set; } // Function
    public string Kind_Name { get; set; } // show main and excel
    public string Profit_Center { get; set; } //Profit_Center code
    public string Code_Name { get; set; } // CostCenterChineseDescription
    public string Code_Name_EN { get; set; } // CostCenterEnglishDescription
    public string Update_By { get; set; }
    public string Update_Time { get; set; }
}

public class D_7_8_SAPCostCenterSettingReportDto : D_7_8_SAPCostCenterSettingDto
{
    public string Error_Message { get; set; }
}

public class D_7_8_SAPCostCenterSettingParam
{
    public string Factory { get; set; }
    public string Company_Code { get; set; }
    public string CostYear { get; set; }
    public string Language { get; set; }
}

public class D_7_8_CheckDuplicateParam
{
    public string Factory { get; set; }
    public string Cost_Year { get; set; }
    public string Company_Code { get; set; }
    public string Cost_Code { get; set; }
}

public class D_7_8_DeleteParam : D_7_8_CheckDuplicateParam
{
}