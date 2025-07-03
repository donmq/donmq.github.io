export interface VW_Production_T1_STF_Delivery_Recor {
    plan_Start_ASY: string | null;
    line_ID_ASY: string;
    line_ID_STF: string;
    mO_No: string;
    mO_Seq: string;
    style_Name: string;
    color_No: string;
    plan_Qty: number | null;
    output_Qty: number | null;
    output_Balance: number | null;
    transfer_Qty: number | null;
    transfer_Balance: number | null;
    building: string;
}