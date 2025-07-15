// Generated at 11/28/2024, 2:39:45 PM
export interface Machine_Safe_CheckDto {
  id: number;
  checkDate: Date | string;
  assnoID: string;
  dept_ID: string;
  check_Item: number;
  resault: string;
  pic_Path: string;
  createTime: Date | string | null;
  createBy: string;
}

export interface Machine_Safe_Check {
  machineID: string;
  machineName: string;
  ownerFty: string;
  location: string;
}

export interface Machine_Safe_Check_Scan {
  key: string;
  value: string;
  answer?: 'y' | 'n' | 'n/a';
  image?: string;
}
