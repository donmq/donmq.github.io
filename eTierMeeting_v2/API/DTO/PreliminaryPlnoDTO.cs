namespace Machine_API.DTO
{
    public class PreliminaryPlnoDTO
    {
        public string EmpName { get; set; }
        public string EmpNumber { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public List<BuildingDto> ListBuilding { get; set; }
        public List<CellDto> ListCell { get; set; }
        public List<Hp_a15Dto> ListHpA15 { get; set; }
        public bool? Is_Manager{get; set;}
        public bool? Is_Preliminary{get; set;}
    }
}