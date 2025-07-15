using Machine_API._Repositories;
using Machine_API.Helpers.Attributes;
using Machine_API.Models.MachineCheckList;
using Microsoft.EntityFrameworkCore.Storage;

namespace Machine_API._Accessor
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IMachineRepositoryAccessor
    {
        IRepository<Building> Building { get; }
        IRepository<Cell_Plno> Cell_Plno { get; }
        IRepository<Cells> Cells { get; }
        IRepository<DataHistoryCheckMachine> DataHistoryCheckMachine { get; }
        IRepository<DataHistoryInventory> DataHistoryInventory { get; }
        IRepository<DateInventory> DateInventory { get; }
        IRepository<Employee> Employee { get; }
        IRepository<EmployPlno> EmployPlno { get; }
        IRepository<ErrorLog> ErrorLog { get; }
        IRepository<History> History { get; }
        IRepository<HistoryCheckMachine> HistoryCheckMachine { get; }
        IRepository<HistoryInventory> HistoryInventory { get; }
        IRepository<hp_a03> hp_a03 { get; }
        IRepository<hp_a04> hp_a04 { get; }
        IRepository<hp_a04_old> hp_a04_old { get; }
        IRepository<hp_a15> hp_a15 { get; }
        IRepository<Machine_IO> Machine_IO { get; }
        IRepository<Machine_Safe_Check> Machine_Safe_Check { get; }
        IRepository<Machine_Safe_Checklist> Machine_Safe_Checklist { get; }
        IRepository<PDC> PDC { get; }
        IRepository<PreliminaryPlno> PreliminaryPlno { get; }
        IRepository<Roles> Roles { get; }
        IRepository<User> User { get; }
        IRepository<UserRoles> UserRoles { get; }

        Task<bool> SaveChangesAsync();

        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}