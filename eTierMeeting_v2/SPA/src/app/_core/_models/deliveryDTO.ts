export interface DeliveryDTO {
    comfirmed_Date: string;
    plan_Finish_Date: string;
    plan_Ship_Date: string;
    plan_Ship_Date_N: string | null;
    line_ID_ASY: string;
    line_ID_STC: string;
    mO_No: string;
    mO_Seq: string;
    model_Name: string;
    article: string;
    plan_Qty: number;
    uTN_Yield_Qty: number;
    iN_Qty: number | null;
    qty: number | null;
    nation: string;
    bG_Color: string;
    building: string;
    pDC_ID: string;
    bG_Color2: string;
    bG_Color3: string;
}
