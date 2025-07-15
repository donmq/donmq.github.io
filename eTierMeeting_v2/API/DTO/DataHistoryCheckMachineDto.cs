namespace Machine_API.DTO
{
    public class DataHistoryCheckMachineDto
    {
        public int ID { get; set; }

        public string MachineID { get; set; }

        public string MachineName { get; set; }

        public string Supplier { get; set; }

        public string Place { get; set; }

        public string State { get; set; }

        public int? StatusCheckMachine { get; set; }

        public int? HistoryCheckMachineID { get; set; }

        public int? Count { get; set; }

        public string Location
        {
            get
            {
                return Place + "-" + State;
            }
            set{

            }
        }
    }
}