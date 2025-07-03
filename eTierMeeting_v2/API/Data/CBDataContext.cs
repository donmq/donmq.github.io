using Microsoft.EntityFrameworkCore;
using eTierV2_API.Models;

namespace eTierV2_API.Data
{
    public class CBDataContext : DbContext
    {
        public CBDataContext(DbContextOptions<CBDataContext> options) : base(options) { }
        public DbSet<Factory> Factory { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<RoleUser> RoleUser { get; set; }
        public DbSet<eTM_Dept_Classification> eTM_Dept_Classification { get; set; }
        public DbSet<VW_DeptFromMES> VW_DeptFromMES { get; set; }
        public DbSet<VW_HPBasis_HPMonthlyEstimateProduceTarget_i14a> VW_HPBasis_HPMonthlyEstimateProduceTarget_i14a { get; set; }
        public DbSet<eTM_Production_T1_Video> eTM_Production_T1_Video { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleUser>().HasKey(x => new { x.user_account, x.role_unique });
            modelBuilder.Entity<eTM_Dept_Classification>().HasKey(x => new { x.Class_Kind, x.Dept_ID });
            modelBuilder.Entity<VW_DeptFromMES>().HasNoKey();
            modelBuilder.Entity<VW_HPBasis_HPMonthlyEstimateProduceTarget_i14a>().HasNoKey();
            modelBuilder.Entity<eTM_Production_T1_Video>().HasKey(x => new { x.Video_Kind, x.Dept_ID, x.Play_Date, x.Seq });

        }

        // partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}