export interface VW_Production_T1_UPF_Delivery_RecordDTO {
  plan_End_STC: string | null;
  line_ID: string;
  mO_No: string;
  mO_Seq: string;
  model_Name: string;
  article: string;
  plan_Qty: number | null;
  stC_Output: number | null;
  stC_FGIN: number | null;
  stC_Forward : number | null;
  balance: number | null;
  building: string;
}
