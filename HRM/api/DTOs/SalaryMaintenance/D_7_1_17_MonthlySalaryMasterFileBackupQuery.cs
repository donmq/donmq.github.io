using API.Models;

namespace API.DTOs.SalaryMaintenance
{
    public class D_7_17_MonthlySalaryMasterFileBackupQueryDto
    {
        public int Seq { get; set; }
        public string USER_GUID { get; set; }
        public string Probation { get; set; }
        public string YearMonth { get; set; }
        public string Factory { get; set; }
        public string Department { get; set; }
        public string Employee_ID { get; set; }
        public string Local_Full_Name { get; set; }
        public string Employment_Status { get; set; }
        public string Position_Grade { get; set; }
        public string Position_Title { get; set; }
        public string ActingPosition_Start { get; set; }
        public string ActingPosition_End { get; set; }
        public string Technical_Type { get; set; }
        public string Expertise_Category { get; set; }
        public string Onboard_Date { get; set; }
        public string Permission_Group { get; set; }
        public string Salary_Type { get; set; }
        public string Salary_Grade { get; set; }
        public string Salary_Level { get; set; }
        public string Currency { get; set; }
        public string Total_Salary { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
    }

    public class CodeNameDto
    {
        public string Code { get; set; }
        public string Code_Name { get; set; }
        public string Type_Seq { get; set; }

    }

    public class SalaryDetailDto
    {
        public string Salary_Item { get; set; }
        public decimal Amount { get; set; }
    }

    public class MonthlySalaryMasterFileBackupQuery_SalaryItem
    {
        public string Probation { get; set; }
        public string Salary_Item { get; set; }
        public DateTime Sal_Month { get; set; }
        public string Employee_ID { get; set; }
        public string Salary_Item_Name { get; set; }
        public string Salary_Item_NameTW { get; set; }
        public int Amount { get; set; }
    }
    public class MonthlySalaryMasterFileBackupQuery_CRUD
    {
        public string Function { get; set; }
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
        public MonthlySalaryMasterFileBackupQuery_General Data { get; set; }

        public MonthlySalaryMasterFileBackupQuery_CRUD() { }
        public MonthlySalaryMasterFileBackupQuery_CRUD(string function, MonthlySalaryMasterFileBackupQuery_General data)
        {
            Function = function;
            IsSuccess = true;
            Data = data;
        }
    }
    public class MonthlySalaryMasterFileBackupQuery_General
    {
        public List<HRMS_Sal_Master> HRMS_Sal_Master_List { get; set; }
        public HRMS_Sal_MasterBackup HRMS_Sal_MasterBackup { get; set; }
        public List<HRMS_Sal_MasterBackup> HRMS_Sal_MasterBackup_List { get; set; }
        public HRMS_Sal_MasterBackup_Detail HRMS_Sal_MasterBackup_Detail { get; set; }
        public List<HRMS_Sal_MasterBackup_Detail> HRMS_Sal_MasterBackup_Detail_List { get; set; }

        public MonthlySalaryMasterFileBackupQuery_General() { }

        public MonthlySalaryMasterFileBackupQuery_General(HRMS_Sal_MasterBackup hRMS_Sal_MasterBackup)
        {
            HRMS_Sal_MasterBackup = hRMS_Sal_MasterBackup;
        }
        public MonthlySalaryMasterFileBackupQuery_General(List<HRMS_Sal_MasterBackup> hRMS_Sal_MasterBackup_List)
        {
            HRMS_Sal_MasterBackup_List = hRMS_Sal_MasterBackup_List;
        }
        public MonthlySalaryMasterFileBackupQuery_General(HRMS_Sal_MasterBackup_Detail hRMS_Sal_MasterBackup_Detail)
        {
            HRMS_Sal_MasterBackup_Detail = hRMS_Sal_MasterBackup_Detail;
        }
        public MonthlySalaryMasterFileBackupQuery_General(List<HRMS_Sal_MasterBackup_Detail> hRMS_Sal_MasterBackup_Detail_List)
        {
            HRMS_Sal_MasterBackup_Detail_List = hRMS_Sal_MasterBackup_Detail_List;
        }
    }
}