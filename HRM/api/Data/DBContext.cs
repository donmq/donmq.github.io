using System;
using System.Collections.Generic;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public partial class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<HP_CharacterMap> HP_CharacterMap { get; set; }

    public virtual DbSet<HP_HRMS_Emp_Personal_g01> HP_HRMS_Emp_Personal_g01 { get; set; }

    public virtual DbSet<HP_g26> HP_g26 { get; set; }

    public virtual DbSet<HP_g33> HP_g33 { get; set; }

    public virtual DbSet<HP_g35b> HP_g35b { get; set; }

    public virtual DbSet<HP_g36> HP_g36 { get; set; }

    public virtual DbSet<HP_g37> HP_g37 { get; set; }

    public virtual DbSet<HP_g59c> HP_g59c { get; set; }

    public virtual DbSet<HP_j02> HP_j02 { get; set; }

    public virtual DbSet<HP_y14> HP_y14 { get; set; }

    public virtual DbSet<HRMS_Att_Annual_Leave> HRMS_Att_Annual_Leave { get; set; }

    public virtual DbSet<HRMS_Att_Calendar> HRMS_Att_Calendar { get; set; }

    public virtual DbSet<HRMS_Att_Change_Reason> HRMS_Att_Change_Reason { get; set; }

    public virtual DbSet<HRMS_Att_Change_Record> HRMS_Att_Change_Record { get; set; }

    public virtual DbSet<HRMS_Att_Female_Menstrual_Hours> HRMS_Att_Female_Menstrual_Hours { get; set; }

    public virtual DbSet<HRMS_Att_Leave_Application> HRMS_Att_Leave_Application { get; set; }

    public virtual DbSet<HRMS_Att_Leave_Maintain> HRMS_Att_Leave_Maintain { get; set; }

    public virtual DbSet<HRMS_Att_Loaned_Monthly> HRMS_Att_Loaned_Monthly { get; set; }

    public virtual DbSet<HRMS_Att_Loaned_Monthly_Detail> HRMS_Att_Loaned_Monthly_Detail { get; set; }

    public virtual DbSet<HRMS_Att_Lunchtime> HRMS_Att_Lunchtime { get; set; }

    public virtual DbSet<HRMS_Att_Monthly> HRMS_Att_Monthly { get; set; }

    public virtual DbSet<HRMS_Att_Monthly_Detail> HRMS_Att_Monthly_Detail { get; set; }

    public virtual DbSet<HRMS_Att_Monthly_Period> HRMS_Att_Monthly_Period { get; set; }

    public virtual DbSet<HRMS_Att_Overtime_Application> HRMS_Att_Overtime_Application { get; set; }

    public virtual DbSet<HRMS_Att_Overtime_Maintain> HRMS_Att_Overtime_Maintain { get; set; }

    public virtual DbSet<HRMS_Att_Overtime_Parameter> HRMS_Att_Overtime_Parameter { get; set; }

    public virtual DbSet<HRMS_Att_Overtime_Temp> HRMS_Att_Overtime_Temp { get; set; }

    public virtual DbSet<HRMS_Att_Posting> HRMS_Att_Posting { get; set; }

    public virtual DbSet<HRMS_Att_Pregnancy_Data> HRMS_Att_Pregnancy_Data { get; set; }

    public virtual DbSet<HRMS_Att_Probation_Monthly> HRMS_Att_Probation_Monthly { get; set; }

    public virtual DbSet<HRMS_Att_Probation_Monthly_Detail> HRMS_Att_Probation_Monthly_Detail { get; set; }

    public virtual DbSet<HRMS_Att_Resign_Monthly> HRMS_Att_Resign_Monthly { get; set; }

    public virtual DbSet<HRMS_Att_Resign_Monthly_Detail> HRMS_Att_Resign_Monthly_Detail { get; set; }

    public virtual DbSet<HRMS_Att_Swipe_Card> HRMS_Att_Swipe_Card { get; set; }

    public virtual DbSet<HRMS_Att_Swipecard_Set> HRMS_Att_Swipecard_Set { get; set; }

    public virtual DbSet<HRMS_Att_Temp_Record> HRMS_Att_Temp_Record { get; set; }

    public virtual DbSet<HRMS_Att_Use_Monthly_Leave> HRMS_Att_Use_Monthly_Leave { get; set; }

    public virtual DbSet<HRMS_Att_Work_Shift> HRMS_Att_Work_Shift { get; set; }

    public virtual DbSet<HRMS_Att_Work_Shift_Change> HRMS_Att_Work_Shift_Change { get; set; }

    public virtual DbSet<HRMS_Att_Work_Type_Days> HRMS_Att_Work_Type_Days { get; set; }

    public virtual DbSet<HRMS_Att_Yearly> HRMS_Att_Yearly { get; set; }
    public virtual DbSet<HRMS_Att_SwipeCard_Anomalies_Set> HRMS_Att_SwipeCard_Anomalies_Set { get; set; }

    public virtual DbSet<HRMS_Basic_Account> HRMS_Basic_Account { get; set; }

    public virtual DbSet<HRMS_Basic_Account_Role> HRMS_Basic_Account_Role { get; set; }

    public virtual DbSet<HRMS_Basic_Code> HRMS_Basic_Code { get; set; }

    public virtual DbSet<HRMS_Basic_Code_Language> HRMS_Basic_Code_Language { get; set; }

    public virtual DbSet<HRMS_Basic_Code_Type> HRMS_Basic_Code_Type { get; set; }

    public virtual DbSet<HRMS_Basic_Code_Type_Language> HRMS_Basic_Code_Type_Language { get; set; }

    public virtual DbSet<HRMS_Basic_Factory_Comparison> HRMS_Basic_Factory_Comparison { get; set; }

    public virtual DbSet<HRMS_Basic_Level> HRMS_Basic_Level { get; set; }

    public virtual DbSet<HRMS_Basic_Role> HRMS_Basic_Role { get; set; }

    public virtual DbSet<HRMS_Basic_Role_Program_Group> HRMS_Basic_Role_Program_Group { get; set; }

    public virtual DbSet<HRMS_Emp_Blacklist> HRMS_Emp_Blacklist { get; set; }

    public virtual DbSet<HRMS_Emp_Certification> HRMS_Emp_Certification { get; set; }

    public virtual DbSet<HRMS_Emp_Contract_Management> HRMS_Emp_Contract_Management { get; set; }

    public virtual DbSet<HRMS_Emp_Contract_Type> HRMS_Emp_Contract_Type { get; set; }

    public virtual DbSet<HRMS_Emp_Contract_Type_Detail> HRMS_Emp_Contract_Type_Detail { get; set; }

    public virtual DbSet<HRMS_Emp_Dependent> HRMS_Emp_Dependent { get; set; }

    public virtual DbSet<HRMS_Emp_Document> HRMS_Emp_Document { get; set; }

    public virtual DbSet<HRMS_Emp_Educational> HRMS_Emp_Educational { get; set; }

    public virtual DbSet<HRMS_Emp_Educational_File> HRMS_Emp_Educational_File { get; set; }

    public virtual DbSet<HRMS_Emp_Emergency_Contact> HRMS_Emp_Emergency_Contact { get; set; }

    public virtual DbSet<HRMS_Emp_Exit_History> HRMS_Emp_Exit_History { get; set; }

    public virtual DbSet<HRMS_Emp_External_Experience> HRMS_Emp_External_Experience { get; set; }

    public virtual DbSet<HRMS_Emp_File> HRMS_Emp_File { get; set; }

    public virtual DbSet<HRMS_Emp_Group> HRMS_Emp_Group { get; set; }

    public virtual DbSet<HRMS_Emp_IDcard_EmpID_History> HRMS_Emp_IDcard_EmpID_History { get; set; }

    public virtual DbSet<HRMS_Emp_Identity_Card_History> HRMS_Emp_Identity_Card_History { get; set; }

    public virtual DbSet<HRMS_Emp_Permission_Group> HRMS_Emp_Permission_Group { get; set; }

    public virtual DbSet<HRMS_Emp_Personal> HRMS_Emp_Personal { get; set; }

    public virtual DbSet<HRMS_Emp_Rehire_Evaluation> HRMS_Emp_Rehire_Evaluation { get; set; }

    public virtual DbSet<HRMS_Emp_Resignation> HRMS_Emp_Resignation { get; set; }

    public virtual DbSet<HRMS_Emp_Resignation_History> HRMS_Emp_Resignation_History { get; set; }

    public virtual DbSet<HRMS_Emp_Skill> HRMS_Emp_Skill { get; set; }

    public virtual DbSet<HRMS_Emp_Transfer_History> HRMS_Emp_Transfer_History { get; set; }

    public virtual DbSet<HRMS_Emp_Transfer_Operation> HRMS_Emp_Transfer_Operation { get; set; }

    public virtual DbSet<HRMS_Emp_Unpaid_Leave> HRMS_Emp_Unpaid_Leave { get; set; }

    public virtual DbSet<HRMS_Ins_Benefits_Maintain> HRMS_Ins_Benefits_Maintain { get; set; }

    public virtual DbSet<HRMS_Ins_Emp_Maintain> HRMS_Ins_Emp_Maintain { get; set; }

    public virtual DbSet<HRMS_Ins_Rate_Setting> HRMS_Ins_Rate_Setting { get; set; }

    public virtual DbSet<HRMS_Org_Department> HRMS_Org_Department { get; set; }

    public virtual DbSet<HRMS_Org_Department_Language> HRMS_Org_Department_Language { get; set; }

    public virtual DbSet<HRMS_Org_Direct_Department> HRMS_Org_Direct_Department { get; set; }

    public virtual DbSet<HRMS_Org_Direct_Section> HRMS_Org_Direct_Section { get; set; }

    public virtual DbSet<HRMS_Org_Work_Type_Headcount> HRMS_Org_Work_Type_Headcount { get; set; }

    public virtual DbSet<HRMS_Rew_EmpRecords> HRMS_Rew_EmpRecords { get; set; }

    public virtual DbSet<HRMS_Rew_ReasonCode> HRMS_Rew_ReasonCode { get; set; }
    
    public virtual DbSet<HRMS_SYS_Directory> HRMS_SYS_Directory { get; set; }

    public virtual DbSet<HRMS_SYS_Language> HRMS_SYS_Language { get; set; }

    public virtual DbSet<HRMS_SYS_Program> HRMS_SYS_Program { get; set; }

    public virtual DbSet<HRMS_SYS_Program_Function> HRMS_SYS_Program_Function { get; set; }

    public virtual DbSet<HRMS_SYS_Program_Function_Code> HRMS_SYS_Program_Function_Code { get; set; }

    public virtual DbSet<HRMS_SYS_Program_Language> HRMS_SYS_Program_Language { get; set; }

    public virtual DbSet<HRMS_Sal_AddDedItem_AccountCode> HRMS_Sal_AddDedItem_AccountCode { get; set; }

    public virtual DbSet<HRMS_Sal_AddDedItem_Monthly> HRMS_Sal_AddDedItem_Monthly { get; set; }

    public virtual DbSet<HRMS_Sal_AddDedItem_Settings> HRMS_Sal_AddDedItem_Settings { get; set; }

    public virtual DbSet<HRMS_Sal_Bank_Account> HRMS_Sal_Bank_Account { get; set; }

    public virtual DbSet<HRMS_Sal_Childcare_Subsidy> HRMS_Sal_Childcare_Subsidy { get; set; }

    public virtual DbSet<HRMS_Sal_Close> HRMS_Sal_Close { get; set; }

    public virtual DbSet<HRMS_Sal_Currency_Rate> HRMS_Sal_Currency_Rate { get; set; }

    public virtual DbSet<HRMS_Sal_Dept_SAPCostCenter_Mapping> HRMS_Sal_Dept_SAPCostCenter_Mapping { get; set; }

    public virtual DbSet<HRMS_Sal_History> HRMS_Sal_History { get; set; }

    public virtual DbSet<HRMS_Sal_History_Detail> HRMS_Sal_History_Detail { get; set; }

    public virtual DbSet<HRMS_Sal_History_Test> HRMS_Sal_History_Test { get; set; }

    public virtual DbSet<HRMS_Sal_Item_Settings> HRMS_Sal_Item_Settings { get; set; }

    public virtual DbSet<HRMS_Sal_Leave_Calc_Maintenance> HRMS_Sal_Leave_Calc_Maintenance { get; set; }

    public virtual DbSet<HRMS_Sal_Master> HRMS_Sal_Master { get; set; }

    public virtual DbSet<HRMS_Sal_MasterBackup> HRMS_Sal_MasterBackup { get; set; }

    public virtual DbSet<HRMS_Sal_MasterBackup_Detail> HRMS_Sal_MasterBackup_Detail { get; set; }

    public virtual DbSet<HRMS_Sal_Master_Detail> HRMS_Sal_Master_Detail { get; set; }

    public virtual DbSet<HRMS_Sal_Monthly> HRMS_Sal_Monthly { get; set; }

    public virtual DbSet<HRMS_Sal_Monthly_Detail> HRMS_Sal_Monthly_Detail { get; set; }

    public virtual DbSet<HRMS_Sal_Parameter> HRMS_Sal_Parameter { get; set; }

    public virtual DbSet<HRMS_Sal_Payslip_Email> HRMS_Sal_Payslip_Email { get; set; }
    public virtual DbSet<HRMS_Sal_Probation_MasterBackup> HRMS_Sal_Probation_MasterBackup { get; set; }
    public virtual DbSet<HRMS_Sal_Probation_MasterBackup_Detail> HRMS_Sal_Probation_MasterBackup_Detail { get; set; }
    public virtual DbSet<HRMS_Sal_Probation_Monthly> HRMS_Sal_Probation_Monthly { get; set; }
    public virtual DbSet<HRMS_Sal_Probation_Monthly_Detail> HRMS_Sal_Probation_Monthly_Detail { get; set; }

    public virtual DbSet<HRMS_Sal_Resign_Monthly> HRMS_Sal_Resign_Monthly { get; set; }

    public virtual DbSet<HRMS_Sal_Resign_Monthly_Detail> HRMS_Sal_Resign_Monthly_Detail { get; set; }

    public virtual DbSet<HRMS_Sal_SAPCostCenter> HRMS_Sal_SAPCostCenter { get; set; }

    public virtual DbSet<HRMS_Sal_SalaryItem_AccountCode> HRMS_Sal_SalaryItem_AccountCode { get; set; }

    public virtual DbSet<HRMS_Sal_Tax> HRMS_Sal_Tax { get; set; }

    public virtual DbSet<HRMS_Sal_TaxFree> HRMS_Sal_TaxFree { get; set; }

    public virtual DbSet<HRMS_Sal_Tax_Number> HRMS_Sal_Tax_Number { get; set; }

    public virtual DbSet<HRMS_Sal_Taxbracket> HRMS_Sal_Taxbracket { get; set; }

    public virtual DbSet<IDX_HRMS_Emp_Personal_g01> IDX_HRMS_Emp_Personal_g01 { get; set; }

    public virtual DbSet<IDX_g03b> IDX_g03b { get; set; }

    public virtual DbSet<IDX_g26> IDX_g26 { get; set; }

    public virtual DbSet<IDX_g33> IDX_g33 { get; set; }

    public virtual DbSet<IDX_g35b> IDX_g35b { get; set; }

    public virtual DbSet<IDX_g36> IDX_g36 { get; set; }

    public virtual DbSet<IDX_g37> IDX_g37 { get; set; }

    public virtual DbSet<IDX_g59c> IDX_g59c { get; set; }

    public virtual DbSet<IDX_y14> IDX_y14 { get; set; }

    public virtual DbSet<test> test { get; set; }

    public virtual DbSet<HRMS_Sal_FinCategory> HRMS_Sal_FinCategory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HP_CharacterMap>(entity =>
        {
            entity.HasKey(e => e.SignChar).HasName("PK__Characte__12C46629A17B96A7");

            entity.Property(e => e.SignChar)
                .IsFixedLength()
                .UseCollation("Chinese_Taiwan_Stroke_CS_AS");
            entity.Property(e => e.UnsignChar).IsFixedLength();
        });

        modelBuilder.Entity<HP_HRMS_Emp_Personal_g01>(entity =>
        {
            entity.Property(e => e.BIZ_FLAG).IsFixedLength();
            entity.Property(e => e.addr).IsFixedLength();
            entity.Property(e => e.arri).IsFixedLength();
            entity.Property(e => e.cardno).IsFixedLength();
            entity.Property(e => e.cname).IsFixedLength();
            entity.Property(e => e.compy).IsFixedLength();
            entity.Property(e => e.depno).IsFixedLength();
            entity.Property(e => e.dept).IsFixedLength();
            entity.Property(e => e.doarea).IsFixedLength();
            entity.Property(e => e.edubk).IsFixedLength();
            entity.Property(e => e.empno).IsFixedLength();
            entity.Property(e => e.flag).IsFixedLength();
            entity.Property(e => e.graym).IsFixedLength();
            entity.Property(e => e.idno).IsFixedLength();
            entity.Property(e => e.impr).IsFixedLength();
            entity.Property(e => e.insno).IsFixedLength();
            entity.Property(e => e.insur1).IsFixedLength();
            entity.Property(e => e.insur2).IsFixedLength();
            entity.Property(e => e.leve1).IsFixedLength();
            entity.Property(e => e.leve2).IsFixedLength();
            entity.Property(e => e.manuf).IsFixedLength();
            entity.Property(e => e.marry).IsFixedLength();
            entity.Property(e => e.medno).IsFixedLength();
            entity.Property(e => e.native).IsFixedLength();
            entity.Property(e => e.outcd).IsFixedLength();
            entity.Property(e => e.posit).IsFixedLength();
            entity.Property(e => e.rtaf).IsFixedLength();
            entity.Property(e => e.sacode).IsFixedLength();
            entity.Property(e => e.sect).IsFixedLength();
            entity.Property(e => e.sex).IsFixedLength();
            entity.Property(e => e.sharea).IsFixedLength();
            entity.Property(e => e.speci).IsFixedLength();
            entity.Property(e => e.squad).IsFixedLength();
            entity.Property(e => e.tel).IsFixedLength();
            entity.Property(e => e.upcode).IsFixedLength();
            entity.Property(e => e.upusr).IsFixedLength();
            entity.Property(e => e.vname).IsFixedLength();
            entity.Property(e => e.wkgrp).IsFixedLength();
        });

        modelBuilder.Entity<HP_g26>(entity =>
        {
            entity.Property(e => e.BIZ_FLAG).IsFixedLength();
            entity.Property(e => e.compy).IsFixedLength();
            entity.Property(e => e.depno).IsFixedLength();
            entity.Property(e => e.flag).IsFixedLength();
            entity.Property(e => e.fun).IsFixedLength();
            entity.Property(e => e.level).IsFixedLength();
            entity.Property(e => e.manuf).IsFixedLength();
            entity.Property(e => e.speci).IsFixedLength();
            entity.Property(e => e.upcode).IsFixedLength();
            entity.Property(e => e.upusr).IsFixedLength();
            entity.Property(e => e.yymm).IsFixedLength();
        });

        modelBuilder.Entity<HP_g33>(entity =>
        {
            entity.Property(e => e.BIZ_FLAG).IsFixedLength();
            entity.Property(e => e.code).IsFixedLength();
            entity.Property(e => e.compy).IsFixedLength();
            entity.Property(e => e.ehm).IsFixedLength();
            entity.Property(e => e.empno).IsFixedLength();
            entity.Property(e => e.manuf).IsFixedLength();
            entity.Property(e => e.shm).IsFixedLength();
            entity.Property(e => e.upcode).IsFixedLength();
            entity.Property(e => e.upusr).IsFixedLength();
        });

        modelBuilder.Entity<HP_g35b>(entity =>
        {
            entity.Property(e => e.BIZ_FLAG).IsFixedLength();
            entity.Property(e => e.code).IsFixedLength();
            entity.Property(e => e.compy).IsFixedLength();
            entity.Property(e => e.empno).IsFixedLength();
            entity.Property(e => e.hm1).IsFixedLength();
            entity.Property(e => e.hm2).IsFixedLength();
            entity.Property(e => e.hm3).IsFixedLength();
            entity.Property(e => e.hm4).IsFixedLength();
            entity.Property(e => e.hm5).IsFixedLength();
            entity.Property(e => e.hm6).IsFixedLength();
            entity.Property(e => e.holi).IsFixedLength();
            entity.Property(e => e.manuf).IsFixedLength();
            entity.Property(e => e.squad).IsFixedLength();
            entity.Property(e => e.upcode).IsFixedLength();
            entity.Property(e => e.upusr).IsFixedLength();
        });

        modelBuilder.Entity<HP_g36>(entity =>
        {
            entity.Property(e => e.BIZ_FLAG).IsFixedLength();
            entity.Property(e => e.compy).IsFixedLength();
            entity.Property(e => e.dept).IsFixedLength();
            entity.Property(e => e.ehm).IsFixedLength();
            entity.Property(e => e.empno).IsFixedLength();
            entity.Property(e => e.hm).IsFixedLength();
            entity.Property(e => e.holi).IsFixedLength();
            entity.Property(e => e.manuf).IsFixedLength();
            entity.Property(e => e.shm).IsFixedLength();
            entity.Property(e => e.squad).IsFixedLength();
            entity.Property(e => e.upcode).IsFixedLength();
            entity.Property(e => e.upusr).IsFixedLength();
        });

        modelBuilder.Entity<HP_g37>(entity =>
        {
            entity.Property(e => e.BIZ_FLAG).IsFixedLength();
            entity.Property(e => e.code).IsFixedLength();
            entity.Property(e => e.compy).IsFixedLength();
            entity.Property(e => e.empno).IsFixedLength();
            entity.Property(e => e.manuf).IsFixedLength();
            entity.Property(e => e.squad).IsFixedLength();
            entity.Property(e => e.upcode).IsFixedLength();
            entity.Property(e => e.upusr).IsFixedLength();
        });

        modelBuilder.Entity<HP_g59c>(entity =>
        {
            entity.Property(e => e.BIZ_FLAG).IsFixedLength();
            entity.Property(e => e.cls).IsFixedLength();
            entity.Property(e => e.compy).IsFixedLength();
            entity.Property(e => e.depno).IsFixedLength();
            entity.Property(e => e.empno).IsFixedLength();
            entity.Property(e => e.manuf).IsFixedLength();
            entity.Property(e => e.remark).IsFixedLength();
            entity.Property(e => e.speci1).IsFixedLength();
            entity.Property(e => e.speci2).IsFixedLength();
            entity.Property(e => e.upcode).IsFixedLength();
            entity.Property(e => e.upusr).IsFixedLength();
        });

        modelBuilder.Entity<HP_j02>(entity =>
        {
            entity.Property(e => e.Status).IsFixedLength();
        });

        modelBuilder.Entity<HP_y14>(entity =>
        {
            entity.Property(e => e.BIZ_FLAG).IsFixedLength();
            entity.Property(e => e.compy).IsFixedLength();
            entity.Property(e => e.depno).IsFixedLength();
            entity.Property(e => e.dept).IsFixedLength();
            entity.Property(e => e.edubk).IsFixedLength();
            entity.Property(e => e.empno).IsFixedLength();
            entity.Property(e => e.idno).IsFixedLength();
            entity.Property(e => e.jcause).IsFixedLength();
            entity.Property(e => e.jname).IsFixedLength();
            entity.Property(e => e.jposit).IsFixedLength();
            entity.Property(e => e.leve1).IsFixedLength();
            entity.Property(e => e.leve2).IsFixedLength();
            entity.Property(e => e.manuf).IsFixedLength();
            entity.Property(e => e.name).IsFixedLength();
            entity.Property(e => e.odepno).IsFixedLength();
            entity.Property(e => e.omanuf).IsFixedLength();
            entity.Property(e => e.outcd).IsFixedLength();
            entity.Property(e => e.posit).IsFixedLength();
            entity.Property(e => e.sex).IsFixedLength();
            entity.Property(e => e.tab).IsFixedLength();
            entity.Property(e => e.tel).IsFixedLength();
            entity.Property(e => e.upcode).IsFixedLength();
            entity.Property(e => e.upusr).IsFixedLength();
        });

        modelBuilder.Entity<HRMS_Att_Annual_Leave>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Att_Annual_Leave_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Annual_Start).HasComment("可休開始日");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Leave_Code).HasComment("出勤代碼40");
            entity.Property(e => e.Annual_End).HasComment("可休結束日");
            entity.Property(e => e.Previous_Hours).HasComment("上期結轉時數");
            entity.Property(e => e.Total_Days).HasComment("換算天數");
            entity.Property(e => e.Total_Hours).HasComment("合計時數");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
            entity.Property(e => e.Year_Hours).HasComment("年度時數");
        });

        modelBuilder.Entity<HRMS_Att_Calendar>(entity =>
        {
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Att_Date).HasComment("假日/重要日期");
            entity.Property(e => e.Describe).HasComment("說明");
            entity.Property(e => e.Reason).HasComment("停用原因");
            entity.Property(e => e.Type_Code).HasComment("類別代碼39");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Att_Change_Reason>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Att_Change_Reason_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Att_Date).HasComment("出勤日期");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Clock_In)
                .IsFixedLength()
                .HasComment("原上班刷卡");
            entity.Property(e => e.Clock_Out)
                .IsFixedLength()
                .HasComment("原下班刷卡");
            entity.Property(e => e.Leave_Code).HasComment("出勤代碼");
            entity.Property(e => e.Overtime_ClockIn)
                .IsFixedLength()
                .HasComment("原加班上班");
            entity.Property(e => e.Overtime_ClockOut)
                .IsFixedLength()
                .HasComment("原加班下班");
            entity.Property(e => e.Reason_Code).HasComment("出勤修改原因");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
            entity.Property(e => e.Work_Shift_Type).HasComment("班別");
        });

        modelBuilder.Entity<HRMS_Att_Change_Record>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("HP_g35b_Trigger_IUD"));

            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Att_Change_Record_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Att_Date).HasComment("出勤日期");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Clock_In)
                .IsFixedLength()
                .HasComment("上班刷卡時間");
            entity.Property(e => e.Clock_Out)
                .IsFixedLength()
                .HasComment("下班刷卡時間");
            entity.Property(e => e.Days).HasComment("天數");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Holiday).HasComment("假日否");
            entity.Property(e => e.Leave_Code).HasComment("出勤代碼");
            entity.Property(e => e.Overtime_ClockIn)
                .IsFixedLength()
                .HasComment("加班上班");
            entity.Property(e => e.Overtime_ClockOut)
                .IsFixedLength()
                .HasComment("加班下班");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
            entity.Property(e => e.Work_Shift_Type).HasComment("班別");
        });

        modelBuilder.Entity<HRMS_Att_Female_Menstrual_Hours>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Att_Female_Menstrual_Hours_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Att_Month).HasComment("月份");
            entity.Property(e => e.Breaks_Date).HasComment("休息日期");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Time_Start)
                .IsFixedLength()
                .HasComment("休息開始時間");
            entity.Property(e => e.Breaks_Hours).HasComment("休息時數");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Time_End)
                .IsFixedLength()
                .HasComment("休息結束時間");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Att_Leave_Application>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("HP_g33_Trigger_IUD"));

            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Att_Leave_Application_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Leave_code).HasComment("出勤代碼");
            entity.Property(e => e.Leave_Start).HasComment("請假開始日");
            entity.Property(e => e.Days).HasComment("天數");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Leave_End).HasComment("請假結束日");
            entity.Property(e => e.Min_End)
                .IsFixedLength()
                .HasComment("時間結束");
            entity.Property(e => e.Min_Start)
                .IsFixedLength()
                .HasComment("時間開始");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Att_Leave_Maintain>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("HP_g37_Trigger_IUD"));

            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Att_Leave_Maintain_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Leave_code).HasComment("出勤代碼");
            entity.Property(e => e.Leave_Date).HasComment("請假日期");
            entity.Property(e => e.Days).HasComment("天數");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
            entity.Property(e => e.Work_Shift_Type).HasComment("班別");
        });

        modelBuilder.Entity<HRMS_Att_Loaned_Monthly>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Att_Loaned_Monthly_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Att_Month).HasComment("出勤月份");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Actual_Days).HasComment("實際上班天數_計薪天數");
            entity.Property(e => e.Delay_Early).HasComment("遲到早退");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Food_Expenses).HasComment("伙食費次數");
            entity.Property(e => e.Night_Eat_Times).HasComment("夜點費次數");
            entity.Property(e => e.No_Swip_Card).HasComment("未刷卡次");
            entity.Property(e => e.Pass).HasComment("過帳碼");
            entity.Property(e => e.Permission_Group).HasComment("權限身分別");
            entity.Property(e => e.Resign_Status)
                .IsFixedLength()
                .HasComment("離職Y");
            entity.Property(e => e.Salary_Days).HasComment("應上班天數");
            entity.Property(e => e.Salary_Type).HasComment("薪資計別");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Att_Loaned_Monthly_Detail>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Att_Loaned_Monthly_Detail_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Att_Month).HasComment("出勤月份");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Leave_Type).HasComment("1:出勤類 2:加班補助類");
            entity.Property(e => e.Leave_Code).HasComment("出勤代碼");
            entity.Property(e => e.Days).HasComment("天數");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Att_Lunchtime>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Att_Lunchtime_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Lunch_End)
                .IsFixedLength()
                .HasComment("午休時間迄");
            entity.Property(e => e.Lunch_Start)
                .IsFixedLength()
                .HasComment("午休時間起");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Att_Monthly>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Att_Monthly_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Att_Month).HasComment("出勤月份");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Actual_Days).HasComment("實際上班天數_計薪天數");
            entity.Property(e => e.DayShift_Food).HasComment("白班伙食次數");
            entity.Property(e => e.Delay_Early).HasComment("遲到早退");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Food_Expenses).HasComment("伙食費次數");
            entity.Property(e => e.NightShift_Food).HasComment("夜班伙食次數");
            entity.Property(e => e.Night_Eat_Times).HasComment("夜點費次數");
            entity.Property(e => e.No_Swip_Card).HasComment("未刷卡次");
            entity.Property(e => e.Pass).HasComment("過帳碼");
            entity.Property(e => e.Permission_Group).HasComment("權限身分別");
            entity.Property(e => e.Probation)
                .IsFixedLength()
                .HasComment("試用期識別_N.正式員工   Y.為試用期轉正");
            entity.Property(e => e.Resign_Status)
                .IsFixedLength()
                .HasComment("離職Y");
            entity.Property(e => e.Salary_Days).HasComment("應上班天數");
            entity.Property(e => e.Salary_Type).HasComment("薪資計別");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Att_Monthly_Detail>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Att_Monthly_Detail_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Att_Month).HasComment("出勤月份");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Leave_Type).HasComment("1:出勤類 2:加班補助類");
            entity.Property(e => e.Leave_Code).HasComment("出勤代碼40");
            entity.Property(e => e.Days).HasComment("天數");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Att_Monthly_Period>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Att_Month).HasComment("出勤月份");
            entity.Property(e => e.Deadline_End).HasComment("結算結束日");
            entity.Property(e => e.Deadline_Start).HasComment("結算開始日");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Att_Overtime_Application>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Att_Overtime_Application_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Overtime_Date).HasComment("日期");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Night_Eat_Times).HasComment("夜點費次數");
            entity.Property(e => e.Night_Hours).HasComment("夜班上班時數");
            entity.Property(e => e.Overtime_End)
                .IsFixedLength()
                .HasComment("加班結束時間");
            entity.Property(e => e.Overtime_Hours).HasComment("加班時數");
            entity.Property(e => e.Overtime_Start)
                .IsFixedLength()
                .HasComment("加班開始時間");
            entity.Property(e => e.Training_Hours).HasComment("訓練時數");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
            entity.Property(e => e.Work_Shift_Type).HasComment("班別");
        });

        modelBuilder.Entity<HRMS_Att_Overtime_Maintain>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("HP_g36_Trigger_IUD"));

            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Att_Overtime_Maintain_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Overtime_Date).HasComment("日期");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Holiday).HasComment("假日否");
            entity.Property(e => e.Night_Eat_Times).HasComment("夜點費次數");
            entity.Property(e => e.Night_Hours).HasComment("夜班上班時數");
            entity.Property(e => e.Night_Overtime_Hours).HasComment("夜班加班時數");
            entity.Property(e => e.Overtime_End)
                .IsFixedLength()
                .HasComment("加班結束");
            entity.Property(e => e.Overtime_Hours).HasComment("加班時數");
            entity.Property(e => e.Overtime_Start)
                .IsFixedLength()
                .HasComment("加班開始");
            entity.Property(e => e.Training_Hours).HasComment("訓練時數");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
            entity.Property(e => e.Work_Shift_Type).HasComment("班別");
        });

        modelBuilder.Entity<HRMS_Att_Overtime_Parameter>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Work_Shift_Type).HasComment("班別");
            entity.Property(e => e.Effective_Month).HasComment("生效月份");
            entity.Property(e => e.Overtime_Start).HasComment("加班時間-起");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Night_Hours).HasComment("夜班上班時數");
            entity.Property(e => e.Overtime_End).HasComment("加班時間-訖");
            entity.Property(e => e.Overtime_Hours).HasComment("加班時數");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Att_Overtime_Temp>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Att_Overtime_Temp_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Overtime_Date).HasComment("日期");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Holiday).HasComment("假日否");
            entity.Property(e => e.Night_Eat_Times).HasComment("夜點費次數");
            entity.Property(e => e.Night_Hours).HasComment("夜班上班時數");
            entity.Property(e => e.Night_Overtime_Hours).HasComment("夜班加班時數");
            entity.Property(e => e.Overtime_End)
                .IsFixedLength()
                .HasComment("加班結束");
            entity.Property(e => e.Overtime_Hours).HasComment("加班時數");
            entity.Property(e => e.Overtime_Start)
                .IsFixedLength()
                .HasComment("加班開始");
            entity.Property(e => e.Training_Hours).HasComment("訓練時數");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
            entity.Property(e => e.Work_Shift_Type).HasComment("班別");
        });

        modelBuilder.Entity<HRMS_Att_Posting>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Posting_Date).HasComment("過帳日期");
            entity.Property(e => e.Posting_Time).HasComment("過帳時間");
            entity.Property(e => e.Att_Date).HasComment("出勤日期");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Att_Pregnancy_Data>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("HP_g59c_Trigger_IUD"));

            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Att_Pregnancy_Data_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Due_Date).HasComment("預產日期");
            entity.Property(e => e.Baby_End).HasComment("育嬰結束日期");
            entity.Property(e => e.Baby_Start).HasComment("育嬰開始日期");
            entity.Property(e => e.Close_Case).HasComment("結案");
            entity.Property(e => e.Estimated_Date1).HasComment("預計產檢日期1");
            entity.Property(e => e.Estimated_Date2).HasComment("預計產檢日期2");
            entity.Property(e => e.Estimated_Date3).HasComment("預計產檢日期3");
            entity.Property(e => e.Estimated_Date4).HasComment("預計產檢日期4");
            entity.Property(e => e.Estimated_Date5).HasComment("預計產檢日期5");
            entity.Property(e => e.GoWork_Date).HasComment("回廠日期");
            entity.Property(e => e.Insurance_Date1).HasComment("保險產檢日期1");
            entity.Property(e => e.Insurance_Date2).HasComment("保險產檢日期2");
            entity.Property(e => e.Insurance_Date3).HasComment("保險產檢日期3");
            entity.Property(e => e.Insurance_Date4).HasComment("保險產檢日期4");
            entity.Property(e => e.Insurance_Date5).HasComment("保險產檢日期5");
            entity.Property(e => e.Leave_Date1).HasComment("請假產檢日期1");
            entity.Property(e => e.Leave_Date2).HasComment("請假產檢日期2");
            entity.Property(e => e.Leave_Date3).HasComment("請假產檢日期3");
            entity.Property(e => e.Leave_Date4).HasComment("請假產檢日期4");
            entity.Property(e => e.Leave_Date5).HasComment("請假產檢日期5");
            entity.Property(e => e.Maternity_End).HasComment("產假結束日期");
            entity.Property(e => e.Maternity_Start).HasComment("產假開始日期");
            entity.Property(e => e.Pregnancy36Weeks).HasComment("懷孕滿36週日期");
            entity.Property(e => e.Pregnancy_Week).HasComment("懷孕週數");
            entity.Property(e => e.Remark).HasComment("備註");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Ultrasound_Date).HasComment("照超音波期");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
            entity.Property(e => e.Work7hours).HasComment("開始上班7小時日期");
            entity.Property(e => e.Work_Type_After).HasComment("懷孕後工種");
            entity.Property(e => e.Work_Type_Before).HasComment("懷孕前工種");
        });

        modelBuilder.Entity<HRMS_Att_Probation_Monthly>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Att_Month).HasComment("出勤月份");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Actual_Days).HasComment("實際上班天數_計薪天數");
            entity.Property(e => e.DayShift_Food).HasComment("白班伙食次數");
            entity.Property(e => e.Delay_Early).HasComment("遲到早退");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Food_Expenses).HasComment("伙食費次數");
            entity.Property(e => e.NightShift_Food).HasComment("夜班伙食次數");
            entity.Property(e => e.Night_Eat_Times).HasComment("夜點費次數");
            entity.Property(e => e.No_Swip_Card).HasComment("未刷卡次");
            entity.Property(e => e.Pass).HasComment("過帳碼");
            entity.Property(e => e.Permission_Group).HasComment("權限身分別");
            entity.Property(e => e.Probation)
                .IsFixedLength()
                .HasComment("試用期識別_N.正式員工   Y.為試用期轉正");
            entity.Property(e => e.Resign_Status)
                .IsFixedLength()
                .HasComment("離職Y");
            entity.Property(e => e.Salary_Days).HasComment("應上班天數");
            entity.Property(e => e.Salary_Type).HasComment("薪資計別");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Att_Probation_Monthly_Detail>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Att_Month).HasComment("出勤月份");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Leave_Type).HasComment("1:出勤類 2:加班補助類");
            entity.Property(e => e.Leave_Code).HasComment("出勤代碼40");
            entity.Property(e => e.Days).HasComment("天數");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Att_Resign_Monthly>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Att_Resign_Monthly").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Att_Month).HasComment("出勤月份");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Actual_Days).HasComment("實際上班天數_計薪天數");
            entity.Property(e => e.DayShift_Food).HasComment("白班伙食次數");
            entity.Property(e => e.Delay_Early).HasComment("遲到早退");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Food_Expenses).HasComment("伙食費次數");
            entity.Property(e => e.NightShift_Food).HasComment("夜班伙食次數");
            entity.Property(e => e.Night_Eat_Times).HasComment("夜點費次數");
            entity.Property(e => e.No_Swip_Card).HasComment("未刷卡次");
            entity.Property(e => e.Pass).HasComment("過帳碼");
            entity.Property(e => e.Permission_Group).HasComment("權限身分別");
            entity.Property(e => e.Probation)
                .IsFixedLength()
                .HasComment("試用期識別_N.正式員工   Y.為試用期轉正");
            entity.Property(e => e.Resign_Status)
                .IsFixedLength()
                .HasComment("離職Y");
            entity.Property(e => e.Salary_Days).HasComment("應上班天數");
            entity.Property(e => e.Salary_Type).HasComment("薪資計別");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Att_Resign_Monthly_Detail>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Att_Resign_Monthly_Detail").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Att_Month).HasComment("出勤月份");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Leave_Type).HasComment("1:出勤類 2:加班補助類");
            entity.Property(e => e.Leave_Code).HasComment("出勤代碼");
            entity.Property(e => e.Days).HasComment("天數");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Att_Swipe_Card>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Card_Time)
                .IsFixedLength()
                .HasComment("時間");
            entity.Property(e => e.Card_Date)
                .IsFixedLength()
                .HasComment("日期");
            entity.Property(e => e.Mark)
                .IsFixedLength()
                .HasComment("入考勤否");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
            entity.Property(e => e.Work_Shift_Type).HasComment("班別");
        });

        modelBuilder.Entity<HRMS_Att_Swipecard_Set>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Date_End).HasComment("日期位置迄");
            entity.Property(e => e.Date_Start).HasComment("日期位置起");
            entity.Property(e => e.Employee_End).HasComment("工號位置迄");
            entity.Property(e => e.Employee_Start).HasComment("工號位置起");
            entity.Property(e => e.Time_End).HasComment("時間位置迄");
            entity.Property(e => e.Time_Start).HasComment("時間位置起");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Att_Temp_Record>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Att_Temp_Record_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Att_Date).HasComment("出勤日期");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Clock_In)
                .IsFixedLength()
                .HasComment("上班刷卡時間");
            entity.Property(e => e.Clock_Out)
                .IsFixedLength()
                .HasComment("下班刷卡時間");
            entity.Property(e => e.Days).HasComment("天數");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Holiday).HasComment("假日否");
            entity.Property(e => e.Leave_Code).HasComment("出勤代碼");
            entity.Property(e => e.Overtime_ClockIn)
                .IsFixedLength()
                .HasComment("加班上班");
            entity.Property(e => e.Overtime_ClockOut)
                .IsFixedLength()
                .HasComment("加班下班");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
            entity.Property(e => e.Work_Shift_Type).HasComment("班別");
        });

        modelBuilder.Entity<HRMS_Att_Use_Monthly_Leave>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Effective_Month).HasComment("生效年月");
            entity.Property(e => e.Leave_Type).HasComment("1:出勤類 2:加班補助類");
            entity.Property(e => e.Code).HasComment("出勤代碼40 加班代碼42");
            entity.Property(e => e.Month_Total).HasComment("月份累計顯示");
            entity.Property(e => e.Seq).HasComment("排序顯示");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
            entity.Property(e => e.Year_Total).HasComment("年度累計顯示");
        });

        modelBuilder.Entity<HRMS_Att_Work_Shift>(entity =>
        {
            entity.HasKey(e => new { e.Factory, e.Work_Shift_Type, e.Week }).HasName("PK_HRMS_Att_Work_Shift_1");

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Work_Shift_Type).HasComment("班別_代碼41");
            entity.Property(e => e.Week)
                .IsFixedLength()
                .HasComment("星期別");
            entity.Property(e => e.Clock_In)
                .IsFixedLength()
                .HasComment("上班");
            entity.Property(e => e.Clock_Out)
                .IsFixedLength()
                .HasComment("下班");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Effective_State).HasComment("生效狀態");
            entity.Property(e => e.Lunch_End)
                .IsFixedLength()
                .HasComment("午餐時間-訖");
            entity.Property(e => e.Lunch_Start)
                .IsFixedLength()
                .HasComment("午餐時間-起");
            entity.Property(e => e.Overnight)
                .IsFixedLength()
                .HasComment("跨日");
            entity.Property(e => e.Overtime_ClockIn)
                .IsFixedLength()
                .HasComment("加班上班");
            entity.Property(e => e.Overtime_ClockOut)
                .IsFixedLength()
                .HasComment("加班下班");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
            entity.Property(e => e.Work_Days).HasComment("工作天數");
            entity.Property(e => e.Work_Hours).HasComment("工作時數");
        });

        modelBuilder.Entity<HRMS_Att_Work_Shift_Change>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Att_Work_Shift_Change_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Effective_Date).HasComment("生效日期");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Effective_State).HasComment("生效狀態");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
            entity.Property(e => e.Work_Shift_Type_New).HasComment("班別_新");
            entity.Property(e => e.Work_Shift_Type_Old).HasComment("班別_舊");
        });

        modelBuilder.Entity<HRMS_Att_Work_Type_Days>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Work_Type).HasComment("工種/職務_代碼5");
            entity.Property(e => e.Annual_leave_days).HasComment("可加休年假天數");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Effective_State).HasComment("生效狀態");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Att_Yearly>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Att_Yearly_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Att_Year).HasComment("出勤年度");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Leave_Type).HasComment("1:出勤類 2:加班補助類");
            entity.Property(e => e.Leave_Code).HasComment("出勤代碼");
            entity.Property(e => e.Days).HasComment("天數");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Att_SwipeCard_Anomalies_Set>(entity =>
            {
                entity.HasKey(e => new { e.Factory, e.Seq, e.Work_Shift_Type });
                entity.Property(e => e.Factory).IsUnicode(false).HasComment("廠別");
                entity.Property(e => e.Work_Shift_Type).IsUnicode(false).HasComment("班別");
                entity.Property(e => e.Kind).IsUnicode(false);
                entity.Property(e => e.Seq).HasComment("序號");
                entity.Property(e => e.Clock_In).IsFixedLength().HasComment("上班時間");
                entity.Property(e => e.Clock_Out_Start).IsFixedLength().HasComment("下班時間起");
                entity.Property(e => e.Clock_Out_End).IsFixedLength().HasComment("下班時間訖");
                entity.Property(e => e.Update_By).IsUnicode(false).HasComment("異動者");
                entity.Property(e => e.Update_Time).HasComment("異動日期");
            });
        modelBuilder.Entity<HRMS_Basic_Account>(entity =>
        {
            entity.Property(e => e.Account).HasComment("帳號");
            entity.Property(e => e.Department_ID).HasComment("部門代碼");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.IsActive).HasComment("啟用：1 啟用 0停用");
            entity.Property(e => e.Name).HasComment("姓名");
            entity.Property(e => e.Password).HasComment("密碼");
            entity.Property(e => e.Password_Reset).HasComment("密碼還原 1.還原初始密碼  0.舊密碼");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日");
        });

        modelBuilder.Entity<HRMS_Basic_Account_Role>(entity =>
        {
            entity.Property(e => e.Account).HasComment("帳號");
            entity.Property(e => e.Role).HasComment("角色");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日");
        });

        modelBuilder.Entity<HRMS_Basic_Code>(entity =>
        {
            entity.Property(e => e.Type_Seq).HasComment("種類序號");
            entity.Property(e => e.Code).HasComment("代碼");
            entity.Property(e => e.Char1).HasComment("字串一");
            entity.Property(e => e.Char2).HasComment("字串二");
            entity.Property(e => e.Code_Name).HasComment("代碼名稱");
            entity.Property(e => e.Date1).HasComment("日期一");
            entity.Property(e => e.Date2).HasComment("日期二");
            entity.Property(e => e.Date3).HasComment("日期三");
            entity.Property(e => e.Decimal1).HasComment("數值一");
            entity.Property(e => e.Decimal2).HasComment("數值二");
            entity.Property(e => e.Decimal3).HasComment("數值三");
            entity.Property(e => e.Int1).HasComment("整數一");
            entity.Property(e => e.Int2).HasComment("整數二");
            entity.Property(e => e.Int3).HasComment("整數三");
            entity.Property(e => e.IsActive).HasComment("啟用：1 啟用 0停用");
            entity.Property(e => e.Remark).HasComment("備註說明");
            entity.Property(e => e.Remark_Code).HasComment("備註碼");
            entity.Property(e => e.Seq).HasComment("序號");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日");
        });

        modelBuilder.Entity<HRMS_Basic_Code_Language>(entity =>
        {
            entity.Property(e => e.Type_Seq).HasComment("種類序號");
            entity.Property(e => e.Code).HasComment("代碼");
            entity.Property(e => e.Language_Code).HasComment("語系代碼");
            entity.Property(e => e.Code_Name).HasComment("代碼名稱");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日");
        });

        modelBuilder.Entity<HRMS_Basic_Code_Type>(entity =>
        {
            entity.Property(e => e.Type_Seq).HasComment("種類序號");
            entity.Property(e => e.Type_Name).HasComment("種類名稱");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日");
        });

        modelBuilder.Entity<HRMS_Basic_Code_Type_Language>(entity =>
        {
            entity.Property(e => e.Type_Seq).HasComment("種類序號");
            entity.Property(e => e.Language_Code).HasComment("語系代碼");
            entity.Property(e => e.Type_Name).HasComment("種類名稱");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日");
        });

        modelBuilder.Entity<HRMS_Basic_Factory_Comparison>(entity =>
        {
            entity.Property(e => e.Kind)
                .IsFixedLength()
                .HasComment("類別");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Modification_Date).HasComment("異動日期");
            entity.Property(e => e.Modified_by).HasComment("異動者");
        });

        modelBuilder.Entity<HRMS_Basic_Level>(entity =>
        {
            entity.Property(e => e.Level).HasComment("職等");
            entity.Property(e => e.Level_Code).HasComment("職稱代碼");
            entity.Property(e => e.Type_Code)
                .IsFixedLength()
                .HasComment("類別代碼");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日");
        });

        modelBuilder.Entity<HRMS_Basic_Role>(entity =>
        {
            entity.Property(e => e.Role).HasComment("角色");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Description).HasComment("說明");
            entity.Property(e => e.Direct).HasComment("直/間接人工：1 直接人工 2 間接人工 3 不區分");
            entity.Property(e => e.Level_End).HasComment("職等-迄");
            entity.Property(e => e.Level_Start).HasComment("職等-起");
            entity.Property(e => e.Permission_Group).HasComment("薪資計別");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日");
        });

        modelBuilder.Entity<HRMS_Basic_Role_Program_Group>(entity =>
        {
            entity.Property(e => e.Role).HasComment("角色");
            entity.Property(e => e.Program_Code).HasComment("程式代碼");
            entity.Property(e => e.Fuction_Code).HasComment("功能代碼");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日");
        });

        modelBuilder.Entity<HRMS_Emp_Blacklist>(entity =>
        {
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Maintenance_Date).HasComment("維護日期");
            entity.Property(e => e.Description).HasComment("說明");
            entity.Property(e => e.Identification_Number).HasComment("身分證號");
            entity.Property(e => e.Nationality).HasComment("國籍");
            entity.Property(e => e.Resign_Reason).HasComment("離職原因");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Emp_Certification>(entity =>
        {
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Seq).HasComment("序號");
            entity.Property(e => e.Certification).HasComment("證照類別");
            entity.Property(e => e.Certification_Valid_Period).HasComment("有效日期");
            entity.Property(e => e.Level).HasComment("級數");
            entity.Property(e => e.Name_Of_Certification).HasComment("證照測驗名稱");
            entity.Property(e => e.Passing_Date).HasComment("通過日期");
            entity.Property(e => e.Program_Code).HasComment("程式來源");
            entity.Property(e => e.Remark).HasComment("備註");
            entity.Property(e => e.Result).HasComment("合格狀態");
            entity.Property(e => e.Score).HasComment("分數");
            entity.Property(e => e.SerNum).HasComment("檔案目錄位置");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Emp_Contract_Management>(entity =>
        {
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Seq).HasComment("序號");
            entity.Property(e => e.Assessment_Result).HasComment("到期維護");
            entity.Property(e => e.Contract_End).HasComment("合同到期日(迄)");
            entity.Property(e => e.Contract_Start).HasComment("合同到期日(起)");
            entity.Property(e => e.Contract_Type).HasComment("合同類別");
            entity.Property(e => e.Effective_Status).HasComment("生效狀態");
            entity.Property(e => e.Extend_to).HasComment("延長至");
            entity.Property(e => e.Probation_End).HasComment("試用到期日");
            entity.Property(e => e.Probation_Start).HasComment("試用起始日");
            entity.Property(e => e.Reason).HasComment("原因");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Emp_Contract_Type>(entity =>
        {
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Contract_Type).HasComment("合同類別");
            entity.Property(e => e.Alert).HasComment("預警");
            entity.Property(e => e.Contract_Title).HasComment("合同名稱");
            entity.Property(e => e.Probationary_Day).HasComment("合同/試用期天數");
            entity.Property(e => e.Probationary_Month).HasComment("合同/試用期月數");
            entity.Property(e => e.Probationary_Period).HasComment("試用期");
            entity.Property(e => e.Probationary_Year).HasComment("合同/試用期年數");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Emp_Contract_Type_Detail>(entity =>
        {
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Contract_Type).HasComment("合同類別");
            entity.Property(e => e.Seq).HasComment("序號");
            entity.Property(e => e.Alert_Rules).HasComment("預警規則");
            entity.Property(e => e.Contract_End).HasComment("合同到期日(迄)");
            entity.Property(e => e.Contract_Start).HasComment("合同到期日(起)");
            entity.Property(e => e.Day_Of_Month).HasComment("每月幾號");
            entity.Property(e => e.Days_Before_Expiry_Date).HasComment("到期前天數");
            entity.Property(e => e.Month_Range).HasComment("月份範圍");
            entity.Property(e => e.Schedule_Frequency).HasComment("排程頻率");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Emp_Dependent>(entity =>
        {
            entity.HasKey(e => new { e.USER_GUID, e.Seq }).HasName("PK_HRMS_Emp_Dependent_1");

            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Seq).HasComment("序號");
            entity.Property(e => e.Dependents).HasComment("扶養親屬");
            entity.Property(e => e.Name).HasComment("眷屬姓名");
            entity.Property(e => e.Occupation).HasComment("職業");
            entity.Property(e => e.Relationship).HasComment("關係");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Emp_Document>(entity =>
        {
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Document_Type).HasComment("證件類別");
            entity.Property(e => e.Seq).HasComment("序號");
            entity.Property(e => e.Document_Number).HasComment("證件號碼");
            entity.Property(e => e.Passport_Name).HasComment("護照姓名");
            entity.Property(e => e.Program_Code).HasComment("程式來源");
            entity.Property(e => e.SerNum).HasComment("檔案目錄位置");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
            entity.Property(e => e.Validity_End).HasComment("有效日期迄");
            entity.Property(e => e.Validity_Start).HasComment("有效日期起");
        });

        modelBuilder.Entity<HRMS_Emp_Educational>(entity =>
        {
            entity.HasKey(e => new { e.USER_GUID, e.Degree, e.Period_Start }).HasName("PK_HRMS_Emp_Educational_1");

            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Degree).HasComment("學位");
            entity.Property(e => e.Period_Start).HasComment("就讀期間起");
            entity.Property(e => e.Academic_System).HasComment("學制");
            entity.Property(e => e.Department).HasComment("科系別　");
            entity.Property(e => e.Graduation).HasComment("畢業");
            entity.Property(e => e.Major).HasComment("專業");
            entity.Property(e => e.Period_End).HasComment("就讀期間迄");
            entity.Property(e => e.School).HasComment("學校");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Emp_Educational_File>(entity =>
        {
            entity.HasKey(e => new { e.USER_GUID, e.SerNum, e.FileID }).HasName("PK_HRMS_Emp_Educational_File_1");

            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.SerNum).HasComment("檔案目錄位置");
            entity.Property(e => e.FileID).HasComment("檔案ID");
            entity.Property(e => e.FileName).HasComment("檔名");
            entity.Property(e => e.FileSize).HasComment("檔案大小");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Emp_Emergency_Contact>(entity =>
        {
            entity.HasKey(e => new { e.USER_GUID, e.Seq }).HasName("PK_HRMS_Emp_Emergency_Contact_1");

            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Seq).HasComment("序號");
            entity.Property(e => e.Emergency_Contact).HasComment("緊急連絡者");
            entity.Property(e => e.Emergency_Contact_Address).HasComment("緊急連絡地址");
            entity.Property(e => e.Emergency_Contact_Phone).HasComment("緊急連絡電話");
            entity.Property(e => e.Relationship).HasComment("親屬關係");
            entity.Property(e => e.Temporary_Address).HasComment("暫住地址");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Emp_Exit_History>(entity =>
        {
            entity.HasIndex(e => new { e.Assigned_Division, e.Assigned_Factory, e.Assigned_Employee_ID }, "IX_HRMS_Emp_Exit_History_Assigned_Division_2").HasFillFactor(80);

            entity.HasIndex(e => new { e.Division, e.Factory, e.Employee_ID }, "IX_HRMS_Emp_Exit_History_Division_2").HasFillFactor(80);

            entity.HasIndex(e => new { e.Nationality, e.Identification_Number }, "IX_HRMS_Emp_Exit_History_Nationality_2").HasFillFactor(80);

            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Resign_Date).HasComment("離職日期");
            entity.Property(e => e.Annual_Leave_Seniority_Start_Date).HasComment("年假年資起算日");
            entity.Property(e => e.Assigned_Department).HasComment("派駐/支援部門");
            entity.Property(e => e.Assigned_Division).HasComment("派駐/支援事業部");
            entity.Property(e => e.Assigned_Employee_ID).HasComment("派駐/支援工號");
            entity.Property(e => e.Assigned_Factory).HasComment("派駐/支援廠別");
            entity.Property(e => e.Blacklist).HasComment("黑名單");
            entity.Property(e => e.Blood_Type)
                .IsFixedLength()
                .HasComment("血型");
            entity.Property(e => e.Chinese_Name).HasComment("中文姓名");
            entity.Property(e => e.Company).HasComment("公司");
            entity.Property(e => e.Date_of_Birth).HasComment("出生日期");
            entity.Property(e => e.Deletion_Code)
                .IsFixedLength()
                .HasComment("刪除碼");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Education).HasComment("學歷");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Employment_Status).HasComment("人員狀態");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Gender)
                .IsFixedLength()
                .HasComment("性別");
            entity.Property(e => e.Group_Date).HasComment("集團到職日");
            entity.Property(e => e.Identification_Number).HasComment("身分證號");
            entity.Property(e => e.Identity_Type).HasComment("身分別");
            entity.Property(e => e.Issued_Date).HasComment("身分證發行日");
            entity.Property(e => e.License_Plate_Number).HasComment("車號");
            entity.Property(e => e.Local_Full_Name).HasComment("本地姓名");
            entity.Property(e => e.Mailing_Address).HasComment("通訊地址");
            entity.Property(e => e.Mailing_City).HasComment("通訊_市/區/縣");
            entity.Property(e => e.Mailing_Province_Directly).HasComment("通訊_省/直轄市/縣");
            entity.Property(e => e.Marital_Status)
                .IsFixedLength()
                .HasComment("婚姻狀況");
            entity.Property(e => e.Mobile_Phone_Number).HasComment("手機號碼");
            entity.Property(e => e.Nationality).HasComment("國籍");
            entity.Property(e => e.Onboard_Date).HasComment("到職日期");
            entity.Property(e => e.Performance_Assessment_Responsibility_Division).HasComment("考核權責事業部");
            entity.Property(e => e.Permission_Group).HasComment("權限身分別");
            entity.Property(e => e.Phone_Number).HasComment("電話");
            entity.Property(e => e.Position_Grade).HasComment("職等");
            entity.Property(e => e.Position_Title).HasComment("職稱");
            entity.Property(e => e.Preferred_English_Full_Name).HasComment("慣用英文姓名");
            entity.Property(e => e.Registered_Address).HasComment("戶籍地址");
            entity.Property(e => e.Registered_City).HasComment("戶籍_市/區/縣");
            entity.Property(e => e.Registered_Province_Directly).HasComment("戶籍_省/直轄市/縣");
            entity.Property(e => e.Religion).HasComment("宗教");
            entity.Property(e => e.Resign_Reason).HasComment("離職原因");
            entity.Property(e => e.Restaurant).HasComment("餐廳");
            entity.Property(e => e.Salary_Payment_Method).HasComment("支薪方式");
            entity.Property(e => e.Salary_Type).HasComment("薪資計別");
            entity.Property(e => e.Seniority_Start_Date).HasComment("年資起算日");
            entity.Property(e => e.Swipe_Card_Number).HasComment("刷卡號");
            entity.Property(e => e.Swipe_Card_Option).HasComment("刷卡否");
            entity.Property(e => e.Transportation_Method).HasComment("交通方式");
            entity.Property(e => e.Union_Membership).HasComment("加入工會");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
            entity.Property(e => e.Vehicle_Type).HasComment("車種");
            entity.Property(e => e.Work_Location).HasComment("工作地點");
            entity.Property(e => e.Work_Shift_Type).HasComment("班別");
            entity.Property(e => e.Work_Type).HasComment("工種/職務");
        });

        modelBuilder.Entity<HRMS_Emp_External_Experience>(entity =>
        {
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Seq).HasComment("序號");
            entity.Property(e => e.Company_Name).HasComment("公司名稱");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Leadership_Role).HasComment("主管職");
            entity.Property(e => e.Position_Title).HasComment("職稱");
            entity.Property(e => e.Tenure_End).HasComment("任職期間迄");
            entity.Property(e => e.Tenure_Start).HasComment("任職期間起");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Emp_File>(entity =>
        {
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Program_Code).HasComment("程式來源");
            entity.Property(e => e.SerNum).HasComment("檔案目錄位置");
            entity.Property(e => e.FileID).HasComment("檔案ID");
            entity.Property(e => e.FileName).HasComment("檔名");
            entity.Property(e => e.FileSize).HasComment("檔案大小");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Emp_Group>(entity =>
        {
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Expertise_Category).HasComment("專長類別");
            entity.Property(e => e.Performance_Category).HasComment("績效類別");
            entity.Property(e => e.Production_Line).HasComment("線別");
            entity.Property(e => e.Technical_Type).HasComment("技術工別");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Emp_IDcard_EmpID_History>(entity =>
        {
            entity.HasIndex(e => new { e.Division, e.Factory, e.Employee_ID }, "IX_HRMS_Emp_IDcard_EmpID_History_Division_2").HasFillFactor(80);

            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Emp_IDcard_EmpID_History_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.History_GUID).HasComment("歷程GUID");
            entity.Property(e => e.Assigned_Department).HasComment("派駐/支援部門");
            entity.Property(e => e.Assigned_Division).HasComment("派駐/支援事業部");
            entity.Property(e => e.Assigned_Employee_ID).HasComment("派駐/支援工號");
            entity.Property(e => e.Assigned_Factory).HasComment("派駐/支援廠別");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Identification_Number).HasComment("身分證號");
            entity.Property(e => e.Nationality).HasComment("國籍");
            entity.Property(e => e.Onboard_Date).HasComment("到職日期");
            entity.Property(e => e.Resign_Date).HasComment("離職日期");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Emp_Identity_Card_History>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Emp_Identity_Card_History_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.History_GUID).HasComment("身分證歷程GUID");
            entity.Property(e => e.Identification_Number_After).HasComment("身分證號-異動後");
            entity.Property(e => e.Identification_Number_Before).HasComment("身分證號-異動前");
            entity.Property(e => e.Issued_Date_After).HasComment("發行日-異動後");
            entity.Property(e => e.Issued_Date_Before).HasComment("發行日-異動前");
            entity.Property(e => e.Nationality_After).HasComment("國籍-異動後");
            entity.Property(e => e.Nationality_Before).HasComment("國籍-異動前");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Emp_Permission_Group>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Permission_Group).HasComment("權限身分別_序號4");
            entity.Property(e => e.Foreign_Flag)
                .IsFixedLength()
                .HasComment("Y外籍 N當地籍");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Emp_Personal>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("HP_g01_Trigger_IUD"));

            entity.HasIndex(e => new { e.Assigned_Division, e.Assigned_Factory, e.Assigned_Employee_ID }, "IX_HRMS_Emp_Personal_Assigned_Division_2").HasFillFactor(80);

            entity.HasIndex(e => new { e.Division, e.Factory, e.Employee_ID }, "UK_HRMS_Emp_Personal_Division_2")
                .IsUnique()
                .HasFillFactor(80);

            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Annual_Leave_Seniority_Start_Date).HasComment("年假年資起算日");
            entity.Property(e => e.Assigned_Department).HasComment("派駐/支援部門");
            entity.Property(e => e.Assigned_Division).HasComment("派駐/支援事業部");
            entity.Property(e => e.Assigned_Employee_ID).HasComment("派駐/支援工號");
            entity.Property(e => e.Assigned_Factory).HasComment("派駐/支援廠別");
            entity.Property(e => e.Birthday).HasComment("出生日期");
            entity.Property(e => e.Blacklist).HasComment("黑名單");
            entity.Property(e => e.Blood_Type)
                .IsFixedLength()
                .HasComment("血型");
            entity.Property(e => e.Chinese_Name).HasComment("中文姓名");
            entity.Property(e => e.Company).HasComment("公司");
            entity.Property(e => e.Deletion_Code)
                .IsFixedLength()
                .HasComment("人員狀態Y.在職 N.離職");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Education).HasComment("學歷");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Employment_Status).HasComment("人員狀態");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Gender)
                .IsFixedLength()
                .HasComment("性別");
            entity.Property(e => e.Group_Date).HasComment("集團到職日");
            entity.Property(e => e.Identification_Number).HasComment("身分證號");
            entity.Property(e => e.Identity_Type).HasComment("身分別");
            entity.Property(e => e.Issued_Date).HasComment("身分證發行日");
            entity.Property(e => e.License_Plate_Number).HasComment("車號");
            entity.Property(e => e.Local_Full_Name).HasComment("本地姓名");
            entity.Property(e => e.Mailing_Address).HasComment("通訊地址");
            entity.Property(e => e.Mailing_City).HasComment("通訊_市/區/縣");
            entity.Property(e => e.Mailing_Province_Directly).HasComment("通訊_省/直轄市/縣");
            entity.Property(e => e.Marital_Status)
                .IsFixedLength()
                .HasComment("婚姻狀況");
            entity.Property(e => e.Mobile_Phone_Number).HasComment("手機號碼");
            entity.Property(e => e.Nationality).HasComment("國籍");
            entity.Property(e => e.Onboard_Date).HasComment("到職日期");
            entity.Property(e => e.Performance_Division).HasComment("考核權責事業部");
            entity.Property(e => e.Permission_Group).HasComment("權限身分別");
            entity.Property(e => e.Phone_Number).HasComment("電話");
            entity.Property(e => e.Position_Grade).HasComment("職等");
            entity.Property(e => e.Position_Title).HasComment("職稱");
            entity.Property(e => e.Preferred_English_Full_Name).HasComment("慣用英文姓名");
            entity.Property(e => e.Registered_Address).HasComment("戶籍地址");
            entity.Property(e => e.Registered_City).HasComment("戶籍_市/區/縣");
            entity.Property(e => e.Registered_Province_Directly).HasComment("戶籍_省/直轄市/縣");
            entity.Property(e => e.Religion).HasComment("宗教");
            entity.Property(e => e.Resign_Date).HasComment("離職日期");
            entity.Property(e => e.Resign_Reason).HasComment("離職原因");
            entity.Property(e => e.Restaurant).HasComment("餐廳");
            entity.Property(e => e.Seniority_Start_Date).HasComment("年資起算日");
            entity.Property(e => e.Swipe_Card_Number).HasComment("刷卡號");
            entity.Property(e => e.Swipe_Card_Option).HasComment("刷卡否");
            entity.Property(e => e.Transportation_Method).HasComment("交通方式");
            entity.Property(e => e.Union_Membership).HasComment("加入工會");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
            entity.Property(e => e.Vehicle_Type).HasComment("車種");
            entity.Property(e => e.Work8hours).HasComment("懷孕上班8小時");
            entity.Property(e => e.Work_Location).HasComment("工作地點");
            entity.Property(e => e.Work_Shift_Type).HasComment("班別");
            entity.Property(e => e.Work_Type).HasComment("工種/職務");
        });

        modelBuilder.Entity<HRMS_Emp_Rehire_Evaluation>(entity =>
        {
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Seq).HasComment("序號");
            entity.Property(e => e.Department).HasComment("任用部門");
            entity.Property(e => e.Division).HasComment("任用事業部");
            entity.Property(e => e.Employee_ID).HasComment("任用工號");
            entity.Property(e => e.Explanation).HasComment("評估說明");
            entity.Property(e => e.Factory).HasComment("任用廠別");
            entity.Property(e => e.Identification_Number).HasComment("身分證號");
            entity.Property(e => e.Nationality).HasComment("國籍");
            entity.Property(e => e.Results).HasComment("評估結果");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Emp_Resignation>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("HP_y14_Trigger_IUD"));

            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Blacklist).HasComment("黑名單");
            entity.Property(e => e.Identification_Number).HasComment("身分證號");
            entity.Property(e => e.Nationality).HasComment("國籍");
            entity.Property(e => e.Onboard_Date).HasComment("到職日期");
            entity.Property(e => e.Remark).HasComment("備註");
            entity.Property(e => e.Resign_Date).HasComment("離職日期");
            entity.Property(e => e.Resign_Reason).HasComment("離職原因");
            entity.Property(e => e.Resignation_Type).HasComment("離職類別");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
            entity.Property(e => e.Verifier).HasComment("判定人員工號");
            entity.Property(e => e.Verifier_Name).HasComment("判定人本地姓名");
            entity.Property(e => e.Verifier_Title).HasComment("判定人職稱");
        });

        modelBuilder.Entity<HRMS_Emp_Resignation_History>(entity =>
        {
            entity.Property(e => e.History_GUID).HasComment("歷程GUID");
            entity.Property(e => e.Blacklist).HasComment("黑名單");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Identification_Number).HasComment("身分證號");
            entity.Property(e => e.Nationality).HasComment("國籍");
            entity.Property(e => e.Onboard_Date).HasComment("到職日期");
            entity.Property(e => e.Remark).HasComment("備註");
            entity.Property(e => e.Resign_Date).HasComment("離職日期");
            entity.Property(e => e.Resign_Reason).HasComment("離職原因");
            entity.Property(e => e.Resignation_Type).HasComment("離職類別");
            entity.Property(e => e.Status)
                .IsFixedLength()
                .HasComment("狀態 D：Delete");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
            entity.Property(e => e.Verifier).HasComment("判定人員工號");
            entity.Property(e => e.Verifier_Name).HasComment("判定人本地姓名");
            entity.Property(e => e.Verifier_Title).HasComment("判定人職稱");
        });

        modelBuilder.Entity<HRMS_Emp_Skill>(entity =>
        {
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Seq).HasComment("序號");
            entity.Property(e => e.Passing_Date).HasComment("通過日期");
            entity.Property(e => e.Skill_Certification).HasComment("技能證照");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Emp_Transfer_History>(entity =>
        {
            entity.ToTable(e => e.HasTrigger("HP_g03b_Trigger_IUD"));

            entity.HasKey(e => e.History_GUID).HasName("PK_HRMS_Emp_Transfer_History_1");

            entity.HasIndex(e => new { e.Assigned_Division_After, e.Assigned_Factory_After, e.Assigned_Employee_ID_After }, "IX_HRMS_Emp_Transfer_History_Assigned_Division_After_2").HasFillFactor(80);

            entity.HasIndex(e => new { e.Division_After, e.Factory_After, e.Employee_ID_After }, "IX_HRMS_Emp_Transfer_History_Division_After_2").HasFillFactor(80);

            entity.HasIndex(e => new { e.USER_GUID, e.Effective_Date, e.Seq }, "UK_HRMS_Emp_Transfer_History_USER_GUID_2")
                .IsUnique()
                .HasFillFactor(80);

            entity.Property(e => e.Assigned_Department_After).HasComment("派駐/支援部門_異動後");
            entity.Property(e => e.Assigned_Department_Before).HasComment("派駐/支援部門");
            entity.Property(e => e.Assigned_Division_After).HasComment("派駐/支援事業部_異動後");
            entity.Property(e => e.Assigned_Division_Before).HasComment("派駐/支援事業部");
            entity.Property(e => e.Assigned_Employee_ID_After).HasComment("派駐/支援工號_異動後");
            entity.Property(e => e.Assigned_Employee_ID_Before).HasComment("派駐/支援工號");
            entity.Property(e => e.Assigned_Factory_After).HasComment("派駐/支援廠別_異動後");
            entity.Property(e => e.Assigned_Factory_Before).HasComment("派駐/支援廠別");
            entity.Property(e => e.Department_After).HasComment("部門_異動後");
            entity.Property(e => e.Department_Before).HasComment("部門");
            entity.Property(e => e.Division_After).HasComment("事業部_異動後");
            entity.Property(e => e.Division_Before).HasComment("事業部");
            entity.Property(e => e.Effective_Date).HasComment("生效日期");
            entity.Property(e => e.Effective_Status).HasComment("生效狀態");
            entity.Property(e => e.Employee_ID_After).HasComment("工號_異動後");
            entity.Property(e => e.Employee_ID_Before).HasComment("工號");
            entity.Property(e => e.Factory_After).HasComment("廠別_異動後");
            entity.Property(e => e.Factory_Before).HasComment("廠別");
            entity.Property(e => e.Identification_Number_After).HasComment("身分證號_異動後");
            entity.Property(e => e.Identification_Number_Before).HasComment("身分證號");
            entity.Property(e => e.Nationality_After).HasComment("國籍_異動後");
            entity.Property(e => e.Nationality_Before).HasComment("國籍");
            entity.Property(e => e.Position_Grade_After).HasComment("職等_異動後");
            entity.Property(e => e.Position_Grade_Before).HasComment("職等");
            entity.Property(e => e.Position_Title_After).HasComment("職稱_異動後");
            entity.Property(e => e.Position_Title_Before).HasComment("職稱");
            entity.Property(e => e.Reason_for_Change).HasComment("異動原因");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
            entity.Property(e => e.Work_Type_After).HasComment("工種/職務_異動後");
            entity.Property(e => e.Work_Type_Before).HasComment("工種/職務");
        });

        modelBuilder.Entity<HRMS_Emp_Transfer_Operation>(entity =>
        {
            entity.HasKey(e => e.History_GUID).HasName("PK_HRMS_Emp_Transfer_Operation_1");

            entity.HasIndex(e => new { e.Assigned_Division_After, e.Assigned_Factory_After, e.Assigned_Employee_ID_After }, "IX_HRMS_Emp_Transfer_Operation_Assigned_Division_After_2").HasFillFactor(80);

            entity.HasIndex(e => new { e.Assigned_Division_Before, e.Assigned_Factory_Before, e.Assigned_Employee_ID_Before }, "IX_HRMS_Emp_Transfer_Operation_Assigned_Division_Before_2").HasFillFactor(80);

            entity.HasIndex(e => new { e.Division_After, e.Factory_After, e.Employee_ID_After }, "IX_HRMS_Emp_Transfer_Operation_Division_2").HasFillFactor(80);

            entity.HasIndex(e => new { e.USER_GUID, e.Effective_Date_After }, "UK_HRMS_Emp_Transfer_Operation_USER_GUID_2")
                .IsUnique()
                .HasFillFactor(80);

            entity.Property(e => e.Assigned_Department_After).HasComment("派駐/支援部門_異動後");
            entity.Property(e => e.Assigned_Department_Before).HasComment("派駐/支援部門");
            entity.Property(e => e.Assigned_Division_After).HasComment("派駐/支援事業部_異動後");
            entity.Property(e => e.Assigned_Division_Before).HasComment("派駐/支援事業部");
            entity.Property(e => e.Assigned_Employee_ID_After).HasComment("派駐/支援工號_異動後");
            entity.Property(e => e.Assigned_Employee_ID_Before).HasComment("派駐/支援工號");
            entity.Property(e => e.Assigned_Factory_After).HasComment("派駐/支援廠別_異動後");
            entity.Property(e => e.Assigned_Factory_Before).HasComment("派駐/支援廠別");
            entity.Property(e => e.Department_After).HasComment("部門_異動後");
            entity.Property(e => e.Department_Before).HasComment("部門");
            entity.Property(e => e.Division_After).HasComment("事業部_異動後");
            entity.Property(e => e.Division_Before).HasComment("事業部");
            entity.Property(e => e.Effective_Date_After).HasComment("生效日期_異動後");
            entity.Property(e => e.Effective_Date_Before).HasComment("生效日期");
            entity.Property(e => e.Effective_Status_After).HasComment("生效狀態");
            entity.Property(e => e.Effective_Status_Before).HasComment("生效狀態");
            entity.Property(e => e.Employee_ID_After).HasComment("工號_異動後");
            entity.Property(e => e.Employee_ID_Before).HasComment("工號");
            entity.Property(e => e.Factory_After).HasComment("廠別_異動後");
            entity.Property(e => e.Factory_Before).HasComment("廠別");
            entity.Property(e => e.Identification_Number_After).HasComment("身分證號_異動後");
            entity.Property(e => e.Identification_Number_Before).HasComment("身分證號");
            entity.Property(e => e.Nationality_After).HasComment("國籍_異動後");
            entity.Property(e => e.Nationality_Before).HasComment("國籍");
            entity.Property(e => e.Position_Grade_After).HasComment("職等_異動後");
            entity.Property(e => e.Position_Grade_Before).HasComment("職等");
            entity.Property(e => e.Position_Title_After).HasComment("職稱_異動後");
            entity.Property(e => e.Position_Title_Before).HasComment("職稱");
            entity.Property(e => e.Reason_for_Change_After).HasComment("異動原因");
            entity.Property(e => e.Reason_for_Change_Before).HasComment("異動原因");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By_After).HasComment("異動者");
            entity.Property(e => e.Update_By_Before).HasComment("異動者");
            entity.Property(e => e.Update_Time_After).HasComment("異動日期");
            entity.Property(e => e.Update_Timee_Before).HasComment("異動日期");
            entity.Property(e => e.Work_Type_After).HasComment("工種/職務_異動後");
            entity.Property(e => e.Work_Type_Before).HasComment("工種/職務");
        });

        modelBuilder.Entity<HRMS_Emp_Unpaid_Leave>(entity =>
        {
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Seq).HasComment("序號");
            entity.Property(e => e.Annual_Leave_Seniority_Retention).HasComment("年假年資保留");
            entity.Property(e => e.Continuation_of_Insurance).HasComment("保險繼續加保");
            entity.Property(e => e.Effective_Status).HasComment("生效狀態");
            entity.Property(e => e.Leave_End).HasComment("留停結束日");
            entity.Property(e => e.Leave_Reason).HasComment("留停原因");
            entity.Property(e => e.Leave_Start).HasComment("留停起始日");
            entity.Property(e => e.Remark).HasComment("備註");
            entity.Property(e => e.Seniority_Retention).HasComment("年資保留");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Ins_Benefits_Maintain>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Ins_Benefits_Maintain_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Declaration_Month).HasComment("申報年月");
            entity.Property(e => e.Benefits_Kind).HasComment("疾病類別_序號58");
            entity.Property(e => e.Benefits_Start).HasComment("開始日期");
            entity.Property(e => e.Amt).HasComment("總金額");
            entity.Property(e => e.Benefits_End).HasComment("結束日期");
            entity.Property(e => e.Benefits_Num).HasComment("疾病編號");
            entity.Property(e => e.Birth_Child).HasComment("小孩出生日期");
            entity.Property(e => e.Declaration_Seq).HasComment("申報次數");
            entity.Property(e => e.Special_Work_Type)
                .IsFixedLength()
                .HasComment("特殊工種Y/N");
            entity.Property(e => e.Total_Days).HasComment("天數");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Ins_Emp_Maintain>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Insurance_Type).HasComment("保險類別_序號57");
            entity.Property(e => e.Insurance_Start).HasComment("投保開始");
            entity.Property(e => e.Insurance_End).HasComment("退保日期");
            entity.Property(e => e.Insurance_Num).HasComment("保險號碼");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Ins_Rate_Setting>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Effective_Month).HasComment("生效年月");
            entity.Property(e => e.Permission_Group).HasComment("權限身分別_序號4");
            entity.Property(e => e.Insurance_Type).HasComment("保險類別_序號57");
            entity.Property(e => e.Employee_Rate).HasComment("員工負擔比率");
            entity.Property(e => e.Employer_Rate).HasComment("公司負擔比率");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Org_Department>(entity =>
        {
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Department_Code).HasComment("部門代碼");
            entity.Property(e => e.Approved_Headcount).HasComment("編制人數");
            entity.Property(e => e.Attribute).HasComment("部門屬性：Directly 直屬；Staff 幕僚；Non-Directly 非直屬。");
            entity.Property(e => e.Center_Code).HasComment("中心代碼");
            entity.Property(e => e.Cost_Center).HasComment("成本中心編號");
            entity.Property(e => e.Department_Name).HasComment("部門名稱");
            entity.Property(e => e.Effective_Date).HasComment("生效日");
            entity.Property(e => e.Expiration_Date).HasComment("失效日");
            entity.Property(e => e.IsActive).HasComment("啟用");
            entity.Property(e => e.Org_Level).HasComment("組織層級");
            entity.Property(e => e.Supervisor_Employee_ID).HasComment("部門主管工號");
            entity.Property(e => e.Supervisor_Type)
                .IsFixedLength()
                .HasComment("主管類型：A Formal正式；B Deputy 代理；C Adjunction兼任；D Informal非正式。");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日");
            entity.Property(e => e.Upper_Department).HasComment("上一層部門代碼");
            entity.Property(e => e.Virtual_Department).HasComment("虛擬歸屬部門代碼");
        });

        modelBuilder.Entity<HRMS_Org_Department_Language>(entity =>
        {
            entity.HasKey(e => new { e.Division, e.Factory, e.Department_Code, e.Language_Code }).HasName("PK_HRMS_Org_Department_Language_1");

            entity.Property(e => e.Department_Code).HasComment("部門代碼");
            entity.Property(e => e.Language_Code).HasComment("語系代碼");
            entity.Property(e => e.Name).HasComment("部門名稱");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日");
        });

        modelBuilder.Entity<HRMS_Org_Direct_Department>(entity =>
        {
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Department_Code).HasComment("部門代碼");
            entity.Property(e => e.Line_Code).HasComment("線別代碼");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日");
        });

        modelBuilder.Entity<HRMS_Org_Direct_Section>(entity =>
        {
            entity.HasKey(e => new { e.Division, e.Factory, e.Effective_Date, e.Work_Type_Code }).HasName("PK_HRMS_Org_Direct_Section_1");

            entity.ToTable(tb => tb.HasTrigger("HP_g26_Trigger_IUD"));

            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Effective_Date).HasComment("生效年月：格式 YYYY/MM");
            entity.Property(e => e.Work_Type_Code).HasComment("工種/職務代碼");
            entity.Property(e => e.Direct_Section)
                .IsFixedLength()
                .HasComment("是否為直接工段：Y 直接工段；N 間接工段。");
            entity.Property(e => e.Section_Code).HasComment("工段代碼");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日");
        });

        modelBuilder.Entity<HRMS_Org_Work_Type_Headcount>(entity =>
        {
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Department_Code).HasComment("部門代碼");
            entity.Property(e => e.Effective_Date).HasComment("生效年月：格式 YYYY/MM。");
            entity.Property(e => e.Work_Type_Code).HasComment("工種/職務代碼");
            entity.Property(e => e.Approved_Headcount).HasComment("編制人數");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日");
        });

        modelBuilder.Entity<HRMS_Rew_EmpRecords>(entity =>
        {
            entity.HasIndex(e => new { e.Factory, e.Employee_ID, e.Reward_Date, e.Reward_Type }, "IX_HRMS_Rew_EmpRecords_Factory_2").HasFillFactor(80);

            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Rew_EmpRecords_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.History_GUID).HasComment("歷程GUID");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Program_Code).HasComment("程式來源");
            entity.Property(e => e.Reason_Code).HasComment("原因代碼");
            entity.Property(e => e.Remark).HasComment("備註");
            entity.Property(e => e.Reward_Date).HasComment("獎懲日期");
            entity.Property(e => e.Reward_Times).HasComment("次數");
            entity.Property(e => e.Reward_Type).HasComment("獎懲類別_序號66");
            entity.Property(e => e.Sal_Month).HasComment("薪資年月");
            entity.Property(e => e.SerNum).HasComment("檔案目錄位置");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Rew_ReasonCode>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Code).HasComment("代碼");
            entity.Property(e => e.Code_Name).HasComment("代碼說明");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_SYS_Directory>(entity =>
        {
            entity.HasKey(e => e.Directory_Code).HasName("PK_HRM_SYS_Directory");

            entity.Property(e => e.Directory_Code).HasComment("目錄代碼");
            entity.Property(e => e.Directory_Name).HasComment("目錄名稱");
            entity.Property(e => e.Language)
                .IsFixedLength()
                .HasComment("預設語系");
            entity.Property(e => e.Parent_Directory_Code).HasComment("上層目錄代碼");
            entity.Property(e => e.Seq).HasComment("順序");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日");
        });

        modelBuilder.Entity<HRMS_SYS_Language>(entity =>
        {
            entity.Property(e => e.Language_Code)
                .IsFixedLength()
                .HasComment("語系代碼");
            entity.Property(e => e.IsActive).HasComment("啟用：1 啟用 0停用");
            entity.Property(e => e.Language_Name).HasComment("語系名稱");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日");
        });

        modelBuilder.Entity<HRMS_SYS_Program>(entity =>
        {
            entity.Property(e => e.Program_Code).HasComment("程式代碼");
            entity.Property(e => e.Parent_Directory_Code).HasComment("上層目錄代碼");
            entity.Property(e => e.Program_Name).HasComment("程式名稱");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日");
        });

        modelBuilder.Entity<HRMS_SYS_Program_Function>(entity =>
        {
            entity.Property(e => e.Program_Code).HasComment("程式代碼");
            entity.Property(e => e.Fuction_Code).HasComment("功能代碼");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日");
        });

        modelBuilder.Entity<HRMS_SYS_Program_Function_Code>(entity =>
        {
            entity.HasKey(e => e.Fuction_Code).HasName("PK_HRM_SYS_Program_Function_Code");

            entity.Property(e => e.Fuction_Code).HasComment("功能代碼");
            entity.Property(e => e.Fuction_Name_EN).HasComment("功能英文名稱");
            entity.Property(e => e.Fuction_Name_TW).HasComment("功能中文名稱");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日");
        });

        modelBuilder.Entity<HRMS_SYS_Program_Language>(entity =>
        {
            entity.Property(e => e.Kind)
                .IsFixedLength()
                .HasComment("類別");
            entity.Property(e => e.Code).HasComment("目錄/程式代碼");
            entity.Property(e => e.Language_Code).HasComment("語系代碼");
            entity.Property(e => e.Name).HasComment("目錄/程式名稱");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日");
        });

        modelBuilder.Entity<HRMS_Sal_AddDedItem_AccountCode>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.AddDed_Item).HasComment("加扣項目_序號49");
            entity.Property(e => e.Main_Acc)
                .IsFixedLength()
                .HasComment("主科目");
            entity.Property(e => e.Sub_Acc)
                .IsFixedLength()
                .HasComment("子科目");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_AddDedItem_Monthly>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Sal_AddDedItem_Monthly_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Sal_Month).HasComment("薪資年月");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.AddDed_Type).HasComment("加扣類別_序號48");
            entity.Property(e => e.AddDed_Item).HasComment("加扣項目_序號49");
            entity.Property(e => e.Amount).HasComment("金額");
            entity.Property(e => e.Currency).HasComment("幣別");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_AddDedItem_Settings>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Permission_Group).HasComment("權限身分別_序號4");
            entity.Property(e => e.Salary_Type).HasComment("薪資計別_序號9");
            entity.Property(e => e.Effective_Month).HasComment("生效年月");
            entity.Property(e => e.AddDed_Type).HasComment("加扣類別_序號48");
            entity.Property(e => e.AddDed_Item).HasComment("加扣項目_序號49");
            entity.Property(e => e.Amount).HasComment("金額");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Bank_Account>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Sal_Bank_Account_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.BankNo).HasComment("帳號");
            entity.Property(e => e.Bank_Code).HasComment("銀行代碼");
            entity.Property(e => e.Create_Date).HasComment("日期");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Childcare_Subsidy>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Sal_Childcare_Subsidy_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Birthday_Child).HasComment("小孩出生日期");
            entity.Property(e => e.Month_End).HasComment("結束年月");
            entity.Property(e => e.Month_Start).HasComment("開始年月");
            entity.Property(e => e.Num_Children).HasComment("人數");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Close>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Sal_Close_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Sal_Month).HasComment("薪資年月");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Close_End).HasComment("關帳日期");
            entity.Property(e => e.Close_Status)
                .IsFixedLength()
                .HasComment("關帳狀態");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Permission_Group).HasComment("權限身分別_序號4");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Currency_Rate>(entity =>
        {
            entity.Property(e => e.Rate_Month).HasComment("匯率年月");
            entity.Property(e => e.Kind).HasComment("類別_序號51");
            entity.Property(e => e.Currency).HasComment("幣別_序號47");
            entity.Property(e => e.Exchange_Currency).HasComment("兌換幣別");
            entity.Property(e => e.Rate).HasComment("匯率");
            entity.Property(e => e.Rate_Date).HasComment("匯率日期");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Dept_SAPCostCenter_Mapping>(entity =>
        {
            entity.HasIndex(e => e.Cost_Code, "IX_HRMS_Sal_Dept_SAPCostCenter_Mapping_Cost_Code").HasFillFactor(80);

            entity.Property(e => e.Cost_Year).HasComment("年度");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Cost_Code).HasComment("成本中心代碼");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_History>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Sal_History_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.History_GUID).HasComment("歷程GUID");
            entity.Property(e => e.ActingPosition_End).HasComment("職務代理期間-END");
            entity.Property(e => e.ActingPosition_Start).HasComment("職務代理期間-Start");
            entity.Property(e => e.Currency).HasComment("幣別");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Effective_Date).HasComment("生效日期");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Expertise_Category).HasComment("專長類別_27");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Permission_Group).HasComment("權限身分別_4");
            entity.Property(e => e.Position_Grade).HasComment("職等");
            entity.Property(e => e.Position_Title).HasComment("職稱_3");
            entity.Property(e => e.Reason).HasComment("異動原因_序號52");
            entity.Property(e => e.Salary_Grade).HasComment("薪資等級-等");
            entity.Property(e => e.Salary_Level).HasComment("薪資等級-級別");
            entity.Property(e => e.Salary_Type).HasComment("薪資計別_序號9");
            entity.Property(e => e.Seq).HasComment("序號");
            entity.Property(e => e.Technical_Type).HasComment("技術工別_25");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_History_Detail>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Sal_History_Detail_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.History_GUID).HasComment("歷程GUID");
            entity.Property(e => e.Salary_Item).HasComment("薪資項_序號45");
            entity.Property(e => e.Amount).HasComment("金額");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Item_Settings>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Permission_Group).HasComment("權限身分別_序號4");
            entity.Property(e => e.Salary_Type).HasComment("薪資計別_序號9");
            entity.Property(e => e.Effective_Month).HasComment("生效年月");
            entity.Property(e => e.Salary_Item).HasComment("薪資項目_序號45");
            entity.Property(e => e.Amount).HasComment("金額");
            entity.Property(e => e.Insurance)
                .IsFixedLength()
                .HasComment("是否計算保險");
            entity.Property(e => e.Kind).HasComment("薪資類別_序號46");
            entity.Property(e => e.Salary_Days).HasComment("計薪天數");
            entity.Property(e => e.Seq).HasComment("序號_排序使用");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Leave_Calc_Maintenance>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Leave_Code).HasComment("假別");
            entity.Property(e => e.Salary_Rate).HasComment("扣薪比例");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Master>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Sal_Master_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.ActingPosition_End).HasComment("職務代理期間-END");
            entity.Property(e => e.ActingPosition_Start).HasComment("職務代理期間-Start");
            entity.Property(e => e.Currency).HasComment("幣別");
            entity.Property(e => e.Department).HasComment("部門 ");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Expertise_Category).HasComment("專長類別_27");
            entity.Property(e => e.Permission_Group).HasComment("權限身分別");
            entity.Property(e => e.Position_Grade).HasComment("職等");
            entity.Property(e => e.Position_Title).HasComment("職稱");
            entity.Property(e => e.Salary_Grade).HasComment("薪資等級-等");
            entity.Property(e => e.Salary_Level).HasComment("薪資等級-級別");
            entity.Property(e => e.Salary_Type).HasComment("薪資計別_序號9");
            entity.Property(e => e.Technical_Type).HasComment("技術工別_25");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_MasterBackup>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Sal_MasterBackup_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Sal_Month).HasComment("薪資年月");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.ActingPosition_End).HasComment("職務代理期間-END");
            entity.Property(e => e.ActingPosition_Start).HasComment("職務代理期間-Start");
            entity.Property(e => e.Currency).HasComment("幣別");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Expertise_Category).HasComment("專長類別_27");
            entity.Property(e => e.Position_Grade).HasComment("職等");
            entity.Property(e => e.Position_Title).HasComment("職稱");
            entity.Property(e => e.Salary_Grade).HasComment("薪資等級-等");
            entity.Property(e => e.Salary_Level).HasComment("薪資等級-級別");
            entity.Property(e => e.Salary_Type).HasComment("薪資計別_序號9");
            entity.Property(e => e.Technical_Type).HasComment("技術工別_25");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_MasterBackup_Detail>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Sal_MasterBackup_Detail_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Sal_Month).HasComment("薪資年月");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Salary_Item).HasComment("薪資項_序號45");
            entity.Property(e => e.Amount).HasComment("金額");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Master_Detail>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Sal_Master_Detail_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Salary_Item).HasComment("薪資項_序號45");
            entity.Property(e => e.Amount).HasComment("金額");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Monthly>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Sal_Monthly_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Sal_Month).HasComment("薪資年月");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.BankTransfer)
                .IsFixedLength()
                .HasComment("銀行轉帳");
            entity.Property(e => e.Currency).HasComment("幣別");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Lock)
                .IsFixedLength()
                .HasComment("薪資 Y.鎖定 N未鎖定");
            entity.Property(e => e.Permission_Group).HasComment("權限身分別_序號4");
            entity.Property(e => e.Salary_Type).HasComment("薪資計別_序號9");
            entity.Property(e => e.Tax).HasComment("所得稅金額_扣項");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Monthly_Detail>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Sal_Monthly_Detail_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Sal_Month).HasComment("薪資年月");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Type_Seq).HasComment("代碼的序號，存放45/42/49 等序號");
            entity.Property(e => e.AddDed_Type).HasComment("加扣類別_序號48 ");
            entity.Property(e => e.Item).HasComment("薪資項目45/加班補助類 42 /加扣項 49");
            entity.Property(e => e.Amount).HasComment("金額");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Parameter>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Seq).HasComment("序號56裡的代碼(大類)");
            entity.Property(e => e.Code).HasComment("代碼(小類)");
            entity.Property(e => e.Code_Amt).HasComment("金額或比率");
            entity.Property(e => e.Code_Name).HasComment("代碼名稱");
            entity.Property(e => e.Num_Times).HasComment("次數");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Payslip_Email>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Sal_Payslip_Email_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Email).HasComment("Email");
            entity.Property(e => e.Status)
                .IsFixedLength()
                .HasComment("啟用");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Probation_MasterBackup>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Sal_Month).HasComment("薪資年月");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.ActingPosition_End).HasComment("職務代理期間-END");
            entity.Property(e => e.ActingPosition_Start).HasComment("職務代理期間-Start");
            entity.Property(e => e.Currency).HasComment("幣別");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Expertise_Category).HasComment("專長類別_27");
            entity.Property(e => e.Permission_Group).HasComment("權限身分別");
            entity.Property(e => e.Position_Grade).HasComment("職等");
            entity.Property(e => e.Position_Title).HasComment("職稱_3");
            entity.Property(e => e.Salary_Grade).HasComment("薪資等級-等");
            entity.Property(e => e.Salary_Level).HasComment("薪資等級-級別");
            entity.Property(e => e.Salary_Type).HasComment("薪資計別_序號9");
            entity.Property(e => e.Technical_Type).HasComment("技術工別_25");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Probation_MasterBackup_Detail>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Sal_Month).HasComment("薪資年月");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Salary_Item).HasComment("薪資項_序號45");
            entity.Property(e => e.Amount).HasComment("金額");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Probation_Monthly>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Sal_Month).HasComment("薪資年月");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Probation)
                .IsFixedLength()
                .HasComment("試用期狀態識別");
            entity.Property(e => e.BankTransfer)
                .IsFixedLength()
                .HasComment("銀行轉帳");
            entity.Property(e => e.Currency).HasComment("幣別");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Lock)
                .IsFixedLength()
                .HasComment("薪資 Y.鎖定 N未鎖定");
            entity.Property(e => e.Permission_Group).HasComment("權限身分別_序號4");
            entity.Property(e => e.Salary_Type).HasComment("薪資計別_序號9");
            entity.Property(e => e.Tax).HasComment("所得稅金額_扣項");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Probation_Monthly_Detail>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Sal_Month).HasComment("薪資年月");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Probation)
                .IsFixedLength()
                .HasComment("試用期狀態識別");
            entity.Property(e => e.Type_Seq).HasComment("代碼的序號，存放2.4的45 / 42 /49 等序號");
            entity.Property(e => e.AddDed_Type).HasComment("加扣類別_序號48 ");
            entity.Property(e => e.Item).HasComment("薪資項目45/加班補助類 42 /加扣項 49");
            entity.Property(e => e.Amount).HasComment("金額");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Resign_Monthly>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Sal_Resign_Monthly_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Sal_Month).HasComment("薪資年月");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.BankTransfer)
                .IsFixedLength()
                .HasComment("銀行轉帳");
            entity.Property(e => e.Currency).HasComment("幣別");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.Lock)
                .IsFixedLength()
                .HasComment("薪資 Y.鎖定 N未鎖定");
            entity.Property(e => e.Permission_Group).HasComment("權限身分別_序號4");
            entity.Property(e => e.Salary_Type).HasComment("薪資計別_序號9");
            entity.Property(e => e.Tax).HasComment("所得稅金額_扣項");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Resign_Monthly_Detail>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Sal_Resign_Monthly_Detail_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Sal_Month).HasComment("薪資年月");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Type_Seq).HasComment("分類別，區分代碼，存放45 / 42 /49 等序號");
            entity.Property(e => e.AddDed_Type).HasComment("加扣類別_序號48 ");
            entity.Property(e => e.Item).HasComment("薪資項目45/加班補助類 42 /加扣項 49");
            entity.Property(e => e.Amount).HasComment("金額");
            entity.Property(e => e.Division).HasComment("事業部");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_SAPCostCenter>(entity =>
        {
            entity.HasIndex(e => e.Factory, "IX_HRMS_Sal_SAPCostCenter_Factory").HasFillFactor(80);

            entity.Property(e => e.Cost_Year).HasComment("年度");
            entity.Property(e => e.Company_Code).HasComment("公司代碼");
            entity.Property(e => e.Cost_Code).HasComment("成本中心代碼");
            entity.Property(e => e.Code_Name).HasComment("中文說明");
            entity.Property(e => e.Code_Name_EN).HasComment("英文說明");
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Group_Id).HasComment("成本中心群組");
            entity.Property(e => e.Kind).HasComment("功能範圍_序號50");
            entity.Property(e => e.Profit_Center).HasComment("利潤中心代碼");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_SalaryItem_AccountCode>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Salary_Item).HasComment("薪資項目_序號45");
            entity.Property(e => e.DC_Code)
                .IsFixedLength()
                .HasComment("借D/貸C");
            entity.Property(e => e.Main_Acc)
                .IsFixedLength()
                .HasComment("主科目");
            entity.Property(e => e.Sub_Acc)
                .IsFixedLength()
                .HasComment("子科目");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Tax>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Sal_Tax_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Sal_Month).HasComment("薪資年月");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Add_Total).HasComment("正項合計");
            entity.Property(e => e.Currency).HasComment("幣別");
            entity.Property(e => e.Ded_Total).HasComment("扣項合計");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Num_Dependents).HasComment("扶養人數");
            entity.Property(e => e.Rate).HasComment("當月所得稅率");
            entity.Property(e => e.Salary_Amt).HasComment("應稅薪資");
            entity.Property(e => e.Tax).HasComment("所得稅金額_等同HRMS_Sal_Monthly.Tax");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_TaxFree>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Type).HasComment("免稅類別_54");
            entity.Property(e => e.Salary_Type).HasComment("薪資計別_9");
            entity.Property(e => e.Effective_Month).HasComment("生效年月");
            entity.Property(e => e.Amount).HasComment("金額");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Tax_Number>(entity =>
        {
            entity.HasIndex(e => e.USER_GUID, "IX_HRMS_Sal_Tax_Number_USER_GUID").HasFillFactor(80);

            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Year).HasComment("年度");
            entity.Property(e => e.Employee_ID).HasComment("工號");
            entity.Property(e => e.Dependents).HasComment("扶養人數");
            entity.Property(e => e.TaxNo).HasComment("繳稅編號");
            entity.Property(e => e.USER_GUID).HasComment("員工GUID");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<HRMS_Sal_Taxbracket>(entity =>
        {
            entity.Property(e => e.Nation).HasComment("國家_10");
            entity.Property(e => e.Tax_Code).HasComment("稅碼_53");
            entity.Property(e => e.Effective_Month).HasComment("生效年月");
            entity.Property(e => e.Tax_Level).HasComment("級別");
            entity.Property(e => e.Deduction).HasComment("累進差額");
            entity.Property(e => e.Income_End).HasComment("所得淨額-迄");
            entity.Property(e => e.Income_Start).HasComment("所得淨額-起");
            entity.Property(e => e.Rate).HasComment("稅率");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        modelBuilder.Entity<IDX_HRMS_Emp_Personal_g01>(entity =>
        {
            entity.HasKey(e => new { e.ID, e.Factory, e.Employee_ID, e.upcode })
                .HasName("PK_IDX_g01")
                .IsClustered(false);

            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.upcode).IsFixedLength();
            entity.Property(e => e.BIZ_FLAG).IsFixedLength();
        });

        modelBuilder.Entity<IDX_g03b>(entity =>
        {
            entity.Property(e => e.BIZ_FLAG).IsFixedLength();
            entity.Property(e => e.upcode).IsFixedLength();
        });

        modelBuilder.Entity<IDX_g26>(entity =>
        {
            entity.Property(e => e.BIZ_FLAG).IsFixedLength();
            entity.Property(e => e.upcode).IsFixedLength();
        });

        modelBuilder.Entity<IDX_g33>(entity =>
        {
            entity.Property(e => e.BIZ_FLAG).IsFixedLength();
            entity.Property(e => e.Min_Start).IsFixedLength();
            entity.Property(e => e.upcode).IsFixedLength();
        });

        modelBuilder.Entity<IDX_g35b>(entity =>
        {
            entity.Property(e => e.BIZ_FLAG).IsFixedLength();
            entity.Property(e => e.upcode).IsFixedLength();
        });

        modelBuilder.Entity<IDX_g36>(entity =>
        {
            entity.Property(e => e.BIZ_FLAG).IsFixedLength();
            entity.Property(e => e.upcode).IsFixedLength();
        });

        modelBuilder.Entity<IDX_g37>(entity =>
        {
            entity.Property(e => e.BIZ_FLAG).IsFixedLength();
            entity.Property(e => e.upcode).IsFixedLength();
        });

        modelBuilder.Entity<IDX_g59c>(entity =>
        {
            entity.Property(e => e.BIZ_FLAG).IsFixedLength();
            entity.Property(e => e.upcode).IsFixedLength();
        });

        modelBuilder.Entity<IDX_y14>(entity =>
        {
            entity.HasIndex(e => new { e.Factory, e.Employee_ID }, "IX_IDX_y14_Covering").HasFillFactor(80);

            entity.Property(e => e.BIZ_FLAG).IsFixedLength();
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.upcode).IsFixedLength();
        });

        modelBuilder.Entity<HRMS_Sal_FinCategory>(entity =>
        {
            entity.Property(e => e.Factory).HasComment("廠別");
            entity.Property(e => e.Kind).HasComment("對應方式_序號63");
            entity.Property(e => e.Department).HasComment("部門");
            entity.Property(e => e.Code).HasComment("職稱_序號3/權限身分別_序號4");
            entity.Property(e => e.Sortcod).HasComment("薪資歸屬類別_64");
            entity.Property(e => e.Update_By).HasComment("異動者");
            entity.Property(e => e.Update_Time).HasComment("異動日期");
        });

        OnModelCreatingPartial(modelBuilder);


    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
