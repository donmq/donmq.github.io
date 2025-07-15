using Machine_API.DTO;
using Machine_API.Models.MachineCheckList;
using Microsoft.EntityFrameworkCore;

namespace Machine_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.SetCommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
        }
        public DbSet<Building> Building { get; set; }
        public DbSet<SearchMachineDto> SearchMachineDto { get; set; }
        public DbSet<HistoryDto> HistoryDto { get; set; }
        public DbSet<Cells> Cell { get; set; }
        public DbSet<Cell_Plno> Cell_Plno { get; set; }
        public DbSet<DataHistoryCheckMachine> DataHistoryCheckMachine { get; set; }
        public DbSet<DataHistoryInventory> DataHistoryInventory { get; set; }
        public DbSet<DateInventory> DateInventory { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<EmployPlno> EmployPlno { get; set; }
        public DbSet<ErrorLog> ErrorLog { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<HistoryCheckMachine> HistoryCheckMachine { get; set; }
        public DbSet<HistoryInventory> HistoryInventory { get; set; }
        public DbSet<hp_a03> hp_a03 { get; set; }
        public DbSet<hp_a04_old> hp_a04_old { get; set; }
        public DbSet<hp_a04> hp_a04 { get; set; }
        public DbSet<hp_a15> hp_a15 { get; set; }
        public DbSet<Machine_IO> Machine_IO { get; set; }
        public virtual DbSet<Machine_Safe_Check> Machine_Safe_Check { get; set; }
        public virtual DbSet<Machine_Safe_Checklist> Machine_Safe_Checklist { get; set; }
        public DbSet<PDC> PDC { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<PreliminaryPlno> PreliminaryPlno { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Building>().HasKey(x => new { x.BuildingID });
            modelBuilder.Entity<Cells>().HasKey(x => new { x.CellID });
            modelBuilder.Entity<Cell_Plno>().HasKey(x => new { x.ID });
            modelBuilder.Entity<DataHistoryCheckMachine>().HasKey(x => new { x.ID });
            modelBuilder.Entity<DataHistoryInventory>().HasKey(x => new { x.ID });
            modelBuilder.Entity<DateInventory>().HasKey(x => new { x.Id });
            modelBuilder.Entity<Employee>().HasKey(x => new { x.ID });
            modelBuilder.Entity<EmployPlno>().HasKey(x => new { x.ID });
            modelBuilder.Entity<History>().HasKey(x => new { x.HistoryID });
            modelBuilder.Entity<HistoryCheckMachine>().HasKey(x => new { x.HistoryCheckMachineID });
            modelBuilder.Entity<HistoryInventory>().HasKey(x => new { x.HistoryInventoryID });
            modelBuilder.Entity<hp_a03>().HasKey(x => new { x.Askid });
            modelBuilder.Entity<hp_a04_old>().HasKey(x => new { x.AssnoID, x.OwnerFty });
            modelBuilder.Entity<hp_a04>().HasKey(x => new {x.Main_Asset_Number, x.AssnoID, x.OwnerFty });
            modelBuilder.Entity<hp_a15>().HasKey(x => new { x.Plno });
            modelBuilder.Entity<Machine_IO>().HasKey(x => new { x.AssnoID, x.OwnerFty });
            modelBuilder.Entity<Machine_Safe_Check>().HasKey(x => new { x.AssnoID, x.CheckDate, x.Id });
            modelBuilder.Entity<Machine_Safe_Checklist>().HasKey(x => new { x.Id });
            modelBuilder.Entity<PDC>().HasKey(x => new { x.PDCID });
            modelBuilder.Entity<User>().HasKey(x => new { x.UserID });
            modelBuilder.Entity<SearchMachineDto>().HasNoKey();
            modelBuilder.Entity<HistoryDto>().HasNoKey();
            modelBuilder.Entity<PreliminaryPlno>().HasKey(x => new { x.Plno, x.BuildingID, x.EmpNumber, x.CellID });
            modelBuilder.Entity<UserRoles>().HasKey(x => new { x.Roles, x.EmpNumber });
            modelBuilder.Entity<Roles>().HasKey(x => new { x.ID });
            
        }
    }
}