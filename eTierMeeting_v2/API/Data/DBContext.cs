using Microsoft.EntityFrameworkCore;
using eTierV2_API.Models;
using System;

namespace eTierV2_API.Data
{
    public partial class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
            Database.SetCommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
        }
        public DbSet<Factory> Factory { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<RoleUser> RoleUser { get; set; }
        public DbSet<eTM_Dept_Classification> eTM_Dept_Classification { get; set; }
        public virtual DbSet<VW_DeptFromMES> VW_DeptFromMES { get; set; }
        // public virtual DbSet<MES_Dept> MES_Dept { get; set; }
        public virtual DbSet<eTM_Dept_Score_Data> eTM_Dept_Score_Data { get; set; }
        public virtual DbSet<eTM_MES_MO_Record> eTM_MES_MO_Record { get; set; }
        public virtual DbSet<eTM_MES_Quality_Defect_Data> eTM_MES_Quality_Defect_Data { get; set; }
        public virtual DbSet<eTM_Production_T1_Video> eTM_Production_T1_Video { get; set; }
        public virtual DbSet<eTM_Settings> eTM_Settings { get; set; }
        public virtual DbSet<VW_HPBasis_HPMonthlyEstimateProduceTarget_i14a> VW_HPBasis_HPMonthlyEstimateProduceTarget_i14a { get; set; }
        public virtual DbSet<eTM_Meeting_Log> eTM_Meeting_Log { get; set; }
        public virtual DbSet<eTM_Meeting_Log_Page> eTM_Meeting_Log_Page { get; set; }
        public virtual DbSet<eTM_Team_Unit> eTM_Team_Unit { get; set; }
        public virtual DbSet<eTM_MES_PT1_Summary> eTM_MES_PT1_Summary { get; set; }
        public virtual DbSet<VW_MES_4MReason> VW_MES_4MReason { get; set; }
        public virtual DbSet<VW_Production_T1_STF_Delivery_Record> VW_Production_T1_STF_Delivery_Record { get; set; }
        public virtual DbSet<VW_Production_T1_UPF_Delivery_Record> VW_Production_T1_UPF_Delivery_Record { get; set; }
        public virtual DbSet<VW_T2_Meeting_Log> VW_T2_Meeting_Log { get; set; }
        public virtual DbSet<VW_eTM_HP_Efficiency_Data> VW_eTM_HP_Efficiency_Data { get; set; }
        public virtual DbSet<eTM_MES_T1_STF_Delivery_Record> eTM_MES_T1_STF_Delivery_Record { get; set; }
        public virtual DbSet<eTM_Page_Settings> eTM_Page_Settings { get; set; }
        public virtual DbSet<eTM_Video> eTM_Video { get; set; }
        public virtual DbSet<eTM_HP_Efficiency_Data> eTM_HP_Efficiency_Data { get; set; }
        public virtual DbSet<eTM_HP_Efficiency_Data_External> eTM_HP_Efficiency_Data_External { get; set; }
        public virtual DbSet<eTM_HSE_Score_Data> eTM_HSE_Score_Data { get; set; }
        public virtual DbSet<eTM_HSE_Score_Image> eTM_HSE_Score_Image { get; set; }
        public virtual DbSet<eTM_Page_Item_Settings> eTM_Page_Item_Settings { get; set; }
        public virtual DbSet<eTM_HP_Dept_Kind> eTM_HP_Dept_Kind { get; set; }
        public virtual DbSet<eTM_HP_G01_Flag> eTM_HP_G01_Flag { get; set; }
        public virtual DbSet<eTM_HP_Efficiency_Data_Subcon> eTM_HP_Efficiency_Data_Subcon { get; set; }
        public virtual DbSet<HP_Production_Line_ie21> VW_HPBasis_HP_Production_Line_ie21 { get; set; }
        public virtual DbSet<VW_LineGroup> VW_LineGroup { get; set; }
        public virtual DbSet<eTM_Video_Play_Log> eTM_Video_Play_Log { get; set; }
        public virtual DbSet<eTM_T2_Meeting_Seeting> eTM_T2_Meeting_Seeting { get; set; }
        public virtual DbSet<SM_Basic_Data> SM_Basic_Data { get; set; }
        public virtual DbSet<SM_Basic_Data_ColDesc> SM_Basic_Data_ColDesc { get; set; }
        public virtual DbSet<VW_Prod_T1_CTB_Delivery> VW_Prod_T1_CTB_Delivery { get; set; }
        public virtual DbSet<VW_Efficiency_ByBrand> VW_Efficiency_ByBrand { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleUser>().HasKey(x => new { x.user_account, x.role_unique });
            modelBuilder.Entity<eTM_Dept_Classification>().HasKey(x => new { x.Class_Kind, x.Dept_ID });
            modelBuilder.Entity<VW_DeptFromMES>().HasNoKey();
            modelBuilder.Entity<VW_HPBasis_HPMonthlyEstimateProduceTarget_i14a>().HasNoKey();

            modelBuilder.Entity<eTM_Dept_Score_Data>(entity =>
            {
                entity.HasKey(e => new { e.Dept_ID, e.Data_Date });

                entity.Property(e => e.Dept_ID).IsUnicode(false);

                entity.Property(e => e.Insert_By)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Update_By)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<eTM_MES_MO_Record>(entity =>
            {
                entity.HasKey(e => new { e.Data_Date, e.MO_No, e.MO_Seq });

                entity.Property(e => e.MO_No).IsUnicode(false);

                entity.Property(e => e.MO_Seq).IsUnicode(false);

                entity.Property(e => e.Color_No).IsUnicode(false);

                entity.Property(e => e.Dept_ID).IsUnicode(false);

                entity.Property(e => e.Insert_By)
                    .IsUnicode(false)
                    .IsFixedLength();



                entity.Property(e => e.Nation).IsUnicode(false);

                entity.Property(e => e.Style_Name).IsUnicode(false);

                entity.Property(e => e.Style_No).IsUnicode(false);

                entity.Property(e => e.Update_By)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<eTM_MES_Quality_Defect_Data>(entity =>
            {
                entity.HasKey(e => new { e.Data_Kind, e.Dept_ID, e.Data_Date, e.Reason_ID });

                entity.Property(e => e.Data_Kind)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Dept_ID).IsUnicode(false);

                entity.Property(e => e.Reason_ID)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Image_Path).IsUnicode(false);

                entity.Property(e => e.Insert_By)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Update_By)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<eTM_Production_T1_Video>(entity =>
            {
                entity.HasKey(e => new { e.Video_Kind, e.Dept_ID, e.Play_Date, e.Seq });

                entity.Property(e => e.Video_Kind).IsUnicode(false);

                entity.Property(e => e.Dept_ID).IsUnicode(false);

                entity.Property(e => e.Insert_By)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Update_By)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Video_Icon_Path).IsUnicode(false);

                entity.Property(e => e.Video_Path).IsUnicode(false);

                entity.Property(e => e.Video_Remark).IsUnicode(false);

                entity.Property(e => e.Video_Title_CHT).IsUnicode(false);

                entity.Property(e => e.Video_Title_ENG).IsUnicode(false);
            });

            modelBuilder.Entity<eTM_Settings>(entity =>
            {
                entity.HasIndex(e => e.Key)
                    .HasDatabaseName("IX_eTM_Settings")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Insert_By)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Key)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Update_By)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<eTM_Meeting_Log>(entity =>
            {
                entity.HasIndex(e => new { e.TU_ID, e.Data_Date, e.Record_Status })
                    .HasFillFactor((byte)80);

                entity.Property(e => e.Record_ID).ValueGeneratedNever();

                entity.Property(e => e.Insert_By)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Record_Status)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.TU_ID)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Update_By)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<eTM_Meeting_Log_Page>(entity =>
          {
              entity.Property(e => e.Record_ID).ValueGeneratedNever();

              entity.Property(e => e.Center_Level).IsUnicode(false);

              entity.Property(e => e.Class_Level).IsUnicode(false);

              entity.Property(e => e.Page_Name).IsUnicode(false);

              entity.Property(e => e.TU_ID)
                  .IsUnicode(false)
                  .IsFixedLength();

              entity.Property(e => e.Tier_Level)
                  .IsUnicode(false)
                  .IsFixedLength();
          });

            modelBuilder.Entity<VW_T2_Meeting_Log>(entity =>
            {
                entity.HasNoKey();
                entity.Property(e => e.Level).IsUnicode(false);
            });
            modelBuilder.Entity<eTM_Team_Unit>(entity =>
            {
                entity.HasKey(e => new { e.TU_ID });
                entity.Property(e => e.TU_ID).IsFixedLength();
                entity.Property(e => e.TU_Name).IsUnicode(false);
                entity.Property(e => e.Center_Level).IsUnicode(false);
                entity.Property(e => e.Tier_Level).IsFixedLength();
                entity.Property(e => e.TU_Code).IsFixedLength();
                entity.Property(e => e.Class1_Level).IsUnicode(false);
                entity.Property(e => e.Class2_Level).IsUnicode(false);
                entity.Property(e => e.Insert_By).IsFixedLength();
                entity.Property(e => e.Update_By).IsFixedLength();
            });

            modelBuilder.Entity<eTM_MES_PT1_Summary>(entity =>
            {
                entity.HasKey(e => new { e.Dept_ID, e.Data_Date, e.Reason_Code, e.In_Ex, e.Action_Code });

                entity.Property(e => e.Dept_ID)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Reason_Code).IsUnicode(false);

                entity.Property(e => e.In_Ex).IsUnicode(false);

                entity.Property(e => e.Action_Code).IsUnicode(false);

                entity.Property(e => e.Insert_By)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Update_By)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<VW_MES_4MReason>().HasNoKey();

            modelBuilder.Entity<VW_Production_T1_STF_Delivery_Record>().HasNoKey();

            modelBuilder.Entity<VW_Production_T1_UPF_Delivery_Record>().HasNoKey();

            modelBuilder.Entity<eTM_MES_T1_STF_Delivery_Record>(entity =>
            {
                entity.HasKey(e => new { e.Update_Date, e.MO_No, e.MO_Seq, e.Line_ID_STF });
            });

            modelBuilder.Entity<eTM_Page_Settings>(entity =>
            {
                entity.HasKey(e => new { e.Center_Level, e.Tier_Level, e.Class_Level, e.Page_Name })
                    .HasName("PK_Setting_Page_Enable");

                entity.Property(e => e.Center_Level).IsUnicode(false);

                entity.Property(e => e.Tier_Level)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Class_Level).IsUnicode(false);

                entity.Property(e => e.Page_Name).IsUnicode(false);

                entity.Property(e => e.Link).IsUnicode(false);
            });

            modelBuilder.Entity<eTM_Video>(entity =>
            {
                entity.HasKey(e => new { e.Video_Kind, e.TU_ID, e.Play_Date, e.Seq });

                entity.Property(e => e.Video_Kind).IsUnicode(false);

                entity.Property(e => e.TU_ID).IsUnicode(false);

                entity.Property(e => e.Insert_By)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Update_By)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Video_Icon_Path).IsUnicode(false);

                entity.Property(e => e.Video_Path).IsUnicode(false);

                entity.Property(e => e.Video_Remark).IsUnicode(false);

                entity.Property(e => e.Video_Title_CHT).IsUnicode(false);

                entity.Property(e => e.Video_Title_ENG).IsUnicode(false);
            });
            modelBuilder.Entity<eTM_Page_Item_Settings>(entity =>
           {
               entity.HasKey(e => new { e.Center_Level, e.Tier_Level, e.Class_Level, e.Page_Name, e.Item_ID });
           });
            modelBuilder.Entity<eTM_HP_Efficiency_Data>(entity =>
            {
                entity.HasKey(e => new { e.Data_Date, e.Factory_ID, e.Dept_ID });

                entity.Property(e => e.Factory_ID).IsUnicode(false);

                entity.Property(e => e.Dept_ID).IsUnicode(false);
            });

            modelBuilder.Entity<eTM_HSE_Score_Data>(entity =>
            {
                entity.Property(e => e.Center_Level).IsUnicode(false);

                entity.Property(e => e.Class_Level).IsUnicode(false);

                entity.Property(e => e.Item_ID).IsUnicode(false);

                entity.Property(e => e.TU_Code)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Tier_Level)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Update_By).IsUnicode(false);
            });

            modelBuilder.Entity<eTM_Page_Item_Settings>(entity =>
            {
                entity.HasKey(e => new { e.Center_Level, e.Tier_Level, e.Class_Level, e.Page_Name, e.Item_ID })
                    .HasName("PK_Page_Item_Settings");

                entity.Property(e => e.Center_Level).IsUnicode(false);

                entity.Property(e => e.Tier_Level)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Class_Level).IsUnicode(false);

                entity.Property(e => e.Page_Name).IsUnicode(false);

                entity.Property(e => e.Item_ID).IsUnicode(false);

                entity.Property(e => e.Update_By).IsUnicode(false);
            });

            modelBuilder.Entity<eTM_HP_Dept_Kind>(entity =>
            {
                entity.HasKey(e => new { e.Factory_ID, e.Data_Year, e.Data_Month, e.Dept_ID });
            });

            modelBuilder.Entity<eTM_HP_G01_Flag>().HasNoKey();

            modelBuilder.Entity<HP_Production_Line_ie21>().HasNoKey();

            modelBuilder.Entity<eTM_HSE_Score_Image>().HasKey(x => x.HSE_Image_ID);

            modelBuilder.Entity<eTM_Video_Play_Log>().HasKey(x => x.Record_ID);
            modelBuilder.Entity<VW_eTM_HP_Efficiency_Data>(entity =>
                        {
                            entity.HasNoKey();
                            entity.Property(e => e.Factory_ID).IsUnicode(false);
                            entity.Property(e => e.Dept_ID).IsUnicode(false);
                        });

            modelBuilder.Entity<VW_LineGroup>(entity =>
            {
                entity.ToView("VW_LineGroup");

                entity.Property(e => e.Building)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Dept_ID)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Factory_ID)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Kind)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Line_Group)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<eTM_T2_Meeting_Seeting>(entity =>
            {
                entity.HasKey(e => new { e.Meeting_Date, e.TU_ID });

                entity.Property(e => e.Update_By).IsFixedLength();
            });

            modelBuilder.Entity<SM_Basic_Data>(entity =>
            {
                entity.HasKey(e => new { e.UID });
                entity.Property(e => e.UID).HasDefaultValueSql("('(newsequentialid())')");
                entity.Property(e => e.Insert_By).IsUnicode(false);
                entity.Property(e => e.Update_By).IsUnicode(false);
            });
            modelBuilder.Entity<SM_Basic_Data_ColDesc>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<VW_Prod_T1_CTB_Delivery>(entity =>
            {
                entity.ToTable("VW_Prod_T1_CTB_Delivery");
                entity.HasNoKey();
            });
            modelBuilder.Entity<eTM_HP_Efficiency_Data_Subcon>(entity =>
            {
                entity.HasKey(e => new {e.Data_Date, e.Factory_ID, e.Operation});
                entity.Property(e => e.Factory_ID).IsUnicode(false);
                entity.Property(e => e.Operation).IsUnicode(false);
            });
            modelBuilder.Entity<VW_Efficiency_ByBrand>(entity =>
            {
                entity.HasNoKey();
                entity.Property(e => e.Factory_ID).IsUnicode(false);
                entity.Property(e => e.Dept_ID).IsUnicode(false);
                entity.Property(e => e.Kind).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}