using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Data
{
    public partial class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
            Database.SetCommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
        }

        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<AreaLang> AreaLang { get; set; }
        public virtual DbSet<BuildLang> BuildLang { get; set; }
        public virtual DbSet<Building> Building { get; set; }
        public virtual DbSet<CatLang> CatLang { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<CommentArchive> CommentArchive { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<CountAll> CountAll { get; set; }
        public virtual DbSet<CountArea> CountArea { get; set; }
        public virtual DbSet<CountBuilding> CountBuilding { get; set; }
        public virtual DbSet<CountDepartment> CountDepartment { get; set; }
        public virtual DbSet<CountPart> CountPart { get; set; }
        public virtual DbSet<DatePickerManager> DatePickerManager { get; set; }
        public virtual DbSet<DefaultSetting> DefaultSetting { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<DetpLang> DetpLang { get; set; }
        public virtual DbSet<EmailContent> EmailContent { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }
        public virtual DbSet<GroupBase> GroupBase { get; set; }
        public virtual DbSet<GroupLang> GroupLang { get; set; }
        public virtual DbSet<HistoryEmp> HistoryEmp { get; set; }
        public virtual DbSet<Holiday> Holiday { get; set; }
        public virtual DbSet<LeaveData> LeaveData { get; set; }
        public virtual DbSet<LeaveLog> LeaveLog { get; set; }
        public virtual DbSet<LoginDetect> LoginDetect { get; set; }
        public virtual DbSet<Part> Part { get; set; }
        public virtual DbSet<PartLang> PartLang { get; set; }
        public virtual DbSet<PosLang> PosLang { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<ReportData> ReportData { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Roles_User> Roles_User { get; set; }
        public virtual DbSet<SetApproveGroupBase> SetApproveGroupBase { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<V_Employee> V_Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>(entity =>
            {
                entity.Property(e => e.AreaCode).IsUnicode(false);

                entity.Property(e => e.AreaSym).IsUnicode(false);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Areas)
                    .HasForeignKey(d => d.CompanyID)
                    .HasConstraintName("FK__Area__CompanyID__5535A963");
            });

            modelBuilder.Entity<AreaLang>(entity =>
            {
                entity.Property(e => e.LanguageID).IsUnicode(false);

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.AreaLangs)
                    .HasForeignKey(d => d.AreaID)
                    .HasConstraintName("FK__AreaLang__AreaID__5629CD9C");
            });

            modelBuilder.Entity<BuildLang>(entity =>
            {
                entity.Property(e => e.LanguageID).IsUnicode(false);

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.BuildLangs)
                    .HasForeignKey(d => d.BuildingID)
                    .HasConstraintName("FK__BuildLang__Build__5812160E");
            });

            modelBuilder.Entity<Building>(entity =>
            {
                entity.Property(e => e.BuildingCode).IsUnicode(false);

                entity.Property(e => e.BuildingSym).IsUnicode(false);

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.Buildings)
                    .HasForeignKey(d => d.AreaID)
                    .HasConstraintName("FK__Building__AreaID__571DF1D5");
            });

            modelBuilder.Entity<CatLang>(entity =>
            {
                entity.HasKey(e => e.CateLangID)
                    .HasName("PK__CatLang__27659C65D8907A0F");

                entity.Property(e => e.LanguageID).IsUnicode(false);

                entity.HasOne(d => d.Cate)
                    .WithMany(p => p.CatLangs)
                    .HasForeignKey(d => d.CateID)
                    .HasConstraintName("FK__CatLang__CateID__59063A47");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CateID)
                    .HasName("PK__Category__27638D74E294E70D");

                entity.Property(e => e.CateSym).IsUnicode(false);

                entity.Property(e => e.Visible).HasDefaultValueSql("('1')");
            });

            modelBuilder.Entity<CommentArchive>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.CommentArchives)
                    .HasForeignKey(d => d.UserID)
                    .HasConstraintName("FK_CommentArchive_Users");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.CompanySym).IsUnicode(false);
            });

            modelBuilder.Entity<CountAll>(entity =>
            {
                entity.HasKey(e => e.Count_ID)
                    .HasName("PK__CountAll__0301077C49C25E14");

                entity.Property(e => e.Count_Time).IsUnicode(false);

                entity.HasOne(d => d.Count_Com)
                    .WithMany(p => p.CountAlls)
                    .HasForeignKey(d => d.Count_ComID)
                    .HasConstraintName("FK__CountAll__Count___5AEE82B9");
            });

            modelBuilder.Entity<CountArea>(entity =>
            {
                entity.HasKey(e => e.Count_ID)
                    .HasName("PK__CountAre__0301077C6B7EEA9B");

                entity.Property(e => e.Count_Time).IsUnicode(false);

                entity.HasOne(d => d.Count_Area)
                    .WithMany(p => p.CountAreas)
                    .HasForeignKey(d => d.Count_AreaID)
                    .HasConstraintName("FK__CountArea__Count__5BE2A6F2");
            });

            modelBuilder.Entity<CountBuilding>(entity =>
            {
                entity.HasKey(e => e.Count_ID)
                    .HasName("PK__CountBui__0301077C0B3DFC24");

                entity.Property(e => e.Count_Time).IsUnicode(false);

                entity.HasOne(d => d.Count_Build)
                    .WithMany(p => p.CountBuildings)
                    .HasForeignKey(d => d.Count_BuildID)
                    .HasConstraintName("FK__CountBuil__Count__5CD6CB2B");
            });

            modelBuilder.Entity<CountDepartment>(entity =>
            {
                entity.HasKey(e => e.Count_ID)
                    .HasName("PK__CountDep__0301077C2E4C7A01");

                entity.Property(e => e.Count_Time).IsUnicode(false);

                entity.HasOne(d => d.Count_Dept)
                    .WithMany(p => p.CountDepartments)
                    .HasForeignKey(d => d.Count_DeptID)
                    .HasConstraintName("FK__CountDepa__Count__5DCAEF64");
            });

            modelBuilder.Entity<CountPart>(entity =>
            {
                entity.HasKey(e => e.Count_ID)
                    .HasName("PK__CountPar__0301077CD11245F9");

                entity.Property(e => e.Count_Time).IsUnicode(false);

                entity.HasOne(d => d.Count_Part)
                    .WithMany(p => p.CountParts)
                    .HasForeignKey(d => d.Count_PartID)
                    .HasConstraintName("FK__CountPart__Count__5EBF139D");
            });

            modelBuilder.Entity<DatePickerManager>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.DatePickerManagers)
                    .HasForeignKey(d => d.UserID)
                    .HasConstraintName("FK__DatePicke__UserI__5CD6CB2B");
            });

            modelBuilder.Entity<DefaultSetting>(entity =>
            {
                entity.Property(e => e.KeySett).IsUnicode(false);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.DeptID)
                    .HasName("PK__Departme__0148818E6343BB34");

                entity.Property(e => e.DeptCode).IsUnicode(false);

                entity.Property(e => e.DeptSym).IsUnicode(false);

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.Departments)
                    .HasForeignKey(d => d.AreaID)
                    .HasConstraintName("FK__Departmen__AreaI__60A75C0F");

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.Departments)
                    .HasForeignKey(d => d.BuildingID)
                    .HasConstraintName("FK__Departmen__Build__619B8048");
            });

            modelBuilder.Entity<DetpLang>(entity =>
            {
                entity.HasKey(e => e.DeptLangID)
                    .HasName("PK__DetpLang__5191E8997898739E");

                entity.Property(e => e.LanguageID).IsUnicode(false);

                entity.HasOne(d => d.Dept)
                    .WithMany(p => p.DetpLangs)
                    .HasForeignKey(d => d.DeptID)
                    .HasConstraintName("FK__DetpLang__DeptID__628FA481");
            });

            modelBuilder.Entity<EmailContent>(entity =>
            {
                entity.Property(e => e.KeyCont).IsUnicode(false);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmpID)
                    .HasName("PK__Employee__AF2DBA798368C392");

                entity.Property(e => e.EmpNumber).IsUnicode(false);

                entity.HasOne(d => d.GB)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.GBID)
                    .HasConstraintName("FK__Employee__GBID__7E37BEF6");

                entity.HasOne(d => d.Part)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.PartID)
                    .HasConstraintName("FK_Employee_Part");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.PositionID)
                    .HasConstraintName("FK__Employee__Positi__7F2BE32F");
            });

            modelBuilder.Entity<GroupBase>(entity =>
            {
                entity.HasKey(e => e.GBID)
                    .HasName("PK__GroupBas__53FA164FCF7BA6B7");

                entity.Property(e => e.BaseSym).IsUnicode(false);
            });

            modelBuilder.Entity<GroupLang>(entity =>
            {
                entity.Property(e => e.LanguageID).IsUnicode(false);

                entity.HasOne(d => d.GB)
                    .WithMany(p => p.GroupLangs)
                    .HasForeignKey(d => d.GBID)
                    .HasConstraintName("FK__GroupLang__GBID__6383C8BA");
            });

            modelBuilder.Entity<HistoryEmp>(entity =>
            {
                entity.HasKey(e => e.HisrotyID)
                    .HasName("PK__HistoryE__56F6B399A24187D1");
            });

            modelBuilder.Entity<Holiday>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Holidays)
                    .HasForeignKey(d => d.UserID)
                    .HasConstraintName("FK_Holiday_Users");
            });

            modelBuilder.Entity<LeaveData>(entity =>
            {
                entity.HasKey(e => e.LeaveID)
                    .HasName("PK__LeaveDat__796DB9799F49299F");

                entity.Property(e => e.LeaveArchive).IsUnicode(false);

                entity.Property(e => e.TimeLine).IsUnicode(false);

                entity.HasOne(d => d.ApprovedByNavigation)
                    .WithMany(p => p.LeaveDataApprovedByNavigations)
                    .HasForeignKey(d => d.ApprovedBy)
                    .HasConstraintName("FK__LeaveData__Appro__3B75D760");

                entity.HasOne(d => d.Cate)
                    .WithMany(p => p.LeaveData)
                    .HasForeignKey(d => d.CateID)
                    .HasConstraintName("FK__LeaveData__CateI__398D8EEE");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.LeaveData)
                    .HasForeignKey(d => d.EmpID)
                    .HasConstraintName("FK__LeaveData__EmpID__38996AB5");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LeaveDataUsers)
                    .HasForeignKey(d => d.UserID)
                    .HasConstraintName("FK__LeaveData__UserI__3A81B327");
            });

            modelBuilder.Entity<LeaveLog>(entity =>
            {
                entity.Property(e => e.EmpNumber).IsUnicode(false);

                entity.Property(e => e.LoggedByIP).IsUnicode(false);
            });

            modelBuilder.Entity<LoginDetect>(entity =>
            {
                entity.Property(e => e.LoggedByIP).IsUnicode(false);

                entity.Property(e => e.UserName).IsUnicode(false);
            });

            modelBuilder.Entity<LunchBreak>(entity =>
            {
                entity.Property(e => e.LunchTimeEnd).HasPrecision(0);

                entity.Property(e => e.LunchTimeStart).HasPrecision(0);

                entity.Property(e => e.WorkTimeEnd).HasPrecision(0);

                entity.Property(e => e.WorkTimeStart).HasPrecision(0);
            });

            modelBuilder.Entity<Part>(entity =>
            {
                entity.Property(e => e.PartCode).IsUnicode(false);

                entity.Property(e => e.PartSym).IsUnicode(false);

                entity.HasOne(d => d.Dept)
                    .WithMany(p => p.Parts)
                    .HasForeignKey(d => d.DeptID)
                    .HasConstraintName("FK__Part__DeptID__656C112C");
            });

            modelBuilder.Entity<PartLang>(entity =>
            {
                entity.Property(e => e.LanguageID).IsUnicode(false);

                entity.HasOne(d => d.Part)
                    .WithMany(p => p.PartLangs)
                    .HasForeignKey(d => d.PartID)
                    .HasConstraintName("FK__PartLang__PartID__66603565");
            });

            modelBuilder.Entity<PosLang>(entity =>
            {
                entity.Property(e => e.LanguageID).IsUnicode(false);

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.PosLangs)
                    .HasForeignKey(d => d.PositionID)
                    .HasConstraintName("FK__PosLang__Positio__6754599E");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.Property(e => e.PositionSym).IsUnicode(false);
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.Property(e => e.RoleSym).IsUnicode(false);
            });

            modelBuilder.Entity<Roles_User>(entity =>
            {
                entity.HasKey(e => e.RolesUserID)
                    .HasName("PK__Roles_Us__6FF55373C937F774");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Roles_User)
                    .HasForeignKey(d => d.RoleID)
                    .HasConstraintName("FK__Roles_Use__RoleI__73BA3083");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Roles_User)
                    .HasForeignKey(d => d.UserID)
                    .HasConstraintName("FK__Roles_Use__UserI__74AE54BC");
            });

            modelBuilder.Entity<SetApproveGroupBase>(entity =>
            {
                entity.HasKey(e => e.SAGBID)
                    .HasName("PK__SetAppro__92F871E48B46DF2C");

                entity.HasOne(d => d.GB)
                    .WithMany(p => p.SetApproveGroupBase)
                    .HasForeignKey(d => d.GBID)
                    .HasConstraintName("FK__SetApprove__GBID__02FC7413");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SetApproveGroupBase)
                    .HasForeignKey(d => d.UserID)
                    .HasConstraintName("FK__SetApprov__UserI__02084FDA");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.EmailAddress).IsUnicode(false);

                entity.Property(e => e.HashImage)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('/')");

                entity.Property(e => e.HashPass).IsUnicode(false);

                entity.Property(e => e.UserName).IsUnicode(false);

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.EmpID)
                    .HasConstraintName("FK__Users__EmpID__01142BA1");
            });

            modelBuilder.Entity<V_Employee>(entity =>
            {
                entity.ToView("V_Employee");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
