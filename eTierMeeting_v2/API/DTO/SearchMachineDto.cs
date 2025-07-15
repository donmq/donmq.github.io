using System.ComponentModel.DataAnnotations.Schema;

namespace Machine_API.DTO
{
    public class SearchMachineDto
    {
        public int? PdcId { get; set; }
        public string AssnoID { get; set; }
        public string MainAssetNumber { get; set; }
        public string Askid { get; set; }
        public string OwnerFty { get; set; }
        public string MachineName_CN { get; set; }
        public string State { get; set; }
        public string Supplier { get; set; }
        public string Trdate { get; set; }

        //Phân trang trong store procedure
        [NotMapped] // Ko map data từ Store sang Model
        public int? TotalRows { get; set; }

        //Lấy thêm bên export exel
        public string MachineName_EN { get; set; }
        public string MachineName_Local { get; set; }
        public string Spec { get; set; }
        public int? BuildingID { get; set; }
        public string BuildingCode { get; set; }
        public string BuildingName { get; set; }
        public string CellCode { get; set; }
        public string Plno { get; set; }
        public string Place { get; set; }
        public string PlaceReport { get; set; }
        public string LastMoveMachineDate { get; set; }
    }
}