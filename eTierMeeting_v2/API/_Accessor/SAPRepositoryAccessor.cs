using Machine_API._Repositories;
using Machine_API.Data;
using Machine_API.Models.SAP;
using Microsoft.EntityFrameworkCore.Storage;

namespace Machine_API._Accessor
{
    public class SAPRepositoryAccessor : ISAPRepositoryAccessor
    {
        private readonly SAPContext _context;

        public SAPRepositoryAccessor(SAPContext context)
        {
            _context = context;

            Control_File = new SAPRepository<Control_File>(_context);
            MT_Asset = new SAPRepository<MT_Asset>(_context);
        }

        public IRepository<Control_File> Control_File { get; private set; }

        public IRepository<MT_Asset> MT_Asset { get; private set; }

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