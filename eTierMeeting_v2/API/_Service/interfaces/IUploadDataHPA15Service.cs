using Machine_API.Helpers.Attributes;
using Machine_API.Helpers.Utilities;

namespace Machine_API._Service.interfaces
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IUploadDataHPA15Service
    {
        Task<OperationResult> ImportDataExcel(IFormFile file);
    }
}