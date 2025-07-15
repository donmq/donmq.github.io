using Machine_API.Models.SAP;
using Microsoft.EntityFrameworkCore;

namespace Machine_API.Data
{
    public partial class SAPContext: DbContext
    {
        public virtual DbSet<Control_File> Control_File { get; set; }
        public virtual DbSet<MT_Asset> MT_Asset { get; set; }

        public SAPContext(DbContextOptions<SAPContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Control_File>(entity =>
            {
                entity.HasKey(e => new { e.SID, e.Table_Name, e.SPEC_ID, e.Factory_ID, e.System_ID, e.DB_Location });

                entity.Property(e => e.SID)
                    .IsFixedLength()
                    .HasComment("唯一序號");

                entity.Property(e => e.Table_Name).HasComment("資料表名稱");

                entity.Property(e => e.SPEC_ID).HasComment("拋轉程式碼");

                entity.Property(e => e.Factory_ID).HasComment("廠別");

                entity.Property(e => e.System_ID).HasComment("接收端系統");

                entity.Property(e => e.DB_Location).HasComment("接收端中介DB位置");

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

            modelBuilder.Entity<MT_Asset>(entity =>
            {
                entity.HasKey(e => new { e.SID, e.Owner_Fty, e.Main_Asset_Number, e.Assno_ID })
                    .HasName("PK_SAP_To_MT");

                entity.Property(e => e.Biz_Flag)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.Biz_Tflag)
                    .IsFixedLength()
                    .HasDefaultValueSql("('I')");

                entity.Property(e => e.HP_Flag)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}