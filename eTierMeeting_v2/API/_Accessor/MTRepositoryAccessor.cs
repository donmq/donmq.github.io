using Machine_API._Repositories;
using Machine_API.Data;
using Machine_API.Models.MT;
using Microsoft.EntityFrameworkCore.Storage;

namespace Machine_API._Accessor
{
    public class MTRepositoryAccessor : IMTRepositoryAccessor
    {
        private readonly MTContext _context;

        public MTRepositoryAccessor(MTContext context)
        {
            _context = context;

            Control_File_Temp = new MTRepository<Control_File_Temp>(_context);
            SAP_Cost_Center_Changed_Record = new MTRepository<SAP_Cost_Center_Changed_Record>(_context);
        }

        public IRepository<Control_File_Temp> Control_File_Temp { get; private set; }

        public IRepository<SAP_Cost_Center_Changed_Record> SAP_Cost_Center_Changed_Record { get; private set; }

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