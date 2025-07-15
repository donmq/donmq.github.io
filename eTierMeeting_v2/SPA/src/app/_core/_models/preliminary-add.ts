export interface PreliminaryPlnoAdd {
  empName: string;
  empNumber: string;
  updateBy: string;
  roleList: UserRoleAdd[];

}
// export interface UserRoleAdd {
//   buildingID: string;
//   cell: string;
//   plno: string;
//   is_Manager: boolean;
//   is_Preliminary: boolean;
// }
export interface UserRoleAdd{
  buildingID: string;
  cell: string;
  plno: string;
  managerList: checkManager[];
}

export interface checkManager {
  is_Manager: boolean | null;
  is_Preliminary: boolean | null;
}
