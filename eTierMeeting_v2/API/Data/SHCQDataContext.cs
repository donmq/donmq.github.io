using eTierV2_API.Models;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API.Data
{
    public partial class SHCQDataContext : DbContext
    {
        public virtual DbSet<FRI_BA_Defect> FRI_BA_Defect { get; set; }
        public SHCQDataContext(DbContextOptions<SHCQDataContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FRI_BA_Defect>(entity =>
            {
                entity.HasKey(e => new { e.Factory_ID, e.BA_Defect_ID });

                entity.HasComment("BA成品缺失檔主檔");

                entity.Property(e => e.Factory_ID)
                    .IsUnicode(false)
                    .HasComment("廠別");

                entity.Property(e => e.BA_Defect_ID)
                    .IsUnicode(false)
                    .HasComment("BA缺失代碼");

                entity.Property(e => e.BA_Defect_Desc).HasComment("BA缺失描述");

                entity.Property(e => e.Update_Time)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("異動時間");

                entity.Property(e => e.Updated_By)
                    .IsUnicode(false)
                    .HasComment("異動者");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}