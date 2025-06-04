using API.Models;
using Microsoft.EntityFrameworkCore.Storage;
namespace API._Repositories
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IRepositoryAccessor
    {
        IRepository<AreaLang> AreaLang { get; }
        IRepository<Area> Area { get; }
        IRepository<Building> Building { get; }
        IRepository<BuildLang> BuildLang { get; }
        IRepository<Category> Category { get; }
        IRepository<CatLang> CatLang { get; }
        IRepository<CommentArchive> CommentArchive { get; }
        IRepository<Company> Company { get; }
        IRepository<CountAll> CountAll { get; }
        IRepository<CountArea> CountArea { get; }
        IRepository<CountBuilding> CountBuilding { get; }
        IRepository<CountDepartment> CountDepartment { get; }
        IRepository<CountPart> CountPart { get; }
        IRepository<DatePickerManager> DatePickerManager { get; }
        IRepository<DefaultSetting> DefaultSetting { get; }
        IRepository<Department> Department { get; }
        IRepository<DetpLang> DetpLang { get; }
        IRepository<EmailContent> EmailContent { get; }
        IRepository<Employee> Employee { get; }
        IRepository<ErrorLog> ErrorLog { get; }
        IRepository<GroupBase> GroupBase { get; }
        IRepository<GroupLang> GroupLang { get; }
        IRepository<HistoryEmp> HistoryEmp { get; }
        IRepository<Holiday> Holiday { get; }
        IRepository<LeaveData> LeaveData { get; }
        IRepository<LeaveLog> LeaveLog { get; }
        IRepository<LoginDetect> LoginDetect { get; }
        IRepository<LunchBreak> LunchBreak { get; }
        IRepository<PartLang> PartLang { get; }
        IRepository<Part> Part { get; }
        IRepository<Position> Position { get; }
        IRepository<PosLang> PosLang { get; }
        IRepository<ReportData> ReportData { get; }
        IRepository<Roles> Roles { get; }
        IRepository<Roles_User> RolesUser { get; }
        IRepository<SetApproveGroupBase> SetApproveGroupBase { get; }
        IRepository<Users> Users { get; }
        Task<bool> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}