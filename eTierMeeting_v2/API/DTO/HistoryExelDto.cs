namespace Machine_API.DTO
{
    public class HistoryExelDto
    {
        public string MachineID { get; set; }
        public string MachineName { get; set; }

        public string OldEmpNumber { get; set; }
        public string NewEmpNumber { get; set; }

        public string UserID { get; set; }
        public string UserName { get; set; }

        public string OldPlnoCode { get; set; }
        public string NewPlnoCode { get; set; }
        public string OldPlnoName { get; set; }
        public string NewPlnoName { get; set; }

        public string TrDate { get; set; }
        public string OldCell { get; set; }
        public string NewCell { get; set; }

        public string OldCellCode { get; set; }
        public string NewCellCode { get; set; }

        public string Ownerfty { get; set; }
    }
}