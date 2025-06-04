namespace API._Services.Interfaces.SeaHr
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IHrDeleteEmployeeService
    {
        Task<OperationResult> UploadExcelDelete(IFormFile file);
       
    }
}