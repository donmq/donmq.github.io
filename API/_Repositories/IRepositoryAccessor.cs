using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using API.Models;
using API.Helper.Attributes;
using API._Repositories;
namespace API.Accessor._Interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IRepositoryAccessor
    {
        IRepository<QualityAfter> QualityAfter { get; }
        IRepository<PositionInformation> PositionInformation { get; }
        IRepository<ExerciseAttributes> ExerciseAttributes { get; }
        IRepository<Information> Information { get; }
        IRepository<Position> Position { get; }
        IRepository<Exercises> Exercises { get; }
        IRepository<MainAttributes> MainAttributes { get; }
        IRepository<Attributes> Attributes { get; }
        IRepository<TypeAttributes> TypeAttributes { get; }
        IRepository<QualityBefore> QualityBefore { get; }
        Task<bool> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}