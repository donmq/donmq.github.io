using API.Dtos.Common;
using API.Dtos.Leave;
using API.Dtos.Leave.Personal;
using API.Models;
namespace API._Services.Interfaces.Common
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface ILeaveCommonService
    {
        List<DateTime> EachDays(DateTime from, DateTime to);
        Task<bool> RecordForReport(int index, List<DateTime> allDay, Employee emp, int leaveID);
        Task AddCount(DateTime from, DateTime to, Employee emp);
        string ReturnArchive(string empnumber, string shift);
        Task<int> AddLeave(LeavePersonalDto leavePersonalDto, string userId);
        Task<HistoryEmployeeDto> GetHistoryEmployee(int empId, int year);
        Task<List<KeyValuePair<int, string>>> GetAllCategory(string language);
        Task<List<HolidayDto>> GetListHoliday();
        Task<double?> GetCountRestAgent(int empId, int year);
        Task<string> CheckDateLeave(string start, string end, int empid);
        Task<LeaveDataViewModel> GetLeaveDataWithCategory(LeaveData leaveDatas);
        Task<EmployeeDataDto> GetEmployeeData(int empId, int? userId = null);
        Task LeaveLogClear();
        Task<bool> CheckDataDatePicker();

        Task<WorkShiftDto> GetWorkShift(string shift);
        Task<List<WorkShiftDto>> GetWorkShifts();
    }
}