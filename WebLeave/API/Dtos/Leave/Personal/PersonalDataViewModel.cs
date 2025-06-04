
namespace API.Dtos.Leave.Personal
{
    public class PersonalDataViewModel
    {
        public List<LeaveDataViewModel> LeaveDataViewModel { get; set; }
        public EmployeeDataDto Employee { get; set; }
        public HistoryEmployeeDto History { get; set; }
    }
}