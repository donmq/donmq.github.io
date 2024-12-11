// Generated at 12/7/2024, 4:03:00 PM
using Microsoft.EntityFrameworkCore;
using API.Models;
namespace API.Data
{
    public partial class DBContext : DbContext
    {
        public virtual DbSet<Attributes> Attributes { get; set; }
        public virtual DbSet<ExerciseAttributes> ExerciseAttributes { get; set; }
        public virtual DbSet<Exercises> Exercises { get; set; }
        public virtual DbSet<Information> Information { get; set; }
        public virtual DbSet<MainAttributes> MainAttributes { get; set; }
        public virtual DbSet<PlayerValue> PlayerValue { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<PositionInformation> PositionInformation { get; set; }
        public virtual DbSet<QualityAfter> QualityAfter { get; set; }
        public virtual DbSet<QualityBefore> QualityBefore { get; set; }
        public virtual DbSet<TypeAttributes> TypeAttributes { get; set; }
        public DBContext()
        {
        }
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
            Database.SetCommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Attributes>(entity =>
            {
                entity.HasKey(e => new {e.ID});
            });
            builder.Entity<ExerciseAttributes>(entity =>
            {
                entity.HasKey(e => new {e.AttributeID, e.ExerciseID});
            });
            builder.Entity<Exercises>(entity =>
            {
                entity.HasKey(e => new {e.ID});
                entity.Property(e => e.Class).IsUnicode(false);
            });
            builder.Entity<Information>(entity =>
            {
                entity.HasKey(e => new {e.ID});
            });
            builder.Entity<MainAttributes>(entity =>
            {
                entity.HasKey(e => new {e.ID});
            });
            builder.Entity<PlayerValue>(entity =>
            {
                entity.HasNoKey();
            });
            builder.Entity<Position>(entity =>
            {
                entity.HasKey(e => new {e.ID});
            });
            builder.Entity<PositionInformation>(entity =>
            {
                entity.HasKey(e => new {e.InforID, e.PositionID});
            });
            builder.Entity<QualityAfter>(entity =>
            {
                entity.HasKey(e => new {e.PlanID, e.ID, e.InforID});
            });
            
            builder.Entity<QualityBefore>(entity =>
            {
                entity.HasKey(e => new {e.InforID});
            });
            builder.Entity<TypeAttributes>(entity =>
            {
                entity.HasNoKey();
            });
            OnModelCreatingPartial(builder);
        }
        partial void OnModelCreatingPartial(ModelBuilder builder);
    }
}