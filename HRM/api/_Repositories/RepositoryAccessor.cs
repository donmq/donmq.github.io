using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API._Repositories
{
    public class RepositoryAccessor<_DBContext> : RepositoryAccessorBase<_DBContext>, IRepositoryAccessor where _DBContext : DbContext
    {
        public RepositoryAccessor(_DBContext dbContext)
        {
            _context = dbContext;
            HP_CharacterMap = new Repository<HP_CharacterMap, _DBContext>(_context);
            HP_g26 = new Repository<HP_g26, _DBContext>(_context);
            HP_g33 = new Repository<HP_g33, _DBContext>(_context);
            HP_g35b = new Repository<HP_g35b, _DBContext>(_context);
            HP_g36 = new Repository<HP_g36, _DBContext>(_context);
            HP_g37 = new Repository<HP_g37, _DBContext>(_context);
            HP_g59c = new Repository<HP_g59c, _DBContext>(_context);
            HP_HRMS_Emp_Personal_g01 = new Repository<HP_HRMS_Emp_Personal_g01, _DBContext>(_context);
            HP_j02 = new Repository<HP_j02, _DBContext>(_context);
            HP_y14 = new Repository<HP_y14, _DBContext>(_context);
            HRMS_Att_Annual_Leave = new Repository<HRMS_Att_Annual_Leave, _DBContext>(_context);
            HRMS_Att_Calendar = new Repository<HRMS_Att_Calendar, _DBContext>(_context);
            HRMS_Att_Change_Reason = new Repository<HRMS_Att_Change_Reason, _DBContext>(_context);
            HRMS_Att_Change_Record = new Repository<HRMS_Att_Change_Record, _DBContext>(_context);
            HRMS_Att_Female_Menstrual_Hours = new Repository<HRMS_Att_Female_Menstrual_Hours, _DBContext>(_context);
            HRMS_Att_Leave_Application = new Repository<HRMS_Att_Leave_Application, _DBContext>(_context);
            HRMS_Att_Leave_Maintain = new Repository<HRMS_Att_Leave_Maintain, _DBContext>(_context);
            HRMS_Att_Loaned_Monthly = new Repository<HRMS_Att_Loaned_Monthly, _DBContext>(_context);
            HRMS_Att_Loaned_Monthly_Detail = new Repository<HRMS_Att_Loaned_Monthly_Detail, _DBContext>(_context);
            HRMS_Att_Lunchtime = new Repository<HRMS_Att_Lunchtime, _DBContext>(_context);
            HRMS_Att_Monthly = new Repository<HRMS_Att_Monthly, _DBContext>(_context);
            HRMS_Att_Monthly_Detail = new Repository<HRMS_Att_Monthly_Detail, _DBContext>(_context);
            HRMS_Att_Monthly_Period = new Repository<HRMS_Att_Monthly_Period, _DBContext>(_context);
            HRMS_Att_Overtime_Application = new Repository<HRMS_Att_Overtime_Application, _DBContext>(_context);
            HRMS_Att_Overtime_Maintain = new Repository<HRMS_Att_Overtime_Maintain, _DBContext>(_context);
            HRMS_Att_Overtime_Parameter = new Repository<HRMS_Att_Overtime_Parameter, _DBContext>(_context);
            HRMS_Att_Overtime_Temp = new Repository<HRMS_Att_Overtime_Temp, _DBContext>(_context);
            HRMS_Att_Posting = new Repository<HRMS_Att_Posting, _DBContext>(_context);
            HRMS_Att_Pregnancy_Data = new Repository<HRMS_Att_Pregnancy_Data, _DBContext>(_context);
            HRMS_Att_Probation_Monthly = new Repository<HRMS_Att_Probation_Monthly, _DBContext>(_context);
            HRMS_Att_Probation_Monthly_Detail = new Repository<HRMS_Att_Probation_Monthly_Detail, _DBContext>(_context);
            HRMS_Att_Resign_Monthly = new Repository<HRMS_Att_Resign_Monthly, _DBContext>(_context);
            HRMS_Att_Resign_Monthly_Detail = new Repository<HRMS_Att_Resign_Monthly_Detail, _DBContext>(_context);
            HRMS_Att_Swipe_Card = new Repository<HRMS_Att_Swipe_Card, _DBContext>(_context);
            HRMS_Att_Swipecard_Set = new Repository<HRMS_Att_Swipecard_Set, _DBContext>(_context);
            HRMS_Att_Temp_Record = new Repository<HRMS_Att_Temp_Record, _DBContext>(_context);
            HRMS_Att_Use_Monthly_Leave = new Repository<HRMS_Att_Use_Monthly_Leave, _DBContext>(_context);
            HRMS_Att_Work_Shift = new Repository<HRMS_Att_Work_Shift, _DBContext>(_context);
            HRMS_Att_Work_Shift_Change = new Repository<HRMS_Att_Work_Shift_Change, _DBContext>(_context);
            HRMS_Att_Work_Type_Days = new Repository<HRMS_Att_Work_Type_Days, _DBContext>(_context);
            HRMS_Att_Yearly = new Repository<HRMS_Att_Yearly, _DBContext>(_context);
            HRMS_Att_SwipeCard_Anomalies_Set = new Repository<HRMS_Att_SwipeCard_Anomalies_Set, _DBContext>(_context);
            HRMS_Basic_Account = new Repository<HRMS_Basic_Account, _DBContext>(_context);
            HRMS_Basic_Account_Role = new Repository<HRMS_Basic_Account_Role, _DBContext>(_context);
            HRMS_Basic_Code = new Repository<HRMS_Basic_Code, _DBContext>(_context);
            HRMS_Basic_Code_Language = new Repository<HRMS_Basic_Code_Language, _DBContext>(_context);
            HRMS_Basic_Code_Type = new Repository<HRMS_Basic_Code_Type, _DBContext>(_context);
            HRMS_Basic_Code_Type_Language = new Repository<HRMS_Basic_Code_Type_Language, _DBContext>(_context);
            HRMS_Basic_Factory_Comparison = new Repository<HRMS_Basic_Factory_Comparison, _DBContext>(_context);
            HRMS_Basic_Level = new Repository<HRMS_Basic_Level, _DBContext>(_context);
            HRMS_Basic_Role = new Repository<HRMS_Basic_Role, _DBContext>(_context);
            HRMS_Basic_Role_Program_Group = new Repository<HRMS_Basic_Role_Program_Group, _DBContext>(_context);
            HRMS_Emp_Blacklist = new Repository<HRMS_Emp_Blacklist, _DBContext>(_context);
            HRMS_Emp_Certification = new Repository<HRMS_Emp_Certification, _DBContext>(_context);
            HRMS_Emp_Contract_Management = new Repository<HRMS_Emp_Contract_Management, _DBContext>(_context);
            HRMS_Emp_Contract_Type = new Repository<HRMS_Emp_Contract_Type, _DBContext>(_context);
            HRMS_Emp_Contract_Type_Detail = new Repository<HRMS_Emp_Contract_Type_Detail, _DBContext>(_context);
            HRMS_Emp_Dependent = new Repository<HRMS_Emp_Dependent, _DBContext>(_context);
            HRMS_Emp_Document = new Repository<HRMS_Emp_Document, _DBContext>(_context);
            HRMS_Emp_Educational = new Repository<HRMS_Emp_Educational, _DBContext>(_context);
            HRMS_Emp_Educational_File = new Repository<HRMS_Emp_Educational_File, _DBContext>(_context);
            HRMS_Emp_Emergency_Contact = new Repository<HRMS_Emp_Emergency_Contact, _DBContext>(_context);
            HRMS_Emp_Exit_History = new Repository<HRMS_Emp_Exit_History, _DBContext>(_context);
            HRMS_Emp_External_Experience = new Repository<HRMS_Emp_External_Experience, _DBContext>(_context);
            HRMS_Emp_File = new Repository<HRMS_Emp_File, _DBContext>(_context);
            HRMS_Emp_Group = new Repository<HRMS_Emp_Group, _DBContext>(_context);
            HRMS_Emp_IDcard_EmpID_History = new Repository<HRMS_Emp_IDcard_EmpID_History, _DBContext>(_context);
            HRMS_Emp_Identity_Card_History = new Repository<HRMS_Emp_Identity_Card_History, _DBContext>(_context);
            HRMS_Emp_Permission_Group = new Repository<HRMS_Emp_Permission_Group, _DBContext>(_context);
            HRMS_Emp_Personal = new Repository<HRMS_Emp_Personal, _DBContext>(_context);
            HRMS_Emp_Rehire_Evaluation = new Repository<HRMS_Emp_Rehire_Evaluation, _DBContext>(_context);
            HRMS_Emp_Resignation = new Repository<HRMS_Emp_Resignation, _DBContext>(_context);
            HRMS_Emp_Resignation_History = new Repository<HRMS_Emp_Resignation_History, _DBContext>(_context);
            HRMS_Emp_Skill = new Repository<HRMS_Emp_Skill, _DBContext>(_context);
            HRMS_Emp_Transfer_History = new Repository<HRMS_Emp_Transfer_History, _DBContext>(_context);
            HRMS_Emp_Transfer_Operation = new Repository<HRMS_Emp_Transfer_Operation, _DBContext>(_context);
            HRMS_Emp_Unpaid_Leave = new Repository<HRMS_Emp_Unpaid_Leave, _DBContext>(_context);
            HRMS_Ins_Benefits_Maintain = new Repository<HRMS_Ins_Benefits_Maintain, _DBContext>(_context);
            HRMS_Ins_Emp_Maintain = new Repository<HRMS_Ins_Emp_Maintain, _DBContext>(_context);
            HRMS_Ins_Rate_Setting = new Repository<HRMS_Ins_Rate_Setting, _DBContext>(_context);
            HRMS_Org_Department = new Repository<HRMS_Org_Department, _DBContext>(_context);
            HRMS_Org_Department_Language = new Repository<HRMS_Org_Department_Language, _DBContext>(_context);
            HRMS_Org_Direct_Department = new Repository<HRMS_Org_Direct_Department, _DBContext>(_context);
            HRMS_Org_Direct_Section = new Repository<HRMS_Org_Direct_Section, _DBContext>(_context);
            HRMS_Org_Work_Type_Headcount = new Repository<HRMS_Org_Work_Type_Headcount, _DBContext>(_context);
            HRMS_Rew_EmpRecords = new Repository<HRMS_Rew_EmpRecords, _DBContext>(_context);
            HRMS_Rew_ReasonCode = new Repository<HRMS_Rew_ReasonCode, _DBContext>(_context);
            HRMS_Sal_AddDedItem_AccountCode = new Repository<HRMS_Sal_AddDedItem_AccountCode, _DBContext>(_context);
            HRMS_Sal_AddDedItem_Monthly = new Repository<HRMS_Sal_AddDedItem_Monthly, _DBContext>(_context);
            HRMS_Sal_AddDedItem_Settings = new Repository<HRMS_Sal_AddDedItem_Settings, _DBContext>(_context);
            HRMS_Sal_Bank_Account = new Repository<HRMS_Sal_Bank_Account, _DBContext>(_context);
            HRMS_Sal_Childcare_Subsidy = new Repository<HRMS_Sal_Childcare_Subsidy, _DBContext>(_context);
            HRMS_Sal_Close = new Repository<HRMS_Sal_Close, _DBContext>(_context);
            HRMS_Sal_Currency_Rate = new Repository<HRMS_Sal_Currency_Rate, _DBContext>(_context);
            HRMS_Sal_Dept_SAPCostCenter_Mapping = new Repository<HRMS_Sal_Dept_SAPCostCenter_Mapping, _DBContext>(_context);
            HRMS_Sal_History = new Repository<HRMS_Sal_History, _DBContext>(_context);
            HRMS_Sal_History_Detail = new Repository<HRMS_Sal_History_Detail, _DBContext>(_context);
            HRMS_Sal_History_Test = new Repository<HRMS_Sal_History_Test, _DBContext>(_context);
            HRMS_Sal_Item_Settings = new Repository<HRMS_Sal_Item_Settings, _DBContext>(_context);
            HRMS_Sal_Leave_Calc_Maintenance = new Repository<HRMS_Sal_Leave_Calc_Maintenance, _DBContext>(_context);
            HRMS_Sal_Master = new Repository<HRMS_Sal_Master, _DBContext>(_context);
            HRMS_Sal_Master_Detail = new Repository<HRMS_Sal_Master_Detail, _DBContext>(_context);
            HRMS_Sal_MasterBackup = new Repository<HRMS_Sal_MasterBackup, _DBContext>(_context);
            HRMS_Sal_MasterBackup_Detail = new Repository<HRMS_Sal_MasterBackup_Detail, _DBContext>(_context);
            HRMS_Sal_Monthly = new Repository<HRMS_Sal_Monthly, _DBContext>(_context);
            HRMS_Sal_Monthly_Detail = new Repository<HRMS_Sal_Monthly_Detail, _DBContext>(_context);
            HRMS_Sal_Parameter = new Repository<HRMS_Sal_Parameter, _DBContext>(_context);
            HRMS_Sal_Payslip_Email = new Repository<HRMS_Sal_Payslip_Email, _DBContext>(_context);
            HRMS_Sal_Probation_MasterBackup = new Repository<HRMS_Sal_Probation_MasterBackup, _DBContext>(_context);
            HRMS_Sal_Probation_MasterBackup_Detail = new Repository<HRMS_Sal_Probation_MasterBackup_Detail, _DBContext>(_context);
            HRMS_Sal_Probation_Monthly = new Repository<HRMS_Sal_Probation_Monthly, _DBContext>(_context);
            HRMS_Sal_Probation_Monthly_Detail = new Repository<HRMS_Sal_Probation_Monthly_Detail, _DBContext>(_context);
            HRMS_Sal_Resign_Monthly = new Repository<HRMS_Sal_Resign_Monthly, _DBContext>(_context);
            HRMS_Sal_Resign_Monthly_Detail = new Repository<HRMS_Sal_Resign_Monthly_Detail, _DBContext>(_context);
            HRMS_Sal_SalaryItem_AccountCode = new Repository<HRMS_Sal_SalaryItem_AccountCode, _DBContext>(_context);
            HRMS_Sal_SAPCostCenter = new Repository<HRMS_Sal_SAPCostCenter, _DBContext>(_context);
            HRMS_Sal_Tax = new Repository<HRMS_Sal_Tax, _DBContext>(_context);
            HRMS_Sal_Tax_Number = new Repository<HRMS_Sal_Tax_Number, _DBContext>(_context);
            HRMS_Sal_Taxbracket = new Repository<HRMS_Sal_Taxbracket, _DBContext>(_context);
            HRMS_Sal_TaxFree = new Repository<HRMS_Sal_TaxFree, _DBContext>(_context);
            HRMS_SYS_Directory = new Repository<HRMS_SYS_Directory, _DBContext>(_context);
            HRMS_SYS_Language = new Repository<HRMS_SYS_Language, _DBContext>(_context);
            HRMS_SYS_Program = new Repository<HRMS_SYS_Program, _DBContext>(_context);
            HRMS_SYS_Program_Function = new Repository<HRMS_SYS_Program_Function, _DBContext>(_context);
            HRMS_SYS_Program_Function_Code = new Repository<HRMS_SYS_Program_Function_Code, _DBContext>(_context);
            HRMS_SYS_Program_Language = new Repository<HRMS_SYS_Program_Language, _DBContext>(_context);
            IDX_g03b = new Repository<IDX_g03b, _DBContext>(_context);
            IDX_g26 = new Repository<IDX_g26, _DBContext>(_context);
            IDX_g33 = new Repository<IDX_g33, _DBContext>(_context);
            IDX_g35b = new Repository<IDX_g35b, _DBContext>(_context);
            IDX_g36 = new Repository<IDX_g36, _DBContext>(_context);
            IDX_g37 = new Repository<IDX_g37, _DBContext>(_context);
            IDX_g59c = new Repository<IDX_g59c, _DBContext>(_context);
            IDX_HRMS_Emp_Personal_g01 = new Repository<IDX_HRMS_Emp_Personal_g01, _DBContext>(_context);
            IDX_y14 = new Repository<IDX_y14, _DBContext>(_context);
            test = new Repository<test, _DBContext>(_context);
            HRMS_Sal_FinCategory = new Repository<HRMS_Sal_FinCategory, _DBContext>(_context);
        }
        public IRepository<HP_CharacterMap> HP_CharacterMap { get; set; }
        public IRepository<HP_g26> HP_g26 { get; set; }
        public IRepository<HP_g33> HP_g33 { get; set; }
        public IRepository<HP_g35b> HP_g35b { get; set; }
        public IRepository<HP_g36> HP_g36 { get; set; }
        public IRepository<HP_g37> HP_g37 { get; set; }
        public IRepository<HP_g59c> HP_g59c { get; set; }
        public IRepository<HP_HRMS_Emp_Personal_g01> HP_HRMS_Emp_Personal_g01 { get; set; }
        public IRepository<HP_j02> HP_j02 { get; set; }
        public IRepository<HP_y14> HP_y14 { get; set; }
        public IRepository<HRMS_Att_Annual_Leave> HRMS_Att_Annual_Leave { get; set; }
        public IRepository<HRMS_Att_Calendar> HRMS_Att_Calendar { get; set; }
        public IRepository<HRMS_Att_Change_Reason> HRMS_Att_Change_Reason { get; set; }
        public IRepository<HRMS_Att_Change_Record> HRMS_Att_Change_Record { get; set; }
        public IRepository<HRMS_Att_Female_Menstrual_Hours> HRMS_Att_Female_Menstrual_Hours { get; set; }
        public IRepository<HRMS_Att_Leave_Application> HRMS_Att_Leave_Application { get; set; }
        public IRepository<HRMS_Att_Leave_Maintain> HRMS_Att_Leave_Maintain { get; set; }
        public IRepository<HRMS_Att_Loaned_Monthly> HRMS_Att_Loaned_Monthly { get; set; }
        public IRepository<HRMS_Att_Loaned_Monthly_Detail> HRMS_Att_Loaned_Monthly_Detail { get; set; }
        public IRepository<HRMS_Att_Lunchtime> HRMS_Att_Lunchtime { get; set; }
        public IRepository<HRMS_Att_Monthly> HRMS_Att_Monthly { get; set; }
        public IRepository<HRMS_Att_Monthly_Detail> HRMS_Att_Monthly_Detail { get; set; }
        public IRepository<HRMS_Att_Monthly_Period> HRMS_Att_Monthly_Period { get; set; }
        public IRepository<HRMS_Att_Overtime_Application> HRMS_Att_Overtime_Application { get; set; }
        public IRepository<HRMS_Att_Overtime_Maintain> HRMS_Att_Overtime_Maintain { get; set; }
        public IRepository<HRMS_Att_Overtime_Parameter> HRMS_Att_Overtime_Parameter { get; set; }
        public IRepository<HRMS_Att_Overtime_Temp> HRMS_Att_Overtime_Temp { get; set; }
        public IRepository<HRMS_Att_Posting> HRMS_Att_Posting { get; set; }
        public IRepository<HRMS_Att_Pregnancy_Data> HRMS_Att_Pregnancy_Data { get; set; }
        public IRepository<HRMS_Att_Probation_Monthly> HRMS_Att_Probation_Monthly { get; set; }
        public IRepository<HRMS_Att_Probation_Monthly_Detail> HRMS_Att_Probation_Monthly_Detail { get; set; }
        public IRepository<HRMS_Att_Resign_Monthly> HRMS_Att_Resign_Monthly { get; set; }
        public IRepository<HRMS_Att_Resign_Monthly_Detail> HRMS_Att_Resign_Monthly_Detail { get; set; }
        public IRepository<HRMS_Att_Swipe_Card> HRMS_Att_Swipe_Card { get; set; }
        public IRepository<HRMS_Att_Swipecard_Set> HRMS_Att_Swipecard_Set { get; set; }
        public IRepository<HRMS_Att_Temp_Record> HRMS_Att_Temp_Record { get; set; }
        public IRepository<HRMS_Att_Use_Monthly_Leave> HRMS_Att_Use_Monthly_Leave { get; set; }
        public IRepository<HRMS_Att_Work_Shift> HRMS_Att_Work_Shift { get; set; }
        public IRepository<HRMS_Att_Work_Shift_Change> HRMS_Att_Work_Shift_Change { get; set; }
        public IRepository<HRMS_Att_Work_Type_Days> HRMS_Att_Work_Type_Days { get; set; }
        public IRepository<HRMS_Att_Yearly> HRMS_Att_Yearly { get; set; }
        public IRepository<HRMS_Att_SwipeCard_Anomalies_Set> HRMS_Att_SwipeCard_Anomalies_Set { get; set; }
        public IRepository<HRMS_Basic_Account> HRMS_Basic_Account { get; set; }
        public IRepository<HRMS_Basic_Account_Role> HRMS_Basic_Account_Role { get; set; }
        public IRepository<HRMS_Basic_Code> HRMS_Basic_Code { get; set; }
        public IRepository<HRMS_Basic_Code_Language> HRMS_Basic_Code_Language { get; set; }
        public IRepository<HRMS_Basic_Code_Type> HRMS_Basic_Code_Type { get; set; }
        public IRepository<HRMS_Basic_Code_Type_Language> HRMS_Basic_Code_Type_Language { get; set; }
        public IRepository<HRMS_Basic_Factory_Comparison> HRMS_Basic_Factory_Comparison { get; set; }
        public IRepository<HRMS_Basic_Level> HRMS_Basic_Level { get; set; }
        public IRepository<HRMS_Basic_Role> HRMS_Basic_Role { get; set; }
        public IRepository<HRMS_Basic_Role_Program_Group> HRMS_Basic_Role_Program_Group { get; set; }
        public IRepository<HRMS_Emp_Blacklist> HRMS_Emp_Blacklist { get; set; }
        public IRepository<HRMS_Emp_Certification> HRMS_Emp_Certification { get; set; }
        public IRepository<HRMS_Emp_Contract_Management> HRMS_Emp_Contract_Management { get; set; }
        public IRepository<HRMS_Emp_Contract_Type> HRMS_Emp_Contract_Type { get; set; }
        public IRepository<HRMS_Emp_Contract_Type_Detail> HRMS_Emp_Contract_Type_Detail { get; set; }
        public IRepository<HRMS_Emp_Dependent> HRMS_Emp_Dependent { get; set; }
        public IRepository<HRMS_Emp_Document> HRMS_Emp_Document { get; set; }
        public IRepository<HRMS_Emp_Educational> HRMS_Emp_Educational { get; set; }
        public IRepository<HRMS_Emp_Educational_File> HRMS_Emp_Educational_File { get; set; }
        public IRepository<HRMS_Emp_Emergency_Contact> HRMS_Emp_Emergency_Contact { get; set; }
        public IRepository<HRMS_Emp_Exit_History> HRMS_Emp_Exit_History { get; set; }
        public IRepository<HRMS_Emp_External_Experience> HRMS_Emp_External_Experience { get; set; }
        public IRepository<HRMS_Emp_File> HRMS_Emp_File { get; set; }
        public IRepository<HRMS_Emp_Group> HRMS_Emp_Group { get; set; }
        public IRepository<HRMS_Emp_IDcard_EmpID_History> HRMS_Emp_IDcard_EmpID_History { get; set; }
        public IRepository<HRMS_Emp_Identity_Card_History> HRMS_Emp_Identity_Card_History { get; set; }
        public IRepository<HRMS_Emp_Permission_Group> HRMS_Emp_Permission_Group { get; set; }
        public IRepository<HRMS_Emp_Personal> HRMS_Emp_Personal { get; set; }
        public IRepository<HRMS_Emp_Rehire_Evaluation> HRMS_Emp_Rehire_Evaluation { get; set; }
        public IRepository<HRMS_Emp_Resignation> HRMS_Emp_Resignation { get; set; }
        public IRepository<HRMS_Emp_Resignation_History> HRMS_Emp_Resignation_History { get; set; }
        public IRepository<HRMS_Emp_Skill> HRMS_Emp_Skill { get; set; }
        public IRepository<HRMS_Emp_Transfer_History> HRMS_Emp_Transfer_History { get; set; }
        public IRepository<HRMS_Emp_Transfer_Operation> HRMS_Emp_Transfer_Operation { get; set; }
        public IRepository<HRMS_Emp_Unpaid_Leave> HRMS_Emp_Unpaid_Leave { get; set; }
        public IRepository<HRMS_Ins_Benefits_Maintain> HRMS_Ins_Benefits_Maintain { get; set; }
        public IRepository<HRMS_Ins_Emp_Maintain> HRMS_Ins_Emp_Maintain { get; set; }
        public IRepository<HRMS_Ins_Rate_Setting> HRMS_Ins_Rate_Setting { get; set; }
        public IRepository<HRMS_Org_Department> HRMS_Org_Department { get; set; }
        public IRepository<HRMS_Org_Department_Language> HRMS_Org_Department_Language { get; set; }
        public IRepository<HRMS_Org_Direct_Department> HRMS_Org_Direct_Department { get; set; }
        public IRepository<HRMS_Org_Direct_Section> HRMS_Org_Direct_Section { get; set; }
        public IRepository<HRMS_Org_Work_Type_Headcount> HRMS_Org_Work_Type_Headcount { get; set; }
        public IRepository<HRMS_Rew_EmpRecords> HRMS_Rew_EmpRecords { get; set; }
        public IRepository<HRMS_Rew_ReasonCode> HRMS_Rew_ReasonCode { get; set; }
        public IRepository<HRMS_Sal_AddDedItem_AccountCode> HRMS_Sal_AddDedItem_AccountCode { get; set; }
        public IRepository<HRMS_Sal_AddDedItem_Monthly> HRMS_Sal_AddDedItem_Monthly { get; set; }
        public IRepository<HRMS_Sal_AddDedItem_Settings> HRMS_Sal_AddDedItem_Settings { get; set; }
        public IRepository<HRMS_Sal_Bank_Account> HRMS_Sal_Bank_Account { get; set; }
        public IRepository<HRMS_Sal_Childcare_Subsidy> HRMS_Sal_Childcare_Subsidy { get; set; }
        public IRepository<HRMS_Sal_Close> HRMS_Sal_Close { get; set; }
        public IRepository<HRMS_Sal_Currency_Rate> HRMS_Sal_Currency_Rate { get; set; }
        public IRepository<HRMS_Sal_Dept_SAPCostCenter_Mapping> HRMS_Sal_Dept_SAPCostCenter_Mapping { get; set; }
        public IRepository<HRMS_Sal_History> HRMS_Sal_History { get; set; }
        public IRepository<HRMS_Sal_History_Detail> HRMS_Sal_History_Detail { get; set; }
        public IRepository<HRMS_Sal_History_Test> HRMS_Sal_History_Test { get; set; }
        public IRepository<HRMS_Sal_Item_Settings> HRMS_Sal_Item_Settings { get; set; }
        public IRepository<HRMS_Sal_Leave_Calc_Maintenance> HRMS_Sal_Leave_Calc_Maintenance { get; set; }
        public IRepository<HRMS_Sal_Master> HRMS_Sal_Master { get; set; }
        public IRepository<HRMS_Sal_Master_Detail> HRMS_Sal_Master_Detail { get; set; }
        public IRepository<HRMS_Sal_MasterBackup> HRMS_Sal_MasterBackup { get; set; }
        public IRepository<HRMS_Sal_MasterBackup_Detail> HRMS_Sal_MasterBackup_Detail { get; set; }
        public IRepository<HRMS_Sal_Monthly> HRMS_Sal_Monthly { get; set; }
        public IRepository<HRMS_Sal_Monthly_Detail> HRMS_Sal_Monthly_Detail { get; set; }
        public IRepository<HRMS_Sal_Parameter> HRMS_Sal_Parameter { get; set; }
        public IRepository<HRMS_Sal_Payslip_Email> HRMS_Sal_Payslip_Email { get; set; }
        public IRepository<HRMS_Sal_Probation_MasterBackup> HRMS_Sal_Probation_MasterBackup { get; set; }
        public IRepository<HRMS_Sal_Probation_MasterBackup_Detail> HRMS_Sal_Probation_MasterBackup_Detail { get; set; }
        public IRepository<HRMS_Sal_Probation_Monthly> HRMS_Sal_Probation_Monthly { get; set; }
        public IRepository<HRMS_Sal_Probation_Monthly_Detail> HRMS_Sal_Probation_Monthly_Detail { get; set; }
        public IRepository<HRMS_Sal_Resign_Monthly> HRMS_Sal_Resign_Monthly { get; set; }
        public IRepository<HRMS_Sal_Resign_Monthly_Detail> HRMS_Sal_Resign_Monthly_Detail { get; set; }
        public IRepository<HRMS_Sal_SalaryItem_AccountCode> HRMS_Sal_SalaryItem_AccountCode { get; set; }
        public IRepository<HRMS_Sal_SAPCostCenter> HRMS_Sal_SAPCostCenter { get; set; }
        public IRepository<HRMS_Sal_Tax> HRMS_Sal_Tax { get; set; }
        public IRepository<HRMS_Sal_Tax_Number> HRMS_Sal_Tax_Number { get; set; }
        public IRepository<HRMS_Sal_Taxbracket> HRMS_Sal_Taxbracket { get; set; }
        public IRepository<HRMS_Sal_TaxFree> HRMS_Sal_TaxFree { get; set; }
        public IRepository<HRMS_SYS_Directory> HRMS_SYS_Directory { get; set; }
        public IRepository<HRMS_SYS_Language> HRMS_SYS_Language { get; set; }
        public IRepository<HRMS_SYS_Program> HRMS_SYS_Program { get; set; }
        public IRepository<HRMS_SYS_Program_Function> HRMS_SYS_Program_Function { get; set; }
        public IRepository<HRMS_SYS_Program_Function_Code> HRMS_SYS_Program_Function_Code { get; set; }
        public IRepository<HRMS_SYS_Program_Language> HRMS_SYS_Program_Language { get; set; }
        public IRepository<IDX_g03b> IDX_g03b { get; set; }
        public IRepository<IDX_g26> IDX_g26 { get; set; }
        public IRepository<IDX_g33> IDX_g33 { get; set; }
        public IRepository<IDX_g35b> IDX_g35b { get; set; }
        public IRepository<IDX_g36> IDX_g36 { get; set; }
        public IRepository<IDX_g37> IDX_g37 { get; set; }
        public IRepository<IDX_g59c> IDX_g59c { get; set; }
        public IRepository<IDX_HRMS_Emp_Personal_g01> IDX_HRMS_Emp_Personal_g01 { get; set; }
        public IRepository<IDX_y14> IDX_y14 { get; set; }
        public IRepository<test> test { get; set; }
        public IRepository<HRMS_Sal_FinCategory> HRMS_Sal_FinCategory { get; set; }      
    }
}