using Machine_API.DTO;

namespace Machine_API.Helpers.Params
{
    public class InventoryParams
    {
        public int IdInventory { get; set; }
        public string IdPlno { get; set; }
        public string FromDateTime { get; set; }
        public string ToDateTime { get; set; }
        public List<SearchMachineInventoryDto> listMachineInventory { get; set; }
    }
}