namespace Machine_API.DTO
{
    public class Hp_a15Dto
    {
        public string Plno { get; set; }
        public string PlnoCode { get; set; }
        public string Place { get; set; }
        public int BuildingID { get; set; }
        public string BuildingCode { get; set; }
        public int? CellID { get; set; }
        public string CellCode { get; set; }
        public string CellName { get; set; }
        public List<UserDto> ListUserManager { get; set; }//list quản ly
        public List<UserDto> ListUser { get; set; }//list kiểm kê
    }
}