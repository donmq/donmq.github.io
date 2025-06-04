using API.Dtos.Manage.HolidayManage;
namespace API._Services.Interfaces.Manage
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IHolidayService
    {
        Task<List<HolidayAndUserDto>> GetHolidayData();
        Task<OperationResult> RemoveHoliday(int IDHoliday);
        Task<OperationResult> AddHoliday(HolidayDto Holidays);
    }
}