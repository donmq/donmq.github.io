export interface UserRoleAddDto {
    buildingID: string;
    cell: string;
    plno: string;
    managerList: checkManagerDto[];
}

export interface checkManagerDto {
    is_Manager: boolean | null;
    is_Preliminary: boolean | null;
}