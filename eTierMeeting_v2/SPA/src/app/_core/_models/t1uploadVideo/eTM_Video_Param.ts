export interface eTMVideoParam {
    videoKind: string;
    center: string;
    tier: string;
    section: string;
    unit: string;
    from_Date: string;
    to_Date: string;
}
export interface batchDeleteParam {
    videoKind: string;
    center: string;
    tier: string;
    section: string;
    units: string[];
    from_Date: string;
    to_Date: string;
}
