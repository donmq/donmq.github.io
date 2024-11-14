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
        public IRepository<QualityAfter> QualityAfter { get; set; }
        public IRepository<PositionInformation> PositionInformation { get; set; }
        public IRepository<ExerciseAttributes> ExerciseAttributes { get; set; }
        public IRepository<Information> Information { get; set; }
        public IRepository<Position> Position { get; set; }
        public IRepository<Exercises> Exercises { get; set; }
        public IRepository<MainAttributes> MainAttributes { get; set; }
        public IRepository<Attributes> Attributes { get; set; }
        public IRepository<TypeAttributes> TypeAttributes { get; set; }
        public IRepository<QualityBefore> QualityBefore { get; set; }

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