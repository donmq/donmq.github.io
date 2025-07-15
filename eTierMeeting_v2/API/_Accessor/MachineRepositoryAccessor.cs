using Machine_API._Repositories;
using Machine_API.Data;
using Machine_API.Models.MachineCheckList;
using Microsoft.EntityFrameworkCore.Storage;

namespace Machine_API._Accessor
{
    public class MachineRepositoryAccessor : IMachineRepositoryAccessor
    {
        private readonly DataContext _context;

        public MachineRepositoryAccessor(DataContext context)
        {
            _context = context;

            Building = new Repository<Building>(_context);
            Cell_Plno = new Repository<Cell_Plno>(_context);
            Cells = new Repository<Cells>(_context);
            DataHistoryCheckMachine = new Repository<DataHistoryCheckMachine>(_context);
            DataHistoryInventory = new Repository<DataHistoryInventory>(_context);
            DateInventory = new Repository<DateInventory>(_context);
            Employee = new Repository<Employee>(_context);
            EmployPlno = new Repository<EmployPlno>(_context);
            ErrorLog = new Repository<ErrorLog>(_context);
            History = new Repository<History>(_context);
            HistoryCheckMachine = new Repository<HistoryCheckMachine>(_context);
            HistoryInventory = new Repository<HistoryInventory>(_context);
            hp_a03 = new Repository<hp_a03>(_context);
            hp_a04 = new Repository<hp_a04>(_context);
            hp_a04_old = new Repository<hp_a04_old>(_context);
            hp_a15 = new Repository<hp_a15>(_context);
            Machine_IO = new Repository<Machine_IO>(_context);
            Machine_Safe_Check = new Repository<Machine_Safe_Check>(_context);
            Machine_Safe_Checklist = new Repository<Machine_Safe_Checklist>(_context);
            PDC = new Repository<PDC>(_context);
            PreliminaryPlno = new Repository<PreliminaryPlno>(_context);
            Roles = new Repository<Roles>(_context);
            User = new Repository<User>(_context);
            UserRoles = new Repository<UserRoles>(_context);
        }

        public IRepository<Building> Building { get; private set; }

        public IRepository<Cell_Plno> Cell_Plno { get; private set; }

        public IRepository<Cells> Cells { get; private set; }

        public IRepository<DataHistoryCheckMachine> DataHistoryCheckMachine { get; private set; }

        public IRepository<DataHistoryInventory> DataHistoryInventory { get; private set; }

        public IRepository<DateInventory> DateInventory { get; private set; }

        public IRepository<Employee> Employee { get; private set; }

        public IRepository<EmployPlno> EmployPlno { get; private set; }

        public IRepository<ErrorLog> ErrorLog { get; private set; }

        public IRepository<History> History { get; private set; }

        public IRepository<HistoryCheckMachine> HistoryCheckMachine { get; private set; }

        public IRepository<HistoryInventory> HistoryInventory { get; private set; }

        public IRepository<hp_a03> hp_a03 { get; private set; }

        public IRepository<hp_a04> hp_a04 { get; private set; }

        public IRepository<hp_a04_old> hp_a04_old { get; private set; }

        public IRepository<hp_a15> hp_a15 { get; private set; }

        public IRepository<Machine_IO> Machine_IO { get; private set; }

        public IRepository<Machine_Safe_Check> Machine_Safe_Check { get; private set; }

        public IRepository<Machine_Safe_Checklist> Machine_Safe_Checklist { get; private set; }

        public IRepository<PDC> PDC { get; private set; }

        public IRepository<PreliminaryPlno> PreliminaryPlno { get; private set; }

        public IRepository<Roles> Roles { get; private set; }

        public IRepository<User> User { get; private set; }

        public IRepository<UserRoles> UserRoles { get; private set; }
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