using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore.Storage;
namespace API._Repositories
{
    public class RepositoryAccessor : IRepositoryAccessor
    {
        private readonly DBContext _context;
        public RepositoryAccessor(DBContext context)
        {
            _context = context;

            AreaLang = new Repository<AreaLang, DBContext>(_context);
            Area = new Repository<Area, DBContext>(_context);
            Building = new Repository<Building, DBContext>(_context);
            BuildLang = new Repository<BuildLang, DBContext>(_context);
            Category = new Repository<Category, DBContext>(_context);
            CatLang = new Repository<CatLang, DBContext>(_context);
            CommentArchive = new Repository<CommentArchive, DBContext>(_context);
            Company = new Repository<Company, DBContext>(_context);
            CountAll = new Repository<CountAll, DBContext>(_context);
            CountArea = new Repository<CountArea, DBContext>(_context);
            CountBuilding = new Repository<CountBuilding, DBContext>(_context);
            CountDepartment = new Repository<CountDepartment, DBContext>(_context);
            CountPart = new Repository<CountPart, DBContext>(_context);
            DatePickerManager = new Repository<DatePickerManager, DBContext>(_context);
            DefaultSetting = new Repository<DefaultSetting, DBContext>(_context);
            Department = new Repository<Department, DBContext>(_context);
            DetpLang = new Repository<DetpLang, DBContext>(_context);
            EmailContent = new Repository<EmailContent, DBContext>(_context);
            Employee = new Repository<Employee, DBContext>(_context);
            ErrorLog = new Repository<ErrorLog, DBContext>(_context);
            GroupBase = new Repository<GroupBase, DBContext>(_context);
            GroupLang = new Repository<GroupLang, DBContext>(_context);
            HistoryEmp = new Repository<HistoryEmp, DBContext>(_context);
            Holiday = new Repository<Holiday, DBContext>(_context);
            LeaveData = new Repository<LeaveData, DBContext>(_context);
            LeaveLog = new Repository<LeaveLog, DBContext>(_context);
            LoginDetect = new Repository<LoginDetect, DBContext>(_context);
            LunchBreak = new Repository<LunchBreak, DBContext>(_context);
            PartLang = new Repository<PartLang, DBContext>(_context);
            Part = new Repository<Part, DBContext>(_context);
            Position = new Repository<Position, DBContext>(_context);
            PosLang = new Repository<PosLang, DBContext>(_context);
            ReportData = new Repository<ReportData, DBContext>(_context);
            Roles = new Repository<Roles, DBContext>(_context);
            RolesUser = new Repository<Roles_User, DBContext>(_context);
            SetApproveGroupBase = new Repository<SetApproveGroupBase, DBContext>(_context);
            Users = new Repository<Users, DBContext>(_context);
        }

        public IRepository<AreaLang> AreaLang { get; private set; }
        public IRepository<Area> Area { get; private set; }
        public IRepository<Building> Building { get; private set; }
        public IRepository<BuildLang> BuildLang { get; private set; }
        public IRepository<Category> Category { get; private set; }
        public IRepository<CatLang> CatLang { get; private set; }
        public IRepository<CommentArchive> CommentArchive { get; private set; }
        public IRepository<Company> Company { get; private set; }
        public IRepository<CountAll> CountAll { get; private set; }
        public IRepository<CountArea> CountArea { get; private set; }
        public IRepository<CountBuilding> CountBuilding { get; private set; }
        public IRepository<CountDepartment> CountDepartment { get; private set; }
        public IRepository<CountPart> CountPart { get; private set; }
        public IRepository<DatePickerManager> DatePickerManager { get; private set; }
        public IRepository<DefaultSetting> DefaultSetting { get; private set; }
        public IRepository<Department> Department { get; private set; }
        public IRepository<DetpLang> DetpLang { get; private set; }
        public IRepository<EmailContent> EmailContent { get; private set; }
        public IRepository<Employee> Employee { get; private set; }
        public IRepository<ErrorLog> ErrorLog { get; private set; }
        public IRepository<GroupBase> GroupBase { get; private set; }
        public IRepository<GroupLang> GroupLang { get; private set; }
        public IRepository<HistoryEmp> HistoryEmp { get; private set; }
        public IRepository<Holiday> Holiday { get; private set; }
        public IRepository<LeaveData> LeaveData { get; private set; }
        public IRepository<LeaveLog> LeaveLog { get; private set; }
        public IRepository<LoginDetect> LoginDetect { get; private set; }
        public IRepository<LunchBreak> LunchBreak { get; private set; }
        public IRepository<PartLang> PartLang { get; private set; }
        public IRepository<Part> Part { get; private set; }
        public IRepository<Position> Position { get; private set; }
        public IRepository<PosLang> PosLang { get; private set; }
        public IRepository<ReportData> ReportData { get; private set; }
        public IRepository<Roles> Roles { get; private set; }
        public IRepository<Roles_User> RolesUser { get; private set; }
        public IRepository<SetApproveGroupBase> SetApproveGroupBase { get; private set; }
        public IRepository<Users> Users { get; private set; }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}