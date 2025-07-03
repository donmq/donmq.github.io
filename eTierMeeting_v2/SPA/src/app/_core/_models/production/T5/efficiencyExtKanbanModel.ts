import { FactoryInfo } from "./efficiency";

export interface EfficiencyExtKanbanModel {
  title: string;
  showValues: number;
  chartUnit: string;
  isActive: boolean;
  digits: number;
  labels: string[];
  data: FactoryDataChartExt[];
}

export interface FactoryDataChartExt {
  name: string;
  value: (number | null)[];
  actualQty: (number | null)[];
}

export interface FactoryExtInfo {
  id: string;
  name: string;
}

export interface MonthOfYearExt {
  month: number;
  year: number;
}

export interface EffiencyExtKanbanParam {
  type: string;
  factorys: FactoryInfo[];
  months: MonthOfYearExt[];
  years: number[];
  weeks: WeekExt[];
  seasons: SeasonExt[];
}

export interface WeekExt {
  name: string;
  weekStart: string;
  weekFinish: string;
}

export interface SeasonExt {
  name: string;
  seasonStart: string;
  seasonFinish: string;
}
