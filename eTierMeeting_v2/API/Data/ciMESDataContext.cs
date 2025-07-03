using Microsoft.EntityFrameworkCore;
using API.Models;
using System;
namespace API.Data
{
    public partial class ciMESDataContext : DbContext
    {
        public ciMESDataContext(DbContextOptions<ciMESDataContext> options) : base(options)
        {
            Database.SetCommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
        }
        public virtual DbSet<CST_WorkCenter_Plan> CST_WorkCenter_Plan { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CST_WorkCenter_Plan>(entity =>
            {
                entity.HasKey(e => new {e.Dept_ID, e.OPER, e.Work_Date});
                entity.Property(e => e.UTN_Total_Minute).HasDefaultValueSql("('((0))')");
                entity.Property(e => e.Tag).HasDefaultValueSql("('((0))')");
                entity.Property(e => e.Update_Time).HasDefaultValueSql("('(getdate())')");
            });
            OnModelCreatingPartial(builder);
        }
        partial void OnModelCreatingPartial(ModelBuilder builder);
    }
}