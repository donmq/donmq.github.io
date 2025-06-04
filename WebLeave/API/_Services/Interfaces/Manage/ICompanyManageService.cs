using API.Dtos.Manage.CompanyManage;
namespace API._Services.Interfaces.Manage
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface ICompanyManageService
    {
        Task<List<CompanyManageDto>> GetAllCompany();
        Task<OperationResult> AddCompany(CompanyManageDto CompanyAdd);
        Task<OperationResult> EditCompany(CompanyManageDto CompanyEdit);
    }
}