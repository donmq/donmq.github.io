namespace Machine_API.DTO
{
    public class MachineScanDto
    {
        public string machineID { get; set; }
        public string machineName { get; set; }
        public int? cellid { get; set; }
        public string plno { get; set; }
        public string place { get; set; }
        public string state { get; set; }
        public string ownerFty { get; set; }
    }
}