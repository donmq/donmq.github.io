namespace Machine_API.DTO
{
    public class CheckMachineDto
    {
        public string MachineID { get; set; }
        public string MachineName { get; set; }
        public string EmpName { get; set; }
        public string PlnoName { get; set; }
        public string PlaceName { get; set; }
        public string Status { get; set; }
        public string OwnerFty { get; set; }
        public string Supplier { get; set; }
        public string TrDate { get; set; }
        public int StatusCheckMachine { get; set; }
        public bool IsNull { get; set; } = true;
        public string MachineCode
        {
            get
            {
                return OwnerFty + MachineID;
            }
            set
            {

            }
        }
        public string Location
        {
            get
            {
                return PlaceName + "-" + PlnoName;
            }
            set
            {

            }
        }
        public CheckMachineDto(string machineID, string ownerFty)
        {
            this.MachineID = machineID;
            this.OwnerFty = ownerFty;
        }

        public CheckMachineDto() { }
    }
}