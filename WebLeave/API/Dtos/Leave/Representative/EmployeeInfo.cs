namespace API.Dtos.Leave.Representative
{
    public class EmployeeInfo
    {
        public string empname { get; set; }
        public int empid { get; set; }
        public bool? isSun { get; set; }
        public double totalleave { get; set; }
        public double counttotal { get; set; }
        public double cagent { get; set; }
        public double carrange { get; set; }
        public double restedleave { get; set; }
    }
}