namespace Machine_API.Helpers.Params
{
    public class HistoryInventoryParams
    {
        public int IdInventory { get; set; }
        public string IdPlno { get; set; }
        public string FromDateTime { get; set; }
        public string ToDateTime { get; set; }
    }
}