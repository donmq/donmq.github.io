export interface AssetsLendMaintainDto {
  stt: number;
  assnoID: string;
  spec: string;
  dept_ID: string;
  plno: string;
  state: string;
  machineName_EN: string;
  machineName_Local: string;
  machineName_CN: string;
  supplier: string;
  ownerFty: string;
  usingFty: string;
  iO_Kind: string;
  iO_Reason: string;
  iO_Date: Date | string;
  iO_Confirm: string;
  re_Date: Date | string;
  re_Confirm: string;
  remark: string;
  insert_By: string;
  insert_At: Date | string | null;
  update_By: string;
  update_At: Date | string | null;
}
export interface AssetsLendMaintainParam {
  lendDate: string;
  machineID: string;
  lendTo: string;
  return: string;
}
