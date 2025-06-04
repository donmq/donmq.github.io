namespace API.Dtos.Common
{
    public class BuildingDto
    {
        public int BuildingID { get; set; }
        public string BuildingName { get; set; }
        public string BuildingSym { get; set; }
        public string BuildingCode { get; set; }
        public int? AreaID { get; set; }
        public int? Number { get; set; }
        public bool? Visible { get; set; }
    }

    public class BuildingInformation {
        public int BuildingID { get; set; }
        public string BuildingName { get; set; }
        public string BuildingNameVi {get;set;}
        public string BuildingNameEn {get;set;}
        public string BuildingNameZh {get;set;}
        public string BuildingSym { get; set; }
        public string BuildingCode { get; set; }
        public int? AreaID { get; set; }
        public string AreaName {get;set;}
        public int? Number { get; set; }
        public bool? Visible { get; set; }
    }
}