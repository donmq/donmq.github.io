export interface TreeNode<T> {
    checked?: boolean;
    label?: string;
    data?: T;
    icon?: string;
    expandedIcon?: any;
    collapsedIcon?: any;
    children?: TreeNode<T>[];
    leaf?: boolean;
    expanded?: boolean;
    type?: string;
    parent?: TreeNode<T>;
    partialSelected?: boolean;
    style?: string;
    styleClass?: string;
    draggable?: boolean;
    droppable?: boolean;
    selectable?: boolean;
    key?: string;
}

export interface RoleNode {
    areaID: number | null;
    buildingID: number | null;
    departmentID: number | null;
    partID: number | null;
    roleID: number | null;
    roleName: string;
    roleSym: string;
    roleRanked: number | null;
    roleAssigned: boolean;
    roleChildAssigned: boolean;
}

export interface GroupBaseNode {
    gBID: number;
    baseName: string;
    baseSym: string;
}