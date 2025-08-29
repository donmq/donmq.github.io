import { Pagination } from '@utilities/pagination-utility';

export interface PayslipDeliveryByEmailMaintenanceParam {
  factory: string;
  employee_ID: string;
}

export interface PayslipDeliveryByEmailMaintenanceDto {
  useR_GUID: string;
  factory: string;
  employee_ID: string;
  local_Full_Name: string;
  email: string;
  status: string;
  update_By: string;
  update_Time: string;
  isDeleteDisable: boolean;
}

export interface PayslipDeliveryByEmailMaintenanceSource {
  pagination: Pagination
  param: PayslipDeliveryByEmailMaintenanceParam,
  selectedData: PayslipDeliveryByEmailMaintenanceDto,
  data: PayslipDeliveryByEmailMaintenanceDto[]
}
