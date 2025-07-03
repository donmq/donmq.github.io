using eTierV2_API.Models;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API.Data
{
    public class MesDataContext : DbContext
    {
        public MesDataContext(DbContextOptions<MesDataContext> options) : base(options) { }
        public DbSet<MES_Dept_Target> MES_Dept_Target { get; set; }
        public DbSet<MES_Dept_Plan> MES_Dept_Plan { get; set; }
        public virtual DbSet<MES_Org> MES_Org { get; set; }
        public virtual DbSet<MES_Dept> MES_Dept { get; set; }
        public virtual DbSet<MES_Defect> MES_Defect { get; set; }
        public virtual DbSet<VW_KanBan_POList_V2> VW_KanBan_POList_V2 { get; set; }
        public virtual DbSet<MES_IPQC_Defect> MES_IPQC_Defect { get; set; }
        public virtual DbSet<MES_Line> MES_Line { get; set; }
        public virtual DbSet<VW_MES_Org_Mapping> VW_MES_Org_Mapping { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VW_MES_Org_Mapping>(entity =>
                     {
                         entity.ToView("VW_MES_Org_Mapping");

                         entity.Property(e => e.ASY).IsUnicode(false);

                         entity.Property(e => e.Line_ID).IsUnicode(false);

                         entity.Property(e => e.Line_ID_2).IsUnicode(false);

                         entity.Property(e => e.Line_Sname).IsUnicode(false);

                         entity.Property(e => e.PDC_ID)
                             .IsUnicode(false)
                             .IsFixedLength(true);

                         entity.Property(e => e.PRE).IsUnicode(false);

                         entity.Property(e => e.STC).IsUnicode(false);

                         entity.Property(e => e.STF).IsUnicode(false);
                         entity.HasNoKey();
                     });
            modelBuilder.Entity<MES_Dept_Target>().HasKey(x => new { x.Factory_ID, x.Year_Target, x.Month_Target, x.Dept_ID });

            modelBuilder.Entity<MES_Dept_Plan>().HasKey(x => new { x.Factory_ID, x.Dept_ID, x.Plan_Date });

            modelBuilder.Entity<MES_Defect>(entity =>
            {
                entity.HasKey(e => new { e.Def_ID, e.Factory_ID });

                entity.HasComment("QC缺點代碼檔");

                entity.Property(e => e.Def_ID)
                    .IsUnicode(false)
                    .HasComment("缺失編號");

                entity.Property(e => e.Factory_ID)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('C')")
                    .HasComment("廠別");

                entity.Property(e => e.Def_Desc)
                    .IsUnicode(false)
                    .HasComment("缺失英文說明");

                entity.Property(e => e.Def_DescVN).HasComment("缺失越文說明");

                entity.Property(e => e.Def_DescZW).HasComment("缺失中文說明");

                entity.Property(e => e.Def_Sname).HasComment("缺失簡寫英文");

                entity.Property(e => e.Share_Msg).HasComment("分享訊息");

                entity.Property(e => e.Sort).HasDefaultValueSql("((99))");

                entity.Property(e => e.Update_Time)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("異動時間");

                entity.Property(e => e.Updated_By)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('eAI')")
                    .HasComment("異動者");
            });

            modelBuilder.Entity<MES_IPQC_Defect>(entity =>
            {
                entity.HasKey(e => new { e.Def_Id, e.Factory_ID, e.Brand_Code, e.Parent_ID, e.Def_Code });

                entity.Property(e => e.Def_Id)
                    .HasComment("異常代碼")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Factory_ID)
                    .IsUnicode(false)
                    .HasComment("廠別");

                entity.Property(e => e.Brand_Code)
                    .IsUnicode(false)
                    .HasComment("品牌代碼");

                entity.Property(e => e.Parent_ID).HasComment("不良原因大類");

                entity.Property(e => e.Def_Code)
                    .IsUnicode(false)
                    .HasComment("不良原因細項");

                entity.Property(e => e.Def_Name).HasComment("不良代碼英文說明");

                entity.Property(e => e.Def_Name_Location).HasComment("不良代碼越文說明");

                entity.Property(e => e.Def_Name_ZW).HasComment("不良代碼中文說明");

                entity.Property(e => e.IsEnable)
                    .HasDefaultValueSql("((1))")
                    .HasComment("是否有效(1/0)");

                entity.Property(e => e.Update_Time).HasComment("最後更新時間");

                entity.Property(e => e.Updated_By)
                    .IsUnicode(false)
                    .HasComment("最後更新者");
            });

            modelBuilder.Entity<MES_Dept>(entity =>
            {
                entity.HasKey(e => new { e.Dept_ID, e.Factory_ID });

                entity.HasComment("生產部門代碼檔");

                entity.Property(e => e.Dept_ID)
                    .IsUnicode(false)
                    .HasComment("生產部門別");

                entity.Property(e => e.Factory_ID)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('C')")
                    .HasComment("廠別");

                entity.Property(e => e.Dept_Desc)
                    .IsUnicode(false)
                    .HasComment("部門說明英文");

                entity.Property(e => e.Dept_DescVN).HasComment("部門說明越文");

                entity.Property(e => e.Dept_DescZW).HasComment("部門說明中文");

                entity.Property(e => e.Dept_Sname)
                    .IsUnicode(false)
                    .HasComment("部門簡稱英文");

                entity.Property(e => e.Dept_Type)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('P')")
                    .HasComment("部門類別");

                entity.Property(e => e.Lunch_Order)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('1')")
                    .HasComment("午休時間");

                entity.Property(e => e.Need_Andon)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')")
                    .HasComment("是否檢視安東");

                entity.Property(e => e.Need_Bulletin)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')")
                    .HasComment("是否顯示在KB");

                entity.Property(e => e.Need_Camera)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')")
                    .HasComment("是否拍照");

                entity.Property(e => e.PS_ID)
                    .IsUnicode(false)
                    .HasComment("所屬工序");

                entity.Property(e => e.Update_Time)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("異動時間");

                entity.Property(e => e.Updated_By)
                    .IsUnicode(false)
                    .HasComment("異動者");

                entity.Property(e => e.Work_Center)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('5')")
                    .HasComment("工段類別");
            });

            modelBuilder.Entity<MES_Org>(entity =>
            {
                entity.HasKey(e => new { e.PDC_ID, e.Line_ID, e.Dept_ID, e.Factory_ID });

                entity.HasComment("廠、生產線、生產部門結構關係檔");

                entity.HasIndex(e => e.Dept_ID);

                entity.HasIndex(e => e.Line_ID);

                entity.HasIndex(e => new { e.PDC_ID, e.Building, e.Line_ID })
                    .HasDatabaseName("IX_MES_Org");

                entity.Property(e => e.PDC_ID)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("生產部");

                entity.Property(e => e.Line_ID)
                    .IsUnicode(false)
                    .HasComment("生產線別");

                entity.Property(e => e.Dept_ID)
                    .IsUnicode(false)
                    .HasComment("生產部門");

                entity.Property(e => e.Factory_ID)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('C')")
                    .HasComment("廠別");

                entity.Property(e => e.Block)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('A')")
                    .HasComment("Block");

                entity.Property(e => e.Building).HasComment("棟別(屬性)");

                entity.Property(e => e.HP_Dept_ID)
                    .IsUnicode(false)
                    .HasComment("HP的對應部門")
                    .HasComputedColumnSql("([Dept_ID])");

                entity.Property(e => e.IsAGV)
                    .HasDefaultValueSql("((0))")
                    .HasComment("是否可以使用AGV");

                entity.Property(e => e.IsT1T3)
                    .HasDefaultValueSql("((0))")
                    .HasComment("是否為T1T3使用的線別");

                entity.Property(e => e.Line_ID_2)
                    .IsUnicode(false)
                    .HasComment("中線")
                    .HasComputedColumnSql("(case when len([Line_ID])=(3) AND TRY_CAST(substring([Line_ID],(2),(1)) AS [int]) IS NULL then [Line_ID] else case when len([Line_ID])=(3) AND TRY_CAST(substring([Line_ID],(3),(1)) AS [int]) IS NULL then substring([Line_ID],(1),len([Line_ID])-(1)) else [Line_ID] end end)");

                entity.Property(e => e.Line_Seq)
                    .HasDefaultValueSql("((20))")
                    .HasComment("線別排序");

                entity.Property(e => e.Status)
                    .HasDefaultValueSql("((1))")
                    .HasComment("狀態");

                entity.Property(e => e.Update_Time)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("異動時間");

                entity.Property(e => e.Updated_By)
                    .IsUnicode(false)
                    .HasComment("異動者");
            });

            modelBuilder.Entity<MES_Line>(entity =>
            {
                entity.HasKey(e => new { e.Line_ID, e.Factory_ID });

                entity.HasComment("生產線代碼檔");

                entity.Property(e => e.Line_ID)
                    .IsUnicode(false)
                    .HasComment("生產線別");

                entity.Property(e => e.Factory_ID)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('C')")
                    .IsFixedLength(true)
                    .HasComment("廠別");

                entity.Property(e => e.Line_AB)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('M')")
                    .IsFixedLength(true);

                entity.Property(e => e.Line_Desc)
                    .IsUnicode(false)
                    .HasComment("線別名稱英文");

                entity.Property(e => e.Line_DescVN).HasComment("線別名稱越文");

                entity.Property(e => e.Line_DescZW).HasComment("線別名稱中文");

                entity.Property(e => e.Line_Model)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('M')");

                entity.Property(e => e.Line_Sname)
                    .IsUnicode(false)
                    .HasComment("線別簡稱英文");

                entity.Property(e => e.Update_Time)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("異動時間");

                entity.Property(e => e.Updated_By)
                    .IsUnicode(false)
                    .HasComment("異動者");
            });

            modelBuilder.Entity<VW_KanBan_POList_V2>().HasNoKey();
        }
    }
}