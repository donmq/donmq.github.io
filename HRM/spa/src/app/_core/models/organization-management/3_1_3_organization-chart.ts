import { PanZoomModel } from 'ngx-panzoom';

export interface OrganizationChartParam {
  division: string;
  factory: string;
  department: string;
  level: string;
  lang: string;
}

export interface INodeDto {
  division: string;
  factory: string;
  department: string;
  root_Code: string;
  supervisor_Employee_ID: string;
  supervisor_Type: string;
  actual_Headcount: number;
  approved_Headcount: number;
  total_sub_actual_Headcount: number;
  total_sub_approved_Headcount: number;
  name: string;
  cssClass: string;
  image: string;
  title: string;
  childs: INodeDto[];
}

export interface OrganizationChart_MainMemory {
  param: OrganizationChartParam,
  position: PanZoomModel,
  data: INodeDto[]
}
