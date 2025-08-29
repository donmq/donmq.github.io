import { TemplateRef } from '@angular/core';
export interface ColumnConfig {
  title: string;
  field: string;
  type?: any; // type input
  valuesSelect?: any; // value select option for type select
  bsConfig?: any; // config for datetimepicker
  pipe?: string; // pipeline
  width?: number; // for td or th
  tdTemplate?: TemplateRef<any>
  class?: string;
  show?: boolean;
  editField?: boolean; //  flag disable field when edit
}

export interface TableConfig {
  showCheckbox?: boolean;
  isTranslate?: boolean;
  columns: ColumnConfig[]; // Column settings
}
