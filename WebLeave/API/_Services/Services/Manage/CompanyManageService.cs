using API._Repositories;
using API._Services.Interfaces.Manage;
using API.Dtos.Manage.CompanyManage;
using API.Helpers.Enums;
using API.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
namespace API._Services.Services.Manage
{
    public class CompanyManageService : ICompanyManageService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IRepositoryAccessor _repoAccessor;

        public CompanyManageService(IMapper mapper, MapperConfiguration configMapper, IRepositoryAccessor repoAccessor)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _repoAccessor = repoAccessor;
        }

        public async Task<OperationResult> AddCompany(CompanyManageDto companyAdd)
        {
            if (companyAdd == null)
            {
                return new OperationResult(true, MessageConstants.ADD_ERROR,MessageConstants.ERROR);
            }
            //Add Company
            Company addCompany = _mapper.Map<Company>(companyAdd);
            _repoAccessor.Company.Add(addCompany);
            try
            {
                await _repoAccessor.SaveChangesAsync();
                return new OperationResult(true,MessageConstants.ADD_SUCCESS,MessageConstants.SUCCESS);
            }
            catch (System.Exception)
            {
                return new OperationResult(false,MessageConstants.ADD_ERROR,MessageConstants.ERROR);
            }
        }

        public async Task<OperationResult> EditCompany(CompanyManageDto companyEdit)
        {
            Company checkID = _repoAccessor.Company.FindAll(x => x.CompanyID == companyEdit.CompanyID,true).FirstOrDefault();
            if (checkID == null)
            {
                 return new OperationResult(false,MessageConstants.UPDATE_ERROR,MessageConstants.ERROR);
            }
            else
            {
                var editCompany = _mapper.Map<Company>(companyEdit);
                
                try
                {
                    _repoAccessor.Company.Update(editCompany);
                    await _repoAccessor.SaveChangesAsync();
                    return new OperationResult(true,MessageConstants.UPDATE_SUCCESS,MessageConstants.SUCCESS);
                }
                catch (System.Exception)
                {
                     return new OperationResult(false,MessageConstants.UPDATE_ERROR,MessageConstants.ERROR);
                }
            }
        }

        public async Task<List<CompanyManageDto>> GetAllCompany()
        {
            var data = await _repoAccessor.Company.FindAll().ProjectTo<CompanyManageDto>(_configMapper).AsNoTracking().ToListAsync();
            return data;
        }
    }
}