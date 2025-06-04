namespace API.Dtos.Leave.Personal
{
    public class LeavePersonalDto
    {
        public int EmpID { get; set; }
        public int CateID { get; set; }
        public string Time_Lunch { get; set; }
        public string Time_Start { get; set; }
        public string Time_End { get; set; }
        public string LeaveDay { get; set; }
        public string Comment { get; set; }
        public string Language { get; set; }
        public string EmpNumber { get; set; }
        public string LeaveType { get; set; }
        public string IpLocal { get; set; }
    }
}