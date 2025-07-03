
export interface EfficiencyKanban {
  title: string;
  showValues: number;
  chartUnit: string;
  isActive: boolean;
  digits: number;
  labels: string[];
  data: FactoryDataChart[];
}
export interface DataGrouped {
  group: string;
  data_By_Groups: EfficiencyKanban[];
}

export interface FactoryDataChart {
  name: string;
  value: number[];
  actualQty: number[];
}

export interface EfficiencyDto {
  factory: string;
  dept_ID: string;
  actual_Qty: number | null;
  target_Qty: number | null;
  impact_Qty: number | null;
  hour_Base: number | null;
  hour_Overtime: number | null;
  line_No: string;
  data_Date: string | null;
  month: number;
  year: number;
}

export interface FactoryInfo {
  id: string;
  name: string;
  status: boolean
}

export interface EfficiencyKanbanParam {
  factorys: FactoryInfo[];
  type: string;
  is_T5_External: boolean;
}
