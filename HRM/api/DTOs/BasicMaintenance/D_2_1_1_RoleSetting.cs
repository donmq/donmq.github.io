
namespace API.DTOs.BasicMaintenance
{
    public class RoleSetting
    {
        public string Role { get; set; }
        public string Description { get; set; }
        public string Factory { get; set; }
        public string Permission_Group { get; set; }
        public string Direct { get; set; }
        public string Direct_Str
        {
            get
            {
                return
                Direct == "1"
                ? Lang == "tw" ? "直接" : "Direct"
                : Direct == "2"
                    ? Lang == "tw" ? "間接" : "Indirect"
                    : Lang == "tw" ? "全部" : "All";
            }
        }
        public string Level_Start { get; set; }
        public string Level_End { get; set; }
        public string Lang { get; set; }
    }
    public class RoleSettingParam
    {
        public string Role { get; set; }
        public string Description { get; set; }
        public string Factory { get; set; }
        public string Permission_Group { get; set; }
        public string Direct { get; set; }
        public string Lang { get; set; }
    }
    public class RoleSettingDetail : RoleSetting
    {
        public string Update_By { get; set; }
        public DateTime? Update_Time { get; set; }
    }
    public partial class RoleSettingDto
    {
        public RoleSetting Role_Setting { get; set; }
        public List<TreeviewItem> Role_List { get; set; }
    }
    public partial class TreeviewItem
    {
        public string text { get; set; }
        public string value { get; set; }
        public bool @checked { get; set; }
        public bool @disabled { get; set; }
        public bool @collapsed { get; set; }
        public IEnumerable<TreeviewItem> children { get; set; }
    }
}
