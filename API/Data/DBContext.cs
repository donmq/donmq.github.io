
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class DBContext : DbContext
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BaiTap> BaiTap { get; set; }

    public virtual DbSet<LoaiThuocTinh> LoaiThuocTinh { get; set; }

    public virtual DbSet<P_ThongTinViTriCauThu> P_ThongTinViTriCauThu { get; set; }

    public virtual DbSet<P_ThuocTinhSangToi> P_ThuocTinhSangToi { get; set; }

    public virtual DbSet<ThongTin> ThongTin { get; set; }

    public virtual DbSet<ThuocTinhChinh> ThuocTinhChinh { get; set; }

    public virtual DbSet<ViTri> ViTri { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaiTap>(entity =>
        {
            entity.Property(e => e.ID).ValueGeneratedNever();
        });

        modelBuilder.Entity<P_ThongTinViTriCauThu>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_ThongTinViTriCauThu");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}