using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using API.Data;
using API.Models;
using API.Accessor._Interfaces;
using API._Repositories;
namespace API.Accessor._Repositories
{
    public class RepositoryAccessor : IRepositoryAccessor
    {
        private readonly DBContext _context;
        public RepositoryAccessor(DBContext context)
        {
            _context = context;
            QualityAfter = new Repository<QualityAfter>(_context);
            PositionInformation = new Repository<PositionInformation>(_context);
            ExerciseAttributes = new Repository<ExerciseAttributes>(_context);
            Information = new Repository<Information>(_context);
            Position = new Repository<Position>(_context);
            Exercises = new Repository<Exercises>(_context);
            MainAttributes = new Repository<MainAttributes>(_context);
            Attributes = new Repository<Attributes>(_context);
            TypeAttributes = new Repository<TypeAttributes>(_context);
            QualityBefore = new Repository<QualityBefore>(_context);
        }
        public IRepository<QualityAfter> QualityAfter { get; private set; }
        public IRepository<PositionInformation> PositionInformation { get; private set; }
        public IRepository<ExerciseAttributes> ExerciseAttributes { get; private set; }
        public IRepository<Information> Information { get; private set; }
        public IRepository<Position> Position { get; private set; }
        public IRepository<Exercises> Exercises { get; private set; }
        public IRepository<MainAttributes> MainAttributes { get; private set; }
        public IRepository<Attributes> Attributes { get; private set; }
        public IRepository<TypeAttributes> TypeAttributes { get; private set; }
        public IRepository<QualityBefore> QualityBefore { get; private set; }
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