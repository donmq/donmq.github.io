using Machine_API._Repositories;
using Machine_API.Helpers.Attributes;
using Machine_API.Models.SAP;
using Microsoft.EntityFrameworkCore.Storage;

namespace Machine_API._Accessor
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped, isCheckSAP: true)]
    public interface ISAPRepositoryAccessor
    {
        IRepository<Control_File> Control_File { get; }
        IRepository<MT_Asset> MT_Asset { get; }

        Task<bool> SaveChangesAsync();

        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}