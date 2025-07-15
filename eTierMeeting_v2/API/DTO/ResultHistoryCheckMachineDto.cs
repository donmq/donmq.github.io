namespace Machine_API.DTO
{
    public class  ResultHistoryCheckMachineDto
    {
        public List<CheckMachineDto> listCheckMachine { get; set; }
        public UserDto user { get; set; }
        public int error { get; set; }
        public HistoryCheckMachineDto historyCheckMachine { get; set; }
        public int totalScans { get; set; }
        public int totalExist { get; set; }
        public int totalNotExist { get; set; }
    }
}