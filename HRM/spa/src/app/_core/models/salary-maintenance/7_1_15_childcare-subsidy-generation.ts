export interface ChildcareSubsidyGenerationParam {
  kind_Tab1: string;
  kind_Tab2: string;
  factory: string;
  permissionGroupMultiple: string[];
  yearMonth: string;
  resignedDate_Start: string;
  resignedDate_End: string;
  is_Delete: boolean;
  lang: string;
}


export interface ChildcareSubsidyGenerationSourceTab1 {
  param: ChildcareSubsidyGenerationParam;
  totalRows: number;
  year_Month: Date;
  dateFrom: Date;
  dateTo: Date;
}

export interface ChildcareSubsidyGenerationSourceTab2 {
  param: ChildcareSubsidyGenerationParam;
  totalRows: number;
  year_Month: Date;
}
