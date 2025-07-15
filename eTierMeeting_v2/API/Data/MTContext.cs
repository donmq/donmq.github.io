using Machine_API.Models.MT;
using Microsoft.EntityFrameworkCore;

namespace Machine_API.Data
{
    public partial class MTContext : DbContext
    {
        public virtual DbSet<Control_File_Temp> Control_File_Temp { get; set; }
        public virtual DbSet<SAP_Cost_Center_Changed_Record> SAP_Cost_Center_Changed_Record { get; set; }

        public MTContext(DbContextOptions<MTContext> options) : base(options)
        {
            Database.SetCommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Control_File_Temp>(entity =>
            {
                entity.HasKey(e => new { e.SID, e.Table_Name, e.SPEC_ID, e.Factory_ID });

                entity.Property(e => e.SID)
                    .IsFixedLength()
                    .HasComment("唯一序號");

                entity.Property(e => e.Table_Name).HasComment("資料表名稱");

                entity.Property(e => e.SPEC_ID).HasComment("拋轉程式碼");

                entity.Property(e => e.Factory_ID).HasComment("廠別");

                entity.Property(e => e.Control_Flag)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')")
                    .HasComment("拋轉碼");

                entity.Property(e => e.Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("資料表拋轉數");

                entity.Property(e => e.Table_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("資料表數");

                entity.Property(e => e.Update_By).HasComment("異動者");

                entity.Property(e => e.Update_Time)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("異動時間");
            });

            modelBuilder.Entity<SAP_Cost_Center_Changed_Record>(entity =>
            {
                entity.HasKey(e => new { e.SID, e.Company_Code, e.Main_Asset_Number, e.Asset_Subnumber })
                    .HasName("SAP_Inbound_changed_Cost_Center_from_Machine_Transfer_PK");

                entity.Property(e => e.SID).HasComment("SID");

                entity.Property(e => e.Company_Code).HasComment("Company Code");

                entity.Property(e => e.Main_Asset_Number).HasComment("Main Asset Number");

                entity.Property(e => e.Asset_Subnumber).HasComment("Asset Subnumber");

                entity.Property(e => e.Asset_Location).HasComment("Asset Location");

                entity.Property(e => e.Biz_Flag)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.Biz_Tflag)
                    .IsFixedLength()
                    .HasDefaultValueSql("('I')");

                entity.ToTable(tb => tb.HasTrigger("TRG_SAP_Cost_Center_Changed_Record_Factory_ID"));
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}