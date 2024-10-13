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

    public virtual DbSet<BaiTap> BaiTap { get; set; }

    public virtual DbSet<ChatLuongAfter> ChatLuongAfter { get; set; }

    public virtual DbSet<ChatLuongBefore> ChatLuongBefore { get; set; }

    public virtual DbSet<LoaiThuocTinh> LoaiThuocTinh { get; set; }

    public virtual DbSet<PThongTinViTriCauThu> PThongTinViTriCauThu { get; set; }

    public virtual DbSet<PThuocTinhBaiTap> PThuocTinhBaiTap { get; set; }

    public virtual DbSet<PThuocTinhSangToi> PThuocTinhSangToi { get; set; }

    public virtual DbSet<ThongTin> ThongTin { get; set; }

    public virtual DbSet<ThuocTinhChinh> ThuocTinhChinh { get; set; }

    public virtual DbSet<ViTri> ViTri { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaiTap>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<PThongTinViTriCauThu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ThongTinViTriCauThu");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
