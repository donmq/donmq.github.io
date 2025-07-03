export interface eTM_Meeting_Log_Page {
  record_ID: string;
  tU_ID: string;
  deptId: string;
  center_Level: string;
  tier_Level: string;
  class_Level: string;
  page_Name: string;
  start_Time: Date | string;
  end_Time: Date | null;
  click_Link: boolean | null;
  isViewFirst: boolean;
}

export interface eTM_Meeting_Log_PageParamDTO {
  record_ID: string;
  clickLinkKaizenSystem: boolean;
}
