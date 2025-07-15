
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Machine_API._Accessor;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Microsoft.EntityFrameworkCore;

namespace Machine_API._Service.service
{
    public class CategoryService : ICategoryService
    {
        private readonly IMachineRepositoryAccessor _repository;
        private readonly MapperConfiguration _mapperConfiguration;

        public CategoryService(IMachineRepositoryAccessor repository, MapperConfiguration mapperConfiguration)
        {
            _repository = repository;
            _mapperConfiguration = mapperConfiguration;
        }

        public async Task<List<CategoryDto>> GetAllCategory()
        {
            var data = await _repository.hp_a03.FindAll(x => x.Visible == true).ProjectTo<CategoryDto>(_mapperConfiguration)
                .ToListAsync();
            return (data);
        }
    }
}