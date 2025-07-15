namespace Machine_API.DTO
{
    public class UserRoleAddDto
    {
        public string BuildingID { get; set; }
        public string Cell { get; set; }
        public string Plno { get; set; }
        public bool? Is_Manager{get; set;}
        public bool? Is_Preliminary{get; set;}
    
    }
}