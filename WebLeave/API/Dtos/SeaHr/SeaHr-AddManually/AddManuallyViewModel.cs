namespace API.Dtos.SeaHr.ViewModel
{
    public class AddManuallyViewModel
    {
        public int LeaveId { get; set; }

        public string EmpNumber { get; set; }

        public string EmpName { get; set; }

        public int? CateID { get; set; }
        public string CateName { get; set; }

        public string DepName { get; set; }

        public DateTime? Time_Start { get; set; }

        public DateTime? Time_End { get; set; }

        public double? LeaveDay { get; set; }
    }
}