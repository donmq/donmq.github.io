import { Pagination } from "@utilities/pagination-utility";

export interface RewardandPenaltyMaintenance {
  factory: string;
  code: string;
  code_Name: string;
  update_By: string;
  update_Time: string;
  isDuplicate: boolean;

}

export interface RewardandPenaltyMaintenanceParam {
  factory: string;
  reason_Code: string;
}
export interface RewardandPenaltyMaintenanceSource {
  param: RewardandPenaltyMaintenanceParam,
  isEdit: boolean;
  data: RewardandPenaltyMaintenance
}

export interface RewardandPenaltyMaintenance_Form {
  param: RewardandPenaltyMaintenanceParam
  data: RewardandPenaltyMaintenance[]
}
export interface RewardandPenaltyMaintenance_Basic {
  param: RewardandPenaltyMaintenanceParam
  pagination: Pagination
  data: RewardandPenaltyMaintenance[]
  source: RewardandPenaltyMaintenance
}
