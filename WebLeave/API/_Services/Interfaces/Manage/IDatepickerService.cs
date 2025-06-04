using API.Dtos.Manage.DatepickerManagement;
namespace API._Services.Interfaces.Manage
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IDatepickerService
    {
        Task<List<DatepickerDto>> GetAll();
        Task<OperationResult> UpdateDatepicker(DatepickerDto datepickerDto, int UserID);
    }
}
