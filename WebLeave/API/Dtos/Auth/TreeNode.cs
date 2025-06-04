namespace API.Dtos.Auth
{
    // Empleament by TreeNode interface of Primeng Tree/TreeTable npm
    public class TreeNode<T> where T : class
    {
        public bool Checked { get; set; }
        public string Label { get; set; }
        public T Data { get; set; }
        public string Icon { get; set; }
        public string ExpandedIcon { get; set; }
        public string CollapsedIcon { get; set; }
        public List<TreeNode<T>> Children { get; set; } = new();
        public bool? Leaf { get; set; }
        public bool? Expanded { get; set; }
        public string Type { get; set; }
        public TreeNode<T> Parent { get; set; }
        public bool? PartialSelected { get; set; }
        public object Style { get; set; }
        public string StyleClass { get; set; }
        public bool? Draggable { get; set; }
        public bool? Droppable { get; set; }
        public bool? Selectable { get; set; }
        public string Key { get; set; }
    }

    public class RoleNode
    {
        public int? AreaID { get; set; }
        public int? BuildingID { get; set; }
        public int? DepartmentID { get; set; }
        public int? PartID { get; set; }
        public int? RoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleSym { get; set; }
        public int? RoleRanked { get; set; }
        public bool RoleAssigned { get; set; }
        public bool RoleChildAssigned { get; set; }

        public int? ParentRoleID { get; set; }
    }

    public class GroupBaseNode
    {
        public int GBID { get; set; }
        public string BaseName { get; set; }
        public string BaseSym { get; set; }
    }
}