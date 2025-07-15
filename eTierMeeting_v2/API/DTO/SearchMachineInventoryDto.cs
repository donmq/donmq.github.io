namespace Machine_API.DTO
{
    public class SearchMachineInventoryDto
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
        public int StatusIventory { get; set; }
        public string Askid { get; set; }
        public string Plno { get; set; }
        public int? CellID { get; set; }
        public int BuildingID { get; set; }
        public int? PDCID { get; set; }
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
            set{

            }
        }
        public SearchMachineInventoryDto(string machineID, string ownerFty)
        {
            this.MachineID = machineID;
            this.OwnerFty = ownerFty;
        }

        public SearchMachineInventoryDto() { }

    }
}