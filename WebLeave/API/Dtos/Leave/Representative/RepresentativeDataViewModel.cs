using API.Dtos.Leave.Personal;

namespace API.Dtos.Leave.Representative
{
    public class RepresentativeDataViewModel
    {
        public LeaveDataViewModel LeaveDataViewModel { get; set; }
        public EmployeeDataDto Employee { get; set; }
    }
}