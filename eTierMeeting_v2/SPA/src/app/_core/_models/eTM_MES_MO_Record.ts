export interface eTM_MES_MO_Record {
  data_Date: string;
  dept_ID: string;
  mO_No: string;
  mO_Seq: string;
  confirmed_Date: string | null;
  plan_Finish_Date: string | null;
  plan_Ship_Date: string | null;
  style_No: string;
  style_Name: string;
  color_No: string;
  plan_Qty: number;
  output_Qty: number;
  fGIN_Qty: number;
  nation: string;
  insert_By: string;
  insert_Time: string;
  update_By: string;
  update_Time: string | null;
  balance: string;
}
