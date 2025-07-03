
export interface T2CTBQuality {
    data_Date: string;
    rfT_Target: number | null;
    bA_Target: number | null;
    rfT_Chart: RFTChart[];
    bA_Chart: BAChart[];
}

export interface RFTChart {
    line_Sname: string;
    rft: number | null;
}

export interface BAChart {
    line_Sname: string;
    ba: number | null;
}