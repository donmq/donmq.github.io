using Machine_API._Repositories;
using Machine_API.Helpers.Attributes;
using Machine_API.Models.MT;
using Microsoft.EntityFrameworkCore.Storage;

namespace Machine_API._Accessor
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped, isCheckSAP: true)]
    public interface IMTRepositoryAccessor
    {
        IRepository<Control_File_Temp> Control_File_Temp { get; }
        IRepository<SAP_Cost_Center_Changed_Record> SAP_Cost_Center_Changed_Record { get; }

        Task<bool> SaveChangesAsync();

        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}