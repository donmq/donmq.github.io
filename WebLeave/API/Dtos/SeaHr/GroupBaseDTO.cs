using API.Models;

namespace API.Dtos.SeaHr
{
    public class GroupBaseDTO
    {
        public GroupBaseDTO()
        {
            SetApproveGroupBase = new HashSet<SetApproveGroupBase>();
        }
        public int GBID { get; set; }
        public string BaseName { get; set; }
        public string BaseSym { get; set; }
        public virtual ICollection<SetApproveGroupBase> SetApproveGroupBase { get; set; }
    }
}